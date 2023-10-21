using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
    public class PlanIntegralDA : GenericDA<PlanIntegralDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<plan_integral_listado_dto> Listar(int estado_registro)
        {
            
            plan_integral_listado_dto plan = new plan_integral_listado_dto();
			List<plan_integral_listado_dto> lstPlanes = new List<plan_integral_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Int32, estado_registro);

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						plan = new plan_integral_listado_dto();

                        plan.codigo_plan_integral = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_plan_integral"]);
						plan.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        plan.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        plan.estado_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_nombre"]);
                        plan.vigencia_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                        plan.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);

						lstPlanes.Add(plan);
					}
				}
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
			
            return lstPlanes;
        }

        public plan_integral_dto Unico(int codigo_plan_integral)
        {

            plan_integral_dto plan = new plan_integral_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_unique");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral", DbType.Int32, codigo_plan_integral);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        plan = new plan_integral_dto();

                        plan.codigo_plan_integral = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_plan_integral"]);
                        plan.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        plan.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        plan.vigencia_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                        plan.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return plan;
        }

        public int Insertar(plan_integral_dto plan)
        {
            int codigo_plan_integral = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, plan.nombre);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, plan.vigencia_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, plan.vigencia_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, plan.usuario);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_plan_integral", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_plan_integral").ToString();
                codigo_plan_integral = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_plan_integral;
        }

        public void Actualizar(plan_integral_dto plan)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_actualizar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral", DbType.Int32, plan.codigo_plan_integral);
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, plan.nombre);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, plan.vigencia_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, plan.vigencia_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, plan.usuario);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }


        public void Desactivar(plan_integral_dto plan)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_desactivar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral", DbType.Int32, plan.codigo_plan_integral);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, plan.usuario);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public bool Validar(int codigo_plan_integral)
        {
            bool retorno = false;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_validar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral", DbType.Int32, codigo_plan_integral);
            oDatabase.AddOutParameter(oDbCommand, "@p_resultado", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_resultado").ToString();
                retorno = (int.Parse(resultado1) == 1? true:false);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return retorno;
        }


    }
}