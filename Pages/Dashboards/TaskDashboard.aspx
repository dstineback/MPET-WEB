<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SiteBase.master"  CodeFile="TaskDashboard.aspx.cs" Inherits="Pages.Dashboards.PagesDashboardsTaskDashboard" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<%@ Register src="~/UserControls/TaskDashboard.ascx" tagPrefix="TaskDash" tagName="TaskDash"%>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">PM Tasks Dashboard</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1 style="padding-left: 46px;">PM TASKS DASHBOARD</h1>
    <div style="height: 575px">
        <div>
            <TaskDash:TaskDash runat="server" ID="TaskDashboard" Width="98%" Height="550"/>
        </div>
    </div>
</asp:Content>