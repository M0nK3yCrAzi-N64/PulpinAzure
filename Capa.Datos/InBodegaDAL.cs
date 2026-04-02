// Capa.Datos/InBodegaDAL.cs
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa.Entity;

namespace Capa.Datos
{
    public class InBodegaDAL
    {
        private readonly string _cadenaConexion;

        public InBodegaDAL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<InBodegaCLS> Listar()
        {
            var lista = new List<InBodegaCLS>();
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand("SELECT BOD_SUCURSAL, BOD_BODEGA, BOD_NOMBRE, BOD_TIPO FROM InBodegas", cn))
            {
                cmd.CommandType = CommandType.Text;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new InBodegaCLS
                        {
                            BOD_SUCURSAL = dr.GetInt16(0),
                            BOD_BODEGA = dr.GetInt32(1),
                            BOD_NOMBRE = dr.GetString(2),
                            BOD_TIPO = dr.GetString(3)[0]
                        });
                    }
                }
            }
            return lista;
        }

        public int Insertar(InBodegaCLS bodega)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                "INSERT INTO InBodegas (BOD_SUCURSAL, BOD_BODEGA, BOD_NOMBRE, BOD_TIPO) VALUES (@BOD_SUCURSAL, @BOD_BODEGA, @BOD_NOMBRE, @BOD_TIPO)", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BOD_SUCURSAL", bodega.BOD_SUCURSAL);
                cmd.Parameters.AddWithValue("@BOD_BODEGA", bodega.BOD_BODEGA);
                cmd.Parameters.AddWithValue("@BOD_NOMBRE", bodega.BOD_NOMBRE);
                cmd.Parameters.AddWithValue("@BOD_TIPO", bodega.BOD_TIPO);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public bool Existe(short sucursal, int bodega)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM InBodegas WHERE BOD_SUCURSAL = @BOD_SUCURSAL AND BOD_BODEGA = @BOD_BODEGA", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BOD_SUCURSAL", sucursal);
                cmd.Parameters.AddWithValue("@BOD_BODEGA", bodega);
                cn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public int Actualizar(InBodegaCLS bodega)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                "UPDATE InBodegas SET BOD_NOMBRE = @BOD_NOMBRE, BOD_TIPO = @BOD_TIPO WHERE BOD_SUCURSAL = @BOD_SUCURSAL AND BOD_BODEGA = @BOD_BODEGA", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BOD_SUCURSAL", bodega.BOD_SUCURSAL);
                cmd.Parameters.AddWithValue("@BOD_BODEGA", bodega.BOD_BODEGA);
                cmd.Parameters.AddWithValue("@BOD_NOMBRE", bodega.BOD_NOMBRE);
                cmd.Parameters.AddWithValue("@BOD_TIPO", bodega.BOD_TIPO);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(short sucursal, int bodega)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(
                "DELETE FROM InBodegas WHERE BOD_SUCURSAL = @BOD_SUCURSAL AND BOD_BODEGA = @BOD_BODEGA", cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BOD_SUCURSAL", sucursal);
                cmd.Parameters.AddWithValue("@BOD_BODEGA", bodega);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}