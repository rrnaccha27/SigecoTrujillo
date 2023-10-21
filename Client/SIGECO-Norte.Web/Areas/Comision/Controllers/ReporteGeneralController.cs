using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Entidades.BeanReporte;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Reports.DataSet;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    [RequiresAuthentication]
    public class ReporteGeneralController : Controller
    {

        private readonly SIGEES.BusinessLogic.ReporteGeneralBL reporteBL = new SIGEES.BusinessLogic.ReporteGeneralBL();
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReporteGeneralController()
        {
            _TipoAccesoItemService = new TipoAccesoItemService();
        }
        public ActionResult ReportePersonal()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult ReporteCanalGrupo()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        #region PERSONAL
        public ActionResult ExportarPersonalEXCEL(string fechaInicio, string fechaFin)
        {
            List<ReportParameter> parametros = new List<ReportParameter>();
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";
            ReportDataSource dataSource = null;
            try
            {
                DateTime fecha1 = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime fecha2 = DateTime.ParseExact(fechaFin, "d/M/yyyy", CultureInfo.InvariantCulture);

                DataTable lista = new DataTable();
                lista = reporteBL.ListarPersonaPorFechaRegistra(fecha1, fecha2);

                dataSource = new ReportDataSource("Personal", lista);

                ReportParameter parametro = new ReportParameter("SubTitulo", "DE " + fechaInicio + " HASTA EL " + fechaFin);
                parametros.Add(parametro);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return Exportar(FileType, ContentType, dataSource, "PERSONAL", "~/Areas/Comision/Reporte/Personal/rdl/rpt_personal_general.rdlc", parametros);
        }

        public ActionResult ExportarPersonalPDF(string fechaInicio, string fechaFin)
        {
            DateTime fecha1 = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime fecha2 = DateTime.ParseExact(fechaFin, "d/M/yyyy", CultureInfo.InvariantCulture);

            DataTable lista = new DataTable();
            lista = reporteBL.ListarPersonaPorFechaRegistra(fecha1, fecha2);

            ReportDataSource dataSource = new ReportDataSource("Personal", lista);
            
            string FileType = "pdf";
            string ContentType = "application/pdf";

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter parametro = new ReportParameter("SubTitulo", "DE " + fechaInicio + " HASTA EL " + fechaFin);
            parametros.Add(parametro);

            return Exportar(FileType, ContentType, dataSource, "PERSONAL", "~/Areas/Comision/Reporte/Personal/rdl/rpt_personal_general.rdlc", parametros);
        }

        public ActionResult VerPersonal(string fechaInicio, string fechaFin)
        {

            DateTime fecha1 = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime fecha2 = DateTime.ParseExact(fechaFin, "d/M/yyyy", CultureInfo.InvariantCulture);

            DataTable lista = new DataTable();
            lista = reporteBL.ListarPersonaPorFechaRegistra(fecha1, fecha2);

            ReportDataSource dataSource = new ReportDataSource("Personal", lista);

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter parametro = new ReportParameter("SubTitulo", "DE " + fechaInicio + " HASTA EL " + fechaFin);
            parametros.Add(parametro);

            return Visualizar(dataSource, "PERSONAL", "~/Areas/Comision/Reporte/Personal/rdl/rpt_personal_general.rdlc", parametros);
        }

        #endregion

        #region CANAL GRUPO
        public ActionResult ExportarCanalGrupoEXCEL(string fechaInicio, string fechaFin)
        {
            List<ReportParameter> parametros = new List<ReportParameter>();
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";
            ReportDataSource dataSource = null;
            try
            {
                DateTime fecha1 = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime fecha2 = DateTime.ParseExact(fechaFin, "d/M/yyyy", CultureInfo.InvariantCulture);

                DataTable lista = new DataTable();
                lista = reporteBL.ListarCanalGrupoPorFechaRegistra(fecha1, fecha2, 1, 0);

                dataSource = new ReportDataSource("CanalGrupo", lista);

                ReportParameter parametro = new ReportParameter("SubTitulo", "DE " + fechaInicio + " HASTA EL " + fechaFin);
                parametros.Add(parametro);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return Exportar(FileType, ContentType, dataSource, "CANAL GRUPO", "~/Areas/Comision/Reporte/CanalGrupo/rdl/rpt_canal_grupo_general.rdlc", parametros);
        }

        public ActionResult ExportarCanalGrupoPDF(string fechaInicio, string fechaFin)
        {
            DateTime fecha1 = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime fecha2 = DateTime.ParseExact(fechaFin, "d/M/yyyy", CultureInfo.InvariantCulture);

            DataTable lista = new DataTable();
            lista = reporteBL.ListarCanalGrupoPorFechaRegistra(fecha1, fecha2, 1, 0);

            ReportDataSource dataSource = new ReportDataSource("CanalGrupo", lista);

            string FileType = "pdf";
            string ContentType = "application/pdf";

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter parametro = new ReportParameter("SubTitulo", "DE " + fechaInicio + " HASTA EL " + fechaFin);
            parametros.Add(parametro);

            return Exportar(FileType, ContentType, dataSource, "CANAL GRUPO", "~/Areas/Comision/Reporte/CanalGrupo/rdl/rpt_canal_grupo_general.rdlc", parametros);
        }

        public ActionResult VerCanalGrupo(string fechaInicio, string fechaFin)
        {

            DateTime fecha1 = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime fecha2 = DateTime.ParseExact(fechaFin, "d/M/yyyy", CultureInfo.InvariantCulture);

            DataTable lista = new DataTable();
            lista = reporteBL.ListarCanalGrupoPorFechaRegistra(fecha1, fecha2, 1, 0);

            ReportDataSource dataSource = new ReportDataSource("CanalGrupo", lista);

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter parametro = new ReportParameter("SubTitulo", "DE " + fechaInicio + " HASTA EL " + fechaFin);
            parametros.Add(parametro);

            return Visualizar(dataSource, "CANAL GRUPO", "~/Areas/Comision/Reporte/CanalGrupo/rdl/rpt_canal_grupo_general.rdlc", parametros);
        }

        #endregion

        #region METODO GENERAL
        private FileContentResult Exportar(string FileType, string ContentType, ReportDataSource dataSource, string nombreExportar, string rutaReporte, List<ReportParameter> parametros)
        {
            try
            {
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Server.MapPath(rutaReporte);
                rpt.SetParameters(parametros);
                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>" + FileType + "</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, string.Format(nombreExportar + ".{0}", fileNameExtension));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EX: " + ex.ToString());
                return null;
            }
            
        }

        private FileStreamResult Visualizar(ReportDataSource dataSource, string nombreExportar, string rutaReporte, List<ReportParameter> parametros)
        {
            try
            {
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Server.MapPath(rutaReporte);
                rpt.SetParameters(parametros);
                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                byte[] Bytes = rpt.Render(format: "PDF", deviceInfo: "");

                MemoryStream output = new MemoryStream();
                output.Write(Bytes, 0, Bytes.Length);
                output.Position = 0;

                HttpContext.Response.AddHeader("content-disposition", "inline; filename=" + nombreExportar + ".pdf");
                return new FileStreamResult(output, "application/pdf"); 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EX: " + ex.ToString());
                return null;
            }

        }

        #endregion
    }
}
