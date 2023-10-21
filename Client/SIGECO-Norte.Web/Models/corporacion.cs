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
    
    public partial class corporacion
    {
        public corporacion()
        {
            this.campo_santo = new HashSet<campo_santo>();
            this.empresa = new HashSet<empresa>();
            this.espacio = new HashSet<espacio>();
            this.persona = new HashSet<persona>();
            this.plataforma = new HashSet<plataforma>();
        }
    
        public int codigo_corporacion { get; set; }
        public string nombre_corporacion { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual ICollection<campo_santo> campo_santo { get; set; }
        public virtual ICollection<empresa> empresa { get; set; }
        public virtual ICollection<espacio> espacio { get; set; }
        public virtual ICollection<persona> persona { get; set; }
        public virtual ICollection<plataforma> plataforma { get; set; }
    }
}
