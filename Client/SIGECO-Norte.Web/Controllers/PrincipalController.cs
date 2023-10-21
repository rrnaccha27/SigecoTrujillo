using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.MemberShip.Filters;
//using SIGEES.Web.MemberShip.Filters;
//using SIGEES.Web.MemberShip.Helpers;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace SIGEES.Web.Controllers
{

    
    public class PrincipalController : BaseController
    {

      //  private readonly IMenuService _MenuService;
        //private IMenuService _MenuService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        //private readonly IEventoUsuarioService _eventoUsuarioService;
          
        public PrincipalController()
        {
            //beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
            //_MenuService = new MenuService();
        }

        
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    //Session.TimeOut = 60;

        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //    if (beanSesionUsuario == null)
        //        beanSesionUsuario = new BeanSesionUsuario();
        //}


         [HttpPost]
         //[SessionTimeout]
        public ActionResult GetMenuByPerfilJSON()
        {
            JObject jo = new JObject();
            string result = "{}";
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                if (Session[Common.Constante.session_name.sesionMenuPerfil] == null)
                {
                   
                    List<menu_dto> _lst_menu = UsuarioSelBL.Instance.GetMenuPrincipalByPerfilJson(beanSesionUsuario.codigoPerfil);
                    List<menu_dto> _lst_menu_permitido = obtenerMenuUsuario(_lst_menu);
                    string _menu_json = ConstruirMenu(_lst_menu_permitido);
                    Session[Common.Constante.session_name.sesionMenuPerfil] = _menu_json;
                }
                if (Session[Common.Constante.session_name.sesionListaRutaMenu] == null)
                {
                    List<String> listaRutaMenu = UsuarioSelBL.Instance.GetListaRutaMenuPerfil(beanSesionUsuario.codigoPerfil);
                    listaRutaMenu.Add("Home/Index");
                    Session[Common.Constante.session_name.sesionListaRutaMenu] = listaRutaMenu;
                }

                result = Session[Common.Constante.session_name.sesionMenuPerfil] as string;
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = "{}";
            }
            return Content(result, "application/json");
        }
         public List<String> ObtenerCodigoMenu(Nullable<int> codigoMenu, List<menu_dto> _lst_menu)
         {
             List<String> listaCodigos = new List<String>();

             if (codigoMenu> 0)
             {
                 menu_dto menu =_lst_menu.FirstOrDefault(x => x.codigo_menu == codigoMenu);

                 if ( menu!=null &&  menu.codigo_menu_padre>0)
                 {
                     listaCodigos.Add(menu.codigo_menu_padre.ToString());

                     listaCodigos.AddRange(ObtenerCodigoMenu(menu.codigo_menu_padre, _lst_menu));
                 }
             }

             return listaCodigos;
         }

         public List<String> ObtenerCodigoMenuPadre(List<int> listaCodigos, List<menu_dto> _lst_menu)
         {
             List<String> listaRetorno = new List<String>();

             foreach (var codigo in listaCodigos)
             {
                 listaRetorno.Add(codigo.ToString());
                 listaRetorno.AddRange(ObtenerCodigoMenu(codigo, _lst_menu));
             }

             listaRetorno = listaRetorno.Distinct().ToList();

             return listaRetorno;
         }
         private List<menu_dto> obtenerMenuUsuario(List<menu_dto> _lst_menu)
         {
             List<menu_dto> dtMenu = new List<menu_dto>();
          

             try
             {
                 List<menu_dto> dtMenuCompleto = _lst_menu.FindAll(x => x.tipo_orden == 2);
                 dtMenu = _lst_menu.FindAll(x => x.tipo_orden == 1);
                 List<int> listaCodigos = new List<int>();
           
                 foreach (var item in dtMenu)
                 {
                     listaCodigos.Add(item.codigo_menu);
                 }
                 List<String> listaCodigosMenu = new List<string>();
                 listaCodigosMenu = ObtenerCodigoMenuPadre(listaCodigos, dtMenuCompleto);

                 foreach (var item in listaCodigosMenu)
                 {
                   var resultdo=  dtMenuCompleto.FirstOrDefault(x=>x.codigo_menu==int.Parse(item));
                    if(resultdo!=null && !dtMenu.Exists(x=>x.codigo_menu==int.Parse(item)))
                    {
                        dtMenu.Add(resultdo);
                    }
                 }

             }
             catch (Exception ex)
             {

                 string mensaje = ex.Message;
             }
             finally
             {
                
             }
             return dtMenu;
         }

        
        public string ConstruirMenu(List<menu_dto> listaMenuPrincipal)
        {
            List<JObject> jObjects = new List<JObject>();
            List<menu_dto> _menu_padre = listaMenuPrincipal.FindAll(x => x.codigo_menu_padre == 0);        
            foreach (var item in _menu_padre)
            {
                BeanPermisoMenu beanPermisoMenu = new BeanPermisoMenu();
                beanPermisoMenu.codigoMenu = item.codigo_menu;
                beanPermisoMenu.nombreMenu = item.nombre_menu;
                JObject root = new JObject
                {
                    {"id", item.codigo_menu.ToString()}, 
                    {"state", "open"},                     
                    {"text", item.nombre_menu}
                };
                root.Add("children", this._GetChildMenuPrincipalJsonArray(item, listaMenuPrincipal));
                jObjects.Add(root);
            }            
            return JsonConvert.SerializeObject(jObjects);

        }

        private JArray _GetChildMenuPrincipalJsonArray(menu_dto parentNode, List<menu_dto> menu_principal)
        {
            JArray childArray = new JArray();
            List<menu_dto> lst_menu = menu_principal.FindAll(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden).ToList();
            foreach (var node in lst_menu)
            {
                JObject subObject = new JObject
                    {
                        {"id", node.codigo_menu.ToString()}, 
                        {"text", node.nombre_menu},                        
                        {"ruta", node.ruta_menu}
                    };
                
                if (string.IsNullOrWhiteSpace(node.ruta_menu))
                    subObject.Add("state","closed");
                            
                subObject.Add("children", this._GetChildMenuPrincipalJsonArray(node, menu_principal));
                childArray.Add(subObject);

            }
            return childArray;
        }
        [HttpPost]
        //[SessionTimeout]
        public ActionResult ActualizarSesion_CodigoMenu(string codigoMenu)
        {
            JObject jo = new JObject();
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                beanSesionUsuario.codigoMenu = int.Parse(codigoMenu);
                Session["usuario"] = beanSesionUsuario;
                //Session["id_menu_seleccionado"] = codigoMenu;

                jo.Add("Msg", "Success");                
            }
            catch (Exception ex)
            {
                ex.ToString();
                jo.Add("Msg", ex.ToString());
            }
            return Content(JsonConvert.SerializeObject(jo));
        }

        public ActionResult Salir()
        {
            Session.Clear();            
            return RedirectToAction("", "");
        }
    }
}
