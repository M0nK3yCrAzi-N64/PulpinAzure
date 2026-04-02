using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa.Datos;
using Capa.Entity;
namespace Capa.Negocio
{
    public class InLocacionBLL
    {
        readonly InLocacionDAL dal = new InLocacionDAL();
        private string cadenaConexion;

        public InLocacionBLL(string cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
        }

        public InLocacionBLL()
        {
        }

        public List<InLocacionCLS> ListarPorBodega(short sucursal, int bodega) => dal.ListarPorBodega(sucursal, bodega);

        public int Insertar(InLocacionCLS dto) => dal.Insertar(dto);

        public bool Editar(InLocacionCLS dto) => dal.Editar(dto);

        public bool Eliminar(int id) => dal.Eliminar(id);
    }
}
