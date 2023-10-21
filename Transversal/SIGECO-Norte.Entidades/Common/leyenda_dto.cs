using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades.Common
{
    [Serializable]
   public class leyenda_dto 
    {

        public int fila { set; get; }
        public int columna { set; get; }
       public int ubicacion { set; get; }
       public string eje { set; get; }      
       public string codigo { set; get; }
    }
}
