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
    public partial class frm_reporte_bono_supervisor : System.Web.UI.Page
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
               // p_codigo_planilla = "14";
               
                int v_codigo_planilla = int.Parse(p_codigo_planilla);                
              
               // Parameter [] param=new Parameter

                
                DataTable dt_General = new DataTable();
                DataTable dt_detalle = new DataTable();

                //var v_entiddad = PlanillaSelBL.Instance.BuscarPlanillaBonoById(v_codigo_planilla);

                dt_General = PlanillaSelBL.Instance.ReportePlanillaBonoSupervisor(v_codigo_planilla,0);

                if (!string.IsNullOrWhiteSpace(p_codigo_personal))
                {
                    string _sqlWhere = string.Format("codigo_personal={0}", p_codigo_personal.Trim());
                    dt_detalle = dt_General.Select(_sqlWhere).CopyToDataTable();
                }
                else {
                    dt_detalle=dt_General.Copy();
                }

                ParametroSistemaService _IParametroSistemaService = new ParametroSistemaService();
                
                var _item_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.porcentaje_detraccion);                
                ReportParameter p_porcentaje_detraccion = new ReportParameter("p_porcentaje_detraccion", _item_detraccion.valor);

                rpt_planilla_bono_supervisor.ZoomMode = ZoomMode.Percent;
                rpt_planilla_bono_supervisor.ZoomPercent = 120;
                rpt_planilla_bono_supervisor.ProcessingMode = ProcessingMode.Local;
                rpt_planilla_bono_supervisor.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBono/rdl/rpt_planilla_bono_supervisor.rdlc");

                ReportDataSource dsDet = new ReportDataSource("dsPlanillaBonoSupervisor", dt_detalle);

                 rpt_planilla_bono_supervisor.LocalReport.DataSources.Clear();
                 rpt_planilla_bono_supervisor.LocalReport.DataSources.Add(dsDet);
                 rpt_planilla_bono_supervisor.LocalReport.SetParameters(new ReportParameter[] { p_porcentaje_detraccion});

                 rpt_planilla_bono_supervisor.LocalReport.Refresh();
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