using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.PlanillaBono.frm
{
    public partial class frm_liquidacion_bono_resumen : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string p_codigo_planilla = Request.QueryString["p_codigo_planilla"];//   CodTipo="+model.CodTipoDocumento+"& pNroDocumento="+model.NroDocumento
                frmVerReporte(p_codigo_planilla);
            }
        }
        private void frmVerReporte(string p_codigo_planilla)
        {
            try
            {
                ParametroSistemaService _IParametroSistemaService = new ParametroSistemaService();
                int v_codigo_planilla = int.Parse(p_codigo_planilla);
                
                DataTable dt_General = new DataTable();
                DataTable dt_detalle = new DataTable();

                dt_General = PlanillaSelBL.Instance.ReporteLiquidacionBonoIndividual(v_codigo_planilla, 0);
				dt_detalle=dt_General.Copy();


                rpt_liquidacion_bono_resumen.ZoomMode = ZoomMode.Percent;
                rpt_liquidacion_bono_resumen.ZoomPercent = 100;
                rpt_liquidacion_bono_resumen.ProcessingMode = ProcessingMode.Local;
                rpt_liquidacion_bono_resumen.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBono/rdl/rpt_liquidacion_bono_resumen.rdlc");

                ReportDataSource dsDet = new ReportDataSource("dsLiquidacionBonoSupervisorIndividual", dt_detalle);

                rpt_liquidacion_bono_resumen.LocalReport.DataSources.Clear();
                rpt_liquidacion_bono_resumen.LocalReport.DataSources.Add(dsDet);                

                rpt_liquidacion_bono_resumen.LocalReport.Refresh();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
            finally
            {

            }
        }
    }
}