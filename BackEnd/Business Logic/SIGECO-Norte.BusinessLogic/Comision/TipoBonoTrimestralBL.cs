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
    public class TipoBonoTrimestralBL : GenericBL<TipoBonoTrimestralBL>
    {
        public string ComboJson()
        {
            return JsonConvert.SerializeObject(TipoBonoTrimestralDA.Instance.ComboJson());
        }
    }
}
