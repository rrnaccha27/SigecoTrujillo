using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    [Serializable]
    public class espacio_xml_dto
    {
        public string codigo_espacio { set; get; }
        public string eje_derecho { set; get; }
        public string eje_izquierdo { set; get; }
        public string eje_superior { set; get; }
        public string eje_inferior { set; get; }

        public int eje_columna { set; get; }
        public int eje_fila { set; get; }




        public bool es_nicho_tipo_yumbo { set; get; }
        public int codigo_vista_nicho { set; get; }
        public string cara_nicho { set; get; }
        public int numero_secuencia_sector { set; get; }

    }
}
