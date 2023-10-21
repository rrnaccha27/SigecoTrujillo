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
    public  class persona_dto
    {
        public persona_dto()
        {
            this.cliente = new HashSet<cliente_dto>();
            this.fallecido = new HashSet<fallecido_dto>();
            this.usuario = new HashSet<UsuarioDTO>();
        }

        public int codigo_persona { get; set; }
        public int codigo_corporacion { get; set; }
        public int codigo_modalidad_persona { get; set; }
        public int codigo_tipo_documento { get; set; }
        public int codigo_estado_civil { get; set; }
        public string nombre_persona { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }
        public string sexo { get; set; }

        public virtual ICollection<cliente_dto> cliente { get; set; }
        public virtual corporacion_dto corporacion { get; set; }
        public virtual estado_civil_dto estado_civil { get; set; }
        public virtual ICollection<fallecido_dto> fallecido { get; set; }
        public virtual modalidad_persona_dto modalidad_persona { get; set; }
        public virtual ICollection<UsuarioDTO> usuario { get; set; }
        public virtual tipo_documento_dto tipo_documento { get; set; }

        public string codigo_usuario { get; set; }

        public string numero_documento { get; set; }

        public string codigo_cliente { get; set; }
    }
}