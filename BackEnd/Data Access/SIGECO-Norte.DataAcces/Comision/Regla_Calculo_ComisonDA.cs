using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
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
    public class Regla_Calculo_ComisonDA : GenericDA<Regla_Calculo_ComisonDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);


        public int Insertar(regla_calculo_comision_dto pEntidad)
        {
            int codigo_regla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("sp_regla_calculo_comision_insertar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla", DbType.Int32, pEntidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_precio", DbType.Int32, pEntidad.codigo_precio);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal", DbType.Int32, pEntidad.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_comision", DbType.Int32, pEntidad.codigo_tipo_comision);
                oDatabase.AddInParameter(oDbCommand, "@valor", DbType.Decimal, pEntidad.valor);


                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);


                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla").ToString();
                codigo_regla = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_regla;
        }

        public int Clonar(regla_calculo_comision_dto pEntidad)
        {
            int codigo_regla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_comision_clonar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@codigo_regla", DbType.Int32, pEntidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_precio", DbType.Int32, pEntidad.codigo_precio);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal", DbType.Int32, pEntidad.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_comision", DbType.Int32, pEntidad.codigo_tipo_comision);
                oDatabase.AddInParameter(oDbCommand, "@valor", DbType.Decimal, pEntidad.valor);


                oDatabase.AddInParameter(oDbCommand, "@vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@estado_registro", DbType.Boolean, pEntidad.estado_registro);


                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla").ToString();
                codigo_regla = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_regla;
        }

        public List<regla_calculo_comision_dto> ListarByPrecio(int codigo_precio)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_comision_listar_by_precio");
            oDatabase.AddInParameter(oDbCommand, "@codigo_precio", DbType.Int32, codigo_precio);

            regla_calculo_comision_dto comision;
            List<regla_calculo_comision_dto> lstComision = new List<regla_calculo_comision_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    comision = new regla_calculo_comision_dto();
                    comision.codigo_precio = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_precio"]);
                    comision.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    comision.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                    comision.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);

                    comision.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);
                    

                    comision.codigo_tipo_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_comision"]);
                    comision.valor = DataUtil.DbValueToDefault<decimal>(oIDataReader["valor"]);
                    comision.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    
                    comision.vigencia_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_inicio"]);
                    comision.vigencia_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_fin"]);

                    comision.str_vigencia_inicio =Fechas.convertDateToShortString(comision.vigencia_inicio);
                    comision.str_vigencia_fin = Fechas.convertDateToShortString(comision.vigencia_fin);
                    comision.actualizado = 0;

                    lstComision.Add(comision);
                }
            }
            return lstComision;
        }

        public void Desactivar(regla_calculo_comision_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_calculo_comision_desactivar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, pEntidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, pEntidad.usuario_registra);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
        }

    }

}
