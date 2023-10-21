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
using System.Configuration;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class PersonalController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly TipoDocumentoService _TipoDocumentoService;
        private readonly BancoService _BancoService;
        private readonly MonedaService _MonedaService;
        private readonly PersonalBL _personalBL;
        private readonly PersonalCanalGrupoBL _personalCanalGrupoBL;
        private readonly TipoCuentaService _TipoCuentaService;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public PersonalController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _TipoDocumentoService = new TipoDocumentoService();
            _BancoService = new BancoService();
            _personalBL = new PersonalBL();
            _personalCanalGrupoBL = new PersonalCanalGrupoBL();
            _MonedaService = new Services.MonedaService();
            _TipoCuentaService = new TipoCuentaService();
        }
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        #region LISTADOS

        public ActionResult GetTipoMonedaJson()
        {
            string result = this._MonedaService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetTipoCuentaJson()
        {
            string result = this._TipoCuentaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetTipoDocumentoJson()
        {
            string result = this._TipoDocumentoService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetBancoJson()
        {
            string result = this._BancoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetListarCanalJson()
        {
            var lista = new CanalGrupoBL().Listar_Canal();
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetListarGrupoJson(int codigo_canal)
        {
            var lista = new CanalGrupoBL().Listar_Grupo(codigo_canal);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetCanalGrupoJson(int es_canal_grupo)
        {
            var lista = new CanalGrupoBL().ListarPersonal(es_canal_grupo);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [RequiresAuthentication]
        public ActionResult GetAllJson(string codigoCanal, string codigoGrupo, string estadoPersonal, string nombre)
        {
            int sede = int.Parse(ConfigurationManager.AppSettings["sede"].ToString());
            var lista = _personalBL.Listar(Convert.ToInt32(codigoCanal), Convert.ToInt32(codigoGrupo), Convert.ToInt32(estadoPersonal), sede, nombre);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetPersonalPlanillaAllJson(personal_planilla_listado_dto v_entidad)
        {
            var lista = _personalBL.ListarByPlanilla(v_entidad);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetPersonalCanalesJson(string codigoPersonal)
        {
            var result = _personalCanalGrupoBL.Listar(Convert.ToInt32(codigoPersonal));
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult GetHistorialValidacionJson(int codigo_personal)
        {
            var lista = _personalBL.ListarHistoricoValidacion(codigo_personal);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetHistorialBloqueoJson(int codigo_personal)
        {
            var lista = _personalBL.GetHistorialBloqueoJson(codigo_personal);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        #endregion

        #region OPERACIONES

        [HttpGet]
        public ActionResult _Registro(int codigo_personal)
        {
            personal_dto item = new personal_dto();
            try
            {
                if (codigo_personal > 0)
                    item = _personalBL.GetReg(codigo_personal);
                else
                    item.codigo_personal = codigo_personal;
            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }

            return PartialView(item);
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

        [RequiresAuthentication]
        [HttpPost]
        public ActionResult Guardar(personal_dto personal, string canalesEliminados)
        {
            JObject jo = new JObject();
            bool esNuevo = personal.codigo_personal == -1 ? true : false;
            MensajeDTO respuesta;
            string canalesEliminar = string.Empty;
            string nro_documento = string.Empty;
            Boolean usarWCF = validarUsoWCF(); ;
            int sede = int.Parse(ConfigurationManager.AppSettings["sede"].ToString());
            personal.codigo_sede = sede;
            personal.nro_ruc = (String.IsNullOrEmpty(personal.nro_ruc) ? "" : personal.nro_ruc);

            try
            {
                nro_documento = personal.es_persona_juridica ? personal.nro_ruc : personal.nro_documento;

                if (personal.nro_documento != null && personal.nro_documento.Length > 0)
                {
                    if (_personalBL.ExisteDocumento(personal, true))
                    {
                        throw new Exception("Ya existe el nro documento, vuelva a ingresar otro.");
                    }
                }

                if (personal.nro_ruc != null && personal.nro_ruc.Length > 0)
                {
                    if (_personalBL.ExisteDocumento(personal, false))
                    {
                        throw new Exception("Ya existe el RUC, vuelva a ingresar otro.");
                    }
                }

                foreach (var item in personal.lista_canal_grupo)
                {
                    if (item.es_supervisor_canal || item.es_supervisor_grupo)
                    {
                        string mensajeSupervisor = string.Empty;
                        if (_personalBL.ExisteSupervisor(personal.codigo_personal, item.codigo_canal_grupo, ref mensajeSupervisor))
                        {
                            throw new Exception(mensajeSupervisor);
                        }
                    }
                }

                if (canalesEliminados.Length > 0 && !esNuevo)
                {
                    var listaCanales = canalesEliminados.Split(Convert.ToChar("|"));
                    StringBuilder xmlCanales = new StringBuilder();

                    xmlCanales.Append("<canales_grupos>");
                    for (int indice = 0; indice < listaCanales.Length; indice++)
                    {
                        xmlCanales.Append("<canal_grupo codigo_registro='" + listaCanales[indice].ToString() + "' />");
                    }
                    xmlCanales.Append("</canales_grupos>");
                    canalesEliminar = xmlCanales.ToString();
                }

                if (esNuevo)
                {
                    personal.usuario_registra = beanSesionUsuario.codigoUsuario;

                    respuesta = _personalBL.Registrar(personal, usarWCF);
                }
                else {
                    personal.fecha_modifica = DateTime.Now;
                    personal.usuario_modifica = beanSesionUsuario.codigoUsuario;

                    respuesta = _personalBL.Actualizar(personal, canalesEliminar, usarWCF);
                }

                if ((respuesta.idOperacion == 1) || (respuesta.idOperacion == -2))
                {
                    jo.Add("Msg", "Success");
                    if (esNuevo)
                    {
                        jo.Add("Equivalencia", respuesta.mensaje);
                    }
                    if (respuesta.idOperacion == -2) {
                        jo.Add("MsgWcf", "No se pudo replicar el SAP BO la información del personal.");
                    }
                }
                else if (respuesta.idOperacion == -1)
                {
                    jo.Add("Msg", "No se pudo guardar la información. " + respuesta.mensaje);
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Eliminar(string codigo)
        {
            JObject jo = new JObject();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                int codigo_personal = Convert.ToInt32(codigo);
                int respuesta = _personalBL.Eliminar(codigo_personal);
                if (respuesta > 0)
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

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Desactivar(string codigo, string esSupervisor, string codigo_equivalencia)
        {
            JObject jo = new JObject();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO.");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                Boolean usarWCF = validarUsoWCF();

                personal_dto oBe = new personal_dto();
                oBe.codigo_personal = Convert.ToInt32(codigo);
                oBe.usuario_modifica = beanSesionUsuario.codigoUsuario;

                MensajeDTO respuesta = _personalBL.Desactivar(oBe, (esSupervisor == "1" ? true : false), codigo_equivalencia, usarWCF);

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
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Activar(string codigo, string esSupervisor, string codigo_equivalencia)
        {
            JObject jo = new JObject();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO.");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                Boolean usarWCF = validarUsoWCF();

                personal_dto oBe = new personal_dto();
                oBe.codigo_personal = Convert.ToInt32(codigo);
                oBe.usuario_modifica = beanSesionUsuario.codigoUsuario;

                MensajeDTO respuesta = _personalBL.Activar(oBe, (esSupervisor == "1" ? true : false), codigo_equivalencia, usarWCF);

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
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        [HttpPost]
        [RequiresAuthentication]
        public ActionResult GetRegistro(string codigo)
        {
            //_personalBL oBL = new _personalBL();
            personal_dto oBE = new personal_dto();
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "Codigo Vacio");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            int ID;
            if (!int.TryParse(codigo, out ID))
            {
                jo.Add("Msg", "Error al parsear codigo");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                int codigo_personal = Convert.ToInt32(codigo);
                oBE = _personalBL.GetReg(codigo_personal);

            }
            catch (Exception ex)
            {
            }
            return Json(oBE, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _Reporte(int p_codigo_canal, int p_codigo_grupo, int p_estado_registro)
        {
            ReporteViewModel vm = new ReporteViewModel();
            StringBuilder sbReporte = new StringBuilder();

            string vUrl = Url.Action("frm_reporte_personal_interface", "Areas/Comision/Reporte/Personal/frm", new { area = string.Empty }) + ".aspx?p_codigo_canal=" + p_codigo_canal + "&p_codigo_grupo=" + p_codigo_grupo + "&p_estado_registro=" + p_estado_registro;

            try
            {

                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView(vm);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult GetCantidadBloqueo(int codigo_personal)
        {
            JObject jo = new JObject();

            try
            {
                int respuesta = _personalBL.GetCantidadBloqueo(codigo_personal);
                if (respuesta == 0)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Fail");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        private bool validarUsoWCF() 
        {
            Boolean retorno = false;

            try
            {
                parametro_sistema parametro = new ParametroSistemaService().GetSingle(Convert.ToInt32(Parametro.usar_wcf));
                retorno = (parametro.valor == "1" ? true : false);
            }
            catch (Exception ex)
            {
                retorno = false;
            }

            return retorno;
        }

        #endregion
    }
}
