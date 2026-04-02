using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa.Entity;

namespace Capa.Datos
{
    public class InTipoMovimientoDAL
    {
        private readonly string _cadenaConexion;

        public InTipoMovimientoDAL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<InTipoMovimientoCLS> Listar()
        {
            var lista = new List<InTipoMovimientoCLS>();
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                       "SELECT INTIPMOV_ID, INTIPMOV_DESCRIPCION, INTIPMOV_TIPO, INTIPMOV_FACTOR FROM dbo.InTipMov",
                       cn))
            {
                cmd.CommandType = CommandType.Text;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new InTipoMovimientoCLS
                        {
                            INTIPMOV_ID = dr.GetInt16(0),
                            INTIPMOV_DESCRIPCION = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                            INTIPMOV_TIPO = dr.IsDBNull(2) ? 'E' : dr.GetString(2)[0],
                            INTIPMOV_FACTOR = dr.IsDBNull(3) ? (short)1 : dr.GetInt16(3)
                        });
                    }
                }
            }
            return lista;
        }

        public InTipoMovimientoCLS? Obtener(short id)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                       "SELECT INTIPMOV_ID, INTIPMOV_DESCRIPCION, INTIPMOV_TIPO, INTIPMOV_FACTOR FROM dbo.InTipMov WHERE INTIPMOV_ID = @ID",
                       cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return null;

                    return new InTipoMovimientoCLS
                    {
                        INTIPMOV_ID = dr.GetInt16(0),
                        INTIPMOV_DESCRIPCION = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                        INTIPMOV_TIPO = dr.IsDBNull(2) ? 'E' : dr.GetString(2)[0],
                        INTIPMOV_FACTOR = dr.IsDBNull(3) ? (short)1 : dr.GetInt16(3)
                    };
                }
            }
        }

        public int Insertar(InTipoMovimientoCLS tipoMovimiento)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            {
                cn.Open();

                using (var tx = cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        int siguienteId;
                        using (var cmdId = new SqlCommand(
                                   @"SELECT ISNULL(MAX(INTIPMOV_ID), 0) + 1
                                     FROM dbo.InTipMov WITH (UPDLOCK, HOLDLOCK);",
                                   cn, tx))
                        {
                            siguienteId = Convert.ToInt32(cmdId.ExecuteScalar());
                        }

                        if (siguienteId > short.MaxValue)
                            throw new Exception("Se alcanzó el límite de INTIPMOV_ID (smallint).");

                        using (var cmd = new SqlCommand(
                                   @"INSERT INTO dbo.InTipMov (INTIPMOV_ID, INTIPMOV_DESCRIPCION, INTIPMOV_TIPO, INTIPMOV_FACTOR)
                                     VALUES (@ID, @DESC, @TIPO, @FACTOR);",
                                   cn, tx))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@ID", (short)siguienteId);
                            cmd.Parameters.AddWithValue("@DESC", tipoMovimiento.INTIPMOV_DESCRIPCION ?? string.Empty);
                            cmd.Parameters.AddWithValue("@TIPO", tipoMovimiento.INTIPMOV_TIPO.ToString());
                            cmd.Parameters.AddWithValue("@FACTOR", tipoMovimiento.INTIPMOV_FACTOR);

                            var filas = cmd.ExecuteNonQuery();
                            if (filas <= 0)
                            {
                                tx.Rollback();
                                return 0;
                            }
                        }

                        tx.Commit();
                        return siguienteId;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public int Actualizar(InTipoMovimientoCLS tipoMovimiento)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                       "UPDATE dbo.InTipMov SET INTIPMOV_DESCRIPCION = @DESC, INTIPMOV_TIPO = @TIPO, INTIPMOV_FACTOR = @FACTOR WHERE INTIPMOV_ID = @ID",
                       cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", tipoMovimiento.INTIPMOV_ID);
                cmd.Parameters.AddWithValue("@DESC", tipoMovimiento.INTIPMOV_DESCRIPCION ?? string.Empty);
                cmd.Parameters.AddWithValue("@TIPO", tipoMovimiento.INTIPMOV_TIPO.ToString());
                cmd.Parameters.AddWithValue("@FACTOR", tipoMovimiento.INTIPMOV_FACTOR);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(short id)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                       "DELETE FROM dbo.InTipMov WHERE INTIPMOV_ID = @ID",
                       cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public bool Existe(short id)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                       "SELECT COUNT(1) FROM dbo.InTipMov WHERE INTIPMOV_ID = @ID",
                       cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                cn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}
