using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class CampoSantoDTO
    {
        public int codigo_corporacion { set; get; }
        public int codigo_empresa { set; get; }
        public int codigo_campo_santo { set; get; }
        public string nombre_campo_santo { set; get; }
        public string estado_registro { set; get; }
        public DateTime fecha_registro { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public DateTime? fecha_modifica { set; get; }
    }
}
