using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class regla_calculo_comision_validacion_dto
    {
        public int codigo_empresa { get; set; }
        public int codigo_canal { get; set; }
        public int codigo_tipo_venta { get; set; }
        public int codigo_articulo { get; set; }
        public int codigo_tipo_planilla { get; set; }
        public DateTime fecha { get; set; }
    }
}
