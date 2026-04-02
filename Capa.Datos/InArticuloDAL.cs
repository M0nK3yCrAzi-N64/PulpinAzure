using Capa.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Capa.Datos
{
    public class InArticuloDAL : CadenaDAL
    {
        public List<InArticuloCLS> listarArticulo()
        {
            var lista = new List<InArticuloCLS>();

            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_InArticulos_CRUD", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ACCION", "LISTAR");

                        using (SqlDataReader drd = cmd.ExecuteReader())
                        {
                            if (drd != null)
                            {
                                int posSku = drd.GetOrdinal("invsku");
                                int posClave = drd.GetOrdinal("invclave");
                                int posDatabar = drd.GetOrdinal("invdatabar");
                                int posNombre = drd.GetOrdinal("invnombre");
                                int posDescripcion = drd.GetOrdinal("invdescripcion");
                                int posUnidad = TryGetOrdinal(drd, "invunidad");
                                int posServicio = drd.GetOrdinal("invservicio");
                                int posEstado = drd.GetOrdinal("investado");
                                int posStock = drd.GetOrdinal("invstockglobal");
                                int posPrecio = drd.GetOrdinal("invprecio");
                                int posFoto = drd.GetOrdinal("invfoto");
                                int posFechaAlta = drd.GetOrdinal("invfechaalta");

                                while (drd.Read())
                                {
                                    lista.Add(new InArticuloCLS
                                    {
                                        InvSku = drd.IsDBNull(posSku) ? string.Empty : drd.GetString(posSku),
                                        InvClave = drd.IsDBNull(posClave) ? string.Empty : drd.GetString(posClave),
                                        InvDatabar = drd.IsDBNull(posDatabar) ? string.Empty : drd.GetString(posDatabar),
                                        InvNombre = drd.IsDBNull(posNombre) ? string.Empty : drd.GetString(posNombre),
                                        InvDescripcion = drd.IsDBNull(posDescripcion) ? string.Empty : drd.GetString(posDescripcion),
                                        InvUnidad = posUnidad >= 0 && !drd.IsDBNull(posUnidad) ? drd.GetString(posUnidad) : string.Empty,
                                        InvServicio = drd.IsDBNull(posServicio) ? false : drd.GetBoolean(posServicio),
                                        InvEstado = drd.IsDBNull(posEstado) ? false : drd.GetBoolean(posEstado),
                                        InvStockGlobal = drd.IsDBNull(posStock) ? 0m : drd.GetDecimal(posStock),
                                        InvPrecio = drd.IsDBNull(posPrecio) ? 0m : drd.GetDecimal(posPrecio),
                                        InvFoto = drd.IsDBNull(posFoto) ? string.Empty : drd.GetString(posFoto),
                                        InvFechaAlta = drd.IsDBNull(posFechaAlta) ? DateTime.MinValue : drd.GetDateTime(posFechaAlta)
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    lista.Clear();
                }
            }

            return lista;
        }

        public (bool Success, string Message) insertarArticulo(InArticuloCLS obj)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_InArticulos_CRUD", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ACCION", "AGREGAR");
                        cmd.Parameters.AddWithValue("@INV_SKU", obj.InvSku ?? string.Empty);
                        cmd.Parameters.AddWithValue("@INV_CLAVE", (object?)obj.InvClave ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_DATABAR", (object?)obj.InvDatabar ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_NOMBRE", (object?)obj.InvNombre ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_DESCRIPCION", (object?)obj.InvDescripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_UNIDAD", string.IsNullOrWhiteSpace(obj.InvUnidad) ? DBNull.Value : obj.InvUnidad.Trim().ToUpperInvariant());
                        cmd.Parameters.AddWithValue("@INV_SERVICIO", obj.InvServicio);
                        cmd.Parameters.AddWithValue("@INV_ESTADO", obj.InvEstado);
                        cmd.Parameters.AddWithValue("@INV_STOCKGLOBAL", obj.InvStockGlobal);
                        cmd.Parameters.AddWithValue("@INV_PRECIO", obj.InvPrecio);
                        cmd.Parameters.AddWithValue("@INV_FOTO", (object?)obj.InvFoto ?? DBNull.Value);

                        var filas = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        return filas > 0
                            ? (true, "Artículo insertado")
                            : (false, "No se insertó");
                    }
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

        public (bool Success, string Message) editarArticulo(InArticuloCLS obj)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_InArticulos_CRUD", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ACCION", "EDITAR");
                        cmd.Parameters.AddWithValue("@INV_SKU", obj.InvSku ?? string.Empty);
                        cmd.Parameters.AddWithValue("@INV_CLAVE", (object?)obj.InvClave ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_DATABAR", (object?)obj.InvDatabar ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_NOMBRE", (object?)obj.InvNombre ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_DESCRIPCION", (object?)obj.InvDescripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@INV_UNIDAD", string.IsNullOrWhiteSpace(obj.InvUnidad) ? DBNull.Value : obj.InvUnidad.Trim().ToUpperInvariant());
                        cmd.Parameters.AddWithValue("@INV_SERVICIO", obj.InvServicio);
                        cmd.Parameters.AddWithValue("@INV_ESTADO", obj.InvEstado);
                        cmd.Parameters.AddWithValue("@INV_STOCKGLOBAL", obj.InvStockGlobal);
                        cmd.Parameters.AddWithValue("@INV_PRECIO", obj.InvPrecio);
                        cmd.Parameters.AddWithValue("@INV_FOTO", (object?)obj.InvFoto ?? DBNull.Value);

                        var filas = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        return filas > 0
                            ? (true, "Artículo actualizado")
                            : (false, "No se actualizó (SKU no encontrado)");
                    }
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

        public (bool Success, string Message) eliminarArticulo(string invSku)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_InArticulos_CRUD", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ACCION", "ELIMINAR");
                        cmd.Parameters.AddWithValue("@INV_SKU", invSku ?? string.Empty);

                        var filas = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        return filas > 0
                            ? (true, "Artículo deshabilitado")
                            : (false, "No se eliminó (SKU no encontrado)");
                    }
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

        public List<InUnidadesCLS> obtenerUnidades()
        {
            var lista = new List<InUnidadesCLS>();

            using (SqlConnection cn = new SqlConnection(Cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("uspListarUnidades", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader drd = cmd.ExecuteReader())
                        {
                            if (drd != null)
                            {
                                int posCodigo = drd.GetOrdinal("codigo");
                                int posDescripcion = drd.GetOrdinal("descripcion");

                                while (drd.Read())
                                {
                                    var o = new InUnidadesCLS
                                    {
                                        UNI_CODIGO = drd.IsDBNull(posCodigo) ? string.Empty : drd.GetString(posCodigo),
                                        UNI_DESCRIPCION = drd.IsDBNull(posDescripcion) ? string.Empty : drd.GetString(posDescripcion)
                                    };

                                    lista.Add(o);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    lista.Clear();
                }
            }

            return lista;
        }

        private static int TryGetOrdinal(IDataRecord record, string columnName)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                if (string.Equals(record.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}