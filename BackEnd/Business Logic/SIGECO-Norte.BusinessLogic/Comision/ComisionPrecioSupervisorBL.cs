using SIGEES.DataAcces;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.BusinessLogic
{
    public class ComisionPrecioSupervisorBL : GenericBL<ComisionPrecioSupervisorBL>
    {
        public List<comision_precio_supervisor_dto> ListarByPrecio(int codigo_precio)
        {
            return ComisionPrecioSupervisorDA.Instance.ListarByPrecio(codigo_precio);
        }

    }
}
