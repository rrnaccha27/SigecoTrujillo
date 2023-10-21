using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_exhumacion_dto
    {

        public informacion_exhumacion_dto()
        {
            //informacion_fallecido_dto = new informacion_fallecido_dto();

        }
        public int codigo_exhumacion { get; set; }

        public string str_exhumacion_fecha_inicio { set; get; }
        public DateTime? exhumacion_fecha_inicio { set; get; }
        public string exhumacion_informe_inicio { set; get; }
        public string exhumacion_observacion_inicio { set; get; }

        public string str_exhumacion_fecha_final { get; set; }
        public DateTime? exhumacion_fecha_final { set; get; }

        public string exhumacion_informe_final { set; get; }
        public string exhumacion_observacion_final { set; get; }

        public int codigo_fallecido { set; get; }
        public string codigo_espacio { set; get; }
        public int codigo_estado_exhumacion { set; get; }

        public bool estado_registro { set; get; }
        public string usuario_registra { get; set; }



        public DateTime fecha_registra { get; set; }


        //public informacion_fallecido_dto informacion_fallecido_dto { get; set; }


    }
}

