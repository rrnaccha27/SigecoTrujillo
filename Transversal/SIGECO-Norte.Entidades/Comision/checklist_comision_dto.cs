using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class checklist_comision_listado_dto
    {
        public int codigo_checklist { get; set; }
        public string numero_checklist { get; set; }
        public int codigo_estado_checklist { get; set; }
        public string nombre_estado_checklist { get; set; }
        public int codigo_regla_tipo_checklist{ get; set; }
        public string nombre_regla_tipo_checklist { get; set; }
        public string fecha_apertura { get; set; }
        public string usuario_apertura { get; set; }
        public string fecha_cierre { get; set; }
        public string usuario_cierre { get; set; }
        public string fecha_anulacion { get; set; }
        public string usuario_anulacion { get; set; }
        public string estilo { get; set; }
    }

    public partial class checklist_comision_estado_dto
    {
        public int codigo_checklist { get; set; }
        public string codigo_planilla { get; set; }
        public int codigo_estado_checklist { get; set; }
        public string fecha { get; set; }
        public string usuario { get; set; }
    }


    public partial class checklist_comision_detalle_listado_dto
    {
        public int codigo_checklist_detalle { get; set; }
        public int codigo_grupo { get; set; }
        public string nombre_grupo { get; set; }
        public string nombre_empresa { get; set; }
        public string nombre_personal { get; set; }
        public int validado { get; set; }
        public string validado_texto { get; set; }
        public decimal importe_abono_personal { get; set; }
        public string numero_planilla { get; set; }
        public string usuario_modifica { get; set; }
        public string fecha_modifica { get; set;}
        public string ruc_personal { get; set; }
    }

    public partial class checklist_comision_type_dto
    {
        public int codigo_checklist_detalle { get; set; }
    }

}
