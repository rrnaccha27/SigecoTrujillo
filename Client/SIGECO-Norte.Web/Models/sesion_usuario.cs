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
    
    public partial class sesion_usuario
    {
        public int codigo_sesion_usuario { get; set; }
        public string codigo_usuario { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
        public string ip_host { get; set; }
    
        public virtual usuario usuario { get; set; }
    }
}
