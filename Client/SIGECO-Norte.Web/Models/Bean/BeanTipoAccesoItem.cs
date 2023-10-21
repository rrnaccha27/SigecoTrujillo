using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanTipoAccesoItem
    {
        public int codigo_tipo_acceso_item { get; set; }
        public string nombre_tipo_acceso_item { get; set; }
        public bool registrado { get; set; }

        public BeanTipoAccesoItem()
        {

        }
    }
}