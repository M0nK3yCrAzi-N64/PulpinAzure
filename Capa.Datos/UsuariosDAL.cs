using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa.Entity;

namespace Capa.Datos
{
    public class UsuariosDAL
    {
        private readonly string _cadenaConexion;

        public UsuariosDAL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<UsuariosCLS> Listar()
        {
            var lista = new List<UsuariosCLS>();

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                "SELECT IdUsuario, Usuario, Nombre, PasswordHash, Activo FROM dbo.Usuario ORDER BY IdUsuario", cn))
            {
                cmd.CommandType = CommandType.Text;
                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new UsuariosCLS
                        {
                            IdUsuario = dr.GetInt32(0),
                            Usuario = dr.GetString(1),
                            Nombre = dr.GetString(2),
                            PasswordHash = dr.GetString(3),
                            Activo = dr.GetBoolean(4)
                        });
                    }
                }
            }

            return lista;
        }

        public UsuariosCLS? Obtener(int idUsuario)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                "SELECT IdUsuario, Usuario, Nombre, PasswordHash, Activo FROM dbo.Usuario WHERE IdUsuario = @IdUsuario", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return null;

                    return new UsuariosCLS
                    {
                        IdUsuario = dr.GetInt32(0),
                        Usuario = dr.GetString(1),
                        Nombre = dr.GetString(2),
                        PasswordHash = dr.GetString(3),
                        Activo = dr.GetBoolean(4)
                    };
                }
            }
        }

        public bool ExisteUsuario(string usuario, int? excluirIdUsuario = null)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                @"SELECT COUNT(1)
                  FROM dbo.Usuario
                  WHERE Usuario = @Usuario
                    AND (@ExcluirIdUsuario IS NULL OR IdUsuario <> @ExcluirIdUsuario)", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@ExcluirIdUsuario", (object?)excluirIdUsuario ?? DBNull.Value);
                cn.Open();

                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public int Insertar(UsuariosCLS usuario)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                @"INSERT INTO dbo.Usuario (Usuario, Nombre, PasswordHash, Activo)
                  VALUES (@Usuario, @Nombre, @PasswordHash, @Activo);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@Activo", usuario.Activo);

                cn.Open();
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : (int)result;
            }
        }

        public int Actualizar(UsuariosCLS usuario)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                @"UPDATE dbo.Usuario
                  SET Usuario = @Usuario,
                      Nombre = @Nombre,
                      PasswordHash = @PasswordHash,
                      Activo = @Activo
                  WHERE IdUsuario = @IdUsuario", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@Activo", usuario.Activo);

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(int idUsuario)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            {
                cn.Open();

                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        // 1) Eliminar relaciones del usuario con roles
                        using (var cmdDetalle = new SqlCommand(
                            "DELETE FROM dbo.UsuarioRol WHERE IdUsuario = @IdUsuario", cn, tx))
                        {
                            cmdDetalle.CommandType = CommandType.Text;
                            cmdDetalle.Parameters.AddWithValue("@IdUsuario", idUsuario);
                            cmdDetalle.ExecuteNonQuery();
                        }

                        // 2) Eliminar usuario
                        using (var cmdUsuario = new SqlCommand(
                            "DELETE FROM dbo.Usuario WHERE IdUsuario = @IdUsuario", cn, tx))
                        {
                            cmdUsuario.CommandType = CommandType.Text;
                            cmdUsuario.Parameters.AddWithValue("@IdUsuario", idUsuario);
                            var filas = cmdUsuario.ExecuteNonQuery();

                            tx.Commit();
                            return filas;
                        }
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
