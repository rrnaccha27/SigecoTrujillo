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
    
    public partial class estado_conversion
    {
        public estado_conversion()
        {
            this.conversion_espacio = new HashSet<conversion_espacio>();
            this.detalle_conversion_espacio = new HashSet<detalle_conversion_espacio>();
        }
    
        public int codigo_estado_conversion { get; set; }
        public string nombre_estado_conversion { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual ICollection<conversion_espacio> conversion_espacio { get; set; }
        public virtual ICollection<detalle_conversion_espacio> detalle_conversion_espacio { get; set; }
    }
}
