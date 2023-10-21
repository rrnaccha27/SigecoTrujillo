using SIGEES.Web.Session;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //verifica si la sesion esta ahun activa, sino redirecciona a login
            //filters.Add(new SessionExpireFilterAttribute());
        }
    }
}