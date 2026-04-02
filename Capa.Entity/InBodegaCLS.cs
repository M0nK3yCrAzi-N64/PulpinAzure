using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Entity
{
    public class InBodegaCLS
    {
        public short BOD_SUCURSAL { get; set; }
        public int BOD_BODEGA { get; set; }
        public string BOD_NOMBRE { get; set; }
        public char BOD_TIPO { get; set; } // 'F' o 'T'
    }
}