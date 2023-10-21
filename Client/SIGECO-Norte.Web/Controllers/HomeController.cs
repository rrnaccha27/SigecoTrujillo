using SIGEES.Web.MemberShip.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;


///SICACO
namespace SIGEES.Web.Controllers
{
    
    public class HomeController : Controller
    {

        
        [RequiresAuthentication]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

         [HttpGet]
        public ActionResult TimeoutRedirect()
        {
            //HtmlMeta RedirectMetaTag = new HtmlMeta();
            //RedirectMetaTag.HttpEquiv = "Refresh";
            //RedirectMetaTag.Content = string.Format("{0}; URL={1}", this.Context.Items["ErrorMessage_Timeout"], NewUrl);
            //this.Response.AddHeader = RedirectMetaTag;
            
            
            return View();
        }
        


        [HttpGet]
        [RequiresAuthentication()]
        public ActionResult AcercaDe()
        {
            return View();
        }

        [HttpGet]
        [RequiresAuthentication()]
        public ActionResult Ayuda()
        {
            return View();
        }

        [HttpGet]
        [RequiresAuthentication()]
        public ActionResult Version()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
