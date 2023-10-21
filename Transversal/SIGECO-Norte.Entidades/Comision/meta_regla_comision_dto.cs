
namespace SIGEES.Entidades
{

    public partial class meta_regla_comision_dto
    {

        public int codigo_meta_regla_comision { set; get; }
        public int codigo_regla_comision { set; get; }
        public decimal? tope_unidad { set; get; }
        public decimal? tope_unidad_comisionable { set; get; }        
        public decimal? tope_unidad_fin { set; get; }
        public bool estado_registro { set; get; }

        public string estado_registro_nombre { get { return estado_registro ? "Activo" : "Inactivo"; } }
        public string usuario_registra { get; set; }
    }



}

