using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGEES.Web.Models.Bean;

namespace SIGEES.Web.Areas.Comision.Models
{
    public class PlanillaViewModel
    {

        public PlanillaViewModel()
        {
            planilla = new Planilla_dto();
            planilla_bono = new Planilla_bono_dto();
            planilla_bono_trimestral = new planilla_bono_trimestral_dto();
            bean = new BeanItemTipoAcceso();
        }
        public Planilla_dto planilla { set; get; }
        public Planilla_bono_dto planilla_bono { set; get; }
        public planilla_bono_trimestral_dto planilla_bono_trimestral { set; get; }
        public BeanItemTipoAcceso bean { set; get; }
    }
}