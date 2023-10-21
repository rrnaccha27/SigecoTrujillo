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

namespace SIGEES.Web.Areas.Comision.Reporte.Comision.frm
{
    public partial class frm_reporte_consulta_comision : System.Web.UI.Page
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
                List<grilla_comision_cronograma_dto> lst = new List<grilla_comision_cronograma_dto>();
                grilla_comision_cronograma_filtro v_entidad = new grilla_comision_cronograma_filtro();

                v_entidad = Session[guid] as grilla_comision_cronograma_filtro;


                if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_habilitado_inicio))
                    v_entidad.fecha_habilitado_inicio = DateTime.Parse(v_entidad.str_fecha_habilitado_inicio);

                if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_habilitado_fin))
                    v_entidad.fecha_habilitado_fin = DateTime.Parse(v_entidad.str_fecha_habilitado_fin);

                if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_contrato_inicio))
                    v_entidad.fecha_contrato_inicio = DateTime.Parse(v_entidad.str_fecha_contrato_inicio);


                if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_contrato_fin))
                    v_entidad.fecha_contrato_fin = DateTime.Parse(v_entidad.str_fecha_contrato_fin);

                lst = DetalleCronogramaPagoSelBL.Instance.CronogramaPagoComisionListar(v_entidad);            


                rpt_comision.ZoomMode = ZoomMode.Percent;
                rpt_comision.ZoomPercent = 100;
                rpt_comision.ProcessingMode = ProcessingMode.Local;
                rpt_comision.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Comision/rdl/rpt_comision.rdlc");
                ReportDataSource dataSource = new ReportDataSource("dsComision", lst);
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