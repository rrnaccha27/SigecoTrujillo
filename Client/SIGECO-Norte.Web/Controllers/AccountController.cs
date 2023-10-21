using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using SIGEES.Web.Helpers;
using SIGEES.Web.Models.Bean;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using SIGEES.Web.Models;
using SIGEES.Web.Session;
using SIGEES.Web.Core;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;

namespace SIGEES.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUsuarioService _UsuarioService;
        private readonly IPerfilUsuarioService _PerfilUsuarioService;
        private readonly IParametroSistemaService _ParametroSistemaService;
        private readonly IMenuService _MenuService;
        public static string urlReference = string.Empty;
        public AccountController()
        {
            _UsuarioService = new UsuarioService();
            _ParametroSistemaService = new ParametroSistemaService();
            _PerfilUsuarioService = new PerfilUsuarioService();
            _MenuService = new MenuService();
            //HttpContext.Current.Request

            //urlReference = HttpContext.Request.QueryString["Reference"];
        }

        [HttpGet]
        //[ExcludeSessionExpire]
        public ActionResult Login()
        {
            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
            {
                urlReference = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            }
            else {
                urlReference = string.Empty;
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string clave)
        {
            JObject jo = new JObject();
            try
            {

            
            if (string.IsNullOrWhiteSpace(usuario))
            {
                jo.Add("Msg", "POR FAVOR INGRESE USUARIO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            else if (string.IsNullOrWhiteSpace(clave))
            {
                jo.Add("Msg", "POR FAVOR INGRESE CLAVE");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            bool loginModoServerDominio = false;

            if ( usuario.ToLower() != "root")
            {
                parametro_sistema parametro_sistema = _ParametroSistemaService.GetParametro(Globales.parametroModoAutenticacion);

                if (parametro_sistema != null)
                {
                    if (parametro_sistema.valor == "1")
                    {
                        loginModoServerDominio = true;
                    }
                }
            }

             
             UsuarioDTO _beanUser = UsuarioSelBL.Instance.Autenticar(usuario);

             BeanUser beanUser = _beanUser == null ? null : new BeanUser() { 
             usuario=_beanUser.usuario,
             codigoPerfilUsuario=_beanUser.codigoPerfilUsuario,
             codigoPersona=_beanUser.codigoPersona,
             estadoRegistro=_beanUser.estadoRegistro,
             clave=_beanUser.clave
             };
                
                //_UsuarioService.Login(usuario);

            if (beanUser == null)
            {
                jo.Add("Msg", "USUARIO NO REGISTRADO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
                 
            

            if (loginModoServerDominio)
            {
                //VERIFICACION DE LOGIN POR ACTIVE DIRECTORY

                MembershipUser usuarioAD = Membership.GetUser(usuario);
                if (!usuarioAD.IsApproved)
                {
                    jo.Add("Msg", "USUARIO DESHABILITADO EN EL SERVIDOR DE DOMINIO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(usuario, clave))
                    {
                        if (beanUser.estadoRegistro.CompareTo("I") == 0)
                        {
                            jo.Add("Msg", "USUARIO SE ENCUENTRA DESACTIVADO POR BD");
                            return Content(JsonConvert.SerializeObject(jo), "application/json");
                        }
                        else if (beanUser.codigoPerfilUsuario == null)
                        {
                            jo.Add("Msg", "USUARIO NO TIENE ASIGNADO UN PERFIL");
                            return Content(JsonConvert.SerializeObject(jo), "application/json");
                        }

                        FormsAuthentication.SetAuthCookie(usuario, false);

                        BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
                        beanSesionUsuario.codigoUsuario = usuario;
                        beanSesionUsuario.codigoMenu = 1;
                        beanSesionUsuario.codigoPerfil = beanUser.codigoPerfilUsuario != null ? beanUser.codigoPerfilUsuario.Value : 0;

                        Session[Common.Constante.session_name.sesionUsuario] = beanSesionUsuario;
                        jo.Add("Msg", "Success");                       
                    }
                    else
                    {
                        jo.Add("Msg", "USUARIO O CLAVE INCORRECTO");
                        return Content(JsonConvert.SerializeObject(jo), "application/json");
                    }
                }

            }
            else
            {
                //VERIFICACION DE LOGIN POR BASE DE DATOS

                if (clave.CompareTo(CifradoAES.DecryptStringAES(beanUser.clave, Globales.llaveCifradoClave)) != 0)
                {
                    jo.Add("Msg", "USUARIO O CLAVE INCORRECTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else
                {
                    if (beanUser.estadoRegistro.CompareTo("I") == 0)
                    {
                        jo.Add("Msg", "USUARIO SE ENCUENTRA DESACTIVADO");
                        return Content(JsonConvert.SerializeObject(jo), "application/json");
                    }
                    else if (beanUser.codigoPerfilUsuario == null)
                    {
                        jo.Add("Msg", "USUARIO NO TIENE ASIGNADO UN PERFIL");
                        return Content(JsonConvert.SerializeObject(jo), "application/json");
                    }

                    BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
                    beanSesionUsuario.codigoUsuario = usuario;
                    beanSesionUsuario.codigoMenu = 1;
                    beanSesionUsuario.codigoPerfil = beanUser.codigoPerfilUsuario != null ? beanUser.codigoPerfilUsuario.Value : 0;
                    Session[Common.Constante.session_name.sesionUsuario] = beanSesionUsuario;
                    jo.Add("Msg", "Success");                 
                }
            }
            string urlReporte = Url.Action("Index", "Home", new { area = string.Empty }) ;
            //if (!string.IsNullOrWhiteSpace(urlReference))
            //    jo.Add("url_pagina", urlReference);
            //else
                jo.Add("url_pagina", urlReporte);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                jo.Add("Msg", "Usuario y/o contraseña incorrecta, o la cuenta esta bloqueada.");  
            }
            //var url_ref = Request.UrlReferrer.Query;
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
       
        [HttpGet()]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            SIGEES.Web.MemberShip.UserManager.Logoff(Session,Response);
            return RedirectToAction("", "");
        }

    }    
}
