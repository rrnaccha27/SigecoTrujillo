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
    
    public partial class tipo_acceso_item
    {
        public tipo_acceso_item()
        {
            this.item_tipo_acceso = new HashSet<item_tipo_acceso>();
        }
    
        public int codigo_tipo_acceso_item { get; set; }
        public string nombre_tipo_acceso_item { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual ICollection<item_tipo_acceso> item_tipo_acceso { get; set; }
    }
}
