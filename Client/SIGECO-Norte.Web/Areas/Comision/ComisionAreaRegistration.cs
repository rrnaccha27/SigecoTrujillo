using System.Web.Mvc;

namespace SIGEES.Web.Areas.Comision
{
    public class ComisionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Comision";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Comision_default",
                "Comision/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
