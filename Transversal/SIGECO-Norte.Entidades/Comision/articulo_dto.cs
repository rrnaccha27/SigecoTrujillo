using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public partial class articulo_grilla_dto {
        public int codigo_articulo { get; set; }
        public string codigo_sku { get; set; }
        public string nombre_unidad_negocio { get; set; }       
        public string nombre_categoria { get; set; }
        public string nombre { get; set; }
        public string abreviatura { get; set; }


        //public int codigo_categoria { get; set; }
        //public int codigo_unidad_negocio { get; set; }
        //public bool genera_comision { get; set; }
        //public bool genera_bono { get; set; }


        //public bool genera_bolsa_bono { get; set; }

        //public bool tiene_precio { get; set; }
        //public bool tiene_comision { get; set; }
        //public bool estado_registro { set; get; }
        public string str_genera_comision { get; set; }

        public string str_genera_bono { get; set; }

        public string str_tiene_precio { get; set; }

        public string str_tiene_comision { get; set; }

        public string str_estado_registro { get; set; }
        public string str_bolsa_bono { get; set; }
    }
    public partial class articulo_dto : Auditoria_dto
    {
       //public articulo_dto() 
       // {
       //     //unidad_negocio = new items_dto();
       //     //categoria = new items_dto();
       // }
        public int codigo_articulo { get; set; }
        public string codigo_sku { get; set; }
        
        
        public int codigo_unidad_negocio { get; set; }
        public int? anio_contrato_vinculante { get; set; }
        public string nombre_unidad_negocio { get; set; }
        public int codigo_categoria { get; set; }
        public string nombre_categoria { get; set; }
        public string nombre { get; set; }
        public string abreviatura { get; set; }

        public bool genera_comision { get; set; }
        public bool genera_bono { get; set; }
        public bool tiene_contrato_vinculante{ get; set; }

        public bool genera_bolsa_bono { get; set; }

        public bool tiene_precio { get; set; }
        public bool tiene_comision { get; set; }

        public List<precio_articulo_dto> lista_precio_articulo { set; get; }

        public int codigo_tipo_articulo { get; set; }

        public bool cantidad_unica { get; set; }
    }

    //public partial class articulo_contrato_grilla_dto : Auditoria_dto
    //{
    //    public int codigo_empresa { get; set; }
    //    public string codigo_equivalencia_empresa { set; get; }
    //    public string nombre_empresa { set; get; }
    //    public string numero_contrato { set; get; }
    //    public int codigo_campo_santo { set; get; }
    //    public string nombre_campo_santo { set; get; }
    //    public int codigo_articulo { get; set; }
    //    public string codigo_sku { get; set; }
    //    public string nombre_articulo { get; set; }
    //    public string abreviatura { get; set; }
        
    //}

    public class articulo_comision_manual_listado_dto
    { 
		public int  codigo_articulo { get; set; }
		public string nombre { get; set; }
		public string codigo_sku { get; set; }
		public decimal precio { get; set; }
        public decimal monto_comision { get; set; }
    }

    public class articulo_comision_manual_busqueda_dto
    {
        public int? codigo_empresa { get; set; }
        public string nro_contrato { get; set; }
        public string nombre  { get; set; }
        public int codigo_personal { get; set; }
        public int codigo_canal  { get; set; }
        public int codigo_tipo_venta  { get; set; }
        public int codigo_tipo_pago { get; set; }
    }

}
