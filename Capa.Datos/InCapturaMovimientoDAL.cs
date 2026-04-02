using Capa.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Capa.Datos
{
    public class InCapturaMovimientoDAL : CadenaDAL
    {
        readonly InExistenciaDAL existenciaDAL = new InExistenciaDAL();

        /// <summary>
        /// Inserta un movimiento y aplica cambios en InExistencias dentro de la misma transacción.
        /// Regla: resta en origen (si existe) y suma en destino (si existe).
        /// </summary>
        public (bool Success, string Message, int? IdMovimiento) InsertarMovimiento(InCapturaMovimientoCLS dto)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        // Insertar movimiento
                        using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.InCapMovimientos (
    INCAPMOV_FECHA,
    INCAPMOV_INTIPMOV_ID,
    INCAPMOV_INVSKU,
    INCAPMOV_SUCORI,
    INCAPMOV_BODORI,
    INCAPMOV_INLOCIDORI,
    INCAPMOV_SUCDES,
    INCAPMOV_BODDES,
    INCAPMOV_INLOCIDDES,
    INCAPMOV_Cantidad,
    INCAPMOV_Referencia
)
VALUES (@fecha, @tipomov, @sku, @sori, @bori, @lori, @sdes, @bdes, @ldes, @cantidad, @referencia);
SELECT SCOPE_IDENTITY();", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@fecha", dto.Fecha);
                            cmd.Parameters.AddWithValue("@tipomov", dto.TipoMovimientoId);
                            cmd.Parameters.AddWithValue("@sku", dto.InvSku ?? string.Empty);
                            cmd.Parameters.AddWithValue("@sori", (object?)dto.SucOrigen ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@bori", (object?)dto.BodOrigen ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@lori", (object?)dto.InlocIdOrigen ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@sdes", (object?)dto.SucDestino ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@bdes", (object?)dto.BodDestino ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ldes", (object?)dto.InlocIdDestino ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@cantidad", dto.Cantidad);
                            cmd.Parameters.AddWithValue("@referencia", (object?)dto.Referencia ?? DBNull.Value);

                            var idMov = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                            // Ajustes en existencias: origen resta, destino suma
                            if (dto.SucOrigen.HasValue && dto.BodOrigen.HasValue)
                            {
                                existenciaDAL.AjustarExistencia(cn, tx, dto.InvSku, dto.SucOrigen.Value, dto.BodOrigen.Value, dto.InlocIdOrigen, -dto.Cantidad);
                            }

                            if (dto.SucDestino.HasValue && dto.BodDestino.HasValue)
                            {
                                existenciaDAL.AjustarExistencia(cn, tx, dto.InvSku, dto.SucDestino.Value, dto.BodDestino.Value, dto.InlocIdDestino, dto.Cantidad);
                            }

                            tx.Commit();
                            return (true, "Movimiento insertado", idMov);
                        }
                    }
                    catch (Exception ex)
                    {
                        try { tx.Rollback(); } catch { }
                        return (false, ex.Message, null);
                    }
                }
            }
        }
    }
}