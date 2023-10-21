using SIGEES.DataAcces;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIGEES.BusinessLogic
{
    public class PeriodoTrimestralBL : GenericBL<PeriodoTrimestralBL>
    {
        public List<combo_periodo_trimestral_dto> GetAllComboJson()
        {
            return PeriodoTrimestralDA.Instance.GetAllComboJson();
        }
    }
}


