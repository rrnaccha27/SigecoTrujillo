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
using SIGEES.Entidades.BeanReporte;
using SIGEES.DataAcces.Helper;
using System.Configuration;

namespace SIGEES.DataAcces
{
    public partial class ReporteGeneralDA : GenericDA<ReporteGeneralDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(SIGEES.DataAcces.Helper.Conexion.cnSIGEES);

        public DataTable ListarPersonaPorFechaRegistra(DateTime fechaInicio, DateTime fechaFin)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_persona_consulta_rango_reporte");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, 1);
                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, fechaInicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, fechaFin);

                dt.Load(oDatabase.ExecuteReader(oDbCommand));

            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }

        public DataTable ListarPersonaPorParametrosInterface(int codigoCanal, int codigoGrupo, int estadoRegistro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_personal_consulta_interface_reporte");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, codigoCanal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, codigoGrupo);
                oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Boolean, estadoRegistro);

                dt.Load(oDatabase.ExecuteReader(oDbCommand));

            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }
        public DataTable ListarCanalGrupoPorFechaRegistra(DateTime fechaInicio, DateTime fechaFin, int esCanalGrupo, int codigoPadre)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_canal_grupo_reporte");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, fechaInicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, fechaFin);
                oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Boolean, esCanalGrupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_padre", DbType.Int32, codigoPadre);

                dt.Load(oDatabase.ExecuteReader(oDbCommand));

            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }

        public DataTable ListarCanalGrupoInterface(int esCanalGrupo, int codigoPadre)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_canal_grupo_interface_reporte");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_es_canal_grupo", DbType.Boolean, esCanalGrupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_padre", DbType.Int32, codigoPadre);

                dt.Load(oDatabase.ExecuteReader(oDbCommand));

            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return dt;
        }

        #region Comun

        public DataTable Anio()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_general_anio");
            DataTable dt = new DataTable();
            try
            {
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

        #region Comercial
        
        public List<reporte_resumen_comercial_dto> ReportePlanillaComisionVendedor(reporte_comercial_busqueda_dto busqueda)
        {
            var reporte = new reporte_resumen_comercial_dto();
            var lstReporte = new List<reporte_resumen_comercial_dto>();

        
           // DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_comercial");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_resumen_comision_chiclayo");
            
            //oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, busqueda.sede);
            oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);            
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.Date, busqueda.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.Date, busqueda.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_reporte", DbType.Int32, busqueda.codigo_tipo_reporte);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_resumen_comercial_dto
                        {
                            //codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            //apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]),
                            //apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]),
                            //tipo = DataUtil.DbValueToDefault<string>(oIDataReader["tipo"]),

                            //nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]),
                            //monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv"]),
                            //monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]),
                            //monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]),
                            //monto_ir = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_ir"]),

                            //monto_neto_espacio = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_espacio"]),
                            //monto_neto_cremacion = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_cremacion"]),
                            //monto_neto_servicio = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_servicio"]),
                            //monto_neto_otros = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_otros"])

                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["personal"]),
                            monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision"]),
                            monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["base_imponible"]),
                            monto_ir = DataUtil.DbValueToDefault<decimal>(oIDataReader["ir"]),

                            monto_neto_espacio = DataUtil.DbValueToDefault<decimal>(oIDataReader["total_campo"]),
                            monto_neto_cremacion = DataUtil.DbValueToDefault<decimal>(oIDataReader["total_cremacion"]),
                            monto_neto_servicio = DataUtil.DbValueToDefault<decimal>(oIDataReader["total_servicio"]),

                            bono_espacio = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_campo"]),
                            bono_cremacion = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_cremacion"]),
                            bono_servicio = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_servicio"])

                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }
        public List<reporte_detallado_vendedores_dto> Detalle(reporte_comercial_busqueda_dto busqueda)
        {
            var reporte = new reporte_detallado_vendedores_dto();
            var lstReporte = new List<reporte_detallado_vendedores_dto>();


            // DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_comercial");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_detallado_comision_chiclayo");

            //oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, busqueda.sede);
            oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_persona", DbType.Int32, busqueda.codigo_persona);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.Date, busqueda.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.Date, busqueda.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_reporte", DbType.Int32, busqueda.codigo_tipo_reporte);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var porcentaje_cuota_inicial = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_cuota_inicial"]);
                        var porcentaje_regla_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_regla_comision"]);

                        reporte = new reporte_detallado_vendedores_dto
                        {
                            nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]),

                            codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]),
                            nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]),

                            codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]),
                            nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]),

                            codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]),
                            nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]),

                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            tipo = DataUtil.DbValueToDefault<string>(oIDataReader["tipo"]),

                            codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]),
                            nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]),

                            nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]),

                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]),
                            apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]),
                            monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]),
                            monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]),


                            monto_total_contrato = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_contrato"]),
                            monto_total_cuota_inicial = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_cuota_inicial"]),
                            porcentaje_cuota_inicial = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_cuota_inicial"]),
                            porcentaje_regla_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_regla_comision"]),

                            nro_ruc = DataUtil.DbValueToDefault<string>(oIDataReader["nro_ruc"]),
                            direccion = DataUtil.DbValueToDefault<string>(oIDataReader["direccion"]),
                            contacto = DataUtil.DbValueToDefault<string>(oIDataReader["contacto"]),
                            nro_cuenta = DataUtil.DbValueToDefault<string>(oIDataReader["nro_cuenta"]),
                            codigo_interbancario = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_interbancario"]),
                            nombre_banco = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_banco"]),
                            telefono_celular = DataUtil.DbValueToDefault<string>(oIDataReader["telefono_celular"]),
                            telefono_fijo = DataUtil.DbValueToDefault<string>(oIDataReader["telefono_fijo"]),
                            fecha_programado = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_programada"]),
                            id_tipo_comision = DataUtil.DbValueToDefault<Int32>(oIDataReader["id_tipo_comision"])


                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        public List<reporte_detallado_vendedores_dto> DetalleComisionPlanillaSupervisor(reporte_comercial_busqueda_dto busqueda)
        {
            var reporte = new reporte_detallado_vendedores_dto();
            var lstReporte = new List<reporte_detallado_vendedores_dto>();

            /*if (!busqueda.fecha_fin.HasValue || !busqueda.fecha_inicio.HasValue)
                return lstReporte;*/

            // DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_comercial");
            //DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_detallado_comision_supervisor_chiclayo");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_detallado_comision_supervisor_chiclayo_new");

            //oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, busqueda.sede);
            //oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_persona", DbType.Int32, busqueda.codigo_persona);
            oDatabase.AddInParameter(oDbCommand, "@p_anio", DbType.Int32, busqueda.codigo_anio);
            oDatabase.AddInParameter(oDbCommand, "@p_mes", DbType.Int32, busqueda.codigo_mes);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_detallado_vendedores_dto
                        {
                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]),
                            codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]),
                            nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]),
                            codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]),
                            concepto_comision_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["concepto_comision_articulo"]),
                            concepto_comision_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["concepto_comision_tipo_venta"]),
                            tope_unidad = DataUtil.DbValueToDefault<int>(oIDataReader["tope_unidad"]),
                            cantidad_contrato = DataUtil.DbValueToDefault<int>(oIDataReader["conteo"]),
                            porcentaje_regla_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision"]),
                            precio_unidad = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_nro_venta"]),
                            monto_total_contrato = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_x_venta"]),
                            monto_contrato = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_contrato"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_x_monto"]),
                            monto_pagado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_pagado"]),
                            meta_tipo_venta = DataUtil.DbValueToDefault<decimal>(oIDataReader["meta_tipo_venta"]),
                            excedente_tipo_venta = DataUtil.DbValueToDefault<decimal>(oIDataReader["excedente_tipo_venta"]),
                            porcentaje_meta = DataUtil.DbValueToDefault<decimal>(oIDataReader["valor_comisionable_tope"]),
                            porcentaje_excedente = DataUtil.DbValueToDefault<decimal>(oIDataReader["valor_comisionable_excedente"]),
                            concepto_bono_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["concepto_bono_tipo_venta"]),
                            monto_prorrateo = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_tipo_venta"]),
                            concepto_excedente_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["concepto_excedente_tipo_venta"]),
                            monto_prorrateo_excedente = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_excedente_tipo_venta"]),
                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        public List<reporte_detallado_vendedores_dto> DetalleComisionPlanillaJefatura(reporte_comercial_busqueda_dto busqueda)
        {
            var reporte = new reporte_detallado_vendedores_dto();
            var lstReporte = new List<reporte_detallado_vendedores_dto>();

            /*if (!busqueda.fecha_fin.HasValue || !busqueda.fecha_inicio.HasValue)
                return lstReporte;*/

            // DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_comercial");
            //DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_detallado_comision_supervisor_chiclayo");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_detallado_comision_jefatura_chiclayo");

            //oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, busqueda.sede);
            //oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_persona", DbType.Int32, busqueda.codigo_persona);
            oDatabase.AddInParameter(oDbCommand, "@p_anio", DbType.Int32, busqueda.codigo_anio);
            oDatabase.AddInParameter(oDbCommand, "@p_mes", DbType.Int32, busqueda.codigo_mes);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_detallado_vendedores_dto
                        {
                            /*nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]),

                            codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]),
                            nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]),

                            codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]),
                            nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]),

                            codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]),
                            nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]),

                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),

                            nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]),*/
                            /*
                            codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]),
                            nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]),
                            nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]),//descripcion de la comision
                            
                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]),
                            apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]),
                            //monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]),
                            precio_unidad = DataUtil.DbValueToDefault<decimal>(oIDataReader["precio_unidad"]),
                            cantidad_contrato = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad"]),
                            tope_unidad = DataUtil.DbValueToDefault<int>(oIDataReader["tope_unidad"]),
                            codigo_detalle_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_rd"])*/

                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]),
                            codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]),
                            nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]),
                            cantidad_contrato = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_ingresado"]),
                            porcentaje_regla_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["com_x_monto"]),
                            precio_unidad = DataUtil.DbValueToDefault<decimal>(oIDataReader["com_x_venta"]),
                            comision_x_meta = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_x_venta"]),
                            comision_excedente = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision_x_monto"])
                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        public List<reporte_detallado_vendedores_dto> DetalleComisionContrato(reporte_comercial_busqueda_dto busqueda)
        {
            var reporte = new reporte_detallado_vendedores_dto();
            var lstReporte = new List<reporte_detallado_vendedores_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_contrato_comision_supervisor_chiclayo");
            oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_persona", DbType.Int32, busqueda.codigo_persona);
            oDatabase.AddInParameter(oDbCommand, "@p_anio", DbType.Int32, busqueda.codigo_anio);
            oDatabase.AddInParameter(oDbCommand, "@p_mes", DbType.Int32, busqueda.codigo_mes);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_detallado_vendedores_dto
                        {
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]),
                            nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]),

                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]),
                            apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]),
                            monto_total_contrato= DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_contrato"]),
                            monto_total_cuota_inicial = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_pagado"])
                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        public List<reporte_resumen_comercial_dto> Detalle_OrdenPago(reporte_comercial_busqueda_dto busqueda)
        {
            var reporte = new reporte_resumen_comercial_dto();
            var lstReporte = new List<reporte_resumen_comercial_dto>();


            // DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_comercial");
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_resumen_comision_trujillo");

            oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_persona", DbType.Int32, busqueda.codigo_persona);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicio", DbType.Date, busqueda.fecha_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_fin", DbType.Date, busqueda.fecha_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_reporte", DbType.Int32, busqueda.codigo_tipo_reporte);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_resumen_comercial_dto
                        {
                            nombre_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["supervisor"]),
                            nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["grupo_venta"]),
                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            nro_documento = DataUtil.DbValueToDefault<string>(oIDataReader["nro_documento"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["personal"]),
                            monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["comision"]),
                            monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["base_imponible"]),
                            monto_ir = DataUtil.DbValueToDefault<decimal>(oIDataReader["ir"]),

                            monto_neto_espacio = DataUtil.DbValueToDefault<decimal>(oIDataReader["total_campo"]),
                            monto_neto_cremacion = DataUtil.DbValueToDefault<decimal>(oIDataReader["total_cremacion"]),
                            monto_neto_servicio = DataUtil.DbValueToDefault<decimal>(oIDataReader["total_servicio"]),

                            bono_espacio = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_campo"]),
                            bono_cremacion = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_cremacion"]),
                            bono_servicio = DataUtil.DbValueToDefault<decimal>(oIDataReader["bono_servicio"])

                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        #endregion

        #region Finanzas

        public List<reporte_resumen_supervisores_dto> ReporteComisionSupervisores(reporte_comercial_busqueda_dto busqueda)
        {
            reporte_resumen_supervisores_dto reporte = new reporte_resumen_supervisores_dto();
            List<reporte_resumen_supervisores_dto> lstReporte = new List<reporte_resumen_supervisores_dto>();

            //DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_finanzas");
     

            /*if (!busqueda.fecha_fin.HasValue || !busqueda.fecha_inicio.HasValue)
                return lstReporte;*/
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_resumen_comision_supervisor_chiclayo");

            //oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, busqueda.sede);
            oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, busqueda.codigo_tipo_planilla);
            //oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_reporte", DbType.Int32, busqueda.codigo_tipo_reporte);
            oDatabase.AddInParameter(oDbCommand, "@p_fanio", DbType.Int32, busqueda.codigo_anio);
            oDatabase.AddInParameter(oDbCommand, "@p_mes", DbType.Int32, busqueda.codigo_mes);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_sede", DbType.Int32, System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString()));
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_resumen_supervisores_dto
                        {
                            codigo_personal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_personal"]),
                            apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_paterno"]),
                            apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader["apellido_materno"]),
                            nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]),
                            monto_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_igv"]),
                            monto_neto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]),
                            monto_bruto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]),
                         

                            monto_neto_espacio = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_espacio"]),
                            monto_neto_cremacion = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_cremacion"]),
                            monto_neto_servicio = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_servicio"]),
                            monto_neto_otros = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto_otros"])

                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        public reporte_finanzas_filtro_dto FinanzasFiltro(reporte_finanzas_busqueda_dto busqueda)
        {
            reporte_finanzas_filtro_dto reporte = new reporte_finanzas_filtro_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_finanzas_filtro");
            oDatabase.AddInParameter(oDbCommand, "@p_tipo", DbType.Int32, busqueda.tipo);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.String, busqueda.codigo_tipo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_reporte", DbType.Int32, busqueda.codigo_tipo_reporte);
            oDatabase.AddInParameter(oDbCommand, "@p_periodo", DbType.String, busqueda.periodo);
            oDatabase.AddInParameter(oDbCommand, "@p_anio", DbType.Int32, busqueda.anio);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    if (oIDataReader.Read())
                    {
                        reporte = new reporte_finanzas_filtro_dto();

                        reporte.tipo = DataUtil.DbValueToDefault<string>(oIDataReader["tipo"]);
                        reporte.canal = DataUtil.DbValueToDefault<string>(oIDataReader["canal"]);
                        reporte.tipo_planilla= DataUtil.DbValueToDefault<string>(oIDataReader["tipo_planilla"]);
                        reporte.tipo_reporte = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_reporte"]);
                        reporte.periodo = DataUtil.DbValueToDefault<string>(oIDataReader["periodo"]);
                        reporte.anio = DataUtil.DbValueToDefault<string>(oIDataReader["anio"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return reporte;
        }

        #endregion

        #region "Migracion de Contratos"

        public List<reporte_migracion_contratos_dto> MigracionContratos(reporte_migracion_contratos_busqueda_dto busqueda)
        {
            reporte_migracion_contratos_dto reporte = new reporte_migracion_contratos_dto();
            List<reporte_migracion_contratos_dto> lstReporte = new List<reporte_migracion_contratos_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_migracion_contratos");
            oDbCommand.CommandTimeout = 0;

            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicial", DbType.Int32, busqueda.fecha_inicial);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_final", DbType.Int32, busqueda.fecha_final);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_migracion_contratos_dto
                        {
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            fecha_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_contrato"]),
                            docentry = DataUtil.DbValueToDefault<int>(oIDataReader["docentry"]),
                            estado = DataUtil.DbValueToDefault<string>(oIDataReader["estado"]),
                            fecha_migracion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_migracion"]),
                            nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]),
                            nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]),
                            nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]),
                            intentos = DataUtil.DbValueToDefault<int>(oIDataReader["intentos"])
                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        #endregion  

        #region "Cuotas Iniciales Pendientes"

        public List<reporte_cuotas_iniciales_dto> CuotasIniciales(reporte_cuotas_iniciales_busqueda_dto busqueda)
        {
            reporte_cuotas_iniciales_dto reporte = new reporte_cuotas_iniciales_dto();
            List<reporte_cuotas_iniciales_dto> lstReporte = new List<reporte_cuotas_iniciales_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reporte_cuotas_iniciales");
            oDbCommand.CommandTimeout = 0;

            oDatabase.AddInParameter(oDbCommand, "@p_fecha_inicial", DbType.Int32, busqueda.fecha_inicial);
            oDatabase.AddInParameter(oDbCommand, "@p_fecha_final", DbType.Int32, busqueda.fecha_final);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        reporte = new reporte_cuotas_iniciales_dto
                        {
                            docentry = DataUtil.DbValueToDefault<int>(oIDataReader["docentry"]),
                            nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]),
                            nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]),
                            nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]),
                            nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]),
                            fecha_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_contrato"]),
                            monto_contratado = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_contratado"]),
                            dinero_ingresado = DataUtil.DbValueToDefault<decimal>(oIDataReader["dinero_ingresado"]),
                            comision_habilitada = DataUtil.DbValueToDefault<string>(oIDataReader["comision_habilitada"]),
                            estado_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["estado_proceso"]),
                            observacion_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["observacion_proceso"])
                        };
                        lstReporte.Add(reporte);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstReporte;
        }

        #endregion  

    }
}
