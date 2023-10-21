using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_venta_dto
    {

        public informacion_venta_dto() {
            det_estado_espacio = new detalle_estado_espacio_dto();
        }
        public int codigo_venta { set; get; }
        public string codigo_cliente { set; get; }
        public string codigo_usuario { set; get; }
        public int codigo_documento { set; get; }
        public int? codigo_producto { set; get; }
        public int codigo_reserva { set; get; }
        public bool estado_registro { set; get; }
        public string numero_contrato { set; get; }
        public string codigo_espacio { set; get; }
        public string observacion { set; get; }
        public DateTime? fecha_registra { set; get; }
        public DateTime? fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }

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

        public string memo_reserva { get; set; }
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


        public DateTime? fecha_venta { get; set; }

        public string str_fecha_venta { get; set; }

        public string codigo_autorizacion { get; set; }
        public string observacion_anulacion { get; set; }
        public detalle_estado_espacio_dto det_estado_espacio { set; get; }        
        public int indica_operacion { get; set; }

        public int codigo_estado_movimiento_tabla { get; set; }

        public string motivo_movimiento { get; set; }

        public bool indica_insertar_estado_espacio { get; set; }



        public int? codigo_catalogo_bien_contrato { get; set; }

        public int? codigo_catalogo_bien_espacio { get; set; }

        public int? codigo_caracteristica_lote { get; set; }

        public string nombre_caracteristica_lote { get; set; }

        public string nombre_catalogo_bien_contrato { get; set; }

        public string nombre_catalogo_bien_espacio { get; set; }

        public string codigo_espacio_auxiliar { get; set; }

        public bool activado_cinerario { get; set; }
    }
}
