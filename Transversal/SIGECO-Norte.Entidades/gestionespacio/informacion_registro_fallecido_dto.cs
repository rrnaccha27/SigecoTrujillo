using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
   public class informacion_registro_fallecido_dto
    {
       public int codigo_fallecido { set; get; }
       public string codigo_espacio { set; get; }
       public int codigo_estado_fallecido { set; get; }
       public int nro_nivel_entierro { set; get; }
       public int indica_operacion { set; get; }

       public string usuario_registra { get; set; }
    }
}
