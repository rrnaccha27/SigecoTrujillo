using Microsoft.SqlServer.Server;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{

    public class ExclusionIdTypeCollection : List<collection_id_exclusion_dto>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                        new SqlMetaData("codigo_exclusion", SqlDbType.Int)
                  );
            foreach (collection_id_exclusion_dto cust in this)
            {
                sqlRow.SetInt32(0, cust.codigo_exclusion); 
                yield return sqlRow;
            }
        }
    }
  
}
