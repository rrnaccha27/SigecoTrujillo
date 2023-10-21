using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    [Serializable]
    public class informacion_lapida_dto
    {

        public int codigo_lapida { set; get; }

        
        public DateTime fecha_registra { set; get; }
        public DateTime? fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }


        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombres { set; get; }
        public int edad { set; get; }


        public string txt_fecha_nacimiento { set; get; }
        public string txt_fecha_defuncion { set; get; }
        public DateTime? fecha_nacimiento { set; get; }
        public DateTime? fecha_defuncion { set; get; }
        //public int codigo_nivel_espacio { set; get; }

        public int nro_nivel_nicho { get; set; }


        public string codigo_tipo_nivel { set; get; }
        public bool estado_registro { set; get; }
        public string codigo_espacio { set; get; }
        public int? codigo_fallecido { set; get; }

        public int indica_operacion { get; set; }


        public string apelativo_fallecido { get; set; }
        public string texto_adicional { get; set; }
        public string nombre_tipo_nivel { get; set; }

        public int codigo_lapida_cabecera { get; set; }
    }
}
