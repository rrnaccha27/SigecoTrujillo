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
    
    public partial class detalle_atributo
    {
        public int codigo_detalle_atributo { get; set; }
        public int codigo_detalle_entidad { get; set; }
        public string nombre_atributo { get; set; }
        public string valor_atributo { get; set; }
    
        public virtual detalle_entidad detalle_entidad { get; set; }
    }
}
