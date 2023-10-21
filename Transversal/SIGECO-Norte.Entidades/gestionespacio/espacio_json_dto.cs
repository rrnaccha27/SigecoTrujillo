using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class espacio_json_dto
    {
        public espacio_json_dto() {
           // Items = new items_dto();
        }
        public int id_sector { set; get; }
        public string codigo_sector { set; get; }

        public int order_ubicacion_sector { set; get; }

        public int numero_columna_pabellon { set; get; }
        public int numero_pisos_pabellon { set; get; }
        public int numero_Cara_pabellon { set; get; }
        public int numero_pisos { get; set; }
        public List<items_dto> Items { set; get; }

      // public List<items_pisos_dto> Pisos { set; get; }
        
    }


    public partial class items_dto
    {

        public int codigo_piso_pabellon { set; get; }
        public string codigo_espacio { set; get; }
        public int orden_ubicacion { set; get; }
        public bool habilitado { set; get; }
        public int codigo_estado_espacio { set; get; }
        public string cara_nicho { set; get; }
        public List<espacio_json_dto> Items { set; get; }
    }


    //public partial class items_pisos_dto
    //{

    //    public int codigo_piso_pabellon { set; get; }
    //    public List<espacio_json_dto> Items { set; get; }
    //}
}
