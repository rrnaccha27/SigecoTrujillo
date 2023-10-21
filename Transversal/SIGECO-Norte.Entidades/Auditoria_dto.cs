using SIGEES.Entidades.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class Auditoria_dto : PaginacionDTO
    {
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public DateTime fecha_registro { set; get; }
        public DateTime? fecha_modifica { set; get; }

        public bool estado_registro { set; get; }
        public string nombre_estado_registro
        {
            get
            {
                return estado_registro ? "Activo" : "Inactivo";
            }
        }

        public string mensaje_operacion { set; get; }
        public int codigo_tipo_operacion { set; get; }
    }
}
