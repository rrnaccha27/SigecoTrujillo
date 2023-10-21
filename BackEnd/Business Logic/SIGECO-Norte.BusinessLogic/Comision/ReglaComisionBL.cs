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
    public class ReglaComisionBL : GenericBL<ReglaComisionBL>
    {

        public List<regla_pago_comision_dto> Listar(regla_pago_comision_search_dto busqueda)
        {
            return ReglaComisionDA.Instance.Listar(busqueda);
        }
        public regla_pago_comision_dto BuscarById(int codigo_regla_pago)
        {
            return ReglaComisionDA.Instance.BuscarById(codigo_regla_pago);
        }

        
        public MensajeDTO Insertar(regla_pago_comision_dto v_entidad)
        {
            int v_codigo_regla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla = ReglaComisionDA.Instance.Insertar(v_entidad);

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
        public MensajeDTO Actualizar(regla_pago_comision_dto v_entidad)
        {
            int v_codigo_regla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla = ReglaComisionDA.Instance.Actualizar(v_entidad);

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
        public MensajeDTO Eliminar(regla_pago_comision_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = ReglaComisionDA.Instance.Eliminar(v_entidad);

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
        /*
        public MensajeDTO Actualizar(old_regla_pago_comision_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = ReglaPagoComisionDA.Instance.Actualizar(v_entidad);

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

     
          public int Validar(old_regla_pago_comision_dto pEntidad)
        {
            return ReglaPagoComisionDA.Instance.Validar(pEntidad);
        }

        public List<regla_pago_comision_orden_dto> ListarOrden()
        {
            return ReglaPagoComisionDA.Instance.ListarOrden();
        }
       */



    }
}
