using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Entity
{
    public class InExistenciaCLS
    {
        public string InvSku { get; set; } = string.Empty; // INEXI_SKU
        public short BodSuc { get; set; }                  // INEXI_BODSUC
        public int Bodega { get; set; }                    // INEXI_BODEGA
        public int? LocId { get; set; }                    // INEXI_LOCID
        public decimal Cantidad { get; set; }              // INEXI_CANTIDAD
    }
}