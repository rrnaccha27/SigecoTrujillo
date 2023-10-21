using SIGEES.DataAcces;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Configuration;

namespace SIGEES.BusinessLogic
{
    public partial class PlanillaBL : GenericBL<PlanillaBL>
    {
        int _TransactionTimeout = 0;

        public PlanillaBL() 
        {
            _TransactionTimeout = System.Convert.ToInt32(ConfigurationManager.AppSettings["TransactionTimeOut"].ToString());
        }

        #region SECCIN-PLANILLA ADMINISTRATVIA Y COMERCIAL

        public MensajeDTO Generar(Planilla_dto v_planilla)
        {

            int v_codigo_planilla = 0;
            int p_cantidad_registro_procesado;
            string usuario = string.Empty;
            var v_mensaje = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {
                    usuario = v_planilla.usuario_registra;
                    
                    /*Se guarda la Planilla*/
                    v_codigo_planilla = PlanillaDA.Instance.Generar(v_planilla, out p_cantidad_registro_procesado);

                    /*Se bloquea la data de Vendedores*/
                    /*Se coloco en el metodo de Generar*/

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    v_mensaje.total_registro_afectado = p_cantidad_registro_procesado;
                    v_mensaje.mensaje = "Se generó correctamente";
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
        public MensajeDTO Cerrar(Planilla_dto v_planilla)
        {
            int v_codigo_planilla = 0;


            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {

                    v_codigo_planilla = PlanillaDA.Instance.Cerrar(v_planilla);

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    //v_mensaje.mensaje = "Se generó correctamente";
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
        public MensajeDTO Registrar_Descuento(List<descuento_dto> v_descuento)
        {

            int v_codigo_descuento = 0;


            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    foreach (var item in v_descuento)
                    {
                        v_codigo_descuento = PlanillaDA.Instance.Registrar_Descuento(item);
                    }
                    

                    v_mensaje.idRegistro = v_codigo_descuento;
                    v_mensaje.idOperacion = 1;
                    v_mensaje.mensaje = "Se generó correctamente";
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
        public MensajeDTO Desactivar_Descuento(descuento_dto v_descuento)
        {

            int v_codigo_descuento = 0;


            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_codigo_descuento = PlanillaDA.Instance.Desactivar_Descuento(v_descuento);

                    v_mensaje.idRegistro = v_codigo_descuento;
                    v_mensaje.idOperacion = 1;
                    v_mensaje.mensaje = "Se generó correctamente";
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

        public MensajeDTO Anular(Planilla_dto v_planilla)
        {
            int v_codigo_planilla = 0;


            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {

                    v_codigo_planilla = PlanillaDA.Instance.Anular(v_planilla);

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    //v_mensaje.mensaje = "Se generó correctamente";
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
        
        #endregion

        #region SECCION-PLANILLA BONO       
        public MensajeDTO GenerarPlanillaBono(Planilla_bono_dto v_planilla, Boolean es_planilla_jn = false)
        {
            int v_codigo_planilla = 0;
            int p_cantidad_registro_procesado;

            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {

                    v_codigo_planilla = PlanillaDA.Instance.GenerarPlanillaBono(v_planilla, out p_cantidad_registro_procesado, es_planilla_jn);

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    v_mensaje.total_registro_afectado = p_cantidad_registro_procesado;
                    v_mensaje.mensaje = "Se generó correctamente";
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
        public MensajeDTO AnularPlanillaBono(Planilla_bono_dto v_planilla)
        {
            int v_codigo_planilla = 0;


            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {

                    v_codigo_planilla = PlanillaDA.Instance.AnularPlanillaBono(v_planilla);

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    //v_mensaje.mensaje = "Se generó correctamente";
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
        public MensajeDTO CerrarPlanillaBono(Planilla_bono_dto v_planilla)
        {
            int v_codigo_planilla = 0;


            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {

                    v_codigo_planilla = PlanillaDA.Instance.CerrarPlanillaBono(v_planilla);

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    //v_mensaje.mensaje = "Se generó correctamente";
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

        public MensajeDTO AnularPagoBono(detalle_planilla_bono_exclusion_dto v_detalle)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {
                    PlanillaDA.Instance.AnularPagoBono(v_detalle);

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

        #endregion

        #region PLANILLA BONO TRIMESTRAL

        public MensajeDTO GenerarPlanillaBonoTrimestral(planilla_bono_trimestral_dto v_planilla)
        {
            int v_codigo_planilla = 0;
            int p_cantidad_registro_procesado;

            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {
                    v_codigo_planilla = PlanillaDA.Instance.GenerarPlanillaBonoTrimestral(v_planilla, out p_cantidad_registro_procesado);

                    v_mensaje.idRegistro = v_codigo_planilla;
                    v_mensaje.idOperacion = 1;
                    v_mensaje.total_registro_afectado = p_cantidad_registro_procesado;
                    //v_mensaje.mensaje = "Se generó correctamente";
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

        public MensajeDTO AnularPlanillaBonoTrimestral(planilla_bono_trimestral_dto v_planilla)
        {
            int v_codigo_planilla = 0;
            var v_mensaje = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {
                    v_codigo_planilla = PlanillaDA.Instance.AnularPlanillaBonoTrimestral(v_planilla);

                    v_mensaje.idRegistro = v_codigo_planilla;
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
        public MensajeDTO CerrarPlanillaBonoTrimestral(planilla_bono_trimestral_dto v_planilla)
        {
            int v_codigo_planilla = 0;
            var v_mensaje = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(_TransactionTimeout)))
            {
                try
                {
                    v_codigo_planilla = PlanillaDA.Instance.CerrarPlanillaBonoTrimestral(v_planilla);

                    v_mensaje.idRegistro = v_codigo_planilla;
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
        
        #endregion

    }

    public partial class PlanillaSelBL : GenericBL<PlanillaSelBL>
   {

        public List<Planilla_dto> Listar() 
        {
            return PlanillaSelDA.Instance.Listar();
        }

        public Planilla_dto BuscarById(int p_codigo_planilla)
        {
            return PlanillaSelDA.Instance.BuscarById(p_codigo_planilla);
        }

        public System.Data.DataTable ReporteCabeceraPlanilla(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteCabeceraPlanilla(v_codigo_planilla);
        }

        public List<reporte_detalle_planilla> ReporteDetallePlanilla(int v_codigo_planilla, int v_codigo_personal)
        {
            return PlanillaSelDA.Instance.ReporteDetallePlanilla(v_codigo_planilla, v_codigo_personal);
        }

        public List<reporte_detalle_liquidacion> ReporteDetalleLiquidacionPlanilla(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteDetalleLiquidacionPlanilla(v_codigo_planilla);
        }

        /* RC*/
        public List<reporte_liquidacion_Supervisor> ReporteDetalleLiquidacionPlanillaSupervisor(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteDetalleLiquidacionPlanillaSupervisor(v_codigo_planilla);
        }

        public List<reporte_liquidacion_Supervisor> ReporteLiquidacionPlanillaBonoSupervisor(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionPlanillaBonoSupervisor(v_codigo_planilla);
        }
        /**/

        public Planilla_bono_dto BuscarPlanillaBonoById(int p_codigo_planilla)
        {
            return PlanillaSelDA.Instance.BuscarPlanillaBonoById(p_codigo_planilla);;
        }

        public planilla_bono_trimestral_dto BuscarPlanillaBonoTrimestralById(int p_codigo_planilla)
        {
            return PlanillaSelDA.Instance.BuscarPlanillaBonoTrimestralById(p_codigo_planilla); ;
        }

        public List<grilla_planilla_bono> ListarPlanillaBono(Boolean es_planilla_jn = false)
        {
            return PlanillaSelDA.Instance.ListarPlanillaBono(es_planilla_jn);
        }

        public List<grilla_planilla_bono_trimestral> ListarPlanillaBonoTrimestral()
        {
            return PlanillaSelDA.Instance.ListarPlanillaBonoTrimestral();
        }

        public System.Data.DataTable ReportePlanillaBonoPersonal(int v_codigo_planilla, int p_codigo_personal)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoPersonal(v_codigo_planilla, p_codigo_personal);
        }

        public System.Data.DataTable ReporteDetallePlanillaComercial(int p_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteDetallePlanillaComercial(p_codigo_planilla);
        }

        public System.Data.DataTable ReporteLiquidacionBonoPlanillaPersonal(int v_codigo_planilla,int p_codigo_personal)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionBonoPlanillaPersonal(v_codigo_planilla, p_codigo_personal);
        }

        public System.Data.DataTable ReporteLiquidacionBonoPlanillaArticulos(int v_codigo_planilla, int p_codigo_empresa, string p_nro_contrato)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionBonoPlanillaArticulos(v_codigo_planilla, p_codigo_empresa, p_nro_contrato);
        }

        public List<grilla_planilla_exclusion_dto> ListarPlanillaAbiertaGestionExclusion()
        {
            return PlanillaSelDA.Instance.ListarPlanillaAbiertaGestionExclusion();
        }

        public List<grilla_cuota_pago_planilla_dto> ListarPagoComisionVsPlanillaAbiertaGestionExclusion(List<collection_id_exclusion_dto> v_lst_id_exclusion)
        {
            return PlanillaSelDA.Instance.ListarPagoComisionVsPlanillaAbiertaGestionExclusion(v_lst_id_exclusion);
        }

        public System.Data.DataTable ReportePlanillaBonoSupervisor(int v_codigo_planilla, int p_codigo_personal)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoSupervisor(v_codigo_planilla, p_codigo_personal);
        }

        public System.Data.DataTable ReporteLiquidacionBonoSupervisor(int v_codigo_planilla, int p_codigo_personal)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionBonoSupervisor(v_codigo_planilla, p_codigo_personal);
        }

        public System.Data.DataTable ReporteLiquidacionBonoIndividual(int v_codigo_planilla, int p_codigo_personal)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionBonoIndividual(v_codigo_planilla, p_codigo_personal);
        }

        public List<reporte_detalle_liquidacion_bono> ReporteLiquidacionBonoIndividual(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionBonoIndividual(v_codigo_planilla);
        }

        public System.Data.DataTable ReporteResumenEmpresaPlanilla(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteResumenEmpresaPlanilla(v_codigo_planilla);
        }

        public List<cabecera_txt_dto> GetReportePlanillaParaTxt(int p_codigo_planilla, bool antiguo = false)
        {
            return PlanillaSelDA.Instance.GetReportePlanillaParaTxt(p_codigo_planilla, antiguo);
        }

        public List<cabecera_txt_dto> GetPlanillaBonoParaTxt(int p_codigo_planilla, bool antiguo = false)
        {
            return PlanillaSelDA.Instance.GetPlanillaBonoParaTxt(p_codigo_planilla, antiguo);
        }

        public List<txt_contabilidad_resumen_planilla_dto> GetResumenPlanillaTxt_Contabilidad(int codigo_planilla, bool antiguo = false)
        {
            return PlanillaSelDA.Instance.GetResumenPlanillaTxt_Contabilidad(codigo_planilla, antiguo);
        }

        public List<txt_contabilidad_resumen_planilla_dto> GetResumenPlanillaBonoTxt_Contabilidad(int codigo_planilla, bool antiguo = false)
        {
            return PlanillaSelDA.Instance.GetResumenPlanillaBonoTxt_Contabilidad(codigo_planilla, antiguo);
        }

        public List<txt_contabilidad_planilla_dto> GetPlanillaTxt_Contabilidad(int codigo_checklist, int codigo_empresa, bool antiguo = false, int codigo_planilla = 0)
        {
            return PlanillaSelDA.Instance.GetPlanillaTxt_Contabilidad(codigo_checklist, codigo_empresa, antiguo, codigo_planilla);
        }

        public List<txt_contabilidad_planilla_dto> GetPlanillaBonoTxt_Contabilidad(int codigo_checklist, int codigo_empresa, bool antiguo = false, int codigo_planilla = 0)
        {
            return PlanillaSelDA.Instance.GetPlanillaBonoTxt_Contabilidad(codigo_checklist, codigo_empresa, antiguo);
        }

        public System.Data.DataTable ReporteLiquidacionBonoPorcentajes(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReporteLiquidacionBonoPorcentajes(v_codigo_planilla);
        }

        public planilla_resumen_dto BuscarUltimoCerrado()
        {
            return PlanillaSelDA.Instance.BuscarUltimoCerrado();
        }

        public planilla_resumen_dto BuscarUltimoAbiertoPorArticulo(regla_calculo_comision_validacion_dto comision)
        {
            return PlanillaSelDA.Instance.BuscarUltimoAbiertoPorArticulo(comision);
        }

        public System.Data.DataTable ReportePlanillaBonoJNDetalle(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoJNDetalle(v_codigo_planilla);
        }
        public System.Data.DataTable ReportePlanillaBonoJNResumen(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoJNResumen(v_codigo_planilla);
        }
        public System.Data.DataTable ReportePlanillaBonoJNResumenTitulos(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoJNResumenTitulos(v_codigo_planilla);
        }

        public System.Data.DataTable ReportePlanillaBonoJN(int p_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoJN(p_codigo_planilla); ;
        }

        public System.Data.DataTable ReportePlanillaBonoJNContabilidad(int p_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoJNContabilidad(p_codigo_planilla);
        }

        public List<planilla_checklist_comision_dto> ListarParaChecklistComision()
        {
            return PlanillaSelDA.Instance.ListarParaChecklistComision();
        }
        public List<planilla_checklist_bono_dto> ListarParaChecklistBono()
        {
            return PlanillaSelDA.Instance.ListarParaChecklistBono();
        }

        public List<planilla_checklist_bono_trimestral_dto> ListarParaChecklistBonoTrimestral()
        {
            return PlanillaSelDA.Instance.ListarParaChecklistBonoTrimestral();
        }

        public System.Data.DataTable ReportePlanillaBonoJNLiquidacion(int v_codigo_planilla)
        {
            return PlanillaSelDA.Instance.ReportePlanillaBonoJNLiquidacion(v_codigo_planilla);
        }

   }
}
