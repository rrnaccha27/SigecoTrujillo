using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;

namespace SIGEES.DataAcces
{
    public class PeriodoTrimestralDA : GenericDA<PeriodoTrimestralDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<combo_periodo_trimestral_dto> GetAllComboJson()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_periodo_trimestral_combo");
            combo_periodo_trimestral_dto v_entidad;
            List<combo_periodo_trimestral_dto> lst = new List<combo_periodo_trimestral_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new combo_periodo_trimestral_dto();
                    v_entidad.id = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_periodo"]);
                    v_entidad.text = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

    }
}
