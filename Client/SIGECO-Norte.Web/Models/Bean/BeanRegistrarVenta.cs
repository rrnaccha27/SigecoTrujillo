using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanRegistrarVenta
    {
        public string numeroContrato { get; set; }
        public string codigoEspacio { get; set; }
        public string observacion { get; set; }
        public string fechaVenta { get; set; }
        public string apellidoPaternoCliente { get; set; }
        public string apellidoMaternoCliente { get; set; }
        public string nombreCliente { get; set; }
        public string codigoCatalogoBienContrato { get; set; }
        public string codigoCatalogoBienEspacio { get; set; }
        public string codigoCatacteristicaLote {get; set;}
    }
}