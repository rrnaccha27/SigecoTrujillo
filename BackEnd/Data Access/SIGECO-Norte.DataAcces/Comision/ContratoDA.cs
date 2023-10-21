using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{
    public partial class ContratoDA : GenericDA<ContratoDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public int Generar(analisis_contrato_dto pEntidad)
        {
            int codigo_planilla = 0;
            //DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_planilla_insertar");
            //try
            //{

            //    oDatabase.AddInParameter(oDbCommand, "@codigo_canal", DbType.Int32, pEntidad.codigo_canal);
            //    oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_planilla", DbType.Int32, pEntidad.codigo_tipo_planilla);

            //    oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, pEntidad.fecha_inicio);
            //    oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, pEntidad.fecha_fin);

            //    oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
            //    oDatabase.AddOutParameter(oDbCommand, "@codigo_planilla", DbType.Int32, 20);
            //    oDatabase.AddOutParameter(oDbCommand, "@total_registro_procesado", DbType.Int32, 20);

                

            //    oDatabase.ExecuteNonQuery(oDbCommand);

            //    var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@codigo_planilla").ToString();
            //    var resultado2 = oDatabase.GetParameterValue(oDbCommand, "@total_registro_procesado").ToString();
            //    p_cantidad_registro_procesado = int.Parse(resultado2);
            //    codigo_planilla = int.Parse(resultado1);



            //}
            //finally
            //{
            //    if (oDbCommand != null) oDbCommand.Dispose();
            //    oDbCommand = null;
            //}
            return codigo_planilla;
        }

    }

    public partial class ContratoSelDA : GenericDA<ContratoSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public analisis_contrato_dto BuscarByIdCronograma(int p_codigo_cronograma)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_cabecera");
            var v_entidad = new analisis_contrato_dto();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_cronograma", DbType.Int32, p_codigo_cronograma);  

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {

                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.apellidos_nombres_cliente = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_cliente"]);

                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);
                        v_entidad.apellidos_nombres_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_vendedor"]);
                        v_entidad.apellidos_nombres_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_supervisor"]);
                        v_entidad.nombre_canal_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);                        
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



        public List<analisis_contrato_articulo_cronograma_dto> ListarArticuloByContrato_Empresa(filtro_contrato_dto v_filtro, int codigo_sede)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_articulo");
            var lst = new List<analisis_contrato_articulo_cronograma_dto>();
            
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, v_filtro.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@numero_contrato", DbType.String, v_filtro.numero_contrato);
                oDatabase.AddInParameter(oDbCommand, "@codigo_sede", DbType.Int32, codigo_sede);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new analisis_contrato_articulo_cronograma_dto();
                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.numero_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["numero_contrato"]);
                        v_entidad.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                        v_entidad.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                        v_entidad.codigo_moneda = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_moneda"]);
                        v_entidad.nombre_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_moneda"]);
                        v_entidad.cantidad_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad_articulo"]);

                        //v_entidad.monto_comision_inicial_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_comision_general_personal"]);
                        //v_entidad.monto_comision_inicial_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_comision_general_supervisor"]);
                        //v_entidad.monto_comision_inicial_total = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_comision_general"]);
                        
                        v_entidad.monto_total_comision_vendedor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_comision_vendedor"]);
                        v_entidad.monto_total_pagado_vendedor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_pagado_vendedor"]);
                        v_entidad.monto_total_pagado_vendedor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_pagado_vendedor"]);
                        v_entidad.monto_total_saldo_vendedor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_saldo_vendedor"]);

                        v_entidad.monto_total_comision_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_comision_supervisor"]);
                        v_entidad.monto_total_pagado_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_pagado_supervisor"]);
                        v_entidad.monto_total_saldo_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_saldo_supervisor"]);
                        v_entidad.monto_total_excluido_supervisor = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_total_excluido_supervisor"]);

                        //v_entidad.anulacion_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["anulacion_vendedor"]);
                        //v_entidad.anulacion_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["anulacion_supervisor"]);

                        //v_entidad.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                        v_entidad.es_hr = DataUtil.DbValueToDefault<int>(oIDataReader["es_hr"]);
                        v_entidad.genera_comision = DataUtil.DbValueToDefault<int>(oIDataReader["genera_comision"]);

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

        public List<detalle_cronograma_comision_dto> ListarCronogramaPagoByArticuloContrato(filtro_contrato_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_detalle_cuota");
            var lst = new List<detalle_cronograma_comision_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, v_filtro.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, v_filtro.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_moneda", DbType.Int32, v_filtro.codigo_moneda);
                oDatabase.AddInParameter(oDbCommand, "@nro_contrato", DbType.String, v_filtro.numero_contrato);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new detalle_cronograma_comision_dto();
                        v_entidad.codigo_detalle_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_cronograma"]);
                        v_entidad.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        v_entidad.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        v_entidad.importe_sing_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bruto"]);
                        v_entidad.importe_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_neto"]);
                        v_entidad.fecha_programada = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_programada"]);
                        if (v_entidad.fecha_programada != null)
                        {
                            v_entidad.str_fecha_programada = DateTime.Parse(v_entidad.fecha_programada.ToString()).ToShortDateString();
                        } 
                        

                        v_entidad.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        v_entidad.nombre_estado_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_cuota"]);
                        v_entidad.codigo_estado_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_cuota"]);
                        
                        v_entidad.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);

                        v_entidad.fecha_exclusion = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_exclusion"]);
                        v_entidad.fecha_anulado = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_anulado"]);
                        v_entidad.fecha_cierre = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_cierre"]);
                        v_entidad.es_registro_manual_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["es_registro_manual_comision"]);

                        
                        if (v_entidad.fecha_anulado != null)
                        {
                            v_entidad.str_fecha_anulado = DateTime.Parse(v_entidad.fecha_anulado.ToString()).ToShortDateString();
                        } 
                        if (v_entidad.fecha_cierre != null)
                        {
                            v_entidad.str_fecha_cierre = DateTime.Parse(v_entidad.fecha_cierre.ToString()).ToShortDateString();
                        }  
                        if (v_entidad.fecha_exclusion!=null)
                        {
                            v_entidad.str_fecha_exclusion =DateTime.Parse(v_entidad.fecha_exclusion.ToString()).ToShortDateString();
                        }

                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);

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

        public analisis_contrato_dto BuscarByEmpresaContrato(int codigo_empresa, string nro_contrato,int codigo_sede)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_cabecera");
            var v_entidad = new analisis_contrato_dto();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@nro_contrato", DbType.String, nro_contrato.Trim());
                oDatabase.AddInParameter(oDbCommand, "@codigo_sede", DbType.Int32, codigo_sede);
                
                v_entidad.existe_registro = -1;
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        v_entidad.existe_registro = 1;
                        //v_entidad.codigo_cronograma = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_cronograma"]);
                        v_entidad.nro_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato"]);
                        v_entidad.apellidos_nombres_cliente = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_cliente"]);

                        v_entidad.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                        v_entidad.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);
                        v_entidad.apellidos_nombres_vendedor = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_vendedor"]);
                        v_entidad.apellidos_nombres_supervisor = DataUtil.DbValueToDefault<string>(oIDataReader["apellidos_nombres_supervisor"]);
                        v_entidad.nombre_canal_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.doc_completa = DataUtil.DbValueToDefault<string>(oIDataReader["doc_completa"]);

                        v_entidad.observacion_contrato_migrado = DataUtil.DbValueToDefault<string>(oIDataReader["observacion_contrato_migrado"]);
                        v_entidad.nombre_estado_proceso_migrado = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_proceso_migrado"]);

                        v_entidad.nro_contrato_ref = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato_ref"]);
                        v_entidad.nombre_empresa_ref = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_ref"]);
                        v_entidad.fecha_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_proceso"]);
                        v_entidad.fecha_migracion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_migracion"]);
                        v_entidad.fecha_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_contrato"]);
                        v_entidad.usuario_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_proceso"]);
                        v_entidad.tiene_transferencia = DataUtil.DbValueToDefault<string>(oIDataReader["tiene_transferencia"]);
                        v_entidad.nombre_empresa_transferencia = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa_transferencia"]);
                        v_entidad.nro_contrato_transferencia = DataUtil.DbValueToDefault<string>(oIDataReader["nro_contrato_transferencia"]);
                        v_entidad.monto_transferencia= DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_transferencia"]);

                        v_entidad.bloqueo= DataUtil.DbValueToDefault<string>(oIDataReader["bloqueo"]);
                        v_entidad.usuario_bloqueo = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_bloqueo"]);
                        v_entidad.fecha_bloqueo = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_bloqueo"]);
                        v_entidad.motivo_bloqueo = DataUtil.DbValueToDefault<string>(oIDataReader["motivo_bloqueo"]);
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

        public List<analisis_contrato_cronograma_cuotas_dto> ListarCronogramaCuotasByContrato_Empresa(filtro_contrato_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_cronograma_cuotas");
            var lst = new List<analisis_contrato_cronograma_cuotas_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, v_filtro.numero_contrato);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, v_filtro.codigo_empresa);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new analisis_contrato_cronograma_cuotas_dto();

                        v_entidad.tipo_cuota = DataUtil.DbValueToDefault<string>(oIDataReader["tipo_cuota"]);
                        v_entidad.cuota = DataUtil.DbValueToDefault<int>(oIDataReader["cuota"]);
                        v_entidad.importe_sin_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_sin_igv"]);
                        v_entidad.importe_igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_igv"]);
                        v_entidad.importe_total = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_total"]);
                        v_entidad.fec_vencimiento = DataUtil.DbValueToDefault<string>(oIDataReader["fec_vencimiento"]);
                        v_entidad.fec_pago = DataUtil.DbValueToDefault<string>(oIDataReader["fec_pago"]);
                        v_entidad.estado = DataUtil.DbValueToDefault<string>(oIDataReader["estado"]);

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

        public List<analisis_contrato_combo_dto> ListarEmpresasByContrato(filtro_contrato_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_empresas");
            var lst = new List<analisis_contrato_combo_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, v_filtro.numero_contrato);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new analisis_contrato_combo_dto
                        {
                            id = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]),
                            text = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"])
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

        public List<analisis_contrato_combo_dto> ListarTipoPlanillaByContrato(filtro_contrato_dto v_filtro)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_contrato_analisis_tipo_planilla");
            var lst = new List<analisis_contrato_combo_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, v_filtro.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, v_filtro.numero_contrato);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new analisis_contrato_combo_dto
                        {
                            id = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]),
                            text = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"])
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

    }
}
