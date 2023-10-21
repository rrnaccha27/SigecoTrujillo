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
    public class ArticuloDA : GenericDA<ArticuloDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<articulo_grilla_dto> Listar(articulo_dto busqueda,int sede)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, busqueda.nombre);
            oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, sede);

            articulo_grilla_dto articulo = new articulo_grilla_dto();
            List<articulo_grilla_dto> lstArticulo = new List<articulo_grilla_dto>();
            
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    articulo = new articulo_grilla_dto();
                    articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                    articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                    articulo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    articulo.abreviatura = DataUtil.DbValueToDefault<string>(oIDataReader["abreviatura"]);
                    articulo.str_genera_comision = DataUtil.DbValueToDefault<string>(oIDataReader["genera_comision"]);
                    articulo.str_genera_bono = DataUtil.DbValueToDefault<string>(oIDataReader["genera_bono"]);
                    articulo.str_tiene_precio = DataUtil.DbValueToDefault<string>(oIDataReader["tiene_precio"]);
                    articulo.str_tiene_comision = DataUtil.DbValueToDefault<string>(oIDataReader["tiene_comision"]);
                    articulo.str_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro"]);
                    articulo.str_bolsa_bono = DataUtil.DbValueToDefault<string>(oIDataReader["bolsa_bono"]);

                    lstArticulo.Add(articulo);
                }
            }
            return lstArticulo;
        }

        public List<articulo_grilla_dto> ListarBySedeAndTipo(int tipoArticulo, int sede)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sel_listado_articulo_by_sede_tipo");
            oDatabase.AddInParameter(oDbCommand, "@p_tipo_articulo", DbType.Int32, tipoArticulo);
            oDatabase.AddInParameter(oDbCommand, "@p_sede", DbType.Int32, sede);

            articulo_grilla_dto articulo = new articulo_grilla_dto();
            List<articulo_grilla_dto> lstArticulo = new List<articulo_grilla_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    articulo = new articulo_grilla_dto();
                    articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                    articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                    articulo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    articulo.abreviatura = DataUtil.DbValueToDefault<string>(oIDataReader["abreviatura"]);
                    //articulo.str_genera_comision = DataUtil.DbValueToDefault<string>(oIDataReader["genera_comision"]);
                    //articulo.str_genera_bono = DataUtil.DbValueToDefault<string>(oIDataReader["genera_bono"]);
                    //articulo.str_tiene_precio = DataUtil.DbValueToDefault<string>(oIDataReader["tiene_precio"]);
                    //articulo.str_tiene_comision = DataUtil.DbValueToDefault<string>(oIDataReader["tiene_comision"]);
                    //articulo.str_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro"]);
                    //articulo.str_bolsa_bono = DataUtil.DbValueToDefault<string>(oIDataReader["bolsa_bono"]);

                    lstArticulo.Add(articulo);
                }
            }
            return lstArticulo;
        }
        public List<articulo_dto> ListarByFiltro(string codigo_empresa, string nro_contrato, string nombre)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_listado_by_filtro");
            oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.String, codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@nro_contrato", DbType.String, nro_contrato);
            oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, nombre);

            articulo_dto articulo = new articulo_dto();
            List<articulo_dto> lstArticulo = new List<articulo_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    articulo = new articulo_dto();
                    articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                    articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                    articulo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    articulo.abreviatura = DataUtil.DbValueToDefault<string>(oIDataReader["abreviatura"]);
                    lstArticulo.Add(articulo);
                }
            }
            return lstArticulo;
        }

        //public List<articulo_contrato_grilla_dto> ListarArticuloByContratoEmpresa(int codigo_empresa, string nro_contrato, string nombre)
        //{
        //    DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_listado_by_contrato_empresa");
        //    oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, codigo_empresa);
        //    oDatabase.AddInParameter(oDbCommand, "@nro_contrato", DbType.String, nro_contrato.Trim());
        //    oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, string.IsNullOrWhiteSpace(nombre)?null:nombre.Trim());

        //    articulo_contrato_grilla_dto articulo;
        //    List<articulo_contrato_grilla_dto> lstArticulo = new List<articulo_contrato_grilla_dto>();

        //    using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
        //    {
        //        while (oIDataReader.Read())
        //        {
        //            articulo = new articulo_contrato_grilla_dto();
        //            articulo.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
        //            articulo.codigo_equivalencia_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_equivalencia_empresa"]);
        //            articulo.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
        //            articulo.numero_contrato = DataUtil.DbValueToDefault<string>(oIDataReader["numero_contrato"]);
        //            articulo.codigo_campo_santo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_campo_santo"]);
        //            articulo.nombre_campo_santo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_campo_santo"]);                    
        //            articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
        //            articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
        //            articulo.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);
        //            articulo.abreviatura = DataUtil.DbValueToDefault<string>(oIDataReader["abreviatura"]);
        //            lstArticulo.Add(articulo);
        //        }
        //    }
        //    return lstArticulo;
        //}


        public int Insertar(articulo_dto pEntidad)
        {
            int codigo_articulo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_insertar");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_unidad_negocio", DbType.Int32, pEntidad.codigo_unidad_negocio);
                oDatabase.AddInParameter(oDbCommand, "@codigo_categoria", DbType.Int32, pEntidad.codigo_categoria);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_articulo", DbType.Int32, pEntidad.codigo_tipo_articulo);

                oDatabase.AddInParameter(oDbCommand, "@codigo_sku", DbType.String, pEntidad.codigo_sku);
                oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, pEntidad.nombre);
                oDatabase.AddInParameter(oDbCommand, "@abreviatura", DbType.String, pEntidad.abreviatura);

                oDatabase.AddInParameter(oDbCommand, "@genera_comision", DbType.Boolean, pEntidad.genera_comision);
                oDatabase.AddInParameter(oDbCommand, "@genera_bono", DbType.Boolean, pEntidad.genera_bono);
                oDatabase.AddInParameter(oDbCommand, "@genera_bolsa_bono", DbType.Boolean, pEntidad.genera_bolsa_bono);

                oDatabase.AddInParameter(oDbCommand, "@tiene_contrato_vinculante", DbType.Boolean, pEntidad.tiene_contrato_vinculante);
                oDatabase.AddInParameter(oDbCommand, "@anio_contrato_vinculante", DbType.Int32, pEntidad.anio_contrato_vinculante);

                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@cantidad_unica", DbType.Boolean, pEntidad.cantidad_unica);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_articulo").ToString();
                codigo_articulo = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_articulo;
        }

        public articulo_dto BuscarById(int p_codigo_articulo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_get_by_id");
            oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, p_codigo_articulo);
            articulo_dto articulo = new articulo_dto();
           

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {                   
                    articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                    articulo.codigo_unidad_negocio = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_unidad_negocio"]);
                    articulo.codigo_categoria = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_categoria"]);
                    articulo.codigo_tipo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_articulo"]);
                    
                    articulo.anio_contrato_vinculante = DataUtil.DbValueToNullable<int>(oIDataReader["anio_contrato_vinculante"]);

                    articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                    articulo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    articulo.abreviatura = DataUtil.DbValueToDefault<string>(oIDataReader["abreviatura"]);
                    articulo.genera_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["genera_comision"]);
                    articulo.genera_bono = DataUtil.DbValueToDefault<bool>(oIDataReader["genera_bono"]);
                    articulo.genera_bolsa_bono = DataUtil.DbValueToDefault<bool>(oIDataReader["genera_bolsa_bono"]);
                    articulo.tiene_contrato_vinculante = DataUtil.DbValueToDefault<bool>(oIDataReader["tiene_contrato_vinculante"]);
                    articulo.cantidad_unica = DataUtil.DbValueToDefault<bool>(oIDataReader["cantidad_unica"]);

                    articulo.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    articulo.fecha_registro = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                    articulo.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);                   
                }
            }


            return articulo;
        }

        public DataTable ReporteDetalladaByArticulo(int v_codigo_articulo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_reporte_by_id");
            DataTable dt = new DataTable();
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, v_codigo_articulo);

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


        public List<articulo_dto> ListarParaReglaPagoComision(string nombre)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_para_regla_pago_comision");
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, nombre);

            articulo_dto articulo = new articulo_dto();
            List<articulo_dto> lstArticulo = new List<articulo_dto>();

            try
            { 
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        articulo = new articulo_dto();
                        articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                        articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                        articulo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        articulo.genera_comision = DataUtil.DbValueToDefault<bool>(oIDataReader["genera_comision"]);
                        lstArticulo.Add(articulo);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lstArticulo;
        }



        public int Eliminar(articulo_dto v_entidad)
        {
            int codigo_articulo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_eliminar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, v_entidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, v_entidad.usuario_registra);  
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_articulo;
        }

        public int Activar(articulo_dto v_entidad)
        {
            int codigo_articulo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_activar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, v_entidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, v_entidad.usuario_registra);
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_articulo;
        }

        public List<articulo_comision_manual_listado_dto> ListarParaComisionManual(articulo_comision_manual_busqueda_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_articulo_listado_comision_manual");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.String, busqueda.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, busqueda.nro_contrato);
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, busqueda.nombre);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_personal", DbType.String, busqueda.codigo_personal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.String, busqueda.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_venta", DbType.String, busqueda.codigo_tipo_venta);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_pago", DbType.String, busqueda.codigo_tipo_pago);

            articulo_comision_manual_listado_dto articulo = new articulo_comision_manual_listado_dto();
            List<articulo_comision_manual_listado_dto> lstArticulo = new List<articulo_comision_manual_listado_dto>();

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        articulo = new articulo_comision_manual_listado_dto();
                        articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                        articulo.codigo_sku = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_sku"]);
                        articulo.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        articulo.precio = DataUtil.DbValueToDefault<decimal>(oIDataReader["precio"]);
                        articulo.monto_comision = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_comision"]);
                        lstArticulo.Add(articulo);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return lstArticulo;
        }

    }
}