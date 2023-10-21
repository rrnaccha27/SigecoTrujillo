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

namespace SIGEES.Web.Areas.Comision.Reporte.PlanillaComercial.frm
{
    public partial class frm_reporte_planilla_comercial : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                string p_codigo_planilla = Request.QueryString["p_codigo_planilla"];//   CodTipo="+model.CodTipoDocumento+"& pNroDocumento="+model.NroDocumento



                frmVerReporte(p_codigo_planilla);

            }
        }
        private void frmVerReporte(string p_codigo_planilla)
        {
            try
            {
                
                DataTable dt_General = new DataTable();
                
                dt_General = PlanillaSelBL.Instance.ReporteDetallePlanillaComercial(int.Parse(p_codigo_planilla));


                // Fit report to screen
               // rpt_planilla.ZoomMode = ZoomMode.FullPage;
                rpt_planilla.ZoomMode = ZoomMode.Percent;                
                rpt_planilla.ZoomPercent = 100;
                
                rpt_planilla.ProcessingMode = ProcessingMode.Local;
                rpt_planilla.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaComercial/rpt/rpt_reporte_planilla_comercial.rdlc");
                ReportDataSource dsDet = new ReportDataSource("dsDetallePlanillaComercial", dt_General);
                rpt_planilla.LocalReport.DataSources.Clear();
                rpt_planilla.LocalReport.DataSources.Add(dsDet);
                rpt_planilla.SizeToReportContent = true; 
                rpt_planilla.LocalReport.Refresh();

                /**/
                /*
                byte[] bytes = null;
                //string strDeviceInfo = "";
                string strDeviceInfo =
          "<DeviceInfo>" +
          "  <OutputFormat>EMF</OutputFormat>" +
          "  <PageWidth>15.5in</PageWidth>" +
          "  <PageHeight>11in</PageHeight>" +
          "  <MarginTop>0.25in</MarginTop>" +
          "  <MarginLeft>0.25in</MarginLeft>" +
          "  <MarginRight>0.25in</MarginRight>" +
          "  <MarginBottom>0.25in</MarginBottom>" +
          "</DeviceInfo>";
               

                string strMimeType = "";
                string strEncoding = "";
                string strExtension = "";
                string[] strStreams = null;
                Warning[] warnings = null;
                bytes = rpt_planilla.LocalReport.Render("pdf", strDeviceInfo, out strMimeType, out  strEncoding, out strExtension, out strStreams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = strMimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + "planilla_001" + "." + "pdf");
                Response.BinaryWrite(bytes); // create the file
                Response.Flush();
                */
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