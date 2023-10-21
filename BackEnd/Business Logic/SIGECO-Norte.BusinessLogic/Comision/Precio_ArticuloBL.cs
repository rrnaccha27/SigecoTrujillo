using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.Entidades;

namespace SIGEES.BusinessLogic
{
    public class Precio_ArticuloBL : GenericBL<Precio_ArticuloBL>
    {

        private readonly SIGEES.DataAcces.Precio_ArticuloDA oPrecio_ArticuloDA = new DataAcces.Precio_ArticuloDA();

        public List<precio_articulo_dto> Listar(precio_articulo_dto busqueda)
        {
            return oPrecio_ArticuloDA.Listar(busqueda);
        }

    }
}
