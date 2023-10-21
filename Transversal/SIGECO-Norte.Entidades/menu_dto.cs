using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class menu_dto
    {
        public int codigo_menu { set; get; }
        public int codigo_menu_padre { set; get; }
        public string nombre_menu { set; get; }
        public string ruta_menu { set; get; }
        public bool estado_registro { set; get; }
        public int orden { set; get; }
        public int tipo_orden { set; get; }

    }
}
