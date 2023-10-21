using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public class precio_articulo_dto:Auditoria_dto
    {
        public int codigo_precio { get; set; }
		public int codigo_articulo { get; set; }
		public int codigo_empresa { get; set; }
        public int codigo_tipo_venta { get; set; }
        public int codigo_moneda { get; set; }
        public decimal precio { get; set; }
         public decimal igv { get; set; }
         public decimal precio_total { get; set; }
         public decimal cuota_inicial { get; set; }

        public string nombre_empresa { get; set; }		
        public string nombre_tipo_venta { get; set; }
		
        public string nombre_moneda { get; set; }
   
        public DateTime vigencia_inicio { get; set; }
        public DateTime vigencia_fin { get; set; }
        public int comisiones { get; set; }
        public bool tiene_comision { get; set; }
        public string str_vigencia_inicio { get; set; }
        public string str_vigencia_fin { get; set; }

        public int actualizado { get; set; }
        public int clonarcomisiones { get; set; }

        public List<regla_calculo_comision_dto> lst_regla_calcula_comision { get; set; }
        public List<comision_precio_supervisor_dto> lst_comision_supervisor { get; set; }
    }

    public class precio_articulo_replicacion_dto
    {
        public string codigo_equivalencia { get; set; }
        public string nombre { get; set; }
        public string abreviatura { get; set; }
        public string codigo_categoria { get; set; }
        public string codigo_unidad_negocio { get; set; }
        public string genera_comision { get; set; }
        public string genera_bono { get; set; }
        public string suma_bolsa_bono { get; set; }
        public string codigo_empresa { get; set; }
        public string codigo_tipo_venta { get; set; }
        public string codigo_moneda { get; set; }
        public decimal precio { get; set; }
        public decimal igv { get; set; }
        public decimal precio_total { get; set; }
        public DateTime vigencia_inicio { get; set; }
        public DateTime vigencia_fin { get; set; }
        public string tipo_operacion { get; set; }
    }

}
