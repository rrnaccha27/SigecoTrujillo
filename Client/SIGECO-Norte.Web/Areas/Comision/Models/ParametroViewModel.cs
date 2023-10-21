using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Areas.Comision.Models
{
    public class ParametroViewModel
    {

        public ParametroViewModel()
        {
            
        }

        public int codigo_empresa { set; get; }
        public string numero_contrato { set; get; }
        //public int codigo_planilla{set;get;}
        //public int codigo_personal { set; get; }
    }
}