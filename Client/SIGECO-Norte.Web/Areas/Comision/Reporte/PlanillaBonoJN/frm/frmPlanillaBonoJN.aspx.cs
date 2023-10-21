using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using SIGEES.Entidades;

namespace SIGEES.Web.Areas.Comision.Reporte.PlanillaBonoJN.frm
{
    public partial class frmPlanillaBonoJN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string p_codigo_planilla = Request.QueryString["p_codigo_planilla"];
                frmVerReporte(p_codigo_planilla);
            }

        }

        private void frmVerReporte(string p_codigo_planilla)
        {
            try
            {
                ParametroSistemaService _IParametroSistemaService = new ParametroSistemaService();
                int v_codigo_planilla = int.Parse(p_codigo_planilla);

                DataTable dt_detalle = PlanillaSelBL.Instance.ReportePlanillaBonoJNDetalle(v_codigo_planilla);
                DataTable dt_resumen = PlanillaSelBL.Instance.ReportePlanillaBonoJNResumen(v_codigo_planilla);
                DataTable dt_titulos = PlanillaSelBL.Instance.ReportePlanillaBonoJNResumenTitulos(v_codigo_planilla);
                DataTable dt_planilla = PlanillaSelBL.Instance.ReportePlanillaBonoJN(v_codigo_planilla);

                //ReportParameter p_fecha_inicio = new ReportParameter("p_fecha_inicio", planilla.fecha_inicio);
                //ReportParameter p_fecha_fin = new ReportParameter("p_fecha_fin", planilla.fecha_fin);
                //ReportParameter p_codigo_estado_planilla = new ReportParameter("p_codigo_estado_planilla", planilla.codigo_estado_planilla.ToString());
                //ReportParameter p_numero_planilla = new ReportParameter("p_numero_planilla", planilla.numero_planilla);
                //ReportParameter demo = new ReportParameter();

                rptPlanillaBonoJN.ZoomMode = ZoomMode.Percent;
                rptPlanillaBonoJN.ZoomPercent = 120;
                rptPlanillaBonoJN.ProcessingMode = ProcessingMode.Local;
                rptPlanillaBonoJN.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBonoJN/rdl/planilla_bono_jn.rdlc");

                ReportDataSource dsDet = new ReportDataSource("dsDetalle", dt_detalle);
                ReportDataSource dsRes = new ReportDataSource("dsResumen", dt_resumen);
                ReportDataSource dsTit = new ReportDataSource("dsTitulos", dt_titulos);
                ReportDataSource dsPla = new ReportDataSource("dsPlanilla", dt_planilla);

                rptPlanillaBonoJN.LocalReport.DataSources.Clear();
                rptPlanillaBonoJN.LocalReport.DataSources.Add(dsDet);
                rptPlanillaBonoJN.LocalReport.DataSources.Add(dsRes);
                rptPlanillaBonoJN.LocalReport.DataSources.Add(dsTit);
                rptPlanillaBonoJN.LocalReport.DataSources.Add(dsPla);
                //rptPlanillaBonoJN.LocalReport.SetParameters(new ReportParameter[] { p_fecha_inicio, p_fecha_fin, p_codigo_estado_planilla, p_numero_planilla });

                rptPlanillaBonoJN.LocalReport.Refresh();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
            finally
            {

            }

        }
 
    }
}