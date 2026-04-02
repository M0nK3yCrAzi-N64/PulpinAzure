using Capa.Datos;
using Capa.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capa.Negocio
{
    public class InArticuloBLL
    {
        private readonly InArticuloDAL _dal = new InArticuloDAL();

        public List<InArticuloCLS> listarArticulo()
        {
            return _dal.listarArticulo() ?? new List<InArticuloCLS>();
        }

        public (bool Success, string Message) insertarArticulo(InArticuloCLS obj)
        {
            return _dal.insertarArticulo(obj);
        }

        public (bool Success, string Message) editarArticulo(InArticuloCLS obj)
        {
            return _dal.editarArticulo(obj);
        }

        public (bool Success, string Message) eliminarArticulo(string invSku)
        {
            return _dal.eliminarArticulo(invSku);
        }
    }
}