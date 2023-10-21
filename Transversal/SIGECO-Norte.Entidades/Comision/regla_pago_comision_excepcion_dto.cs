using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public partial class regla_pago_comision_excepcion_dto
    {
 
        public int codigo_regla { get; set; }
        public string nombre { get; set; }

        public int codigo_empresa { get; set; }
        public int codigo_campo_santo { get; set; }
        public int codigo_canal_grupo { get; set; }
        public int codigo_articulo { get; set; }
        public int valor_promocion { get; set; }
        public int cuotas { get; set; }
        public DateTime vigencia_inicio { get; set; }
        public DateTime vigencia_fin { get; set; }

        public bool estado_registro { get; set; }

        public DateTime? fecha_registra { get; set; }
        public DateTime? fecha_modifica { get; set; }

        public String usuario_registra { get; set; }
        public String usuario_modifica { get; set; }

        //ATRIBUTOS REFERENCIAS
        public String nombre_empresa { get; set; }
        public String nombre_campo_santo { get; set; }
        public String nombre_canal_grupo { get; set; }
        public String nombre_articulo { get; set; }

        public String vigencia_inicio_str { get; set; }
        public String vigencia_fin_str { get; set; }

        public string estado_registro_nombre { get; set; }
    }
     
}

