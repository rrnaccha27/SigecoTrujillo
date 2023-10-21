using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
	public class personal_canal_grupo_listado_dto
	{
        public int codigo_registro { get; set; }
        public int codigo_personal { get; set; }
		public string codigo_canal { get; set; }		
		public String nombre_canal { get; set; }
		public string codigo_grupo { get; set; }
		public String nombre_grupo { get; set; }
		public string es_supervisor_canal { get; set; }
		public string es_supervisor_grupo { get; set; }
		public string percibe_comision { get; set; }
		public string percibe_bono { get; set; }
        public bool confirmado { get; set; }
        public string estado_registro { get; set; }
	}

    public class personal_canal_grupo_dto
    {
        public int codigo_registro { get; set; }
        public int codigo_personal { get; set; }
        public int codigo_canal_grupo { get; set; }
        public int codigo_canal { get; set; }
        public bool es_supervisor_canal { get; set; }
        public bool es_supervisor_grupo { get; set; }
        public bool percibe_comision { get; set; }
        public bool percibe_bono { get; set; }
        public bool confirmado { get; set; }
        public string estado_registro { get; set; }
        public String usuario_registra { get; set; }
        public DateTime? fecha_registra { get; set; }
        public String usuario_modifica { get; set; }
        public DateTime? fecha_modifica { get; set; }
    }


    public class personal_canal_grupo_replicacion_dto
    {
        public string codigo_equivalencia { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string flag_supervisor { get; set; }
        public string codigo_grupo { get; set; }
        public string nombre_grupo { get; set; }
        public string codigo_canal { get; set; }
        public string nombre_canal { get; set; }
        public string codigo_supervisor { get; set; }
        public string nombre_supervisor { get; set; }
        public string apellido_paterno_supervisor { get; set; }
        public string apellido_materno_supervisor { get; set; }
        public string tipo_operacion { get; set; }
    }



}
