<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="myJobs.aspx.cs" Inherits="Pages_PlannedJobs_myJobs" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<%@ Register assembly="Infragistics4.WebUI.WebResizingExtender.v15.1, Version=15.1.20151.2400, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI" tagprefix="igui" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">MY Jobs</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    This is the test MyJobs page
    <asp:GridView runat="server" ID="myJobsGrid" AllowPaging="True" AllowSorting="True" DataSourceID="MyJobsDS" BorderStyle="Solid" CellPadding="4" ForeColor="#333333" GridLines="None">
         
        <AlternatingRowStyle BackColor="White" BorderStyle="Solid" />
         
       <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="n_jobstepid" HeaderText="n_jobstepid" InsertVisible="False" ReadOnly="True" SortExpression="n_jobstepid" />
            <asp:BoundField DataField="n_jobid" HeaderText="n_jobid" SortExpression="n_jobid" />
            <asp:BoundField DataField="T" HeaderText="T" ReadOnly="True" SortExpression="T" />
            <asp:BoundField DataField="A" HeaderText="A" ReadOnly="True" SortExpression="A" />
            <asp:BoundField DataField="Jobid" HeaderText="Jobid" SortExpression="Jobid" />
            <asp:BoundField DataField="Object ID" HeaderText="Object ID" SortExpression="Object ID" />
            <asp:BoundField DataField="step" HeaderText="step" SortExpression="step" />
            <asp:BoundField DataField="Step Title" HeaderText="Step Title" SortExpression="Step Title" />
            <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="Requested" HeaderText="Requested" ReadOnly="True" SortExpression="Requested" />
            <asp:BoundField DataField="Starting Date" HeaderText="Starting Date" ReadOnly="True" SortExpression="Starting Date" />
            <asp:BoundField DataField="Labor Class" HeaderText="Labor Class" SortExpression="Labor Class" />
            <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" />
            <asp:BoundField DataField="Outcome" HeaderText="Outcome" SortExpression="Outcome" />
            <asp:BoundField DataField="Supervisor" HeaderText="Supervisor" SortExpression="Supervisor" />
            <asp:BoundField DataField="H" HeaderText="H" SortExpression="H" />
            <asp:BoundField DataField="I" HeaderText="I" ReadOnly="True" SortExpression="I" />
            <asp:BoundField DataField="cReasoncode" HeaderText="cReasoncode" SortExpression="cReasoncode" />
            <asp:BoundField DataField="AssignedTaskID" HeaderText="AssignedTaskID" SortExpression="AssignedTaskID" />
            <asp:BoundField DataField="Completion Date" HeaderText="Completion Date" ReadOnly="True" SortExpression="Completion Date" />
            <asp:BoundField DataField="WorkOpID" HeaderText="WorkOpID" SortExpression="WorkOpID" />
            <asp:BoundField DataField="HighwayRouteID" HeaderText="HighwayRouteID" ReadOnly="True" SortExpression="HighwayRouteID" />
            <asp:BoundField DataField="MileMarker" HeaderText="MileMarker" ReadOnly="True" SortExpression="MileMarker" />
            <asp:BoundField DataField="AssetNumber" HeaderText="AssetNumber" SortExpression="AssetNumber" />
            <asp:BoundField DataField="ResponsiblePersonID" HeaderText="ResponsiblePersonID" ReadOnly="True" SortExpression="ResponsiblePersonID" />
            <asp:BoundField DataField="nMaintObjectID" HeaderText="nMaintObjectID" InsertVisible="False" ReadOnly="True" SortExpression="nMaintObjectID" />
            <asp:BoundField DataField="BreakdownJob" HeaderText="BreakdownJob" ReadOnly="True" SortExpression="BreakdownJob" />
            <asp:BoundField DataField="DowntimeHrs" HeaderText="DowntimeHrs" SortExpression="DowntimeHrs" />
            <asp:BoundField DataField="Ontime" HeaderText="Ontime" ReadOnly="True" SortExpression="Ontime" />
            <asp:BoundField DataField="ReturnWithin" HeaderText="ReturnWithin" SortExpression="ReturnWithin" />
            <asp:BoundField DataField="SubAssembly" HeaderText="SubAssembly" SortExpression="SubAssembly" />
            <asp:BoundField DataField="AreaID" HeaderText="AreaID" SortExpression="AreaID" />
            <asp:BoundField DataField="FlaggedRecordID" HeaderText="FlaggedRecordID" ReadOnly="True" SortExpression="FlaggedRecordID" />
            <asp:BoundField DataField="ObjectDescr" HeaderText="ObjectDescr" SortExpression="ObjectDescr" />
            <asp:BoundField DataField="CostCodeID" HeaderText="CostCodeID" ReadOnly="True" SortExpression="CostCodeID" />
            <asp:BoundField DataField="FundSrcCodeID" HeaderText="FundSrcCodeID" ReadOnly="True" SortExpression="FundSrcCodeID" />
            <asp:BoundField DataField="WorkOrderCodeID" HeaderText="WorkOrderCodeID" ReadOnly="True" SortExpression="WorkOrderCodeID" />
            <asp:BoundField DataField="OrganizationCodeID" HeaderText="OrganizationCodeID" ReadOnly="True" SortExpression="OrganizationCodeID" />
            <asp:BoundField DataField="FundingGroupCodeID" HeaderText="FundingGroupCodeID" ReadOnly="True" SortExpression="FundingGroupCodeID" />
            <asp:BoundField DataField="ControlSectionID" HeaderText="ControlSectionID" ReadOnly="True" SortExpression="ControlSectionID" />
            <asp:BoundField DataField="EquipmentNumberID" HeaderText="EquipmentNumberID" ReadOnly="True" SortExpression="EquipmentNumberID" />
            <asp:BoundField DataField="MileMarkerTo" HeaderText="MileMarkerTo" ReadOnly="True" SortExpression="MileMarkerTo" />
            <asp:BoundField DataField="EstimatedUnits" HeaderText="EstimatedUnits" SortExpression="EstimatedUnits" />
            <asp:BoundField DataField="ActualUnits" HeaderText="ActualUnits" SortExpression="ActualUnits" />
            <asp:BoundField DataField="ShiftID" HeaderText="ShiftID" SortExpression="ShiftID" />
            <asp:BoundField DataField="Crew Assigned" HeaderText="Crew Assigned" ReadOnly="True" SortExpression="Crew Assigned" />
            <asp:BoundField DataField="Parts Assigned" HeaderText="Parts Assigned" ReadOnly="True" SortExpression="Parts Assigned" />
            <asp:BoundField DataField="Equip. Assigned" HeaderText="Equip. Assigned" ReadOnly="True" SortExpression="Equip. Assigned" />
            <asp:BoundField DataField="Other Assigned" HeaderText="Other Assigned" ReadOnly="True" SortExpression="Other Assigned" />
            <asp:BoundField DataField="Route To ID" HeaderText="Route To ID" ReadOnly="True" SortExpression="Route To ID" />
            <asp:BoundField DataField="Created By" HeaderText="Created By" ReadOnly="True" SortExpression="Created By" />
            <asp:BoundField DataField="Created On" HeaderText="Created On" ReadOnly="True" SortExpression="Created On" />
            <asp:BoundField DataField="Modified By" HeaderText="Modified By" ReadOnly="True" SortExpression="Modified By" />
            <asp:BoundField DataField="Modified On" HeaderText="Modified On" ReadOnly="True" SortExpression="Modified On" />
            <asp:BoundField DataField="Quik-Post Job" HeaderText="Quik-Post Job" SortExpression="Quik-Post Job" />
            <asp:BoundField DataField="Post Notes" HeaderText="Post Notes" ReadOnly="True" SortExpression="Post Notes" />
        </Columns>
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>

    <asp:SqlDataSource ID="MyJobsDS" runat="server" ConnectionString="<%$ ConnectionStrings:ClientConnectionString %>" SelectCommand="filter_GetFilteredPlannedJobstepsList" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="MatchJobType" Type="Int32" />
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