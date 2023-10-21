using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
	public class regla_recupero_dto
	{
		public int codigo_regla_recupero { get; set; }
		public string nombre { get; set; }
		public int nro_cuota { get; set; }
		public bool estado_registro { get; set; }
		public string usuario { get; set; }
        public string vigencia_inicio { get; set; }
        public string vigencia_fin { get; set; }

	}

    public class regla_recupero_listado_dto
    {
        public int codigo_regla_recupero { get; set; }
        public string nombre { get; set; }
		public int nro_cuota { get; set; }
        public bool estado_registro { get; set; }
        public string estado_nombre { get; set; }
        public string vigencia_inicio { get; set; }
		public string vigencia_fin { get; set; }
    }

}
