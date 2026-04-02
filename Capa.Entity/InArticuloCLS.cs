using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Entity
{
    public class InArticuloCLS
    {

        [Key]
        [StringLength(15)]
        public string InvSku { get; set; }

        [Required, StringLength(15)]
        public string? InvClave { get; set; }

        [StringLength(20)]
        public string? InvDatabar { get; set; }

        [Required, StringLength(60)]
        public string InvNombre { get; set; }

        [StringLength(511)]
        public string? InvDescripcion { get; set; }

        [StringLength(5)]
        public string? InvUnidad { get; set; }

        public bool InvServicio { get; set; } /*= true;*/

        public bool InvEstado { get; set; } = true;

        [Column(TypeName = "decimal(10,2)")]
        public decimal InvStockGlobal { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal InvPrecio { get; set; }

        [StringLength(255)]
        public string? InvFoto { get; set; }

        public DateTime InvFechaAlta { get; set; }
    }
}
