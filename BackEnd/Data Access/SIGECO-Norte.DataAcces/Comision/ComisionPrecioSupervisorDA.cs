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
    public class ComisionPrecioSupervisorDA : GenericDA<ComisionPrecioSupervisorDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<comision_precio_supervisor_dto> ListarByPrecio(int codigo_precio)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_precio_supervisor_listar_by_precio");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_precio", DbType.Int32, codigo_precio);

            comision_precio_supervisor_dto comision;
            List<comision_precio_supervisor_dto> lstComision = new List<comision_precio_supervisor_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    comision = new comision_precio_supervisor_dto();
                    comision.codigo_precio = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_precio"]);
                    comision.codigo_comision = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_comision"]);
                    comision.codigo_canal_grupo = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal_grupo"]);
                    comision.codigo_tipo_pago = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_pago"]);

                    comision.nombre_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_venta"]);


                    comision.codigo_tipo_comision_supervisor = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_comision_supervisor"]);
                    comision.valor = DataUtil.DbValueToDefault<decimal>(oIDataReader["valor"]);
                    comision.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);

                    comision.vigencia_inicio = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_inicio"]);
                    comision.vigencia_fin = DataUtil.DbValueToDefault<DateTime>(oIDataReader["vigencia_fin"]);

                    comision.str_vigencia_inicio = Fechas.convertDateToShortString(comision.vigencia_inicio);
                    comision.str_vigencia_fin = Fechas.convertDateToShortString(comision.vigencia_fin);
                    comision.actualizado = 0;

                    lstComision.Add(comision);
                }
            }
            return lstComision;
        }

        public int Insertar(comision_precio_supervisor_dto pEntidad)
        {
            int codigo_comision = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_precio_supervisor_insertar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision", DbType.Int32, pEntidad.codigo_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_precio", DbType.Int32, pEntidad.codigo_precio);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_comision_supervisor", DbType.Int32, pEntidad.codigo_tipo_comision_supervisor);
                oDatabase.AddInParameter(oDbCommand, "@p_valor", DbType.Decimal, pEntidad.valor);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_comision_out", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_comision_out").ToString();
                codigo_comision = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_comision;
        }

        public int Clonar(comision_precio_supervisor_dto pEntidad)
        {
            int codigo_comision = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_precio_supervisor_clonar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision", DbType.Int32, pEntidad.codigo_comision);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_precio", DbType.Int32, pEntidad.codigo_precio);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal_grupo", DbType.Int32, pEntidad.codigo_canal_grupo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_pago", DbType.Int32, pEntidad.codigo_tipo_pago);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_comision_supervisor", DbType.Int32, pEntidad.codigo_tipo_comision_supervisor);
                oDatabase.AddInParameter(oDbCommand, "@p_valor", DbType.Decimal, pEntidad.valor);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.DateTime, pEntidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.DateTime, pEntidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, pEntidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Boolean, pEntidad.estado_registro);

                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_comision_out", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_comision_out").ToString();
                codigo_comision = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_comision;
        }

        public void Desactivar(comision_precio_supervisor_dto pEntidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_comision_precio_supervisor_desactivar");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_comision", DbType.Int32, pEntidad.codigo_comision);
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