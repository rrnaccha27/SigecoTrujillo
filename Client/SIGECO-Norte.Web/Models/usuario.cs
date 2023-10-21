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
    
    public partial class usuario
    {
        public usuario()
        {
            this.clave_usuario = new HashSet<clave_usuario>();
            this.evento_usuario = new HashSet<evento_usuario>();
            this.permiso_empresa = new HashSet<permiso_empresa>();
            this.sesion_usuario = new HashSet<sesion_usuario>();
        }
    
        public string codigo_usuario { get; set; }
        public int codigo_perfil_usuario { get; set; }
        public string estado_registro { get; set; }
        public Nullable<int> codigo_persona { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual ICollection<clave_usuario> clave_usuario { get; set; }
        public virtual ICollection<evento_usuario> evento_usuario { get; set; }
        public virtual perfil_usuario perfil_usuario { get; set; }
        public virtual ICollection<permiso_empresa> permiso_empresa { get; set; }
        public virtual persona persona { get; set; }
        public virtual ICollection<sesion_usuario> sesion_usuario { get; set; }
    }
}
