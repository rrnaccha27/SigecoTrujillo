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
    public class DescuentoComisionBL : GenericBL<DescuentoComisionBL>
    {
        public List<descuento_comision_listado_dto> Listar()
        {
            return DescuentoComisionDA.Instance.Listar();
        }

        public descuento_comision_detalle_dto Detalle(descuento_comision_dto busqueda)
        {
            return DescuentoComisionDA.Instance.Detalle(busqueda);
        }

        public List<descuento_comision_planilla_dto> DetallePlanilla(descuento_comision_dto busqueda)
        {
            return DescuentoComisionDA.Instance.DetallePlanilla(busqueda);
        }

        public MensajeDTO Insertar(descuento_comision_dto descuento_comision)
        {
            int codigo_descuento_comision = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_descuento_comision = DescuentoComisionDA.Instance.Insertar(descuento_comision);
                    
                    v_mensaje.idRegistro = codigo_descuento_comision;
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

        public MensajeDTO Desactivar(descuento_comision_dto descuento_comision)
        {
            int codigo_descuento_comision = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_descuento_comision = descuento_comision.codigo_descuento_comision;

                    DescuentoComisionDA.Instance.Desactivar(descuento_comision);

                    v_mensaje.idRegistro = codigo_descuento_comision;
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

        public int Validar(descuento_comision_dto descuento_comision)
        {
            return DescuentoComisionDA.Instance.Validar(descuento_comision);
        }

        public MensajeDTO GenerarDescuento(descuento_comision_generar_dto descuento_comision)
        {
            int cantidad = 0;
            MensajeDTO v_mensaje = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    cantidad = DescuentoComisionDA.Instance.GenerarDescuento(descuento_comision);

                    v_mensaje.idOperacion = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;

                }
                v_mensaje.total_registro_afectado = cantidad;
            }

            return v_mensaje;
        }

        public MensajeDTO ValidarPlanilla(descuento_comision_generar_dto validacion)
        {
            int cantidad = 0;
            MensajeDTO v_mensaje = new MensajeDTO();

            try
            {
                cantidad = DescuentoComisionDA.Instance.ValidarPlanilla(validacion);
                v_mensaje.idOperacion = 1;
            }
            catch (Exception ex)
            {
                v_mensaje.mensaje = ex.Message;
                v_mensaje.idOperacion = -1;
            }

            v_mensaje.total_registro_afectado = cantidad;

            return v_mensaje;
        }
    }
}
