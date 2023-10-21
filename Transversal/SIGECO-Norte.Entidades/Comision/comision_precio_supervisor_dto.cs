using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{
    public partial class comision_precio_supervisor_dto : Auditoria_dto
    {
        public int codigo_comision { get; set; }
        public int codigo_precio { get; set; }
        public int codigo_canal_grupo { get; set; }
        public int codigo_tipo_pago { get; set; }
        public int codigo_tipo_comision_supervisor { get; set; }
        public decimal valor { get; set; }
        public DateTime vigencia_inicio { get; set; }
        public DateTime vigencia_fin { get; set; }
        public string str_vigencia_inicio { get; set; }
        public string str_vigencia_fin { get; set; }
        public string nombre_tipo_venta { get; set; }
        public int actualizado { get; set; }
    }
}

