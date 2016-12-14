<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="myJobs.aspx.cs" Inherits="Pages_PlannedJobs_myJobs" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<%@ Register assembly="Infragistics4.WebUI.WebResizingExtender.v15.1, Version=15.1.20151.2400, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI" tagprefix="igui" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">MY Jobs</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>My Jobs</h1>
    <script type="text/javascript">
        function OnGetRowID(idValue) {
            Selection.Set('Jobid', idValue[0].toString());
            Selection.Set('n_Jobid', idValue[1].toString());
            Selection.Set('n_jobstepid', idValue[2].toString());
            Selection.Set('step', idValue[3].toString());
        }
    </script>
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <dx:ASPxGridView ID="myJobsGrid" runat="server" Theme="Mulberry" Width="1012px" AutoGenerateColumns="False" EnableTheming="True" ClientInstanceName="myJobsGrid"  SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectByRowClick="false" KeyFieldName="n_jobstepid">

        <Styles>
<Header CssClass="gridViewHeader"></Header>



<Row CssClass="gridViewRow"></Row>

<RowHotTrack CssClass="gridViewRow"></RowHotTrack>

            <SelectedRow BackColor="#FF3300">
            </SelectedRow>
            <FocusedRow BackColor="#FF3300">
            </FocusedRow>

<FilterRow CssClass="gridViewFilterRow"></FilterRow>

            <InlineEditCell BackColor="#FFFFCC">
            </InlineEditCell>
            <SearchPanel Border-BorderColor="Black" Border-BorderStyle="Solid">
            </SearchPanel>
        </Styles>
        <StylesContextMenu>
            <Row>
                <Link HoverColor="#66FFFF">
                </Link>
            </Row>
        </StylesContextMenu>

        <SettingsPager Position="Top"></SettingsPager>

        <Settings AutoFilterCondition="Equals" EnableFilterControlPopupMenuScrolling="True" HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollableHeight="450" VerticalScrollBarMode="Visible"></Settings>

        <SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

        <SettingsCommandButton>
            <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

            <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
        </SettingsCommandButton>

        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False"></SettingsDataSecurity>
        <SettingsSearchPanel ShowApplyButton="True" ShowClearButton="True" Visible="True"></SettingsSearchPanel>
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" Width="100px" FixedStyle="Left" Visible="False">
                <CellStyle HorizontalAlign="Left" Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="n_jobstepid" ReadOnly="True" VisibleIndex="9" Visible="False">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="n_jobid" VisibleIndex="4" FixedStyle="Left" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="T" ReadOnly="True" VisibleIndex="6" Caption="Type" Width="50px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="A" ReadOnly="True" VisibleIndex="45" Caption="Job Against">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Object ID" VisibleIndex="8" Caption="Object ID">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="step" VisibleIndex="5" Caption="Step #" Width="50px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Step Title" VisibleIndex="3" Caption="Description" Width="300px" CellStyle-Wrap="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="10" Caption="Priority">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="11" Caption="Status" Settings-AllowSort="True" Settings-AllowFilterBySearchPanel="True">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Requested" ReadOnly="True" VisibleIndex="12" Caption="Requested">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="Starting Date" ReadOnly="True" VisibleIndex="1" FixedStyle="Left" Caption="Starting" Width="75px" Settings-AllowSort="True" Settings-AllowFilterBySearchPanel="True">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Labor Class" VisibleIndex="13" Caption="Labor Class">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Group" VisibleIndex="46" Caption="Group">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Outcome" VisibleIndex="47" Caption="Outcome">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Supervisor" VisibleIndex="48" Caption="Supervisor">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="H" VisibleIndex="49" Caption="History">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="I" ReadOnly="True" VisibleIndex="7" Caption="Issued">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="cReasoncode" VisibleIndex="14" Caption="Reason">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssignedTaskID" VisibleIndex="15" Caption="Task">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Completion Date" ReadOnly="True" VisibleIndex="16" Caption="Completed">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOpID" VisibleIndex="17" Caption="Work Op">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HighwayRouteID" ReadOnly="True" VisibleIndex="18" Caption="Hwy. Route">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MileMarker" ReadOnly="True" VisibleIndex="19" Caption="MM. Form">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssetNumber" VisibleIndex="20" Caption="Asset #">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ResponsiblePersonID" ReadOnly="True" VisibleIndex="21" Caption="Resp. Person">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="nMaintObjectID" ReadOnly="True" VisibleIndex="50" Visible="False">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BreakdownJob" ReadOnly="True" VisibleIndex="22" Caption="Breakdown">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DowntimeHrs" VisibleIndex="23" Caption="DT Hours">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Ontime" ReadOnly="True" VisibleIndex="24" Caption="Ontime">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ReturnWithin" VisibleIndex="25" Caption="Rtn W/in">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SubAssembly" VisibleIndex="26" Caption="Sub Assembly">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AreaID" VisibleIndex="27" Caption="Area">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FlaggedRecordID" ReadOnly="True" VisibleIndex="51" Caption="Flagged Record">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ObjectDescr" VisibleIndex="28" Caption="Object Descr" Width="200px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CostCodeID" ReadOnly="True" VisibleIndex="29" Caption="Cost Code">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" ReadOnly="True" VisibleIndex="30" Caption="Funding Group">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" ReadOnly="True" VisibleIndex="31" Caption="Work Order Code">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" ReadOnly="True" VisibleIndex="32" Caption="Org. Code">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" ReadOnly="True" VisibleIndex="33">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ControlSectionID" ReadOnly="True" VisibleIndex="34" Caption="Ctl. Section">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" ReadOnly="True" VisibleIndex="35" Caption="Equip #">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MileMarkerTo" ReadOnly="True" VisibleIndex="36" Caption="MM To">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EstimatedUnits" VisibleIndex="37" Caption="Est. Units">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ActualUnits" VisibleIndex="38" Caption="Actual Units">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShiftID" VisibleIndex="39" Caption="Shift">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Crew Assigned" ReadOnly="True" VisibleIndex="40" Caption="Crew Assigned">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Parts Assigned" ReadOnly="True" VisibleIndex="41" Caption="Parts Assigned">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Equip. Assigned" ReadOnly="True" VisibleIndex="42" Caption="Equip. Assigned">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Other Assigned" ReadOnly="True" VisibleIndex="43" Caption="Other Assigned">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Route To ID" ReadOnly="True" VisibleIndex="44" Caption="Route To">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Created By" ReadOnly="True" VisibleIndex="52" Caption="Created By">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Created On" ReadOnly="True" VisibleIndex="53" Caption="Created On">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Modified By" ReadOnly="True" VisibleIndex="54" Caption="Modified By">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Modified On" ReadOnly="True" VisibleIndex="55" Caption="Modified On">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Quik-Post Job" VisibleIndex="56" Caption="Quick Post Job">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Post Notes" ReadOnly="True" VisibleIndex="57" Caption="Post Notes" CellStyle-Wrap="False" Width="200">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Jobid" VisibleIndex="2" Caption="Job ID" FixedStyle="Left" Settings-AllowSort="True" Settings-AllowFilterBySearchPanel="True">
                <DataItemTemplate>
                    <asp:HyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# GetUrl(Container) %>"
                        Text='<%# Eval("Jobid") %>' Width="100%" Theme="Mulberry"></asp:HyperLink>
                </DataItemTemplate>
                <PropertiesTextEdit DisplayFormatString="{0}">
                </PropertiesTextEdit>
                <Settings AllowSort="False" />
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem ShowInColumn="n_jobid" SummaryType="Sum" />
        </TotalSummary>

        <Styles FilterRow-CssClass="gridViewFilterRow" FocusedRow-CssClass="gridViewRowFocused" Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" RowHotTrack-CssClass="gridViewRow">
            <Header CssClass="gridViewHeader"></Header>
            <Row CssClass="gridViewRow"></Row>

            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
            <SelectedRow BackColor="#FF3300"></SelectedRow>
            <FocusedRow BackColor="#FF3300" CssClass="gridViewRowFocused"></FocusedRow>
            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
            <PagerBottomPanel>
                <Paddings Padding="1px"></Paddings>
            </PagerBottomPanel>

            <InlineEditCell BackColor="#FFFFCC"> </InlineEditCell>
            <SearchPanel Border-BorderColor="#E2E1E0" Border-BorderStyle="Solid" Border-BorderWidth="1px"></SearchPanel>
        </Styles>

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