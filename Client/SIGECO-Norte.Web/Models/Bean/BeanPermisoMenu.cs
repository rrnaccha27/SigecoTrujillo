using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanPermisoMenu
    {
        public int codigoMenu { get; set; }
        public Nullable<int> codigoMenuPadre { get; set; }
        public string nombreMenu { get; set; }
        public string rutaMenu { get; set; }
        public int orden { get; set; }
        public BeanPermisoMenu()
        {

        }
    }
}