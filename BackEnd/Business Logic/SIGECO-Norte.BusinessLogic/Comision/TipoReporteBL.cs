using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using Newtonsoft.Json;

using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
    public class TipoReporteBL: GenericBL<TipoReporteBL>
    {
        public string ComboJson()
        {
            return JsonConvert.SerializeObject(TipoReporteDA.Instance.ComboJson());
        }
    }
}
