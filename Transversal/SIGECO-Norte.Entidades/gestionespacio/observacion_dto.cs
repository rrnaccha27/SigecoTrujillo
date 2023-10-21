using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
  public  class observacion_dto
    {

      public int codigo_tipo_operacion { set; get; }
      public string codigo_espacio { set; get; }
      public int codigo_venta { set; get; }
      public int codigo_reserva { set; get; }
      public int codigo_encofrado { get; set; }
      public string codigo_autorizacion { get; set; }
      public string titulo { set; get; }

      public string sub_titulo { get; set; }
    }
}
