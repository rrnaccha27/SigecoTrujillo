using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.Personal.frm
{
    public partial class frm_reporte_personal_interface : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                string p_codigo_canal_canal = Request.QueryString["p_codigo_canal"];

                string p_codigo_grupo = Request.QueryString["p_codigo_grupo"];

                string p_estado_registro = Request.QueryString["p_estado_registro"];
                
                frmVerReporte(p_codigo_canal_canal, p_codigo_grupo, p_estado_registro);

            }
        }
        private void frmVerReporte(string p_codigo_planilla, string p_codigo_personal, string p_estado_registro)
        {
            try
            {
                
                List<ReportParameter> parametros = new List<ReportParameter>();
                //ReportParameter parametro = new ReportParameter("nombreParametro", "valorParametro");
                //parametros.Add(parametro);
                
                DataTable dt_detalle = new DataTable();
                dt_detalle = ReporteGeneralBL.Instance.ListarPersonaPorParametrosInterface(Convert.ToInt32(p_codigo_planilla), Convert.ToInt32(p_codigo_personal), Convert.ToInt32(p_estado_registro));

                rpt_personal_interface.ZoomMode = ZoomMode.Percent;
                rpt_personal_interface.ZoomPercent = 100;
                rpt_personal_interface.ProcessingMode = ProcessingMode.Local;
                rpt_personal_interface.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Personal/rdl/rpt_personal_interface.rdlc");
                rpt_personal_interface.LocalReport.SetParameters(parametros);
                
                ReportDataSource dsDet = new ReportDataSource("Personal", dt_detalle);

                rpt_personal_interface.LocalReport.DataSources.Clear();
                rpt_personal_interface.LocalReport.DataSources.Add(dsDet);

                rpt_personal_interface.LocalReport.Refresh();
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