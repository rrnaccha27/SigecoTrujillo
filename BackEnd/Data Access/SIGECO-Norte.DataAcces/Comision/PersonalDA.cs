using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

using SIGEES.Entidades;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;

namespace SIGEES.DataAcces
{
    public class PersonalDA 
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public DataTable ListarDT(int codigoCanal, int codigoGrupo, int estadoRegistro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_listar");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, codigoCanal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, codigoGrupo);
                oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Boolean, estadoRegistro);

                dt.Load(oDatabase.ExecuteReader(oDbCommand));

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }
        public bool AsignarCanalGrupoMasivo(int codigo_canal_grupo, int codigo_personal, bool es_canal_grupo, string usuario_modifica, string xmlPersonal)
        {
            bool resultado = false;
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_asignar_canal_grupo");
			oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal_supervisor", DbType.Int32, codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Int32, (es_canal_grupo?1:0));
			oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, usuario_modifica);
			oDatabase.AddInParameter(oDbCommand, "@p_PersonalXML", DbType.Xml, xmlPersonal);
			
			articulo_dto articulo = new articulo_dto();
            List<articulo_dto> lstArticulo = new List<articulo_dto>();
            
			try
			{
				oDatabase.ExecuteNonQuery(oDbCommand);
            }
			finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
				resultado = true;
            }
			
			return resultado;
        }

        public int Registrar(personal_dto personal)
        {
            int codigo_personal = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_banco", DbType.Int32, personal.codigo_banco);
                oDatabase.AddInParameter(oDbCommand, "@codigo_moneda", DbType.Int32, personal.codigo_moneda);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_cuenta", DbType.Int32, personal.codigo_tipo_cuenta);

                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_documento", DbType.Int32, personal.codigo_tipo_documento);
                oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, personal.nombre);
                oDatabase.AddInParameter(oDbCommand, "@apellido_paterno", DbType.String, personal.apellido_paterno);
                oDatabase.AddInParameter(oDbCommand, "@apellido_materno", DbType.String, personal.apellido_materno);
                oDatabase.AddInParameter(oDbCommand, "@nro_documento", DbType.String, personal.nro_documento);
                oDatabase.AddInParameter(oDbCommand, "@nro_ruc", DbType.String, personal.nro_ruc);
                oDatabase.AddInParameter(oDbCommand, "@telefono_fijo", DbType.String, personal.telefono_fijo);
                oDatabase.AddInParameter(oDbCommand, "@telefono_celular", DbType.String, personal.telefono_celular);
                oDatabase.AddInParameter(oDbCommand, "@correo_electronico", DbType.String, personal.correo_electronico);
                oDatabase.AddInParameter(oDbCommand, "@nro_cuenta", DbType.String, personal.nro_cuenta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_interbancario", DbType.String, personal.codigo_interbancario);
                oDatabase.AddInParameter(oDbCommand, "@es_persona_juridica", DbType.Boolean, personal.es_persona_juridica);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, personal.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@direccion", DbType.String, personal.direccion);
                oDatabase.AddInParameter(oDbCommand, "@contacto", DbType.String, personal.contacto);
                oDatabase.AddInParameter(oDbCommand, "@codigo_sede", DbType.Int32, personal.codigo_sede);

                oDatabase.AddOutParameter(oDbCommand, "@codigo_personal", DbType.Int32, 0);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@codigo_personal").ToString();
                codigo_personal = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_personal;
        }

        public int Actualizar(personal_dto personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_actualizar");
            List<personal_planilla_listado_dto> lst = new List<personal_planilla_listado_dto>();
            
            int res = 0;
            oDatabase.AddInParameter(oDbCommand, "@codigo_personal", DbType.Int32, personal.codigo_personal);

            oDatabase.AddInParameter(oDbCommand, "@codigo_moneda", DbType.Int32, personal.codigo_moneda);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_cuenta", DbType.Int32, personal.codigo_tipo_cuenta);

            oDatabase.AddInParameter(oDbCommand, "@codigo_banco", DbType.Int32, personal.codigo_banco);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_documento", DbType.Int32, personal.codigo_tipo_documento);
            oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, personal.nombre);
            oDatabase.AddInParameter(oDbCommand, "@apellido_paterno", DbType.String, personal.apellido_paterno);
            oDatabase.AddInParameter(oDbCommand, "@apellido_materno", DbType.String, personal.apellido_materno);
            oDatabase.AddInParameter(oDbCommand, "@nro_documento", DbType.String, personal.nro_documento);
            oDatabase.AddInParameter(oDbCommand, "@nro_ruc", DbType.String,  personal.nro_ruc);
            oDatabase.AddInParameter(oDbCommand, "@telefono_fijo", DbType.String, personal.telefono_fijo);
            oDatabase.AddInParameter(oDbCommand, "@telefono_celular", DbType.String, personal.telefono_celular);
            oDatabase.AddInParameter(oDbCommand, "@correo_electronico", DbType.String, personal.correo_electronico);
            oDatabase.AddInParameter(oDbCommand, "@nro_cuenta", DbType.String, personal.nro_cuenta);
            oDatabase.AddInParameter(oDbCommand, "@codigo_interbancario", DbType.String, personal.codigo_interbancario);
            oDatabase.AddInParameter(oDbCommand, "@es_persona_juridica", DbType.Boolean, personal.es_persona_juridica);
            oDatabase.AddInParameter(oDbCommand, "@usuario_modifica", DbType.String, personal.usuario_modifica);
            oDatabase.AddInParameter(oDbCommand, "@contacto", DbType.String, personal.contacto);
            oDatabase.AddInParameter(oDbCommand, "@direccion", DbType.String, personal.direccion);

            
            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return res;
        }

        public int Eliminar(Int32 codigo_personal)
        {
            int res = 0;
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_personal_eliminar", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_personal", SqlDbType.Int).Value = codigo_personal;
            try
            {
                oConexion.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return res;
        }

        public void Desactivar(personal_dto personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_actualizar_estado");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, personal.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, personal.usuario_modifica);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Activar (personal_dto personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_activar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, personal.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, personal.usuario_modifica);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public personal_dto GetReg(Int32 codigo_personal)
        {
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_personal_getreg", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_personal", SqlDbType.Int).Value = codigo_personal;
            personal_dto entidad = null;
            try
            {
                oConexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    entidad = new personal_dto();

                    int icodigo_personal = reader.GetOrdinal("codigo_personal");
                    if (!reader.IsDBNull(icodigo_personal)) entidad.codigo_personal = reader.GetInt32(icodigo_personal);

                    int icodigo_banco = reader.GetOrdinal("codigo_banco");
                    if (!reader.IsDBNull(icodigo_banco)) entidad.codigo_banco = reader.GetInt32(icodigo_banco);

                    int icodigo_cuenta_moneda = reader.GetOrdinal("codigo_cuenta_moneda");
                    if (!reader.IsDBNull(icodigo_cuenta_moneda)) entidad.codigo_moneda = reader.GetInt32(icodigo_cuenta_moneda);

                    int icodigo_tipo_documento = reader.GetOrdinal("codigo_tipo_documento");
                    if (!reader.IsDBNull(icodigo_tipo_documento)) entidad.codigo_tipo_documento = reader.GetInt32(icodigo_tipo_documento);

                    int inombre = reader.GetOrdinal("nombre");
                    if (!reader.IsDBNull(inombre)) entidad.nombre = reader.GetString(inombre);


                    int codigo_tipo_cuenta = reader.GetOrdinal("codigo_tipo_cuenta");
                    if (!reader.IsDBNull(codigo_tipo_cuenta)) entidad.codigo_tipo_cuenta = reader.GetInt32(codigo_tipo_cuenta);


                    int iapellido_paterno = reader.GetOrdinal("apellido_paterno");
                    if (!reader.IsDBNull(iapellido_paterno)) entidad.apellido_paterno = reader.GetString(iapellido_paterno);

                    int iapellido_materno = reader.GetOrdinal("apellido_materno");
                    if (!reader.IsDBNull(iapellido_materno)) entidad.apellido_materno = reader.GetString(iapellido_materno);

                    int inro_documento = reader.GetOrdinal("nro_documento");
                    if (!reader.IsDBNull(inro_documento)) entidad.nro_documento = reader.GetString(inro_documento);

                    int inro_ruc = reader.GetOrdinal("nro_ruc");
                    if (!reader.IsDBNull(inro_ruc)) entidad.nro_ruc = reader.GetString(inro_ruc);

                    int itelefono_fijo = reader.GetOrdinal("telefono_fijo");
                    if (!reader.IsDBNull(itelefono_fijo)) entidad.telefono_fijo = reader.GetString(itelefono_fijo);

                    int itelefono_celular = reader.GetOrdinal("telefono_celular");
                    if (!reader.IsDBNull(itelefono_celular)) entidad.telefono_celular = reader.GetString(itelefono_celular);

                    int icorreo_electronico = reader.GetOrdinal("correo_electronico");
                    if (!reader.IsDBNull(icorreo_electronico)) entidad.correo_electronico = reader.GetString(icorreo_electronico);
                    
                    int inro_cuenta = reader.GetOrdinal("nro_cuenta");
                    if (!reader.IsDBNull(inro_cuenta)) entidad.nro_cuenta = reader.GetString(inro_cuenta);

                    int icodigo_interbancario = reader.GetOrdinal("codigo_interbancario");
                    if (!reader.IsDBNull(icodigo_interbancario)) entidad.codigo_interbancario = reader.GetString(icodigo_interbancario);

                    int ies_persona_juridica = reader.GetOrdinal("es_persona_juridica");
                    if (!reader.IsDBNull(ies_persona_juridica)) entidad.es_persona_juridica = reader.GetBoolean(ies_persona_juridica);

                    int iestado_registro = reader.GetOrdinal("estado_registro");
                    if (!reader.IsDBNull(iestado_registro)) entidad.estado_registro = reader.GetBoolean(iestado_registro);

                    int iestado_registro_texto = reader.GetOrdinal("estado_registro_texto");
                    if (!reader.IsDBNull(iestado_registro_texto)) entidad.estado_registro_texto = reader.GetString(iestado_registro_texto);

                    int iusuario_registra = reader.GetOrdinal("usuario_registra");
                    if (!reader.IsDBNull(iusuario_registra)) entidad.usuario_registra = reader.GetString(iusuario_registra);

                    int ifecha_registra = reader.GetOrdinal("fecha_registra");
                    if (!reader.IsDBNull(ifecha_registra)) entidad.fecha_registra_texto = reader.GetString(ifecha_registra);

                    int iusuario_modifica = reader.GetOrdinal("usuario_modifica");
                    if (!reader.IsDBNull(iusuario_modifica)) entidad.usuario_modifica = reader.GetString(iusuario_modifica);

                    int ifecha_modifica = reader.GetOrdinal("fecha_modifica");
                    if (!reader.IsDBNull(ifecha_modifica)) entidad.fecha_modifica_texto = reader.GetString(ifecha_modifica);

                    /*VALIDACION*/
                    int ivalidado = reader.GetOrdinal("validado");
                    if (!reader.IsDBNull(ivalidado)) entidad.validado = reader.GetInt32(ivalidado);

                    int ivalidado_texto = reader.GetOrdinal("validado_texto");
                    if (!reader.IsDBNull(ivalidado_texto)) entidad.validado_texto = reader.GetString(ivalidado_texto);

                    int ifecha_validado = reader.GetOrdinal("fecha_validado");
                    if (!reader.IsDBNull(ifecha_validado)) entidad.fecha_validado = reader.GetString(ifecha_validado);

                    int iusuario_validado = reader.GetOrdinal("usuario_validado");
                    if (!reader.IsDBNull(iusuario_validado)) entidad.usuario_validado = reader.GetString(iusuario_validado);

                    int direccion = reader.GetOrdinal("direccion");
                    if (!reader.IsDBNull(direccion)) entidad.direccion = reader.GetString(direccion);

                    int contacto = reader.GetOrdinal("contacto");
                    if (!reader.IsDBNull(contacto)) entidad.contacto = reader.GetString(contacto);
                }
                reader.Close();
            }
          
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return entidad;
        }

        //public List<personal_dto> ListarAll(int codigoCanal, int codigoGrupo)
        //{
        //    SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
        //    SqlCommand cmd = new SqlCommand("up_personal_LISTAR_All", oConexion);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@codigo_canal_grupo", SqlDbType.Int).Value = codigo_canal_grupo;
        //    List<personal_dto> lista = new List<personal_dto>();
        //    try
        //    {
        //        oConexion.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            personal_dto entidad = new personal_dto();

        //            int icodigo_personal = reader.GetOrdinal("codigo_personal");
        //            if (!reader.IsDBNull(icodigo_personal)) entidad.codigo_personal = reader.GetInt32(icodigo_personal);

        //            int icodigo_banco = reader.GetOrdinal("codigo_banco");
        //            if (!reader.IsDBNull(icodigo_banco)) entidad.codigo_banco = reader.GetInt32(icodigo_banco);

        //            int icodigo_tipo_documento = reader.GetOrdinal("codigo_tipo_documento");
        //            if (!reader.IsDBNull(icodigo_tipo_documento)) entidad.codigo_tipo_documento = reader.GetInt32(icodigo_tipo_documento);

        //            int inombre_tipo_documento = reader.GetOrdinal("nombre_tipo_documento");
        //            if (!reader.IsDBNull(inombre_tipo_documento)) entidad.nombre_tipo_documento = reader.GetString(inombre_tipo_documento);

        //            int inombre = reader.GetOrdinal("nombre");
        //            if (!reader.IsDBNull(inombre)) entidad.nombre = reader.GetString(inombre);

        //            int iapellido_paterno = reader.GetOrdinal("apellido_paterno");
        //            if (!reader.IsDBNull(iapellido_paterno)) entidad.apellido_paterno = reader.GetString(iapellido_paterno);

        //            int iapellido_materno = reader.GetOrdinal("apellido_materno");
        //            if (!reader.IsDBNull(iapellido_materno)) entidad.apellido_materno = reader.GetString(iapellido_materno);

        //            int inro_documento = reader.GetOrdinal("nro_documento");
        //            if (!reader.IsDBNull(inro_documento)) entidad.nro_documento = reader.GetString(inro_documento);

        //            int itelefono_fijo = reader.GetOrdinal("telefono_fijo");
        //            if (!reader.IsDBNull(itelefono_fijo)) entidad.telefono_fijo = reader.GetString(itelefono_fijo);

        //            int itelefono_celular = reader.GetOrdinal("telefono_celular");
        //            if (!reader.IsDBNull(itelefono_celular)) entidad.telefono_celular = reader.GetString(itelefono_celular);

        //            int icorreo_electronico = reader.GetOrdinal("correo_electronico");
        //            if (!reader.IsDBNull(icorreo_electronico)) entidad.correo_electronico = reader.GetString(icorreo_electronico);

        //            int inro_cuenta = reader.GetOrdinal("nro_cuenta");
        //            if (!reader.IsDBNull(inro_cuenta)) entidad.nro_cuenta = reader.GetString(inro_cuenta);

        //            int icodigo_interbancario = reader.GetOrdinal("codigo_interbancario");
        //            if (!reader.IsDBNull(icodigo_interbancario)) entidad.codigo_interbancario = reader.GetString(icodigo_interbancario);

        //            int ies_persona_juridica = reader.GetOrdinal("es_persona_juridica");
        //            if (!reader.IsDBNull(ies_persona_juridica)) entidad.es_persona_juridica = reader.GetBoolean(ies_persona_juridica);

        //            int iestado_registro = reader.GetOrdinal("estado_registro");
        //            if (!reader.IsDBNull(iestado_registro)) entidad.estado_registro = reader.GetBoolean(iestado_registro);

        //            int iEstado = reader.GetOrdinal("Estado");
        //            if (!reader.IsDBNull(iEstado)) entidad.Estado = reader.GetString(iEstado);

        //            int iusuario_registra = reader.GetOrdinal("usuario_registra");
        //            if (!reader.IsDBNull(iusuario_registra)) entidad.usuario_registra = reader.GetString(iusuario_registra);

        //            int ifecha_registra = reader.GetOrdinal("fecha_registra");
        //            if (!reader.IsDBNull(ifecha_registra)) entidad.fecha_registra = reader.GetDateTime(ifecha_registra);

        //            int iusuario_modifica = reader.GetOrdinal("usuario_modifica");
        //            if (!reader.IsDBNull(iusuario_modifica)) entidad.usuario_modifica = reader.GetString(iusuario_modifica);

        //            int ifecha_modifica = reader.GetOrdinal("fecha_modifica");
        //            if (!reader.IsDBNull(ifecha_modifica)) entidad.fecha_modifica = reader.GetDateTime(ifecha_modifica);

        //            int iApellidos = reader.GetOrdinal("Apellidos");
        //            if (!reader.IsDBNull(iApellidos)) entidad.Apellidos = reader.GetString(iApellidos);

        //            lista.Add(entidad);
        //            entidad = null;
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        if (oConexion.State == ConnectionState.Open) oConexion.Close();
        //        cmd.Dispose();
        //        oConexion.Dispose();
        //    }
        //    return lista;
        //}

        public List<personal_listado_dto> Listar(int codigoCanal, int codigoGrupo, int estadoPersonal, int sede, string nombre = "")
        {
            
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_personal_listar", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@p_codigo_canal", SqlDbType.Int).Value = codigoCanal;
            cmd.Parameters.Add("@p_codigo_grupo", SqlDbType.Int).Value = codigoGrupo;
            cmd.Parameters.Add("@p_estado_registro", SqlDbType.Int).Value = estadoPersonal;
            cmd.Parameters.Add("@p_nombre", SqlDbType.VarChar, 100).Value = nombre;
            cmd.Parameters.Add("@p_sede", SqlDbType.Int, 100).Value = sede;

            List<personal_listado_dto> lista = new List<personal_listado_dto>();
            try
            {
                oConexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    personal_listado_dto entidad = new personal_listado_dto();

                    int icodigo_personal = reader.GetOrdinal("codigo_personal");
                    if (!reader.IsDBNull(icodigo_personal)) entidad.codigo_personal = reader.GetInt32(icodigo_personal);

                    int inombre_tipo_documento = reader.GetOrdinal("nombre_tipo_documento");
                    if (!reader.IsDBNull(inombre_tipo_documento)) entidad.nombre_tipo_documento = reader.GetString(inombre_tipo_documento);

                    int inombre = reader.GetOrdinal("nombre");
                    if (!reader.IsDBNull(inombre)) entidad.nombre = reader.GetString(inombre);

                    int iapellidos = reader.GetOrdinal("apellidos");
                    if (!reader.IsDBNull(iapellidos)) entidad.apellidos = reader.GetString(iapellidos);

                    int inro_documento = reader.GetOrdinal("nro_documento");
                    if (!reader.IsDBNull(inro_documento)) entidad.nro_documento = reader.GetString(inro_documento);

                    int ies_supervisor_canal = reader.GetOrdinal("es_supervisor_canal");
                    if (!reader.IsDBNull(ies_supervisor_canal)) entidad.es_supervisor_canal = reader.GetBoolean(ies_supervisor_canal);

                    int ies_supervisor_grupo = reader.GetOrdinal("es_supervisor_grupo");
                    if (!reader.IsDBNull(ies_supervisor_grupo)) entidad.es_supervisor_grupo = reader.GetBoolean(ies_supervisor_grupo);

                    int iestado_registro = reader.GetOrdinal("estado_registro");
                    if (!reader.IsDBNull(iestado_registro)) entidad.estado_registro = reader.GetBoolean(iestado_registro);

                    int iSupervisor_Canal = reader.GetOrdinal("es_supervisor_canal");
                    if (!reader.IsDBNull(iSupervisor_Canal)) entidad.es_supervisor_canal = reader.GetBoolean(iSupervisor_Canal);

                    int iSupervisor_Grupo = reader.GetOrdinal("es_supervisor_grupo");
                    if (!reader.IsDBNull(iSupervisor_Grupo)) entidad.es_supervisor_grupo = reader.GetBoolean(iSupervisor_Grupo);

                    int iCanal = reader.GetOrdinal("nombre_canal");
                    if (!reader.IsDBNull(iCanal)) entidad.nombre_canal = reader.GetString(iCanal);

                    int iGrupo = reader.GetOrdinal("nombre_grupo");
                    if (!reader.IsDBNull(iGrupo)) entidad.nombre_grupo = reader.GetString(iGrupo);

                    int iCodigoEquivalencia = reader.GetOrdinal("codigo_equivalencia");
                    if (!reader.IsDBNull(iCodigoEquivalencia)) entidad.codigo_equivalencia = reader.GetString(iCodigoEquivalencia);

                    int iNombreCompleto = reader.GetOrdinal("nombre_completo");
                    if (!reader.IsDBNull(iNombreCompleto)) entidad.nombre_completo = reader.GetString(iNombreCompleto);

                    int ivalidado_texto = reader.GetOrdinal("validado_texto");
                    if (!reader.IsDBNull(ivalidado_texto)) entidad.validado = reader.GetString(ivalidado_texto);


                    lista.Add(entidad);
                    entidad = null;
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return lista;
        }

        public List<personal_x_canal_grupo_listado_dto> ListarPorCanalGrupo(int codigo_canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_listado_canal_grupo");

            personal_x_canal_grupo_listado_dto personal = new personal_x_canal_grupo_listado_dto();
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);

            List<personal_x_canal_grupo_listado_dto> lstPersonal = new List<personal_x_canal_grupo_listado_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    personal = new personal_x_canal_grupo_listado_dto();
                    personal.codigo_registro = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_registro"]);
                    personal.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                    personal.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    personal.fecha_creacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                    personal.usuario_creacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    personal.es_supervisor = DataUtil.DbValueToDefault<bool>(oIDataReader["es_supervisor"]);

                    lstPersonal.Add(personal);
                }
            }
            return lstPersonal;
        }

        public List<personal_planilla_listado_dto> ListarByPlanilla(personal_planilla_listado_dto v_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_planilla_listar");
            List<personal_planilla_listado_dto> lst = new List<personal_planilla_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, v_personal.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@nro_documento", DbType.String, v_personal.nro_documento);
                oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, v_personal.nombre);
                oDatabase.AddInParameter(oDbCommand, "@apellido_paterno", DbType.String, v_personal.apellido_paterno);
                oDatabase.AddInParameter(oDbCommand, "@apellido_materno", DbType.String, v_personal.apellido_materno);
                


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new personal_planilla_listado_dto();
                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);
                        v_entidad.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);

                        lst.Add(v_entidad);
                    }

                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }

        public List<personal_planilla_listado_dto> ListarByPlanillaEstado(int p_codigo_planilla, int p_codigo_estado_cuota)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_planilla_listar_by_estado_cuota");
            List<personal_planilla_listado_dto> lst = new List<personal_planilla_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, p_codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_estado_cuota", DbType.Int32, p_codigo_estado_cuota);


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new personal_planilla_listado_dto();
                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);
                        v_entidad.correo_electronico = DataUtil.DbValueToDefault<string>(oIDataReader["correo_electronico"]);
                        v_entidad.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);

                        lst.Add(v_entidad);
                    }

                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }

        public personal_dto CrearNuevo(int codigo_personal, int codigo_canal_grupo, string usuario)
        {
            personal_dto retorno = new personal_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_nuevo_codigo");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, usuario);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        retorno.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        retorno.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return retorno;
        }

        public List<personal_x_nombre_para_canal_grupo_dto> ListarPorNombresCanalGrupo(string nombres)
        {
            personal_x_nombre_para_canal_grupo_dto personal = new personal_x_nombre_para_canal_grupo_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_listado_por_nombres_canal_grupo");
            List<personal_x_nombre_para_canal_grupo_dto> lstPersonal = new List<personal_x_nombre_para_canal_grupo_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nombres", DbType.String, nombres);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        personal = new personal_x_nombre_para_canal_grupo_dto();
                        personal.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        personal.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        personal.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        personal.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        personal.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        personal.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);


                        lstPersonal.Add(personal);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lstPersonal;
        }

        public bool ExisteDocumento(personal_dto personal, bool tipo)
        {
            bool retorno = false;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_validacion_" + (tipo? "documento":"ruc"));
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, personal.codigo_personal);

            if (!tipo)
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nro_ruc", DbType.String, personal.nro_ruc);
            }
            else
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_documento", DbType.Int32, personal.codigo_tipo_documento);
                oDatabase.AddInParameter(oDbCommand, "@p_nro_documento", DbType.String, personal.nro_documento);
            }

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        public bool ExisteSupervisor(int codigo_personal, int codigo_canal_grupo, ref string mensaje )
        {
            bool retorno = false;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_validacion_supervisor");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.String, codigo_canal_grupo);

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    mensaje = "El canal/grupo " + DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal_grupo"]) + " tiene a " + DataUtil.DbValueToDefault<string>(oIDataReader["nombre_supervisor"]) + " como supervisor.";
                    retorno = true;
                }
            }

            return retorno;
        }

        public List<personal_comision_manual_listado_dto> ListarParaComisionManual(string nombre)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_listado_comision_manual");
            List<personal_comision_manual_listado_dto> lst = new List<personal_comision_manual_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, nombre);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new personal_comision_manual_listado_dto();
                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.nombres = DataUtil.DbValueToDefault<string>(oIDataReader["nombres"]);
                        v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);

                        lst.Add(v_entidad);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }

        public string ObtenerEquivalencia(int codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_codigo_equivalencia");
            string codigo_equivalencia = string.Empty;
            
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return codigo_equivalencia;
        }

        public List<reclamo_personal_listado_dto> ListarParaReclamos(string codigo_usuario)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reclamo_listar_personal");
            List<reclamo_personal_listado_dto> lst = new List<reclamo_personal_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_usuario", DbType.String, codigo_usuario);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var personal = new reclamo_personal_listado_dto();
                        personal.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        personal.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);
                        personal.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        personal.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        personal.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        personal.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        personal.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);

                        lst.Add(personal);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lst;
        }

        public List<personal_historico_validacion_dto> ListarHistoricoValidacion(int codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_historial_validacion_listado_por_codigo");
            List<personal_historico_validacion_dto> lst = new List<personal_historico_validacion_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var personal = new personal_historico_validacion_dto();

                        personal.id = DataUtil.DbValueToDefault<int>(oIDataReader["id"]);
                        personal.texto_registra = DataUtil.DbValueToDefault<string>(oIDataReader["texto_registra"]);
                        personal.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        personal.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        personal.texto_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["texto_modifica"]);
                        personal.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                        personal.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);

                        lst.Add(personal);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lst;
        }

        public List<personal_no_validado_dto> ListarNoValidados()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_listado_no_validados");
            List<personal_no_validado_dto> lst = new List<personal_no_validado_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var personal = new personal_no_validado_dto();

                        personal.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        personal.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);
                        personal.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        personal.documento = DataUtil.DbValueToDefault<string>(oIDataReader["documento"]);
                        personal.banco = DataUtil.DbValueToDefault<string>(oIDataReader["banco"]);
                        personal.tipo_cuenta = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_cuenta"]);
                        personal.nro_cuenta = DataUtil.DbValueToDefault<string>(oIDataReader["nro_cuenta"]);
                        personal.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                        personal.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
                        personal.tipo_validacion = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_validacion"]);
                        personal.canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["canal_grupo"]);

                        lst.Add(personal);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lst;
        }

        public List<personal_historico_bloqueo_dto> GetHistorialBloqueoJson(int codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_bloqueo_listado_por_codigo");
            List<personal_historico_bloqueo_dto> lst = new List<personal_historico_bloqueo_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var personal = new personal_historico_bloqueo_dto();

                        personal.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        personal.descripcion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]);
                        personal.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        personal.descripcion_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion_planilla"]);
                        personal.estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["estado_planilla"]);
                        personal.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        personal.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                        personal.estado_registro_texto = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro_texto"]);

                        lst.Add(personal);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lst;
        }

        public int GetCantidadBloqueo(int codigo_personal)
        {
            int cantidad = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_bloqueo_validacion_por_codigo");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    if (oIDataReader.Read())
                    {
                        cantidad = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return cantidad;
        }


        public void Validar(List<personal_no_validado_dto> lst_personal, string usuario_modifica)
        {
            var custColl = new PersonalNoValidadoTypeCollection();

            foreach (var item in lst_personal)
            {
                personal_type_dto type = new personal_type_dto();
                type.codigo_personal = item.codigo_personal;
                custColl.Add(type);
            }

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_validar");

            try
            {
                var parameter = new SqlParameter("@p_array_personal", SqlDbType.Structured)
                {
                    TypeName = "dbo.array_personal_type",
                    SqlValue = custColl.Count == 0 ? null : custColl
                };

                oDbCommand.Parameters.Add(parameter);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_valida", DbType.String, usuario_modifica);
                oDatabase.ExecuteNonQuery(oDbCommand);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void ActivarValidado(int codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_activar_validado");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

    }
}