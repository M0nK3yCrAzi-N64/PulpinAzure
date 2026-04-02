using Capa.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Capa.Datos
{
    public class InLocacionDAL : CadenaDAL
    {
        public List<InLocacionCLS> ListarPorBodega(short sucursal, int bodega)
        {
            var lista = new List<InLocacionCLS>();
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT INLOC_ID, INLOC_SUCURSAL, INLOC_BODEGA, INLOC_CODIGO, INLOC_DESCRIPCION FROM dbo.InLocaciones WHERE INLOC_SUCURSAL=@s AND INLOC_BODEGA=@b", cn))
                {
                    cmd.Parameters.AddWithValue("@s", sucursal);
                    cmd.Parameters.AddWithValue("@b", bodega);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new InLocacionCLS
                            {
                                Id = dr.GetInt32(0),
                                Sucursal = dr.GetInt16(1),
                                Bodega = dr.GetInt32(2),
                                Codigo = dr.IsDBNull(3) ? string.Empty : dr.GetString(3),
                                Descripcion = dr.IsDBNull(4) ? null : dr.GetString(4)
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public int Insertar(InLocacionCLS dto)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.InLocaciones (INLOC_SUCURSAL, INLOC_BODEGA, INLOC_CODIGO, INLOC_DESCRIPCION) VALUES (@s,@b,@c,@d); SELECT SCOPE_IDENTITY();", cn))
                {
                    cmd.Parameters.AddWithValue("@s", dto.Sucursal);
                    cmd.Parameters.AddWithValue("@b", dto.Bodega);
                    cmd.Parameters.AddWithValue("@c", dto.Codigo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@d", (object?)dto.Descripcion ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                }
            }
        }

        public bool Editar(InLocacionCLS dto)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE dbo.InLocaciones SET INLOC_CODIGO=@c, INLOC_DESCRIPCION=@d WHERE INLOC_ID=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@c", dto.Codigo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@d", (object?)dto.Descripcion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", dto.Id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM dbo.InLocaciones WHERE INLOC_ID=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
