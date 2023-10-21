using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SIGEES.DataAcces
{
	public class CanalGrupoDA
	{
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<canal_grupo_dto> Listar_Canal()
        {
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_canal_grupo_LISTAR_Canal", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@codigo_padre", SqlDbType.Int).Value = codigo_padre;
            List<canal_grupo_dto> lista = new List<canal_grupo_dto>();
            try
            {
                oConexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    canal_grupo_dto entidad = new canal_grupo_dto();

                    int iid = reader.GetOrdinal("codigo_canal_grupo");
                    if (!reader.IsDBNull(iid)) entidad.id = reader.GetInt32(iid);

                    int itext = reader.GetOrdinal("nombre");
                    if (!reader.IsDBNull(itext)) entidad.text = reader.GetString(itext);

                    int icodigo_canal_grupo = reader.GetOrdinal("codigo_canal_grupo");
                    if (!reader.IsDBNull(icodigo_canal_grupo)) entidad.codigo_canal_grupo = reader.GetInt32(icodigo_canal_grupo);

                    int ies_canal_grupo = reader.GetOrdinal("es_canal_grupo");
                    if (!reader.IsDBNull(ies_canal_grupo)) entidad.es_canal_grupo = reader.GetBoolean(ies_canal_grupo);

                    int inombre = reader.GetOrdinal("nombre");
                    if (!reader.IsDBNull(inombre)) entidad.nombre = reader.GetString(inombre);

                    int icodigo_padre = reader.GetOrdinal("codigo_padre");
                    if (!reader.IsDBNull(icodigo_padre)) entidad.codigo_padre = reader.GetInt32(icodigo_padre);

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
        public List<canal_grupo_dto> Listar_Grupo(Int32 codigo_padre)
        {
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_canal_grupo_LISTAR_Grupo", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_padre", SqlDbType.Int).Value = codigo_padre;
            List<canal_grupo_dto> lista = new List<canal_grupo_dto>();
            try
            {
                oConexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    canal_grupo_dto entidad = new canal_grupo_dto();

                    int iid = reader.GetOrdinal("codigo_canal_grupo");
                    if (!reader.IsDBNull(iid)) entidad.id = reader.GetInt32(iid);

                    int itext = reader.GetOrdinal("nombre");
                    if (!reader.IsDBNull(itext)) entidad.text = reader.GetString(itext);

                    int icodigo_canal_grupo = reader.GetOrdinal("codigo_canal_grupo");
                    if (!reader.IsDBNull(icodigo_canal_grupo)) entidad.codigo_canal_grupo = reader.GetInt32(icodigo_canal_grupo);

                    int ies_canal_grupo = reader.GetOrdinal("es_canal_grupo");
                    if (!reader.IsDBNull(ies_canal_grupo)) entidad.es_canal_grupo = reader.GetBoolean(ies_canal_grupo);

                    int inombre = reader.GetOrdinal("nombre");
                    if (!reader.IsDBNull(inombre)) entidad.nombre = reader.GetString(inombre);

                    int icodigo_padre = reader.GetOrdinal("codigo_padre");
                    if (!reader.IsDBNull(icodigo_padre)) entidad.codigo_padre = reader.GetInt32(icodigo_padre);
                    
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

        public List<canal_grupo_listado_dto> Listar(bool esCanalGrupo, int codigoPadre)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_canal_grupo_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Boolean, esCanalGrupo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_padre", DbType.Int32, codigoPadre);

            canal_grupo_listado_dto canal_grupo = new canal_grupo_listado_dto();
            List<canal_grupo_listado_dto> lstCanalGrupo = new List<canal_grupo_listado_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    canal_grupo = new canal_grupo_listado_dto();
                    canal_grupo.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                    canal_grupo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    canal_grupo.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                    canal_grupo.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                    canal_grupo.s_percibe_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["s_percibe_comision"]);
                    canal_grupo.s_percibe_bono = DataUtil.DbValueToDefault<bool>(oIDataReader["s_percibe_bono"]);
                    canal_grupo.p_percibe_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["p_percibe_comision"]);
                    canal_grupo.p_percibe_bono = DataUtil.DbValueToDefault<bool>(oIDataReader["p_percibe_bono"]);
                    canal_grupo.administra_grupos = DataUtil.DbValueToDefault<bool>(oIDataReader["administra_grupos"]);
                    canal_grupo.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    canal_grupo.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);

                    lstCanalGrupo.Add(canal_grupo);
                }
            }
            return lstCanalGrupo;
        }

        public List<canal_grupo_personal_dto> ListarPersonal(int es_canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_canal_grupo_registro_personal");

            canal_grupo_personal_dto canal_grupo = new canal_grupo_personal_dto();
            oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Int32, es_canal_grupo);

            List<canal_grupo_personal_dto> lstCanalGrupo = new List<canal_grupo_personal_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    canal_grupo = new canal_grupo_personal_dto();
                    canal_grupo.codigo_padre = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_padre"]);
                    canal_grupo.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                    canal_grupo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    canal_grupo.supervisor_percibe_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["supervisor_percibe_comision"]);
                    canal_grupo.supervisor_percibe_bono = DataUtil.DbValueToDefault<bool>(oIDataReader["supervisor_percibe_bono"]);
                    canal_grupo.personal_percibe_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["personal_percibe_comision"]);
                    canal_grupo.personal_percibe_bono = DataUtil.DbValueToDefault<bool>(oIDataReader["personal_percibe_bono"]);

                    lstCanalGrupo.Add(canal_grupo);
                }
            }
            return lstCanalGrupo;
        }

        public canal_grupo_detalle_dto Detalle(int codigo_canal_grupo) 
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_canal_grupo_detalle");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);
            canal_grupo_detalle_dto canal_grupo = new canal_grupo_detalle_dto();


            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    canal_grupo.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                    canal_grupo.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);
                    canal_grupo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    canal_grupo.administra_grupos = DataUtil.DbValueToDefault<bool>(oIDataReader["administra_grupos"]);
                    canal_grupo.s_percibe_comision = DataUtil.DbValueToDefault<int>(oIDataReader["s_percibe_comision"]);
                    canal_grupo.s_percibe_bono = DataUtil.DbValueToDefault<int>(oIDataReader["s_percibe_bono"]);
                    canal_grupo.p_percibe_comision = DataUtil.DbValueToDefault<int>(oIDataReader["p_percibe_comision"]);
                    canal_grupo.p_percibe_bono = DataUtil.DbValueToDefault<int>(oIDataReader["p_percibe_bono"]);
                    canal_grupo.s_c_empresa_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["s_c_empresa_planilla"]);
                    canal_grupo.s_c_empresa_factura = DataUtil.DbValueToDefault<string>(oIDataReader["s_c_empresa_factura"]);
                    canal_grupo.s_b_empresa_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["s_b_empresa_planilla"]);
                    canal_grupo.s_b_empresa_factura = DataUtil.DbValueToDefault<string>(oIDataReader["s_b_empresa_factura"]);
                    canal_grupo.p_c_empresa_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["p_c_empresa_planilla"]);
                    canal_grupo.p_c_empresa_factura = DataUtil.DbValueToDefault<string>(oIDataReader["p_c_empresa_factura"]);
                    canal_grupo.p_b_empresa_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["p_b_empresa_planilla"]);
                    canal_grupo.p_b_empresa_factura = DataUtil.DbValueToDefault<string>(oIDataReader["p_b_empresa_factura"]);
                    canal_grupo.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                }
            }


            return canal_grupo;
        }

        public void EliminarConfiguracion(int codigo_canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_configuracion_canal_grupo_eliminar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);
            canal_grupo_detalle_dto canal_grupo = new canal_grupo_detalle_dto();


            oDatabase.ExecuteNonQuery(oDbCommand);
        }

        public bool ExisteCodigoEquivalencia(int codigo_canal_grupo, string codigo_equivalencia)
        {
            bool retorno = false;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_canal_grupo_validar_codigo_equivalencia");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_equivalencia", DbType.String, codigo_equivalencia);

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        public List<canal_grupo_combo_dto> Listar_Canal_Planilla_Bono(Boolean es_planilla_jn = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_proceso_generacion_bono_obtener_canal");
            oDatabase.AddInParameter(oDbCommand, "@p_es_planilla_jn", DbType.Boolean, es_planilla_jn);

            List<canal_grupo_combo_dto> lista = new List<canal_grupo_combo_dto>();
            canal_grupo_combo_dto canal_grupo;

            try
            { 
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        canal_grupo = new canal_grupo_combo_dto();

                        canal_grupo.id = DataUtil.DbValueToDefault<int>(oIDataReader["codigo"]);
                        canal_grupo.text = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);

                        lista.Add(canal_grupo);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }


            return lista;
        }

        public List<canal_jefatura_dto> ListarJefatura()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_canal_jefatura_listado");

            canal_jefatura_dto elemento = new canal_jefatura_dto();
            List<canal_jefatura_dto> listado = new List<canal_jefatura_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    elemento = new canal_jefatura_dto();
                    elemento.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                    elemento.email = DataUtil.DbValueToDefault<string>(oIDataReader["email"]);
                    elemento.email_copia = DataUtil.DbValueToDefault<string>(oIDataReader["email_copia"]);
                    elemento.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);

                    listado.Add(elemento);
                }
            }
            return listado;
        }

	}
}
