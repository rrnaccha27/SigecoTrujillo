using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
	public class plan_integral_dto
	{
		public int codigo_plan_integral { get; set; }
		public string nombre { get; set; }
		public bool estado_registro { get; set; }
		public string usuario { get; set; }
        public string vigencia_inicio { get; set; }
        public string vigencia_fin { get; set; }
        public List<plan_integral_detalle_dto> plan_integral_detalle{ get; set; }
	}

    public class plan_integral_listado_dto
    {
        public int codigo_plan_integral { get; set; }
        public string nombre { get; set; }
        public bool estado_registro { get; set; }
        public string estado_nombre { get; set; }
        public string vigencia_inicio { get; set; }
		public string vigencia_fin { get; set; }
    }

    public class plan_integral_detalle_dto
    {
        public int codigo_plan_integral_detalle { get; set; }
        public int codigo_plan_integral { get; set; }
        public int codigo_campo_santo { get; set; }
        public int codigo_tipo_articulo { get; set; }
        public int codigo_tipo_articulo_2 { get; set; }
        public bool estado_registro { get; set; }
        public string estado_registro_nombre { get; set; }
        public string usuario { get; set; }

    }


}
