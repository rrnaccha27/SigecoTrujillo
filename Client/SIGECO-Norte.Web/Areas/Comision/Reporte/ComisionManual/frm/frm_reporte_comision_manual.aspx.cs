using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.ComisionManual.frm
{
    public partial class frm_reporte_comision_manual : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string p_guid = Request.QueryString["p_guid"];
                frmVerReporte(p_guid);
            }
        }
        private void frmVerReporte(string guid)
        {
            try
            {
                List<comision_manual_reporte_dto> lst = new List<comision_manual_reporte_dto>();
                comision_manual_filtro_dto v_entidad = new comision_manual_filtro_dto();

                v_entidad = Session[guid] as comision_manual_filtro_dto;

                //if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_habilitado_inicio))
                //    v_entidad.fecha_habilitado_inicio = DateTime.Parse(v_entidad.str_fecha_habilitado_inicio);

                //if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_habilitado_fin))
                //    v_entidad.fecha_habilitado_fin = DateTime.Parse(v_entidad.str_fecha_habilitado_fin);

                //if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_contrato_inicio))
                //    v_entidad.fecha_contrato_inicio = DateTime.Parse(v_entidad.str_fecha_contrato_inicio);


                //if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_contrato_fin))
                //    v_entidad.fecha_contrato_fin = DateTime.Parse(v_entidad.str_fecha_contrato_fin);

                lst = ComisionManualBL.Instance.Reporte(v_entidad);

                comision_manual_reporte_param_dto parametro = ComisionManualBL.Instance.ReporteParam(v_entidad);
                ReportParameter p_usuario = new ReportParameter("p_usuario", parametro.usuario);
                ReportParameter p_fecha_impresion = new ReportParameter("p_fecha_impresion", parametro.fecha_impresion);
                ReportParameter p_fechas = new ReportParameter("p_fechas", parametro.fechas);

                rpt_comision.ZoomMode = ZoomMode.Percent;
                rpt_comision.ZoomPercent = 100;
                rpt_comision.ProcessingMode = ProcessingMode.Local;
                rpt_comision.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/ComisionManual/rdl/rpt_comision_manual.rdlc");
                rpt_comision.LocalReport.SetParameters(new ReportParameter[] { p_usuario, p_fecha_impresion, p_fechas });
                ReportDataSource dataSource = new ReportDataSource("dsComisionManual", lst);
                rpt_comision.LocalReport.DataSources.Clear();
                rpt_comision.LocalReport.DataSources.Add(dataSource);
                rpt_comision.LocalReport.Refresh();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }

        }
    }
}