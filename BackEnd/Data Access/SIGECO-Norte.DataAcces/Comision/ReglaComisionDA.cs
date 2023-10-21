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
    public class ReglaComisionDA : GenericDA<ReglaComisionDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<regla_pago_comision_dto> Listar(regla_pago_comision_search_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_listado_regla_comision");
            oDatabase.AddInParameter(oDbCommand, "@codigo_sede", DbType.String, busqueda.codigo_sede);
            oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.String, busqueda.codigo_canal_grupo);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.String, busqueda.codigo_tipo_venta);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo", DbType.String, busqueda.codigo_tipo_articulo);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_planilla", DbType.String, busqueda.codigo_tipo_planilla);


            List<regla_pago_comision_dto> lstRegla = new List<regla_pago_comision_dto>();
            
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                   var regla = new regla_pago_comision_dto();
                    regla.codigo_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_comision"]);
                    regla.nombre_regla_comision = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_comision"]);
                    regla.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                    regla.nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]);
                    regla.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
                    regla.nombre_canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal_grupo"]);
                    regla.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                    
                    regla.estado_registro_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro_nombre"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);


                    lstRegla.Add(regla);
                }
            }
            return lstRegla;
        }
        public regla_pago_comision_dto BuscarById(int codigo_regla_pago)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_regla_comision_by_id");
            oDatabase.AddInParameter(oDbCommand, "@codigo_regla_comision", DbType.Int32, codigo_regla_pago);
            regla_pago_comision_dto regla = null;

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    regla = new regla_pago_comision_dto();

                    regla.codigo_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_comision"]);
                    regla.nombre_regla_comision = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_comision"]);

                    regla.codigo_tipo_venta = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_venta"]);
                    regla.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                    
                    regla.codigo_tipo_articulo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_articulo"]);
                    regla.nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]);

                    regla.codigo_articulo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_articulo"]);
                    regla.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                    regla.codigo_canal_grupo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_canal_grupo"]);
                    regla.nombre_canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal_grupo"]);
                    
                    regla.codigo_tipo_planilla = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_planilla"]);
                    regla.codigo_sede = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_sede"]);

                    regla.tope_minimo_contrato = DataUtil.DbValueToNullable<decimal>(oIDataReader["tope_minimo_contrato"]);
                    regla.tope_unidad = DataUtil.DbValueToNullable<decimal>(oIDataReader["tope_unidad"]);
                    regla.meta_general = DataUtil.DbValueToNullable<decimal>(oIDataReader["meta_general"]);

                    regla.estado_registro_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro_nombre"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);

                }
            }
  

            return regla;
        }

        public int Insertar(regla_pago_comision_dto pEntidad)
        {
            int codigo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("INS_REGLA_COMISION");
            try
            {
               
                oDatabase.AddInParameter(oDbCommand, "@nombre_regla_comision", DbType.String, pEntidad.nombre_regla_comision);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo", DbType.Int32, pEntidad.codigo_tipo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@tope_minimo_contrato", DbType.Decimal, pEntidad.tope_minimo_contrato);
                oDatabase.AddInParameter(oDbCommand, "@tope_unidad", DbType.Decimal, pEntidad.tope_unidad);
                oDatabase.AddInParameter(oDbCommand, "@meta_general", DbType.Decimal, pEntidad.meta_general);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
             
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_planilla", DbType.Int32, pEntidad.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_sede", DbType.Int32, pEntidad.codigo_sede);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_pago", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_pago").ToString();
                codigo = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo;
        }

        public int Actualizar(regla_pago_comision_dto pEntidad)
        {
            int codigo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("UPD_REGLA_COMISION");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_comision", DbType.String, pEntidad.codigo_regla_comision);
                oDatabase.AddInParameter(oDbCommand, "@nombre_regla_comision", DbType.String, pEntidad.nombre_regla_comision);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo", DbType.Int32, pEntidad.codigo_tipo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@tope_minimo_contrato", DbType.Decimal, pEntidad.tope_minimo_contrato);
                oDatabase.AddInParameter(oDbCommand, "@tope_unidad", DbType.Decimal, pEntidad.tope_unidad);
                oDatabase.AddInParameter(oDbCommand, "@meta_general", DbType.Decimal, pEntidad.meta_general);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);

                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_planilla", DbType.Int32, pEntidad.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);                
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                

                oDatabase.ExecuteNonQuery(oDbCommand);
                
                codigo = pEntidad.codigo_regla_comision;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo;
        }
        public int Eliminar(regla_pago_comision_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("DEL_REGLA_COMISION");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_comision", DbType.Int32, pEntidad.codigo_regla_comision);

                oDatabase.ExecuteNonQuery(oDbCommand);
                resultado = pEntidad.codigo_regla_comision;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return resultado;
        }
       

        /*
        public int Validar(old_regla_pago_comision_dto pEntidad)
        {
            int cantidadExiste = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_validar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_pago", DbType.Int32, pEntidad.codigo_regla_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.Int32, pEntidad.codigo_campo_santo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);

                oDatabase.AddInParameter(oDbCommand, "@evaluar_plan_integral", DbType.Int32, pEntidad.evaluar_plan_integral);
                oDatabase.AddInParameter(oDbCommand, "@evaluar_anexado", DbType.Int32, pEntidad.evaluar_anexado);

                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, pEntidad.fecha_inicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, pEntidad.fecha_fin);


                oDatabase.AddOutParameter(oDbCommand, "@cantidad", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@cantidad").ToString();
                cantidadExiste = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return cantidadExiste;
        }


        public List<regla_pago_comision_orden_dto> ListarOrden()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_pago_comision_orden");

            regla_pago_comision_orden_dto regla = new regla_pago_comision_orden_dto();
            List<regla_pago_comision_orden_dto> lstRegla = new List<regla_pago_comision_orden_dto>();
            
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    regla = new regla_pago_comision_orden_dto();
                    regla.codigo_orden = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_orden"]);
                    regla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    regla.orden = DataUtil.DbValueToDefault<int>(oIDataReader["orden"]);
                    regla.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);

                    lstRegla.Add(regla);
                }
            }
            return lstRegla;
        }
        */
    }
}