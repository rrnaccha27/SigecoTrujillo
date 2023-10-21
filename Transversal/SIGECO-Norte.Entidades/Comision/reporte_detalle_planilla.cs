using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class reporte_detalle_planilla
    {
        public int codigo_tipo_planilla { set; get; }

        public string nombre_tipo_planilla { set; get; }
        public string numero_planilla { set; get; }
        public int codigo_estado_planilla { set; get; }
        public string fecha_inicio { set; get; }
        public string fecha_fin { set; get; }

        public int codigo_moneda { set; get; }
        public string moneda { set; get; }
        public string articulo { set; get; }
        public string nro_contrato { set; get; }
        public int nro_cuota { set; get; }

        public string tipo_venta { set; get; }

        public string tipo_pago { set; get; }
        public int codigo_empresa { set; get; }
        public string empresa { set; get; }
        public int codigo_canal { set; get; }
        public string canal { set; get; }
        public int codigo_grupo { set; get; }
        public string grupo { set; get; }
        
        public int codigo_personal { set; get; }
        public int codigo_supervisor { set; get; }

        public string personal { set; get; }
        public string personal_referencial { set; get; }

        public decimal monto_bruto { set; get; }
        public decimal igv { set; get; }
        public decimal monto_neto { set; get; }

        public decimal monto_bruto_empresa { set; get; }
        public decimal igv_empresa { set; get; }
        public decimal monto_neto_empresa { set; get; }

        public decimal monto_bruto_canal { set; get; }
        public decimal igv_canal { set; get; }
        public decimal monto_neto_canal { set; get; }

        public decimal monto_bruto_grupo { set; get; }
        public decimal igv_grupo { set; get; }
        public decimal monto_neto_grupo { set; get; }

        public decimal monto_bruto_personal { set; get; }
        public decimal igv_personal { set; get; }
        public decimal monto_neto_personal { set; get; }
        public decimal monto_descuento { set; get; }

        public decimal monto_neto_personal_con_descuento { set; get; }
        public decimal monto_detraccion_personal { set; get; }
        public decimal monto_neto_pagar_personal { set; get; }
        public int es_comision_manual { set; get; }
        public string fecha_contrato { set; get; }
        public string usuario_comision_manual { set; get; }
        public string tipo_reporte { set; get; }
    }

    public partial class reporte_detalle_liquidacion
    {


        public string neto_pagar_empresa_letra { get; set; }

        public decimal neto_pagar_empresa { get; set; }

        public decimal detraccion_empresa { get; set; }

        public decimal descuento_empresa { get; set; }

        public decimal neto_empresa { get; set; }

        public decimal bruto_empresa { get; set; }

        public decimal igv_empresa { get; set; }

        public decimal monto_neto { get; set; }

        public decimal monto_bruto { get; set; }

        public decimal igv { get; set; }

        public string nombre_tipo_pago { get; set; }

        public string nombre_tipo_venta { get; set; }

        public string nombre_articulo { get; set; }

        public int nro_cuota { get; set; }

        public string nro_contrato { get; set; }

        public string nombre_personal_referencial { get; set; }

        public int codigo_personal_referencial { get; set; }

        public string nro_documento { get; set; }

        public string nombre_personal { get; set; }

        public string nombre_tipo_documento { get; set; }

        public int codigo_personal { get; set; }

        public string direccion_fiscal { get; set; }

        public string ruc { get; set; }

        public string nombre_empresa_largo { get; set; }

        public string nombre_empresa { get; set; }

        public int codigo_empresa { get; set; }

        public string fecha_fin { get; set; }

        public string fecha_inicio { get; set; }

        public int codigo_estado_planilla { get; set; }

        public string numero_planilla { get; set; }

        public int codigo_tipo_planilla { get; set; }

        public string email_personal { get; set; }

        public string email_personal_referencial { get; set; }

        public string nombre_envio_correo { get; set; }

        

        public string apellido_envio_correo { get; set; }

        public string canal_grupo_nombre { get; set; }

        public string descuento_motivo { get; set; }
        public string codigo_jardines { get; set; }

        public int codigo_moneda { get; set; }
        public string tipo_reporte { set; get; }

        public int codigo_canal { get; set; }
    }

    public partial class reporte_detalle_liquidacion_bono
    {
        public int	codigo_planilla { get; set; }
        public string 	concepto { get; set; }
        public decimal	porcentaje_detraccion { get; set; }
        public string 	fecha_inicio { get; set; }
        public string 	fecha_fin { get; set; }
        public string 	nombre_canal { get; set; }
        public int	codigo_estado_planilla { get; set; }
        public string 	nombre_empresa_largo { get; set; }
        public string 	direccion_fiscal { get; set; }
        public string 	ruc { get; set; }
        public string 	nro_documento { get; set; }
        public string 	nombre_tipo_documento { get; set; }
        public int	codigo_supervisor { get; set; }
        public string 	apellidos_nombres_supervisor { get; set; }
        public int	codigo_empresa { get; set; }
        public string 	nombre_empresa { get; set; }
        public int	codigo_grupo { get; set; }
        public string 	nombre_grupo { get; set; }
        public decimal	monto_bruto_empresa_supervisor { get; set; }
        public decimal	monto_igv_empresa_supervisor { get; set; }
        public decimal	monto_neto_empresa_supervisor { get; set; }
        public decimal	monto_detraccion_empresa_supervisor { get; set; }
        public decimal	monto_neto_pagar_empresa_supervisor { get; set; }
        public string 	neto_pagar_empresa_supervisor_letra { get; set; }
        public int	codigo_canal { get; set; }
        public string 	nombre_supervisor { get; set; }
        public string email_supervisor { get; set; }
    }

    /**/
    public partial class reporte_liquidacion_Supervisor
    {
        public string numero_planilla { set; get; }
        public int codigo_empresa { get; set; }
        public string nombre_empresa { get; set; }
        public string nombre_empresa_largo { get; set; }
        public string ruc { get; set; }
        public string direccion_fiscal { get; set; }
        public int codigo_personal { set; get; }
        public string nombre_tipo_documento { get; set; }
        public string nombre_personal { get; set; }
        public string nro_documento { get; set; }
        public string email_personal { get; set; }
        public string nombre_envio_correo { get; set; }
        public string apellido_envio_correo { get; set; }
        public string canal_grupo_nombre { get; set; }
        public decimal neto_pagar_empresa { get; set; }
        public string tipo_reporte { set; get; }
        public int codigo_canal { get; set; }
    }
    /**/

}
