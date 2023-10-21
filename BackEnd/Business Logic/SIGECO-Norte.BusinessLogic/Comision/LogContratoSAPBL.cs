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
    public class LogContratoSAPBL : GenericBL<LogContratoSAPBL>
    {
        public List<log_contrato_sap_listado_dto> Listar(log_contrato_sap_fechas_dto busqueda,int pSede)
        {
            return LogContratoSAPDA.Instance.Listar(busqueda, pSede);
        }

        public log_contrato_sap_detalle_dto Detalle(string codigo_empresa, string nro_contrato)
        {
            return LogContratoSAPDA.Instance.Detalle(codigo_empresa, nro_contrato);
        }

        public log_contrato_sap_fechas_dto Fechas()
        {
            return LogContratoSAPDA.Instance.Fechas();
        }

        public MensajeDTO HabilitarReproceso(string procesar, string usuario)
        {
            return LogContratoSAPDA.Instance.HabilitarReproceso(procesar, usuario);
        }

        public MensajeDTO Bloquear(log_contrato_sap_bloqueo_dto v_entidad)
        {
            var v_mensaje = new MensajeDTO();
            int v_resultado = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_resultado = LogContratoSAPDA.Instance.Bloquear(v_entidad);

                    v_mensaje.idOperacion = v_resultado;
                    v_mensaje.mensaje = "Se realizó la operación satisfactoriamente.";
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;
                }
            }

            return v_mensaje;
        }

    }
}
