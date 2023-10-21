using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
    public class log_proceso_bono_listado_dto
	{

		public string id { get; set; }
        public int codigo_planilla { get; set; }
        public string nro_planilla { get; set; }
		public string canal { get; set; }
        public string tipo_planilla { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
		public string usuario { get; set; }
		public string fecha_registra { get; set; }

	}

    public class log_proceso_bono_detalle_dto
    {

		public string nro_contrato { get; set; }
		public string empresa { get; set; }
		public string canal_grupo { get; set; }
		public string estado_nombre { get; set; }
    	public string observacion { get; set; }
        public int codigo_estado { get; set; }
        public decimal monto_ingresado { get; set; }
	}

    public class log_proceso_bono_fechas_dto
    {
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
    }
}
