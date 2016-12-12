<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="myJobs.aspx.cs" Inherits="Pages_PlannedJobs_myJobs" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<%@ Register assembly="Infragistics4.WebUI.WebResizingExtender.v15.1, Version=15.1.20151.2400, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI" tagprefix="igui" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">MY Jobs</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    This is the test MyJobs page
    <asp:GridView runat="server" ID="myJobsGrid" AllowSorting="True" Theme="Mulberry" 
        KeyFieldName="n_jobstepid" 
        Width="98%" 
        KeyboardSupport="True" 
        ClientInstanceName="PlannedGrid" 
        AutoPostBack="True" 
        Settings-HorizontalScrollBarMode="Auto" 
        SettingsPager-Mode="ShowPager" 
        SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
        SettingsBehavior-AllowFocusedRow="True"
        SelectionMode="Multiple">
         
        <AlternatingRowStyle BorderStyle="Solid" />
         
    </asp:GridView>

       <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
            <Header CssClass="gridViewHeader"></Header>

            <Row CssClass="gridViewRow"></Row>

            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

            <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
        </Styles>

    <asp:SqlDataSource ID="MyJobsDS" runat="server" ConnectionString="<%$ ConnectionStrings:ClientConnectionString %>" SelectCommand="filter_GetFilteredPlannedJobstepsList" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="MatchJobType" Type="Int32"/>
            <asp:Parameter DefaultValue="0" Name="MatchJobAgainst" Type="Int32" />
            <asp:Parameter Name="JobIDLike" Type="String" />
            <asp:Parameter DefaultValue="N" Name="FromHistoryYNB" Type="String" />
            <asp:Parameter DefaultValue="B" Name="IsIssuedYNB" Type="String" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="StartingReqDate" Type="DateTime" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="EndingReqDate" Type="DateTime" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="StartDateStart" Type="DateTime" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="StartDateEnd" Type="DateTime" />
            <asp:Parameter Name="TitleContains" Type="String" />
            <asp:Parameter Name="MatchLaborClass" Type="String" />
            <asp:Parameter Name="MatchStatus" Type="String" />
            <asp:Parameter Name="MatchGroup" Type="String" />
            <asp:Parameter Name="MatchPriority" Type="String" />
            <asp:Parameter Name="MatchReason" Type="String" />
            <asp:Parameter Name="MatchArea" Type="String" />
            <asp:Parameter Name="MatchObjectType" Type="String" />
            <asp:Parameter Name="MachineIDContains" Type="String" />
            <asp:Parameter Name="MatchOutcomeCode" Type="String" />
            <asp:Parameter Name="ObjIDDescrContains" Type="String" />
            <asp:Parameter Name="MatchTaskID" Type="String" />
            <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="CompStartDate" Type="DateTime" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="CompEndDate" Type="DateTime" />
            <asp:SessionParameter Name="JobCrew" SessionField="userName" Type="String" />
            <asp:Parameter Name="JobSupervisor" Type="String" />
            <asp:Parameter Name="MatchStateRoute" Type="String" />
            <asp:Parameter Name="MatchLocation" Type="String" />
            <asp:Parameter Name="PostedBy" Type="String" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="PostStartDate" Type="DateTime" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="PostEndDate" Type="DateTime" />
            <asp:Parameter DefaultValue="" Name="MatchWorkType" Type="String" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="IssuedStartDate" Type="DateTime" />
            <asp:Parameter DefaultValue="1960-01-01 23:59:59" Name="IssuedEndDate" Type="DateTime" />
            <asp:Parameter Name="RequestedBy" Type="String" />
            <asp:Parameter Name="RouteTo" Type="String" />
            <asp:Parameter Name="Notes" Type="String" />
            <asp:Parameter Name="MiscRef" Type="String" />
            <asp:Parameter Name="CostCodeID" Type="String" />
            <asp:Parameter Name="FundSourceID" Type="String" />
            <asp:Parameter Name="WorkOrderCodeID" Type="String" />
            <asp:Parameter Name="OrgCodeID" Type="String" />
            <asp:Parameter Name="FundGroupID" Type="String" />
            <asp:Parameter Name="ControlSectionID" Type="String" />
            <asp:Parameter Name="EquipNumberID" Type="String" />
            <asp:Parameter DefaultValue="B" Name="IsBreakdownYNB" Type="String" />
            <asp:Parameter DefaultValue="B" Name="HasAttachments" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>