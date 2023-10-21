using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public partial class old_regla_pago_comision_dto
    {
 
        public int codigo_regla_pago { get; set; }
        public string nombre_regla_pago { get; set; }

        public int? codigo_empresa { get; set; }
        public int? codigo_campo_santo { get; set; }
        public int? codigo_canal_grupo { get; set; }
        public int? codigo_tipo_venta { get; set; }
        public int? codigo_tipo_pago { get; set; }
        public int? codigo_articulo { get; set; }
        public int? codigo_tipo_articulo { get; set; }
        public int evaluar_plan_integral { get; set; }
        public int evaluar_anexado { get; set; }
        public int? codigo_tipo_articulo_anexado { get; set; }

        public int tipo_pago { get; set; }
        public string valor_inicial_pago { get; set; }
        public string valor_cuota_pago { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }

        public bool estado_registro { get; set; }
        public string estado_registro_nombre { get; set; }

        public DateTime? fecha_registra { get; set; }
        public DateTime? fecha_modifica { get; set; }

        public String usuario_registra { get; set; }
        public String usuario_modifica { get; set; }

        //ATRIBUTOS REFERENCIAS
        public String nombre_empresa { get; set; }
        public String nombre_campo_santo { get; set; }
        public String nombre_tipo_venta { get; set; }
        public String nombre_tipo_pago { get; set; }
        public String nombre_canal_grupo { get; set; }
        public String nombre_articulo { get; set; }
        public String nombre_tipo_articulo { get; set; }
        public String nombre_tipo_articulo_anexado { get; set; }

        public String fecha_inicio_str { get; set; }
        public String fecha_fin_str { get; set; }
        public string vigencia { get; set; }
    }

    public partial class regla_pago_comision_orden_dto
    {
        public int codigo_orden { get; set; }
        public string nombre { get; set; }
        public int orden { get; set; }
        public string usuario_registra { get; set; } 
    }
     
}

