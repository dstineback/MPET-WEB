 <%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="myJobs.aspx.cs" Inherits="Pages_PlannedJobs_myJobs" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">MY Jobs</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>MY JOBS</h1>
    <dx:ASPxGridView ID="myJobsGrid" runat="server" Theme="Mulberry" Width="1012px" AutoGenerateColumns="False" EnableTheming="True" SettingsBehavior-AllowFocusedRow="false" SettingsBehavior-AllowSelectByRowClick="true" KeyFieldName="n_jobstepid" SettingsText-EmptyDataRow="No Data" SettingsText-EmptyHeaders="No Data">
        <SettingsAdaptivity AdaptivityMode="HideDataCells"></SettingsAdaptivity>
        <Styles>
            <Header CssClass="gridViewHeader"></Header>
            <Row CssClass="gridViewRow"></Row>
            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                <SelectedRow BackColor="#FF3300"></SelectedRow>
                <FocusedRow BackColor="#FF3300"></FocusedRow>
            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                <InlineEditCell BackColor="#FFFFCC"></InlineEditCell>
                <SearchPanel Border-BorderColor="Black" Border-BorderStyle="Solid"></SearchPanel>
        </Styles>
        <StylesContextMenu>
            <Row>
                <Link HoverColor="#66FFFF"></Link>
            </Row>
        </StylesContextMenu>
        <SettingsPager Position="Top"></SettingsPager>
        <Settings ShowFilterRow="True" ShowGroupPanel="True" AutoFilterCondition="Equals" EnableFilterControlPopupMenuScrolling="True" HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollableHeight="500" VerticalScrollBarMode="Visible"></Settings>

<SettingsBehavior AllowSelectByRowClick="True"></SettingsBehavior>

        <SettingsCommandButton>
            <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>
            <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
        </SettingsCommandButton>
        <SettingsDataSecurity AllowDelete="False"></SettingsDataSecurity>
        <SettingsSearchPanel Visible="True" ShowApplyButton="True" ShowClearButton="True"></SettingsSearchPanel>

<SettingsText EmptyHeaders="No Data" EmptyDataRow="No Data"></SettingsText>
        <Columns>
            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Width="100px">
                <CellStyle HorizontalAlign="Left" Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="n_jobstepid" ReadOnly="True" VisibleIndex="9" Visible="False">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="n_jobid" VisibleIndex="4" FixedStyle="Left" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="T" 
                ReadOnly="True" VisibleIndex="6" Caption="Type" 
                Width="75px" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="A" 
                ReadOnly="True" VisibleIndex="45" 
                Caption="Job Against" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Object ID" VisibleIndex="8" Caption="Object ID">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="step" 
                VisibleIndex="5" Caption="Step #" Width="50px" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Step Title" VisibleIndex="3" Caption="Description" Width="300px" CellStyle-Wrap="False">
<CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="10" Caption="Priority">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="11" Caption="Status" Settings-AllowSort="True" Settings-AllowFilterBySearchPanel="True">
<Settings AllowSort="True" AllowFilterBySearchPanel="True"></Settings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Requested" 
                ReadOnly="True" VisibleIndex="12" 
                Caption="Requested" Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="Starting Date" ReadOnly="True" VisibleIndex="1" FixedStyle="Left" Caption="Starting" Width="75px" Settings-AllowSort="True" Settings-AllowFilterBySearchPanel="True">
<Settings AllowSort="True" AllowFilterBySearchPanel="True"></Settings>

                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Labor Class" 
                VisibleIndex="13" Caption="Labor Class" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Group" 
                VisibleIndex="46" Caption="Group" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Outcome" 
                VisibleIndex="47" Caption="Outcome" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Supervisor" 
                VisibleIndex="48" Caption="Supervisor" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="H" 
                VisibleIndex="49" Caption="History" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="I" 
                ReadOnly="True" VisibleIndex="7" Caption="Issued" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="cReasoncode" 
                VisibleIndex="14" Caption="Reason" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssignedTaskID" 
                VisibleIndex="15" Caption="Task" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Completion Date" 
                ReadOnly="True" VisibleIndex="16" 
                Caption="Completed" Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOpID" 
                VisibleIndex="17" Caption="Work Op" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HighwayRouteID" 
                ReadOnly="True" VisibleIndex="18" 
                Caption="Hwy. Route" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MileMarker" 
                ReadOnly="True" VisibleIndex="19" 
                Caption="MM. Form" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssetNumber" 
                VisibleIndex="20" Caption="Asset #" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ResponsiblePersonID" 
                ReadOnly="True" VisibleIndex="21" 
                Caption="Resp. Person" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="nMaintObjectID" ReadOnly="True" VisibleIndex="50" Visible="False">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BreakdownJob" 
                ReadOnly="True" VisibleIndex="22" 
                Caption="Breakdown" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DowntimeHrs" 
                VisibleIndex="23" Caption="DT Hours" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Ontime" 
                ReadOnly="True" VisibleIndex="24" 
                Caption="Ontime" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ReturnWithin" 
                VisibleIndex="25" Caption="Rtn W/in" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SubAssembly" 
                VisibleIndex="26" Caption="Sub Assembly" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AreaID" 
                VisibleIndex="27" Caption="Area" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FlaggedRecordID" 
                ReadOnly="True" VisibleIndex="51" 
                Caption="Flagged Record" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ObjectDescr" 
                VisibleIndex="28" Caption="Object Descr" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CostCodeID" 
                ReadOnly="True" VisibleIndex="29" 
                Caption="Cost Code" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" 
                ReadOnly="True" VisibleIndex="30" 
                Caption="Funding Group" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" 
                ReadOnly="True" VisibleIndex="31" 
                Caption="Work Order Code" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" 
                ReadOnly="True" VisibleIndex="32" 
                Caption="Org. Code" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" 
                ReadOnly="True" VisibleIndex="33" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ControlSectionID" 
                ReadOnly="True" VisibleIndex="34" 
                Caption="Ctl. Section" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" 
                ReadOnly="True" VisibleIndex="35" 
                Caption="Equip #" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MileMarkerTo" 
                ReadOnly="True" VisibleIndex="36" Caption="MM To" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EstimatedUnits" 
                VisibleIndex="37" Caption="Est. Units" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ActualUnits" 
                VisibleIndex="38" Caption="Actual Units" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShiftID" 
                VisibleIndex="39" Caption="Shift" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Crew Assigned" 
                ReadOnly="True" VisibleIndex="40" 
                Caption="Crew Assigned" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Parts Assigned" 
                ReadOnly="True" VisibleIndex="41" 
                Caption="Parts Assigned" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Equip. Assigned" 
                ReadOnly="True" VisibleIndex="42" 
                Caption="Equip. Assigned" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Other Assigned" 
                ReadOnly="True" VisibleIndex="43" 
                Caption="Other Assigned" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Route To ID" 
                ReadOnly="True" VisibleIndex="44" 
                Caption="Route To" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Created By" 
                ReadOnly="True" VisibleIndex="52" 
                Caption="Created By" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Created On" 
                ReadOnly="True" VisibleIndex="53" 
                Caption="Created On" Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Modified By" 
                ReadOnly="True" VisibleIndex="54" 
                Caption="Modified By" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Modified On" 
                ReadOnly="True" VisibleIndex="55" 
                Caption="Modified On" Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Quik-Post Job" 
                VisibleIndex="56" Caption="Quick Post Job" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Post Notes" 
                ReadOnly="True" VisibleIndex="57" 
                Caption="Post Notes" CellStyle-Wrap="False" 
                Width="200" Visible="False">
<CellStyle Wrap="False"></CellStyle>
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
            <Row CssClass="gridViewRow" ></Row>
            
            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
            <SelectedRow BackColor="#FF3300"></SelectedRow>
            <FocusedRow BackColor="#FF3300" CssClass="gridViewRowFocused"></FocusedRow>
            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
            <InlineEditCell BackColor="#FFFFCC"> </InlineEditCell>
            <SearchPanel Border-BorderColor="#E2E1E0" Border-BorderStyle="Solid" Border-BorderWidth="1px"></SearchPanel>
        </Styles>

    </dx:ASPxGridView>
&nbsp;
    
   

</asp:Content>