using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class regla_bono_trimestral_dto
    {
        public int codigo_regla { set; get; }
        public int codigo_tipo_bono { get; set; }
        public string nombre_tipo_bono { set; get; }
        public string vigencia_inicio { set; get; }
        public string vigencia_fin { set; get; }
        public string descripcion { set; get; }
        public bool estado_registro { set; get; }
        public string nombre_estado_registro { set; get; }
        public string fecha_registra { set; get; }
        public string fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
    }


    public partial class grilla_regla_bono_trimestral_dto
    {
        public int codigo_regla { set; get; }
        public string descripcion { set; get; }
        //public bool estado_registro { set; get; }
        public string fecha_registra { set; get; }
        public string fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public bool estado_registro { get; set; }
        public string nombre_estado_registro { get; set; }
        public string nombre_tipo_bono { get; set; }
        public string vigencia  { set; get; }
    }

    public partial class combo_regla_bono_trimestral_dto
    {
        public int id { set; get; }
        public string text { set; get; }
      
    }
}
