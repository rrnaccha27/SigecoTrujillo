using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Core;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Controllers
{
    [RequiresAuthentication]
    public class MenuController : Controller
    {

        private readonly IMenuService _service;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly IEventoUsuarioService _EventoUsuarioService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public MenuController()
        {
            _service = new MenuService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _EventoUsuarioService = new EventoUsuarioService();
        }

        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult LoadTreeNodeDDL()
        {
            var items = this._service.GetTreeJson();
            return Content(items, "application/json");
        }

        public ActionResult GetTreeNodeJSON()
        {
            string result = this._service.GetAllJson(true);
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
                string jsonContent = this._service.GetSingleJSON(int.Parse(id));
                return Content(jsonContent, "application/json");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
        }


        [HttpPost]
        public ActionResult Registrar(string codigoMenu, string nombre, string orden, string ruta)
        {
            bool estadoEvento = false;
            string codigo = "NULL";

            JObject jo = new JObject();

            menu menu = new menu();

            try
            {
                var allNodes = this._service.GetNodes(false);
                bool esRoot = false;

                if (allNodes.Count() == 0)
                {
                    esRoot = true;
                }

                if (!esRoot)
                {
                    if (string.IsNullOrWhiteSpace(codigoMenu))
                    {
                        jo.Add("Msg", "POR FAVOR SELECCIONE UN MENU CABECERA");
                        return Content(JsonConvert.SerializeObject(jo), "application/json");
                    }
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
                else if (string.IsNullOrWhiteSpace(orden))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NRO ORDEN");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                if (!esRoot)
                {
                    menu.codigo_menu_padre = int.Parse(codigoMenu);
                }
                menu.nombre_menu = nombre;
                menu.estado_registro = true;
                menu.orden = int.Parse(orden);
                menu.ruta_menu = ruta;
                menu.fecha_registra = DateTime.Now;
                menu.usuario_registra = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.Create(menu);
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

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("nombre_menu", menu.nombre_menu));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_menu_padre", menu.codigo_menu_padre != null ? menu.codigo_menu_padre.ToString() : "NULL"));
                    listaBeanAtributo.Add(new BeanAtributo("orden", menu.orden.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("ruta_menu", menu.ruta_menu != null ? menu.ruta_menu : "NULL"));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", menu.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(menu.fecha_registra)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_registra", menu.usuario_registra));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "menu", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL REGISTRAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "menu", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 1, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(string codigoMenu, string codigoMenuCabecera, string nombre, string orden, string ruta, bool estadoRegistro)
        {
            bool estadoEvento = false;
            menu registro = null;

            JObject jo = new JObject();

            try
            {
                if (string.IsNullOrWhiteSpace(codigoMenu))
                {
                    jo.Add("Msg", "CODIGO REGISTRO NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (codigoMenu == codigoMenuCabecera)
                {
                    jo.Add("Msg", "MENU CABECERA NO PUEDE SER EL MISMO REGISTRO A MODIFICAR");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                bool esRoot = this._service.EsRoot(int.Parse(codigoMenu));

                if (!esRoot)
                {
                    if (string.IsNullOrWhiteSpace(codigoMenuCabecera))
                    {
                        jo.Add("Msg", "POR FAVOR SELECCIONE UN MENU CABECERA");
                        return Content(JsonConvert.SerializeObject(jo), "application/json");
                    }
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
                else if (string.IsNullOrWhiteSpace(orden))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NRO ORDEN");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                registro = this._service.GetSingle(int.Parse(codigoMenu));
                if (!esRoot)
                {
                    registro.codigo_menu_padre = int.Parse(codigoMenuCabecera);
                }
                registro.nombre_menu = nombre;
                registro.estado_registro = estadoRegistro;
                registro.orden = int.Parse(orden);
                registro.ruta_menu = ruta;
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
                    jo.Add("Msg", "Error al Modificar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("nombre_menu", registro.nombre_menu));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_menu_padre", registro.codigo_menu_padre != null ? registro.codigo_menu_padre.ToString() : "NULL"));
                    listaBeanAtributo.Add(new BeanAtributo("orden", registro.orden.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("ruta_menu", registro.ruta_menu != null ? registro.ruta_menu : "NULL"));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigoMenu, "menu", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigoMenu, "menu", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 2, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Eliminar(string codigoMenu)
        {
            bool estadoEvento = false;
            menu registro = null;

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigoMenu))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                registro = this._service.GetSingle(int.Parse(codigoMenu));

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
                    jo.Add("Msg", "Error al desactivar registro");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigoMenu, "menu", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL DESACTIVAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigoMenu, "menu", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 3, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
    }
}
