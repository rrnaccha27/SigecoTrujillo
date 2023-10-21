using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
    public class log_contrato_sap_listado_dto
	{

		public string nro_contrato { get; set; }
		public string codigo_empresa { get; set; }
		public string nombre_empresa { get; set; }
        public string fecha_contrato { get; set; }
        public string fecha_migracion { get; set; }
		public string fecha_proceso { get; set; }
		public string estado { get; set; }
        public string usuario { get; set; }
        public int codigo_estado_proceso { get; set; }
        public int codigo_empresa_sigeco { get; set; }
        public string identificador_contrato { get; set; }
    }

    [Serializable]
    public class log_contrato_sap_detalle_dto
    {
		public string nro_contrato { get; set; }
		public string nombre_empresa { get; set; }
		public string personal { get; set; }
		public int nro_articulos { get; set; }
		public int nro_cuotas { get; set; }
		public string fecha_contrato { get; set; }
        public string fecha_migracion { get; set; }
		public string estado { get; set; }
		public string fecha_proceso { get; set; }
		public string observacion { get; set; }
        public string tipo_venta { get; set; }
        public string tipo_pago { get; set; }
	}

    [Serializable]
    public class log_contrato_sap_fechas_dto
    {
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string codigo_canal { get; set; }
    }

    
    public class log_contrato_sap_habilitar_dto
    {
        public string codigo_empresa { get; set; }
        public string nro_contrato { get; set; }
    }

    public class log_contrato_sap_bloqueo_dto
    {
        public string nro_contrato { get; set; }
        public int codigo_empresa { get; set; }
        public string usuario_bloqueo { get; set; }
        public int bloqueo { get; set; }
        public string motivo { get; set; }
    }

}
