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
    public partial class frmLiquidacionJN : System.Web.UI.Page
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

                DataTable dt_detalle = PlanillaSelBL.Instance.ReportePlanillaBonoJNLiquidacion(v_codigo_planilla);

                rptLiquidacionJN.ZoomMode = ZoomMode.Percent;
                rptLiquidacionJN.ZoomPercent = 120;
                rptLiquidacionJN.ProcessingMode = ProcessingMode.Local;
                rptLiquidacionJN.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBonoJN/rdl/liquidacion_jn.rdlc");

                ReportDataSource dsDet = new ReportDataSource("dsLiquidacion", dt_detalle);

                rptLiquidacionJN.LocalReport.DataSources.Clear();
                rptLiquidacionJN.LocalReport.DataSources.Add(dsDet);

                rptLiquidacionJN.LocalReport.Refresh();
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