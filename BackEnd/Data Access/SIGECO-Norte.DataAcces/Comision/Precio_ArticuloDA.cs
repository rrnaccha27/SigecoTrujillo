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
    public class Precio_ArticuloDA : GenericDA<Precio_ArticuloDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<precio_articulo_dto> Listar(precio_articulo_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_precio_articulo_listado_by_articulo");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, busqueda.codigo_articulo);
			
			precio_articulo_dto precio = new precio_articulo_dto();
            List<precio_articulo_dto> lstPrecios = new List<precio_articulo_dto>();
            
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    precio = new precio_articulo_dto();
                    precio.codigo_precio = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_precio"]);
					precio.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
					precio.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                    precio.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
					precio.codigo_tipo_venta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_venta"]);
                    precio.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
					precio.codigo_moneda = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_moneda"]);
                    precio.nombre_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_moneda"]);
					precio.precio = DataUtil.DbValueToDefault<decimal>(oIDataReader["precio"]);
                    precio.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                    precio.precio_total = DataUtil.DbValueToDefault<decimal>(oIDataReader["precio_total"]);
					precio.comisiones = DataUtil.DbValueToDefault<int>(oIDataReader["comisiones"]);
                    precio.cuota_inicial = DataUtil.DbValueToDefault<decimal>(oIDataReader["cuota_inicial"]);
                    

                    precio.tiene_comision = precio.comisiones > 0;
                    precio.vigencia_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_inicio"]);
                    precio.vigencia_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_fin"]);

                    precio.str_vigencia_inicio =Fechas.convertDateToShortString(precio.vigencia_inicio);//.ToShortDateString();
                    precio.str_vigencia_fin = Fechas.convertDateToShortString(precio.vigencia_fin);
                    precio.actualizado = 0;
                    precio.clonarcomisiones = 0;

                    lstPrecios.Add(precio);
                }
            }
            return lstPrecios;
        }


        public int Insertar(precio_articulo_dto pEntidad)
        {
            int codigo_precio_articulo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_precio_articulo_insertar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_precio", DbType.Int32, pEntidad.codigo_precio);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_moneda", DbType.Int32, pEntidad.codigo_moneda);
                oDatabase.AddInParameter(oDbCommand, "@precio", DbType.Decimal, pEntidad.precio);
                oDatabase.AddInParameter(oDbCommand, "@igv", DbType.Decimal, pEntidad.igv);
                oDatabase.AddInParameter(oDbCommand, "@precio_total", DbType.Decimal, pEntidad.precio_total);
                oDatabase.AddInParameter(oDbCommand, "@cuota_inicial", DbType.Decimal, pEntidad.cuota_inicial);

                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_precio_articulo", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_precio_articulo").ToString();
                codigo_precio_articulo = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_precio_articulo;
        }

        public int Clonar(precio_articulo_dto pEntidad)
        {
            int codigo_precio_articulo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_precio_articulo_clonar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_precio", DbType.Int32, pEntidad.codigo_precio);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.Int32, pEntidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_moneda", DbType.Int32, pEntidad.codigo_moneda);
                oDatabase.AddInParameter(oDbCommand, "@precio", DbType.Decimal, pEntidad.precio);
                oDatabase.AddInParameter(oDbCommand, "@igv", DbType.Decimal, pEntidad.igv);
                oDatabase.AddInParameter(oDbCommand, "@precio_total", DbType.Decimal, pEntidad.precio_total);
                oDatabase.AddInParameter(oDbCommand, "@cuota_inicial", DbType.Decimal, pEntidad.cuota_inicial);

                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@clonarcomisiones", DbType.Int32, pEntidad.clonarcomisiones);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_precio_articulo", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_precio_articulo").ToString();
                codigo_precio_articulo = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_precio_articulo;
        }


        public precio_articulo_replicacion_dto BuscarParaReplicacion(int codigo_precio, string tipo_operacion)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_precio_articulo_replicacion");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_precio", DbType.Int32, codigo_precio);

            precio_articulo_replicacion_dto precio = new precio_articulo_replicacion_dto();
            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        precio.codigo_equivalencia = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                        precio.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        precio.abreviatura = DataUtil.DbValueToDefault<string>(oIDataReader["abreviatura"]);
                        precio.codigo_categoria = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_categoria"]);

                        precio.codigo_unidad_negocio = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_unidad_negocio"]);
                        precio.genera_comision = DataUtil.DbValueToDefault<string>(oIDataReader["genera_comision"]);
                        precio.genera_bono = DataUtil.DbValueToDefault<string>(oIDataReader["genera_bono"]);
                        precio.suma_bolsa_bono = DataUtil.DbValueToDefault<string>(oIDataReader["suma_bolsa_bono"]);
                        precio.codigo_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_empresa"]);
                        precio.codigo_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_tipo_venta"]);

                        precio.codigo_moneda = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_moneda"]);
                        precio.precio = DataUtil.DbValueToDefault<decimal>(oIDataReader["precio"]);
                        precio.igv = DataUtil.DbValueToDefault<decimal>(oIDataReader["igv"]);
                        precio.precio_total = DataUtil.DbValueToDefault<decimal>(oIDataReader["precio_total"]);
                        precio.vigencia_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_inicio"]);
                        precio.vigencia_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_fin"]);
                        precio.tipo_operacion = tipo_operacion;
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return precio;
        }
       
    }
}