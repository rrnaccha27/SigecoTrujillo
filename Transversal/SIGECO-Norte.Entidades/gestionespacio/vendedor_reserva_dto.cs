using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
   public class vendedor_reserva_dto
    {

    public string   nombre_persona{set;get;}
    public string   apellido_materno{set;get;}
    public string   apellido_paterno{set;get;}
    public int   codigo_persona{set;get;}
    public string   codigo_usuario{set;get;}
    


 
    public DateTime? fecha_caducidad { get; set; }

    public string ape_materno_cliente { get; set; }

    public string ape_paterno_cliente { get; set; }

    public string nombre_cliente { get; set; }

    

    //public bool estado_registro_reserva { get; set; }

    public string nombre_estado_movimiento_tabla { get; set; }

    public DateTime? fecha_venta { get; set; }
    public bool estado_registro { get; set; }
    #region ATRIBUTOS DE AUDITORIA
    public DateTime fecha_registra { set; get; }
    public string usuario_registra { get; set; }

    public string usuario_modifica { get; set; }

    public DateTime? fecha_modifica { get; set; }
    #endregion


    public DateTime? fecha_reserva { get; set; }

    public string nombre_completo_persona { get; set; }

    public string nombre_completo_persona_fallecido { get; set; }

    public DateTime? fecha_registra_vendedor { get; set; }

    public DateTime fecha_movimiento { get; set; }

    public string cliente { get; set; }

    public string vendedor { get; set; }

    public string nombres_fallecido { get; set; }

    public string estado { get; set; }

    public string numero_contrato { get; set; }
    }


}
