﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskDashboard.ascx.cs" Inherits="UserControls.TaskDashboard" %>
<%@ Register Assembly="DevExpress.Dashboard.v17.1.Web, Version=17.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>
<dx:ASPxDashboardViewer ID="ASPxDashboardViewer1" ColorScheme="Mulberry" runat="server" DashboardSource="~/UserControls/DashboardSource/TaskCrewAverages.xml" Height="550px" Width="98%"></dx:ASPxDashboardViewer>
