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
    public  class detalle_producto_dto
    {
        public int codigo_detalle_producto { get; set; }
        public string codigo_espacio { get; set; }
        public int codigo_producto { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public string usuario_registra { get; set; }

        public virtual espacio_dto espacio { get; set; }
        public virtual producto_dto producto { get; set; }
    }
}