using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Capa.Entity
{
    public class InCapturaMovimientoCLS
    {
        public int Id { get; set; }                        // INCAPMOV_ID (cuando aplique)
        public DateTime Fecha { get; set; } = DateTime.Now; // INCAPMOV_FECHA
        public int TipoMovimientoId { get; set; }          // INCAPMOV_INTIPMOV_ID
        public string InvSku { get; set; } = string.Empty; // INCAPMOV_INVSKU

        // Origen (NULL si entrada)
        public short? SucOrigen { get; set; }               // INCAPMOV_SUCORI
        public int? BodOrigen { get; set; }                 // INCAPMOV_BODORI
        public int? InlocIdOrigen { get; set; }             // INCAPMOV_INLOCIDORI

        // Destino (NULL si salida)
        public short? SucDestino { get; set; }              // INCAPMOV_SUCDES
        public int? BodDestino { get; set; }                // INCAPMOV_BODDES
        public int? InlocIdDestino { get; set; }            // INCAPMOV_INLOCIDDES

        public decimal Cantidad { get; set; }               // INCAPMOV_Cantidad
        public string? Referencia { get; set; }             // INCAPMOV_Referencia
    }
}