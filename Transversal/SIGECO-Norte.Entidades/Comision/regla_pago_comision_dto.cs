

namespace SIGEES.Entidades
{

    public partial class regla_pago_comision_dto
    {
        public int? codigo_tipo_venta { get; set; }

        public int codigo_regla_comision { get; set; }
        public string nombre_regla_comision { get; set; }
        public string nombre_tipo_venta { get; set; }
        public string nombre_tipo_articulo { get; set; }
        public string nombre_articulo { get; set; }
        public string nombre_canal_grupo { get; set; }        

        public string estado_registro_nombre { get; set; }
        public bool estado_registro { get; set; }
        public int? codigo_tipo_articulo { get; set; }
        public int? codigo_articulo { get; set; }
        public int? codigo_canal_grupo { get; set; }
        public int? codigo_tipo_planilla { get; set; }
        public int? codigo_sede { get; set; }
        public decimal? tope_minimo_contrato { get; set; }
        public decimal? tope_unidad { get; set; }
        public decimal? meta_general { get; set; }
        public string usuario_registra { get; set; }
        public string nombre_tipo_planilla { get; set; }

        public int tipoOperacion { get; set; }
    }

    
     
}

