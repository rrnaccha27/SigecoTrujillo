using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using SIGEES.Entidades;
using SIGEES.DataAcces;
using SIGEES.DataAcces.Helper;
using System.Collections.Generic;

namespace SIGEES.DataAcces
{

    #region TRANSACCION
    public partial class UsuarioDA : GenericDA<UsuarioDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public MensajeDTO Insertar(UsuarioDTO oUsuarioBE)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(Procedimiento.SP_USUARIO_INSERTAR);
            Int32 iOperacio = 1;
            Int64 idRegistro = 0;
            string vMensaje = string.Empty;

            MensajeDTO oMensajeBE = new MensajeDTO();
            try
            {
                //oDatabase.AddInParameter(oDbCommand, "@pidUsuario", DbType.Int16, oUsuarioBE.iCodUsuario);
                //oDatabase.AddInParameter(oDbCommand, "@pnombreUsuario", DbType.String, oUsuarioBE.vApePaterno);



                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    Int32 iiOperacion = oIDataReader.GetOrdinal("vIdOperacion");
                    Int32 iiMensaje = oIDataReader.GetOrdinal("vMensaje");
                    Int32 iiRegistro = oIDataReader.GetOrdinal("vIdRegistro");

                    if (oIDataReader.Read())
                    {
                        iOperacio = DataUtil.DbValueToDefault<Int32>(oIDataReader[iiOperacion]);
                        idRegistro = DataUtil.DbValueToDefault<Int64>(oIDataReader[iiRegistro]);
                        vMensaje = DataUtil.DbValueToDefault<string>(oIDataReader[iiMensaje]);
                    }
                }


            }
            catch (Exception ex)
            {
                iOperacio = -1;
                vMensaje = ex.Message;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

                oMensajeBE.idOperacion = iOperacio;
                oMensajeBE.idRegistro = idRegistro;
                oMensajeBE.mensaje = vMensaje;
            }

            return oMensajeBE;
        }
    }
    #endregion TRANSACCION

    #region NO TRANSACCIONAL
    public partial class UsuarioSelDA : GenericDA<UsuarioSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public UsuarioDTO Autenticar(string usuario)
        {
             UsuarioDTO oEntidad = null;           
   
             DbCommand oDbCommand = oDatabase.GetStoredProcCommand(Procedimiento.SP_USUARIO_AUTENTICAR);

             try
             {
                 oDatabase.AddInParameter(oDbCommand, "@p_codigo_usuario", DbType.String, usuario);

                 using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                 {
                     


                     Int32 codigo_usuario = oIDataReader.GetOrdinal("codigo_usuario");
                     Int32 clave = oIDataReader.GetOrdinal("clave");
                     Int32 codigo_persona = oIDataReader.GetOrdinal("codigo_persona");
                     Int32 codigo_perfil_usuario = oIDataReader.GetOrdinal("codigo_perfil_usuario");
                     Int32 estado_registro = oIDataReader.GetOrdinal("estado_registro");


                     if (oIDataReader.Read())
                     {
                         oEntidad = new UsuarioDTO();
                         oEntidad.usuario = DataUtil.DbValueToDefault<string>(oIDataReader[codigo_usuario]);
                         oEntidad.clave = DataUtil.DbValueToDefault<string>(oIDataReader[clave]);
                         oEntidad.codigoPersona = DataUtil.DbValueToNullable<int>(oIDataReader[codigo_persona]);
                         oEntidad.codigoPerfilUsuario = DataUtil.DbValueToNullable<int>(oIDataReader[codigo_perfil_usuario]);
                         oEntidad.estadoRegistro = DataUtil.DbValueToDefault<string>(oIDataReader[estado_registro]);   
                     }
                 }
             }
             catch
             {
                 throw;
             }
             finally
             {
                 if (oDbCommand != null) oDbCommand.Dispose();
                 oDbCommand = null;
             }
             
            return oEntidad;
        }

        public List<string> GetListaRutaMenuPerfil(int p_codigo_usuario_perfil)
        {
            
            List<string> _lst_menu = new List<string>();
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(Procedimiento.SP_MENU_BY_PERFIL);

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_perfil", DbType.Int32, p_codigo_usuario_perfil);
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                   

                    string _menu=string.Empty;
                    int _tipo_orden = 0;
                    Int32 ruta_menu = oIDataReader.GetOrdinal("ruta_menu");
                    Int32 tipo_orden = oIDataReader.GetOrdinal("tipo_orden");

                    while (oIDataReader.Read())
                    {
                        _menu = DataUtil.DbValueToDefault<string>(oIDataReader[ruta_menu]);
                        _tipo_orden = DataUtil.DbValueToDefault<int>(oIDataReader[tipo_orden]);
                        if (!string.IsNullOrWhiteSpace(_menu) && _tipo_orden == 1)
                             _lst_menu.Add(_menu);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return _lst_menu;
        }

        public List<menu_dto> GetMenuPrincipalByPerfilJson(int p_codigo_usuario_perfil)
        {
            List<menu_dto> _lst_menu = new List<menu_dto>();
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(Procedimiento.SP_MENU_BY_PERFIL);

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_perfil", DbType.Int32, p_codigo_usuario_perfil);
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {


                    
                    Int32 codigo_menu = oIDataReader.GetOrdinal("codigo_menu");
                    Int32 codigo_menu_padre = oIDataReader.GetOrdinal("codigo_menu_padre");
                    Int32 nombre_menu = oIDataReader.GetOrdinal("nombre_menu");
                    Int32 ruta_menu = oIDataReader.GetOrdinal("ruta_menu");
                    Int32 estado_registro = oIDataReader.GetOrdinal("estado_registro");
                    Int32 orden = oIDataReader.GetOrdinal("orden");
                    Int32 tipo_orden = oIDataReader.GetOrdinal("tipo_orden");

    
                    while (oIDataReader.Read())
                    {
                        menu_dto _menu = new menu_dto();

                        _menu.codigo_menu = DataUtil.DbValueToDefault<int>(oIDataReader[codigo_menu]);
                        _menu.codigo_menu_padre = DataUtil.DbValueToDefault<int>(oIDataReader[codigo_menu_padre]);
                        _menu.nombre_menu = DataUtil.DbValueToDefault<string>(oIDataReader[nombre_menu]);
                        _menu.ruta_menu = DataUtil.DbValueToDefault<string>(oIDataReader[ruta_menu]);
                        _menu.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader[estado_registro]);
                        _menu.orden = DataUtil.DbValueToDefault<int>(oIDataReader[orden]);
                        _menu.tipo_orden = DataUtil.DbValueToDefault<int>(oIDataReader[tipo_orden]);
                        _lst_menu.Add(_menu);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return _lst_menu;
        }

        public datos_usuario GetUsuarioById(string p_user_name)
        {
            return new datos_usuario();
        }
    }
    #endregion NO TRANSACCIONAL
}