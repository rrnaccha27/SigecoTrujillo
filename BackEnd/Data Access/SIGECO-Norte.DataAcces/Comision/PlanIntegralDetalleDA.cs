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
    public class PlanIntegralDetalleDA : GenericDA<PlanIntegralDetalleDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<plan_integral_detalle_dto> Listar(int codigo_plan_integral)
        {
            
            plan_integral_detalle_dto detalle = new plan_integral_detalle_dto();
			List<plan_integral_detalle_dto> lstPlan = new List<plan_integral_detalle_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_detalle_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral", DbType.Int32, codigo_plan_integral);

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						detalle = new plan_integral_detalle_dto();

                        detalle.codigo_plan_integral_detalle = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_plan_integral_detalle"]);
                        detalle.codigo_campo_santo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_campo_santo"]);
                        detalle.codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]);
                        detalle.codigo_tipo_articulo_2 = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo_2"]);
                        detalle.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        detalle.estado_registro_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro_nombre"]);

						lstPlan.Add(detalle);
					}
				}
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
			
            return lstPlan;
        }

        //public plan_integral_detalle_dto Unico(int codigo_plan_integral_detalle)
        //{

        //    plan_integral_detalle_dto detalle = new plan_integral_detalle_dto();

        //    DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_detalle_unique");
        //    oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral_detalle", DbType.Int32, codigo_plan_integral_detalle);

        //    try
        //    {
        //        using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
        //        {
        //            while (oIDataReader.Read())
        //            {
        //                detalle = new plan_integral_detalle_dto();

        //                detalle.codigo_plan_integral_detalle = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_plan_integral_detalle"]);
        //                detalle.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
        //                detalle.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (oDbCommand != null) oDbCommand.Dispose();
        //        oDbCommand = null;
        //    }

        //    return detalle;
        //}

        public int Insertar(plan_integral_detalle_dto detalle)
        {
            int codigo_plan_integral_detalle = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_detalle_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral", DbType.String, detalle.codigo_plan_integral);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_campo_santo", DbType.String, detalle.codigo_campo_santo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_articulo", DbType.String, detalle.codigo_tipo_articulo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_articulo_2", DbType.String, detalle.codigo_tipo_articulo_2);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, detalle.usuario);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_plan_integral_detalle", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_plan_integral_detalle").ToString();
                codigo_plan_integral_detalle = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_plan_integral_detalle;
        }

        //public void Actualizar(plan_integral_detalle_dto plan)
        //{
        //    DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_detalle_actualizar");
        //    oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral_detalle", DbType.Int32, detalle.codigo_plan_integral_detalle);
        //    oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, detalle.nombre);
        //    oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, detalle.vigencia_inicio);
        //    oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, detalle.vigencia_fin);
        //    oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, detalle.usuario);

        //    try
        //    {
        //        oDatabase.ExecuteNonQuery(oDbCommand);
        //    }
        //    finally
        //    {
        //        if (oDbCommand != null) oDbCommand.Dispose();
        //        oDbCommand = null;
        //    }
        //}


        public void Desactivar(plan_integral_detalle_dto detalle)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_plan_integral_detalle_desactivar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_plan_integral_detalle", DbType.Int32, detalle.codigo_plan_integral_detalle);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, detalle.usuario);

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

    }
}