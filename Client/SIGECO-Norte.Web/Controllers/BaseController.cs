using SIGEES.Web.Helpers;
//using SIGEES.Web.MemberShip.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
namespace SIGEES.Web.Controllers
{
   //[SessionExpire]
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(   ActionExecutingContext filterContext)        
        {

            base.OnActionExecuting(filterContext); // re-added in edit
        }
    }
}

//namespace SIGEES.Web.Controllers
//{
//    public class BaseController : Controller
//    {
//        protected override void OnActionExecuting(
//        ActionExecutingContext filterContext)
//        {
//            // code involving this.Session // edited to simplify
//            base.OnActionExecuting(filterContext); // re-added in edit
//        }
//        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
//        {
//            string cultureName = null;

//            // Attempt to read the culture cookie from Request
//            HttpCookie cultureCookie = Request.Cookies["_culture"];
//            if (cultureCookie != null)
//                cultureName = cultureCookie.Value;
//            else
//                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
//                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
//                        null;
//            // Validate culture name
//            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

//            // Modify current thread's cultures            
//            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
//            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

//            return base.BeginExecuteCore(callback, state);
//        }

//    }
//}
