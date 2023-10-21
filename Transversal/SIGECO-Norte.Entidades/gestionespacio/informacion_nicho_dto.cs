using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
   [Serializable]
     public class informacion_nicho_dto
    {
         public int orden_ubicacion_nicho { set; get; }
         public string codigo_nicho { set; get; }

         public string codigo_espacio_visual { set; get; }
         public int codigo_vista_nicho { set; get; }
         public string cara_nicho { set; get; }
         public int id_sector { set; get; }
        public int id_edificio { set; get; }
         


         public string usuario_registra { get; set; }

         public bool es_nicho_tipo_yumbo { get; set; }

          public int eje_x{ get; set; }
          public int eje_y { get; set; }

          public string eje_derecho { get; set; }

          public string eje_izquierdo { get; set; }

          public string eje_superior { get; set; }

          public string eje_inferior { get; set; }

          public bool habilitado { get; set; }

          public int codigo_piso_pabellon { get; set; }

          public string observacion { get; set; }

          public bool es_leyenda { set; get; }

          public string eje_leyenda { get; set; }

          public int numero_columna { get; set; }

          public int numero_fila { get; set; }
    }
}
