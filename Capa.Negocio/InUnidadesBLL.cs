using System.Collections.Generic;
using Capa.Datos;
using Capa.Entity;

namespace Capa.Negocio
{
    public class InUnidadesBLL
    {
        private readonly InUnidadesDAL _dal;

        public InUnidadesBLL(string cadenaConexion)
        {
            _dal = new InUnidadesDAL(cadenaConexion);
        }

        public List<InUnidadesCLS> Listar() => _dal.Listar();

        public InUnidadesCLS? Obtener(string codigo) => _dal.Obtener(codigo);

        public bool Insertar(InUnidadesCLS dto) => _dal.Insertar(dto) > 0;

        public bool Editar(InUnidadesCLS dto) => _dal.Actualizar(dto) > 0;

        public bool Eliminar(string codigo) => _dal.Eliminar(codigo) > 0;

        public bool Existe(string codigo) => _dal.Existe(codigo);

        public List<SatUnidadCLS> ListarClavesSat() => _dal.ListarClavesSat();

        public bool ExisteClaveSat(string clave) => _dal.ExisteClaveSat(clave);

        public bool InsertarClaveSat(SatUnidadCLS sat) => _dal.InsertarClaveSat(sat) > 0;
    }
}
