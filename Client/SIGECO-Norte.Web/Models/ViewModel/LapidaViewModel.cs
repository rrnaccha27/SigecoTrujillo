using SIGEES.Entidades;
using SIGEES.Web.Models.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.ViewModel
{
    public class LapidaViewModel
    {
        public LapidaViewModel() 
        {
            informacion_espacio_dto = new informacion_espacio_dto();
            informacion_lapida_dto = new informacion_lapida_dto();
            beanItemTipoAcceso = new BeanItemTipoAcceso();
        }

        public informacion_espacio_dto informacion_espacio_dto { set; get; }
        public informacion_lapida_dto informacion_lapida_dto { set; get; }
        public informacion_cabecera_lapida_dto informacion_cabecera_lapida_dto { set; get; }
        public BeanItemTipoAcceso beanItemTipoAcceso { get; set; }
    }
}