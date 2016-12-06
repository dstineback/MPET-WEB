<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewReport.aspx.cs" Inherits="Reports_ViewReport" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>M-PET.NET Reporting</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" 
            runat="server" 
            AutoDataBind="true" 
            HasPrintButton="True" 
            HasRefreshButton="True"
            ReuseParameterValuesOnRefresh="True"
            Height="50px"
            Width="100%"
            OnReportRefresh="CrystalReportViewer1_ReportRefresh"
            PrintMode="ActiveX"
            HasCrystalLogo="False"/>
    </div>
    </form>
</body>
</html>
