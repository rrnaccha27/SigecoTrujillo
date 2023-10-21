
namespace SIGEES.Entidades
{

    public partial class detalle_regla_comision_dto
    {

        public int codigo_detalle_regla_comision { get; set; }
        public int codigo_regla_comision { get; set; }
        public decimal? rango_inicio { get; set; }
        public decimal? rango_fin { get; set; }
        public decimal? comision { get; set; }
        public int? codigo_tipo_comision { get; set; }
        public decimal? porcentaje_pago_comision { get; set; }
        public bool existe_condicional { get; set; }
        public decimal? valor_condicion { get; set; }
        public string descripcion_condicion { get; set; }

        public int orden_regla { get; set; }
        public string formula_calculo { get; set; }
        public bool estado_registro { get; set; }
        public string estado_registro_nombre { get; set; }
        public int tipoOperacion { get; set; }
        public string usuario_registra { get; set; }
    }



}

