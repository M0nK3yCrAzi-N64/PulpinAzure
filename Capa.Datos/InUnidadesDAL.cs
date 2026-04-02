using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa.Entity;

namespace Capa.Datos
{
    public class InUnidadesDAL
    {
        private readonly string _cadenaConexion;

        public InUnidadesDAL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<InUnidadesCLS> Listar()
        {
            var lista = new List<InUnidadesCLS>();

            const string sql = @"
SELECT RTRIM(UNI_CODIGO) AS UNI_CODIGO,
       RTRIM(UNI_DESCRIPCION) AS UNI_DESCRIPCION,
       ISNULL(UNI_CVESAT, '') AS UNI_CVESAT,
       ISNULL(UNI_CVESATADUANA, '') AS UNI_CVESATADUANA
FROM dbo.InUnidades
ORDER BY UNI_CODIGO;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new InUnidadesCLS
                        {
                            UNI_CODIGO = dr.IsDBNull(0) ? string.Empty : dr.GetString(0).Trim(),
                            UNI_DESCRIPCION = dr.IsDBNull(1) ? string.Empty : dr.GetString(1).Trim(),
                            UNI_CVESAT = dr.IsDBNull(2) ? string.Empty : dr.GetString(2).Trim(),
                            UNI_CVESATADUANA = dr.IsDBNull(3) ? string.Empty : dr.GetString(3).Trim()
                        });
                    }
                }
            }

            return lista;
        }

        public InUnidadesCLS? Obtener(string codigo)
        {
            const string sql = @"
SELECT RTRIM(UNI_CODIGO) AS UNI_CODIGO,
       RTRIM(UNI_DESCRIPCION) AS UNI_DESCRIPCION,
       ISNULL(UNI_CVESAT, '') AS UNI_CVESAT,
       ISNULL(UNI_CVESATADUANA, '') AS UNI_CVESATADUANA
FROM dbo.InUnidades
WHERE UNI_CODIGO = @UNI_CODIGO;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UNI_CODIGO", (codigo ?? string.Empty).Trim());

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        return null;
                    }

                    return new InUnidadesCLS
                    {
                        UNI_CODIGO = dr.IsDBNull(0) ? string.Empty : dr.GetString(0).Trim(),
                        UNI_DESCRIPCION = dr.IsDBNull(1) ? string.Empty : dr.GetString(1).Trim(),
                        UNI_CVESAT = dr.IsDBNull(2) ? string.Empty : dr.GetString(2).Trim(),
                        UNI_CVESATADUANA = dr.IsDBNull(3) ? string.Empty : dr.GetString(3).Trim()
                    };
                }
            }
        }

        public int Insertar(InUnidadesCLS unidad)
        {
            const string sql = @"
INSERT INTO dbo.InUnidades
(
    UNI_CODIGO,
    UNI_DESCRIPCION,
    UNI_CVESAT,
    UNI_CVESATADUANA
)
VALUES
(
    @UNI_CODIGO,
    @UNI_DESCRIPCION,
    @UNI_CVESAT,
    @UNI_CVESATADUANA
);";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                AgregarParametros(cmd, unidad);

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Actualizar(InUnidadesCLS unidad)
        {
            const string sql = @"
UPDATE dbo.InUnidades
SET UNI_DESCRIPCION = @UNI_DESCRIPCION,
    UNI_CVESAT = @UNI_CVESAT,
    UNI_CVESATADUANA = @UNI_CVESATADUANA
WHERE UNI_CODIGO = @UNI_CODIGO;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                AgregarParametros(cmd, unidad);

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(string codigo)
        {
            const string sql = @"DELETE FROM dbo.InUnidades WHERE UNI_CODIGO = @UNI_CODIGO;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UNI_CODIGO", (codigo ?? string.Empty).Trim());

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public bool Existe(string codigo)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.InUnidades WHERE UNI_CODIGO = @UNI_CODIGO;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UNI_CODIGO", (codigo ?? string.Empty).Trim());

                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public List<SatUnidadCLS> ListarClavesSat()
        {
            var lista = new List<SatUnidadCLS>();

            const string sql = @"
SELECT RTRIM(UND_CLAVE) AS UND_CLAVE,
       RTRIM(UND_NOMBRE) AS UND_NOMBRE
FROM dbo.satUnidades
ORDER BY UND_CLAVE;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new SatUnidadCLS
                        {
                            UND_CLAVE = dr.IsDBNull(0) ? string.Empty : dr.GetString(0).Trim(),
                            UND_NOMBRE = dr.IsDBNull(1) ? string.Empty : dr.GetString(1).Trim()
                        });
                    }
                }
            }

            return lista;
        }

        public bool ExisteClaveSat(string clave)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.satUnidades WHERE UND_CLAVE = @UND_CLAVE;";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@UND_CLAVE", SqlDbType.VarChar, 3).Value = (clave ?? string.Empty).Trim().ToUpperInvariant();

                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int InsertarClaveSat(SatUnidadCLS sat)
        {
            const string sql = @"
INSERT INTO dbo.satUnidades (UND_CLAVE, UND_NOMBRE)
VALUES (@UND_CLAVE, @UND_NOMBRE);";

            using (var cn = new SqlConnection(_cadenaConexion))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@UND_CLAVE", SqlDbType.VarChar, 3).Value = (sat.UND_CLAVE ?? string.Empty).Trim().ToUpperInvariant();
                cmd.Parameters.Add("@UND_NOMBRE", SqlDbType.VarChar, 250).Value = (sat.UND_NOMBRE ?? string.Empty).Trim();

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private static void AgregarParametros(SqlCommand cmd, InUnidadesCLS unidad)
        {
            cmd.Parameters.AddWithValue("@UNI_CODIGO", (unidad.UNI_CODIGO ?? string.Empty).Trim());
            cmd.Parameters.AddWithValue("@UNI_DESCRIPCION", (object?)NullSiVacio(unidad.UNI_DESCRIPCION) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UNI_CVESAT", (object?)NullSiVacio(unidad.UNI_CVESAT) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UNI_CVESATADUANA", (object?)NullSiVacio(unidad.UNI_CVESATADUANA) ?? DBNull.Value);
        }

        private static string? NullSiVacio(string? value)
        {
            var texto = (value ?? string.Empty).Trim();
            return string.IsNullOrWhiteSpace(texto) ? null : texto;
        }
    }
}
