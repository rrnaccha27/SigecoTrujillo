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
    public class ComisionManualDA : GenericDA<ComisionManualDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<comision_manual_listado_dto> Listar(comision_manual_filtro_dto filtro)
        {
            comision_manual_listado_dto comision = new comision_manual_listado_dto();
			List<comision_manual_listado_dto> lstComisiones = new List<comision_manual_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, filtro.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, filtro.codigo_canal_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.String, filtro.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.String, filtro.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, filtro.usuario);
            oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Int16, filtro.estado_registro);

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						comision = new comision_manual_listado_dto();

                        comision.codigo_comision_manual = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_comision_manual"]);
                        comision.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        comision.nombre_fallecido = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_fallecido"]);
                        comision.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        comision.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        comision.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        comision.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        comision.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        comision.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        comision.nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);
                        comision.nombre_estado_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_proceso"]);
                        comision.nro_factura_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["nro_factura_vendedor"]);
                        comision.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);

                        lstComisiones.Add(comision);
					}
				}
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
			
            return lstComisiones;
        }

        public comision_manual_dto Detalle(int codigo_comision_manual)
        {

            comision_manual_dto comision = new comision_manual_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_detalle");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision_manual", DbType.Int32, codigo_comision_manual);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        comision = new comision_manual_dto();

                        comision.codigo_comision_manual = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_comision_manual"]);
                        comision.codigo_tipo_documento = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_documento"]);
                        comision.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);
                        comision.nombre_fallecido = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        comision.apellido_paterno_fallecido = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        comision.apellido_materno_fallecido = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        comision.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        comision.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        comision.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        comision.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        comision.codigo_empresa = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_empresa"]);

                        comision.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        comision.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);


                        comision.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        comision.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);
                        
                        comision.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        comision.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                        comision.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        comision.comentario = DataUtil.DbValueToDefault<string>(oIDataReader["comentario"]);
                        comision.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_sin_igv"]);
                        comision.monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        comision.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision"]);
                        comision.nro_factura_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["nro_factura_vendedor"]);
                        comision.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        comision.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        comision.nombre_estado_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_proceso"]);
                        comision.nombre_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_planilla"]);
                        comision.nombre_completo_fallecido = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_completo_fallecido"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return comision;
        }

        public string Insertar(comision_manual_dto comision)
        {
            string retorno = string.Empty;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_insertar");
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_estado_cuota", DbType.Int32, comision.codigo_estado_cuota);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_documento", DbType.Int32, comision.codigo_tipo_documento);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_documento", DbType.String, comision.nro_documento);
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, comision.nombre_fallecido);
            oDatabase.AddInParameter(oDbCommand, "@p_apellido_paterno", DbType.String, comision.apellido_paterno_fallecido);
            oDatabase.AddInParameter(oDbCommand, "@p_apellido_materno", DbType.String, comision.apellido_materno_fallecido);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, comision.codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, comision.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, comision.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, comision.nro_contrato);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, comision.codigo_articulo);
            oDatabase.AddInParameter(oDbCommand, "@p_comentario", DbType.String, comision.comentario);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_venta", DbType.Int32, comision.codigo_tipo_venta);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_pago", DbType.Int32, comision.codigo_tipo_pago);
            oDatabase.AddInParameter(oDbCommand, "@p_comision_sin_igv", DbType.Decimal, comision.monto_bruto);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_factura_vendedor", DbType.String, comision.nro_factura_vendedor);
            oDatabase.AddInParameter(oDbCommand, "@p_igv", DbType.Decimal, comision.monto_igv);
            oDatabase.AddInParameter(oDbCommand, "@p_comision", DbType.Decimal, comision.monto_neto);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, comision.usuario_registra);
            oDatabase.AddOutParameter(oDbCommand, "@p_retorno", DbType.String, 200);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_retorno").ToString();
                retorno = resultado1.ToString();

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return retorno;
        }

        public string Actualizar(comision_manual_dto comision)
        {
            string retorno = string.Empty;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_actualizar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision_manual", DbType.Int32, comision.codigo_comision_manual);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_estado_cuota", DbType.Int32, comision.codigo_estado_cuota);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_documento", DbType.Int32, comision.codigo_tipo_documento);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_documento", DbType.String, comision.nro_documento);
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, comision.nombre_fallecido);
            oDatabase.AddInParameter(oDbCommand, "@p_apellido_paterno", DbType.String, comision.apellido_paterno_fallecido);
            oDatabase.AddInParameter(oDbCommand, "@p_apellido_materno", DbType.String, comision.apellido_materno_fallecido);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, comision.codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, comision.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, comision.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, comision.nro_contrato);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, comision.codigo_articulo);
            oDatabase.AddInParameter(oDbCommand, "@p_comentario", DbType.String, comision.comentario);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_venta", DbType.Int32, comision.codigo_tipo_venta);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_pago", DbType.Int32, comision.codigo_tipo_pago);
            oDatabase.AddInParameter(oDbCommand, "@p_comision_sin_igv", DbType.Decimal, comision.monto_bruto);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_factura_vendedor", DbType.String, comision.nro_factura_vendedor);
            oDatabase.AddInParameter(oDbCommand, "@p_igv", DbType.Decimal, comision.monto_igv);
            oDatabase.AddInParameter(oDbCommand, "@p_comision", DbType.Decimal, comision.monto_neto);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, comision.usuario_modifica);
            oDatabase.AddOutParameter(oDbCommand, "@p_retorno", DbType.String, 200);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_retorno").ToString();
                retorno = resultado1.ToString();
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return retorno;
        }

        public void ActualizarLimitado(comision_manual_dto comision)
        {
            string retorno = string.Empty;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_actualizar_limitado");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision_manual", DbType.Int32, comision.codigo_comision_manual);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_factura_vendedor", DbType.String, comision.nro_factura_vendedor);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, comision.usuario_modifica);

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

        public MensajeDTO Desactivar(comision_manual_dto comision)
        {
            MensajeDTO mensaje = new MensajeDTO();
            mensaje.codigoError = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_desactivar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision_manual", DbType.Int32, comision.codigo_comision_manual);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, comision.usuario_modifica);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        mensaje.mensaje = DataUtil.DbValueToDefault<string>(oIDataReader["mensaje"]);
                        mensaje.codigoError = -1;
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

        public MensajeDTO Validar(comision_manual_dto comision)
        {
            MensajeDTO mensaje = new MensajeDTO();
            mensaje.codigoError = 0;
            mensaje.mensaje = string.Empty;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_validar");

            oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision_manual", DbType.Int32, comision.codigo_comision_manual);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_documento", DbType.Int32, comision.codigo_tipo_documento);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_documento", DbType.String, comision.nro_documento);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, comision.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, comision.nro_contrato);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, comision.codigo_articulo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, comision.codigo_personal);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        mensaje.mensaje = DataUtil.DbValueToDefault<string>(oIDataReader["mensaje"]);
                        mensaje.codigoError = -1;
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

        public List<comision_manual_reporte_dto> Reporte(comision_manual_filtro_dto filtro)
        {
            comision_manual_reporte_dto comision = new comision_manual_reporte_dto();
            List<comision_manual_reporte_dto> lstComisiones = new List<comision_manual_reporte_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_reporte");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, filtro.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, filtro.codigo_canal_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.String, filtro.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.String, filtro.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, filtro.usuario_registra);
            oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Int16, filtro.estado_registro);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        comision = new comision_manual_reporte_dto();

                        comision.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        comision.empresa = DataUtil.DbValueToDefault<string>(oIDataReader["empresa"]);
                        comision.tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_venta"]);
                        comision.tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_pago"]);
                        comision.vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["vendedor"]);
                        comision.canal = DataUtil.DbValueToDefault<string>(oIDataReader["canal"]);
                        comision.articulo = DataUtil.DbValueToDefault<string>(oIDataReader["articulo"]);
                        comision.comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision"]);
                        comision.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        comision.nro_factura_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["nro_factura_vendedor"]);
                        comision.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);

                        lstComisiones.Add(comision);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstComisiones;
        }

        public comision_manual_reporte_param_dto ReporteParam(comision_manual_filtro_dto filtro)
        {
            comision_manual_reporte_param_dto comision = new comision_manual_reporte_param_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_reporte_param");
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.String, filtro.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.String, filtro.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, filtro.usuario);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        comision = new comision_manual_reporte_param_dto();

                        comision.fecha_impresion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_impresion"]);
                        comision.fechas = DataUtil.DbValueToDefault<string>(oIDataReader["fechas"]);
                        comision.usuario = DataUtil.DbValueToDefault<string>(oIDataReader["usuario"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return comision;
        }

        public string ValidarReferencia(int codigo_comision_manual)
        {
            string mensaje = string.Empty;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_manual_validar_referencia");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision_manual", DbType.Int32, codigo_comision_manual);
            oDatabase.AddOutParameter(oDbCommand, "@p_mensaje", DbType.String, 250);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_mensaje").ToString();

                mensaje = resultado1.ToString();
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return mensaje;
        }

    }
}