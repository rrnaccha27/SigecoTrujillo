using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class MensajeDTO
    {
        public int idOperacion { set; get; }
        public string mensaje { set; get; }
        public string mensaje_wcf { set; get; }

        public Int32 codigoError { set; get; }
        public Int64 idRegistro { set; get; }

        public int total_registro_afectado { set; get; }
    }
}
