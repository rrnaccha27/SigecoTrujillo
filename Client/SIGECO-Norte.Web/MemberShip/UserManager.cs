using Newtonsoft.Json;
using SIGEES.Web.Models.Bean;
using System;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SIGEES.Web.MemberShip
{
    public static class UserManager
    {
        /// <summary>
        /// Returns the User from the Context.User.Identity by decrypting the forms auth ticket and returning the user object.
        /// </summary>
        public static BeanSesionUsuario USUARIO
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // The user is authenticated. Return the user from the forms auth ticket.
                    return ((MyPrincipal)(HttpContext.Current.User)).User;
                }
                else if (HttpContext.Current.Items.Contains("USUARIO"))
                {
                    // The user is not authenticated, but has successfully logged in.
                    return (BeanSesionUsuario)HttpContext.Current.Items["USUARIO"];
                }
                else
                {
                    return null;
                }
            }

            
        }

        /// <summary>
        /// Authenticates a user against a database, web service, etc.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>User</returns>
        public static SIGEES.Entidades.UsuarioDTO AuthenticateUser(string username, string password)
        {
            var oUSUARIO = new SIGEES.Entidades.UsuarioDTO();

            try
            {
                oUSUARIO.vUsuario = username;
                oUSUARIO.vPassword = password;                
                var oSeguridad = new SIGEES.BusinessLogic.UsuarioSelBL();
                //oUSUARIO = oSeguridad.Autenticar(oUSUARIO);
            }
            catch (Exception ex)
            {
                //QaliWarma.Framework.Common.App.Exception.Registrar(ex);
                throw;
            }

            return oUSUARIO;
        }

        /// <summary>
        /// Authenticates a user via the MembershipProvider and creates the associated forms authentication ticket.
        /// </summary>
        /// <param name="logon">Logon</param>
        /// <param name="response">HttpResponseBase</param>
        /// <returns>bool</returns>
        public static bool ValidateUser(SIGEES.Entidades.UsuarioDTO logon, HttpResponseBase response)
        {
            bool result = false;

            if (Membership.ValidateUser(logon.vUsuario, logon.vPassword))
            {
                // Create the authentication ticket with custom user data.
                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(UserManager.USUARIO);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        logon.vUsuario,
                        DateTime.Now,
                        DateTime.Now.AddHours(3),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);                
                string encTicket = FormsAuthentication.Encrypt(ticket);                
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Clears the user session, clears the forms auth ticket, expires the forms auth cookie.
        /// </summary>
        /// <param name="session">HttpSessionStateBase</param>
        /// <param name="response">HttpResponseBase</param>
        public static void Logoff(HttpSessionStateBase session, HttpResponseBase response)
        {
            // Delete the user details from cache.
            session.Abandon();

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie);
        }

        public static void _AuthenticateRequest()
        {
            
           
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms");
                var principal = new MyPrincipal(identity);

                // Get the custom user data encrypted in the ticket.
                string userData = ((FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;

                // Deserialize the json data and set it on the custom principal.
                var serializer = new JavaScriptSerializer();
                principal.User = new BeanSesionUsuario();
                principal.User = JsonConvert.DeserializeObject<BeanSesionUsuario>(userData);
                // Set the context user.
                System.Web.HttpContext.Current.User = principal;
            }
        }
    }
}