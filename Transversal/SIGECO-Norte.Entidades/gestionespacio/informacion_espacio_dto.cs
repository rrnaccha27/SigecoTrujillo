using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_espacio_dto
    {



        public informacion_espacio_dto()
        {

        }

        public string codigo_espacio { set; get; }
        public string codigo_espacio_auxiliar { set; get; }

        public int codigo_corporacion { set; get; }
        public string nombre_corporacion { set; get; }


        public int codigo_empresa { set; get; }
        public string nombre_empresa { set; get; }

        public int codigo_campo_santo { set; get; }
        public string nombre_campo_santo { set; get; }

        public int codigo_plataforma { set; get; }
        public string nombre_plataforma { set; get; }

        public string codigo_sector { set; get; }


        public int codigo_tipo_espacio { set; get; }
        public string nombre_tipo_espacio { set; get; }

        public int codigo_tipo_bien { set; get; }
        public string nombre_tipo_bien { set; get; }

        public bool encofrado { set; get; }
        public bool lapida { set; get; }

        public string codigo_tipo_nivel { set; get; }
        public string nombre_tipo_nivel { set; get; }

        public string eje_derecho { set; get; }
        public string eje_izquierdo { set; get; }
        public string eje_superior { set; get; }
        public string eje_inferior { set; get; }
        public string codigo_plano { set; get; }
        public DateTime fecha_registra { set; get; }
        public DateTime fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }

        public int cantidad_nivel_espacio { set; get; }
        public int cantidad_nivel_ocupado { set; get; }
        public int orden_ubicacion { get; set; }

        /*atrubutos adicionales*/

        public int codigo_estado_espacio { get; set; }
        public string nombre_estado_espacio { get; set; }




        public int codigo_producto { get; set; }
        public string nombre_producto { get; set; }

        public int codigo_tipo_producto { get; set; }
        public string nombre_tipo_producto { get; set; }

        /*indica si el espacio se encuentra en proceso de conversion*/
        public bool en_conversion { get; set; }


        public int numero_filas { get; set; }

        public int numero_columnas { get; set; }

        public int? codigo_venta { get; set; }
        public int? codigo_reserva { get; set; }

        public int cantidad_conversion { get; set; }


        public int nro_columna_grupo { get; set; }

        public int nro_fila_grupo { get; set; }

        public int codigo_agrupador { get; set; }

        public DateTime? fecha_ultimo_entierro { get; set; }

        public bool disponible_conversion { get; set; }

        public int numero_sector { get; set; }

        public bool  reserva_indefinida { get; set; }

        public int cantidad_total_renovado_reserva { get; set; }

        public int limite_validacion_renovacion_reserva { get; set; }

        public string caracteristica_lote { set; get; }
        public bool habilitado { get; set; }

        public int id_sector { get; set; }

        public string cara_nicho { get; set; }

        public bool es_nicho_tipo_yumbo { get; set; }

        public int numero_cara_pabellon { get; set; }

        public int order_ubicacion_sector { get; set; }

        public int codigo_tipo_plataforma { get; set; }

        public int numero_columna_pabellon { get; set; }

        public int numero_pisos_pabellon { get; set; }

        public int? codigo_vista_espacio { get; set; }

        public int? id_sector_padre { get; set; }

        public int cantidad_pisos_sector { get; set; }

        public int codigo_piso_pabellon { get; set; }

        public string observacion { get; set; }

        public string codigo_espacio_visual { get; set; }
    }
}
