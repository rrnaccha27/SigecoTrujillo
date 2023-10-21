using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class sector_piso_pabellon_dto
    {
        public int codigo_sector_piso_pabellon { set; get; }
        public int cantidad_filas { set; get; }
        public int codigo_piso_pabellon { set; get; }
        public int? id_sector { set; get; }
        public bool estado_registro { set; get; }
    }
}
