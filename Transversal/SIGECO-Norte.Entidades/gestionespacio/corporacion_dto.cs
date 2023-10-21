
using System;
using System.Collections.Generic;
namespace SIGEES.Entidades { 
public  class corporacion_dto
{
    public corporacion_dto()
    {
        this.campo_santo = new HashSet<campo_santo_dto>();
        this.empresa = new HashSet<empresa_dto>();
        this.espacio = new HashSet<espacio_dto>();
        this.persona = new HashSet<persona_dto>();
        this.plataforma = new HashSet<plataforma_dto>();
    }

    public int codigo_corporacion { get; set; }
    public string nombre_corporacion { get; set; }
    public bool estado_registro { get; set; }
    public System.DateTime fecha_registra { get; set; }
    public Nullable<System.DateTime> fecha_modifica { get; set; }
    public string usuario_registra { get; set; }
    public string usuario_modifica { get; set; }

    public virtual ICollection<campo_santo_dto> campo_santo { get; set; }
    public virtual ICollection<empresa_dto> empresa { get; set; }
    public virtual ICollection<espacio_dto> espacio { get; set; }
    public virtual ICollection<persona_dto> persona { get; set; }
    public virtual ICollection<plataforma_dto> plataforma { get; set; }
}
}