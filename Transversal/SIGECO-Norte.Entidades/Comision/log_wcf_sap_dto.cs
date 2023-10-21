using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
    public class log_wcf_sap_dto
	{

		public string objeto { get; set; }
		public int codigo_sigeco { get; set; }
		public string codigo_equivalencia { get; set; }
        public string tipo_operacion { get; set; }
        public string mensaje_excepcion { get; set; }
		public string usuario_registro { get; set; }
		public string fecha_registro { get; set; }
    }
}
