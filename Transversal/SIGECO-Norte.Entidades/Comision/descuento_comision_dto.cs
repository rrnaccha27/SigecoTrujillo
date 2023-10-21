using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
	public class descuento_comision_dto
	{
		public int codigo_descuento_comision { get; set; }
		public int codigo_empresa { get; set; }
		public int codigo_personal { get; set; }
		public string motivo { get; set; }
		public decimal monto { get; set; }
		public string usuario { get; set; }
	}

	public class descuento_comision_listado_dto
    {
        public int codigo_descuento_comision { get; set; }
        public string nombre_empresa { get; set; }
        public string nombre_personal { get; set; }
        public decimal monto { get; set; }
        public decimal saldo { get; set; }
        public string nombre_estado_registro { get; set; }
        public int estado_registro { get; set; }
        public string usuario_registra { get; set; }
        public string fecha_registra { get; set; }
    }

    public class descuento_comision_generar_dto
    {
        public int codigo_planilla { get; set; }
        public string usuario { get; set; }
    }

    public class descuento_comision_detalle_dto
    {
        public int codigo_descuento_comision { get; set; }
        public string nombre_empresa { get; set; }
        public string nombre_personal { get; set; }
        public decimal monto { get; set; }
        public decimal saldo { get; set; }
        public string nombre_estado_registro { get; set; }
        public string usuario_registra { get; set; }
        public string fecha_registra { get; set; }
        public string usuario_desactiva { get; set; }
        public string fecha_desactiva { get; set; }
        public string motivo { get; set; }
    }

    public class descuento_comision_planilla_dto
    {
        public string nro_planilla { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string nombre_planilla { get; set; }
        public string nombre_tipo_planilla { get; set; }
        public string estado_planilla { get; set; }
        public decimal monto { get; set; }
        public string usuario_registra { get; set; }
        public string fecha_registra { get; set; }
    }

}
