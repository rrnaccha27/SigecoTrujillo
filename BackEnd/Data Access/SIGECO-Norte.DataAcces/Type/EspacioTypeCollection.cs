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

    public class EspacioTypeCollection : List<informacion_nicho_dto>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                        new SqlMetaData("codigo_espacio", SqlDbType.VarChar, 20),
                        new SqlMetaData("eje_derecho", SqlDbType.VarChar, 10),
                        new SqlMetaData("eje_izquierdo", SqlDbType.VarChar, 10),
                        new SqlMetaData("eje_superior", SqlDbType.VarChar, 10),
                        new SqlMetaData("eje_inferior", SqlDbType.VarChar, 10),
                        new SqlMetaData("eje_columna", SqlDbType.Int),
                        new SqlMetaData("eje_fila", SqlDbType.Int),
                        new SqlMetaData("es_nicho_tipo_yumbo", SqlDbType.Bit),
                        new SqlMetaData("codigo_vista_nicho", SqlDbType.Int),
                        new SqlMetaData("codigo_piso_pabellon", SqlDbType.Int),
                        new SqlMetaData("cara_nicho", SqlDbType.Char, 1),
                        new SqlMetaData("numero_secuencia_sector", SqlDbType.Int),
                         
                        new SqlMetaData("es_leyenda", SqlDbType.Bit),
                        new SqlMetaData("eje_leyenda", SqlDbType.VarChar,5)

                  );
            foreach (informacion_nicho_dto cust in this)
            {
                sqlRow.SetString(0, string.IsNullOrEmpty(cust.codigo_nicho)?" ":cust.codigo_nicho);

                sqlRow.SetString(1, cust.eje_derecho);
                sqlRow.SetString(2, cust.eje_izquierdo);
                sqlRow.SetString(3, cust.eje_superior);
                sqlRow.SetString(4, cust.eje_inferior);

                sqlRow.SetInt32(5, cust.eje_x);
                sqlRow.SetInt32(6, cust.eje_y);
                sqlRow.SetBoolean(7, cust.es_nicho_tipo_yumbo);
                sqlRow.SetInt32(8, cust.codigo_vista_nicho);
                sqlRow.SetInt32(9, cust.codigo_piso_pabellon);
                sqlRow.SetString(10, cust.cara_nicho);
                sqlRow.SetInt32(11, cust.orden_ubicacion_nicho);
                sqlRow.SetBoolean(12, cust.es_leyenda);
                sqlRow.SetString(13, string.IsNullOrEmpty(cust.eje_leyenda) ? " " : cust.eje_leyenda);
                

                yield return sqlRow;
            }
        }
    }
  
}
