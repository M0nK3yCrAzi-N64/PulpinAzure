using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa.Datos;
using Capa.Entity;

namespace Capa.Negocio
{
    public class InCapturaMovimientoBLL
    {
        readonly InCapturaMovimientoDAL dal = new InCapturaMovimientoDAL();
        private string? cadenaConexion;

        public InCapturaMovimientoBLL(string? cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
        }

        /// <summary>
        /// Inserta un movimiento y aplica cambios en existencias (transaccional).
        /// Devuelve (Success, Message, IdMovimiento).
        /// </summary>
        public (bool Success, string Message, int? IdMovimiento) InsertarMovimiento(InCapturaMovimientoCLS dto)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.InvSku))
                return (false, "SKU vacío", null);
            if (dto.Cantidad <= 0)
                return (false, "Cantidad debe ser mayor a cero", null);

            return dal.InsertarMovimiento(dto);
        }
    }
}
