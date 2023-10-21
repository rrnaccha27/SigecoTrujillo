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
    public partial class DetallePlanillaBL : GenericBL<DetallePlanillaBL>
    {

        public MensajeDTO Excluir(detalle_planilla_dto v_detalle_planilla)
        {
            int v_codigo_detalle_cronograma = 0;
            
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_detalle_cronograma = DetallePlanillaDA.Instance.Excluir(v_detalle_planilla);

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
    
        public MensajeDTO Excluir(detalle_planilla_exclusion_dto v_detalle_planilla)
        {
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    DetallePlanillaDA.Instance.Excluir(v_detalle_planilla);

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

    public partial class DetallePlanillaSelBL : GenericBL<DetallePlanillaSelBL>
    {

        public List<detalle_planilla_resumen_dto> ListarByIdPlanilla(detalle_planilla_resumen_dto v_entidad)
        {
            return DetallePlanillaSelDA.Instance.ListarByIdPlanilla(v_entidad);
        }

        public List<detalle_planilla_resumen_dto> ListarComisionManualByIdPlanilla(detalle_planilla_resumen_dto v_entidad)
        {
            return DetallePlanillaSelDA.Instance.ListarComisionManualByIdPlanilla(v_entidad);
        }

        public List<detalle_planilla_resumen_dto> ListarDetalleByIdPlanilla(int p_codigo_planilla)
        {
           return DetallePlanillaSelDA.Instance.ListarDetalleByIdPlanilla(p_codigo_planilla);
        }
       
        public List<lista_descuento_dto> ListarDescuentoByIdPlanilla(int p_codigo_planilla)
        {
           return DetallePlanillaSelDA.Instance.ListarDescuentoByIdPlanilla(p_codigo_planilla);
        }
        
        public List<detalle_planilla_resumen_dto> ObtenerSaldoPersonalByPlanilla(int p_codigo_planilla, int p_codigo_persona, int p_codigo_estado_cuota)
        {
           return DetallePlanillaSelDA.Instance.ObtenerSaldoPersonalByPlanilla(p_codigo_planilla, p_codigo_persona, p_codigo_estado_cuota);
        }

        public List<detalle_planilla_resumen_dto> ListarPagoHabilitadoByIdPlanilla(int p_codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ListarPagoHabilitadoByIdPlanilla(p_codigo_planilla);
        }

        public List<detalle_planilla_resumen_dto> ListarByIdPlanillaBono(int codigo_planilla, Boolean es_planilla_jn = false)
        {
            return DetallePlanillaSelDA.Instance.ListarByIdPlanillaBono(codigo_planilla, es_planilla_jn);
        }
        public List<detalle_planilla_bono_trimestral_dto> ListarByIdPlanillaBonoTrimestral(int codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ListarByIdPlanillaBonoTrimestral(codigo_planilla);
        }

        public List<detalle_planilla_resumen_dto> ListarExcluidosByIdPlanillaBono(int codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ListarExcluidosByIdPlanillaBono(codigo_planilla);
        }

        public List<detalle_planilla_resumen_dto> ListarDetallePlanillaExcluidoByIdPlanilla(int codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ListarDetallePlanillaExcluidoByIdPlanilla(codigo_planilla);
        }
       
        //MYJ - 20171124
        public List<detalle_planilla_resumen_dto> IncluirListar(string nro_contrato, int codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.IncluirListar(nro_contrato, codigo_planilla);
        }

        //MYJ - 20171124
        public MensajeDTO IncluirProcesar(int codigo_planilla, List<detalle_planilla_inclusion_dto>lst_inclusion, string usuario)
        {
           int cantidad = 0;
           MensajeDTO v_mensaje = new MensajeDTO();
           
           using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
           {
               try
               {
                   cantidad = DetallePlanillaSelDA.Instance.IncluirProcesar(codigo_planilla, lst_inclusion, usuario);

                   v_mensaje.idRegistro = cantidad;
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

        #region BONO TRIMESTRAL

        public System.Data.DataTable ReporteBonoTrimestralLiquidacion(int v_codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ReporteBonoTrimestralLiquidacion(v_codigo_planilla);
        }

        public List<detalle_liquidacion_planilla_bono_trimestral_dto> ReporteBonoTrimestralLiquidacionLst(int v_codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ReporteBonoTrimestralLiquidacionLst(v_codigo_planilla);
        }

        public System.Data.DataTable ReporteBonoTrimestralPlanilla(int v_codigo_planilla)
        {
            return DetallePlanillaSelDA.Instance.ReporteBonoTrimestralPlanilla(v_codigo_planilla);
        }
        #endregion

    }
}
