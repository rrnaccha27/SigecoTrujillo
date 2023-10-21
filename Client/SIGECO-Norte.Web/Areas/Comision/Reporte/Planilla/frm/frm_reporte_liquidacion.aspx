﻿

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_reporte_liquidacion.aspx.cs" Inherits="SIGEES.Web.Areas.Comision.Reporte.Planilla.frm.frm_reporte_liquidacion" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
    .ReportViewer table
    {
        border-collapse: collapse;
        border-spacing: 0;
    }
    .ReportViewer *  
    {
        background-image:none;       
    }
</style>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">        
    <div class=".ReportViewer" >
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>            
    
        <rsweb:ReportViewer ID="rpt_liquidacion" runat="server" Font-Names="Verdana" Font-Size="9pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="12pt"  Height="99%" Width="99%" ExportContentDisposition="AlwaysAttachment" SizeToReportContent="true" BackColor="White" ShowRefreshButton="False">
            <localreport reportembeddedresource="SIGEES.Web.Areas.Comision.Reporte.Planilla.rdl.rpt_liquidacion.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="dsLiquidacion" />                    
                </DataSources>
            </localreport>
        </rsweb:ReportViewer>        
        </div>
    </form>
</body>
</html>





