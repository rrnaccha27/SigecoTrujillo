using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Web.Models.ViewModel
{
    public class EncofradoViewModel
    {
        public EncofradoViewModel() 
        {
            informacion_espacio_dto = new informacion_espacio_dto();
            informacion_encofrado_dto = new informacion_encofrado_dto();
        }
        public informacion_espacio_dto informacion_espacio_dto { set; get; }
        public informacion_encofrado_dto informacion_encofrado_dto { set; get; }
    }
}
