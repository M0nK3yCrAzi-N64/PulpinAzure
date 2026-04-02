using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa.Datos;
using Capa.Entity;

namespace Capa.Negocio
{
    public class InExistenciaBLL
    {
        readonly InExistenciaDAL dal = new InExistenciaDAL();

        /// <summary>
        /// Obtiene la cantidad actual para una combinación SKU / sucursal / bodega / locación.
        /// </summary>
        public decimal ObtenerCantidad(string sku, short sucursal, int bodega, int? locId) =>
            dal.ObtenerCantidad(sku, sucursal, bodega, locId);

        /// <summary>
        /// Ajusta existencias (uso administrativo). Recuerda la regla de oro: los cambios deberían venir de InCapMovimientos.
        /// </summary>
        public void AjustarExistencia(string sku, short sucursal, int bodega, int? locId, decimal delta) =>
            dal.AjustarExistencia(sku, sucursal, bodega, locId, delta);
    }
}
