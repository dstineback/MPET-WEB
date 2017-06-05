﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestDashboard.aspx.cs" Inherits="UserControls.RequestDashboard" %>

<%@ Register Assembly="DevExpress.Dashboard.v17.1.Web, Version=17.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxDashboardViewer ID="ASPxDashboardViewer1" runat="server" DashboardSource="~/UserControls/DashboardSource/Request Dashboard.xml" Height="600px" Width="98%" OnConfigureDataConnection="ASPxDashboardViewer1_ConfigureDataConnection"></dx:ASPxDashboardViewer>
    </div>
    </form>
</body>
</html>
