using System.Collections.Generic;
using Capa.Datos;
using Capa.Entity;

namespace Capa.Negocio
{
    public class InTipoMovimientoBLL
    {
        private readonly InTipoMovimientoDAL dal;

        public InTipoMovimientoBLL(string cadenaConexion)
        {
            dal = new InTipoMovimientoDAL(cadenaConexion);
        }

        public List<InTipoMovimientoCLS> Listar() => dal.Listar();

        public InTipoMovimientoCLS? Obtener(short id) => dal.Obtener(id);

        public int Insertar(InTipoMovimientoCLS dto) => dal.Insertar(dto);

        // Corregido: Usar el método Actualizar en lugar de Editar
        public bool Editar(InTipoMovimientoCLS dto) => dal.Actualizar(dto) > 0;

        public bool Eliminar(short id) => dal.Eliminar(id) > 0;
    }
}
