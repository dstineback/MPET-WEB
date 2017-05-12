<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBase.master" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="main" %>

<asp:Content ID="main" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div id="mainPageDiv" runat="server">Welcome to M-PET <div id="divUser" runat="server"></div> <br />What would you like to do? </div>
    <div id="quickLinkButton">
        <asp:LinkButton runat="server" class="mainButton" ID="myJobButton" href="Pages/PlannedJobs/myJobs.aspx">My Jobs</asp:LinkButton>
        <asp:LinkButton runat="server" class="mainButton" ID="QuickPost" href="Pages/QuickPost/QuickPost.aspx">Quick Post</asp:LinkButton>
        <asp:LinkButton runat="server" class="mainButton" ID="workRequestButton" href="Pages/WorkRequests/WorkRequestform.aspx">Add Work Request</asp:LinkButton>
        <asp:LinkButton runat="server" class="mainButton" ID="PlanJobBtn" href="Pages/PlannedJobs/PlannedJobsForm.aspx" OnClick="PlanJobBtn_Click" >Add Planned Job</asp:LinkButton>
        <asp:LinkButton runat="server" class="mainButton" ID="WorkRequestListButton" href="Pages/WorkRequests/RequestsList.aspx">Work Request List</asp:LinkButton>
        <asp:LinkButton runat="server" class="mainButton" ID="plannedJObsButton" href="Pages/PlannedJobs/PlannedJobsList.aspx">Planned Jobs list</asp:LinkButton>
        <asp:LinkButton runat="server" class="mainButton" ID="jobHistoryButton" href="Pages/History/JobsHistoryList.aspx">Job History</asp:LinkButton>

    </div> 
</asp:Content>