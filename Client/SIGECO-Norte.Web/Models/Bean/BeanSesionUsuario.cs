using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    [Serializable]
    public class BeanSesionUsuario
    {
        public string codigoUsuario { get; set; }
        public int codigoMenu { get; set; }
        public int codigoPerfil { get; set; }
    }
}