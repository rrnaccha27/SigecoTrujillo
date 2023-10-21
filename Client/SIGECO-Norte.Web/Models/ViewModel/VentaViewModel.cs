using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.ViewModel
{
    public class VentaViewModel
    {

        public VentaViewModel() 
        {
            informacion_venta_dto = new informacion_venta_dto();
            informacion_espacio_dto = new informacion_espacio_dto();
        }
        public informacion_venta_dto informacion_venta_dto { set; get; }
        public informacion_espacio_dto informacion_espacio_dto { set; get; }
    }
}