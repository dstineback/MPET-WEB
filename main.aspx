<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBase.master" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="main" %>


<asp:Content ID="main" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

  
      <div id="mainPageDiv" runat="server">Welcome to M-PET <div id="divUser" runat="server"></div> <br />What would you like to do? </div>

      <div id="quickLinkButton">

         <asp:LinkButton runat="server" class="button" ID="myJobButton" href="Pages/PlannedJobs/myJobs.aspx">My Jobs</asp:LinkButton>
         <asp:LinkButton runat="server" class="button" ID="workRequestButton" href="Pages/WorkRequests/Requests.aspx">Make a work Request</asp:LinkButton>
         <asp:LinkButton runat="server" class="button" ID="plannedJObsButton" href="Pages/PlannedJobs/PlannedJobsList.aspx">Planned Jobs</asp:LinkButton>
         <asp:LinkButton runat="server" class="button" ID="jobHistoryButton" href="Pages/History/JobsHistoryList.aspx">Job History</asp:LinkButton> 
      
      </div>

   
</asp:Content>