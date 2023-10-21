using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{
    public partial class DetallePlanillaDA : GenericDA<DetallePlanillaDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public int Excluir(detalle_planilla_dto pEntidad)
        {
            int codigo_detalle_cronograma = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_excluir");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_planilla", DbType.Int32, pEntidad.codigo_detalle_planilla);
                oDatabase.AddInParameter(oDbCommand, "@observacion", DbType.String, pEntidad.observacion);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_permanente", DbType.Boolean, pEntidad.excluido);
                oDatabase.AddOutParameter(oDbCommand, "@p_error", DbType.String, 200);
                oDatabase.AddOutParameter(oDbCommand, "@p_excluyo", DbType.Boolean, 1);
                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_detalle_cronograma = pEntidad.codigo_detalle_cronograma;

                //var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_error").ToString();
                //retorno = Convert.ToInt32(resultado1.ToString());
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle_cronograma;
        }
    
        public void Excluir(detalle_planilla_exclusion_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_excluir_analisis_comision");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_XmlDetalleCronograma", DbType.Xml, pEntidad.procesarXML);
                oDatabase.AddInParameter(oDbCommand, "@p_motivo", DbType.String, pEntidad.motivo);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_permanente", DbType.Boolean, pEntidad.permanente);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }
    }

    public partial class DetallePlanillaSelDA : GenericDA<DetallePlanillaSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<detalle_planilla_resumen_dto> ListarByIdPlanilla(detalle_planilla_resumen_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_by_id");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, v_filtro.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, v_filtro.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_personal", DbType.Int32, v_filtro.codigo_personal);                                
                
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);
                        
                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);

                        
                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);

                        v_entidad.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        v_entidad.fecha_pago = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_pago"]);
                        v_entidad.str_fecha_pago = v_entidad.fecha_pago.ToShortDateString();


                        v_entidad.apellidos_nombres = v_entidad.nombre_persona + " " + v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

                        v_entidad.nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo_canal"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);


                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        v_entidad.es_registro_manual_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["es_registro_manual_comision"]);
                        v_entidad.es_transferencia = DataUtil.DbValueToDefault<Int32>(oIDataReader["es_transferencia"]);

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

        public List<detalle_planilla_resumen_dto> ListarComisionManualByIdPlanilla(detalle_planilla_resumen_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_by_id_comision_manual");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, v_filtro.codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);

                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);


                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);

                        v_entidad.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        v_entidad.fecha_pago = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_pago"]);
                        v_entidad.str_fecha_pago = v_entidad.fecha_pago.ToShortDateString();


                        v_entidad.apellidos_nombres = v_entidad.nombre_persona + " " + v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

                        v_entidad.nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo_canal"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);


                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        v_entidad.es_registro_manual_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["es_registro_manual_comision"]);
                        v_entidad.es_transferencia = DataUtil.DbValueToDefault<Int32>(oIDataReader["es_transferencia"]);
                        v_entidad.personal_comision_manual = DataUtil.DbValueToDefault<string>(oIDataReader["personal_cm"]);
                        v_entidad.usuario_comision_manual = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cm"]);

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

        public List<detalle_planilla_resumen_dto> ListarDetalleByIdPlanilla(int p_codigo_planilla)
        {

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_listar_by_id");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, p_codigo_planilla);               

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);

                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);


                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);

                        v_entidad.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        v_entidad.fecha_pago = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_pago"]);
                        v_entidad.str_fecha_pago = v_entidad.fecha_pago.ToShortDateString();


                        v_entidad.apellidos_nombres = v_entidad.nombre_persona + " " + v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

                        v_entidad.nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo_canal"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);


                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);

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


        public List<lista_descuento_dto> ListarDescuentoByIdPlanilla(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_descuento_by_planilla");
            List<lista_descuento_dto> lst = new List<lista_descuento_dto>();



            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, p_codigo_planilla);
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new lista_descuento_dto();
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_descuento = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_descuento"]);
                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.motivo = DataUtil.DbValueToDefault<string>(oIDataReader["motivo"]);
                        v_entidad.monto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto"]);
                        v_entidad.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);

                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        
                        
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        
                        v_entidad.apellidos_nombres = v_entidad.nombre +  " " +v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

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



        public List<detalle_planilla_resumen_dto> ObtenerSaldoPersonalByPlanilla(int p_codigo_planilla, int p_codigo_personal, int p_codigo_estado_cuota)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_saldo_personal_by_estado");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            detalle_planilla_resumen_dto v_entidad = null;


            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, p_codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_personal", DbType.Int32, p_codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@codigo_estado_cuota", DbType.Int32, p_codigo_estado_cuota);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        v_entidad=new detalle_planilla_resumen_dto();

                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.monto_descuento = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_descuento"]);
                        v_entidad.tiene_descuento = DataUtil.DbValueToDefault<int>(oIDataReader["tiene_descuento"])==1;
                        v_entidad.monto_afecto_descuento = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_afecto_descuento"]);
                        v_entidad.motivo = DataUtil.DbValueToDefault<string>(oIDataReader["motivo"]);
                        
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

        public List<detalle_planilla_resumen_dto> ListarPagoHabilitadoByIdPlanilla(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_pago_habilitados_by_id");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();



            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);               

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto();

                        v_entidad.codigo_planilla = p_codigo_planilla;
                        v_entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                        v_entidad.nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);

                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.monto_descuento = DataUtil.DbValueToDefault<decimal>(oIDataReader["descuento"]);
                        v_entidad.comision_total = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_total"]);

                        v_entidad.apellidos_nombres = v_entidad.nombre_persona + " " + v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

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

        public List<detalle_planilla_resumen_dto> ListarByIdPlanillaBono(int p_codigo_planilla, Boolean es_planilla_jn = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_bono_by_id");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto {
                            codigo_planilla = p_codigo_planilla,
                            codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]),
                            nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]),
                            nombre_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_moneda"]),
                            nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]),
                            codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            //v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                            //v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                            //v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);
                            monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]),
                            igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]),
                            codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]),
                            nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]),
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            apellidos_nombres = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"])
                        };

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

        public List<detalle_planilla_bono_trimestral_dto> ListarByIdPlanillaBonoTrimestral(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_trimestral_detalle_by_id");
            List<detalle_planilla_bono_trimestral_dto> lst = new List<detalle_planilla_bono_trimestral_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_bono_trimestral_dto
                        {
                            nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]),
                            nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]),
                            nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]),
                            nombre_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_supervisor"]),
                            monto_contratado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_contratado"]),
                            rango = DataUtil.DbValueToDefault<int>(oIDataReader["rango"]),
                            monto_bono = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bono"])
                        };

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

        public List<detalle_planilla_resumen_dto> ListarExcluidosByIdPlanillaBono(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_bono_excluido_by_id");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto
                        {
                            codigo_planilla = p_codigo_planilla,
                            codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]),
                            nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]),
                            nombre_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_moneda"]),
                            nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]),
                            codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]),
                            igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]),
                            codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]),
                            nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]),
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            apellidos_nombres = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]),
                            motivo_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["excluido_motivo"]),
                            usuario_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["excluido_usuario"]),
                            fecha_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["excluido_fecha"])
                        };

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

        public List<detalle_planilla_resumen_dto> ListarDetallePlanillaExcluidoByIdPlanilla(int codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_planilla_excluido_by_id");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, codigo_planilla);               


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);

                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);


                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);

                        v_entidad.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        v_entidad.fecha_pago = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_pago"]);
                        v_entidad.str_fecha_pago = v_entidad.fecha_pago.ToShortDateString();


                        v_entidad.apellidos_nombres = v_entidad.nombre_persona+" "+v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

                        v_entidad.nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo_canal"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);


                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

                        v_entidad.usuario_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_exclusion"]);
                        v_entidad.motivo_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_exclusion"]);
                        
                        v_entidad.usuario_habilita_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_habilita"]);
                        v_entidad.motivo_habilitacion_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_habilita"]);
                        v_entidad.nombre_estado_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_exclusion"]);
                        v_entidad.es_transferencia = DataUtil.DbValueToDefault<Int32>(oIDataReader["es_transferencia"]);


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

        //MYJ - 20171124
        public List<detalle_planilla_resumen_dto> IncluirListar(string nro_contrato, int codigo_planilla)
        {

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_insertar_inclusion_listar");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                //oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, "");
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, nro_contrato);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@p_tipo_ejecucion", DbType.Boolean, false);
                //oDatabase.AddOutParameter(oDbCommand, "@p_total_registro_procesado", DbType.Int32,0);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_planilla_resumen_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);

                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);


                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);

                        v_entidad.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        //v_entidad.fecha_pago = 
                        v_entidad.str_fecha_pago = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_pago"]);


                        v_entidad.apellidos_nombres = v_entidad.nombre_persona + " " + v_entidad.apellido_paterno + " " + v_entidad.apellido_materno;

                        v_entidad.nombre_grupo_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo_canal"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);


                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        v_entidad.es_registro_manual_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["es_registro_manual_comision"]);


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

        //MYJ - 20171124
        public int IncluirProcesar(int codigo_planilla, List<detalle_planilla_inclusion_dto>lst_inclusion, string usuario)
        {
            int retorno = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_insertar_inclusion");
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                StringBuilder xmlContratos = new StringBuilder();

                xmlContratos.Append("<contratos>");
                foreach(var elemento in lst_inclusion)
                {
                    xmlContratos.Append("<contrato codigo_detalle_cronograma='" + elemento.codigo_detalle_cronograma.ToString() + "'  />");
                }
                xmlContratos.Append("</contratos>");

                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, usuario);
                oDatabase.AddInParameter(oDbCommand, "@p_contratos", DbType.Xml, xmlContratos.ToString());
                oDatabase.AddOutParameter(oDbCommand, "@p_total_registro_procesado", DbType.Int32, 0);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_total_registro_procesado").ToString();

                retorno = Convert.ToInt32(resultado1.ToString());
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return retorno;
        }

        #region Planilla Bono Trimestral

        public DataTable ReporteBonoTrimestralLiquidacion(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_bono_trimestral_liquidacion");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    dt.Load(oIDataReader);
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }

        public List<detalle_liquidacion_planilla_bono_trimestral_dto> ReporteBonoTrimestralLiquidacionLst(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_bono_trimestral_liquidacion");
            List<detalle_liquidacion_planilla_bono_trimestral_dto> lst = new List<detalle_liquidacion_planilla_bono_trimestral_dto>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_liquidacion_planilla_bono_trimestral_dto();

		                v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
		                v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
		                v_entidad.nombre_empresa_largo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_largo"]);
		                v_entidad.direccion_fiscal_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["direccion_fiscal_empresa"]);
		                v_entidad.ruc_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["ruc_empresa"]);
		                v_entidad.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
		                v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
		                v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
		                v_entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
		                v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
		                v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
		                v_entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
		                v_entidad.documento_personal = DataUtil.DbValueToDefault<string>(oIDataReader["documento_personal"]);
		                v_entidad.codigo_personal_j = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_personal_j"]);
		                v_entidad.codigo_supervisor = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_supervisor"]);
                        v_entidad.correo_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["correo_supervisor"]);
		                v_entidad.nombre_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_supervisor"]);
		                v_entidad.monto_contratado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_contratado"]);
		                v_entidad.rango = DataUtil.DbValueToDefault<int>(oIDataReader["rango"]);
		                v_entidad.monto_bono = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bono"]);
		                v_entidad.monto_sin_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_sin_igv"]);
		                v_entidad.monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv"]);
		                v_entidad.monto_bono_letras = DataUtil.DbValueToDefault<string>(oIDataReader["monto_bono_letras"]);
		                v_entidad.concepto_liquidacion = DataUtil.DbValueToDefault<string>(oIDataReader["concepto_liquidacion"]);
                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
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


        public DataTable ReporteBonoTrimestralPlanilla(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_bono_trimestral_planilla");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    dt.Load(oIDataReader);
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }

        #endregion
    }
}
