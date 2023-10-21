using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIGEES.Web.MemberShip.Filters
{
    /*
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (!(HttpContext.Current.Session[Common.Constante.session_name.sesionUsuario] != null))
            {
                filterContext.Result = SIGEES.Web.MemberShip.Helpers.RedirectResultTo.Redirect(
                             new RouteValueDictionary(new
                             {
                                 controller = "Home",
                                 action = "TimeoutRedirect",
                                 area = string.Empty
                             }

                         ), filterContext, false);
               // string redirectTo = "~/Home/TimeoutRedirect";
                filterContext.HttpContext.Response.StatusCode = 123;
                //filterContext.HttpContext.Response.Redirect(redirectTo, true);
                //return;
                //return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
    */
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // If the browser session or authentication session has expired...
            //if (ctx.Session[Common.Constante.session_name.sesionUsuario] == null || !filterContext.HttpContext.Request.IsAuthenticated)
            if (ctx.Session[Common.Constante.session_name.sesionUsuario] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.HttpContext.Request.HttpMethod == "POST")
                {
                    // For AJAX requests, we're overriding the returned JSON result with a simple string,
                    // indicating to the calling JavaScript code that a redirect should be performed.
                    filterContext.Result = new JsonResult { Data = "_Logon_" };
                }
                else
                {
                    // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
                    // simply displays a temporary 5 second notification that they have timed out, and
                    // will, in turn, redirect to the logon page.
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "TimeoutRedirect" },
                        { "area", string.Empty }
                });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LocsAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // If the browser session has expired...
            if (ctx.Session[Common.Constante.session_name.sesionUsuario] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    // For AJAX requests, we're overriding the returned JSON result with a simple string,
                    // indicating to the calling JavaScript code that a redirect should be performed.
                    filterContext.Result = new JsonResult { Data = "_Logon_" };
                }
                else
                {
                    // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
                    // simply displays a temporary 5 second notification that they have timed out, and
                    // will, in turn, redirect to the logon page.
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "TimeoutRedirect" },
                        { "area", string.Empty }
                });
                }
            }
            else if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                // Otherwise the reason we got here was because the user didn't have access rights to the
                // operation, and a 403 should be returned.
                filterContext.Result = new HttpStatusCodeResult(403);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}