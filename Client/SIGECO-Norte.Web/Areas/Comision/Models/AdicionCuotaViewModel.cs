using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Areas.Comision.Models
{
    public class AdicionCuotaViewModel
    {

        public AdicionCuotaViewModel()
        {
        }
		
		public int codigo_empresa { set; get; }
        public string nro_contrato { set; get; }
        public int codigo_articulo { set; get; }
        public string nombre_articulo { set; get; }

    }
}