using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Core;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Areas.Comision.Entity;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEES.Web.Areas.Comision.Services;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    [RequiresAuthentication]
    public class TipoCuentaController : Controller
    {
        //
        private readonly TipoCuentaService _service;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly IEventoUsuarioService _EventoUsuarioService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public TipoCuentaController()
        {
            _service = new TipoCuentaService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _EventoUsuarioService = new EventoUsuarioService();
        }

        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult GetRegistrosJSON()
        {
            string result = this._service.GetAllJson(false);
            return Content(result, "application/json");
        }

        [HttpPost]
        public ActionResult GetRegistro(string id)
        {

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo));
            }

            try
            {
                string jsonContent = this._service.GetSingleJson(int.Parse(id));
                return Content(jsonContent, "application/json");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
        }

        [HttpPost]
        public ActionResult Registrar(string nombre, string simbolo)
        {
            bool estadoEvento = false;
            string codigo = "NULL";

            JObject jo = new JObject();

            tipo_cuenta tipo = new tipo_cuenta();

            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombre.Length > 50)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 50");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (simbolo.Length > 1)
                {
                    jo.Add("Msg", "SIMBOLO, NUMERO MAXIMO DE CARACTERES 1");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                tipo.nombre = nombre;
                tipo.simbolo = simbolo;
                tipo.estado_registro = true;
                tipo.fecha_registra = DateTime.Now;
                tipo.usuario_registra = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.Create(tipo);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    codigo = respuesta.IdRegistro;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al registrar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            //try
            //{
            //    List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
            //    List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
            //    if (estadoEvento)
            //    {
            //        listaBeanAtributo.Add(new BeanAtributo("nombre", tipo.nombre));
            //        listaBeanAtributo.Add(new BeanAtributo("simbolo", tipo.simbolo));
            //        listaBeanAtributo.Add(new BeanAtributo("estado_registro", tipo.estado_registro.ToString()));
            //        listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(tipo.fecha_registra)));
            //        listaBeanAtributo.Add(new BeanAtributo("usuario_registra", tipo.usuario_registra));
            //        listaBeanEntidad.Add(new BeanEntidad(codigo, "moneda", listaBeanAtributo));
            //    }
            //    else
            //    {
            //        listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL REGISTRAR"));
            //        listaBeanEntidad.Add(new BeanEntidad(codigo, "moneda", listaBeanAtributo));
            //    }

            //    _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 1, estadoEvento, listaBeanEntidad);

            //}
            //catch (Exception ex)
            //{

            //}
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(string codigo, string nombre, string simbolo)
        {
            bool estadoEvento = false;
            tipo_cuenta registro = null;

            JObject jo = new JObject();

            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    jo.Add("Msg", "CODIGO REGISTRO NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(nombre))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombre.Length > 50)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 50");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (simbolo.Length > 1)
                {
                    jo.Add("Msg", "SIMBOLO, NUMERO MAXIMO DE CARACTERES 1");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                registro = this._service.GetSingle(int.Parse(codigo));
                registro.nombre = nombre;
                registro.simbolo = simbolo;
                registro.estado_registro = true;
                registro.fecha_modifica = DateTime.Now;
                registro.usuario_modifica = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.Update(registro);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al modificar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            //try
            //{
            //    List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
            //    List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
            //    if (estadoEvento)
            //    {
            //        listaBeanAtributo.Add(new BeanAtributo("nombre", registro.nombre));
            //        listaBeanAtributo.Add(new BeanAtributo("simbolo", registro.simbolo));
            //        listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
            //        listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
            //        listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
            //        listaBeanEntidad.Add(new BeanEntidad(codigo, "moneda", listaBeanAtributo));
            //    }
            //    else
            //    {
            //        listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
            //        listaBeanEntidad.Add(new BeanEntidad(codigo, "moneda", listaBeanAtributo));
            //    }

            //    _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 2, estadoEvento, listaBeanEntidad);

            //}
            //catch (Exception ex)
            //{

            //}
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        [HttpPost]
        public ActionResult Eliminar(string codigo)
        {
            bool estadoEvento = false;
            tipo_cuenta registro = null;

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                registro = this._service.GetSingle(int.Parse(codigo));
                registro.estado_registro = false;
                registro.fecha_modifica = DateTime.Now;
                registro.usuario_modifica = beanSesionUsuario.codigoUsuario;
                IResult respuesta = this._service.Update(registro);
                if (respuesta.Success)
                {
                    estadoEvento = true;
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

            //try
            //{
            //    List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
            //    List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
            //    if (estadoEvento)
            //    {
            //        listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
            //        listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
            //        listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
            //        listaBeanEntidad.Add(new BeanEntidad(codigo, "moneda", listaBeanAtributo));
            //    }
            //    else
            //    {
            //        listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL ELIMINAR"));
            //        listaBeanEntidad.Add(new BeanEntidad(codigo, "moneda", listaBeanAtributo));
            //    }

            //    _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 3, estadoEvento, listaBeanEntidad);

            //}
            //catch (Exception ex)
            //{

            //}
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
    }
}
