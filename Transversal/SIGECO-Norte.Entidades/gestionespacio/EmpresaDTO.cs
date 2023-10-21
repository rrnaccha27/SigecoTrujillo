using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class EmpresaDTO
    {
        public int codigo_empresa { get; set; }
        public int codigo_corporacion { get; set; }
        public string nombre_empresa { get; set; }
        public string estado_registro { get; set; }
        public DateTime fecha_registro { get; set; }
        public string usuario_registra { get; set; }
        public DateTime? fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }
    }
}
