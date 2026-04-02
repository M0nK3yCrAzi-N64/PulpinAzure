// Capa.Negocio/InBodegaBLL.cs
using System.Collections.Generic;
using Capa.Entity;
using Capa.Datos;

namespace Capa.Negocio
{
    public class InBodegaBLL
    {
        private readonly InBodegaDAL dal;

        public InBodegaBLL(string cadenaConexion)
        {
            dal = new InBodegaDAL(cadenaConexion);
        }

        public List<InBodegaCLS> Listar() => dal.Listar();

        public int Insertar(InBodegaCLS bodega) => dal.Insertar(bodega);

        public int Actualizar(InBodegaCLS bodega) => dal.Actualizar(bodega);


        public int Eliminar(short sucursal, int bodega) => dal.Eliminar(sucursal, bodega);

        public bool Existe(short sucursal, int bodega) => dal.Existe(sucursal, bodega);
    }
}