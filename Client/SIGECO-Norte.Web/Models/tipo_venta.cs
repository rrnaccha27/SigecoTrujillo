//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGEES.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tipo_venta
    {
        public tipo_venta()
        {
            this.precio_articulo = new HashSet<precio_articulo>();
        }
    
        public int codigo_tipo_venta { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre { get; set; }
        public string abreviatura { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual ICollection<precio_articulo> precio_articulo { get; set; }
    }
}
