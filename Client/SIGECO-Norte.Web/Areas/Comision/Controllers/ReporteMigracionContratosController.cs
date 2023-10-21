using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using SIGEES.BusinessLogic;
using SIGEES.Entidades;

using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.Areas.Comision.Utils;

using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.MemberShip.Filters;

using Microsoft.Reporting.WebForms;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ReporteMigracionContratosController : Controller
    {
        //
        // GET: /Comision/LogContratoSAP/

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly CanalGrupoService _canalService;
        private readonly TipoPlanillaService _tipoPlanillaService;

        // private canal_grupo _canal_grupo = null;

        #region Inicializacion de Controller - Menu
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReporteMigracionContratosController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
            _tipoPlanillaService = new TipoPlanillaService();
        }
        #endregion

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        [RequiresAuthentication]
        public ActionResult GetAllJson(reporte_migracion_contratos_busqueda_dto busqueda)
        {
            var lista = ReporteGeneralBL.Instance.MigracionContratos(busqueda);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [RequiresAuthentication]
        public ActionResult SetDataGrilla(List<reporte_migracion_contratos_dto> v_entidad)
        {
            Guid id = Guid.NewGuid();
            string v_guid = id.ToString().Replace('-', '_');
            Session[v_guid] = v_entidad;
            return Json(new { v_guid = v_guid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarExcel(string id)
        {
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";

            List<reporte_migracion_contratos_dto> lst = new List<reporte_migracion_contratos_dto>();
            try
            {
                lst = Session[id] as List<reporte_migracion_contratos_dto>;
                reporte_migracion_contratos_dto detalle = lst.FirstOrDefault();

                ReportDataSource dataSource = new ReportDataSource("dsReporteMigracionContratos", lst);
                LocalReport rpt = new LocalReport
                {
                    ReportPath = Server.MapPath("~/Areas/Comision/Reporte/ReporteMigracionContratos/rdl/rpt_reporte_migracion_contratos.rdlc")
                };

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, "ReporteMigracionContratos.xls");
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            finally
            {
                Session.Remove(id);
            }
            return null;
        }

    }
}
