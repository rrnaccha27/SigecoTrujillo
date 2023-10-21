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

using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.MemberShip.Filters;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class PersonalNoValidadoController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly PersonalBL _personalBL;
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        
        public PersonalNoValidadoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _personalBL = new PersonalBL();
        }

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        [RequiresAuthentication]
        public ActionResult GetAllJson()
        {
            var lista = _personalBL.ListarNoValidados();
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpGet]
        public ActionResult _Detalle(int codigo_personal)
        {
            personal_dto item = new personal_dto();
            try
            {
                item = _personalBL.GetReg(codigo_personal);
            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Validar(List<personal_no_validado_dto> lst_personal)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            string usuario = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                usuario = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = _personalBL.Validar(lst_personal, usuario);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

    }
}
