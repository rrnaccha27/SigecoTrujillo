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
    
    public partial class encofrado
    {
        public int codigo_encofrado { get; set; }
        public string codigo_espacio { get; set; }
        public int codigo_tipo_bien { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
        public string numero_orden_ejecucion { get; set; }
        public string numero_orden_finaliza { get; set; }
        public string observacion_ejecucion { get; set; }
        public string observacion_finaliza { get; set; }
        public string observacion_anulacion { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual espacio espacio { get; set; }
        public virtual tipo_bien tipo_bien { get; set; }
    }
}
