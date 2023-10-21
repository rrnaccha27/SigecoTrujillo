using SIGEES.Entidades;
using SIGEES.Web.Models.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.ViewModel
{
    public class EspacioViewModel
    {
        public EspacioViewModel()
        {
            informacion_exhumacion_dto = new Entidades.informacion_exhumacion_dto();
            informacion_espacio_dto = new informacion_espacio_dto();
            informacion_conversion_dto = new informacion_conversion_dto();
            informacion_venta_dto = new informacion_venta_dto();
            lst_informacion_fallecido_dto = new List<informacion_fallecido_dto>();
            informacion_fallecido_dto = new informacion_fallecido_dto();
            beanItemTipoAcceso = new BeanItemTipoAcceso();
            //informacion_reserva_dto = new informacion_reserva_dto();
        }

        public informacion_espacio_dto informacion_espacio_dto { set; get; }
        public informacion_exhumacion_dto informacion_exhumacion_dto { set; get; }
        public informacion_conversion_dto informacion_conversion_dto { get; set; }

        public informacion_venta_dto informacion_venta_dto { set; get; }

        public informacion_fallecido_dto informacion_fallecido_dto { set; get; }

        public informacion_encofrado_dto informacion_encofrado_dto { get; set; }

        public List<informacion_fallecido_dto> lst_informacion_fallecido_dto { set; get; }
        public BeanItemTipoAcceso beanItemTipoAcceso { get; set; }

        //public informacion_reserva_dto informacion_reserva_dto { set; get; }
    }
}