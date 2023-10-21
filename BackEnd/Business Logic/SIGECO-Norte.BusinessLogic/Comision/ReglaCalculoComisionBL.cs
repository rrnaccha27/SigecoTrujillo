using SIGEES.DataAcces;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.BusinessLogic
{
    public class ReglaCalculoComisionBL : GenericBL<ReglaCalculoComisionBL>
    {
        public List<regla_calculo_comision_dto> ListarByPrecio(int codigo_precio)
        {
            return Regla_Calculo_ComisonDA.Instance.ListarByPrecio(codigo_precio);
        }

    }
}
