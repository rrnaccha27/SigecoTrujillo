using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_encofrado_dto
    {
        public int codigo_encofrado { get; set; }
        public string codigo_espacio { get; set; }
        public int codigo_tipo_bien { get; set; }
        public Nullable<System.DateTime> fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
        public string numero_orden_ejecucion { get; set; }
        public string numero_orden_finaliza { get; set; }
        public string observacion_ejecucion { get; set; }
        public string observacion_finaliza { get; set; }
        public string observacion_anulacion { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
        public string str_fecha_registra { get; set; }
        public string str_fecha_modifica { get; set; }

        public string str_fecha_inicio { get; set; }
        public string str_fecha_fin { get; set; }
    }
}
