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
    public class ReglaRecuperoDA : GenericDA<ReglaRecuperoDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<regla_recupero_listado_dto> Listar(int estado_registro)
        {
            
            regla_recupero_listado_dto regla = new regla_recupero_listado_dto();
			List<regla_recupero_listado_dto> lstReglas = new List<regla_recupero_listado_dto>();
			
			DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_recupero_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_estado_registro", DbType.Int32, estado_registro);

            try
            {
				using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
				{
					while (oIDataReader.Read())
					{
						regla = new regla_recupero_listado_dto();

                        regla.codigo_regla_recupero = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_recupero"]);
						regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        regla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        regla.estado_nombre = DataUtil.DbValueToDefault<string>(oIDataReader["estado_nombre"]);
                        regla.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
                        regla.vigencia_inicio = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                        regla.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);

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

        public regla_recupero_dto Unico(int codigo_regla_recupero)
        {

            regla_recupero_dto regla = new regla_recupero_dto();

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_recupero_unique");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_recupero", DbType.Int32, codigo_regla_recupero);

            try
            {
                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    while (oIDataReader.Read())
                    {
                        regla = new regla_recupero_dto();

                        regla.codigo_regla_recupero = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_recupero"]);
                        regla.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                        regla.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        regla.nro_cuota = DataUtil.DbValueToDefault<int>(oIDataReader["nro_cuota"]);
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

        public int Insertar(regla_recupero_dto regla)
        {
            int codigo_regla_recupero = 0;

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_recupero_insertar");
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, regla.nombre);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_cuota", DbType.Int32, regla.nro_cuota);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.Date,DateTime.Parse(regla.vigencia_inicio));
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.Date, DateTime.Parse(regla.vigencia_fin));
            oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, regla.usuario);
            oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_recupero", DbType.Int32, 0);

            try
            {
                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_recupero").ToString();
                codigo_regla_recupero = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_regla_recupero;
        }

        public void Actualizar(regla_recupero_dto regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_recupero_actualizar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_recupero", DbType.Int32, regla.codigo_regla_recupero);
            oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, regla.nombre);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_cuota", DbType.Int32, regla.nro_cuota);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, regla.vigencia_inicio);
            oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, regla.vigencia_fin);
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


        public void Desactivar(regla_recupero_dto regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_recupero_desactivar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_recupero", DbType.Int32, regla.codigo_regla_recupero);
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

    }
}