using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Areas.Comision.Models
{
    public class ReporteViewModel
    {

        public ReporteViewModel()
        {
            
        }
        public string url { set; get; }
        public int codigo_planilla{set;get;}
        public int codigo_personal { set; get; }
    }
}