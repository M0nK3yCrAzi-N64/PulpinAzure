using System;

namespace Capa.Entity
{
    public class FaClientesCLS
    {
        public short? CTE_SUCURSAL { get; set; }
        public int CTE_CLIENTEID { get; set; }
        public string CTE_RFC { get; set; } = string.Empty;
        public string CTE_NOMBRE { get; set; } = string.Empty;

        public string CTE_DIRECCION { get; set; } = string.Empty;
        public string CTE_NUMEXT { get; set; } = string.Empty;
        public string CTE_NUMINT { get; set; } = string.Empty;
        public int? CTE_COLONIA { get; set; }
        public string CTE_CPOSTAL { get; set; } = string.Empty;
        public string CTE_CIUDAD { get; set; } = string.Empty;
        public string CTE_ESTADO { get; set; } = string.Empty;
        public string CTE_PAIS { get; set; } = string.Empty;

        public int? CTE_AGENTE { get; set; }
        public double? CTE_PIVA { get; set; }
        public double? CTE_DESCUENTO { get; set; }
        public double? CTE_LIMITECR { get; set; }
        public double? CTE_SALDO { get; set; }

        public string CTE_CONTACTO { get; set; } = string.Empty;
        public string CTE_TELEFONO { get; set; } = string.Empty;
        public string CTE_CORREO { get; set; } = string.Empty;

        public string CTE_OBSERVACIONES { get; set; } = string.Empty;
        public bool CTE_BLOQUEAR { get; set; }

        public DateTime? CTE_FECHAALTA { get; set; }
        public DateTime? CTE_ULTIMOMOVIMIENTO { get; set; }
    }
}
