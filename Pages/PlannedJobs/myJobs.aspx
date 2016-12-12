<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="myJobs.aspx.cs" Inherits="Pages_PlannedJobs_myJobs" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<%@ Register assembly="Infragistics4.WebUI.WebResizingExtender.v15.1, Version=15.1.20151.2400, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI" tagprefix="igui" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">MY Jobs</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    This is the test MyJobs page
    <dx:ASPxGridView ID="myJobsGrid" runat="server" Theme="Mulberry" Width="1012px" AutoGenerateColumns="False" EnableTheming="True">


        <Settings ShowFilterRow="True" ShowGroupPanel="True" AutoFilterCondition="Equals" EnableFilterControlPopupMenuScrolling="True"></Settings>

        <SettingsCommandButton>
            <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

            <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
        </SettingsCommandButton>

        <SettingsDataSecurity AllowDelete="False"></SettingsDataSecurity>
        <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
        <Columns>
            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="n_jobstepid" ReadOnly="True" VisibleIndex="1">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="n_jobid" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="T" ReadOnly="True" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="A" ReadOnly="True" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Object ID" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="step" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Step Title" VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Requested" ReadOnly="True" VisibleIndex="11">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="Starting Date" ReadOnly="True" VisibleIndex="12">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Labor Class" VisibleIndex="13">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Group" VisibleIndex="14">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Outcome" VisibleIndex="15">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Supervisor" VisibleIndex="16">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="H" VisibleIndex="17">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="I" ReadOnly="True" VisibleIndex="18">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="cReasoncode" VisibleIndex="19">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssignedTaskID" VisibleIndex="20">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Completion Date" ReadOnly="True" VisibleIndex="21">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOpID" VisibleIndex="22">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HighwayRouteID" ReadOnly="True" VisibleIndex="23">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MileMarker" ReadOnly="True" VisibleIndex="24">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssetNumber" VisibleIndex="25">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ResponsiblePersonID" ReadOnly="True" VisibleIndex="26">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="nMaintObjectID" ReadOnly="True" VisibleIndex="27">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BreakdownJob" ReadOnly="True" VisibleIndex="28">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DowntimeHrs" VisibleIndex="29">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Ontime" ReadOnly="True" VisibleIndex="30">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ReturnWithin" VisibleIndex="31">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SubAssembly" VisibleIndex="32">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AreaID" VisibleIndex="33">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FlaggedRecordID" ReadOnly="True" VisibleIndex="34">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ObjectDescr" VisibleIndex="35">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CostCodeID" ReadOnly="True" VisibleIndex="36">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" ReadOnly="True" VisibleIndex="37">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" ReadOnly="True" VisibleIndex="38">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" ReadOnly="True" VisibleIndex="39">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" ReadOnly="True" VisibleIndex="40">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ControlSectionID" ReadOnly="True" VisibleIndex="41">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" ReadOnly="True" VisibleIndex="42">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MileMarkerTo" ReadOnly="True" VisibleIndex="43">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EstimatedUnits" VisibleIndex="44">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ActualUnits" VisibleIndex="45">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShiftID" VisibleIndex="46">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Crew Assigned" ReadOnly="True" VisibleIndex="47">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Parts Assigned" ReadOnly="True" VisibleIndex="48">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Equip. Assigned" ReadOnly="True" VisibleIndex="49">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Other Assigned" ReadOnly="True" VisibleIndex="50">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Route To ID" ReadOnly="True" VisibleIndex="51">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Created By" ReadOnly="True" VisibleIndex="52">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Created On" ReadOnly="True" VisibleIndex="53">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Modified By" ReadOnly="True" VisibleIndex="54">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Modified On" ReadOnly="True" VisibleIndex="55">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Quik-Post Job" VisibleIndex="56">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Post Notes" ReadOnly="True" VisibleIndex="57">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Jobid" VisibleIndex="5">
                <PropertiesTextEdit DisplayFormatString="{0}">
                </PropertiesTextEdit>
                <Settings AllowSort="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
&nbsp;
    
    <%--<asp:SqlDataSource DataSourceMode="DataReader" runat="server" ConnectionString="<%$ ConnectionStrings:ClientConnectionString %>" SelectCommand="filter_GetFilteredPlannedJobstepsList" SelectCommandType="StoredProcedure" ProviderName="<%$ ConnectionStrings:ClientConnectionString.ProviderName %>">
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
    </asp:SqlDataSource>--%>

</asp:Content>