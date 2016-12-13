<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBase.master" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="main" %>


<asp:Content ID="main" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

  
      <div id="mainPageDiv" runat="server">Welcome to M-PET <div id="divUser" runat="server"></div> <br />What would you like to do? </div>

   
     <asp:LinkButton runat="server" ID="myJobButton" href="Pages/PlannedJobs/myJobs.aspx">My Jobs</asp:LinkButton>
     <asp:LinkButton runat="server" ID="LinkButton1" href="Pages/WorkRequests/Requests.aspx">Make a work Request</asp:LinkButton>
     <asp:LinkButton runat="server" ID="LinkButton2" href="Pages/PlannedJobs/PlannedJobsList.aspx">Planned Jobs</asp:LinkButton>
     <asp:LinkButton runat="server" ID="LinkButton3" href="Pages/History/JobsHistoryList.aspx">Job History</asp:LinkButton> 
      

   
</asp:Content>