using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_detalle_exhumacion_dto
    {
        public int codigo_detalle_exhumacion { set; get; }
        public string descripcion_proceso_exhumacion { set; get; }
        public bool estado_registro { set; get; }
        public DateTime fecha_registra { set; get; }
        public DateTime? fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public int codigo_exhumacion { set; get; }
        public int codigo_estado_exhumacion { set; get; }
    }
}
