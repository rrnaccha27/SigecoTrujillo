using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_cabecera_lapida_dto
    {
        public int codigo_cabecera_lapida { set; get; }
        public string titulo { set; get; }

        public string descripcion { set; get; }
        public bool estado_registro { set; get; }
        public DateTime fecha_registra { set; get; }
        public DateTime? fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public string codigo_espacio { set; get; }
        public int codigo_estado_lapida { set; get; }

        public string nombre_estado_lapida { set; get; }
    }
}
