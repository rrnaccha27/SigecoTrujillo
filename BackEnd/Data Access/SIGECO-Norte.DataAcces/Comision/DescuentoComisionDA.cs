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
    public class DescuentoComisionDA : GenericDA<DescuentoComisionDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<descuento_comision_listado_dto> Listar()
        {
            
            descuento_comision_listado_dto descuento_comision = new descuento_comision_listado_dto();
			List<descuento_comision_listado_dto> lstDescuento = new List<descuento_comision_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_listar");

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						descuento_comision = new descuento_comision_listado_dto { 
                            codigo_descuento_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_descuento_comision"]),
                            nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            monto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto"]),
                            saldo = DataUtil.DbValueToDefault<decimal>(oIDataReader["saldo"]),
                            nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]),
                            estado_registro = DataUtil.DbValueToDefault<int>(oIDataReader["estado_registro"]),
                            usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]),
                            fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"])
                        };
                        lstDescuento.Add(descuento_comision);
					}
				}
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
			
            return lstDescuento;
        }

        public descuento_comision_detalle_dto Detalle(descuento_comision_dto busqueda)
        {
            descuento_comision_detalle_dto descuento_comision = new descuento_comision_detalle_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_detalle");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_descuento_comision", DbType.Int32, busqueda.codigo_descuento_comision);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    if (oIDataReader.Read())
                    {
                        descuento_comision = new descuento_comision_detalle_dto
                        {
                            codigo_descuento_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_descuento_comision"]),
                            nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            monto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto"]),
                            saldo = DataUtil.DbValueToDefault<decimal>(oIDataReader["saldo"]),
                            nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]),
                            usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]),
                            fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]),
                            usuario_desactiva = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_desactiva"]),
                            fecha_desactiva = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_desactiva"]),
                            motivo = DataUtil.DbValueToDefault<string>(oIDataReader["motivo"])
                        };
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return descuento_comision;
        }

        public List<descuento_comision_planilla_dto> DetallePlanilla(descuento_comision_dto busqueda)
        {

            descuento_comision_planilla_dto descuento_comision = new descuento_comision_planilla_dto();
            List<descuento_comision_planilla_dto> lstDetalle = new List<descuento_comision_planilla_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_detalle_planilla_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_descuento_comision", DbType.Int32, busqueda.codigo_descuento_comision);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        descuento_comision = new descuento_comision_planilla_dto
                        {
                            nro_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nro_planilla"]),
                            fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]),
                            fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]),
                            nombre_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_planilla"]),
                            nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]),
                            estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["estado_planilla"]),
                            monto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto"]),
                            usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]),
                            fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"])
                        };
                        lstDetalle.Add(descuento_comision);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstDetalle;
        }

        public int Insertar(descuento_comision_dto descuento_comision)
        {
            int codigo_descuento_comision = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, descuento_comision.codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, descuento_comision.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_monto", DbType.Decimal, descuento_comision.monto);
            oDatabase.AddInParameter(oDbCommand, "@p_motivo", DbType.String, descuento_comision.motivo);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, descuento_comision.usuario);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_descuento_comision", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_descuento_comision").ToString();
                codigo_descuento_comision = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_descuento_comision;
        }

        public void Desactivar(descuento_comision_dto descuento_comision)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_desactivar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_descuento_comision", DbType.Int32, descuento_comision.codigo_descuento_comision);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, descuento_comision.usuario);

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

        public int Validar(descuento_comision_dto descuento_comision)
        {
            int codigo_descuento_comision = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_validar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, descuento_comision.codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, descuento_comision.codigo_empresa);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_descuento_comision", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_descuento_comision").ToString();
                codigo_descuento_comision = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_descuento_comision;
        }

        public int GenerarDescuento(descuento_comision_generar_dto descuento_comision)
        {
            int cantidad = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_generar_descuento");

            oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, descuento_comision.codigo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, descuento_comision.usuario);
            oDatabase.AddOutParameter(oDbCommand, "@p_cantidad", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_cantidad").ToString();
                cantidad = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return cantidad;
        }

        public int ValidarPlanilla(descuento_comision_generar_dto validacion)
        {
            int cantidad = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_comision_validar_planilla");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, validacion.codigo_planilla);
            oDatabase.AddOutParameter(oDbCommand, "@p_cantidad", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_cantidad").ToString();
                cantidad = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return cantidad;
        }

    }
}