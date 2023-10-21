using System.Web.Mvc;
using System.Web.Routing;

namespace SIGEES.Web.MemberShip.Helpers
{
    public class RedirectResultTo
    {
        public static ActionResult Redirect(RouteValueDictionary route, ActionExecutingContext context,bool autenticado )
        {
            ActionResult result;

           
            if (context.HttpContext.Request.IsAjaxRequest() && !autenticado)
            {               

                UrlHelper url = new UrlHelper(context.RequestContext);
                context.HttpContext.Response.StatusCode = 405;
                context.HttpContext.Response.StatusDescription = url.Action(null, route);
                context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                result = new JavaScriptResult()
                {
                    Script = "window.location = '" + url.Action(null, route)
                };
                context.HttpContext.Response.End();
            }
            else
            {
                result = new RedirectToRouteResult(route);
            }
           
            return result;
        }
    }
}