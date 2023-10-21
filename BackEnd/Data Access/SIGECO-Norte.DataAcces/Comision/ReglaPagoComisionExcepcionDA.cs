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
    public class ReglaPagoComisionExcepcionDA : GenericDA<ReglaPagoComisionExcepcionDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<regla_pago_comision_excepcion_dto> Listar(regla_pago_comision_excepcion_dto busqueda)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_excepcion_listado");
            oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.String, busqueda.codigo_campo_santo);
            oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.String, busqueda.codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.String, busqueda.codigo_canal_grupo);

            regla_pago_comision_excepcion_dto regla = new regla_pago_comision_excepcion_dto();
            List<regla_pago_comision_excepcion_dto> lstRegla = new List<regla_pago_comision_excepcion_dto>();
            
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    regla = new regla_pago_comision_excepcion_dto();
                    regla.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    regla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    regla.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                    regla.codigo_campo_santo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_campo_santo"]);
                    regla.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                    regla.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                    regla.valor_promocion = DataUtil.DbValueToDefault<int>(oIDataReader["valor_promocion"]);
                    regla.cuotas = DataUtil.DbValueToDefault<int>(oIDataReader["cuotas"]);
                    regla.vigencia_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_inicio"]);
                    regla.vigencia_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_fin"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    regla.fecha_registra = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_registra"]);
                    regla.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);
                    regla.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    regla.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);

                    regla.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                    regla.nombre_campo_santo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_campo_santo"]);
                    regla.nombre_canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal_grupo"]);
                    regla.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                    regla.vigencia_inicio_str = Fechas.convertDateToShortString(regla.vigencia_inicio);
                    regla.vigencia_fin_str = Fechas.convertDateToShortString(regla.vigencia_fin);

                    regla.estado_registro_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_registro_nombre"]);

                    lstRegla.Add(regla);
                }
            }
            return lstRegla;
        }


        public int Insertar(regla_pago_comision_excepcion_dto pEntidad)
        {
            int codigo = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_excepcion_insertar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, pEntidad.nombre);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.Int32, pEntidad.codigo_campo_santo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@valor_promocion", DbType.Int32, pEntidad.valor_promocion);
                oDatabase.AddInParameter(oDbCommand, "@cuotas", DbType.Int32, pEntidad.cuotas);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);

                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);
                oDatabase.AddInParameter(oDbCommand, "@fecha_registra", DbType.DateTime, pEntidad.fecha_registra);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla").ToString();
                codigo = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo;
        }

        public int Actualizar(regla_pago_comision_excepcion_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_excepcion_actualizar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@nombre", DbType.String, pEntidad.nombre);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.Int32, pEntidad.codigo_campo_santo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@valor_promocion", DbType.Int32, pEntidad.valor_promocion);
                oDatabase.AddInParameter(oDbCommand, "@cuotas", DbType.Int32, pEntidad.cuotas);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);

                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);
                oDatabase.AddInParameter(oDbCommand, "@fecha_modifica", DbType.DateTime, pEntidad.fecha_modifica);
                oDatabase.AddInParameter(oDbCommand, "@usuario_modifica", DbType.String, pEntidad.usuario_modifica);

                oDatabase.AddInParameter(oDbCommand, "@codigo_regla", DbType.Int32, pEntidad.codigo_regla);

                oDatabase.ExecuteNonQuery(oDbCommand);
                resultado = pEntidad.codigo_regla;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return resultado;
        }

        public int Eliminar(regla_pago_comision_excepcion_dto pEntidad)
        {
            int resultado = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_excepcion_eliminar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddInParameter(oDbCommand, "@fecha_modifica", DbType.DateTime, pEntidad.fecha_modifica);
                oDatabase.AddInParameter(oDbCommand, "@usuario_modifica", DbType.String, pEntidad.usuario_modifica);
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla", DbType.Int32, pEntidad.codigo_regla);

                oDatabase.ExecuteNonQuery(oDbCommand);
                resultado = pEntidad.codigo_regla;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return resultado;
        }
        public regla_pago_comision_excepcion_dto BuscarById(int codigo_regla_pago)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_excepcion_get_by_id");
            oDatabase.AddInParameter(oDbCommand, "@codigo_regla", DbType.Int32, codigo_regla_pago);
            regla_pago_comision_excepcion_dto regla = null;

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    regla = new regla_pago_comision_excepcion_dto();
                    regla.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    regla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    regla.codigo_empresa = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_empresa"]);
                    regla.codigo_campo_santo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_campo_santo"]);
                    regla.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                    regla.codigo_articulo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_articulo"]);
                    regla.valor_promocion = DataUtil.DbValueToDefault<int>(oIDataReader["valor_promocion"]);
                    regla.cuotas = DataUtil.DbValueToDefault<int>(oIDataReader["cuotas"]);
                    regla.vigencia_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_inicio"]);
                    regla.vigencia_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_fin"]);
                    regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    regla.fecha_registra = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_registra"]);
                    regla.fecha_modifica = DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_modifica"]);
                    regla.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    regla.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);

                    regla.nombre_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_empresa"]);
                    regla.nombre_campo_santo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_campo_santo"]);
                    regla.nombre_canal_grupo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_canal_grupo"]);
                    regla.nombre_articulo = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_articulo"]);

                    regla.vigencia_inicio_str = Fechas.convertDateToShortString(regla.vigencia_inicio);
                    regla.vigencia_fin_str = Fechas.convertDateToShortString(regla.vigencia_fin);

                }
            }


            return regla;
        }

        public int Validar(regla_pago_comision_excepcion_dto pEntidad)
        {
            int cantidadExiste = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_pago_comision_excepcion_validar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla", DbType.Int32, pEntidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.Int32, pEntidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.Int32, pEntidad.codigo_campo_santo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@codigo_articulo", DbType.Int32, pEntidad.codigo_articulo);
                oDatabase.AddInParameter(oDbCommand, "@valor_promocion", DbType.Int32, pEntidad.valor_promocion);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);

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
    }
}