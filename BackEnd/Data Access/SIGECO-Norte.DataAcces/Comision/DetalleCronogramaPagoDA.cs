using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.DataAcces.Comision.Helper;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;

namespace SIGEES.DataAcces
{
    public partial class DetalleCronogramaPagoDA : GenericDA<DetalleCronogramaPagoDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);
        public int HabilitarPago(detalle_cronograma_dto pEntidad)
        {
            int codigo_detalle_cronograma = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_cronograma_habilitar_pago");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_cronograma", DbType.Int32, pEntidad.codigo_detalle_cronograma);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@observacion", DbType.String, pEntidad.observacion);

                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_detalle_cronograma = pEntidad.codigo_detalle_cronograma;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle_cronograma;
        }

        public int RefinanciarPagoComisionCuota(detalle_cronograma_comision_dto v_cuota)
        {
            int codigo_detalle_cronograma = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_refinanciar_pago_comision");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_cronograma", DbType.Int32, v_cuota.codigo_detalle_cronograma);
                oDatabase.AddInParameter(oDbCommand, "@monto_financiar", DbType.Decimal, v_cuota.importe_comision);
                oDatabase.AddInParameter(oDbCommand, "@numero_cuota", DbType.Int32, v_cuota.nro_cuota);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, v_cuota.usuario_registra);
                

                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_detalle_cronograma = v_cuota.codigo_detalle_cronograma;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle_cronograma;
        }

        public int AnularCuotaComision(detalle_cronograma_comision_dto v_cuota)
        {

            int codigo_detalle_cronograma = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_anular_pago_comision");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_cronograma", DbType.Int32, v_cuota.codigo_detalle_cronograma);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, v_cuota.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@motivo", DbType.String, v_cuota.motivo_anulacion);
                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_detalle_cronograma = v_cuota.codigo_detalle_cronograma;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle_cronograma;
        }

        public int GestionExclusionHabilitarPagoComision(grilla_cuota_pago_planilla_dto pEntidad)
        {
            int codigo_detalle_cronograma = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_gestion_exclusion_habilitar_pago");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_exclusion", DbType.Int32, pEntidad.codigo_exclusion);
                //oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_cronograma", DbType.Int32, pEntidad.codigo_detalle_cronograma);
                //oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_planilla", DbType.Int32, pEntidad.codigo_detalle_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, pEntidad.codigo_planilla);

                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@observacion", DbType.String, pEntidad.motivo_registro);

                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_detalle_cronograma = pEntidad.codigo_detalle_cronograma;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle_cronograma;
        }

        public void Modificar(detalle_cronograma_comision_dto detalle_cronograma)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_cronograma_modificar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_detalle_cronograma", DbType.Int32, detalle_cronograma.codigo_detalle_cronograma);
                oDatabase.AddInParameter(oDbCommand, "@p_importe_comision", DbType.Decimal, detalle_cronograma.importe_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, detalle_cronograma.usuario_modifica);
                oDatabase.AddInParameter(oDbCommand, "@p_motivo", DbType.String, detalle_cronograma.observacion);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public int Adicionar(detalle_cronograma_adicionar_dto detalle_cronograma)
        {
            int nro_cuota = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_cronograma_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, detalle_cronograma.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, detalle_cronograma.nro_contrato);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, detalle_cronograma.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, detalle_cronograma.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@p_monto_neto", DbType.Decimal, detalle_cronograma.monto_neto);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, detalle_cronograma.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_motivo", DbType.String, detalle_cronograma.motivo);
                oDatabase.AddOutParameter(oDbCommand, "p_nro_cuota", DbType.Int32, 0);
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_nro_cuota").ToString();
                nro_cuota = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return nro_cuota;
        }

        public void Deshabilitar(detalle_cronograma_deshabilitacion_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_cronograma_deshabilitar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_XmlDetalleCronograma", DbType.Xml, pEntidad.procesarXML);
                oDatabase.AddInParameter(oDbCommand, "@p_motivo", DbType.String, pEntidad.motivo);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Aprobar(List<detalle_cronograma_personal_inactivo_dto> lst_detalle, int nivel, int codigo_resultado, string codigo_usuario, string observacion)
        {
            var custColl = new DetalleCronogramaTypeCollection();

            foreach (var item in lst_detalle)
            {
                detalle_cronograma_type_dto type = new detalle_cronograma_type_dto();
                type.codigo_detalle_cronograma = item.codigo_detalle_cronograma;
                custColl.Add(type);
            }

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_cronograma_aprobar_comision_inactiva");

            try
            {
                var parameter = new SqlParameter("@p_array_detalle_cronograma", SqlDbType.Structured)
                {
                    TypeName = "dbo.array_detalle_cronograma_type",
                    SqlValue = custColl.Count == 0 ? null : custColl
                };

                oDbCommand.Parameters.Add(parameter);
                oDatabase.AddInParameter(oDbCommand, "@p_nivel", DbType.Int32, nivel);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_resultado", DbType.Int32, codigo_resultado);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_aprobacion", DbType.String, codigo_usuario);
                oDatabase.AddInParameter(oDbCommand, "@p_observacion", DbType.String, observacion);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }   

    }

    public partial class DetalleCronogramaPagoSelDA : GenericDA<DetalleCronogramaPagoSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);
       
        public detalle_cronograma_comision_dto CuotaPagoById(int codigo_detalle_cronograma)
        {
            var v_entidad = new detalle_cronograma_comision_dto();
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_cronograma_obtener_cuota_by_id");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@p_codigo_detalle_cronograma", DbType.Int32, codigo_detalle_cronograma);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        v_entidad.importe_sing_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.importe_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.fecha_programada = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_programada"]);
                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);

                        v_entidad.nro_total_contrato = DataUtil.DbValueToDefault<int>(oIDataReader["nro_total_contrato"]);
                        v_entidad.nro_cuota_libre_refinanciar = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota_libre_refinanciar"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                        v_entidad.s_importe_comision = Format.DecimalToString(v_entidad.importe_comision, 2);
                        v_entidad.s_importe_sing_igv = Format.DecimalToString(v_entidad.importe_sing_igv, 2);
                        v_entidad.s_igv = Format.DecimalToString(v_entidad.igv, 2);
                        v_entidad.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);
                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
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

        #region GESTION DE EXCLUSION


        public List<listado_exclusion_grilla_dto> ListarPagosExcluidosAll(listado_exclusion_grilla_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_gestion_exclusion_listar");
            List<listado_exclusion_grilla_dto> lst = new List<listado_exclusion_grilla_dto>();



            try
            {
                //oDatabase.AddInParameter(oDbCommand, "@codigo_planilla", DbType.Int32, v_filtro.codigo_planilla);
                //oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, v_filtro.codigo_tipo_venta);
                //oDatabase.AddInParameter(oDbCommand, "@codigo_personal", DbType.Int32, v_filtro.codigo_personal);                
                //oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_busqueda", DbType.Boolean, v_filtro.codigo_tipo_busqueda);
                //oDatabase.AddInParameter(oDbCommand, "@excluido", DbType.Boolean, v_filtro.estado_registro);               


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new listado_exclusion_grilla_dto();

                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.codigo_detalle_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_planilla"]);

                        v_entidad.codigo_exclusion = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_exclusion"]);
                        v_entidad.usuario_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_exclusion"]);
                        v_entidad.fecha_exclusion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_exclusion"]);
                        if (v_entidad.fecha_exclusion != null)
                        {
                            v_entidad.str_fecha_exclusion = DateTime.Parse(v_entidad.fecha_exclusion.ToString()).ToShortDateString();
                        }
                        v_entidad.motivo_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_exclusion"]);
                        v_entidad.usuario_habilita = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_habilita"]);
                        v_entidad.fecha_habilita = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_habilita"]);
                        if (v_entidad.fecha_habilita != null)
                        {
                            v_entidad.str_fecha_habilita = DateTime.Parse(v_entidad.fecha_habilita.ToString()).ToShortDateString();
                        }
                        v_entidad.motivo_habilita = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_habilita"]);

                        v_entidad.numero_planilla_incluido = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla_incluido"]);

                        v_entidad.codigo_estado_exclusion = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_exclusion"]);

                        v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.fecha_registra = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                        v_entidad.str_fecha_registra = v_entidad.fecha_registra.ToShortDateString();

                        v_entidad.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                        v_entidad.str_fecha_inicio = v_entidad.fecha_inicio.ToShortDateString();

                        v_entidad.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);
                        v_entidad.str_fecha_fin = v_entidad.fecha_fin.ToShortDateString();


                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);

                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_persona"]);

                        v_entidad.nombre_estado_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_exclusion"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);

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
        #endregion

        public detalle_exclusion_dto GetDetalleExclusionCuotaPagoComision(int p_codigo_exclusion)
        {

            var v_entidad = new detalle_exclusion_dto();
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_gestion_exclusion_cuota_by_id");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_exclusion", DbType.Int32, p_codigo_exclusion);                

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);

                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        v_entidad.nombre_estado_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_exclusion"]);

                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.codigo_exclusion = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_exclusion"]);
                        v_entidad.usuario_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_exclusion"]);
                        v_entidad.motivo_exclusion = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_exclusion"]);
                        v_entidad.fecha_exclusion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_exclusion"]);
                        v_entidad.str_fecha_exclusion =Fechas.convertDateTimeToString(v_entidad.fecha_exclusion);
                        
                        v_entidad.usuario_habilitacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_habilita"]);
                        v_entidad.motivo_habilitacion = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_habilita"]);

                        v_entidad.fecha_habilitacion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_habilita"]);
                        v_entidad.str_fecha_habilitacion = Fechas.convertDateTimeToString(v_entidad.fecha_habilitacion);                        
         
                        v_entidad.s_igv = Format.DecimalToString(v_entidad.igv, 2);
                        v_entidad.s_monto_neto = Format.DecimalToString(v_entidad.monto_neto, 2);
                        v_entidad.s_monto_bruto = Format.DecimalToString(v_entidad.monto_bruto, 2);
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

        public resumen_pago_comision_personal_dto GetPagoComisionByArticuloPersonal(cronograma_pago_filtro v_search)
        {
            var v_entidad = new resumen_pago_comision_personal_dto();
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_obtener_pago_comision_by_articulo_personal");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, v_search.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, v_search.codigo_personal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, v_search.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, v_search.nro_contrato.Trim());
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);
                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.total_registro_encontrado = DataUtil.DbValueToDefault<int>(oIDataReader["total_registro_encontrado"]);
                        v_entidad.monto_comision_articulo = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_comision_articulo"]);

                        v_entidad.monto_neto_pendiente = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_pendiente"]);
                        v_entidad.monto_neto_en_proceso_pago = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_en_proceso_pago"]);
                        v_entidad.monto_neto_pagado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_pagado"]);
                        v_entidad.monto_neto_excluido = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_excluido"]);
                        v_entidad.monto_neto_anulado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_anulado"]);

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

        public List<grilla_comision_cronograma_dto> CronogramaPagoComisionListar(grilla_comision_cronograma_filtro v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_pago_comision_listar");
            oDbCommand.CommandTimeout = 360000;
            List<grilla_comision_cronograma_dto> lst = new List<grilla_comision_cronograma_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_estado_cuota", DbType.Int32, v_filtro.codigo_estado_cuota);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, v_filtro.codigo_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, v_filtro.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, v_filtro.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, v_filtro.codigo_personal);

                oDatabase.AddInParameter(oDbCommand, "@p_fecha_habilitado_inicio", DbType.Date, v_filtro.fecha_habilitado_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_fecha_habilitado_fin", DbType.Date, v_filtro.fecha_habilitado_fin);

                oDatabase.AddInParameter(oDbCommand, "@p_fecha_contrato_inicio", DbType.DateTime, v_filtro.fecha_contrato_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_fecha_contrato_fin", DbType.DateTime, v_filtro.fecha_contrato_fin);


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new grilla_comision_cronograma_dto();

                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.nombre_regla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla"]);
                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        v_entidad.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.datos_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["datos_vendedor"]);

                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.fecha_programada = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_programada"]);

                        v_entidad.monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);

                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<Int32>(oIDataReader["nro_cuota"]);
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

        public DataTable CronogramaPagoComisionDataTable(grilla_comision_cronograma_filtro v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_pago_comision_listar");
            DataTable lst = new DataTable();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_estado_cuota", DbType.Int32, v_filtro.codigo_estado_cuota);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, v_filtro.codigo_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, v_filtro.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, v_filtro.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.Int32, v_filtro.codigo_personal);


                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    lst.Load(oIDataReader);

                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }

        public List<operacion_cuota_comision_listado_dto> ListadoOperacion(operacion_cuota_comision_listado_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_operacion_cuota_comision_listado");
            List<operacion_cuota_comision_listado_dto> lst = new List<operacion_cuota_comision_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_detalle_cronograma", DbType.Int32, busqueda.codigo_detalle_cronograma);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_usuario", DbType.String, busqueda.usuario);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var operacion = new operacion_cuota_comision_listado_dto();

                        operacion.codigo_operacion_cuota_comision = DataUtil.DbValueToDefault<Int32>(oIDataReader["codigo_operacion_cuota_comision"]);
                        operacion.nombre_operacion = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_operacion"]);
                        operacion.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);
                        operacion.fecha_operacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_operacion"]);
                        operacion.nombre_estado = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado"]);
                        operacion.usuario = DataUtil.DbValueToDefault<string>(oIDataReader["usuario"]);
                        operacion.valor_original = DataUtil.DbValueToDefault<string>(oIDataReader["valor_original"]);

                        lst.Add(operacion);
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


        public List<detalle_cronograma_personal_inactivo_dto> ListadoComisionPersonalInactivo(detalle_cronograma_personal_inactivo_busqueda_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_personal_inactivo_listado");
            List<detalle_cronograma_personal_inactivo_dto> lst = new List<detalle_cronograma_personal_inactivo_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.String, busqueda.fecha_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.String, busqueda.fecha_fin);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_liquidado", DbType.Int32, busqueda.liquidado);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var comision = new detalle_cronograma_personal_inactivo_dto();

                        comision.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        comision.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        comision.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        comision.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        comision.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        comision.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        comision.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        comision.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        comision.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);
                        comision.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        comision.nro_cuota = DataUtil.DbValueToDefault<Int32>(oIDataReader["nro_cuota"]);
                        comision.monto_sin_igv = DataUtil.DbValueToDefault<Decimal>(oIDataReader["monto_sin_igv"]);
                        comision.monto_igv = DataUtil.DbValueToDefault<Decimal>(oIDataReader["monto_igv"]);
                        comision.monto_con_igv = DataUtil.DbValueToDefault<Decimal>(oIDataReader["monto_con_igv"]);
                        comision.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);
                        comision.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        comision.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        comision.liquidado = DataUtil.DbValueToDefault<int>(oIDataReader["liquidado"]);
                        comision.resultado_n1 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n1"]);
                        comision.resultado_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n2"]);
                        comision.estilo_n1 = DataUtil.DbValueToDefault<string>(oIDataReader["estilo_n1"]);
                        comision.estilo_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["estilo_n2"]);
                        comision.codigo_resultado_n1 = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_resultado_n1"]);
                        comision.codigo_resultado_n2 = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_resultado_n2"]);
                        lst.Add(comision);
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

        public detalle_cronograma_aprobacion_dto ListadoComisionPersonalInactivo(int codigo_detalle)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_personal_inactivo_por_codigo");
            detalle_cronograma_aprobacion_dto comision = new detalle_cronograma_aprobacion_dto();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_detalle_cronograma", DbType.Int32, codigo_detalle);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    if (oIDataReader.Read())
                    {
                        comision.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        comision.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        comision.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        comision.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        comision.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        comision.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        comision.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        comision.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        comision.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);
                        comision.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                        comision.nro_cuota = DataUtil.DbValueToDefault<Int32>(oIDataReader["nro_cuota"]);
                        comision.monto_sin_igv = DataUtil.DbValueToDefault<Decimal>(oIDataReader["monto_sin_igv"]);
                        comision.monto_igv = DataUtil.DbValueToDefault<Decimal>(oIDataReader["monto_igv"]);
                        comision.monto_con_igv = DataUtil.DbValueToDefault<Decimal>(oIDataReader["monto_con_igv"]);
                        comision.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);
                        comision.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                        comision.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                        comision.liquidado = DataUtil.DbValueToDefault<int>(oIDataReader["liquidado"]);
                        comision.estado_liquidado = DataUtil.DbValueToDefault<string>(oIDataReader["estado_liquidado"]);
                        comision.resultado_n1 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n1"]);
                        comision.resultado_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n2"]);
                        comision.codigo_resultado_n1 = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_resultado_n1"]);
                        comision.codigo_resultado_n2 = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_resultado_n2"]);
                        comision.resultado_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n2"]);
                        comision.resultado_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n2"]);
                        comision.resultado_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n2"]);
                        comision.resultado_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["resultado_n2"]);
                        comision.fecha_aprobacion_n1 = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_aprobacion_n1"]);
                        comision.fecha_aprobacion_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_aprobacion_n2"]);
                        comision.usuario_aprobacion_n1 = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_aprobacion_n1"]);
                        comision.usuario_aprobacion_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_aprobacion_n2"]);
                        comision.observacion_n1 = DataUtil.DbValueToDefault<string>(oIDataReader["observacion_n1"]);
                        comision.observacion_n2 = DataUtil.DbValueToDefault<string>(oIDataReader["observacion_n2"]);
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

        public detalle_cronograma_personal_inactivo_busqueda_dto FechasComisionPersonalInactivo()
        {
            detalle_cronograma_personal_inactivo_busqueda_dto fechas = new detalle_cronograma_personal_inactivo_busqueda_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_personal_inactivo_fechas");

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        fechas = new detalle_cronograma_personal_inactivo_busqueda_dto
                        {
                            fecha_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_inicio"]),
                            fecha_fin = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_fin"])
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
    }

}
