using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public class operacion_cuota_comision_listado_dto : Auditoria_dto
    {
        public int codigo_operacion_cuota_comision { set; get; }
        public int codigo_detalle_cronograma { set; get; }
        public string nombre_operacion { set; get; }
        public string observacion { set; get; }
        public string fecha_operacion { set; get; }
        public string nombre_estado { set; get; }
        public string usuario{ set; get; }
        public string valor_original { set; get; }
    }

}
