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
    public class ReglaPagoComisionExcepcionBL : GenericBL<ReglaPagoComisionExcepcionBL>
    {

        public List<regla_pago_comision_excepcion_dto> Listar(regla_pago_comision_excepcion_dto busqueda)
        {
            return ReglaPagoComisionExcepcionDA.Instance.Listar(busqueda);
        }


        public MensajeDTO Insertar(regla_pago_comision_excepcion_dto v_entidad)
        {
            int v_codigo_regla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla = ReglaPagoComisionExcepcionDA.Instance.Insertar(v_entidad);

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

        public MensajeDTO Actualizar(regla_pago_comision_excepcion_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = ReglaPagoComisionExcepcionDA.Instance.Actualizar(v_entidad);

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

        public MensajeDTO Eliminar(regla_pago_comision_excepcion_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = ReglaPagoComisionExcepcionDA.Instance.Eliminar(v_entidad);

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

        public regla_pago_comision_excepcion_dto BuscarById(int codigo_regla_pago)
        {
            return ReglaPagoComisionExcepcionDA.Instance.BuscarById(codigo_regla_pago);
        }

        public int Validar(regla_pago_comision_excepcion_dto pEntidad)
        {
            return ReglaPagoComisionExcepcionDA.Instance.Validar(pEntidad);
        }
    }
}
