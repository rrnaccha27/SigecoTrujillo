using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
    [Serializable]
    public class reclamo_dto
    {
        public int codigo_reclamo { get; set; }
        public int codigo_personal { get; set; }
        public string NroContrato { get; set; }
        public int codigo_articulo { get; set; }
        public int codigo_empresa { get; set; }
        public int Cuota { get; set; }
        public decimal Importe { get; set; }

        public int atencion_codigo_articulo { get; set; }
        public int atencion_codigo_empresa { get; set; }
        public int atencion_Cuota { get; set; }
        public decimal atencion_Importe { get; set; }

        public int codigo_estado_reclamo { get; set; }
        public int codigo_estado_resultado { get; set; }
        public string Observacion { get; set; }
        public string Respuesta { get; set; }
        public string usuario_registra { get; set; }
        public DateTime? fecha_registra { get; set; }
        public string usuario_modifica { get; set; }
        public DateTime? fecha_modifica { get; set; }

        public string PersonalVentas { get; set; }
        public string Articulo { get; set; }
        public string Empresa { get; set; }
        public string atencion_Articulo { get; set; }
        public string atencion_Empresa { get; set; }
        public string Estado { get; set; }
        public string Resultado { get; set; }
        public int codigo_planilla { get; set; }
        public string numero_planilla { get; set; }
        public string UsuarioAtencion { get; set; }
        public string FechaAtencion { get; set; }
        public string TipoAfectaPlanilla { get; set; }

        public string FechaRegistra { get; set; }
        public string UsuarioRegistra { get; set; }

        public int es_contrato_migrado { get; set; }
        public string error_contrato_migrado { get; set; }

        public int codigo_estado_resultado_n1 { get; set; }
        public int codigo_estado_resultado_n2 { get; set; } 
        public string nombre_estado_resultado_n1 { get; set; }
        public string nombre_estado_resultado_n2 { get; set; }
        public string observacion_n1 { get; set; }
        public string usuario_n1 { get; set; }
        public string fecha_n1 { get; set; }
        public string observacion_n2 { get; set; }
        public string usuario_n2 { get; set; }
        public string fecha_n2 { get; set; }

        public string nombre_estado_reclamo { get; set; }
        public string nombre_empresa { get; set; }
        public string estilo { get; set; }
    }

    public class reclamo_valida_dto
    {
        public int codigo_reclamo { get; set; }
        public int codigo_personal { get; set; }
        public string NroContrato { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_articulo { get; set; }
        public int Cuota { get; set; }
        public decimal Importe { get; set; }
        public string Observacion { get; set; }
    }

    public class reclamo_estado_contrato_dto
    {
        public int codigo_estado_proceso { get; set; }
        public string nombre_estado_proceso { get; set; }
        public string observacion { get; set; }
    }

    public class reclamo_personal_listado_dto {
        public int codigo_personal { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre_personal { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public int codigo_canal { get; set; }
        public int codigo_grupo { get; set; }
    }

    public class reclamo_busqueda_dto {
        public string nro_contrato { get; set; }
        public string personal_ventas { get; set; }
        public int codigo_estado { get; set; }
        public string usuario { get; set; }
        public int codigo_perfil { get; set; }
    }

    public class reclamo_atencion_n1_dto {
        public int codigo_reclamo { get; set; }
        public int codigo_estado_resultado { get; set; }
        public string observacion { get; set; }
        public string usuario { get; set; }
    }
}
