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
    
    public partial class conversion_espacio
    {
        public conversion_espacio()
        {
            this.detalle_conversion_espacio = new HashSet<detalle_conversion_espacio>();
        }
    
        public int codigo_conversion_espacio { get; set; }
        public Nullable<System.DateTime> fecha_exhumacion { get; set; }
        public Nullable<int> codigo_tipo_conversion_espacio { get; set; }
        public string numero_informe_exhumacion { get; set; }
        public string observacion_exhumacion { get; set; }
        public Nullable<System.DateTime> fecha_inicio_encofrado { get; set; }
        public string numero_orden_encofrado { get; set; }
        public string observacion_encofrado { get; set; }
        public Nullable<System.DateTime> fecha_inhumacion { get; set; }
        public string numero_orden_inhumacion { get; set; }
        public string observacion_inhumacion { get; set; }
        public Nullable<System.DateTime> fecha_final_ejecucion { get; set; }
        public string numero_orden_ejecucion { get; set; }
        public string observacion_ejecucion { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }
        public Nullable<System.DateTime> fecha_exhumacion_final { get; set; }
        public Nullable<System.DateTime> fecha_encofrado_fin { get; set; }
        public Nullable<System.DateTime> fecha_inhumacion_fin { get; set; }
        public System.DateTime fecha_inicio_conversion { get; set; }
        public Nullable<System.DateTime> fecha_fin_conversion { get; set; }
        public string observacion_conversion { get; set; }
        public string nro_informe_conversion { get; set; }
        public string codigo_espacio { get; set; }
        public Nullable<int> codigo_estado_conversion_actual { get; set; }
    
        public virtual espacio espacio { get; set; }
        public virtual estado_conversion estado_conversion { get; set; }
        public virtual tipo_conversion_espacio tipo_conversion_espacio { get; set; }
        public virtual ICollection<detalle_conversion_espacio> detalle_conversion_espacio { get; set; }
    }
}
