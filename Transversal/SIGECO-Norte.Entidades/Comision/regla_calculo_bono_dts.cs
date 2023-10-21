using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
	[Serializable]
	public class regla_calculo_bono_dto
	{
		public int codigo_regla_calculo_bono { get; set; }
		public int codigo_tipo_planilla { get; set; }
		public int codigo_canal { get; set; }
		public int codigo_grupo { get; set; }
		public string vigencia_inicio { get; set; }
		public string vigencia_fin { get; set; }
		public decimal monto_meta { get; set; }
		public decimal porcentaje_pago { get; set; }
		public decimal monto_tope { get; set; }
		public int cantidad_ventas { get; set; }
        public bool calcular_igv { get; set; }

        public Boolean estado_registro { get; set; }
		public string usuario { get; set; }
		
        public List<regla_calculo_bono_matriz_dto> lista_matriz { set; get; }
		public List<regla_calculo_bono_articulo_dto> lista_articulo { set; get; }
	}

    public class regla_calculo_bono_matriz_dto
    {
        public int codigo_regla_calculo_bono { get; set; }
        public decimal porcentaje_meta { get; set; }
        public decimal monto_meta { get; set; }
        public decimal porcentaje_pago { get; set; }
    }

    public class regla_calculo_bono_matriz_listado_dto
    {
        public int codigo_registro { get; set; }
        public string porcentaje_meta { get; set; }
        public decimal monto_meta { get; set; }
        public string porcentaje_pago { get; set; }
    }


    public class regla_calculo_bono_articulo_dto
    {
        public int codigo_regla_calculo_bono { get; set; }
        public int codigo_articulo { get; set; }
        public int cantidad { get; set; }
    }

    public class regla_calculo_bono_articulo_listado_dto
    {
        public int codigo_articulo { get; set; }
        public string nombre_articulo { get; set; }
        public int cantidad { get; set; }
    }

    public class regla_calculo_bono_listado_dto
    {
        public int codigo_regla_calculo_bono { get; set; }
        public string nombre_tipo_planilla { get; set; }
        public string canal_nombre { get; set; }
        public string grupo_nombre { get; set; }
        public string vigencia_inicio { get; set; }
		public string vigencia_fin { get; set; }
        public string estado_registro { get; set; }
        public string calcular_igv { get; set; }
    }

    public class regla_calculo_bono_detalle_dto
    {
        public int codigo_regla_calculo_bono { get; set; }
        public string nombre_tipo_planilla { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public string vigencia_inicio { get; set; }
        public string vigencia_fin { get; set; }
        public decimal monto_meta { get; set; }
        public decimal porcentaje_pago { get; set; }
        public decimal monto_tope { get; set; }
        public int cantidad_ventas { get; set; }
        public bool calcular_igv { get; set; }


        //public Boolean estado_registro { get; set; }
        //public string usuario { get; set; }

        //public List<regla_calculo_bono_matriz_dto> lista_matriz { set; get; }
        //public List<regla_calculo_bono_articulo_dto> lista_articulo { set; get; }
    }


}
