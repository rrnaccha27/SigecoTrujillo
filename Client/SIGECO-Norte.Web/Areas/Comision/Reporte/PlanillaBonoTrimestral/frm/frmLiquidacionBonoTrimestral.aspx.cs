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

namespace SIGEES.Web.Areas.Comision.Reporte.PlanillaBonoTrimestral.frm
{
    public partial class frmLiquidacionBonoTrimestral : System.Web.UI.Page
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

                DataTable dt_detalle = DetallePlanillaSelBL.Instance.ReporteBonoTrimestralLiquidacion(v_codigo_planilla);

                //ReportParameter p_fecha_inicio = new ReportParameter("p_fecha_inicio", planilla.fecha_inicio);
                //ReportParameter p_fecha_fin = new ReportParameter("p_fecha_fin", planilla.fecha_fin);
                //ReportParameter p_codigo_estado_planilla = new ReportParameter("p_codigo_estado_planilla", planilla.codigo_estado_planilla.ToString());
                //ReportParameter p_numero_planilla = new ReportParameter("p_numero_planilla", planilla.numero_planilla);
                //ReportParameter demo = new ReportParameter();

                rptLiquidacionBonoTrimestral.ZoomMode = ZoomMode.Percent;
                rptLiquidacionBonoTrimestral.ZoomPercent = 120;
                rptLiquidacionBonoTrimestral.ProcessingMode = ProcessingMode.Local;
                rptLiquidacionBonoTrimestral.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBonoTrimestral/rdlc/rpt_liquidacion.rdlc");

                ReportDataSource dsDet = new ReportDataSource("dsLiquidacion", dt_detalle);

                rptLiquidacionBonoTrimestral.LocalReport.DataSources.Clear();
                rptLiquidacionBonoTrimestral.LocalReport.DataSources.Add(dsDet);
                //rptLiquidacionBonoTrimestral.LocalReport.SetParameters(new ReportParameter[] { p_fecha_inicio, p_fecha_fin, p_codigo_estado_planilla, p_numero_planilla });

                rptLiquidacionBonoTrimestral.LocalReport.Refresh();
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