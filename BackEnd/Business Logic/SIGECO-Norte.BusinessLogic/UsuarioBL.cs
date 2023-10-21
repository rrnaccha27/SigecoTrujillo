


using SIGEES.DataAcces;
using SIGEES.Entidades;
using System.Collections.Generic;

namespace SIGEES.BusinessLogic
{
    #region SECCION TRANSACCIONAL

    /*
    public partial class UsuarioBL
    {
        private readonly UsuarioDA oUsuarioDA = new UsuarioDA();
        /// <summary>
        /// Registrar usuario
        /// </summary>
        /// <param name="oUsuarioBE">
        /// Envia informaciòn del usuario.
        ///</param>
        /// <returns>
        /// Retorna informaciòn de operaciòn realizada.
        /// </returns>
        public MensajeDTO Insertar(UsuarioDTO oUsuarioBE)
        {

            MensajeDTO oMensajeBE = null;

            using (TransactionScope ts = new TransactionScope())
            {
                oMensajeBE = oUsuarioDA.Insertar(oUsuarioBE);

                if (oMensajeBE.idOperacion == 1)
                {
                    ts.Complete();
                }

            }
            return oMensajeBE;

        }
    }*/
    #endregion

    #region SECCION NO TRANSACCIONAL
    public partial class UsuarioSelBL : GenericBL<UsuarioSelBL>
    {
        //private readonly SIGEES.DataAcces.UsuarioSelDA oUsuarioDA = new SIGEES.DataAcces.UsuarioSelDA();
        /// <summary>
        /// Autenticar usuario
        /// </summary>
        /// <param name="oUsuarioBE">
        /// Envia informaciòn del usuario para autenticar.
        ///</param>
        /// <returns>
        /// Retorna al usuario.
        /// </returns>
        /*
        public UsuarioDTO BuscarId(UsuarioDTO oUsuarioBE)
        {
            return oUsuarioDA.Autenticar(oUsuarioBE);
        }*/

        public UsuarioDTO Autenticar(string usuario)
        {
          return  UsuarioSelDA.Instance.Autenticar(usuario);
            //return usu .Autenticar(usuario);
        }




        public List<string> GetListaRutaMenuPerfil(int p_codigo_usuario_perfil)
        {
            return UsuarioSelDA.Instance.GetListaRutaMenuPerfil(p_codigo_usuario_perfil);
        }

        public List<menu_dto> GetMenuPrincipalByPerfilJson(int p_codigo_usuario_perfil)
        {
            return UsuarioSelDA.Instance.GetMenuPrincipalByPerfilJson(p_codigo_usuario_perfil);
        }

        public datos_usuario GetUsuarioById(string p_user_name)
        {
            return UsuarioSelDA.Instance.GetUsuarioById(p_user_name);
        }
    }

    #endregion
}