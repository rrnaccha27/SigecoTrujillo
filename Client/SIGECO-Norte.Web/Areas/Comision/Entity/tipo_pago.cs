//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGEES.Web.Areas.Comision.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tipo_pago
    {
        public tipo_pago()
        {
            this.regla_calculo_comision = new HashSet<regla_calculo_comision>();
            this.cronograma_pago_comision = new HashSet<cronograma_pago_comision>();
        }
    
        public int codigo_tipo_pago { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }
    
        public virtual ICollection<regla_calculo_comision> regla_calculo_comision { get; set; }
        public virtual ICollection<cronograma_pago_comision> cronograma_pago_comision { get; set; }
    }
}
