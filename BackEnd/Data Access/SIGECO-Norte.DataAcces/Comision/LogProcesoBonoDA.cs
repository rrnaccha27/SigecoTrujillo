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
    public class LogProcesoBonoDA : GenericDA<LogProcesoBonoDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<log_proceso_bono_listado_dto> Listar(string fecha_inicio, string fecha_fin)
        {
            
            log_proceso_bono_listado_dto proceso = new log_proceso_bono_listado_dto();
			List<log_proceso_bono_listado_dto> lstProcesos = new List<log_proceso_bono_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_proceso_bono_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.String, fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.String, fecha_fin);

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						proceso = new log_proceso_bono_listado_dto();

                        proceso.id = DataUtil.DbValueToDefault<string>(oIDataReader["id"]);
                        proceso.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        proceso.nro_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nro_planilla"]);
                        proceso.canal = DataUtil.DbValueToDefault<string>(oIDataReader["canal"]);
                        proceso.tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_planilla"]);
                        proceso.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        proceso.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);
                        proceso.usuario = DataUtil.DbValueToDefault<string>(oIDataReader["usuario"]);
                        proceso.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);

						lstProcesos.Add(proceso);
					}
				}
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
			
            return lstProcesos;
        }

        public List<log_proceso_bono_detalle_dto> Detalle(int codigo_planilla)
        {

            log_proceso_bono_detalle_dto contrato = new log_proceso_bono_detalle_dto();
            List<log_proceso_bono_detalle_dto> lstContratos = new List<log_proceso_bono_detalle_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_proceso_bono_detalle");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.String, codigo_planilla);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        contrato = new log_proceso_bono_detalle_dto();

                        contrato.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        contrato.empresa = DataUtil.DbValueToDefault<string>(oIDataReader["empresa"]);
                        contrato.canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["canal_grupo"]);
                        contrato.estado_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_nombre"]);
                        contrato.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);
                        contrato.codigo_estado = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado"]);
                        contrato.monto_ingresado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_ingresado"]);

                        lstContratos.Add(contrato);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstContratos;
        }

        public log_proceso_bono_fechas_dto Fechas()
        {

            log_proceso_bono_fechas_dto fechas = new log_proceso_bono_fechas_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_proceso_bono_fechas");

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        fechas = new log_proceso_bono_fechas_dto();

                        fechas.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        fechas.fecha_fin= DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return fechas;
        }

    }
}