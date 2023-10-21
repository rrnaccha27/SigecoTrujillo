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
    public class ReglaCalculoComisionSupervisorBL : GenericBL<ReglaCalculoComisionSupervisorBL>
    {

        public List<regla_calculo_comision_supervisor_dto> Listar(regla_calculo_comision_supervisor_dto busqueda)
        {
            return ReglaCalculoComisionSupervisorDA.Instance.Listar(busqueda);
        }


        public MensajeDTO Insertar(regla_calculo_comision_supervisor_dto v_entidad)
        {
            int v_codigo_regla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla = ReglaCalculoComisionSupervisorDA.Instance.Insertar(v_entidad);

                    v_mensaje.idRegistro = v_codigo_regla;
                    v_mensaje.idOperacion = 1;
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

        public MensajeDTO Actualizar(regla_calculo_comision_supervisor_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = ReglaCalculoComisionSupervisorDA.Instance.Actualizar(v_entidad);

                    v_mensaje.idRegistro = v_resultado;
                    v_mensaje.idOperacion = 1;
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

        public MensajeDTO Eliminar(regla_calculo_comision_supervisor_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = ReglaCalculoComisionSupervisorDA.Instance.Eliminar(v_entidad);

                    v_mensaje.idRegistro = v_resultado;
                    v_mensaje.idOperacion = 1;
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

        public regla_calculo_comision_supervisor_dto BuscarById(int codigo_regla_pago)
        {
            return ReglaCalculoComisionSupervisorDA.Instance.BuscarById(codigo_regla_pago);
        }

        public int Validar(regla_calculo_comision_supervisor_dto pEntidad)
        {
            return ReglaCalculoComisionSupervisorDA.Instance.Validar(pEntidad);
        }
    }
}
