using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace SIGEES.Web.MemberShip.Providers
{
    public class SIGEESRoleProvider : RoleProvider
    {
       

        [Dependency]
        public  SIGEES.BusinessLogic.UsuarioSelBL oISeguridadService { get; set; }
        public SIGEESRoleProvider()
        {
            
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            List<String> Roles = new List<String>();
            //ListaRol ListaRol = new ListaRol();

            try
            {
                //Usuario usuario = new Usuario();
                //usuario.Login = username;
                //ListaRol = oISeguridadService.ObtenerRolPorUsuario(usuario);
                //foreach (Rol item in ListaRol)
                //    Roles.Add(item.Nombre);
            }
            catch (Exception)
            {
                return Roles.ToArray();
            }

            return Roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}