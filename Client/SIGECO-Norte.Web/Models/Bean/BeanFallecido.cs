using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanFallecido
    {
        public int codigoPersona { get; set; }
        public string nombrePersona { get; set; }
        
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string codigoEspacio { get; set; }
        public string nombreTipoFallecido { get; set; }
        public string nombreCampoSanto { get; set; }

        public DateTime fechaRegistra { get; set; }
    }
}