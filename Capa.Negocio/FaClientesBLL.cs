using System.Collections.Generic;
using Capa.Datos;
using Capa.Entity;

namespace Capa.Negocio
{
    public class FaClientesBLL
    {
        private readonly FaClientesDAL _dal;

        public FaClientesBLL(string cadenaConexion)
        {
            _dal = new FaClientesDAL(cadenaConexion);
        }

        public List<FaClientesCLS> Listar() => _dal.Listar();

        public FaClientesCLS? Obtener(int idCliente) => _dal.Obtener(idCliente);

        public int Insertar(FaClientesCLS dto) => _dal.Insertar(dto);

        public bool Editar(FaClientesCLS dto) => _dal.Actualizar(dto) > 0;

        public bool Eliminar(int idCliente) => _dal.Eliminar(idCliente) > 0;
    }
}
