using System;
using System.Collections.Generic;

namespace SIGEES.Web.Areas.Comision.Utils
{

    public enum TipoAuditoria : int { Registrar = 1, Modificar = 2, Eliminar = 3 };
    public enum Parametro : int
    {
        igv = 9,
        correo_servidor = 10,
        correo_usuario = 11,
        correo_password = 12,
        correo_asunto = 13,
        correo_puerto = 14,
        porcentaje_detraccion=15,
        aplicar_detraccion_superior = 16,
        comision_manual_tipo_venta = 17,
        comision_manual_tipo_pago = 18,
        ruta_archivo_txt = 20,
        usar_wcf = 24,
        planilla_bono_jn_tipo_planilla = 27,
        planilla_bono_jn_canales = 28,
        envio_correo_planilla = 33
    }

    public enum EstadoCuota : int
    {
        pendiente = 1,
        en_proceso = 2,
        pagado=3,
        excluido=4      ,  
        anulado=5

    }
     public enum EstadoPlanilla : int
    {
        abierto = 1,
        cerrado = 2,          
        anulado=3

    }

     public enum EstadoReclamo : int
     { 
        pendiente = 1,
        atendido = 2
     }

    public enum ReporteFinanzasTipo : int
    {
        pagado = 1, 
        generado = 2
    }

    public enum ReporteFinanzasTipoSumatoria : int
    {
        resumen = 1,
        detalle = 2
    }

    public enum ReporteFinanzas : int
    {
        comision = 1,
        detalle = 2
    }

    public enum TemplateCorreo : int
    {
        Supervisor = 1
        ,Jefatura = 2
        , TxtChecklist = 3
    }

    public enum TemplateCorreoParametros : int
    {
        planilla = 0,
        numero_planilla = 1,
        cuenta = 2,
        fecha = 3,
        total_enum = 4 /* length del enum */
    }

    public enum TemplateTxtParametros : int
    {
        tipo_checklist = 0,
        numero_checklist = 1,
        cuenta = 2,
        fecha = 3,
        total_enum = 4 /* length del enum */
    }

    public enum CuentasCorreo : int
    { 
        funes = 1,
        gestores = 2,
        oficina = 3,
        representatnes = 4,
        overall = 5,
        contabilidad=98,
        recursos_humanos=99
    }

    public enum ReporteBonoTrimestral : int
    { 
        planilla = 1,
        liquidacion = 2,
        resumen_liquidacion = 3
    }
}