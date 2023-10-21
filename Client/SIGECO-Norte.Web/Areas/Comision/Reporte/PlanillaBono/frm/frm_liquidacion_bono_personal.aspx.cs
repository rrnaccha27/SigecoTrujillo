using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.PlanillaBono.frm
{
    public partial class frm_liquidacion_bono_personal : System.Web.UI.Page
    {
        string p_codigo_planilla = string.Empty;
        DataTable dt_detalle_articulo = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                p_codigo_planilla = Request.QueryString["p_codigo_planilla"];//   CodTipo="+model.CodTipoDocumento+"& pNroDocumento="+model.NroDocumento

                string p_codigo_personal = Request.QueryString["p_codigo_personal"];

                frmVerReporte(p_codigo_planilla, p_codigo_personal);

            }
        }
        private void frmVerReporte(string p_codigo_planilla, string p_codigo_personal)
        {
            try
            {
                //ParametroSistemaService _IParametroSistemaService = new ParametroSistemaService();
               
                int v_codigo_planilla = int.Parse(p_codigo_planilla);
                
                DataTable dt_porcentajes = new DataTable();
                dt_porcentajes = PlanillaSelBL.Instance.ReporteLiquidacionBonoPorcentajes(v_codigo_planilla);
                string mensaje_1 = string.Empty;
                string mensaje_2 = string.Empty;
                string mensaje_3 = string.Empty;

                if (dt_porcentajes.Rows.Count > 0)
                {
                    mensaje_1 = dt_porcentajes.Rows[0]["mensaje_1"].ToString();
                    mensaje_2 = dt_porcentajes.Rows[0]["mensaje_2"].ToString();
                    mensaje_3 = dt_porcentajes.Rows[0]["mensaje_3"].ToString();
                }

                ReportParameter p_mensaje_1 = new ReportParameter("p_mensaje_1", mensaje_1);
                ReportParameter p_mensaje_2 = new ReportParameter("p_mensaje_2", mensaje_2);
                ReportParameter p_mensaje_3 = new ReportParameter("p_mensaje_3", mensaje_3);

                //DataTable dt_cabecera = new DataTable();
                DataTable dt_General = new DataTable();
                DataTable dt_detalle = new DataTable();
                dt_General = PlanillaSelBL.Instance.ReporteLiquidacionBonoPlanillaPersonal(v_codigo_planilla,0);
                dt_detalle_articulo = PlanillaSelBL.Instance.ReporteLiquidacionBonoPlanillaArticulos(v_codigo_planilla, 0, "");

                if (!string.IsNullOrWhiteSpace(p_codigo_personal))
                {
                    string _sqlWhere = string.Format("codigo_personal={0}", p_codigo_personal.Trim());
                    dt_detalle = dt_General.Select(_sqlWhere).CopyToDataTable();
                }
                else {
                    dt_detalle=dt_General.Copy();
                }

                rpt_liquidacion.ZoomMode = ZoomMode.Percent;
                rpt_liquidacion.ZoomPercent = 120;
                rpt_liquidacion.ProcessingMode = ProcessingMode.Local;
                rpt_liquidacion.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBono/rdl/rpt_detalle_bono_personal.rdlc");
                rpt_liquidacion.LocalReport.SetParameters(new ReportParameter[] { p_mensaje_1, p_mensaje_2, p_mensaje_3 });

                ReportDataSource dsDet = new ReportDataSource("dsLiquidacionBonoPersonal", dt_detalle);
                


                rpt_liquidacion.LocalReport.DataSources.Clear();                
                rpt_liquidacion.LocalReport.DataSources.Add(dsDet);

                rpt_liquidacion.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);

                rpt_liquidacion.LocalReport.Refresh();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            DataTable dt_articulos = new DataTable();

            string valor1 = e.Parameters[0].Values[0].ToString();
            string valor2 = e.Parameters[1].Values[0].ToString();
            string _sqlWhere = string.Format("nro_contrato='{0}' AND codigo_empresa={1}", valor2, valor1);

            dt_articulos = dt_detalle_articulo.Select(_sqlWhere).CopyToDataTable();
            //dt_articulos = PlanillaSelBL.Instance.ReporteLiquidacionBonoPlanillaArticulos(Convert.ToInt32(p_codigo_planilla), Convert.ToInt32(valor1), valor2);
            e.DataSources.Add(new ReportDataSource("dsBonoArticulo", dt_articulos));
        }
    }
}