using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ExportadorController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }
        /*
        var url = project.ActionUrls.ExportarFallecidoPDF
                            window.location.href = url + '?fechaInicio=' + fechaInicio + '&fechaFin=' + fechaFin;
        */
        public ActionResult ExportarComisionPdf()
        {
            grilla_comision_cronograma_filtro v_entidad = new grilla_comision_cronograma_filtro();
            string FileType = "pdf";
            string ContentType = "application/pdf";
            //DataTable lst = new DataTable();
               List<grilla_comision_cronograma_dto> lst=new  List<grilla_comision_cronograma_dto>();
            try
            {
               
                
//                lst = DetalleCronogramaPagoSelBL.Instance.CronogramaPagoComisionDataTable(v_entidad);

                lst = DetalleCronogramaPagoSelBL.Instance.CronogramaPagoComisionListar(v_entidad);
                ReportDataSource dataSource = new ReportDataSource("dsComision", lst);                
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Comision/rdl/rpt_comision.rdlc");
                
                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;                
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, string.Format("Cronograma Pago Comision.{0}", FileType));


                //return Exportar(FileType, ContentType, dataSource, "Fallecidos", "~/Reports/RFallecido.rdlc", parametros);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return null;

            
        }
    }
}
