using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanUser
    {
        public string usuario { get; set; }
        public string clave { get; set; }

        public Nullable<int> codigoPersona { get; set; }
        public Nullable<int> codigoPerfilUsuario { get; set; }

        public string estadoRegistro { get; set; }
    }
}