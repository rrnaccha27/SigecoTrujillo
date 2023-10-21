using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_fallecido_dto
    {
        public informacion_fallecido_dto()
        {
            det_estado_espacio_dto = new detalle_estado_espacio_dto();
            registro_fallecido_dto = new informacion_registro_fallecido_dto();
            informacion_espacio_dto = new informacion_espacio_dto();
        }

        public string observacion_traslado { get; set; }
        public string nombre_motivo_traslado { get; set; }
        public string codigo_motivo_traslado { get; set; }
        public string nombre_tipo_documento { get; set; }
        public informacion_espacio_dto informacion_espacio_dto { get; set; }

        public detalle_estado_espacio_dto det_estado_espacio_dto { set; get; }
        public int codigo_fallecido { set; get; }
        public int codigo_persona { set; get; }
        public int codigo_campo_santo { set; get; }
        public string nombre_campo_santo { set; get; }


        public string numero_contrato { set; get; }
        public string estado_registro { set; get; }
        public DateTime fecha_registra { set; get; }
        public DateTime fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public string numero_documento { set; get; }
        public int codigo_tipo_documento { set; get; }
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombres { set; get; }
        public int edad { set; get; }


        public string txt_fecha_nacimiento { set; get; }
        public string txt_fecha_defuncion { set; get; }
        public DateTime? fecha_nacimiento { set; get; }
        public DateTime? fecha_defuncion { set; get; }
        public string codigo_sexo { set; get; }

        public DateTime? fecha_traslado_plaza { set; get; }
        

        public int codigo_tipo_fallecido { set; get; }
        public int nombre_tipo_fallecido { set; get; }

        public int codigo_agencia { set; get; }
        public string nombre_agencia { set; get; }

        public int codigo_nivel_servicio { set; get; }
        public string nombre_nivel_servicio { set; get; }


        public int? codigo_tipo_bien_servicio { set; get; }// contratado
        public string nombre_tipo_bien_servicio { set; get; }

        public int? codigo_tipo_bien { set; get; }
        public string nombre_tipo_bien { set; get; }


        public int? codigo_caracteristica_lote { set; get; }
        public string nombre_caracteristica_lote { set; get; }



         //public int? codigo_catalogo_bien_espacio { set; get; }
         //public int nombre_catalogo_bien_espacio { set; get; }

         //public int? codigo_catalogo_bien_contrato { set; get; }
         //public int nombre_catalogo_bien_contrato { set; get; }
        
        

       


        public int codigo_tipo_traslado { set; get; }
        public string nombre_tipo_traslado { set; get; }


        public int codigo_estado_espacio { set; get; }

        public string codigo_espacio { get; set; }

        public string nombre_sexo { get; set; }

        public string informe { get; set; }
        public int nro_nivel_nicho { get; set; }

        public DateTime? fecha_entierro { get; set; }

        public int codigo_estado_fallecido { get; set; }

        public string nombre_estado_fallecido { get; set; }

        public informacion_registro_fallecido_dto registro_fallecido_dto { get; set; }
        
        public string descripcion_nivel_nicho { get; set; }


        public bool es_tipo_fallecido_cinerario { get; set; }

        public string codigo_espacio_auxiliar { get; set; }

        public bool activado_cinerario { get; set; }
    }
}
