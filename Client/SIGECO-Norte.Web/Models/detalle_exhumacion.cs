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
    
    public partial class detalle_exhumacion
    {
        public int codigo_detalle_exhumacion { get; set; }
        public string descripcion_proceso_exhumacion { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
        public int codigo_exhumacion { get; set; }
        public int codigo_estado_exhumacion { get; set; }
    
        public virtual estado_exhumacion estado_exhumacion { get; set; }
        public virtual exhumacion_fallecido exhumacion_fallecido { get; set; }
    }
}
