using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.Planilla.frm
{
    public partial class frm_reporte_liquidacion : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string p_codigo_planilla = Request.QueryString["p_codigo_planilla"];//   CodTipo="+model.CodTipoDocumento+"& pNroDocumento="+model.NroDocumento

                string p_codigo_personal = Request.QueryString["p_codigo_personal"];

                frmVerReporte(p_codigo_planilla, p_codigo_personal);

            }
        }
        private void frmVerReporte(string p_codigo_planilla, string p_codigo_personal)
        {
            try
            {
                string v_titulo = string.Empty;
                ParametroSistemaService _IParametroSistemaService = new ParametroSistemaService();

                int v_codigo_planilla = int.Parse(p_codigo_planilla);
                var _item_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.porcentaje_detraccion);
                ReportParameter p_porcentaje_detraccion = new ReportParameter("p_porcentaje_detraccion", _item_detraccion.valor);
                List<reporte_detalle_liquidacion> dt_General = PlanillaSelBL.Instance.ReporteDetalleLiquidacionPlanilla(v_codigo_planilla);

                var _v_planilla = dt_General.FirstOrDefault();
                if (_v_planilla == null)
                {
                    v_titulo = "No existe información para generar reporte de liquidación";
                    _v_planilla = new reporte_detalle_liquidacion();
                }

                else
                {
                    v_titulo = "CORRESPONDIENTE AL PERIODO DEL " + getNombreDia(_v_planilla.fecha_inicio) + " " + getDia(_v_planilla.fecha_inicio) + " de " + getNombreMes(_v_planilla.fecha_inicio) + " del " + getAnho(_v_planilla.fecha_inicio) +
                        " AL " + getNombreDia(_v_planilla.fecha_fin) + " " + getDia(_v_planilla.fecha_fin) + " de " + getNombreMes(_v_planilla.fecha_fin) + " del " + getAnho(_v_planilla.fecha_fin);

                }



                ReportParameter p_titulo_reporte = new ReportParameter("p_titulo_reporte", v_titulo);

                rpt_liquidacion.ZoomMode = ZoomMode.Percent;
                rpt_liquidacion.ZoomPercent = 100;
                rpt_liquidacion.ProcessingMode = ProcessingMode.Local;
                rpt_liquidacion.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Planilla/rdl/rpt_liquidacion" + _v_planilla.tipo_reporte + ".rdlc");

                rpt_liquidacion.LocalReport.SetParameters(new ReportParameter[] { p_porcentaje_detraccion, p_titulo_reporte });
                if (!string.IsNullOrWhiteSpace(p_codigo_personal))
                {
                    var resultado = dt_General.FindAll(x => x.codigo_personal == int.Parse(p_codigo_personal));
                    ReportDataSource dsDet = new ReportDataSource("dsDetalleLiquidacion", resultado);
                    rpt_liquidacion.LocalReport.DataSources.Clear();
                    rpt_liquidacion.LocalReport.DataSources.Add(dsDet);
                    rpt_liquidacion.LocalReport.Refresh();
                }
                else
                {
                    ReportDataSource dsDet = new ReportDataSource("dsDetalleLiquidacion", dt_General);
                    rpt_liquidacion.LocalReport.DataSources.Clear();
                    rpt_liquidacion.LocalReport.DataSources.Add(dsDet);
                    rpt_liquidacion.LocalReport.Refresh();
                }





            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }

        }
        private string getNombreMes(string p_fecha)
        {
            var fecha = DateTime.Parse(p_fecha);



            switch (fecha.Month)
            {
                case 1:
                    return "ENERO";

                case 2:
                    return "FEBRERO";
                case 3:
                    return "MARZO";
                case 4:
                    return "ABRIL";
                case 5:
                    return "MAYO";
                case 6:
                    return "JUNIO";
                case 7:
                    return "JULIO";
                case 8:
                    return "AGOSTO";
                case 9:
                    return "SEPTIEMBRE";
                case 10:
                    return "OCTUBRE";
                case 11:
                    return "NOVIEMBRE";
                case 12:
                    return "DICIEMBRE";
            }
            return "";

        }
        private string getNombreDia(string p_fecha)
        {
            var fecha = DateTime.Parse(p_fecha);
            return fecha.ToString("dddd", new CultureInfo("es-ES")).ToUpper();
            /*
            switch (fecha.Day)
            {
                case 1:
                    return "Lunes";
                case 2:
                    return "Martes";
                case 3:
                    return "Miercoles";
                case 4:
                    return "Jueves";
                case 5:
                    return "Viernes";
                case 6:
                    return "Sábado";
                case 7:
                    return "Domingo";
            }

            return "";
             * */

        }
        private string getDia(string p_fecha)
        {

            var fecha = DateTime.Parse(p_fecha);

            return fecha.Day.ToString().PadLeft(2, '0');

        }
        private string getAnho(string p_fecha)
        {

            var fecha = DateTime.Parse(p_fecha);

            return fecha.Year.ToString();

        }
    }
}