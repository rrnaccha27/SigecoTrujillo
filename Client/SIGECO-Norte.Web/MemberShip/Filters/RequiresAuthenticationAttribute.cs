

using SIGEES.BusinessLogic;
using SIGEES.Web.MemberShip.Helpers;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace SIGEES.Web.MemberShip.Filters
{
    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Session.Timeout = 60;

            if (!(HttpContext.Current.Session[Common.Constante.session_name.sesionUsuario] != null))
            {
                FormsAuthentication.SignOut();

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Clear();
                    var route = new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "TimeoutRedirect",
                        area = string.Empty
                    });
                    UrlHelper url = new UrlHelper(filterContext.RequestContext);
                    filterContext.HttpContext.Response.StatusCode = 405;
                    filterContext.HttpContext.Response.StatusDescription = url.Action(null, route);
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new ViewResult { ViewName = "Index" };


                }
                else
                {
                    filterContext.Result = SIGEES.Web.MemberShip.Helpers.RedirectResultTo.Redirect(
                           new RouteValueDictionary(new
                           {
                               controller = "Home",
                               action = "TimeoutRedirect",

                               //controller = "Account",
                               //action = "Login",
                               area = string.Empty
                               //Reference = path
                           }

                       ), filterContext, false);
                }




            }
            else
            {
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string areaName = filterContext.RequestContext.RouteData.DataTokens["area"] as string;

                //string controllerDefault = "Home";
                //string actionDefault = "Index";
                bool permisoMenu = false;
                List<string> listaRutaMenu = new List<string>();
                if (HttpContext.Current.Session[Common.Constante.session_name.sesionListaRutaMenu] == null)
                {
                    BeanSesionUsuario beanSesionUsuario = HttpContext.Current.Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                    listaRutaMenu = UsuarioSelBL.Instance.GetListaRutaMenuPerfil(beanSesionUsuario.codigoPerfil);
                    HttpContext.Current.Session[Common.Constante.session_name.sesionListaRutaMenu] = listaRutaMenu;
                }
                else
                {
                    listaRutaMenu = HttpContext.Current.Session[Common.Constante.session_name.sesionListaRutaMenu] as List<string>;
                }

                if (!listaRutaMenu.Exists(x => x == "Comision/Grupo"))
                {
                    listaRutaMenu.Add("Comision/Grupo");
                }


                if (!listaRutaMenu.Exists(x => x == "Home/Index"))
                    listaRutaMenu.Add("Home/Index");
                //listaRutaMenu.Add(controllerDefault + "/" + actionDefault);
                if (string.IsNullOrWhiteSpace(areaName))
                {
                    permisoMenu = listaRutaMenu.Exists(x => x.Split('/')[0] == controllerName || x.Split('/')[1] == controllerName);
                }
                else
                {
                    permisoMenu = listaRutaMenu.Exists(x => x.Split('/')[1] == controllerName);
                }
                //permisoMenu = listaRutaMenu.Exists(x => x.Split('/')[0] == controllerName);
                if (!permisoMenu && actionName.ToLower() == "index")
                {
                    filterContext.Result = SIGEES.Web.MemberShip.Helpers.RedirectResultTo.Redirect(
                        new RouteValueDictionary(new
                        {
                            controller = "Home",
                            action = "AccesoDenegado",
                            area = string.Empty
                        }

                    ), filterContext, true);
                }


            }
            base.OnActionExecuting(filterContext);
        }
    }
}