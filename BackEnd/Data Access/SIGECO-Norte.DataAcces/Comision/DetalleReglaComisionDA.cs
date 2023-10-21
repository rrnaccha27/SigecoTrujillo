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
    public class DetalleReglaComisionDA : GenericDA<DetalleReglaComisionDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);
        /*
        public List<regla_pago_comision_dto> Listar(regla_pago_comision_search_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_listado_regla_comision");
            oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.String, busqueda.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.String, busqueda.codigo_canal_grupo);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.String, busqueda.codigo_tipo_venta);
            oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo", DbType.String, busqueda.codigo_tipo_articulo);

            
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
                    regla.estado_registro_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro_nombre"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);


                    lstRegla.Add(regla);
                }
            }
            return lstRegla;
        }


      

        public int Eliminar(old_regla_pago_comision_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_eliminar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddInParameter(oDbCommand, "@fecha_modifica", DbType.DateTime, pEntidad.fecha_modifica);
                oDatabase.AddInParameter(oDbCommand, "@usuario_modifica", DbType.String, pEntidad.usuario_modifica);
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_pago", DbType.Int32, pEntidad.codigo_regla_pago);

                oDatabase.ExecuteNonQuery(oDbCommand);
                resultado = pEntidad.codigo_regla_pago;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return resultado;
        }
       

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

        */



        public detalle_regla_comision_dto GetById(int codigo_regla_pago)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_detalle_regla_comision_by_id");
            oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_regla_comision", DbType.Int32, codigo_regla_pago);
            detalle_regla_comision_dto regla = null;

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    regla = new detalle_regla_comision_dto();
                    regla.codigo_detalle_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_regla_comision"]);
                    regla.codigo_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_comision"]);
                    regla.rango_inicio = DataUtil.DbValueToNullable<decimal>(oIDataReader["rango_inicio"]);
                    regla.rango_fin = DataUtil.DbValueToNullable<decimal>(oIDataReader["rango_fin"]);
                    regla.comision = DataUtil.DbValueToNullable<decimal>(oIDataReader["comision"]);
                    regla.codigo_tipo_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_comision"]);
                    regla.porcentaje_pago_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_pago_comision"]);
                    regla.existe_condicional = DataUtil.DbValueToDefault<bool>(oIDataReader["existe_condicional"]);

                    regla.valor_condicion = DataUtil.DbValueToNullable<decimal>(oIDataReader["valor_condicion"]);
                    regla.descripcion_condicion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion_condicion"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    regla.orden_regla = DataUtil.DbValueToDefault<int>(oIDataReader["orden_regla"]);
                    regla.formula_calculo = DataUtil.DbValueToDefault<string>(oIDataReader["formula_calculo"]);

                }
            }


            return regla;
        }
        public List<detalle_regla_comision_dto> GetListByIdRegla(int codigo_regla_pago)
        {

            List<detalle_regla_comision_dto> lstRegla = new List<detalle_regla_comision_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_listado_detalle_regla_comision_by_id");
            oDatabase.AddInParameter(oDbCommand, "@codigo_regla_comision", DbType.Int32, codigo_regla_pago);


            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    var regla = new detalle_regla_comision_dto();
                    regla.codigo_detalle_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_regla_comision"]);
                    regla.codigo_regla_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_comision"]);
                    regla.rango_inicio = DataUtil.DbValueToNullable<decimal>(oIDataReader["rango_inicio"]);
                    regla.rango_fin = DataUtil.DbValueToNullable<decimal>(oIDataReader["rango_fin"]);
                    regla.comision = DataUtil.DbValueToNullable<decimal>(oIDataReader["comision"]);
                    regla.codigo_tipo_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_comision"]);
                    regla.porcentaje_pago_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_pago_comision"]);
                    regla.existe_condicional = DataUtil.DbValueToDefault<bool>(oIDataReader["existe_condicional"]);

                    regla.valor_condicion = DataUtil.DbValueToNullable<decimal>(oIDataReader["valor_condicion"]);
                    regla.descripcion_condicion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion_condicion"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    regla.orden_regla = DataUtil.DbValueToDefault<int>(oIDataReader["orden_regla"]);
                    regla.formula_calculo = DataUtil.DbValueToDefault<string>(oIDataReader["formula_calculo"]);
                    lstRegla.Add(regla);
                }
            }


            return lstRegla;
        }
        public int Insertar(detalle_regla_comision_dto pEntidad)
        {
            int codigo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("INS_DETALLE_REGLA_COMISION");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_comision", DbType.Int32, pEntidad.codigo_regla_comision);
                oDatabase.AddInParameter(oDbCommand, "@rango_inicio", DbType.Decimal, pEntidad.rango_inicio);
                oDatabase.AddInParameter(oDbCommand, "@rango_fin", DbType.Decimal, pEntidad.rango_fin);
                oDatabase.AddInParameter(oDbCommand, "@comision", DbType.Decimal, pEntidad.comision);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_comision", DbType.Int32, pEntidad.codigo_tipo_comision);
                oDatabase.AddInParameter(oDbCommand, "@porcentaje_pago_comision", DbType.Decimal, pEntidad.porcentaje_pago_comision);
                oDatabase.AddInParameter(oDbCommand, "@existe_condicional", DbType.Boolean, pEntidad.existe_condicional);

                oDatabase.AddInParameter(oDbCommand, "@valor_condicion", DbType.Decimal, pEntidad.valor_condicion);
                oDatabase.AddInParameter(oDbCommand, "@descripcion_condicion", DbType.String, pEntidad.descripcion_condicion);
                oDatabase.AddInParameter(oDbCommand, "@orden_regla", DbType.Int32, pEntidad.orden_regla);  
                oDatabase.AddInParameter(oDbCommand, "@formula_calculo", DbType.String, pEntidad.formula_calculo);          
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                oDatabase.AddOutParameter(oDbCommand, "@codigo_detalle_regla_comision", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@codigo_detalle_regla_comision").ToString();
                codigo = int.Parse(resultado1);
          
      
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo;
        }

        public int Actualizar(detalle_regla_comision_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("UPD_DETALLE_REGLA_COMISION");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_detalle_regla_comision", DbType.Int32, pEntidad.codigo_detalle_regla_comision);
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_comision", DbType.Int32, pEntidad.codigo_regla_comision);
                oDatabase.AddInParameter(oDbCommand, "@rango_inicio", DbType.Decimal, pEntidad.rango_inicio);
                oDatabase.AddInParameter(oDbCommand, "@rango_fin", DbType.Decimal, pEntidad.rango_fin);
                oDatabase.AddInParameter(oDbCommand, "@comision", DbType.Decimal, pEntidad.comision);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_comision", DbType.Int32, pEntidad.codigo_tipo_comision);
                oDatabase.AddInParameter(oDbCommand, "@porcentaje_pago_comision", DbType.Decimal, pEntidad.porcentaje_pago_comision);
                oDatabase.AddInParameter(oDbCommand, "@existe_condicional", DbType.Boolean, pEntidad.existe_condicional);

                oDatabase.AddInParameter(oDbCommand, "@valor_condicion", DbType.Decimal, pEntidad.valor_condicion);
                oDatabase.AddInParameter(oDbCommand, "@descripcion_condicion", DbType.String, pEntidad.descripcion_condicion);
                oDatabase.AddInParameter(oDbCommand, "@orden_regla", DbType.Int32, pEntidad.orden_regla);
                oDatabase.AddInParameter(oDbCommand, "@formula_calculo", DbType.String, pEntidad.formula_calculo);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                

                oDatabase.ExecuteNonQuery(oDbCommand);
                resultado = pEntidad.codigo_detalle_regla_comision;

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