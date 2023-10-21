using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Entidades
{
    public class IdiomaDTO
    {
        public Int32 codigoIdioma { get; set; }
	    public string iso6391 { get; set; }
	    public string nombreIdioma { get; set; }
	    public string estadoRegistro { get; set; }
        public DateTime fechaRegistra { get; set; }
        public DateTime fechaModifica { get; set; }
        public String usuarioRegistra { get; set; }
        public String usuarioModifica { get; set; }
    }
}