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
    public class ReglaPagoComisionDA : GenericDA<ReglaPagoComisionDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<regla_pago_comision_dto> Listar(regla_pago_comision_search_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_listado_regla_comision");
            oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.String, busqueda.codigo_sede);
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


        public int Insertar(old_regla_pago_comision_dto pEntidad)
        {
            int codigo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_insertar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@nombre_regla_pago", DbType.String, pEntidad.nombre_regla_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.Int32, pEntidad.codigo_campo_santo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);

                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo", DbType.Int32, pEntidad.codigo_tipo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@evaluar_plan_integral", DbType.Boolean, (pEntidad.evaluar_plan_integral == 1? true: false));
                oDatabase.AddInParameter(oDbCommand, "@evaluar_anexado", DbType.Boolean, (pEntidad.evaluar_anexado == 1 ? true : false));
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo_anexado", DbType.Int32, pEntidad.codigo_tipo_articulo_anexado);
                
                oDatabase.AddInParameter(oDbCommand, "@tipo_pago", DbType.Int32, pEntidad.tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@valor_inicial_pago", DbType.String, pEntidad.valor_inicial_pago);
                oDatabase.AddInParameter(oDbCommand, "@valor_cuota_pago", DbType.String, pEntidad.valor_cuota_pago);

                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, pEntidad.fecha_inicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, pEntidad.fecha_fin);
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddInParameter(oDbCommand, "@fecha_registra", DbType.DateTime, pEntidad.fecha_registra);
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

        public int Actualizar(old_regla_pago_comision_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_actualizar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@nombre_regla_pago", DbType.String, pEntidad.nombre_regla_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.Int32, pEntidad.codigo_campo_santo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                
                oDatabase.AddInParameter(oDbCommand, "@tipo_pago", DbType.Int32, pEntidad.tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@valor_inicial_pago", DbType.String, pEntidad.valor_inicial_pago);
                oDatabase.AddInParameter(oDbCommand, "@valor_cuota_pago", DbType.String, pEntidad.valor_cuota_pago);

                oDatabase.AddInParameter(oDbCommand, "@fecha_inicio", DbType.DateTime, pEntidad.fecha_inicio);
                oDatabase.AddInParameter(oDbCommand, "@fecha_fin", DbType.DateTime, pEntidad.fecha_fin);
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
        public old_regla_pago_comision_dto BuscarById(int codigo_regla_pago)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_get_by_id");
            oDatabase.AddInParameter(oDbCommand, "@codigo_regla_pago", DbType.Int32, codigo_regla_pago);
            old_regla_pago_comision_dto regla = null;

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    regla = new old_regla_pago_comision_dto();
                    regla.codigo_regla_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_pago"]);
                    regla.nombre_regla_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_regla_pago"]);
                    
                    regla.codigo_empresa = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_empresa"]);
                    regla.codigo_campo_santo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_campo_santo"]);
                    regla.codigo_canal_grupo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_canal_grupo"]);
                    regla.codigo_tipo_venta = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_venta"]);
                    regla.codigo_tipo_pago = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_pago"]);
                    regla.codigo_articulo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_articulo"]);
                    regla.codigo_tipo_articulo = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_articulo"]);
                    regla.codigo_tipo_articulo_anexado = DataUtil.DbValueToNullable<int>(oIDataReader["codigo_tipo_articulo_anexado"]);

                    regla.tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["tipo_pago"]);
                    regla.valor_inicial_pago = DataUtil.DbValueToDefault<string>(oIDataReader["valor_inicial_pago"]);
                    regla.valor_cuota_pago = DataUtil.DbValueToDefault<string>(oIDataReader["valor_cuota_pago"]);
                    regla.fecha_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_inicio"]);
                    regla.fecha_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_fin"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    regla.fecha_registra = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_registra"]);
                    regla.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);
                    regla.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    regla.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);

                    regla.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                    regla.nombre_campo_santo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_campo_santo"]);
                    regla.nombre_canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal_grupo"]);
                    regla.nombre_tipo_pago = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_pago"]);
                    regla.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                    regla.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                    regla.nombre_tipo_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo"]);
                    regla.evaluar_plan_integral = DataUtil.DbValueToDefault<int>(oIDataReader["evaluar_plan_integral_b"]);
                    regla.evaluar_anexado = DataUtil.DbValueToDefault<int>(oIDataReader["evaluar_anexado_b"]);
                    regla.nombre_tipo_articulo_anexado = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_articulo_anexado"]);

                    regla.fecha_inicio_str = Fechas.convertDateToShortString(regla.fecha_inicio);
                    regla.fecha_fin_str = Fechas.convertDateToShortString(regla.fecha_fin);

                }
            }


            return regla;
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

    }
}