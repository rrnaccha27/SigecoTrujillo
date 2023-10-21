using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class regla_bono_trimestral_meta_dto
    {
        public int codigo_regla_meta { set; get; }
        public int codigo_regla{ set; get; }
        public int rango_inicio { set; get; }
        public int rango_fin { set; get; }
        public decimal monto { set; get; }
        public bool estado_registro { set; get; }
        public string nombre_estado_registro { set; get; }
        public string fecha_registra { set; get; }
        public string fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
    }

    public class grilla_regla_bono_trimestral_meta_dto
    {
        public int codigo_regla_meta { set; get; }
        public int codigo_regla { set; get; }
        public int rango_inicio { set; get; }
        public int rango_fin { set; get; }
        public decimal monto { set; get; }
        public string nombre_estado_registro { set; get; }
        public string usuario_registra { set; get; }
    }
}
