using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGEES.Web.Models.Bean;

namespace SIGEES.Web.Areas.Comision.Models
{
    public class ChecklistComisionViewModel
    {

        public ChecklistComisionViewModel()
        {
            checklist = new checklist_comision_listado_dto();
            bean = new BeanItemTipoAcceso();
        }
        public checklist_comision_listado_dto checklist { set; get; }
        public BeanItemTipoAcceso bean { set; get; }
    }
}