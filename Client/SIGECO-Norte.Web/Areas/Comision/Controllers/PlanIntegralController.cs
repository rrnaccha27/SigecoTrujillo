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
    public class PlanIntegralController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly CampoSantoSigecoService _camposantoService;
        private readonly TipoArticuloService _tipoarticuloService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        
		public PlanIntegralController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _camposantoService = new CampoSantoSigecoService();
            _tipoarticuloService = new TipoArticuloService();
        }
        
		[RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }
        
        public ActionResult GetAllJson(string estado_registro)
        {
            var lista = PlanIntegralBL.Instance.Listar(Convert.ToInt32(estado_registro));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetDetalleJson(string codigo_plan_integral)
        {
            var lista = PlanIntegralBL.Instance.DetalleListar(Convert.ToInt32(codigo_plan_integral));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetCamposantoJson()
        {
            string result = this._camposantoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetTipoArticuloJson()
        {
            string result = this._tipoarticuloService.GetAllComboJson();
            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult _Registro(int codigo_plan_integral)
        {
            plan_integral_dto item = new plan_integral_dto();
            try
            {
                if (codigo_plan_integral > 0)
                    item = PlanIntegralBL.Instance.Unico(codigo_plan_integral);
                else
                    item.codigo_plan_integral = codigo_plan_integral;
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpPost]
        public ActionResult Guardar(plan_integral_dto plan)
        {
            JObject jo = new JObject();
            bool esNuevo = plan.codigo_plan_integral == -1 ? true : false;
            MensajeDTO respuesta;
            string canalesEliminar = string.Empty;

            try
            {
                plan.usuario = beanSesionUsuario.codigoUsuario;
                if (esNuevo)
                {
                    respuesta = PlanIntegralBL.Instance.Insertar(plan);
                }
                else
                {
                    respuesta = PlanIntegralBL.Instance.Actualizar(plan);
                }

                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", respuesta.mensaje);
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Desactivar(string codigo_plan_integral)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (string.IsNullOrWhiteSpace(codigo_plan_integral))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                plan_integral_dto plan = new plan_integral_dto();
                plan.codigo_plan_integral = Convert.ToInt32(codigo_plan_integral);
                plan.usuario = beanSesionUsuario.codigoUsuario;
                
                respuesta = PlanIntegralBL.Instance.Desactivar(plan);
                
                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al desactivar");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

    }
}
