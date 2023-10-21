

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SIGEES.Web.MemberShip.Filters
{
    public class PersistentSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Diagnostics.Debug.WriteLine("PRUEBAAAAAAAAAAAAAAAAAAAA DEEEEEEEEEEEEEEEEEEEE INGRESOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO" );
            if (filterContext.HttpContext.Request.IsAuthenticated && HttpContext.Current.Session["Usuario"] == null)
            {
                HttpCookie userData = HttpContext.Current.Request.Cookies["userCookie"];
                if (userData != null)
                {
                    
                    string stringUserData = SIGEES.Web.MemberShip.Cryptography.StringCipher.Decrypt(userData.Value, "1234");
                    HttpContext.Current.Session["Usuario"] = DeserializeJsonTo<SIGEES.Entidades.UsuarioDTO>(stringUserData);
                }
            }

            if (!filterContext.HttpContext.Request.IsAuthenticated && HttpContext.Current.Session["Usuario"] != null)
            {
                HttpContext.Current.Session["Usuario"] = null;
            }

            base.OnActionExecuting(filterContext);
        }

        public static string SerializeToJson(object objeto)
        {
            var jsonSerializer = new DataContractJsonSerializer(objeto.GetType());
            var ms = new MemoryStream();
            jsonSerializer.WriteObject(ms, objeto);
            var jsonResult = Encoding.Default.GetString(ms.ToArray());
            return jsonResult;
        }

        ///
        /// Método extensor para deserializar JSON cualquier objeto
        ///
        public static T DeserializeJsonTo<T>(string jsonSerializado)
        {
            var jss = new JavaScriptSerializer();
            T entidad = jss.Deserialize<T>(jsonSerializado);
            return entidad;
        }
    }
}