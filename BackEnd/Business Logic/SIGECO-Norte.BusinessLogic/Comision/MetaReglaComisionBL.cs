
using System.Collections.Generic;
using SIGEES.Entidades;
using SIGEES.DataAcces;
using System.Transactions;
using System;

namespace SIGEES.BusinessLogic
{
    public class MetaReglaComisionBL : GenericBL<MetaReglaComisionBL>
    {
        /*
        public List<regla_pago_comision_dto> Listar(regla_pago_comision_search_dto busqueda)
        {
            return ReglaPagoComisionDA.Instance.Listar(busqueda);
        }


        

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
        }*/

        public MensajeDTO Actualizar(meta_regla_comision_dto v_entidad)
        {
            int v_codigo_regla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla = MetaReglaComisionDA.Instance.Actualizar(v_entidad);

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
        public MensajeDTO Insertar(meta_regla_comision_dto v_entidad)
        {
            int v_codigo_regla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla = MetaReglaComisionDA.Instance.Insertar(v_entidad);

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
        public MensajeDTO Eliminar(meta_regla_comision_dto v_entidad)
        {
            int v_resultado = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_resultado = MetaReglaComisionDA.Instance.Eliminar(v_entidad);

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
        public List<meta_regla_comision_dto> GetListByIdRegla(int codigo_regla_pago)
        {
            return MetaReglaComisionDA.Instance.GetListByIdRegla(codigo_regla_pago);
        }

     
    }
}
