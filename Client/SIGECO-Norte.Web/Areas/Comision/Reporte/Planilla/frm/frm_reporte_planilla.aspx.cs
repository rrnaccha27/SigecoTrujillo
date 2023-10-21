using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.Planilla.frm
{
    public partial class frm_reporte_planilla : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string p_codigo_planilla = Request.QueryString["p_codigo_planilla"];//   CodTipo="+model.CodTipoDocumento+"& pNroDocumento="+model.NroDocumento
                string p_codigo_personal = Request.QueryString["p_codigo_personal"];
                frmVerReporte(p_codigo_planilla, p_codigo_personal);

            }
        }
        private void frmVerReporte(string p_codigo_planilla, string p_codigo_personal)
        {

            try
            {

                int v_codigo_planilla = 1779;// int.Parse(p_codigo_planilla);
                int v_codigo_personal = int.Parse(p_codigo_personal);
                ParametroSistemaService _IParametroSistemaService = new ParametroSistemaService();
                var _item_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.porcentaje_detraccion);
                var _item_aplicar_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.aplicar_detraccion_superior);
                ReportParameter p_porcentaje_detraccion = new ReportParameter("p_porcentaje_detraccion", _item_detraccion.valor);
                ReportParameter p_valor_maximo_detraccion = new ReportParameter("p_valor_maximo_detraccion", _item_aplicar_detraccion.valor);
                             


                reporte_detalle_planilla v_planilla = new reporte_detalle_planilla();

                List<reporte_detalle_planilla> dt_detalle = new List<reporte_detalle_planilla>();

                dt_detalle = PlanillaSelBL.Instance.ReporteDetallePlanilla(v_codigo_planilla, v_codigo_personal);
                v_planilla = dt_detalle.FirstOrDefault();
                
                var query = dt_detalle.AsQueryable().Where( x => x.es_comision_manual > 0);
                ReportParameter p_tiene_comision_manual = new ReportParameter("p_tiene_comision_manual", (query.Count() > 0)?"*Es una comision pagada manualmente.":"");

                rpt_planilla_borradora.ZoomMode = ZoomMode.Percent;
                rpt_planilla_borradora.ZoomPercent = 140;
                rpt_planilla_borradora.ProcessingMode = ProcessingMode.Local;
                v_planilla.tipo_reporte = "_c";
                if (v_planilla!=null && v_planilla.codigo_tipo_planilla == 1)
                {
                    rpt_planilla_borradora.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Planilla/rdl/rpt_planilla_personal" + v_planilla.tipo_reporte + ".rdlc");
                }
                else
                {
                    rpt_planilla_borradora.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Planilla/rdl/rpt_planilla_supervisor" + v_planilla.tipo_reporte + ".rdlc");
                }


                rpt_planilla_borradora.LocalReport.SetParameters(new ReportParameter[] { p_porcentaje_detraccion, p_valor_maximo_detraccion, p_tiene_comision_manual });
                ReportDataSource dsDet = new ReportDataSource("dsDetallePlanilla", dt_detalle);
                rpt_planilla_borradora.LocalReport.DataSources.Clear();
                rpt_planilla_borradora.LocalReport.DataSources.Add(dsDet);
                rpt_planilla_borradora.LocalReport.Refresh();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }

        }
    }
}