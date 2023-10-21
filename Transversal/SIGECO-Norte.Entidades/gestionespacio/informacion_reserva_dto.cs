using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_reserva_dto
    {
        public informacion_reserva_dto() {
            det_estado_espacio = new detalle_estado_espacio_dto();
        }
        public int ind_operacion { get; set; }
        public int codigo_reserva { get; set; }
        public string codigo_espacio { set; get; }
        public string memo_reserva { get; set; }
        public string str_fecha_reserva { get; set; }
        //public string codigo_cliente { get; set; }
        //public string codigo_usuario { get; set; }
        public int? codigo_producto { get; set; }
        public DateTime fecha_registra { get; set; }
        public DateTime? fecha_modifica { get; set; }
        public string usuario_registra { get; set; }
        public string usuario_modifica { get; set; }

        public virtual detalle_estado_espacio_dto det_estado_espacio { set; get; }

        public DateTime? fecha_caducidad { get; set; }
        /***********************************/



        public bool estado_registro { set; get; }



        /*informaion del cliente*/

        public string numero_documento_cliente { get; set; }
        public string nombre_persona_cliente { get; set; }
        public string apellido_paterno_cliente { get; set; }
        public string apellido_materno_cliente { get; set; }

        /*informaion del vendedor*/
        public int codigo_persona_vendedor { get; set; }
        public string numero_documento_vendedor { get; set; }
        public string nombre_persona_vendedor { get; set; }
        public string apellido_paterno_vendedor { get; set; }
        public string apellido_materno_vendedor { get; set; }

        /*informacion de reserva*/


        public DateTime? fecha_reserva { get; set; }


        /*informacion de corporacion*/

        public int codigo_corporacion { get; set; }
        public string nombre_corporacion { get; set; }

        /*informacion de plataforma*/
        public int codigo_plataforma { get; set; }
        public string nombre_plataforma { get; set; }
        /*informacion de empresa*/

        public int codigo_empresa { get; set; }
        public string nombre_empresa { get; set; }
        /*informacion de empresa*/
        public int codigo_campo_santo { get; set; }
        public string nombre_camposanto { get; set; }
        /* inforacion del estado actual del espacio*/
        public int codigo_estado_espacio { get; set; }
        public string nombre_estado_espacio { get; set; }
        /*informacion del producto*/

        public string nombre_producto { get; set; }
        /*informacion tipo espacio*/
        public int codigo_tipo_espacio { get; set; }
        public string nombre_tipo_espacio { get; set; }

        /*informacion tipo espacio*/
        public string codigo_tipo_nivel { get; set; }
        public string nombre_tipo_nivel { get; set; }

        /*informacion del espacio*/
        public string eje_derecho { get; set; }
        public string eje_superior { get; set; }
        public string eje_inferior { get; set; }
        public string eje_izquierdo { get; set; }
        public int cantidad_reserva_por_vendedor { get; set; }
        public string codigo_autorizacion { set; get; }
        public string observacion_anulacion { set; get; }

        public bool validar_autorizacion { set; get; }

        public int codigo_estado_movimiento_tabla { get; set; }

        public string mensaje_cantidad_maxima_reserva { get; set; }

        public bool reserva_indefinida { get; set; }

        public string motivo_registro { get; set; }

        
    }
}
