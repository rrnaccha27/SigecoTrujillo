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
    public class ComisionPersonalInactivoController : Controller
    {
        //
        // GET: /Comision/LogContratoSAP/

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly CanalGrupoService _canalService;

        // private canal_grupo _canal_grupo = null;

        #region Inicializacion de Controller - Menu
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ComisionPersonalInactivoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
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
        [HttpPost]
        public ActionResult GetAllJson(string fecha_inicio, string fecha_fin, string codigo_canal, string liquidado)
        {
            detalle_cronograma_personal_inactivo_busqueda_dto busqueda = new detalle_cronograma_personal_inactivo_busqueda_dto
            {
                fecha_inicio = fecha_inicio,
                fecha_fin = fecha_fin,
                codigo_canal = codigo_canal,
                liquidado = Convert.ToInt32(liquidado)
            };

            var lista = DetalleCronogramaPagoSelBL.Instance.ListadoComisionPersonalInactivo(busqueda);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpGet]
        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }


        [HttpGet]
        public ActionResult GetLiquidadoJson()
        {
            string result = Listados.GetLiquidadoJson(true);
            return Content(result, "application/json");
        }


        public ActionResult GetFechasJson()
        {
            var lista = DetalleCronogramaPagoSelBL.Instance.FechasComisionPersonalInactivo();
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
        public ActionResult SetExportData(List<detalle_cronograma_personal_inactivo_dto> listado)
        {
            Guid id = Guid.NewGuid();

            string v_guid = id.ToString().Replace('-', '_');
            Session[v_guid] = listado;

            return Json(new { v_guid = v_guid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarExcel(string id)
        {
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";

            List<detalle_cronograma_personal_inactivo_dto> lst = new List<detalle_cronograma_personal_inactivo_dto>();
            try
            {
                lst = Session[id] as List<detalle_cronograma_personal_inactivo_dto>;

                ReportDataSource dataSource = new ReportDataSource("dsComision", lst);
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/ComisionPersonalInactivo/rdl/rpt_comisionpersonalinactivo.rdlc");

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, "ComisionPersonalInactivo.xls");
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

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Aprobar(List<detalle_cronograma_personal_inactivo_dto> lst_detalle, int nivel, int codigo_resultado, string observacion)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            string usuario = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                usuario = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.Aprobar(lst_detalle, nivel, codigo_resultado, usuario, observacion);

                if (mensaje.idOperacion != 1)
                {
                    vResultado = -1;
                    vMensaje = mensaje.mensaje;
                }
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        #region VISTAS
        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Detalle(int codigo_detalle)
        {
            detalle_cronograma_aprobacion_dto item = new detalle_cronograma_aprobacion_dto();
            try
            {
                item = DetalleCronogramaPagoSelBL.Instance.ComisionPersonalInactivo(codigo_detalle);
            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }
        #endregion

    }
}
