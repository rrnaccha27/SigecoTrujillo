using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace SIGEES.Web.Session
{
    public class SessionExpireFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(ExcludeSessionExpire), false).Any())
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            HttpContext ctx = HttpContext.Current;
            // check  sessions here

            System.Diagnostics.Debug.WriteLine("entro action filter atributo");

            if (HttpContext.Current.Session["usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}