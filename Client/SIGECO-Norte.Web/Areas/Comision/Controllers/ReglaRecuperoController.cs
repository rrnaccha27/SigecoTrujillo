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
    public class ReglaRecuperoController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly TipoPlanillaService _TipoPlanillaService;
        private readonly PersonalCanalGrupoBL _reglaCanalGrupoBL;
        private readonly ArticuloService _ArticuloService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReglaRecuperoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _TipoPlanillaService = new TipoPlanillaService();
            _reglaCanalGrupoBL = new PersonalCanalGrupoBL();
            _ArticuloService = new ArticuloService();
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
            var lista = ReglaRecuperoBL.Instance.Listar(Convert.ToInt32(estado_registro));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpGet]
        public ActionResult _Registro(int codigo_regla_recupero)
        {
            regla_recupero_dto item = new regla_recupero_dto();
            try
            {
                if (codigo_regla_recupero > 0)
                    item = ReglaRecuperoBL.Instance.Unico(codigo_regla_recupero);
                else
                    item.codigo_regla_recupero = codigo_regla_recupero;
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [RequiresAuthentication]
        [HttpPost]
        public ActionResult Guardar(regla_recupero_dto regla)
        {
            JObject jo = new JObject();
            bool esNuevo = regla.codigo_regla_recupero == -1 ? true : false;
            MensajeDTO respuesta;
            string canalesEliminar = string.Empty;

            try
            {
                regla.usuario = beanSesionUsuario.codigoUsuario;
                if (esNuevo)
                {
                    respuesta = ReglaRecuperoBL.Instance.Insertar(regla);
                }
                else 
				{
                    respuesta = ReglaRecuperoBL.Instance.Actualizar(regla);
                }

                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", string.IsNullOrWhiteSpace(respuesta.mensaje) ? "NO SE PUDO GUARDAR EL REGISTRO." : respuesta.mensaje);
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [RequiresAuthentication]
        [HttpPost]
        public ActionResult Desactivar(string codigo_regla_recupero)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (string.IsNullOrWhiteSpace(codigo_regla_recupero))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                regla_recupero_dto regla = new regla_recupero_dto();
                regla.codigo_regla_recupero = Convert.ToInt32(codigo_regla_recupero);
                regla.usuario = beanSesionUsuario.codigoUsuario;
                
                respuesta = ReglaRecuperoBL.Instance.Desactivar(regla);
                
                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al eliminar");
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
