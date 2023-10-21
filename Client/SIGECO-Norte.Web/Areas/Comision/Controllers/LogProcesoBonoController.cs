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

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class LogProcesoBonoController : Controller
    {
        //
        // GET: /Comision/LogProcesoBono/

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
		
        #region Inicializacion de Controller - Menu
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public LogProcesoBonoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
        }
        #endregion

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        [HttpPost]
		public ActionResult GetAllJson(string fecha_inicio, string fecha_fin)
        {
            var lista = LogProcesoBonoBL.Instance.Listar(fecha_inicio, fecha_fin);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
        public ActionResult GetDetalleJson(int codigo_planilla)
        {
            var lista = LogProcesoBonoBL.Instance.Detalle(codigo_planilla);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
		public ActionResult GetFechas()
        {
            var lista = LogProcesoBonoBL.Instance.Fechas();
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpGet]
        public ActionResult _Detalle(int codigo_planilla)
        {
            Planilla_bono_dto item = new Planilla_bono_dto();
            try
            {
                item = PlanillaSelBL.Instance.BuscarPlanillaBonoById(codigo_planilla);
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

    }
}
