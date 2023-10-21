using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanEntidad
    {
        public String codigoEntidad { get; set; }
        public String nombreEntidad { get; set; }
        public List<BeanAtributo> listaBeanAtributo { get; set; }

        public BeanEntidad(String codigoEntidad, String nombreEntidad, List<BeanAtributo> listaBeanAtributo)
        {
            this.codigoEntidad = codigoEntidad;
            this.nombreEntidad = nombreEntidad;
            this.listaBeanAtributo = listaBeanAtributo;
        }
        public BeanEntidad()
        {

        }
    }
}