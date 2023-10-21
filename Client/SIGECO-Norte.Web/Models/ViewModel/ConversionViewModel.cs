using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.ViewModel
{
    public class ConversionViewModel
    {
        public ConversionViewModel() 
        {
            informacion_espacio_dto = new informacion_espacio_dto();
            informacion_conversion_dto = new informacion_conversion_dto();
        }
        public informacion_espacio_dto informacion_espacio_dto { set; get; }
        public informacion_conversion_dto informacion_conversion_dto { set; get; }

        public int p_operacion { set; get; }
        public string p_mensaje { set; get; }
    }
}