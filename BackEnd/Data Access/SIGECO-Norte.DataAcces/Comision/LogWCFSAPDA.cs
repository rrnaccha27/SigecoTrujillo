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
    public class LogWCFSAPDA : GenericDA<LogWCFSAPDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public void Insertar(log_wcf_sap_dto v_entidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_log_wcf_sap_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_objeto", DbType.String, v_entidad.objeto);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_sigeco", DbType.Int32, v_entidad.codigo_sigeco);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_equivalencia", DbType.String, v_entidad.codigo_equivalencia);
                oDatabase.AddInParameter(oDbCommand, "@p_tipo_operacion", DbType.String, v_entidad.tipo_operacion);
                oDatabase.AddInParameter(oDbCommand, "@p_mensaje_excepcion", DbType.String, v_entidad.mensaje_excepcion);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registro", DbType.String, v_entidad.usuario_registro);

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
