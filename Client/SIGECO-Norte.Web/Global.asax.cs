using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Security.Principal;
using System.Web.Script.Serialization;
using System.Web.Security;
using SIGEES.Web.App_Start;
using SIGEES.Web.MemberShip;


namespace SIGEES.Web
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            Bootstrapper.Initialise();

              
            foreach(var factory in ValueProviderFactories.Factories)
            {
                if(factory is JsonValueProviderFactory)
                {
                    ValueProviderFactories.Factories.Remove(factory as JsonValueProviderFactory);
                    break;
                }
            }
            ValueProviderFactories.Factories.Add(new CustomJsonValueProviderFactory());
           
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            //Session["IsActiveSession"] = DateTime.Now;
            //Session.Timeout = 1;
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //HttpContext.Current.Session.Timeout = 60;
           // UserManager._AuthenticateRequest();
        }
    }
}