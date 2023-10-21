using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class TipoDocumentoDTO
    {
        public int codigoTipoDocumento { get; set; }
        public string nombreTipoDocumento { get; set; }
        public string estadoRegistro { get; set; }
        public DateTime fechaRegistra { get; set; }
        public DateTime fechaModifica { get; set; }
        public String usuarioRegistra { get; set; }
        public String usuarioModifica { get; set; }
    }
}
