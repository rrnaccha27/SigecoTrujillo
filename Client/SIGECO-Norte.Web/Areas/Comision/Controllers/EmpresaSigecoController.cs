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
    public class EmpresaSigecoController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;


        private readonly MonedaService _MonedaService;
        private readonly EmpresaSIGECOService _EmpresaSIGECOService;


        private readonly TipoCuentaService _TipoCuentaService;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public EmpresaSigecoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _MonedaService = new Services.MonedaService();
            _TipoCuentaService = new TipoCuentaService();
            _EmpresaSIGECOService = new EmpresaSIGECOService();
        }
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

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

        public ActionResult GetRegistrosJSON()
        {
            string result = this._EmpresaSIGECOService.GetAllJson(false);
            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult _Registro(int codigo_empresa)
        {
            empresa_sigeco_dto item = new empresa_sigeco_dto();
            //try
            //{
            //    if (codigo_personal > 0)
            //        item = _personalBL.GetReg(codigo_personal);
            //    else
            //        item.codigo_personal = codigo_personal;
            //}
            //catch (Exception ex)
            //{

            //    string v_mensaje = ex.Message;
            //}

            return PartialView(item);
        }

        [HttpPost]
        public ActionResult Guardar(personal_dto personal, string canalesEliminados)
        {
            JObject jo = new JObject();
            //bool esNuevo = personal.codigo_personal == -1 ? true : false;
            //MensajeDTO respuesta;
            //string canalesEliminar = string.Empty;
            //string nro_documento = string.Empty;

            //try
            //{
            //    nro_documento = personal.es_persona_juridica? personal.nro_ruc: personal.nro_documento;

            //    if (personal.nro_documento != null && personal.nro_documento.Length > 0)
            //    { 
            //        if (_personalBL.ExisteDocumento(personal, true))
            //        {
            //            throw new Exception("Ya existe el nro documento, vuelva a ingresar otro.");
            //        }
            //    }

            //    if (personal.nro_ruc != null && personal.nro_ruc.Length > 0)
            //    {
            //        if (_personalBL.ExisteDocumento(personal, false))
            //        {
            //            throw new Exception("Ya existe el RUC, vuelva a ingresar otro.");
            //        }
            //    }

            //    foreach (var item in personal.lista_canal_grupo)
            //    {
            //        if (item.es_supervisor_canal || item.es_supervisor_grupo)
            //        {
            //            string mensajeSupervisor = string.Empty;
            //            if (_personalBL.ExisteSupervisor(personal.codigo_personal, item.codigo_canal_grupo, ref mensajeSupervisor))
            //            {
            //                throw new Exception(mensajeSupervisor);
            //            }
            //        }
            //    }


            //    if (canalesEliminados.Length > 0 && !esNuevo)
            //    {
            //        var listaCanales = canalesEliminados.Split(Convert.ToChar("|"));
            //        StringBuilder xmlCanales = new StringBuilder();

            //        xmlCanales.Append("<canales_grupos>");
            //        for (int indice = 0; indice < listaCanales.Length; indice++)
            //        {
            //            xmlCanales.Append("<canal_grupo codigo_registro='" + listaCanales[indice].ToString() + "' />");
            //        }
            //        xmlCanales.Append("</canales_grupos>");
            //        canalesEliminar = xmlCanales.ToString();
            //    }


            //    if (esNuevo)
            //    {
            //        personal.usuario_registra = beanSesionUsuario.codigoUsuario;

            //        respuesta = _personalBL.Registrar(personal);
            //    }
            //    else {
            //        personal.fecha_modifica = DateTime.Now;
            //        personal.usuario_modifica = beanSesionUsuario.codigoUsuario;

            //        respuesta = _personalBL.Actualizar(personal, canalesEliminar);
            //    }

            //    if (respuesta.idOperacion == 1)
            //    {
            //        jo.Add("Msg", "Success");
            //        if (esNuevo)
            //        { 
            //            jo.Add("Equivalencia", respuesta.mensaje);
            //        }
            //    }
            //    else
            //    {
            //        jo.Add("Msg", "NO SE PUDO GUARDAR EL REGISTRO.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    jo.Add("Msg", ex.Message);
            //}

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }



        [HttpPost]
        public ActionResult Desactivar(string codigo)
        {
            JObject jo = new JObject();
            //if (string.IsNullOrWhiteSpace(codigo))
            //{
            //    jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
            //    return Content(JsonConvert.SerializeObject(jo), "application/json");
            //}
            //try
            //{
            //    personal_dto oBe = new personal_dto();
            //    oBe.codigo_personal = Convert.ToInt32(codigo);
            //    //oBe.estado_registro = false;
            //    //oBe.fecha_modifica = DateTime.Now;
            //    oBe.usuario_modifica = beanSesionUsuario.codigoUsuario;
            //    int respuesta = _personalBL.Desactivar(oBe);
            //    if (respuesta > 0)
            //    {
            //        jo.Add("Msg", "Success");
            //    }
            //    else
            //    {
            //        jo.Add("Msg", "Error al desactivar");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    jo.Add("Msg", ex.Message);
            //}
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult GetRegistro(string codigo)
        {
            //_personalBL oBL = new _personalBL();
            personal_dto oBE = new personal_dto();
            JObject jo = new JObject();

            //if (string.IsNullOrWhiteSpace(codigo))
            //{
            //    jo.Add("Msg", "Codigo Vacio");
            //    return Content(JsonConvert.SerializeObject(jo), "application/json");
            //}

            //int ID;
            //if (!int.TryParse(codigo, out ID))
            //{
            //    jo.Add("Msg", "Error al parsear codigo");
            //    return Content(JsonConvert.SerializeObject(jo), "application/json");
            //}

            //try
            //{
            //    int codigo_personal = Convert.ToInt32(codigo);
            //    oBE = _personalBL.GetReg(codigo_personal);

            //}
            //catch (Exception ex)
            //{
            //}
            return Json(oBE, JsonRequestBehavior.AllowGet);
        }


    }
}
