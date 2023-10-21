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
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;

namespace SIGEES.DataAcces
{
    public partial class ChecklistComisionDA : GenericDA<ChecklistComisionDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        #region SECCION OPERACIONES
        public void Aperturar(checklist_comision_estado_dto planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_aperturar");
            oDbCommand.CommandTimeout = 0;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, planilla.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, planilla.usuario);
                
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Cerrar(checklist_comision_estado_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_cerrar");
            oDbCommand.CommandTimeout = 0;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, pEntidad.codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Anular(checklist_comision_estado_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_anular");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, pEntidad.codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Validar(List<checklist_comision_detalle_listado_dto> checklist_detalle, string usuario_modifica)
        {
            var custColl = new ChecklistComisionTypeCollection();

            foreach (var item in checklist_detalle)
            {
                checklist_comision_type_dto type = new checklist_comision_type_dto();
                type.codigo_checklist_detalle = item.codigo_checklist_detalle;
                custColl.Add(type);
            }

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_detalle_validar");

            try
            {
                var parameter = new SqlParameter("@p_array_checklist_comision_detalle", SqlDbType.Structured)
                {
                    TypeName = "dbo.array_checklist_comision_detalle_type",
                    SqlValue = custColl.Count == 0 ? null : custColl
                };

                oDbCommand.Parameters.Add(parameter);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, usuario_modifica);
                oDatabase.ExecuteNonQuery(oDbCommand);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }
        #endregion

    }

    public partial class ChecklistComisionSelDA : GenericDA<ChecklistComisionSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        #region SECCION LISTADOS BUSQUEDA

        public List<checklist_comision_listado_dto> Listar()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_listar");
            List<checklist_comision_listado_dto> lst = new List<checklist_comision_listado_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new checklist_comision_listado_dto();

                        v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                        v_entidad.numero_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["numero_checklist"]);

                        v_entidad.codigo_regla_tipo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_checklist"]);
                        v_entidad.nombre_regla_tipo_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_checklist"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_estado_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_checklist"]);
                        v_entidad.nombre_estado_checklist= DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_checklist"]);

                        v_entidad.estilo = DataUtil.DbValueToDefault<string>(oIDataReader["estilo"]);

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

        public checklist_comision_listado_dto Detalle(int codigo_checklist)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_por_codigo");
            var v_entidad = new checklist_comision_listado_dto();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    if (oIDataReader.Read())
                    {
                        v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                        v_entidad.numero_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["numero_checklist"]);

                        v_entidad.codigo_regla_tipo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_checklist"]);
                        v_entidad.nombre_regla_tipo_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_checklist"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_estado_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_checklist"]);
                        v_entidad.nombre_estado_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_checklist"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return v_entidad;
        }


        public List<checklist_comision_detalle_listado_dto> ListarDetalle(int codigo_checklist, bool validado)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_comision_detalle_listado");
            List<checklist_comision_detalle_listado_dto> lst = new List<checklist_comision_detalle_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_validado", DbType.Int32, validado?1:0);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new checklist_comision_detalle_listado_dto();

                        v_entidad.codigo_checklist_detalle = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist_detalle"]);
                        v_entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        v_entidad.validado = DataUtil.DbValueToDefault<int>(oIDataReader["validado"]);
                        v_entidad.validado_texto = DataUtil.DbValueToDefault<string>(oIDataReader["validado_texto"]);
                        v_entidad.importe_abono_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_abono_personal"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                        v_entidad.ruc_personal = DataUtil.DbValueToDefault<string>(oIDataReader["ruc_personal"]);

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



        #endregion
    }
}
