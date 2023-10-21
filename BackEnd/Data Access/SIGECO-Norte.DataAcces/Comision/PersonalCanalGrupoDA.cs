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
	public class PersonalCanalGrupoDA
	{
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<personal_canal_grupo_listado_dto> Listar(int codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);

            personal_canal_grupo_listado_dto personal_cg = new personal_canal_grupo_listado_dto();
            List<personal_canal_grupo_listado_dto> lstCanalGrupo = new List<personal_canal_grupo_listado_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    personal_cg = new personal_canal_grupo_listado_dto();
                    personal_cg.codigo_registro = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_registro"]);
                    personal_cg.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                    personal_cg.codigo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_canal"]);
                    personal_cg.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                    personal_cg.codigo_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_grupo"]);
                    personal_cg.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                    personal_cg.es_supervisor_canal = DataUtil.DbValueToDefault<string>(oIDataReader["es_supervisor_canal"]);
					personal_cg.es_supervisor_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["es_supervisor_grupo"]);
					personal_cg.percibe_comision = DataUtil.DbValueToDefault<string>(oIDataReader["percibe_comision"]);
                    personal_cg.percibe_bono = DataUtil.DbValueToDefault<string>(oIDataReader["percibe_bono"]);
                    personal_cg.confirmado = false;
                    personal_cg.estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro"]);

                    lstCanalGrupo.Add(personal_cg);
                }
            }
            return lstCanalGrupo;
        }

        public int Registrar(personal_canal_grupo_dto personal_cg)
        {
            int codigo_registro = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, personal_cg.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, personal_cg.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, personal_cg.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_es_supervisor_canal", DbType.Boolean, personal_cg.es_supervisor_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_es_supervisor_grupo", DbType.Boolean, personal_cg.es_supervisor_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_comision", DbType.Boolean, personal_cg.percibe_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_bono", DbType.Boolean, personal_cg.percibe_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, personal_cg.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_registro", DbType.Int32, 0);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_registro").ToString();
                codigo_registro = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_registro;
        }


        public void Actualizar(personal_canal_grupo_dto personal_cg)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_actualizar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_registro", DbType.Int32, personal_cg.codigo_registro);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, personal_cg.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, personal_cg.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, personal_cg.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_es_supervisor_canal", DbType.Boolean, personal_cg.es_supervisor_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_es_supervisor_grupo", DbType.Boolean, personal_cg.es_supervisor_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_comision", DbType.Boolean, personal_cg.percibe_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_bono", DbType.Boolean, personal_cg.percibe_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, personal_cg.usuario_modifica);
                oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Boolean, personal_cg.estado_registro);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

        }

        public void Desactivar(personal_canal_grupo_dto personal_cg)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_desactivar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_registro", DbType.Int32, personal_cg.codigo_registro);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, personal_cg.usuario_modifica);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void EliminarPorPersonal(string canalesEliminar)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_eliminar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_CanalesXML", DbType.Xml, canalesEliminar);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

        }

        public void AsignarSupervisor(int esCanalGrupo, personal_canal_grupo_dto canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_asignar_supervisor");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, canal_grupo.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, canal_grupo.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Boolean, esCanalGrupo);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_comision", DbType.Boolean, canal_grupo.percibe_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_bono", DbType.Boolean, canal_grupo.percibe_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, canal_grupo.usuario_modifica);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void AsignarPersonal(int esCanalGrupo, personal_canal_grupo_dto canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_asignar_personal");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, canal_grupo.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, canal_grupo.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Boolean, esCanalGrupo);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_comision", DbType.Boolean, canal_grupo.percibe_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_bono", DbType.Boolean, canal_grupo.percibe_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, canal_grupo.usuario_modifica);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void DesasignarSupervisor(personal_canal_grupo_dto canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_desasignar_supervisor");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, canal_grupo.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, canal_grupo.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, canal_grupo.usuario_modifica);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Transferir(int esCanalGrupo, int codigo_canal_crupo, personal_canal_grupo_dto canal_grupo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_transferir");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, canal_grupo.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, canal_grupo.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_comision", DbType.Boolean, canal_grupo.percibe_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_percibe_bono", DbType.Boolean, canal_grupo.percibe_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, canal_grupo.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo_old", DbType.Int32, codigo_canal_crupo);
                oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Boolean, esCanalGrupo);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public int GetOtrasSupervisiones(int codigo_personal, int codigo_canal_grupo)
        {
            int cantidad = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_cantidad");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);
                oDatabase.AddOutParameter(oDbCommand, "@p_cantidad_canales", DbType.Int32, 0);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_cantidad_canales").ToString();
                cantidad = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return cantidad;
        }

        public personal_canal_grupo_replicacion_dto BuscarParaReplicacion(int codigo_personal, int codigo_canal_grupo, string tipo_operacion)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_personal_canal_grupo_replicacion");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, codigo_canal_grupo);

            personal_canal_grupo_replicacion_dto personal_cg = new personal_canal_grupo_replicacion_dto();

            try
            { 
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        personal_cg.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia"]);
                        personal_cg.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        personal_cg.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        personal_cg.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        personal_cg.flag_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["flag_supervisor"]);
                        personal_cg.codigo_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_grupo"]);
                        personal_cg.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        personal_cg.codigo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_canal"]);
                        personal_cg.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        personal_cg.codigo_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_supervisor"]);
                        personal_cg.nombre_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_supervisor"]);
                        personal_cg.apellido_paterno_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno_supervisor"]);
                        personal_cg.apellido_materno_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno_supervisor"]);
                        personal_cg.tipo_operacion = tipo_operacion;
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return personal_cg;
        }


	}
}
