using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;

namespace SIGEES.DataAcces
{
    public partial class ChecklistBonoDA : GenericDA<ChecklistBonoDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        #region SECCION OPERACIONES
        public void Aperturar(checklist_bono_estado_dto planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_aperturar");
            oDbCommand.CommandTimeout = 0;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, planilla.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, planilla.usuario);
                
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Cerrar(checklist_bono_estado_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_cerrar");
            oDbCommand.CommandTimeout = 0;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, pEntidad.codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Anular(checklist_bono_estado_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_anular");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, pEntidad.codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void Validar(List<checklist_bono_detalle_listado_dto> checklist_detalle, string usuario_modifica)
        {
            var custColl = new ChecklistComisionTypeCollection();

            foreach (var item in checklist_detalle)
            {
                checklist_comision_type_dto type = new checklist_comision_type_dto();
                type.codigo_checklist_detalle = item.codigo_checklist_detalle;
                custColl.Add(type);
            }

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_detalle_validar");

            try
            {
                var parameter = new SqlParameter("@p_array_checklist_bono_detalle", SqlDbType.Structured)
                {
                    TypeName = "dbo.array_checklist_comision_detalle_type",
                    SqlValue = custColl.Count == 0 ? null : custColl
                };

                oDbCommand.Parameters.Add(parameter);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, usuario_modifica);
                oDatabase.ExecuteNonQuery(oDbCommand);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }
        #endregion

        #region BONO TRIMESTRAL - SECCION OPERACIONES
        public void AperturarTrimestral(checklist_bono_estado_dto planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_aperturar");
            oDbCommand.CommandTimeout = 0;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_planilla", DbType.Int32, planilla.codigo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, planilla.usuario);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void CerrarTrimestral(checklist_bono_estado_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_cerrar");
            oDbCommand.CommandTimeout = 0;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, pEntidad.codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void AnularTrimestral(checklist_bono_estado_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_anular");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, pEntidad.codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void ValidarTrimestral(List<checklist_bono_detalle_listado_dto> checklist_detalle, string usuario_modifica)
        {
            var custColl = new ChecklistComisionTypeCollection();

            foreach (var item in checklist_detalle)
            {
                checklist_comision_type_dto type = new checklist_comision_type_dto();
                type.codigo_checklist_detalle = item.codigo_checklist_detalle;
                custColl.Add(type);
            }

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_detalle_validar");

            try
            {
                var parameter = new SqlParameter("@p_array_checklist_bono_detalle", SqlDbType.Structured)
                {
                    TypeName = "dbo.array_checklist_comision_detalle_type",
                    SqlValue = custColl.Count == 0 ? null : custColl
                };

                oDbCommand.Parameters.Add(parameter);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, usuario_modifica);
                oDatabase.ExecuteNonQuery(oDbCommand);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }
        #endregion
    }

    public partial class ChecklistBonoSelDA : GenericDA<ChecklistBonoSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        #region BONO - SECCION LISTADOS BUSQUEDA

        public List<checklist_bono_listado_dto> Listar()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_listar");
            List<checklist_bono_listado_dto> lst = new List<checklist_bono_listado_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new checklist_bono_listado_dto();

                        v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                        v_entidad.numero_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["numero_checklist"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_estado_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_checklist"]);
                        v_entidad.nombre_estado_checklist= DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_checklist"]);

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

        public checklist_bono_listado_dto Detalle(int codigo_checklist)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_por_codigo");
            var v_entidad = new checklist_bono_listado_dto();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    if (oIDataReader.Read())
                    {
                        v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                        v_entidad.numero_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["numero_checklist"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_estado_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_checklist"]);
                        v_entidad.nombre_estado_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_checklist"]);
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

        public List<checklist_bono_detalle_listado_dto> ListarDetalle(int codigo_checklist, bool validado)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_detalle_listado");
            List<checklist_bono_detalle_listado_dto> lst = new List<checklist_bono_detalle_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_validado", DbType.Int32, validado?1:0);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new checklist_bono_detalle_listado_dto();

                        v_entidad.codigo_checklist_detalle = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist_detalle"]);
                        v_entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        v_entidad.validado = DataUtil.DbValueToDefault<int>(oIDataReader["validado"]);
                        v_entidad.validado_texto = DataUtil.DbValueToDefault<string>(oIDataReader["validado_texto"]);
                        v_entidad.importe_abono_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["importe_abono_personal"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                        v_entidad.ruc_personal = DataUtil.DbValueToDefault<string>(oIDataReader["ruc_personal"]);

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

        #region BONO TRIMESTRAL - SECCION LISTADOS BUSQUEDA

        public List<checklist_bono_listado_dto> ListarTrimestral()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_listar");
            List<checklist_bono_listado_dto> lst = new List<checklist_bono_listado_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new checklist_bono_listado_dto();

                        v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                        v_entidad.numero_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["numero_checklist"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_estado_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_checklist"]);
                        v_entidad.nombre_estado_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_checklist"]);

                        v_entidad.estilo = DataUtil.DbValueToDefault<string>(oIDataReader["estilo"]);
                        v_entidad.nombre_regla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla"]);

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

        public checklist_bono_listado_dto DetalleTrimestral(int codigo_checklist)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_por_codigo");
            var v_entidad = new checklist_bono_listado_dto();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    if (oIDataReader.Read())
                    {
                        v_entidad.codigo_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist"]);
                        v_entidad.numero_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["numero_checklist"]);

                        v_entidad.fecha_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_anulacion"]);
                        v_entidad.usuario_anulacion = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_anulacion"]);

                        v_entidad.fecha_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_apertura"]);
                        v_entidad.usuario_apertura = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_apertura"]);

                        v_entidad.fecha_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_cierre"]);
                        v_entidad.usuario_cierre = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_cierre"]);

                        v_entidad.codigo_estado_checklist = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_checklist"]);
                        v_entidad.nombre_estado_checklist = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_checklist"]);

                        v_entidad.nombre_regla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla"]);
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

        public List<checklist_bono_detalle_listado_dto> ListarDetalleTrimestral(int codigo_checklist, bool validado)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_detalle_listado");
            List<checklist_bono_detalle_listado_dto> lst = new List<checklist_bono_detalle_listado_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);
                oDatabase.AddInParameter(oDbCommand, "@p_validado", DbType.Int32, validado ? 1 : 0);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new checklist_bono_detalle_listado_dto();

                        v_entidad.codigo_checklist_detalle = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_checklist_detalle"]);
                        v_entidad.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        v_entidad.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        v_entidad.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                        v_entidad.nombre_personal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_personal"]);
                        v_entidad.validado = DataUtil.DbValueToDefault<int>(oIDataReader["validado"]);
                        v_entidad.validado_texto = DataUtil.DbValueToDefault<string>(oIDataReader["validado_texto"]);
                        v_entidad.importe_abono_personal = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_bono"]);
                        v_entidad.numero_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["numero_planilla"]);
                        v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
                        v_entidad.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                        v_entidad.ruc_personal = DataUtil.DbValueToDefault<string>(oIDataReader["ruc_personal"]);

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

        public List<cabecera_txt_dto> TXTParaRRHH(int codigo_checklist)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_detalle_txt_rrhh");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);
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

        public List<txt_contabilidad_resumen_planilla_dto> TXTResumenParaContabilidad(int codigo_checklist)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimentral_contabilidad_resumen");
            oDatabase.AddInParameter(oDbCommand, ("@p_codigo_checklist"), DbType.Int32, codigo_checklist);
            List<txt_contabilidad_resumen_planilla_dto> lst = new List<txt_contabilidad_resumen_planilla_dto>();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        var v_entidad = new txt_contabilidad_resumen_planilla_dto();

                        //v_entidad.codigo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_planilla"]);
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


        public List<txt_contabilidad_planilla_dto> TXTParaContabilidad(int codigo_checklist, int codigo_empresa)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_checklist_bono_trimestral_contabilidad");
            oDbCommand.CommandTimeout = 0;
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_checklist", DbType.Int32, codigo_checklist);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, codigo_empresa);
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


        #endregion

    }
}
