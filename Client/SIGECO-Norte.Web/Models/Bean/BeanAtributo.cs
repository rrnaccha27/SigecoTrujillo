using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanAtributo
    {
        public String valorAtributo { get; set; }
        public String nombreAtributo { get; set; }

        public BeanAtributo(String nombreAtributo, String valorAtributo)
        {
            this.nombreAtributo = nombreAtributo;
            this.valorAtributo = valorAtributo;
        }
        public BeanAtributo()
        {

        }
    }
}