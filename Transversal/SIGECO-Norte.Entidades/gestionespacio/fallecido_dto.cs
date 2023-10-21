//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
namespace SIGEES.Entidades
{
    public  class fallecido_dto
    {
        public fallecido_dto()
        {
            this.registro_fallecido = new HashSet<registro_fallecido_dto>();
            this.registro_incinerado = new HashSet<registro_incinerado_dto>();
        }

        public int codigo_fallecido { get; set; }
        public int codigo_persona { get; set; }
        public int codigo_tipo_fallecido { get; set; }
        public string estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }

        public virtual ICollection<registro_fallecido_dto> registro_fallecido { get; set; }
        public virtual ICollection<registro_incinerado_dto> registro_incinerado { get; set; }
        public virtual persona_dto persona { get; set; }
        public virtual tipo_fallecido_dto tipo_fallecido { get; set; }
    }
}