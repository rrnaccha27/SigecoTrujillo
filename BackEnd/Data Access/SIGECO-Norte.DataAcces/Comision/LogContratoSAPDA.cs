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
    public class LogContratoSAPDA : GenericDA<LogContratoSAPDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<log_contrato_sap_listado_dto> Listar(log_contrato_sap_fechas_dto busqueda,int pSede)
        {
            
            log_contrato_sap_listado_dto contrato = new log_contrato_sap_listado_dto();
			List<log_contrato_sap_listado_dto> lstContratos = new List<log_contrato_sap_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_contrato_sap_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_inicio", DbType.String, busqueda.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fin", DbType.String, busqueda.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, pSede);
            

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
                    while (oIDataReader.Read())
                    {
                        contrato = new log_contrato_sap_listado_dto {
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            codigo_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_empresa"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            fecha_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_contrato"]),
                            fecha_migracion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_migracion"]),
                            fecha_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_proceso"]),
                            estado = DataUtil.DbValueToDefault<string>(oIDataReader["estado"]),
                            usuario = DataUtil.DbValueToDefault<string>(oIDataReader["usuario"]),
                            codigo_estado_proceso = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_proceso"]),
                            codigo_empresa_sigeco = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa_sigeco"]),
                            identificador_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["identificador_contrato"])
                        };
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

        public log_contrato_sap_detalle_dto Detalle(string codigo_empresa, string nro_contrato)
        {

            log_contrato_sap_detalle_dto contrato = new log_contrato_sap_detalle_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_contrato_sap_detalle");
            oDatabase.AddInParameter(oDbCommand, "@p_Codigo_empresa", DbType.String, codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_NumAtCard", DbType.String, nro_contrato);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        contrato = new log_contrato_sap_detalle_dto
                        {
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            personal = DataUtil.DbValueToDefault<string>(oIDataReader["personal"]),
                            nro_articulos = DataUtil.DbValueToDefault<int>(oIDataReader["nro_articulos"]),
                            nro_cuotas = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuotas"]),
                            fecha_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_contrato"]),
                            fecha_migracion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_migracion"]),
                            estado = DataUtil.DbValueToDefault<string>(oIDataReader["estado"]),
                            fecha_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_proceso"]),
                            observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]),
                            tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_venta"]),
                            tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_pago"])
                        };
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return contrato;
        }

        public log_contrato_sap_fechas_dto Fechas()
        {

            log_contrato_sap_fechas_dto fechas = new log_contrato_sap_fechas_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_contrato_sap_fechas");

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        fechas = new log_contrato_sap_fechas_dto
                        { 
                            fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]),
                            fecha_fin= DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"])
                        };
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

        public MensajeDTO HabilitarReproceso(string procesar, string usuario)
        {
            MensajeDTO mensaje = new MensajeDTO();
            mensaje.mensaje = string.Empty;
            mensaje.idOperacion = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_contrato_sap_habilitar_reproceso");
            oDatabase.AddInParameter(oDbCommand, "@p_contratosXML", DbType.Xml, procesar);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_usuario", DbType.String, usuario);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        mensaje.mensaje = DataUtil.DbValueToDefault<string>(oIDataReader["mensaje"]);
                        if (mensaje.mensaje.Length > 0)
                            mensaje.idOperacion = -1;
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return mensaje;
        }

        public int Bloquear(log_contrato_sap_bloqueo_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_contrato_sap_bloqueo_reproceso");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, pEntidad.nro_contrato);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_bloqueo", DbType.Int32, pEntidad.bloqueo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_usuario_bloqueo", DbType.String, pEntidad.usuario_bloqueo);
                oDatabase.AddInParameter(oDbCommand, "@p_motivo", DbType.String, pEntidad.motivo);

                oDatabase.ExecuteNonQuery(oDbCommand);

                resultado = 1;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return resultado;
        }

    }
}