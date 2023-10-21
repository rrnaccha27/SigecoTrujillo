using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.Entidades;
using SIGEES.DataAcces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SIGEES.BusinessLogic
{
    public class TipoComisionSupervisorBL : GenericBL<TipoComisionSupervisorBL>
    {
        //private readonly SIGEES.DataAcces.ArticuloDA oArticuloDA = new DataAcces.ArticuloDA();

        public string Listar()
        {
            return JsonConvert.SerializeObject(TipoComisionSupervisorDA.Instance.Listar());
        }
    }
}
