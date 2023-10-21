using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{
    public partial class PlanillaDA : GenericDA<PlanillaDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);
        #region SECCION PLANILLA ADMINISTRATIVA Y COMERCIAL
        public int Generar(Planilla_dto pEntidad, out int p_cantidad_registro_procesado)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_comision_trujillo_insertar");
            oDbCommand.CommandTimeout = 0;
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_tipo_planilla", DbType.Int32, pEntidad.codigo_regla_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, pEntidad.fecha_inicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, pEntidad.fecha_fin);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                oDatabase.AddOutParameter(oDbCommand, "@codigo_planilla", DbType.Int32, 20);
                oDatabase.AddOutParameter(oDbCommand, "@total_registro_procesado", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@codigo_planilla").ToString();
                var resultado2 = oDatabase.GetParameterValue(oDbCommand, "@total_registro_procesado").ToString();
                p_cantidad_registro_procesado = int.Parse(resultado2);
                codigo_planilla = int.Parse(resultado1);



            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_planilla;
        }
        public int Cerrar(Planilla_dto pEntidad)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_cerrar");
            oDbCommand.CommandTimeout = 0;
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.ExecuteNonQuery(oDbCommand);
                codigo_planilla = pEntidad.codigo_planilla;



            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_planilla;
        }
        public int Registrar_Descuento(descuento_dto pEntidad)
        {
            int codigo_descuento = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_descuento_insertar");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_descuento", DbType.Int32, pEntidad.codigo_descuento);
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_personal", DbType.Int32, pEntidad.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@motivo", DbType.String, pEntidad.motivo);
                oDatabase.AddInParameter(oDbCommand, "@monto", DbType.Decimal, pEntidad.monto);
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.String, pEntidad.estado_registro);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_descuento", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_descuento").ToString();
                codigo_descuento = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_descuento;
        }
        public int Desactivar_Descuento(descuento_dto pEntidad)
        {
            int codigo_descuento = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_descuento_desactivar");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_descuento", DbType.Int32, pEntidad.codigo_descuento);
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.String, pEntidad.estado_registro);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_descuento", DbType.Int32, 20);
                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_descuento").ToString();
                codigo_descuento = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_descuento;
        }
        public int Anular(Planilla_dto pEntidad)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_anular");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.ExecuteNonQuery(oDbCommand);
                codigo_planilla = pEntidad.codigo_planilla;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_planilla;
        }
        #endregion
        #region SECCION PLANILLA BONO
        public int AnularPlanillaBono(Planilla_bono_dto pEntidad)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_anular");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.ExecuteNonQuery(oDbCommand);
                codigo_planilla = pEntidad.codigo_planilla;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_planilla;
        }
        public int CerrarPlanillaBono(Planilla_bono_dto pEntidad)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_cerrar");
            oDbCommand.CommandTimeout = 0;
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.ExecuteNonQuery(oDbCommand);
                codigo_planilla = pEntidad.codigo_planilla;



            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_planilla;
        }
        public int GenerarPlanillaBono(Planilla_bono_dto pEntidad, out int p_cantidad_registro_procesado, Boolean es_planilla_jn = false)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_insertar_trujillo");
            oDbCommand.CommandTimeout = 0;
            try
            {

                //oDatabase.AddInParameter(oDbCommand, "@codigo_regla_tipo_planilla", DbType.Int32, pEntidad.codigo_regla_tipo_planilla);

                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, pEntidad.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, pEntidad.codigo_canal);

                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, pEntidad.fecha_inicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, pEntidad.fecha_fin);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
//                oDatabase.AddInParameter(oDbCommand, "@p_es_planilla_jn", DbType.Boolean, es_planilla_jn);

                /*************************/
                oDatabase.AddOutParameter(oDbCommand, "@codigo_planilla", DbType.Int32, 20);
                oDatabase.AddOutParameter(oDbCommand, "@total_registro_procesado", DbType.Int32, 20);
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@codigo_planilla").ToString();
                var resultado2 = oDatabase.GetParameterValue(oDbCommand, "@total_registro_procesado").ToString();
                p_cantidad_registro_procesado = int.Parse(resultado2);
                codigo_planilla = int.Parse(resultado1);



            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_planilla;
        }

        public void AnularPagoBono(detalle_planilla_bono_exclusion_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_anular_pago");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla_bono", DbType.Int32, pEntidad.codigo_planilla_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, pEntidad.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, pEntidad.nro_contrato);
                oDatabase.AddInParameter(oDbCommand, "@p_excluido_motivo", DbType.String, pEntidad.excluido_motivo);
                oDatabase.AddInParameter(oDbCommand, "@p_excluido_usuario", DbType.String, pEntidad.excluido_usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        #endregion
        #region PLANILLA BONO TRIMESTRAL

        public int GenerarPlanillaBonoTrimestral(planilla_bono_trimestral_dto pEntidad, out int p_cantidad_registro_procesado)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_trimestral_insertar");
            oDbCommand.CommandTimeout = 0;

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, pEntidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_periodo", DbType.Int32, pEntidad.codigo_periodo);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, pEntidad.usuario_registra);

                /*************************/
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, 20);
                oDatabase.AddOutParameter(oDbCommand, "@p_total_registro_procesado", DbType.Int32, 20);
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_planilla").ToString();
                var resultado2 = oDatabase.GetParameterValue(oDbCommand, "@p_total_registro_procesado").ToString();
                p_cantidad_registro_procesado = int.Parse(resultado2);
                codigo_planilla = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_planilla;
        }

        public int AnularPlanillaBonoTrimestral(planilla_bono_trimestral_dto pEntidad)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_trimestral_anular");
            
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_anulacion", DbType.String, pEntidad.usuario_anulacion);
                oDatabase.ExecuteNonQuery(oDbCommand);
                codigo_planilla = pEntidad.codigo_planilla;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            
            return codigo_planilla;
        }

        public int CerrarPlanillaBonoTrimestral(planilla_bono_trimestral_dto pEntidad)
        {
            int codigo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_trimestral_cerrar");
            oDbCommand.CommandTimeout = 0;
            
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_cerrar", DbType.String, pEntidad.usuario_cierre);
                oDatabase.ExecuteNonQuery(oDbCommand);
                codigo_planilla = pEntidad.codigo_planilla;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_planilla;
        }
        
        #endregion
    }

    public partial class PlanillaSelDA : GenericDA<PlanillaSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<Planilla_dto> Listar()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_listar");
            List<Planilla_dto> lst = new List<Planilla_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, 5);
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new Planilla_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.nombre_regla_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_planilla"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        v_entidad.tipo_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);

                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.estado_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);

                        v_entidad.fecha_registro = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);

                        v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);
                        v_entidad.estilo = DataUtil.DbValueToDefault<string>(oIDataReader["estilo"]);

                        v_entidad.envio_liquidacion = DataUtil.DbValueToDefault<bool>(oIDataReader["envio_liquidacion"]);

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

        public Planilla_dto BuscarById(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_buscar_by_id");
            var v_entidad = new Planilla_dto();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                        v_entidad.nombre_regla_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_planilla"]);

                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);


                        v_entidad.fecha_anulacion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        v_entidad.tipo_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);

                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.estado_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);

                        v_entidad.fecha_registro = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);

                        v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);

                        v_entidad.envio_liquidacion = DataUtil.DbValueToDefault<bool>(oIDataReader["envio_liquidacion"]);
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

        public DataTable ReporteCabeceraPlanilla(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_planilla");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, p_codigo_planilla);

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

        public List<reporte_detalle_planilla> ReporteDetallePlanilla(int p_codigo_planilla, int p_codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_planilla_detalle_v2");
            List<reporte_detalle_planilla> lst = new List<reporte_detalle_planilla>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, p_codigo_personal);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var entidad = new reporte_detalle_planilla();
                        entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        entidad.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);

                        entidad.codigo_moneda = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_moneda"]);
                        entidad.moneda = DataUtil.DbValueToDefault<string>(oIDataReader["moneda"]);
                        entidad.articulo = DataUtil.DbValueToDefault<string>(oIDataReader["articulo"]);
                        entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);

                        entidad.tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_venta"]);
                        entidad.tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_pago"]);
                        entidad.empresa = DataUtil.DbValueToDefault<string>(oIDataReader["empresa"]);
                        entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);

                        entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        //entidad.codigo_supervisor = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_supervisor"]);

                        entidad.canal = DataUtil.DbValueToDefault<string>(oIDataReader["canal"]);
                        entidad.grupo = DataUtil.DbValueToDefault<string>(oIDataReader["grupo"]);
                        entidad.personal = DataUtil.DbValueToDefault<string>(oIDataReader["personal"]);
                        entidad.personal_referencial = DataUtil.DbValueToDefault<string>(oIDataReader["personal_referencial"]);

                        entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);


                        entidad.monto_bruto_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto_empresa"]);
                        entidad.igv_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv_empresa"]);
                        entidad.monto_neto_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_empresa"]);

                        entidad.monto_bruto_canal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto_canal"]);
                        entidad.igv_canal = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv_canal"]);
                        entidad.monto_neto_canal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_canal"]);
                        entidad.monto_bruto_grupo = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto_grupo"]);
                        entidad.igv_grupo = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv_grupo"]);
                        entidad.monto_neto_grupo = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_grupo"]);
                        entidad.monto_bruto_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto_personal"]);
                        entidad.igv_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv_personal"]);
                        entidad.monto_neto_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_personal"]);
                        entidad.monto_descuento = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_descuento"]);
                        entidad.monto_neto_personal_con_descuento = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_personal_con_descuento"]);
                        entidad.monto_detraccion_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_detraccion_personal"]);
                        entidad.monto_neto_pagar_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_pagar_personal"]);
                        entidad.es_comision_manual = (DataUtil.DbValueToDefault<bool>(oIDataReader["es_comision_manual"])?1:0);
                        entidad.fecha_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_contrato"]);
                        entidad.usuario_comision_manual = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cm"]);
                        entidad.tipo_reporte = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_reporte"]);

                        lst.Add(entidad);
                    }
                    //dt.Load(oIDataReader);
                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }



        public List<reporte_detalle_liquidacion> ReporteDetalleLiquidacionPlanilla(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_detalle_v2");
            List<reporte_detalle_liquidacion> lst = new List<reporte_detalle_liquidacion>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var entidad = new reporte_detalle_liquidacion();


                        entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);

                        entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        entidad.nombre_empresa_largo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_largo"]);
                        entidad.ruc = DataUtil.DbValueToDefault<string>(oIDataReader["ruc"]);
                        entidad.direccion_fiscal = DataUtil.DbValueToDefault<string>(oIDataReader["direccion_fiscal"]);

                        entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        entidad.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);

                        entidad.codigo_personal_referencial = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal_referencial"]);
                        entidad.nombre_personal_referencial = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal_referencial"]);
                        
                        entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                        entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                         entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);

                        entidad.igv_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv_empresa"]);
                        entidad.bruto_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["bruto_empresa"]);
                        entidad.neto_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["neto_empresa"]);

                        entidad.descuento_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["descuento_empresa"]);
                        entidad.detraccion_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["detraccion_empresa"]);
                        entidad.neto_pagar_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["neto_pagar_empresa"]);
                        entidad.neto_pagar_empresa_letra = DataUtil.DbValueToDefault<string>(oIDataReader["neto_pagar_empresa_letra"]);
                        entidad.email_personal = DataUtil.DbValueToDefault<string>(oIDataReader["email_personal"]);
                        entidad.email_personal_referencial = DataUtil.DbValueToDefault<string>(oIDataReader["email_personal_referencial"]);


                        entidad.nombre_envio_correo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_envio_correo"]);
                        entidad.apellido_envio_correo = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_envio_correo"]);

                        entidad.canal_grupo_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["canal_grupo_nombre"]);
                        entidad.descuento_motivo= DataUtil.DbValueToDefault<string>(oIDataReader["descuento_motivo"]);
                        entidad.codigo_jardines = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_jardines"]);

                        entidad.codigo_moneda = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_moneda"]);
                        entidad.tipo_reporte = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_reporte"]);

                        entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        lst.Add(entidad);
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

        /**/
        public List<reporte_liquidacion_Supervisor> ReporteDetalleLiquidacionPlanillaSupervisor(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_detalle_v2_Contabilidad");
            List<reporte_liquidacion_Supervisor> lst = new List<reporte_liquidacion_Supervisor>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var entidad = new reporte_liquidacion_Supervisor();

                        entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

                        entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        entidad.nombre_empresa_largo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_largo"]);
                        entidad.ruc = DataUtil.DbValueToDefault<string>(oIDataReader["ruc"]);
                        entidad.direccion_fiscal = DataUtil.DbValueToDefault<string>(oIDataReader["direccion_fiscal"]);

                        entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        entidad.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);

                        entidad.neto_pagar_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["neto_pagar_empresa"]);
                        entidad.email_personal = DataUtil.DbValueToDefault<string>(oIDataReader["email_personal"]);


                        entidad.nombre_envio_correo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_envio_correo"]);
                        entidad.apellido_envio_correo = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_envio_correo"]);

                        entidad.canal_grupo_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["canal_grupo_nombre"]);

                        entidad.tipo_reporte = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_reporte"]);

                        entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        lst.Add(entidad);
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

        public List<reporte_liquidacion_Supervisor> ReporteLiquidacionPlanillaBonoSupervisor(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_individual_v2_contabilidad");
            List<reporte_liquidacion_Supervisor> lst = new List<reporte_liquidacion_Supervisor>();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var entidad = new reporte_liquidacion_Supervisor();

                        entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

                        entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        entidad.nombre_empresa_largo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_largo"]);
                        entidad.ruc = DataUtil.DbValueToDefault<string>(oIDataReader["ruc"]);
                        entidad.direccion_fiscal = DataUtil.DbValueToDefault<string>(oIDataReader["direccion_fiscal"]);

                        entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        entidad.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);

                        entidad.neto_pagar_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["neto_pagar_empresa"]);
                        entidad.email_personal = DataUtil.DbValueToDefault<string>(oIDataReader["email_personal"]);


                        entidad.nombre_envio_correo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_envio_correo"]);
                        entidad.apellido_envio_correo = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_envio_correo"]);

                        entidad.canal_grupo_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["canal_grupo_nombre"]);

                        entidad.tipo_reporte = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_reporte"]);

                        entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        lst.Add(entidad);
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
        /**/

        public Planilla_bono_dto BuscarPlanillaBonoById(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_buscar_by_id");
            var v_entidad = new Planilla_bono_dto();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        //v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                        v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.fecha_anulacion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);
                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);
                        v_entidad.fecha_cierre = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);
                        v_entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        v_entidad.tipo_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.estado_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);
                        v_entidad.fecha_registro = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);
                        v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);
                        v_entidad.codigos_canales = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_canales"]);
                        v_entidad.es_planilla_jn = DataUtil.DbValueToDefault<int>(oIDataReader["es_planilla_jn"]);
                        v_entidad.envio_liquidacion = DataUtil.DbValueToDefault<bool>(oIDataReader["envio_liquidacion"]);
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

        public planilla_bono_trimestral_dto BuscarPlanillaBonoTrimestralById(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_trimestral_buscar_by_id");
            var v_entidad = new planilla_bono_trimestral_dto();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                        v_entidad.nombre_regla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla"]);
                        v_entidad.codigo_periodo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_periodo"]);
                        v_entidad.fecha_anulacion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);
                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);
                        v_entidad.fecha_cierre = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);
                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.estado_planilla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);
                        v_entidad.fecha_registro = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);
                        v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
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

        public List<grilla_planilla_bono> ListarPlanillaBono(Boolean es_planilla_jn = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(es_planilla_jn? "up_planilla_bono_jn_listar" : "up_planilla_bono_listar");
            List<grilla_planilla_bono> lst = new List<grilla_planilla_bono>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, 5);
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new grilla_planilla_bono();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.fecha_inicio = Fechas.convertDateToString(DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]));
                        v_entidad.fecha_fin = Fechas.convertDateToString(DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]));
                        v_entidad.fecha_apertura = Fechas.convertDateToString(DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_apertura"]));
                        v_entidad.fecha_cierre = Fechas.convertDateToString(DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]));
                        v_entidad.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        v_entidad.nombre_estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);
                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.estilo = DataUtil.DbValueToDefault<string>(oIDataReader["estilo"]);

                        if (!es_planilla_jn)
                        {
                            v_entidad.envio_liquidacion = DataUtil.DbValueToDefault<bool>(oIDataReader["envio_liquidacion"]);
                        }
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

        public List<grilla_planilla_bono_trimestral> ListarPlanillaBonoTrimestral()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_trimestral_listar");
            List<grilla_planilla_bono_trimestral> lst = new List<grilla_planilla_bono_trimestral>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new grilla_planilla_bono_trimestral();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.anio_periodo = DataUtil.DbValueToDefault<int>(oIDataReader["anio_periodo"]);
                        v_entidad.periodo = DataUtil.DbValueToDefault<string>(oIDataReader["periodo"]);
                        v_entidad.fecha_apertura = Fechas.convertDateToString(DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_apertura"]));
                        v_entidad.fecha_cierre = Fechas.convertDateToString(DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]));
                        v_entidad.nombre_tipo_bono = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_bono"]);
                        v_entidad.nombre_regla_bono = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_bono"]);
                        v_entidad.nombre_estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);
                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
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


        public DataTable ReportePlanillaBonoPersonal(int p_codigo_planilla,int p_codigo_personal )
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_repote_planilla_bono_personal_V2");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, p_codigo_personal);

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

        public DataTable ReporteDetallePlanillaComercial(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_comercial_exportar");
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

        public DataTable ReporteLiquidacionBonoPlanillaPersonal(int p_codigo_planilla, int p_codigo_personal)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_personal_v2");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, p_codigo_personal);

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

        public DataTable ReporteLiquidacionBonoPlanillaArticulos(int p_codigo_planilla, int p_codigo_empresa, string p_nro_contrato)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_articulos_v2");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, p_codigo_empresa);
                //oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, p_nro_contrato);

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

        public List<grilla_planilla_exclusion_dto> ListarPlanillaAbiertaGestionExclusion()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_gestion_exclusion_listar_planilla_abierta");
            List<grilla_planilla_exclusion_dto> lst = new List<grilla_planilla_exclusion_dto>();
            try
            {

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new grilla_planilla_exclusion_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                        v_entidad.str_fecha_inicio = v_entidad.fecha_inicio.ToShortDateString();
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);
                        v_entidad.str_fecha_fin = v_entidad.fecha_fin.ToShortDateString();


                        //v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        v_entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                        v_entidad.nombre_regla_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_planilla"]);
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

        public List<grilla_cuota_pago_planilla_dto> ListarPagoComisionVsPlanillaAbiertaGestionExclusion(List<collection_id_exclusion_dto> v_lst_id_exclusion)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_gestion_exclusion_listar_detalle_pago_comision");
            List<grilla_cuota_pago_planilla_dto> lst = new List<grilla_cuota_pago_planilla_dto>();
            try
            {
                ExclusionIdTypeCollection custColl = new ExclusionIdTypeCollection();
                foreach (var item in v_lst_id_exclusion)
                {
                    custColl.Add(item);
                }
                var parameter = new SqlParameter("@arrya_exclusion_id", SqlDbType.Structured)
                {
                    TypeName = "dbo.arrya_exclusion_id_type",
                    SqlValue = custColl
                };
                oDbCommand.Parameters.Add(parameter);
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new grilla_cuota_pago_planilla_dto();

                        v_entidad.codigo_exclusion = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_exclusion"]);
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);
                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronogrma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);


                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);



                        v_entidad.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);
                        v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                        v_entidad.apellidos_nombres = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres"]);

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                        v_entidad.str_fecha_inicio = v_entidad.fecha_inicio.ToShortDateString();
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);
                        v_entidad.str_fecha_fin = v_entidad.fecha_fin.ToShortDateString();
                        v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        v_entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);

                        v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                        v_entidad.nombre_regla_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_planilla"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

                        v_entidad.indica_modificar = v_entidad.codigo_planilla == 0 ? 0 : 1;

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

        public DataTable ReportePlanillaBonoSupervisor(int p_codigo_planilla, int p_codigo_personal)
        {
            //DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_planilla_bono_supervisor");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_planilla_bono_supervisor_general_v2");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@v_codigo_personal", DbType.Int32, p_codigo_personal);

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

        public DataTable ReporteLiquidacionBonoIndividual(int p_codigo_planilla, int p_codigo_personal)
        {


            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_individual_v2");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@v_codigo_personal", DbType.Int32, p_codigo_personal);

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

        public List<reporte_detalle_liquidacion_bono> ReporteLiquidacionBonoIndividual(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_individual_v2");
            List<reporte_detalle_liquidacion_bono> lst = new List<reporte_detalle_liquidacion_bono>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new reporte_detalle_liquidacion_bono();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.concepto = DataUtil.DbValueToDefault<string>(oIDataReader["concepto"]);
                        v_entidad.porcentaje_detraccion = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_detraccion"]);
                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.nombre_empresa_largo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_largo"]);
                        v_entidad.direccion_fiscal = DataUtil.DbValueToDefault<string>(oIDataReader["direccion_fiscal"]);
                        v_entidad.ruc = DataUtil.DbValueToDefault<string>(oIDataReader["ruc"]);
                        v_entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);
                        v_entidad.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        v_entidad.codigo_supervisor = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_supervisor"]);
                        v_entidad.apellidos_nombres_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_supervisor"]);
                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.monto_bruto_empresa_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto_empresa_supervisor"]);
                        v_entidad.monto_igv_empresa_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv_empresa_supervisor"]);
                        v_entidad.monto_neto_empresa_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_empresa_supervisor"]);
                        v_entidad.monto_detraccion_empresa_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_detraccion_empresa_supervisor"]);
                        v_entidad.monto_neto_pagar_empresa_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_pagar_empresa_supervisor"]);
                        v_entidad.neto_pagar_empresa_supervisor_letra = DataUtil.DbValueToDefault<string>(oIDataReader["neto_pagar_empresa_supervisor_letra"]);
                        v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        v_entidad.nombre_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_supervisor"]);
                        v_entidad.email_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["email_supervisor"]);

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


        public DataTable ReporteLiquidacionBonoSupervisor(int p_codigo_planilla, int p_codigo_personal)
        {
            
            //DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_supervisor");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_supervisor_general_v2");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, p_codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@v_codigo_personal", DbType.Int32, p_codigo_personal);

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

        public DataTable ReporteResumenEmpresaPlanilla(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_planilla_resumen_empresa");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, p_codigo_planilla);

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

        public List<cabecera_txt_dto> GetReportePlanillaParaTxt(int p_codigo_planilla, bool antiguo = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(antiguo ? "up_detalle_planilla_txt_v2" : "up_detalle_planilla_txt_v3");
            oDatabase.AddInParameter(oDbCommand, (antiguo ? "@p_codigo_planilla" : "@p_codigo_checklist"), DbType.Int32, p_codigo_planilla);
            List<cabecera_txt_dto> lst = new List<cabecera_txt_dto>();
            try
            {

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {

                        var v_entidad = new cabecera_txt_dto();
                        
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.fecha_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_proceso"]);
                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        ///v_entidad.simbolo_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["simbolo_moneda"]);
                        
                        v_entidad.numero_cuenta_abono = DataUtil.DbValueToDefault<string>(oIDataReader["numero_cuenta_abono"]);
                        v_entidad.tipo_cuenta_abono = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_cuenta_abono"]);
                        v_entidad.moneda_cuenta_abono = DataUtil.DbValueToDefault<string>(oIDataReader["simbolo_moneda_cuenta_abono"]);
                        
                        v_entidad.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        v_entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);
                        v_entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.importe_abono = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_abono_personal"]);
                        
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.tipo_cuenta_desembolso = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_cuenta_desembolso"]);
                        v_entidad.numero_cuenta_desembolso = DataUtil.DbValueToDefault<string>(oIDataReader["numero_cuenta_desembolso"]);
                        v_entidad.moneda_cuenta_desembolso = DataUtil.DbValueToDefault<string>(oIDataReader["simbolo_moneda_cuenta_desembolso"]);
                        v_entidad.importe_desembolso_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_desembolso_empresa"]);
                        v_entidad.calcular_detraccion = DataUtil.DbValueToDefault<bool>(oIDataReader["calcular_detraccion"]);
                        v_entidad.checksum = DataUtil.DbValueToDefault<string>(oIDataReader["checksum"]);
                        v_entidad.codigo_agrupador = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_agrupador"]);

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

        public List<cabecera_txt_dto> GetPlanillaBonoParaTxt(int p_codigo_planilla, bool antiguo = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(antiguo ? "up_detalle_planilla_bono_txt_v2" : "up_detalle_planilla_bono_txt_v3");
            oDatabase.AddInParameter(oDbCommand, antiguo ? "@p_codigo_planilla": "@p_codigo_checklist", DbType.Int32, p_codigo_planilla);
            List<cabecera_txt_dto> lst = new List<cabecera_txt_dto>();
            try
            {

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {

                        var v_entidad = new cabecera_txt_dto();

                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.fecha_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_proceso"]);
                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        ///v_entidad.simbolo_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["simbolo_moneda"]);

                        v_entidad.numero_cuenta_abono = DataUtil.DbValueToDefault<string>(oIDataReader["numero_cuenta_abono"]);
                        v_entidad.tipo_cuenta_abono = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_cuenta_abono"]);
                        v_entidad.moneda_cuenta_abono = DataUtil.DbValueToDefault<string>(oIDataReader["simbolo_moneda_cuenta_abono"]);

                        v_entidad.nombre_tipo_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_documento"]);
                        v_entidad.nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]);
                        v_entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        v_entidad.codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]);
                        v_entidad.importe_abono = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_abono_personal"]);

                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.tipo_cuenta_desembolso = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_cuenta_desembolso"]);
                        v_entidad.numero_cuenta_desembolso = DataUtil.DbValueToDefault<string>(oIDataReader["numero_cuenta_desembolso"]);
                        v_entidad.moneda_cuenta_desembolso = DataUtil.DbValueToDefault<string>(oIDataReader["simbolo_moneda_cuenta_desembolso"]);
                        v_entidad.importe_desembolso_empresa = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_desembolso_empresa"]);
                        v_entidad.calcular_detraccion = DataUtil.DbValueToDefault<bool>(oIDataReader["calcular_detraccion"]);
                        v_entidad.checksum = DataUtil.DbValueToDefault<string>(oIDataReader["checksum"]);

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

        public List<txt_contabilidad_resumen_planilla_dto> GetResumenPlanillaTxt_Contabilidad(int codigo_planilla, bool antiguo = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(antiguo ? "up_planilla_contabilidad_resumen_planilla_v2" : "up_planilla_contabilidad_resumen_planilla_v3");
            oDatabase.AddInParameter(oDbCommand, (antiguo ? "@p_codigo_planilla" : "@p_codigo_checklist"), DbType.Int32, codigo_planilla);
            List<txt_contabilidad_resumen_planilla_dto> lst = new List<txt_contabilidad_resumen_planilla_dto>();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new txt_contabilidad_resumen_planilla_dto();

                        if (!antiguo) {
                            //v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                            v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                            v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        }
                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.comisiones = DataUtil.DbValueToDefault<int>(oIDataReader["comisiones"]);

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

        public List<txt_contabilidad_resumen_planilla_dto> GetResumenPlanillaBonoTxt_Contabilidad(int codigo_planilla, bool antiguo = false)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(antiguo ? "up_planilla_bono_contabilidad_resumen_planilla_v2" : "up_planilla_bono_contabilidad_resumen_planilla_v3");
            oDatabase.AddInParameter(oDbCommand, (antiguo ? "@p_codigo_planilla" : "@p_codigo_checklist"), DbType.Int32, codigo_planilla);
            List<txt_contabilidad_resumen_planilla_dto> lst = new List<txt_contabilidad_resumen_planilla_dto>();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new txt_contabilidad_resumen_planilla_dto();

                        if (!antiguo) {
                            //v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                            v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                            v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        }
                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.comisiones = DataUtil.DbValueToDefault<int>(oIDataReader["bonos"]);

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

        public List<txt_contabilidad_planilla_dto> GetPlanillaTxt_Contabilidad(int codigo_checklist, int codigo_empresa, bool antiguo = false, int codigo_planilla = 0)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(antiguo ? "up_planilla_contabilidad_planilla_v2" : "up_planilla_contabilidad_planilla_v3");
            oDbCommand.CommandTimeout = 0;
            oDatabase.AddInParameter(oDbCommand, (antiguo ? "@p_codigo_planilla" : "@p_codigo_checklist"), DbType.Int32, codigo_checklist);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, codigo_empresa);

            if (!antiguo)
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, codigo_planilla);
            }

            List<txt_contabilidad_planilla_dto> lst = new List<txt_contabilidad_planilla_dto>();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new txt_contabilidad_planilla_dto();

                        v_entidad.COD_COMISION = DataUtil.DbValueToDefault<string>(oIDataReader["COD_COMISION"]);
                        v_entidad.N_CUOTA = DataUtil.DbValueToDefault<string>(oIDataReader["N_CUOTA"]);
                        v_entidad.IMP_PAGAR = DataUtil.DbValueToDefault<string>(oIDataReader["IMP_PAGAR"]);
                        v_entidad.COD_EMPRESA_G = DataUtil.DbValueToDefault<string>(oIDataReader["COD_EMPRESA_G"]);
                        v_entidad.NUM_CONTRATO = DataUtil.DbValueToDefault<string>(oIDataReader["NUM_CONTRATO"]);
                        v_entidad.IGV = DataUtil.DbValueToDefault<string>(oIDataReader["IGV"]);
                        v_entidad.DES_TIPO_VENTA = DataUtil.DbValueToDefault<string>(oIDataReader["DES_TIPO_VENTA"]);
                        v_entidad.TIPO_VENTA = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_VENTA"]);
                        v_entidad.FEC_HAVILITADO = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_HAVILITADO"]);
                        v_entidad.DES_FORMA_PAGO = DataUtil.DbValueToDefault<string>(oIDataReader["DES_FORMA_PAGO"]);
                        v_entidad.ID_FORMA_DE_PAGO = DataUtil.DbValueToDefault<string>(oIDataReader["ID_FORMA_DE_PAGO"]);
                        v_entidad.DES_TIPO_COMISION = DataUtil.DbValueToDefault<string>(oIDataReader["DES_TIPO_COMISION"]);
                        v_entidad.TIPO_COMISION = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_COMISION"]);
                        v_entidad.CUARTA = DataUtil.DbValueToDefault<string>(oIDataReader["CUARTA"]);
                        v_entidad.IES = DataUtil.DbValueToDefault<string>(oIDataReader["IES"]);
                        v_entidad.TIPO_MONEDA = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_MONEDA"]);
                        v_entidad.TIPO_AGENTE_G = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_AGENTE_G"]);
                        v_entidad.C_AGENTE = DataUtil.DbValueToDefault<string>(oIDataReader["C_AGENTE"]);
                        v_entidad.COD_GRUPO_VENTA_G = DataUtil.DbValueToDefault<string>(oIDataReader["COD_GRUPO_VENTA_G"]);
                        v_entidad.NOMBRE_GRUPO = DataUtil.DbValueToDefault<string>(oIDataReader["NOMBRE_GRUPO"]);
                        v_entidad.DESCRIPCION_1 = DataUtil.DbValueToDefault<string>(oIDataReader["DESCRIPCION_1"]);
                        v_entidad.COD_BIEN = DataUtil.DbValueToDefault<string>(oIDataReader["COD_BIEN"]);
                        v_entidad.DESCRIPCION_2 = DataUtil.DbValueToDefault<string>(oIDataReader["DESCRIPCION_2"]);
                        v_entidad.COD_CONCEPTO = DataUtil.DbValueToDefault<string>(oIDataReader["COD_CONCEPTO"]);
                        v_entidad.TIPO_CAMBIO = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_CAMBIO"]);
                        v_entidad.SALDO_A_PAGAR = DataUtil.DbValueToDefault<string>(oIDataReader["SALDO_A_PAGAR"]);
                        v_entidad.FEC_PLANILLA = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_PLANILLA"]);
                        v_entidad.USU_PLANILLA = DataUtil.DbValueToDefault<string>(oIDataReader["USU_PLANILLA"]);
                        v_entidad.N_OPERACION = DataUtil.DbValueToDefault<string>(oIDataReader["N_OPERACION"]);
                        v_entidad.FEC_CIERRE = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_CIERRE"]);
                        v_entidad.DESC_TIPO_DOCUM = DataUtil.DbValueToDefault<string>(oIDataReader["DESC_TIPO_DOCUM"]);
                        v_entidad.RUC = DataUtil.DbValueToDefault<string>(oIDataReader["RUC"]);
                        v_entidad.NUM_DOC = DataUtil.DbValueToDefault<string>(oIDataReader["NUM_DOC"]);
                        v_entidad.NOM_AGENTE = DataUtil.DbValueToDefault<string>(oIDataReader["NOM_AGENTE"]);
                        v_entidad.NOM_EMPRESA = DataUtil.DbValueToDefault<string>(oIDataReader["NOM_EMPRESA"]);
                        v_entidad.DIR_EMPRESA = DataUtil.DbValueToDefault<string>(oIDataReader["DIR_EMPRESA"]);
                        v_entidad.RUC_EMPRESA = DataUtil.DbValueToDefault<string>(oIDataReader["RUC_EMPRESA"]);
                        v_entidad.FEC_INICIO = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_INICIO"]);
                        v_entidad.FEC_TERMINO = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_TERMINO"]);
                        v_entidad.DSCTO_ESTUDIO_C = DataUtil.DbValueToDefault<string>(oIDataReader["DSCTO_ESTUDIO_C"]);
                        v_entidad.DSCTO_DETRACCION = DataUtil.DbValueToDefault<string>(oIDataReader["DSCTO_DETRACCION"]);
                        v_entidad.PORC_IGV = DataUtil.DbValueToDefault<string>(oIDataReader["PORC_IGV"]);
                        v_entidad.UNIDAD_NEGOCIO = DataUtil.DbValueToDefault<string>(oIDataReader["UNIDAD_NEGOCIO"]);
                        v_entidad.DIMENSION_3 = DataUtil.DbValueToDefault<string>(oIDataReader["DIMENSION_3"]);
                        v_entidad.DIMENSION_4 = DataUtil.DbValueToDefault<string>(oIDataReader["DIMENSION_4"]);
                        v_entidad.DIMENSION_5 = DataUtil.DbValueToDefault<string>(oIDataReader["DIMENSION_5"]);

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

        public List<txt_contabilidad_planilla_dto> GetPlanillaBonoTxt_Contabilidad(int codigo_checklist, int codigo_empresa, bool antiguo = false, int codigo_planilla = 0)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(antiguo ? "up_planilla_bono_contabilidad_planilla_v2" : "up_planilla_bono_contabilidad_planilla_v3");
            oDbCommand.CommandTimeout = 0;
            oDatabase.AddInParameter(oDbCommand, antiguo ? "@p_codigo_planilla":"@p_codigo_checklist", DbType.Int32, codigo_checklist);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, codigo_empresa);

            if(!antiguo)
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, codigo_planilla);
            }

            List<txt_contabilidad_planilla_dto> lst = new List<txt_contabilidad_planilla_dto>();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new txt_contabilidad_planilla_dto();

                        v_entidad.COD_COMISION = DataUtil.DbValueToDefault<string>(oIDataReader["COD_COMISION"]);
                        v_entidad.N_CUOTA = DataUtil.DbValueToDefault<string>(oIDataReader["N_CUOTA"]);
                        v_entidad.IMP_PAGAR = DataUtil.DbValueToDefault<string>(oIDataReader["IMP_PAGAR"]);
                        v_entidad.COD_EMPRESA_G = DataUtil.DbValueToDefault<string>(oIDataReader["COD_EMPRESA_G"]);
                        v_entidad.NUM_CONTRATO = DataUtil.DbValueToDefault<string>(oIDataReader["NUM_CONTRATO"]);
                        v_entidad.IGV = DataUtil.DbValueToDefault<string>(oIDataReader["IGV"]);
                        v_entidad.DES_TIPO_VENTA = DataUtil.DbValueToDefault<string>(oIDataReader["DES_TIPO_VENTA"]);
                        v_entidad.TIPO_VENTA = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_VENTA"]);
                        v_entidad.FEC_HAVILITADO = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_HAVILITADO"]);
                        v_entidad.DES_FORMA_PAGO = DataUtil.DbValueToDefault<string>(oIDataReader["DES_FORMA_PAGO"]);
                        v_entidad.ID_FORMA_DE_PAGO = DataUtil.DbValueToDefault<string>(oIDataReader["ID_FORMA_DE_PAGO"]);
                        v_entidad.DES_TIPO_COMISION = DataUtil.DbValueToDefault<string>(oIDataReader["DES_TIPO_COMISION"]);
                        v_entidad.TIPO_COMISION = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_COMISION"]);
                        v_entidad.CUARTA = DataUtil.DbValueToDefault<string>(oIDataReader["CUARTA"]);
                        v_entidad.IES = DataUtil.DbValueToDefault<string>(oIDataReader["IES"]);
                        v_entidad.TIPO_MONEDA = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_MONEDA"]);
                        v_entidad.TIPO_AGENTE_G = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_AGENTE_G"]);
                        v_entidad.C_AGENTE = DataUtil.DbValueToDefault<string>(oIDataReader["C_AGENTE"]);
                        v_entidad.COD_GRUPO_VENTA_G = DataUtil.DbValueToDefault<string>(oIDataReader["COD_GRUPO_VENTA_G"]);
                        v_entidad.NOMBRE_GRUPO = DataUtil.DbValueToDefault<string>(oIDataReader["NOMBRE_GRUPO"]);
                        v_entidad.DESCRIPCION_1 = DataUtil.DbValueToDefault<string>(oIDataReader["DESCRIPCION_1"]);
                        v_entidad.COD_BIEN = DataUtil.DbValueToDefault<string>(oIDataReader["COD_BIEN"]);
                        v_entidad.DESCRIPCION_2 = DataUtil.DbValueToDefault<string>(oIDataReader["DESCRIPCION_2"]);
                        v_entidad.COD_CONCEPTO = DataUtil.DbValueToDefault<string>(oIDataReader["COD_CONCEPTO"]);
                        v_entidad.TIPO_CAMBIO = DataUtil.DbValueToDefault<string>(oIDataReader["TIPO_CAMBIO"]);
                        v_entidad.SALDO_A_PAGAR = DataUtil.DbValueToDefault<string>(oIDataReader["SALDO_A_PAGAR"]);
                        v_entidad.FEC_PLANILLA = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_PLANILLA"]);
                        v_entidad.USU_PLANILLA = DataUtil.DbValueToDefault<string>(oIDataReader["USU_PLANILLA"]);
                        v_entidad.N_OPERACION = DataUtil.DbValueToDefault<string>(oIDataReader["N_OPERACION"]);
                        v_entidad.FEC_CIERRE = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_CIERRE"]);
                        v_entidad.DESC_TIPO_DOCUM = DataUtil.DbValueToDefault<string>(oIDataReader["DESC_TIPO_DOCUM"]);
                        v_entidad.RUC = DataUtil.DbValueToDefault<string>(oIDataReader["RUC"]);
                        v_entidad.NUM_DOC = DataUtil.DbValueToDefault<string>(oIDataReader["NUM_DOC"]);
                        v_entidad.NOM_AGENTE = DataUtil.DbValueToDefault<string>(oIDataReader["NOM_AGENTE"]);
                        v_entidad.NOM_EMPRESA = DataUtil.DbValueToDefault<string>(oIDataReader["NOM_EMPRESA"]);
                        v_entidad.DIR_EMPRESA = DataUtil.DbValueToDefault<string>(oIDataReader["DIR_EMPRESA"]);
                        v_entidad.RUC_EMPRESA = DataUtil.DbValueToDefault<string>(oIDataReader["RUC_EMPRESA"]);
                        v_entidad.FEC_INICIO = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_INICIO"]);
                        v_entidad.FEC_TERMINO = DataUtil.DbValueToDefault<string>(oIDataReader["FEC_TERMINO"]);
                        v_entidad.DSCTO_ESTUDIO_C = DataUtil.DbValueToDefault<string>(oIDataReader["DSCTO_ESTUDIO_C"]);
                        v_entidad.DSCTO_DETRACCION = DataUtil.DbValueToDefault<string>(oIDataReader["DSCTO_DETRACCION"]);
                        v_entidad.PORC_IGV = DataUtil.DbValueToDefault<string>(oIDataReader["PORC_IGV"]);
                        v_entidad.UNIDAD_NEGOCIO = DataUtil.DbValueToDefault<string>(oIDataReader["UNIDAD_NEGOCIO"]);
                        v_entidad.DIMENSION_3 = DataUtil.DbValueToDefault<string>(oIDataReader["DIMENSION_3"]);
                        v_entidad.DIMENSION_4 = DataUtil.DbValueToDefault<string>(oIDataReader["DIMENSION_4"]);
                        v_entidad.DIMENSION_5 = DataUtil.DbValueToDefault<string>(oIDataReader["DIMENSION_5"]);

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

        public DataTable ReporteLiquidacionBonoPorcentajes(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_liquidacion_bono_personal_porcentajes_v2");
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

        public planilla_resumen_dto BuscarUltimoCerrado()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_buscar_ultimo_cerrado");
            var v_entidad = new planilla_resumen_dto();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.nombre_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_planilla"]);
                        v_entidad.estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["estado_planilla"]);
                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);
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

        public planilla_resumen_dto BuscarUltimoAbiertoPorArticulo(regla_calculo_comision_validacion_dto comision)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_buscar_validacion_comision");
            var v_entidad = new planilla_resumen_dto();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, comision.codigo_tipo_planilla); 
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, comision.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, comision.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_venta", DbType.Int32, comision.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, comision.codigo_articulo);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.nombre_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_planilla"]);
                        v_entidad.estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["estado_planilla"]);
                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);
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

        public DataTable ReportePlanillaBonoJNDetalle(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_jn_detalle_v2");
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

        public DataTable ReportePlanillaBonoJNResumen(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_jn_resumen_v2");
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

        public DataTable ReportePlanillaBonoJNResumenTitulos(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_jn_resumen_titulos_v2");
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

        public DataTable ReportePlanillaBonoJN(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_jn_v2");
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

        public DataTable ReportePlanillaBonoJNContabilidad(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_jn_contabilidad_v2");
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

        public List<planilla_checklist_comision_dto> ListarParaChecklistComision()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_listar_checklist_comision");
            List<planilla_checklist_comision_dto> lst = new List<planilla_checklist_comision_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new planilla_checklist_comision_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.nombre_regla_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_tipo_planilla"]);
                        v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);

                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.nombre_estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);

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

        public List<planilla_checklist_bono_dto> ListarParaChecklistBono()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_listar_checklist_bono");
            List<planilla_checklist_bono_dto> lst = new List<planilla_checklist_bono_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new planilla_checklist_bono_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]);
                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"]);

                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.nombre_estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);

                        v_entidad.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        v_entidad.nombre_canal= DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);

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

        public List<planilla_checklist_bono_trimestral_dto> ListarParaChecklistBonoTrimestral()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_listar_checklist_bono_trimestral");
            List<planilla_checklist_bono_trimestral_dto> lst = new List<planilla_checklist_bono_trimestral_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new planilla_checklist_bono_trimestral_dto();

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);

                        v_entidad.anio_periodo = DataUtil.DbValueToDefault<string>(oIDataReader["anio_periodo"]);
                        v_entidad.periodo = DataUtil.DbValueToDefault<string>(oIDataReader["periodo"]);

                        v_entidad.codigo_estado_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_planilla"]);
                        v_entidad.nombre_estado_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_planilla"]);

                        v_entidad.nombre_regla= DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla"]);

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

        public DataTable ReportePlanillaBonoJNLiquidacion(int p_codigo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_bono_jn_liquidacion_v2");
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

    }
}
