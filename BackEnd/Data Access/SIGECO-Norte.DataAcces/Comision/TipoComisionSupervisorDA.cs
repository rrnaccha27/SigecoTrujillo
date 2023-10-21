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
using Newtonsoft.Json.Linq;

namespace SIGEES.DataAcces
{
    public class TipoComisionSupervisorDA : GenericDA<TipoComisionSupervisorDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<JObject> Listar()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_tipo_comision_supervisor_listado");

            List<JObject> jObjects = new List<JObject>();
            
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    JObject root = new JObject
                    {
                        {"id", DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_comision_supervisor"])},
                        {"text", DataUtil.DbValueToDefault<string>(oIDataReader["nombre"])},
                    };
                    jObjects.Add(root);

                }
            }
            return jObjects;
        }

    }
}