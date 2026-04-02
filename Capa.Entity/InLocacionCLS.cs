using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Entity
{
    public class InLocacionCLS
    {
        public int Id { get; set; }              // INLOC_ID
        public short Sucursal { get; set; }      // INLOC_SUCURSAL
        public int Bodega { get; set; }          // INLOC_BODEGA
        public string Codigo { get; set; } = string.Empty;     // INLOC_CODIGO
        public string? Descripcion { get; set; }               // INLOC_DESCRIPCION
    }
}