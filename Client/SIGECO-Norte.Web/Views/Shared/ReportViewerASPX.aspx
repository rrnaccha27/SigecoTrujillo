<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Fallecidos Report</title>
    <script runat="server">
        void Page_Load(object sender, EventArgs e)
        {
            SIGEES.BusinessLogic.ReporteBL reporteBL = new SIGEES.BusinessLogic.ReporteBL();
            
            if (!IsPostBack)
            {
                String fechaInicio = ViewData["fechaInicio"] as String;
                String fechaFin = ViewData["fechaFin"] as String;
                
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/RFallecido.rdlc");
                ReportDataSource rdc = new ReportDataSource("DSFallecido", reporteBL.ListarFallecidoPorFecha());
                ReportViewer1.LocalReport.DataSources.Add(rdc);
                ReportViewer1.LocalReport.Refresh();
            }
        }
    </script>
</head>
<body>
    <center>
        <form runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true">
                </rsweb:ReportViewer>
        </form>
        
        <%= ViewData["fechaInicio"] %>
        <%= ViewData["fechaFin"] %>

    </center>
</body>
</html>
