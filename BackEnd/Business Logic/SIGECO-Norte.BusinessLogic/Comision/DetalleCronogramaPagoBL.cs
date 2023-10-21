using SIGEES.DataAcces;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIGEES.BusinessLogic
{
    public partial class DetalleCronogramaPagoBL : GenericBL<DetalleCronogramaPagoBL>
    {

        public MensajeDTO HabilitarPago(detalle_cronograma_dto v_detalle_planilla)
        {
            int v_codigo_detalle_cronograma = 0;
            
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_codigo_detalle_cronograma = DetalleCronogramaPagoDA.Instance.HabilitarPago(v_detalle_planilla);

                    v_mensaje.idRegistro = v_codigo_detalle_cronograma;
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

        public MensajeDTO RefinanciarPagoComisionCuota(detalle_cronograma_comision_dto v_cuota)
        {
            int v_codigo_detalle_cronograma = 0;

            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_codigo_detalle_cronograma = DetalleCronogramaPagoDA.Instance.RefinanciarPagoComisionCuota(v_cuota);

                    v_mensaje.idRegistro = v_codigo_detalle_cronograma;
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

        public MensajeDTO AnularCuotaComision(detalle_cronograma_comision_dto v_cuota)
        {
            int v_codigo_detalle_cronograma = 0;

            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_codigo_detalle_cronograma = DetalleCronogramaPagoDA.Instance.AnularCuotaComision(v_cuota);

                    v_mensaje.idRegistro = v_codigo_detalle_cronograma;
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

        public MensajeDTO GestionExclusionHabilitarPagoComision(List<grilla_cuota_pago_planilla_dto> lst_cuota_pago_comision)
        {
            int v_codigo_detalle_cronograma = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    foreach (var item in lst_cuota_pago_comision)
                    {
                        v_codigo_detalle_cronograma = DetalleCronogramaPagoDA.Instance.GestionExclusionHabilitarPagoComision(item);
                    }                   

                    v_mensaje.idRegistro = v_codigo_detalle_cronograma;
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

        public MensajeDTO Modificar(detalle_cronograma_comision_dto detalle_cronograma)
        {
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    DetalleCronogramaPagoDA.Instance.Modificar(detalle_cronograma);

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

        public MensajeDTO Adicionar(detalle_cronograma_adicionar_dto detalle_cronograma)
        {
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_mensaje.idRegistro = DetalleCronogramaPagoDA.Instance.Adicionar(detalle_cronograma);
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

        public MensajeDTO Deshabilitar(detalle_cronograma_deshabilitacion_dto v_detalle_cronograma)
        {
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    DetalleCronogramaPagoDA.Instance.Deshabilitar(v_detalle_cronograma);

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

        public MensajeDTO Aprobar(List<detalle_cronograma_personal_inactivo_dto> lst_detalle, int nivel, int codigo_resultado, string codigo_usuario, string observacion)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    DetalleCronogramaPagoDA.Instance.Aprobar(lst_detalle, nivel, codigo_resultado, codigo_usuario, observacion);
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

    }

    public partial class DetalleCronogramaPagoSelBL : GenericBL<DetalleCronogramaPagoSelBL>
   {

        public detalle_cronograma_comision_dto CuotaPagoById(int codigo_detalle_cronograma)
        {
            return DetalleCronogramaPagoSelDA.Instance.CuotaPagoById(codigo_detalle_cronograma);
        }

        public List<listado_exclusion_grilla_dto> ListarPagosExcluidosAll(listado_exclusion_grilla_dto v_entidad)
        {
            return DetalleCronogramaPagoSelDA.Instance.ListarPagosExcluidosAll(v_entidad);
        }

        public detalle_exclusion_dto GetDetalleExclusionCuotaPagoComision( int p_codigo_exclusion)
        {
            return DetalleCronogramaPagoSelDA.Instance.GetDetalleExclusionCuotaPagoComision(p_codigo_exclusion);
        }

        public resumen_pago_comision_personal_dto GetPagoComisionByArticuloPersonal(cronograma_pago_filtro p_cronograma_pago_filtro)
        {
            return DetalleCronogramaPagoSelDA.Instance.GetPagoComisionByArticuloPersonal(p_cronograma_pago_filtro);
        }

        public List<grilla_comision_cronograma_dto> CronogramaPagoComisionListar(grilla_comision_cronograma_filtro v_entidad)
        {
            return DetalleCronogramaPagoSelDA.Instance.CronogramaPagoComisionListar(v_entidad);
        }

        public System.Data.DataTable CronogramaPagoComisionDataTable(grilla_comision_cronograma_filtro v_entidad)
        {
            return DetalleCronogramaPagoSelDA.Instance.CronogramaPagoComisionDataTable(v_entidad);
        }

        public List<operacion_cuota_comision_listado_dto> ListadoOperacion(operacion_cuota_comision_listado_dto busqueda)
        {
            return DetalleCronogramaPagoSelDA.Instance.ListadoOperacion(busqueda);
        }

        public List<detalle_cronograma_personal_inactivo_dto> ListadoComisionPersonalInactivo(detalle_cronograma_personal_inactivo_busqueda_dto busqueda)
        {
            return DetalleCronogramaPagoSelDA.Instance.ListadoComisionPersonalInactivo(busqueda);
        }

        public detalle_cronograma_personal_inactivo_busqueda_dto FechasComisionPersonalInactivo()
        {
            return DetalleCronogramaPagoSelDA.Instance.FechasComisionPersonalInactivo();
        }
        public detalle_cronograma_aprobacion_dto ComisionPersonalInactivo(int codigo_detalle_cronograma)
        {
            return DetalleCronogramaPagoSelDA.Instance.ListadoComisionPersonalInactivo(codigo_detalle_cronograma);
        }

    }
}
