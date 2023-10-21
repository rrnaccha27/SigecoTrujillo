using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using SIGEES.Entidades;

namespace SIGEES.DataAcces
{

    public class ChecklistComisionTypeCollection : List<checklist_comision_type_dto>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                        new SqlMetaData("codigo_checklist_detalle", SqlDbType.Int)

                  );
            foreach (checklist_comision_type_dto cust in this)
            {
                sqlRow.SetInt32(0, cust.codigo_checklist_detalle);

                yield return sqlRow;
            }
        }
    }

}
