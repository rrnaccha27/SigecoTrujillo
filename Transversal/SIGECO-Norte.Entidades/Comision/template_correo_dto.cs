using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class template_correo_dto
    {
        public int codigo_template { get; set; }
        public string nombre { get; set; }
        public int indice { get; set; }
        public string parametro { get; set; }

    }
}
