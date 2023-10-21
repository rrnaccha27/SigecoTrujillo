

using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIGEES.Web.MemberShip.Filters
{
    public class RequiresAuthorizationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Boolean isAccessibleToUser = false;

            //if (SiteMaps.Current.CurrentNode != null)
            //{
            //    isAccessibleToUser = SiteMaps.Current.CurrentNode.IsAccessibleToUser();
            //}
            if (!isAccessibleToUser)
            {
                filterContext.Result = SIGEES.Web.MemberShip.Helpers.RedirectResultTo.Redirect(
                    new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Index"
                    }

                ), filterContext,true);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}