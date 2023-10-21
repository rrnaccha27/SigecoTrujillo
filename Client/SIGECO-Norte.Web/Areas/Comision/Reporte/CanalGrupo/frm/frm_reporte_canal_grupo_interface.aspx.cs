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

namespace SIGEES.Web.Areas.Comision.Reporte.CanalGrupo.frm
{
    public partial class frm_reporte_canal_grupo_interface : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string p_es_canal_grupo = Request.QueryString["p_es_canal_grupo"];

                string p_codigo_padre = Request.QueryString["p_codigo_padre"];

                frmVerReporte(p_es_canal_grupo, p_codigo_padre);

            }
        }
        private void frmVerReporte(string p_es_canal_grupo, string p_codigo_padre)
        {
            try
            {
                
                List<ReportParameter> parametros = new List<ReportParameter>();
                //ReportParameter parametro = new ReportParameter("nombreParametro", "valorParametro");
                //parametros.Add(parametro);
                
                DataTable dt_detalle = new DataTable();
                dt_detalle = ReporteGeneralBL.Instance.ListarCanalGrupoInterface(Convert.ToInt32(p_es_canal_grupo), Convert.ToInt32(p_codigo_padre));

                rpt_canal_grupo_interface.ZoomMode = ZoomMode.Percent;
                rpt_canal_grupo_interface.ZoomPercent = 100;
                rpt_canal_grupo_interface.ProcessingMode = ProcessingMode.Local;
                rpt_canal_grupo_interface.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/CanalGrupo/rdl/rpt_canal_grupo_interface.rdlc");
                rpt_canal_grupo_interface.LocalReport.SetParameters(parametros);
                
                ReportDataSource dsDet = new ReportDataSource("CanalGrupo", dt_detalle);

                rpt_canal_grupo_interface.LocalReport.DataSources.Clear();
                rpt_canal_grupo_interface.LocalReport.DataSources.Add(dsDet);

                rpt_canal_grupo_interface.LocalReport.Refresh();
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