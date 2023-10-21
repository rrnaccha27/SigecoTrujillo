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
    public class ReglaCalculoBonoDA : GenericDA<ReglaCalculoBonoDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<regla_calculo_bono_listado_dto> Listar(int codigo_tipo_planilla)
        {
            
            regla_calculo_bono_listado_dto regla = new regla_calculo_bono_listado_dto();
			List<regla_calculo_bono_listado_dto> lstReglas = new List<regla_calculo_bono_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, codigo_tipo_planilla);

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						regla = new regla_calculo_bono_listado_dto();

                        regla.codigo_regla_calculo_bono = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_calculo_bono"]);
                        regla.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        regla.canal_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["canal_nombre"]);
                        regla.grupo_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["grupo_nombre"]);
                        regla.vigencia_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                        regla.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);
                        regla.estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro"]);
                        regla.calcular_igv = DataUtil.DbValueToDefault<string>(oIDataReader["calcular_igv"]);

                        lstReglas.Add(regla);
					}
				}
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
			
            return lstReglas;
        }

        public regla_calculo_bono_dto Unico(int codigo_regla_calculo_bono)
        {

            regla_calculo_bono_dto regla = new regla_calculo_bono_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_unique");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, codigo_regla_calculo_bono);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        regla = new regla_calculo_bono_dto();

                        regla.codigo_regla_calculo_bono = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_calculo_bono"]);
                        regla.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                        regla.codigo_canal= DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                        regla.codigo_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_grupo"]);
                        regla.vigencia_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                        regla.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);
                        regla.monto_meta = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_meta"]);
                        regla.porcentaje_pago = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_pago"]);
                        regla.monto_tope = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_tope"]);
                        regla.cantidad_ventas = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad_ventas"]);
                        regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        regla.calcular_igv = DataUtil.DbValueToDefault<bool>(oIDataReader["calcular_igv"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return regla;
        }

        public int Insertar(regla_calculo_bono_dto regla)
        {
            int codigo_regla_calculo_bono = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, regla.codigo_tipo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_calcular_igv", DbType.Int32, regla.calcular_igv);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, regla.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, regla.codigo_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, regla.vigencia_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, regla.vigencia_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_monto_meta", DbType.Decimal, regla.monto_meta);
            oDatabase.AddInParameter(oDbCommand, "@p_porcentaje_pago", DbType.Decimal, regla.porcentaje_pago);
            oDatabase.AddInParameter(oDbCommand, "@p_monto_tope", DbType.Decimal, regla.monto_tope);
            oDatabase.AddInParameter(oDbCommand, "@p_cantidad_ventas", DbType.Int32, regla.cantidad_ventas);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, regla.usuario);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_calculo_bono").ToString();
                codigo_regla_calculo_bono = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_regla_calculo_bono;
        }

        public void Actualizar(regla_calculo_bono_dto regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_actualizar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, regla.codigo_regla_calculo_bono);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, regla.codigo_tipo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, regla.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, regla.codigo_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, regla.vigencia_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, regla.vigencia_fin);
            oDatabase.AddInParameter(oDbCommand, "@p_monto_meta", DbType.Decimal, regla.monto_meta);
            oDatabase.AddInParameter(oDbCommand, "@p_porcentaje_pago", DbType.Decimal, regla.porcentaje_pago);
            oDatabase.AddInParameter(oDbCommand, "@p_monto_tope", DbType.Decimal, regla.monto_tope);
            oDatabase.AddInParameter(oDbCommand, "@p_cantidad_ventas", DbType.Int32, regla.cantidad_ventas);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, regla.usuario);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }


        public void Desactivar(regla_calculo_bono_dto regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_desactivar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, regla.codigo_regla_calculo_bono);
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, regla.usuario);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void ArticuloInsertar(regla_calculo_bono_articulo_dto articulo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_articulo_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, articulo.codigo_regla_calculo_bono);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_articulo", DbType.Int32, articulo.codigo_articulo);
            oDatabase.AddInParameter(oDbCommand, "@p_cantidad", DbType.Int32, articulo.cantidad);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void ArticuloEliminar(int codigo_regla_calculo_bono)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_articulo_eliminar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, codigo_regla_calculo_bono);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public List<regla_calculo_bono_articulo_listado_dto> ArticuloListar(int codigo_regla_calculo_bono)
        {
            regla_calculo_bono_articulo_listado_dto articulo = new regla_calculo_bono_articulo_listado_dto();
            List<regla_calculo_bono_articulo_listado_dto> lstArticulos = new List<regla_calculo_bono_articulo_listado_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_articulo_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, codigo_regla_calculo_bono);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        articulo = new regla_calculo_bono_articulo_listado_dto();

                        articulo.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                        articulo.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        articulo.cantidad = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad"]);

                        lstArticulos.Add(articulo);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstArticulos;
        }

        public void MatrizInsertar(regla_calculo_bono_matriz_dto matriz)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_matriz_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, matriz.codigo_regla_calculo_bono);
            oDatabase.AddInParameter(oDbCommand, "@p_porcentaje_meta", DbType.Decimal, matriz.porcentaje_meta);
            oDatabase.AddInParameter(oDbCommand, "@p_monto_meta", DbType.Decimal, matriz.monto_meta);
            oDatabase.AddInParameter(oDbCommand, "@p_porcentaje_pago", DbType.Decimal, matriz.porcentaje_pago);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public void MatrizEliminar(int codigo_regla_calculo_bono)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_matriz_eliminar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, codigo_regla_calculo_bono);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

        public List<regla_calculo_bono_matriz_listado_dto> MatrizListar(int codigo_regla_calculo_bono)
        {
            regla_calculo_bono_matriz_listado_dto matriz = new regla_calculo_bono_matriz_listado_dto();
            List<regla_calculo_bono_matriz_listado_dto> lstMatriz = new List<regla_calculo_bono_matriz_listado_dto>();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_matriz_listado");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, codigo_regla_calculo_bono);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        matriz = new regla_calculo_bono_matriz_listado_dto();

                        matriz.codigo_registro = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_registro"]);

                        matriz.monto_meta = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_meta"]);
                        matriz.porcentaje_meta = DataUtil.DbValueToDefault<string>(oIDataReader["porcentaje_meta"]);
                        matriz.porcentaje_pago = DataUtil.DbValueToDefault<string>(oIDataReader["porcentaje_pago"]);

                        lstMatriz.Add(matriz);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return lstMatriz;
        }

        public regla_calculo_bono_detalle_dto Detalle(int codigo_regla_calculo_bono)
        {

            regla_calculo_bono_detalle_dto regla = new regla_calculo_bono_detalle_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_detalle");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, codigo_regla_calculo_bono);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        regla = new regla_calculo_bono_detalle_dto();

                        regla.codigo_regla_calculo_bono = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_calculo_bono"]);
                        regla.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                        regla.nombre_canal = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal"]);
                        regla.nombre_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_grupo"]);
                        regla.vigencia_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                        regla.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);
                        regla.monto_meta = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_meta"]);
                        regla.porcentaje_pago = DataUtil.DbValueToDefault<decimal>(oIDataReader["porcentaje_pago"]);
                        regla.monto_tope = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto_tope"]);
                        regla.cantidad_ventas = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad_ventas"]);
                        regla.calcular_igv = DataUtil.DbValueToDefault<bool>(oIDataReader["calcular_igv"]);
                        //regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return regla;
        }

        public int Validar(regla_calculo_bono_dto regla)
        {
            int codigo_regla_calculo_bono = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_bono_validar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, regla.codigo_tipo_planilla);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, regla.codigo_canal);
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_grupo", DbType.Int32, regla.codigo_grupo);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, regla.vigencia_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, regla.vigencia_fin);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_calculo_bono", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_calculo_bono").ToString();
                codigo_regla_calculo_bono = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_regla_calculo_bono;
        }
    }
}