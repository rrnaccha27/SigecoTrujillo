using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
   public class empresa_sigeco_dto
    {
        public int codigo_empresa { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre { get; set; }
        public string nombre_largo { get; set; }
        public string ruc { get; set; }
        public string direccion_fiscal { get; set; }
        public bool estado_registro { get; set; }
        public System.DateTime fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }
        public string nro_cuenta { get; set; }
        public Nullable<int> codigo_cuenta_moneda { get; set; }
        public Nullable<int> codigo_tipo_cuenta { get; set; }    
     
    }
}
