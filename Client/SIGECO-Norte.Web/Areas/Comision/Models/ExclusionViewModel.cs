using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Areas.Comision.Models
{
    public class ExclusionViewModel
    {

        public ExclusionViewModel()
        {
        }
		
		public int codigo_detalle_planilla { set; get; }
		public int codigo_planilla { set; get; }
    }
}