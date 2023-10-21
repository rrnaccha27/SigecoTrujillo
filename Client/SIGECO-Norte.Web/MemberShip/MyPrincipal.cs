using SIGEES.Web.Models.Bean;
using System.Security.Principal;

namespace SIGEES.Web.MemberShip
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public BeanSesionUsuario User { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}