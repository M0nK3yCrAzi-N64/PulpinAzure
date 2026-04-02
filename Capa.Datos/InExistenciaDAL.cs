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
    public class InExistenciaDAL : CadenaDAL
    {
        /// <summary>
        /// Ajusta (incrementa o decrementa) la existencia. Usa la conexión/transaction provista para participar en la misma transacción.
        /// </summary>
        public void AjustarExistencia(SqlConnection cn, SqlTransaction? tx, string sku, short sucursal, int bodega, int? locId, decimal delta)
        {
            using (SqlCommand cmd = new SqlCommand(@"
IF EXISTS (
    SELECT 1 FROM dbo.InExistencias 
    WHERE INEXI_SKU=@sku AND INEXI_BODSUC=@s AND INEXI_BODEGA=@b AND ((@locid IS NULL AND INEXI_LOCID IS NULL) OR INEXI_LOCID=@locid)
)
    UPDATE dbo.InExistencias
    SET INEXI_CANTIDAD = INEXI_CANTIDAD + @delta
    WHERE INEXI_SKU=@sku AND INEXI_BODSUC=@s AND INEXI_BODEGA=@b AND ((@locid IS NULL AND INEXI_LOCID IS NULL) OR INEXI_LOCID=@locid)
ELSE
    INSERT INTO dbo.InExistencias (INEXI_SKU, INEXI_BODSUC, INEXI_BODEGA, INEXI_LOCID, INEXI_CANTIDAD)
    VALUES(@sku, @s, @b, @locid, @delta);
", cn, tx))
            {
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@s", sucursal);
                cmd.Parameters.AddWithValue("@b", bodega);
                cmd.Parameters.AddWithValue("@locid", (object?)locId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@delta", delta);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Overload sin transacción: ajusta existencias en operación independiente.
        /// </summary>
        public void AjustarExistencia(string sku, short sucursal, int bodega, int? locId, decimal delta)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                AjustarExistencia(cn, null, sku, sucursal, bodega, locId, delta);
            }
        }

        public decimal ObtenerCantidad(string sku, short sucursal, int bodega, int? locId)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT INEXI_CANTIDAD FROM dbo.InExistencias WHERE INEXI_SKU=@sku AND INEXI_BODSUC=@s AND INEXI_BODEGA=@b AND ((@locid IS NULL AND INEXI_LOCID IS NULL) OR INEXI_LOCID=@locid)", cn))
                {
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@s", sucursal);
                    cmd.Parameters.AddWithValue("@b", bodega);
                    cmd.Parameters.AddWithValue("@locid", (object?)locId ?? DBNull.Value);
                    var res = cmd.ExecuteScalar();
                    return res == null || res == DBNull.Value ? 0m : Convert.ToDecimal(res);
                }
            }
        }
    }
}
