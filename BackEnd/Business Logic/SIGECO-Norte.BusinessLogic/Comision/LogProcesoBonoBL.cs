using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
    public class LogProcesoBonoBL : GenericBL<LogProcesoBonoBL>
    {
        public List<log_proceso_bono_listado_dto> Listar(string fecha_inicio, string fecha_fin)
        {
            return LogProcesoBonoDA.Instance.Listar(fecha_inicio, fecha_fin);
        }

        public List<log_proceso_bono_detalle_dto> Detalle(int codigo_planilla)
        {
            return LogProcesoBonoDA.Instance.Detalle(codigo_planilla);
        }

        public log_proceso_bono_fechas_dto Fechas()
        {
            return LogProcesoBonoDA.Instance.Fechas();
        }

    }
}
