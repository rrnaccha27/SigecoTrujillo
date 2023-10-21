using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class motivo_traslado_dto
    {
        public int codigo_tipo_traslado { get; set; }
        public string nombre_tipo_traslado { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
    }
}
