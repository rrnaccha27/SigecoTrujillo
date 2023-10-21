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
    
    public partial class edificio
    {
        public edificio()
        {
            this.espacio = new HashSet<espacio>();
        }
    
        public int id_edificio { get; set; }
        public string codigo_edificio { get; set; }
        public int id_sector { get; set; }
        public int secuencia_edificio { get; set; }
        public string nombre_edificio { get; set; }
        public string identificador_edificio { get; set; }
        public int numero_fila { get; set; }
        public int numero_columna { get; set; }
        public bool es_seleccionado { get; set; }
        public bool estado { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }
        public Nullable<int> id_edificio_padre { get; set; }
        public string orientacion_eje_x { get; set; }
        public Nullable<int> codigo_vista_edificio { get; set; }
        public string cara_vista_edificio { get; set; }
        public Nullable<int> codigo_piso_pabellon { get; set; }
    
        public virtual sector sector { get; set; }
        public virtual ICollection<espacio> espacio { get; set; }
    }
}
