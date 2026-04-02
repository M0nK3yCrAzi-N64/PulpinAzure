using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa.Entity;

namespace Capa.Datos
{
    public class FaClientesDAL
    {
        private readonly string _cadenaConexion;

        public FaClientesDAL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<FaClientesCLS> Listar()
        {
            var lista = new List<FaClientesCLS>();

            const string sql = @"
SELECT CTE_SUCURSAL, CTE_CLIENTEID, CTE_RFC, CTE_NOMBRE,
       CTE_DIRECCION, CTE_NUMEXT, CTE_NUMINT, CTE_COLONIA, CTE_CPOSTAL, CTE_CIUDAD, CTE_ESTADO, CTE_PAIS,
       CTE_AGENTE, CTE_PIVA, CTE_DESCUENTO, CTE_LIMITECR, CTE_SALDO,
       CTE_CONTACTO, CTE_TELEFONO, CTE_CORREO,
       CTE_OBSERVACIONES, CTE_BLOQUEAR, CTE_FECHAALTA, CTE_ULTIMOMOVIMIENTO
FROM dbo.FaClientes
ORDER BY CTE_CLIENTEID;";
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(Mapear(dr));
                    }
                }
            }

            return lista;
        }

        public FaClientesCLS? Obtener(int idCliente)
        {
            const string sql = @"
SELECT CTE_SUCURSAL, CTE_CLIENTEID, CTE_RFC, CTE_NOMBRE,
       CTE_DIRECCION, CTE_NUMEXT, CTE_NUMINT, CTE_COLONIA, CTE_CPOSTAL, CTE_CIUDAD, CTE_ESTADO, CTE_PAIS,
       CTE_AGENTE, CTE_PIVA, CTE_DESCUENTO, CTE_LIMITECR, CTE_SALDO,
       CTE_CONTACTO, CTE_TELEFONO, CTE_CORREO,
       CTE_OBSERVACIONES, CTE_BLOQUEAR, CTE_FECHAALTA, CTE_ULTIMOMOVIMIENTO
FROM dbo.FaClientes
WHERE CTE_CLIENTEID = @CTE_CLIENTEID;";
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@CTE_CLIENTEID", idCliente);
                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        return null;
                    }

                    return Mapear(dr);
                }
            }
        }

        public int Insertar(FaClientesCLS cliente)
        {
            using (var cn = new SqlConnection(_cadenaConexion))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        int nuevoId = ObtenerSiguienteIdDisponible(cn, tx);

                        const string sql = @"
INSERT INTO dbo.FaClientes
(
    CTE_SUCURSAL, CTE_CLIENTEID, CTE_RFC, CTE_NOMBRE,
    CTE_DIRECCION, CTE_NUMEXT, CTE_NUMINT, CTE_COLONIA, CTE_CPOSTAL, CTE_CIUDAD, CTE_ESTADO, CTE_PAIS,
    CTE_AGENTE, CTE_PIVA, CTE_DESCUENTO, CTE_LIMITECR, CTE_SALDO,
    CTE_CONTACTO, CTE_TELEFONO, CTE_CORREO,
    CTE_OBSERVACIONES, CTE_BLOQUEAR, CTE_FECHAALTA, CTE_ULTIMOMOVIMIENTO
)
VALUES
(
    @CTE_SUCURSAL, @CTE_CLIENTEID, @CTE_RFC, @CTE_NOMBRE,
    @CTE_DIRECCION, @CTE_NUMEXT, @CTE_NUMINT, @CTE_COLONIA, @CTE_CPOSTAL, @CTE_CIUDAD, @CTE_ESTADO, @CTE_PAIS,
    @CTE_AGENTE, @CTE_PIVA, @CTE_DESCUENTO, @CTE_LIMITECR, @CTE_SALDO,
    @CTE_CONTACTO, @CTE_TELEFONO, @CTE_CORREO,
    @CTE_OBSERVACIONES, @CTE_BLOQUEAR, GETDATE(), GETDATE()
);";

                        using (var cmd = new SqlCommand(sql, cn, tx))
                        {
                            AgregarParametros(cmd, cliente, nuevoId);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return nuevoId;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public int Actualizar(FaClientesCLS cliente)
        {
            const string sql = @"
UPDATE dbo.FaClientes
SET CTE_SUCURSAL = @CTE_SUCURSAL,
    CTE_RFC = @CTE_RFC,
    CTE_NOMBRE = @CTE_NOMBRE,
    CTE_DIRECCION = @CTE_DIRECCION,
    CTE_NUMEXT = @CTE_NUMEXT,
    CTE_NUMINT = @CTE_NUMINT,
    CTE_COLONIA = @CTE_COLONIA,
    CTE_CPOSTAL = @CTE_CPOSTAL,
    CTE_CIUDAD = @CTE_CIUDAD,
    CTE_ESTADO = @CTE_ESTADO,
    CTE_PAIS = @CTE_PAIS,
    CTE_AGENTE = @CTE_AGENTE,
    CTE_PIVA = @CTE_PIVA,
    CTE_DESCUENTO = @CTE_DESCUENTO,
    CTE_LIMITECR = @CTE_LIMITECR,
    CTE_SALDO = @CTE_SALDO,
    CTE_CONTACTO = @CTE_CONTACTO,
    CTE_TELEFONO = @CTE_TELEFONO,
    CTE_CORREO = @CTE_CORREO,
    CTE_OBSERVACIONES = @CTE_OBSERVACIONES,
    CTE_BLOQUEAR = @CTE_BLOQUEAR,
    CTE_ULTIMOMOVIMIENTO = GETDATE()
WHERE CTE_CLIENTEID = @CTE_CLIENTEID;";
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                AgregarParametros(cmd, cliente, cliente.CTE_CLIENTEID);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(int idCliente)
        {
            const string sql = @"DELETE FROM dbo.FaClientes WHERE CTE_CLIENTEID = @CTE_CLIENTEID;";
            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@CTE_CLIENTEID", idCliente);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private static FaClientesCLS Mapear(SqlDataReader dr)
        {
            return new FaClientesCLS
            {
                CTE_SUCURSAL = dr.IsDBNull(0) ? null : dr.GetInt16(0),
                CTE_CLIENTEID = dr.GetInt32(1),
                CTE_RFC = dr.IsDBNull(2) ? string.Empty : dr.GetString(2),
                CTE_NOMBRE = dr.IsDBNull(3) ? string.Empty : dr.GetString(3),

                CTE_DIRECCION = dr.IsDBNull(4) ? string.Empty : dr.GetString(4),
                CTE_NUMEXT = dr.IsDBNull(5) ? string.Empty : dr.GetString(5),
                CTE_NUMINT = dr.IsDBNull(6) ? string.Empty : dr.GetString(6),
                CTE_COLONIA = dr.IsDBNull(7) ? null : dr.GetInt32(7),
                CTE_CPOSTAL = dr.IsDBNull(8) ? string.Empty : dr.GetString(8),
                CTE_CIUDAD = dr.IsDBNull(9) ? string.Empty : dr.GetString(9),
                CTE_ESTADO = dr.IsDBNull(10) ? string.Empty : dr.GetString(10),
                CTE_PAIS = dr.IsDBNull(11) ? string.Empty : dr.GetString(11),

                CTE_AGENTE = dr.IsDBNull(12) ? null : dr.GetInt32(12),
                CTE_PIVA = dr.IsDBNull(13) ? null : dr.GetDouble(13),
                CTE_DESCUENTO = dr.IsDBNull(14) ? null : dr.GetDouble(14),
                CTE_LIMITECR = dr.IsDBNull(15) ? null : dr.GetDouble(15),
                CTE_SALDO = dr.IsDBNull(16) ? null : dr.GetDouble(16),

                CTE_CONTACTO = dr.IsDBNull(17) ? string.Empty : dr.GetString(17),
                CTE_TELEFONO = dr.IsDBNull(18) ? string.Empty : dr.GetString(18),
                CTE_CORREO = dr.IsDBNull(19) ? string.Empty : dr.GetString(19),

                CTE_OBSERVACIONES = dr.IsDBNull(20) ? string.Empty : dr.GetString(20),
                CTE_BLOQUEAR = !dr.IsDBNull(21) && dr.GetBoolean(21),
                CTE_FECHAALTA = dr.IsDBNull(22) ? null : dr.GetDateTime(22),
                CTE_ULTIMOMOVIMIENTO = dr.IsDBNull(23) ? null : dr.GetDateTime(23)
            };
        }

        private static void AgregarParametros(SqlCommand cmd, FaClientesCLS cliente, int idCliente)
        {
            cmd.Parameters.AddWithValue("@CTE_SUCURSAL", (object?)cliente.CTE_SUCURSAL ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_CLIENTEID", idCliente);
            cmd.Parameters.AddWithValue("@CTE_RFC", (cliente.CTE_RFC ?? string.Empty).Trim());
            cmd.Parameters.AddWithValue("@CTE_NOMBRE", (cliente.CTE_NOMBRE ?? string.Empty).Trim());

            cmd.Parameters.AddWithValue("@CTE_DIRECCION", (object?)NullSiVacio(cliente.CTE_DIRECCION) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_NUMEXT", (object?)NullSiVacio(cliente.CTE_NUMEXT) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_NUMINT", (object?)NullSiVacio(cliente.CTE_NUMINT) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_COLONIA", (object?)cliente.CTE_COLONIA ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_CPOSTAL", (object?)NullSiVacio(cliente.CTE_CPOSTAL) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_CIUDAD", (object?)NullSiVacio(cliente.CTE_CIUDAD) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_ESTADO", (object?)NullSiVacio(cliente.CTE_ESTADO) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_PAIS", (object?)NullSiVacio(cliente.CTE_PAIS) ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@CTE_AGENTE", (object?)cliente.CTE_AGENTE ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_PIVA", (object?)cliente.CTE_PIVA ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_DESCUENTO", (object?)cliente.CTE_DESCUENTO ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_LIMITECR", (object?)cliente.CTE_LIMITECR ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_SALDO", (object?)cliente.CTE_SALDO ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@CTE_CONTACTO", (object?)NullSiVacio(cliente.CTE_CONTACTO) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_TELEFONO", (object?)NullSiVacio(cliente.CTE_TELEFONO) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_CORREO", (object?)NullSiVacio(cliente.CTE_CORREO) ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@CTE_OBSERVACIONES", (object?)NullSiVacio(cliente.CTE_OBSERVACIONES) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_BLOQUEAR", cliente.CTE_BLOQUEAR);
        }

        private static string? NullSiVacio(string? value)
        {
            var v = (value ?? string.Empty).Trim();
            return string.IsNullOrWhiteSpace(v) ? null : v;
        }

        private static int ObtenerSiguienteIdDisponible(SqlConnection cn, SqlTransaction tx)
        {
            const string sql = @"SELECT CTE_CLIENTEID FROM dbo.FaClientes WITH (UPDLOCK, HOLDLOCK) ORDER BY CTE_CLIENTEID;";
            using (var cmd = new SqlCommand(sql, cn, tx))
            using (var dr = cmd.ExecuteReader())
            {
                int expected = 1;
                while (dr.Read())
                {
                    int idActual = dr.GetInt32(0);
                    if (idActual == expected)
                    {
                        expected++;
                        continue;
                    }

                    if (idActual > expected)
                    {
                        return expected;
                    }
                }

                return expected;
            }
        }
    }
}
