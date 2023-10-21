using Microsoft.Reporting.WebForms;
using SIGEES.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIGEES.Web.Areas.Comision.Reporte.Articulo.frm
{
    public partial class frm_reporte_articulo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string p_codigo_articulo = Request.QueryString["p_codigo_articulo"];//   CodTipo="+model.CodTipoDocumento+"& pNroDocumento="+model.NroDocumento



                frmVerReporte(p_codigo_articulo);

            }
        }
        private void frmVerReporte(string p_codigo_articulo)
        {
            try
            {

                //p_codigo_articulo = "1";
                int v_codigo_articulo = int.Parse(p_codigo_articulo);                
                DataTable dt_detalle = new DataTable();
                dt_detalle = ArticuloBL.Instance.ReporteDetalladaByArticulo(v_codigo_articulo);


                rpt_articulo.ZoomMode = ZoomMode.Percent;
                rpt_articulo.ZoomPercent = 100;
                rpt_articulo.ProcessingMode = ProcessingMode.Local;
                rpt_articulo.LocalReport.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Articulo/rdl/rpt_articulo.rdlc");
                ReportDataSource dsDet = new ReportDataSource("dsArticulo", dt_detalle);
                rpt_articulo.LocalReport.DataSources.Clear();
                rpt_articulo.LocalReport.DataSources.Add(dsDet);

                rpt_articulo.LocalReport.Refresh();
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