﻿using Newtonsoft.Json;
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
    public class PerfilUsuarioController : Controller
    {

        private readonly IPerfilUsuarioService _service;
        private readonly IMenuService _Menuservice;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly IEventoUsuarioService _EventoUsuarioService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public PerfilUsuarioController()
        {
            _service = new PerfilUsuarioService();
            _Menuservice = new MenuService();
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
        public ActionResult Registrar(string nombre, string menu, string tipoAcceso)
        {
            bool estadoEvento = false;
            string codigo = "NULL";

            JObject jo = new JObject();

            string[] splitMenu = menu.Split(',');
            string[] splitTipoAcceso = tipoAcceso.Split(',');

            perfil_usuario perfil = new perfil_usuario();

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

                perfil.nombre_perfil_usuario = nombre;
                perfil.estado_registro = true;
                perfil.fecha_registra = DateTime.Now;
                perfil.usuario_registra = beanSesionUsuario.codigoUsuario;
                
                List<permiso_menu> listaPermiso = new List<permiso_menu>();
                
                foreach (string nodo in splitMenu)
                {
                    if (nodo.Length > 0)
                    {
                        permiso_menu permiso_menu = new permiso_menu();
                        int codigoMenu;
                        int.TryParse(nodo, out codigoMenu);

                        permiso_menu.codigo_menu = codigoMenu;
                        permiso_menu.usuario_registra = beanSesionUsuario.codigoUsuario;
                        permiso_menu.fecha_registra = DateTime.Now;
                        permiso_menu.estado_registro = true;

                        listaPermiso.Add(permiso_menu);
                    }
                }

                List<item_tipo_acceso> listaItemTipoAcceso = new List<item_tipo_acceso>();
                foreach (string nodoTipo in splitTipoAcceso)
                {
                    if (nodoTipo.Length > 0)
                    {
                        item_tipo_acceso item_tipo_acceso = new item_tipo_acceso();
                        int codigoTipoAccesoItem;
                        int.TryParse(nodoTipo, out codigoTipoAccesoItem);

                        item_tipo_acceso.codigo_tipo_acceso_item = codigoTipoAccesoItem;
                        item_tipo_acceso.usuario_registra = beanSesionUsuario.codigoUsuario;
                        item_tipo_acceso.fecha_registra = DateTime.Now;
                        item_tipo_acceso.estado_registro = true;
                        listaItemTipoAcceso.Add(item_tipo_acceso); 
                    }
                }

                perfil.permiso_menu = listaPermiso;
                perfil.item_tipo_acceso = listaItemTipoAcceso;

                IResult respuesta = this._service.Create_Multiple(perfil);

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
                    listaBeanAtributo.Add(new BeanAtributo("nombre_perfil_usuario", perfil.nombre_perfil_usuario));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", perfil.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(perfil.fecha_registra)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_registra", perfil.usuario_registra));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "perfil_usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL REGISTRAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "perfil_usuario", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 1, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(string codigo, string nombre, string menu, string tipoAcceso)
        {
            bool estadoEvento = false;
            perfil_usuario registro = null;

            JObject jo = new JObject();
            
            string[] splitMenu = menu.Split(',');
            string[] splitTipoAcceso = tipoAcceso.Split(',');

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

                registro = this._service.GetSingle(int.Parse(codigo));
                registro.nombre_perfil_usuario = nombre;
                registro.fecha_modifica = DateTime.Now;
                registro.usuario_modifica = beanSesionUsuario.codigoUsuario;

                List<permiso_menu> listaPermiso = new List<permiso_menu>();
                foreach (string nodo in splitMenu)
                {
                    if (nodo.Length > 0)
                    {
                        permiso_menu permiso_menu = new permiso_menu();
                        int codigoMenu;
                        int.TryParse(nodo, out codigoMenu);

                        permiso_menu.codigo_menu = codigoMenu;
                        permiso_menu.usuario_registra = beanSesionUsuario.codigoUsuario;
                        permiso_menu.fecha_registra = DateTime.Now;
                        permiso_menu.estado_registro = true;

                        listaPermiso.Add(permiso_menu);
                    }
                }

                List<item_tipo_acceso> listaItemTipoAcceso = new List<item_tipo_acceso>();
                foreach (string nodoTipo in splitTipoAcceso)
                {
                    if (nodoTipo.Length > 0)
                    {
                        item_tipo_acceso item_tipo_acceso = new item_tipo_acceso();
                        int codigoTipoAccesoItem;
                        int.TryParse(nodoTipo, out codigoTipoAccesoItem);

                        item_tipo_acceso.codigo_tipo_acceso_item = codigoTipoAccesoItem;
                        item_tipo_acceso.usuario_registra = beanSesionUsuario.codigoUsuario;
                        item_tipo_acceso.fecha_registra = DateTime.Now;
                        item_tipo_acceso.estado_registro = true;
                        listaItemTipoAcceso.Add(item_tipo_acceso);
                    }
                }

                registro.item_tipo_acceso = listaItemTipoAcceso;
                registro.permiso_menu = listaPermiso;

                IResult respuesta = this._service.Update_Multiple(registro);
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

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("nombre_perfil_usuario", registro.nombre_perfil_usuario));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "perfil_usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "perfil_usuario", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 2, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        [HttpPost]
        public ActionResult Eliminar(string codigo)
        {
            bool estadoEvento = false;
            perfil_usuario registro = null;

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

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "perfil_usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL ELIMINAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "perfil_usuario", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 3, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult GetMenuJSON()
        {
            string result = this._Menuservice.GetTreeJson();
            return Content(result, "application/json");
        }


        [HttpPost]
        public ActionResult GetMenuByPerfilJSON(string codigoPerfil)
        {
            JObject jo = new JObject();

            int parseCodigoPerfil;
            if (!int.TryParse(codigoPerfil, out parseCodigoPerfil))
            {
                jo.Add("Msg", "Error al parsear codigo");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            string result = this._Menuservice.GetTreeJsonByPerfil(parseCodigoPerfil);
            return Content(result, "application/json");
        }

        public ActionResult GetListaTipoAccesoItemJSON()
        {
            string result = this._TipoAccesoItemService.GetAllJson(false);
            return Content(result, "application/json");
        }

        public ActionResult GetListaTipoAccesoItemBYPerfilJSON(string codigoPerfil)
        {
            JObject jo = new JObject();

            int parseCodigoPerfil;
            if (!int.TryParse(codigoPerfil, out parseCodigoPerfil))
            {
                jo.Add("Msg", "Error al parsear codigo");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            string result = this._TipoAccesoItemService.GetAllBeanByPerfilJson(parseCodigoPerfil, false);
            return Content(result, "application/json");
        }
    }
}
