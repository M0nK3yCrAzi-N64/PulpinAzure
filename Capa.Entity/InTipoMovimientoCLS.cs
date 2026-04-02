using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capa.Entity
{
    public class InTipoMovimientoCLS
    {
        public short INTIPMOV_ID { get; set; }
        public string INTIPMOV_DESCRIPCION { get; set; } = string.Empty;
        public char INTIPMOV_TIPO { get; set; } // 'E' (entrada) / 'S' (salida)
        public short INTIPMOV_FACTOR { get; set; } // +1 / -1
    }
}