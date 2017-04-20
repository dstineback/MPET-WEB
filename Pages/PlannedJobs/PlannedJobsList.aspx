<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedJobsList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.PlannedJobs.PlannedJobsList" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Planned Jobs</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">  
    <h1>PLANNED JOBS</h1> 
        <script type="text/javascript">

            function OnGetRowId(idValue) {
                Selection.Set('Jobid', idValue[0].toString());
                Selection.Set('n_jobid', idValue[1].toString());
                Selection.Set('n_jobstepid', idValue[2].toString());
                Selection.Set('step', idValue[3].toString());
                Selection.Set('Step Title', idValue[4].toString());
                Selection.Set('Object ID', idValue[5].toString());
                Selection.Set('Latitude', idValue[6].toString());
                Selection.Set('Longitude', idValue[7].toString());
            }

            function HidePopup() {
                window.BatchCrewPopup.Hide();
                window.BatchSuperPopup.Hide();
                window.BatchPostPopup.Hide();
                window.RoutineJobPopup.Hide();
                window.ForcePMPopup.Hide();
                window.BatchEquipPopup.Hide();
                window.BatchPartPopup.Hide();
            }
        </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
            <dx:ASPxGridView
                ID="PlannedGrid"
                runat="server"
                Theme="Mulberry"
                KeyFieldName="n_jobstepid"
                Width="98%"
                KeyboardSupport="True"
                ClientInstanceName="PlannedGrid"
                AutoPostBack="True"
                Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Visible" Settings-VerticalScrollBarStyle="Standard" SettingsBehavior-AllowSelectByRowClick="true"
                SettingsPager-Mode="ShowPager" 
                SettingsBehavior-ProcessFocusedRowChangedOnServer="False"
                SettingsBehavior-AllowFocusedRow="False" 
                SelectionMode="Multiple"
                DataSourceID="ObjectGridDataSource"
                OnHtmlDataCellPrepared="ASPxGridView1_HtmlRowPrepared" AutoGenerateColumns="False">
                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"
                    RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow">
                    <Header CssClass="gridViewHeader"></Header>

                    <Row  CssClass="gridViewRow"></Row>

                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                </Styles>
                <ClientSideEvents RowClick="function(s, e) {
                        PlannedGrid.GetRowValues(e.visibleIndex, 'Jobid;n_jobid;n_jobstepid;step;Step Title;Object ID;Latitude;Longitude', OnGetRowId);
                        
                    }" />

                <Settings ShowGroupPanel="True" VerticalScrollBarStyle="Standard"
                    VerticalScrollableHeight="500" ShowFilterBar="Visible"
                    HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Visible">
                </Settings>


                <Columns>
                    <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True"
                        Visible="false" VisibleIndex="0" />
                    <dx:GridViewDataTextColumn FieldName="n_jobstepid"
                        ReadOnly="True" Visible="false" VisibleIndex="1">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_jobid" ReadOnly="True"
                        Visible="false" VisibleIndex="5">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="T" Caption="Type"
                        Width="60px" VisibleIndex="16">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Jobid" FixedStyle="Left"
                        HeaderStyle-Font-Bold="True" Caption="Job ID" Width="120px"
                        VisibleIndex="3">
                        <CellStyle Wrap="False"></CellStyle>
                        <%--<PropertiesHyperLinkEdit NavigateUrl="<%# GetUrl(Container) %>"></PropertiesHyperLinkEdit>--%>
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server"
                                NavigateUrl="<%# GetUrl(Container) %>"
                                Text='<%# Eval("Jobid") %>' Width="100%" Theme="Mulberry">
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Object ID" Caption="Object ID"
                        Width="160px" VisibleIndex="4">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="step" Caption="Step #"
                        Width="60px" VisibleIndex="10">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Step Title" Caption="Description"
                        Width="300px" VisibleIndex="4">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Priority" Caption="Priority"
                        Width="100px" VisibleIndex="8">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Status" Caption="Status"
                        Width="100px" VisibleIndex="9">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Requested" Caption="Requested"
                        Width="120px"
                        Settings-AllowHeaderFilter="True" ReadOnly="True"
                        VisibleIndex="6">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="Starting Date"
                        FixedStyle="Left" Caption="Starting" Width="120px"
                        Settings-AllowHeaderFilter="True" SortOrder="Descending"
                        ReadOnly="True" VisibleIndex="2">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy">
                        </PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="Labor Class"
                        Caption="Labor Class" Width="120px" ReadOnly="True"
                        VisibleIndex="12" Settings-AllowHeaderFilter="True">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="I" Caption="Issued"
                        Width="100px" ReadOnly="True" VisibleIndex="13">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cReasoncode"
                        Caption="Reason" Width="100px" ReadOnly="True"
                        VisibleIndex="14">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AssignedTaskID"
                        Caption="Task" Width="100px" ReadOnly="True" VisibleIndex="15">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Completion Date"
                        Caption="Completed" Width="120px"
                        Settings-AllowHeaderFilter="True" ReadOnly="True"
                        VisibleIndex="7">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOpID" Caption="Work Op"
                        Width="120px" ReadOnly="True" VisibleIndex="17"
                        Settings-AllowHeaderFilter="True" Settings-AllowGroup="true">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="HighwayRouteID"
                        Caption="Hwy. Route" Width="100px" ReadOnly="True"
                        VisibleIndex="18">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MileMarker" Caption="MM. From"
                        Width="100px" ReadOnly="True" VisibleIndex="19">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MileMarkerTo"
                        Caption="MM. To" Width="100px" ReadOnly="True"
                        VisibleIndex="37">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AssetNumber"
                        Caption="Asset #" Width="100px" ReadOnly="True"
                        VisibleIndex="20">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ResponsiblePersonID"
                        Caption="Resp. Person" Width="100px" ReadOnly="True"
                        VisibleIndex="21">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="nMaintObjectID"
                        ReadOnly="True" Visible="false" VisibleIndex="22">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="BreakdownJob"
                        Caption="Breakdown" Width="100px" ReadOnly="True"
                        VisibleIndex="23">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DowntimeHrs"
                        Caption="Dt. Hours" Width="100px" ReadOnly="True"
                        VisibleIndex="24">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Ontime" Caption="Ontime"
                        Width="100px" ReadOnly="True" VisibleIndex="24">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ReturnWithin"
                        Caption="Rtn W/in" Width="100px" ReadOnly="True"
                        VisibleIndex="26">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SubAssembly"
                        Caption="Sub. Asmbly" Width="100px" ReadOnly="True"
                        VisibleIndex="27">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AreaID" Settings-AllowHeaderFilter="True"
                        Caption="Area" Width="100px" ReadOnly="True" VisibleIndex="3">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ObjectDescr"
                        Caption="Obj. Descr." Width="100px" ReadOnly="True"
                        VisibleIndex="29">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CostCodeID" Caption="Cost Code"
                        Width="100px" ReadOnly="True" VisibleIndex="30">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundSrcCodeID"
                        Caption="Fund. Src." Width="100px" ReadOnly="True"
                        VisibleIndex="31">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID"
                        Caption="Work Order" Width="100px" ReadOnly="True"
                        VisibleIndex="32">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="OrganizationCodeID"
                        Caption="Org. Code" Width="100px" ReadOnly="True"
                        VisibleIndex="33">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID"
                        Caption="Fund. Group" Width="100px" ReadOnly="True"
                        VisibleIndex="34">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ControlSectionID"
                        Caption="Ctl. Section" Width="100px" ReadOnly="True"
                        VisibleIndex="35">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EquipmentNumberID"
                        Caption="Equip. #" Width="100px" ReadOnly="True"
                        VisibleIndex="36">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EstimatedUnits"
                        Caption="Est. Units" Width="100px" ReadOnly="True"
                        VisibleIndex="38">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ActualUnits"
                        Caption="Act. Units" Width="100px" ReadOnly="True"
                        VisibleIndex="39">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ShiftID" Caption="Shift"
                        Width="100px" ReadOnly="True" VisibleIndex="40">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Crew Assigned"
                        Caption="Crew Asgd." Width="100px" ReadOnly="True"
                        VisibleIndex="41">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Parts Assigned"
                        Caption="Parts Asgd." Width="100px" ReadOnly="True"
                        VisibleIndex="42">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Equip. Assigned"
                        Caption="Equip. Asgd." Width="100px" ReadOnly="True"
                        VisibleIndex="43">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Other Assigned"
                        Caption="Other Asgd." Width="100px" ReadOnly="True"
                        VisibleIndex="44">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Route To ID"
                        Caption="Route To" Width="100px" ReadOnly="True"
                        VisibleIndex="45">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Latitude" Caption="Latitude"
                        Width="100px" ReadOnly="true" VisibleIndex="46">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Longitude" Caption="Longitude"
                        Width="100px" ReadOnly="true" VisibleIndex="47">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsSearchPanel Visible="true" />
                <SettingsBehavior
                    EnableRowHotTrack="True"
                    AllowFocusedRow="True"
                    AllowClientEventsOnLoad="false"
                    ColumnResizeMode="NextColumn" />
                <SettingsDataSecurity
                    AllowDelete="False"
                    AllowInsert="False" />
                <SettingsPopup HeaderFilter-Width="360" HeaderFilter-Height="360">
                </SettingsPopup>

                <GroupSummary>
                    <dxe:ASPxSummaryItem SummaryType="Count"></dxe:ASPxSummaryItem>
                </GroupSummary>

                <Styles>
                    <DetailRow CssClass="GridViewRow" Font-Size="12pt">
                    </DetailRow>

                    <DetailCell Wrap="False">
                        <Paddings PaddingLeft="0px" PaddingTop="0px" PaddingRight="0px"
                            PaddingBottom="0px"></Paddings>
                    </DetailCell>

                </Styles>
                <Settings 
                    ShowFilterBar="Visible"
                    VerticalScrollBarMode="Visible" 
                    VerticalScrollBarStyle="Standard" 
                    VerticalScrollableHeight="500" />
                <SettingsPager PageSize="500">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>

    </dx:ASPxGridView>
            <asp:SqlDataSource ID="ObjectGridDataSource" 
                               runat="server" 
                               ConnectionString="<%$ ConnectionStrings:connection %>"  
                               SelectCommand="
                                DECLARE @NullDate DATETIME
                                SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
                            
                            --Create Area Filering On Variable
                                DECLARE @areaFilteringOn VARCHAR(1)
                            
                            --Setup Area Filering Variable
                                IF ( ( SELECT   COUNT(dbo.UsersAreaFilter.RecordID)
                                       FROM     dbo.UsersAreaFilter WITH ( NOLOCK )
                                       WHERE    UsersAreaFilter.UserID = @UserID
                                                AND UsersAreaFilter.FilterActive = 'Y'
                                     ) > 0 )
                                    BEGIN
                                        SET @areaFilteringOn = 'Y'
                                    END
                                ELSE
                                    BEGIN
                                        SET @areaFilteringOn = 'N'
                                    END                           
                            ;
WITH    cte_Jobsteps
              AS ( SELECT   tbl_Jobsteps.n_jobstepid ,
                            tbl_Jobsteps.n_jobid ,
                            tbl_Jobs.Jobid ,
                            tbl_Priorities.priorityid ,
                            tbl_Jobs.JobTypeCode ,
                            tbl_Jobs.objectid ,
                            tbl_Jobsteps.stepnumber ,
                            tbl_Jobsteps.JobstepTitle ,
                            tbl_Jobs.EstimatedUnits ,
                            tbl_Jobs.ActualUnits ,
                            tbl_Jobs.RequestDate ,
                            tbl_Jobs.n_objectid ,
                            tbl_Jobs.IsIssued ,
                            CASE tbl_Jobsteps.StartingDate
                              WHEN @NullDate THEN NULL
                              ELSE CAST(CAST(tbl_Jobsteps.StartingDate AS VARCHAR(11)) AS DATETIME)
                            END AS StartingDate ,
                            CASE tbl_Jobsteps.DateTimeCompleted
                              WHEN @NullDate THEN NULL
                              ELSE CAST(CAST(tbl_Jobsteps.DateTimeCompleted AS VARCHAR(11)) AS DATETIME)
                            END AS CompletionDate ,
                            CASE WHEN tbl_Jobsteps.DateTimeCompleted = @NullDate THEN ''
                                 WHEN ( CAST(CAST(tbl_Jobsteps.DateTimeCompleted AS VARCHAR(11)) AS DATETIME) > ( (DATEADD(dd, tbl_Jobsteps.return_within, CAST(CAST(tbl_Jobsteps.StartingDate AS VARCHAR(11)) AS DATETIME))) ) ) THEN 'N'
                                 ELSE 'Y' --On Time By Default
                            END AS [Ontime] ,
                            tbl_Jobs.assetnumber ,
                            tbl_Jobsteps.return_within ,
                            tbl_Jobsteps.actualdowntime ,
                            tbl_Jobs.ObjectDescr ,
                            tbl_Jobs.MileMarkerTo ,
                            tbl_Jobs.MileMarker ,
                            tbl_LC.laborclassid ,
                            tbl_Groups.groupid ,
                            tbl_Outcome.outcomecodeid ,
                            tbl_Supervisor.Username ,
                            tbl_JobReasons.JobReasonID ,
                            tbl_Jobs.taskid ,
                            tbl_WorkOp.WorkOpID ,
                            tbl_Jobs.BreakdownJob ,
                            tbl_Jobs.HighwayRouteID ,
                            tbl_Jobs.ResponsiblePersonID ,
                            tbl_SubAssembly.SubAssemblyName ,
                            tbl_Jobs.areaid ,
                            CASE tbl_CostCodes.n_costcodeid
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_CostCodes.CostCodeID
                            END AS [CostCodeID] ,
                            CASE tbl_FundSource.n_FundSrcCodeID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_FundSource.FundSrcCodeID
                            END AS [FundSrcCodeID] ,
                            CASE tbl_WorkOrders.n_WorkOrderCodeID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_WorkOrders.WorkOrderCodeID
                            END AS [WorkOrderCodeID] ,
                            CASE tbl_OrgCode.n_OrganizationCodeID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_OrgCode.OrganizationCodeID
                            END AS [OrganizationCodeID] ,
                            CASE tbl_FundGroup.n_FundingGroupCodeID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_FundGroup.FundingGroupCodeID
                            END AS [FundingGroupCodeID] ,
                            CASE tbl_ControlSection.n_ControlSectionID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_ControlSection.ControlSectionID
                            END AS [ControlSectionID] ,
                            CASE tbl_EquipNumber.n_EquipmentNumberID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_EquipNumber.EquipmentNumberID
                            END AS [EquipmentNumberID] ,
                            tbl_Statuses.StatusID ,
                            CASE tbl_Jobsteps.n_RouteToID
                              WHEN -1 THEN ''
                              WHEN 0 THEN ''
                              ELSE tbl_RouteTo.Username
                            END AS 'RouteToID' ,
                            tbl_Shifts.ShiftID ,
                            tbl_CrewExists.[CrewExists],
							tbl_PartsExists.[PartsExists],
							tbl_EquipExists.[EquipExists],
							tbl_OtherExists.[OtherExists],
                            tbl_Jobs.X,
							tbl_Jobs.Y
                   FROM     dbo.Jobsteps tbl_Jobsteps
                            INNER JOIN ( SELECT tblJobs.n_Jobid ,
                                                tblJobs.Jobid ,
                                                CASE tblJobs.TypeOfJob
                                                  WHEN 1 THEN 'P'
                                                  WHEN 2 THEN 'C'
                                                  WHEN 3 THEN 'R'
                                                  WHEN 4 THEN 'B'
                                                  ELSE 'U'
                                                END AS JobTypeCode ,
                                                CASE tblJobs.TypeOfJob
                                                  WHEN 4 THEN 'Y'
                                                  ELSE 'N'
                                                END AS BreakdownJob ,
                                                tbl_Objects.n_objectid ,
                                                tbl_Objects.objectid ,
                                                tblJobs.EstimatedUnits ,
                                                tblJobs.ActualUnits ,
                                                CASE tblJobs.RequestDate
                                                  WHEN @NullDate THEN NULL
                                                  ELSE CAST(CAST(tblJobs.RequestDate AS VARCHAR(11)) AS DATETIME)
                                                END AS RequestDate ,
                                                CASE tblJobs.IssuedDate
                                                  WHEN @NullDate THEN 'N'
                                                  ELSE 'Y'
                                                END AS IsIssued ,
                                                tbl_Objects.assetnumber ,
                                                tbl_Objects.description AS ObjectDescr ,
                                                CASE tblJobs.MilepostTo
                                                  WHEN 0.0000 THEN NULL
                                                  ELSE tblJobs.MilepostTo
                                                END AS MileMarkerTo ,
                                                CASE tblJobs.Milepost
                                                  WHEN 0.0000 THEN NULL
                                                  ELSE tblJobs.Milepost
                                                END AS MileMarker ,
                                                tbl_Tasks.taskid ,
                                                CASE tbl_StateRoutes.n_StateRouteID
                                                  WHEN 0 THEN ''
                                                  WHEN -1 THEN ''
                                                  ELSE tbl_StateRoutes.StateRouteID
                                                END AS HighwayRouteID ,
                                                tbl_Objects.ResponsiblePersonID ,
                                                tbl_Objects.areaid,
                                                tbl_Objects.X,
												tbl_Objects.Y
                                         FROM   dbo.Jobs tblJobs
                                                INNER JOIN ( SELECT tblObjects.n_objectid ,
                                                                    tblObjects.objectid ,
                                                                    tblObjects.description ,
                                                                    tblObjects.assetnumber ,
                                                                    CASE tblObjects.n_ResponsiblePerson
                                                                      WHEN -1 THEN ''
                                                                      WHEN 0 THEN ''
                                                                      ELSE tblRespPerson.FullName
                                                                    END AS ResponsiblePersonID ,
                                                                    tbl_Areas.areaid,
                                                                    tblObjects.GPS_X AS X,
																	tblObjects.GPS_Y AS Y
                                                             FROM   dbo.MaintenanceObjects tblObjects
                                                                    INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                        tblAreas.areaid
                                                                                 FROM   dbo.Areas tblAreas
                                                                                 WHERE  ( ( @areaFilteringOn = 'Y'
                                                                                                AND EXISTS ( SELECT recordMatches.AreaFilterID
                                                                                                             FROM   dbo.UsersAreaFilter AS recordMatches
                                                                                                             WHERE  tblAreas.n_areaid = recordMatches.AreaFilterID
                                                                                                                    AND recordMatches.UserID = @UserID
                                                                                                                    AND recordMatches.FilterActive = 'Y' )
                                                                                              )
                                                                                              OR ( @areaFilteringOn = 'N' )
                                                                                            )
                                                                               ) tbl_Areas ON tblObjects.n_areaid = tbl_Areas.n_areaid
                                                                    INNER JOIN ( SELECT tblLocations.n_locationid ,
                                                                                        tblLocations.locationid
                                                                                 FROM   dbo.locations tblLocations
                                                                               ) tbl_Locations ON tblObjects.n_locationid = tbl_Locations.n_locationid
                                                                    INNER JOIN ( SELECT tblObjectTypes.n_objtypeid ,
                                                                                        tblObjectTypes.objtypeid
                                                                                 FROM   dbo.objecttypes tblObjectTypes
                                                                               ) tbl_ObjectTypes ON tblObjects.n_objtypeid = tbl_ObjectTypes.n_objtypeid
                                                                    INNER JOIN ( SELECT dbo.MPetUsers.UserID ,
                                                                                        dbo.MpetUsers.LastName + ', ' + dbo.Mpetusers.FirstName AS 'FullName'
                                                                                 FROM   dbo.MPetUsers
                                                                               ) tblRespPerson ON tblRespPerson.UserID = tblObjects.n_ResponsiblePerson
                                                           ) tbl_Objects ON tbl_Objects.n_objectid = tblJobs.n_MaintObjectID
                                                INNER JOIN ( SELECT tblTasks.n_taskid ,
                                                                    tblTasks.taskid
                                                             FROM   dbo.Tasks tblTasks
                                                           ) tbl_Tasks ON tblJobs.n_TaskID = tbl_Tasks.n_taskid
                                                INNER JOIN ( SELECT tblStateRoutes.n_StateRouteID ,
                                                                    tblStateRoutes.StateRouteID
                                                             FROM   dbo.StateRoutes tblStateRoutes
                                                           ) tbl_StateRoutes ON tblJobs.n_StateRouteID = tbl_StateRoutes.n_StateRouteID
                                                INNER JOIN ( SELECT tblRequestor.UserID ,
                                                                    tblRequestor.Username
                                                             FROM   dbo.MPetUsers tblRequestor
 
                                                           ) tbl_Reqestor ON tblJobs.n_requestor = tbl_Reqestor.UserID
                                                INNER JOIN ( SELECT tblPoster.UserID ,
                                                                    tblPoster.Username
                                                             FROM   dbo.MPetUsers tblPoster
                                                           ) tbl_Poster ON tblJobs.PostedBy = tbl_Poster.UserID
										WHERE tblJobs.IsHistory = 'N'
                                       ) tbl_Jobs ON tbl_Jobs.n_Jobid = tbl_Jobsteps.n_jobid
                            INNER JOIN ( SELECT tblPriorities.n_priorityid ,
                                                tblPriorities.priorityid
                                         FROM   dbo.Priorities tblPriorities
                                       ) tbl_Priorities ON tbl_Priorities.n_priorityid = tbl_Jobsteps.PriorityID
                            INNER JOIN ( SELECT tblRouteTo.UserID ,
                                                tblRouteTo.Username
                                         FROM   dbo.MPetUsers tblRouteTo
                                       ) tbl_RouteTo ON tbl_Jobsteps.n_RouteToID = tbl_RouteTo.UserID
                            INNER JOIN ( SELECT tblLC.n_laborclassid ,
                                                tblLC.laborclassid
                                         FROM   dbo.laborclasses tblLC
                                       ) tbl_LC ON tbl_LC.n_laborclassid = tbl_Jobsteps.n_laborclassid
                            INNER JOIN ( SELECT tblGroups.n_groupid ,
                                                tblGroups.groupid
                                         FROM   dbo.groups tblGroups
                                       ) tbl_Groups ON tbl_Groups.n_groupid = tbl_Jobsteps.n_groupid
                            INNER JOIN ( SELECT tblOutcome.n_outcomecodeid ,
                                                tblOutcome.outcomecodeid
                                         FROM   dbo.OutcomeCodes tblOutcome
                                       ) tbl_Outcome ON tbl_Outcome.n_outcomecodeid = tbl_Jobsteps.n_OutcomeCode
                            INNER JOIN ( SELECT tblSupervisor.UserID ,
                                                tblSupervisor.Username
                                         FROM   dbo.MPetUsers tblSupervisor
                                       ) tbl_Supervisor ON tbl_Supervisor.UserID = tbl_Jobsteps.n_supervisorid
                            INNER JOIN ( SELECT tblReasons.nJobReasonID ,
                                                tblReasons.JobReasonID
                                         FROM   dbo.JobReasons tblReasons
                                       ) tbl_JobReasons ON tbl_Jobsteps.ReasoncodeID = tbl_JobReasons.nJobReasonID
                            INNER JOIN ( SELECT tblWorkOp.n_WorkOpID ,
                                                tblWorkOp.WorkOpID
                                         FROM   dbo.WorkOperations tblWorkOp
                                       ) tbl_WorkOp ON tbl_Jobsteps.n_WorkOpID = tbl_WorkOp.n_WorkOpID
                            LEFT JOIN ( SELECT  dbo.SubAssemblyNames.nSubAssemblyID ,
                                                dbo.SubAssemblyNames.SubAssemblyName
                                        FROM    dbo.SubAssemblyNames
                                      ) tbl_SubAssembly ON tbl_JobSteps.SubAssemblyID = tbl_SubAssembly.nSubAssemblyID
                            INNER JOIN ( SELECT tblCostCodes.n_costcodeid ,
                                                tblCostCodes.costcodeid ,
                                                tblCostCodes.SupplementalCode
                                         FROM   dbo.CostCodes tblCostCodes
                                       ) tbl_CostCodes ON tbl_JobSteps.n_costcodeid = tbl_CostCodes.n_costcodeid
                            INNER JOIN ( SELECT tblFundSource.n_FundSrcCodeID ,
                                                tblFundSource.FundSrcCodeID
                                         FROM   dbo.FundSrcCodes tblFundSource
                                       ) tbl_FundSource ON tbl_JobSteps.n_FundSrcCodeID = tbl_FundSource.n_FundSrcCodeID
                            INNER JOIN ( SELECT tblWorkOrders.n_WorkOrderCodeID ,
                                                tblWorkOrders.WorkOrderCodeID
                                         FROM   dbo.WorkOrderCodes tblWorkOrders
                                       ) tbl_WorkOrders ON tbl_JobSteps.n_WorkOrderCodeID = tbl_WorkOrders.n_WorkOrderCodeID
                            INNER JOIN ( SELECT tblOrgCode.n_OrganizationCodeID ,
                                                tblOrgCode.OrganizationCodeID
                                         FROM   dbo.OrganizationCodes tblOrgCode
                                       ) tbl_OrgCode ON tbl_JobSteps.n_OrganizationCodeID = tbl_OrgCode.n_OrganizationCodeID
                            INNER JOIN ( SELECT tblFundGroup.n_FundingGroupCodeID ,
                                                tblFundGroup.FundingGroupCodeID
                                         FROM   dbo.FundingGroupCodes tblFundGroup
                                       ) tbl_FundGroup ON tbl_JobSteps.n_FundingGroupCodeID = tbl_FundGroup.n_FundingGroupCodeID
                            INNER JOIN ( SELECT tblEquipNumber.n_EquipmentNumberID ,
                                                tblEquipNumber.EquipmentNumberID
                                         FROM   dbo.EquipmentNumber tblEquipNumber
                                       ) tbl_EquipNumber ON tbl_JobSteps.n_EquipmentNumberID = tbl_EquipNumber.n_EquipmentNumberID
                            INNER JOIN ( SELECT tblControlSection.n_ControlSectionID ,
                                                tblControlSection.ControlSectionID
                                         FROM   dbo.ControlSections tblControlSection
                                       ) tbl_ControlSection ON tbl_JobSteps.n_ControlSectionID = tbl_ControlSection.n_ControlSectionID
                            INNER JOIN ( SELECT tbltatuses.nStatusID ,
                                                tbltatuses.StatusID
                                         FROM   dbo.Statuses tbltatuses
                                       ) tbl_Statuses ON tbl_Jobsteps.n_statusid = tbl_Statuses.nStatusID
                            INNER JOIN ( SELECT tblShifts.n_shiftid ,
                                                tblShifts.ShiftID
                                         FROM   dbo.Shifts tblShifts
                                       ) tbl_Shifts ON tbl_Jobsteps.n_shiftid = tbl_Shifts.n_shiftid
                            OUTER APPLY ( SELECT    CASE COUNT(tblCrew.n_jobcrewid)
                                                      WHEN 0 THEN 'N'
                                                      ELSE 'Y'
                                                    END AS 'CrewExists'
                                          FROM      dbo.Jobcrews tblCrew WITH ( NOLOCK )
                                          WHERE     tblCrew.JobstepID = tbl_Jobsteps.n_jobstepid
                                        ) AS tbl_CrewExists
							OUTER APPLY ( SELECT    CASE COUNT(tblParts.n_jobpartid)
                                                      WHEN 0 THEN 'N'
                                                      ELSE 'Y'
                                                    END AS 'PartsExists'
                                          FROM      dbo.JobParts tblParts WITH ( NOLOCK )
                                          WHERE     tblParts.n_jobstepid = tbl_Jobsteps.n_jobstepid
                                        ) AS tbl_PartsExists
							OUTER APPLY ( SELECT    CASE COUNT(tblEquip.n_JobEquipmentID)
                                                      WHEN 0 THEN 'N'
                                                      ELSE 'Y'
                                                    END AS 'EquipExists'
                                          FROM      dbo.JobEquipment tblEquip WITH ( NOLOCK )
                                          WHERE     tblEquip.n_jobstepid = tbl_Jobsteps.n_jobstepid
                                        ) AS tbl_EquipExists
							OUTER APPLY ( SELECT    CASE COUNT(tblOtehr.n_JobOtherID)
                                                      WHEN 0 THEN 'N'
                                                      ELSE 'Y'
                                                    END AS 'OtherExists'
                                          FROM      dbo.JobOther tblOtehr WITH ( NOLOCK )
                                          WHERE     tblOtehr.n_jobstepid = tbl_Jobsteps.n_jobstepid
                                        ) AS tbl_OtherExists
                 )
        SELECT  cte_Jobsteps.n_jobstepid ,
                cte_Jobsteps.n_jobid ,
                cte_Jobsteps.JobTypeCode AS T ,
                cte_Jobsteps.Jobid ,
                cte_Jobsteps.ObjectID AS [Object ID] ,
                cte_Jobsteps.stepnumber AS step ,
                cte_Jobsteps.JobstepTitle AS [Step Title] ,
                cte_Jobsteps.priorityid AS [Priority] ,
                cte_Jobsteps.StatusID AS [Status] ,
                cte_Jobsteps.RequestDate AS [Requested] ,
                cte_Jobsteps.StartingDate AS [Starting Date] ,
                cte_Jobsteps.laborclassid AS [Labor Class] ,
                cte_Jobsteps.groupid AS [Group] ,
                cte_Jobsteps.outcomecodeid AS [Outcome] ,
                cte_Jobsteps.Username AS [Supervisor] ,
                cte_Jobsteps.IsIssued AS [I] ,
                cte_Jobsteps.JobReasonID AS cReasoncode ,
                cte_Jobsteps.taskid AS AssignedTaskID ,
                cte_Jobsteps.CompletionDate AS [Completion Date] ,
                cte_Jobsteps.WorkOpID ,
                cte_Jobsteps.HighwayRouteID ,
                cte_Jobsteps.MileMarker ,
                cte_Jobsteps.AssetNumber ,
                cte_Jobsteps.ResponsiblePersonID ,
                cte_Jobsteps.n_objectid AS [nMaintObjectID] ,
                cte_Jobsteps.BreakdownJob ,
                cte_Jobsteps.actualdowntime AS DowntimeHrs ,
                cte_Jobsteps.Ontime AS [Ontime] ,
                cte_Jobsteps.return_within AS ReturnWithin ,
                cte_Jobsteps.SubAssemblyName AS SubAssembly ,
                cte_Jobsteps.areaid AS AreaID ,
                cte_Jobsteps.ObjectDescr ,
                cte_Jobsteps.CostCodeID ,
                cte_Jobsteps.FundSrcCodeID ,
                cte_Jobsteps.WorkOrderCodeID ,
                cte_Jobsteps.OrganizationCodeID ,
                cte_Jobsteps.FundingGroupCodeID ,
                cte_Jobsteps.ControlSectionID ,
                cte_Jobsteps.EquipmentNumberID ,
                cte_Jobsteps.MileMarkerTo ,
                cte_Jobsteps.EstimatedUnits ,
                cte_Jobsteps.ActualUnits ,
                cte_Jobsteps.ShiftID ,
                cte_Jobsteps.[CrewExists] AS 'Crew Assigned',
				cte_Jobsteps.[PartsExists] AS 'Parts Assigned',
				cte_Jobsteps.[EquipExists] AS 'Equip. Assigned',
				cte_Jobsteps.[OtherExists] AS 'Other Assigned',
                cte_Jobsteps.RouteToID AS 'Route To ID',
                cte_Jobsteps.X AS 'Latitude',
				cte_Jobsteps.Y AS 'Longitude'
               
        FROM    cte_Jobsteps">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                                                        </SelectParameters>
                                                        
                                                    </asp:SqlDataSource>
                                        </ContentTemplate>

    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="PlannedGrid"></dx:ASPxGridViewExporter>
    <dx:ASPxPopupControl ID="BatchCrewPopup"
        ClientInstanceName="BatchCrewPopup"
        ShowCloseButton="false" 
        ShowHeader="false" 
        HeaderText=""
        CloseAnimationType="Fade" 
        PopupAnimationType="Fade" 
        runat="server" 
        ShowShadow="true" 
        ShowFooter="true"
        CloseAction="None" 
        Modal="True" 
        PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" 
        Width="600px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxLogonLayout" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Crew Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="CrewLookupGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="nUserID" 
                                                                                Width="98%" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="CrewLookupGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="CrewLookupDataSource"
                                                                                SelectionMode="Multiple">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="nUserID" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UserID" ReadOnly="True" Caption="Username" SortOrder="Ascending" Width="100px" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Name" ReadOnly="True" Caption="Full Name"  Width="200px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Area" ReadOnly="True" Caption="Area ID"  Width="100px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Position Code" ReadOnly="True"  Caption="Position"  Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Person Class" ReadOnly="True" Caption="Person Class"  Width="100px" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Location" ReadOnly="True"  Caption="Location"  Width="100px" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="LaborClassID" ReadOnly="True"  Visible="False"  Width="100px" VisibleIndex="7">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="LaborClass" ReadOnly="True" Caption="Laborclass ID"  Width="100px" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="GroupID" ReadOnly="True" Visible="False"  Width="100px" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Group" ReadOnly="True" Caption="Group ID"  Width="100px" VisibleIndex="10">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="CrewLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        ;
                                                            WITH    cte_Personnel
                                                                      AS ( SELECT   tbl_MPETUsers.UserID AS nUserID ,
                                                                                    tbl_MPETUsers.Username AS UserID ,
                                                                                    tbl_MPETUsers.FirstName AS Firstname ,
                                                                                    tbl_MPETUsers.LastName AS LastName ,
                                                                                    LTRIM(RTRIM(LTRIM(RTRIM(tbl_MPETUsers.LastName)) + ', ' + LTRIM(RTRIM(tbl_MPETUsers.FirstName)))) AS [Name] ,
                                                                                    tbl_Areas.areaid AS 'Area' ,
                                                                                    tbl_PositionCodes.positioncodeid AS 'Position Code' ,
                                                                                    tbl_PersonClasses.personclassid AS 'Person Class' ,
                                                                                    tbl_Locations.locationid AS 'Location' ,
                                                                                    tbl_MPETUsers.AreaID ,
                                                                                    tbl_MPETUsers.PositionCodeID ,
                                                                                    tbl_MPETUsers.PersonClassID ,
                                                                                    tbl_MPETUsers.LocationID ,
                                                                                    tbl_MPETUsers.Active ,
                                                                                    tbl_MPETUsers.LaborClassID AS 'laborclassid' ,
                                                                                    tbl_LaborClasses.laborclassid AS 'LaborClass',
                                                                                    tbl_MPETUsers.GroupID ,
                                                                                    tbl_Groups.groupid AS 'Group' 
                                                                           FROM     dbo.MPetUsers tbl_MPETUsers
                                                                                    INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                                        tblAreas.areaid
                                                                                                 FROM   dbo.Areas tblAreas
                                                                                               ) tbl_Areas ON tbl_MPETUsers.AreaID = tbl_Areas.n_areaid
                                                                                    INNER JOIN ( SELECT tblLocations.n_locationid ,
                                                                                                        tblLocations.locationid
                                                                                                 FROM   dbo.locations tblLocations
                                                                                               ) tbl_Locations ON tbl_MPETUsers.LocationID = tbl_Locations.n_locationid
                                                                                    INNER JOIN ( SELECT tblPositionCode.n_positioncodeid ,
                                                                                                        tblPositionCode.positioncodeid
                                                                                                 FROM   dbo.positioncodes tblPositionCode
                                                                                               ) tbl_PositionCodes ON tbl_MPETUsers.PositionCodeID = tbl_PositionCodes.n_positioncodeid
                                                                                    INNER JOIN ( SELECT tblPersonClasses.n_personclassid ,
                                                                                                        tblPersonClasses.personclassid
                                                                                                 FROM   dbo.personclasses tblPersonClasses
                                                                                               ) tbl_PersonClasses ON tbl_MPETUsers.PersonClassID = tbl_PersonClasses.n_personclassid
                                                                                    INNER JOIN ( SELECT tblLaborClasses.n_laborclassid ,
                                                                                                        tblLaborClasses.laborclassid
                                                                                                 FROM   dbo.laborclasses tblLaborClasses
                                                                                               ) tbl_LaborClasses ON tbl_MPETUsers.LaborClassID = tbl_LaborClasses.n_laborclassid
                                                                                    INNER JOIN ( SELECT tblGroups.n_groupid ,
                                                                                                        tblGroups.groupid
                                                                                                 FROM   dbo.groups tblGroups
                                                                                               ) tbl_Groups ON tbl_MPETUsers.GroupID = tbl_Groups.n_groupid
                                                                           WHERE    ( tbl_MPETUsers.UserID > 0 AND tbl_MPETUsers.Active=1)
							
                                                                         )
                                                                --Return Filtered Data
                                                            SELECT  cte_Personnel.nUserID ,
                                                                    cte_Personnel.UserID ,
                                                                    cte_Personnel.Name ,
                                                                    cte_Personnel.Area ,
                                                                    cte_Personnel.[Position Code] ,
                                                                    cte_Personnel.[Person Class] ,
                                                                    cte_Personnel.Location ,
                                                                    cte_Personnel.LaborClassID ,
                                                                    cte_Personnel.LaborClass ,
                                                                    cte_Personnel.GroupID ,
                                                                    cte_Personnel.[Group] 
                                                            FROM    cte_Personnel
                                                          ">
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="LogonButton" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddCrew_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="OkButton" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                                                                                    <ClientSideEvents Click="HidePopup" />
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                            </Items>
                                                        </dx:LayoutGroup>

                                                    </Items>
                                                </dx:ASPxFormLayout>
                                            </div>
                                        </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
        </FooterTemplate>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="BatchSuperPopup"
        ClientInstanceName="BatchSuperPopup"
        ShowCloseButton="false" 
        ShowHeader="false" 
        HeaderText=""
        CloseAnimationType="Fade" 
        PopupAnimationType="Fade" 
        runat="server" 
        ShowShadow="true" 
        ShowFooter="true"
        CloseAction="None" 
        Modal="True" 
        PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" 
        Width="600px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Supervisor Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="SupervisorGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="nUserID" 
                                                                                Width="98%" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="SupervisorGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="SupervisorDS"
                                                                                SelectionMode="Single">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="nUserID" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UserID" ReadOnly="True" Caption="Username" SortOrder="Ascending" Width="100px" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Name" ReadOnly="True" Caption="Full Name"  Width="200px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Area" ReadOnly="True" Caption="Area ID"  Width="100px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Position Code" ReadOnly="True"  Caption="Position"  Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Person Class" ReadOnly="True" Caption="Person Class"  Width="100px" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Location" ReadOnly="True"  Caption="Location"  Width="100px" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="LaborClassID" ReadOnly="True"  Visible="False"  Width="100px" VisibleIndex="7">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="LaborClass" ReadOnly="True" Caption="Laborclass ID"  Width="100px" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="GroupID" ReadOnly="True" Visible="False"  Width="100px" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Group" ReadOnly="True" Caption="Group ID"  Width="100px" VisibleIndex="10">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="SupervisorDS" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        ;
                                                            WITH    cte_Personnel
                                                                      AS ( SELECT   tbl_MPETUsers.UserID AS nUserID ,
                                                                                    tbl_MPETUsers.Username AS UserID ,
                                                                                    tbl_MPETUsers.FirstName AS Firstname ,
                                                                                    tbl_MPETUsers.LastName AS LastName ,
                                                                                    LTRIM(RTRIM(LTRIM(RTRIM(tbl_MPETUsers.LastName)) + ', ' + LTRIM(RTRIM(tbl_MPETUsers.FirstName)))) AS [Name] ,
                                                                                    tbl_Areas.areaid AS 'Area' ,
                                                                                    tbl_PositionCodes.positioncodeid AS 'Position Code' ,
                                                                                    tbl_PersonClasses.personclassid AS 'Person Class' ,
                                                                                    tbl_Locations.locationid AS 'Location' ,
                                                                                    tbl_MPETUsers.AreaID ,
                                                                                    tbl_MPETUsers.PositionCodeID ,
                                                                                    tbl_MPETUsers.PersonClassID ,
                                                                                    tbl_MPETUsers.LocationID ,
                                                                                    tbl_MPETUsers.Active ,
                                                                                    tbl_MPETUsers.LaborClassID AS 'laborclassid' ,
                                                                                    tbl_LaborClasses.laborclassid AS 'LaborClass',
                                                                                    tbl_MPETUsers.GroupID ,
                                                                                    tbl_Groups.groupid AS 'Group' 
                                                                           FROM     dbo.MPetUsers tbl_MPETUsers
                                                                                    INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                                        tblAreas.areaid
                                                                                                 FROM   dbo.Areas tblAreas
                                                                                               ) tbl_Areas ON tbl_MPETUsers.AreaID = tbl_Areas.n_areaid
                                                                                    INNER JOIN ( SELECT tblLocations.n_locationid ,
                                                                                                        tblLocations.locationid
                                                                                                 FROM   dbo.locations tblLocations
                                                                                               ) tbl_Locations ON tbl_MPETUsers.LocationID = tbl_Locations.n_locationid
                                                                                    INNER JOIN ( SELECT tblPositionCode.n_positioncodeid ,
                                                                                                        tblPositionCode.positioncodeid
                                                                                                 FROM   dbo.positioncodes tblPositionCode
                                                                                               ) tbl_PositionCodes ON tbl_MPETUsers.PositionCodeID = tbl_PositionCodes.n_positioncodeid
                                                                                    INNER JOIN ( SELECT tblPersonClasses.n_personclassid ,
                                                                                                        tblPersonClasses.personclassid
                                                                                                 FROM   dbo.personclasses tblPersonClasses
                                                                                               ) tbl_PersonClasses ON tbl_MPETUsers.PersonClassID = tbl_PersonClasses.n_personclassid
                                                                                    INNER JOIN ( SELECT tblLaborClasses.n_laborclassid ,
                                                                                                        tblLaborClasses.laborclassid
                                                                                                 FROM   dbo.laborclasses tblLaborClasses
                                                                                               ) tbl_LaborClasses ON tbl_MPETUsers.LaborClassID = tbl_LaborClasses.n_laborclassid
                                                                                    INNER JOIN ( SELECT tblGroups.n_groupid ,
                                                                                                        tblGroups.groupid
                                                                                                 FROM   dbo.groups tblGroups
                                                                                               ) tbl_Groups ON tbl_MPETUsers.GroupID = tbl_Groups.n_groupid
                                                                           WHERE    ( tbl_MPETUsers.UserID > 0 AND tbl_MPETUsers.Active=1)
							
                                                                         )
                                                                --Return Filtered Data
                                                            SELECT  cte_Personnel.nUserID ,
                                                                    cte_Personnel.UserID ,
                                                                    cte_Personnel.Name ,
                                                                    cte_Personnel.Area ,
                                                                    cte_Personnel.[Position Code] ,
                                                                    cte_Personnel.[Person Class] ,
                                                                    cte_Personnel.Location ,
                                                                    cte_Personnel.LaborClassID ,
                                                                    cte_Personnel.LaborClass ,
                                                                    cte_Personnel.GroupID ,
                                                                    cte_Personnel.[Group] 
                                                            FROM    cte_Personnel
                                                          ">
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton1" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddSupervisor_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton2" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                                                                                    <ClientSideEvents Click="HidePopup" />
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                            </Items>
                                                        </dx:LayoutGroup>

                                                    </Items>
                                                </dx:ASPxFormLayout>
                                            </div>
                                        </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
        </FooterTemplate>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="BatchEquipPopup"
        ClientInstanceName="BatchEquipPopup"
        ShowCloseButton="false" 
        ShowHeader="false" 
        HeaderText=""
        CloseAnimationType="Fade" 
        PopupAnimationType="Fade" 
        runat="server" 
        ShowShadow="true" 
        ShowFooter="true"
        CloseAction="None" 
        Modal="True" 
        PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" 
        Width="600px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout4" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Equipment Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="EquipmentGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_objectid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="EquipmentGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="BatchEquipDS"
                                                                                SelectionMode="Single">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_objectid" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="objectid" ReadOnly="True" Caption="Object/Asset ID" ToolTip="M-PET.NET Object/Asset ID" SortOrder="Ascending" Width="100px" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" ReadOnly="True" Caption="Description" ToolTip="M-PET.NET Object/Asset Description" Width="200px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Area" ReadOnly="True" Caption="Area ID" ToolTip="M-PET.NET Object/Asset Assigned Area ID" Width="100px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectType" ReadOnly="True" Caption="Obj. Type ID" ToolTip="M-PET.NET Object/Asset Assigned Object Type ID" Width="100px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectClass" ReadOnly="True" Caption="Obj. Class ID" ToolTip="M-PET.NET Object/Asset Assigned Object Class ID" Width="100px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ChargeRate" ReadOnly="True" Caption="Rate" ToolTip="M-PET.NET Object/Asset Charge Rate" Width="100px" VisibleIndex="3">
                                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="BatchEquipDS" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        DECLARE @NullDate DATETIME
                                                                            SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
                            
                                                                        --Create Area Filering On Variable
                                                                            DECLARE @areaFilteringOn VARCHAR(1)
                            
                                                                        --Setup Area Filering Variable
                                                                            IF ( ( SELECT   COUNT(dbo.UsersAreaFilter.RecordID)
                                                                                   FROM     dbo.UsersAreaFilter WITH ( NOLOCK )
                                                                                   WHERE    UsersAreaFilter.UserID = @UserID
                                                                                            AND UsersAreaFilter.FilterActive = 'Y'
                                                                                 ) <> 0 )
                                                                                BEGIN
                                                                                    SET @areaFilteringOn = 'Y'
                                                                                END
                                                                            ELSE
                                                                                BEGIN
                                                                                    SET @areaFilteringOn = 'N'
                                                                                END
                            
                                                                        ;
                                                                            WITH    cte_MaintenanceObjects
                                                          AS ( SELECT   tbl_MaintObj.n_objectid ,
                                                                        tbl_MaintObj.objectid ,
                                                                        tbl_MaintObj.description ,
                                                                        tbl_Areas.areaid ,
                                                                        tbl_ObjectTypes.objtypeid ,
                                                                        tbl_ObjectClasses.objclassid ,
                                                                        tbl_MaintObj.charge_rate 
                                                               FROM     dbo.MaintenanceObjects tbl_MaintObj
                                                                        INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                            tblAreas.areaid
                                                                                     FROM   dbo.Areas tblAreas
                                                                                     WHERE  ( ( @areaFilteringOn = 'Y'
                                                                                                    AND EXISTS ( SELECT recordMatches.AreaFilterID
                                                                                                                 FROM   dbo.UsersAreaFilter AS recordMatches
                                                                                                                 WHERE  tblAreas.n_areaid = recordMatches.AreaFilterID
                                                                                                                        AND recordMatches.UserID = @UserID
                                                                                                                        AND recordMatches.FilterActive = 'Y' )
                                                                                                  )
                                                                                                  OR ( @areaFilteringOn = 'N' )
                                                                                                )
                                                                                   ) tbl_Areas ON tbl_MaintObj.n_areaid = tbl_Areas.n_areaid
                                                                        INNER JOIN ( SELECT tblObjectTypes.n_objtypeid ,
                                                                                            tblObjectTypes.objtypeid
                                                                                     FROM   dbo.objecttypes tblObjectTypes
                                                                                   ) tbl_ObjectTypes ON tbl_MaintObj.n_objtypeid = tbl_ObjectTypes.n_objtypeid
                                                                        INNER JOIN ( SELECT tblObjectClasses.n_objclassid ,
                                                                                            tblObjectClasses.objclassid
                                                                                     FROM   dbo.objectclasses tblObjectClasses
                                                                                   ) tbl_ObjectClasses ON tbl_MaintObj.n_objclassid = tbl_ObjectClasses.n_objclassid
                                                               WHERE    tbl_MaintObj.n_objectid > 0
						                                                AND tbl_MaintObj.b_chargeable = 'Y'
							                                            AND tbl_MaintObj.b_active = 'Y'
                                                             )
                                                    SELECT  cte_MaintenanceObjects.n_objectid AS 'n_objectid' ,
                                                            cte_MaintenanceObjects.objectid AS 'objectid' ,
                                                            cte_MaintenanceObjects.description AS 'description' ,
                                                            cte_MaintenanceObjects.areaid AS 'Area' ,
                                                            cte_MaintenanceObjects.objtypeid AS 'ObjectType' ,
                                                            cte_MaintenanceObjects.objclassid AS 'ObjectClass' ,
                                                            cte_MaintenanceObjects.charge_rate AS 'ChargeRate' 
                                                    FROM    cte_MaintenanceObjects
                                                          ">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" Type="Int32" />
                                                                                </SelectParameters>                                                                                                                                        
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton7" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddEquipment_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton8" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                                                                                    <ClientSideEvents Click="HidePopup" />
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                            </Items>
                                                        </dx:LayoutGroup>

                                                    </Items>
                                                </dx:ASPxFormLayout>
                                            </div>
                                        </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
        </FooterTemplate>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="BatchPartPopup"
        ClientInstanceName="BatchPartPopup"
        ShowCloseButton="false" 
        ShowHeader="false" 
        HeaderText=""
        CloseAnimationType="Fade" 
        PopupAnimationType="Fade" 
        runat="server" 
        ShowShadow="true" 
        ShowFooter="true"
        CloseAction="None" 
        Modal="True" 
        PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" 
        Width="600px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout5" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Part Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="PartGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_masterpartid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="PartGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="BatchPartDS"
                                                                                SelectionMode="Single">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_masterpartid" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="masterpartid" Caption="Masterpart" SortOrder="Ascending" ToolTip="M-PET.NET Masterpart ID" Width="100px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Description" Caption="Description" ToolTip="M-PET.NET Masterpart Description" Width="200px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="listcost" Caption="List $" ToolTip="M-PET.NET Masterpart List Cost" Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="lastcost" Caption="Last $" ToolTip="M-PET.NET Masterpart Last Cost" Width="100px" ReadOnly="True" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="avgcost" Caption="Avg. $" ToolTip="M-PET.NET Masterpart Average Cost" Width="100px" ReadOnly="True" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_buyerid" ReadOnly="True" Visible="false" VisibleIndex="7">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_parttypeid" ReadOnly="True" Visible="false" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_prefmfgid" ReadOnly="True" Visible="false" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_prefvndid" ReadOnly="True" Visible="false" VisibleIndex="10">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_PrefMFGPartID" ReadOnly="True" Visible="false" VisibleIndex="11">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_PrefVndPartID" ReadOnly="True" Visible="false" VisibleIndex="12">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                                                                                    <dx:GridViewDataTextColumn FieldName="SpecialHandlingNotes" Caption="Notes" ToolTip="M-PET.NET Masterpart Special Handling Notes" Width="200px" VisibleIndex="13">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="BuyerComment" Caption="Comments" ToolTip="M-PET.NET Masterpart Buyers Comments" Width="200px" VisibleIndex="14">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="b_tool" Caption="Tool?" ToolTip="M-PET.NET Masterpart Tool Status Yes/No?" Width="75px" ReadOnly="True" VisibleIndex="15">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="b_SpecHandling" Caption="Spec. Hndl?" ToolTip="M-PET.NET Masterpart Special Handling Status Yes/No?" Width="75px" ReadOnly="True" VisibleIndex="16">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="chkoutunits" ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="MRPRuleType"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="YieldPct"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ShelfLife"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="CountCycle"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="cPrefMfg" Caption="Pref. Mfg."  ToolTip="M-PET.NET Masterpart Preferred Manufacturer ID" Width="100px" VisibleIndex="17">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="PrefMFGPartID" Caption="Pref. Mfg. Part"  ToolTip="M-PET.NET Masterpart Preferred Manufacturer Part ID" Width="100px" VisibleIndex="18">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="cVendor" Caption="Pref. Vnd."  ToolTip="M-PET.NET Masterpart Preferred Vendor ID" Width="100px" VisibleIndex="19">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="PrefVndPartID" Caption="Pref. Vnd. Part"  ToolTip="M-PET.NET Masterpart Preferred Vendor Part ID" Width="100px" VisibleIndex="20">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UOI" Caption="UOI"  ToolTip="M-PET.NET Masterpart Units Of Issue" Width="100px" VisibleIndex="21">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UOM" Caption="UOM"  ToolTip="M-PET.NET Masterpart Units Of Measure" Width="100px" VisibleIndex="22">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectCodeID" Caption="Object Code"  ToolTip="M-PET.NET Masterpart Object Code ID" Width="100px" VisibleIndex="23">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="FlaggedRecordID" ReadOnly="True" Visible="false" VisibleIndex="12">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="BatchPartDS" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                            ;WITH    cte_Masterparts
                                                                                                  AS ( SELECT   tbl_Masterparts.n_masterpartid ,
                                                                                                                tbl_Masterparts.masterpartid ,
                                                                                                                tbl_Masterparts.Description ,
                                                                                                                tbl_Masterparts.listcost ,
                                                                                                                tbl_Masterparts.lastcost ,
                                                                                                                tbl_Masterparts.avgcost ,
                                                                                                                tbl_Masterparts.n_buyerid ,
                                                                                                                tbl_Masterparts.n_parttypeid ,
                                                                                                                tbl_Masterparts.n_prefmfgid ,
                                                                                                                tbl_Masterparts.n_prefvndid ,
                                                                                                                tbl_Masterparts.n_PrefMFGPartID ,
                                                                                                                tbl_Masterparts.n_PrefVndPartID ,
                                                                                                                tbl_Masterparts.SpecialHandlingNotes ,
                                                                                                                tbl_Masterparts.BuyerComment ,
                                                                                                                tbl_Masterparts.b_tool ,
                                                                                                                tbl_Masterparts.b_SpecHandling ,
                                                                                                                tbl_Masterparts.chkoutunits ,
                                                                                                                tbl_Masterparts.MRPRuleType ,
                                                                                                                tbl_Masterparts.YieldPct ,
                                                                                                                tbl_Masterparts.ShelfLife ,
                                                                                                                tbl_Masterparts.CountCycle ,
                                                                                                                tbl_Manufacturer.mfgid ,
                                                                                                                tbl_Vendors.vendorid ,
                                                                                                                tbl_PartTypes.parttypeid ,
                                                                                                                tbl_Masterparts.b_active ,
                                                                                                                tbl_Masterparts.UnitOfIssue ,
                                                                                                                tbl_Masterparts.UnitOfMeasure ,
                                                                                                                tbl_MfgParts.mfgpartid ,
                                                                                                                tbl_VendorParts.VendorPartID ,
                                                                                                                tbl_ObjectCodes.n_ObjectCodeID ,
                                                                                                                tbl_ObjectCodes.ObjectCodeID ,
                                                                                                                ISNULL(tbl_IsFlaggedRecord.RecordID, -1) AS FlaggedRecordID
                                                                                                       FROM     dbo.Masterparts tbl_Masterparts
                                                                                                                INNER JOIN ( SELECT tbl_Mfg.n_mfgid ,
                                                                                                                                    tbl_Mfg.mfgid
                                                                                                                             FROM   dbo.Manufacturers tbl_Mfg
                                                                                                                           ) tbl_Manufacturer ON tbl_Masterparts.n_prefmfgid = tbl_Manufacturer.n_mfgid
                                                                                                                INNER JOIN ( SELECT tbl_Vendors.n_vendorid ,
                                                                                                                                    tbl_Vendors.vendorid
                                                                                                                             FROM   dbo.Vendors tbl_Vendors
                                                                                                                           ) tbl_Vendors ON tbl_Masterparts.n_prefvndid = tbl_Vendors.n_vendorid
                                                                                                                INNER JOIN ( SELECT tbl_PartTypes.n_parttypeid ,
                                                                                                                                    tbl_PartTypes.parttypeid
                                                                                                                             FROM   dbo.PartTypes tbl_PartTypes
                                                                                                                           ) tbl_PartTypes ON tbl_Masterparts.n_parttypeid = tbl_PartTypes.n_parttypeid
                                                                                                                INNER JOIN ( SELECT tbl_VendorParts.n_VendorPartID ,
                                                                                                                                    tbl_VendorParts.VendorPartID
                                                                                                                             FROM   dbo.VendorParts tbl_VendorParts
                                                                                                                           ) tbl_VendorParts ON tbl_Masterparts.n_PrefVndPartID = tbl_VendorParts.n_VendorPartID
                                                                                                                INNER JOIN ( SELECT tbl_MfgParts.n_mfgpartid ,
                                                                                                                                    tbl_MfgParts.mfgpartid
                                                                                                                             FROM   dbo.MfgParts tbl_MfgParts
                                                                                                                           ) tbl_MfgParts ON tbl_Masterparts.n_PrefMFGPartID = tbl_MfgParts.n_mfgpartid
                                                                                                                INNER JOIN ( SELECT tbl_ObjectCodes.n_ObjectCodeID ,
                                                                                                                                    tbl_ObjectCodes.ObjectCodeID
                                                                                                                             FROM   dbo.ObjectCodes tbl_ObjectCodes
                                                                                                                           ) tbl_ObjectCodes ON tbl_Masterparts.n_ObjectCodeID = tbl_ObjectCodes.n_ObjectCodeID
                                                                                                                LEFT JOIN ( SELECT  dbo.UsersFlaggedRecords.RecordID ,
                                                                                                                                    dbo.UsersFlaggedRecords.n_masterpartid
                                                                                                                            FROM    dbo.UsersFlaggedRecords
                                                                                                                            WHERE   dbo.UsersFlaggedRecords.UserID = @UserID
                                                                                                                                    AND dbo.UsersFlaggedRecords.n_masterpartid > 0
                                                                                                                          ) tbl_IsFlaggedRecord ON tbl_Masterparts.n_masterpartid = tbl_IsFlaggedRecord.n_masterpartid
                                                                                                       WHERE    ( tbl_Masterparts.b_active = 'Y'
                                                                                                                  AND tbl_Masterparts.n_masterpartid > 0
                                                                                                                )
                                                                                                     )
                                                                                            --Return Data
                                                                                            SELECT  cte_Masterparts.n_masterpartid ,
                                                                                                    cte_Masterparts.masterpartid ,
                                                                                                    cte_Masterparts.Description ,
                                                                                                    cte_Masterparts.listcost ,
                                                                                                    cte_Masterparts.lastcost ,
                                                                                                    cte_Masterparts.avgcost ,
                                                                                                    cte_Masterparts.n_buyerid ,
                                                                                                    cte_Masterparts.n_parttypeid ,
                                                                                                    cte_Masterparts.n_prefmfgid ,
                                                                                                    cte_Masterparts.n_prefvndid ,
                                                                                                    cte_Masterparts.n_PrefMFGPartID ,
                                                                                                    cte_Masterparts.n_PrefVndPartID ,
                                                                                                    cte_Masterparts.SpecialHandlingNotes ,
                                                                                                    cte_Masterparts.BuyerComment ,
                                                                                                    cte_Masterparts.b_tool ,
                                                                                                    cte_Masterparts.b_SpecHandling ,
                                                                                                    cte_Masterparts.chkoutunits ,
                                                                                                    cte_Masterparts.MRPRuleType ,
                                                                                                    cte_Masterparts.UnitOfIssue ,
                                                                                                    cte_Masterparts.UnitOfMeasure ,
                                                                                                    cte_Masterparts.YieldPct ,
                                                                                                    cte_Masterparts.ShelfLife ,
                                                                                                    cte_Masterparts.CountCycle ,
                                                                                                    cte_Masterparts.mfgid AS cPrefMfg ,
                                                                                                    cte_Masterparts.vendorid AS cVendor ,
                                                                                                    cte_Masterparts.parttypeid AS cPartType ,
                                                                                                    cte_Masterparts.UnitOfIssue AS UOI ,
                                                                                                    cte_Masterparts.UnitOfMeasure AS UOM ,
                                                                                                    cte_Masterparts.mfgpartid AS PrefMFGPartID ,
                                                                                                    cte_Masterparts.VendorPartID AS PrefVndPartID ,
                                                                                                    cte_Masterparts.ObjectCodeID ,
                                                                                                    cte_Masterparts.FlaggedRecordID
                                                                                            FROM    cte_Masterparts">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" Type="Int32" />
                                                                                </SelectParameters>                                                                                                                                        
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton9" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddPart_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton10" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                                                                                    <ClientSideEvents Click="HidePopup" />
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                            </Items>
                                                        </dx:LayoutGroup>

                                                    </Items>
                                                </dx:ASPxFormLayout>
                                            </div>
                                        </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
        </FooterTemplate>
    </dx:ASPxPopupControl>
     <dx:ASPxPopupControl ID="BatchPostPopup" ClientInstanceName="BatchPostPopup" ShowCloseButton="false" HeaderText=""
        CloseAnimationType="Fade" PopupAnimationType="Fade" runat="server" ShowShadow="true" ShowFooter="true"
        CssClass="popup" CloseAction="None" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <div class="popup-text">
                    <dx:ASPxFormLayout ID="flError" runat="server" Font-Size="Medium">
                        <Items>
                            <dx:LayoutGroup Caption="Batch Post">
                                <Items>
                                    <dx:LayoutItem Caption="Completion Date:">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxDateEdit ID="txtPostDate"
                                                    ClientInstanceName="txtPostDate"
                                                    Theme="Mulberry"
                                                    Width="300px"
                                                    runat="server">
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Outcome Code:" HorizontalAlign="Right">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="ComboOutcomeCode"
                                                    runat="server"
                                                    EnableCallbackMode="true"
                                                    CallbackPageSize="10"
                                                    ValueType="System.String"
                                                    ValueField="n_outcomecodeid"
                                                    OnItemsRequestedByFilterCondition="ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ComboOutcomeCode_OnItemRequestedByValue_SQL"
                                                    TextFormatString="{0} - {1}"
                                                    Width="300px"
                                                    DropDownStyle="DropDown"
                                                    Theme="Mulberry"
                                                    TextField="outcomecodeid"
                                                    DropDownButton-Enabled="True"
                                                    AutoPostBack="False"
                                                    ClientInstanceName="ComboOutcomeCode">
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="n_outcomecodeid" Visible="False" />
                                                        <dx:ListBoxColumn FieldName="outcomecodeid" Caption="Outcome ID" Width="75px" ToolTip="M-PET.NET Outcome Code ID" />
                                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Outcome Code Description" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Name="PostDefaults" Caption="Post Defaults:" HorizontalAlign="Center" ShowCaption="False">
                                            <LayoutItemNestedControlCollection >
                                                <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxCheckBox ID="chkPostDefaults" runat="server" Text="Post Defaults"
                                                            ValueField="ID" TextField="Post Defaults" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Horizontal">
                                                        </dx:ASPxCheckBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
            <div class="popup-buttons-centered">
                <dx:ASPxButton ID="ASPxButton1" AutoPostBack="True" runat="server" CssClass="button" Text="Post" OnClick="BatchPostSelection">
                    <HoverStyle CssClass="hover"></HoverStyle>
                </dx:ASPxButton>
                <dx:ASPxButton ID="ASPxButton2" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                    <ClientSideEvents Click="HidePopup" />
                    <HoverStyle CssClass="hover"></HoverStyle>
                </dx:ASPxButton>
            </div>
        </FooterTemplate>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="RoutineJobPopup"
        ClientInstanceName="RoutineJobPopup"
        ShowCloseButton="false" 
        ShowHeader="false" 
        HeaderText=""
        CloseAnimationType="Fade" 
        PopupAnimationType="Fade" 
        runat="server" 
        ShowShadow="true" 
        ShowFooter="true"
        CloseAction="None" 
        Modal="True" 
        PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" 
        Width="600px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Routine Task Selection" Width="600px">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="RoutineTaskGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_taskid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="RoutineTaskGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="RoutineJobDS"
                                                                                SelectionMode="Single">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                        <dx:GridViewDataTextColumn FieldName="n_taskid" ReadOnly="True" Visible="false" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="taskid"
                                                                                        Caption="Task ID"
                                                                                        Width="150px"
                                                                                        VisibleIndex="3"
                                                                                        HeaderStyle-Font-Bold="True"
                                                                                        SortOrder="Ascending">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="200px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="estdowntime" Caption="Est. Downtime" Width="100px" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="estlength" Caption="Est. Length" Width="100px" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="IsStandardJob" Caption="Routine" Width="100px" VisibleIndex="7">
                                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="nAssignedArea" ReadOnly="True" Visible="false" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="AssignedAreaID" Caption="Area ID" Width="150px" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="IsActive" Caption="Active" Width="75px" ReadOnly="True" VisibleIndex="1">
                                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="RoutineJobDS" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" 
                                                                                SelectCommand="--Create/Set Null Date
                                                                                DECLARE @NullDate DATETIME
                                                                                SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
                            
                                                                            --Create Area Filering On Variable
                                                                                DECLARE @areaFilteringOn VARCHAR(1)
                            
                                                                            --Setup Area Filering Variable
                                                                                IF ( ( SELECT   COUNT(dbo.UsersAreaFilter.RecordID)
                                                                                       FROM     dbo.UsersAreaFilter WITH ( NOLOCK )
                                                                                       WHERE    UsersAreaFilter.UserID = @UserID
                                                                                                AND UsersAreaFilter.FilterActive = 'Y'
                                                                                     ) <> 0 )
                                                                                    BEGIN
                                                                                        SET @areaFilteringOn = 'Y'
                                                                                    END
                                                                                ELSE
                                                                                    BEGIN
                                                                                        SET @areaFilteringOn = 'N'
                                                                                    END
                            
                                                                            ;
                                                                                WITH    cte_Tasks
                                                                                          AS ( SELECT   tbl_Tasks.n_taskid ,
                                                                                                        tbl_Tasks.taskid ,
                                                                                                        tbl_Tasks.description ,
                                                                                                        ISNULL(tbl_Tasks.estlength, 0) AS 'estlength' ,
                                                                                                        ISNULL(tbl_Tasks.estdowntime, 0) AS 'estdowntime' ,
                                                                                                        CASE tbl_Tasks.IsStandardJob
                                                                                                          WHEN 'Y' THEN 'Y'
                                                                                                          WHEN 'N' THEN 'N'
                                                                                                          ELSE 'B'
                                                                                                        END AS TskType ,
                                                                                                        tbl_Tasks.IsStandardJob,
                                                                                                        tbl_Tasks.nAssignedArea ,
                                                                                                        tbl_Areas.areaid AS 'AssignedAreaID' ,
                                                                                                        tbl_Tasks.b_IsActive AS 'IsActive'
                                                                                               FROM     dbo.Tasks tbl_Tasks
                                                                                                        INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                                                            tblAreas.areaid
                                                                                                                     FROM   dbo.Areas tblAreas
                                                                                                                     WHERE  ( (  ( ( @areaFilteringOn = 'Y'
                                                                                                                                     AND EXISTS ( SELECT    recordMatches.AreaFilterID
                                                                                                                                                  FROM      dbo.UsersAreaFilter AS recordMatches
                                                                                                                                                  WHERE     tblAreas.n_areaid = recordMatches.AreaFilterID
                                                                                                                                                            AND recordMatches.UserID = @UserID
                                                                                                                                                            AND recordMatches.FilterActive = 'Y' )
                                                                                                                                   )
                                                                                                                              OR ( @areaFilteringOn = 'N' )
                                                                                                                                    )
                                                                                                                              )
                                                                                                                            )
                                                                                                                   ) tbl_Areas ON tbl_Tasks.nAssignedArea = tbl_Areas.n_areaid
                                                                                               WHERE    ( tbl_Tasks.n_taskid > 0 ) AND ( tbl_Tasks.IsStandardJob = 'Y' )
                                                                                             )
                                                --Return Data
                                                    SELECT  cte_Tasks.n_taskid ,
                                                            cte_Tasks.taskid ,
                                                            cte_Tasks.description ,
                                                            cte_Tasks.estdowntime ,
                                                            cte_Tasks.estlength ,
                                                            cte_Tasks.IsStandardJob ,
                                                            cte_Tasks.nAssignedArea ,
                                                            cte_Tasks.AssignedAreaID ,
                                                            cte_Tasks.IsActive
                                                    FROM    cte_Tasks">
                                                                                                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" />
                                                        </SelectParameters>
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton3" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="ProcessRoutineJob" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton4" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                                                                                    <ClientSideEvents Click="HidePopup" />
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                            </Items>
                                                        </dx:LayoutGroup>

                                                    </Items>
                                                </dx:ASPxFormLayout>
                                            </div>
                                        </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
        </FooterTemplate>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="ForcePMPopup"
        ClientInstanceName="ForcePMPopup"
        ShowCloseButton="false" 
        ShowHeader="false" 
        HeaderText=""
        CloseAnimationType="Fade" 
        PopupAnimationType="Fade" 
        runat="server" 
        ShowShadow="true" 
        ShowFooter="true"
        CloseAction="None" 
        Modal="True" 
        PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" 
        Width="600px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Object Task Selection" Width="600px">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="ForcePMGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_objtaskid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="ForcePMGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="ForcePMDS"
                                                                                SelectionMode="Single">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_objtaskid" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="TASKID"
                                                                                        Caption="Object / Task ID"
                                                                                        Width="250px"
                                                                                        VisibleIndex="2"
                                                                                        HeaderStyle-Font-Bold="True"
                                                                                        SortOrder="Ascending">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="300px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="D" Caption="Disabled" Width="75px" VisibleIndex="5">
                                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="S" Caption="Seasonal" Width="100px" VisibleIndex="6">
                                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="AreaID" Caption="Area ID" Width="100px" VisibleIndex="7">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="PriLaborID" Caption="Laborclass ID" Width="100px" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="GroupID" Caption="Group ID" Width="100px" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="SupervisorID" Caption="Supervisor ID" Width="100px" ReadOnly="True" VisibleIndex="10">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="WorkTypeID" Caption="Work Op" Width="100px" ReadOnly="True" VisibleIndex="11">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ShiftID" Caption="Shift ID" Width="100px" ReadOnly="True" VisibleIndex="12">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Every" Caption="Every" Width="100px" ReadOnly="True" VisibleIndex="13">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Freq" Caption="Frequency" Width="100px" ReadOnly="True" VisibleIndex="14">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="ForcePMDS" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" 
                                                                                SelectCommand="--Create/Set Null Date
                                                                                        DECLARE @NullDate DATETIME
                                                                                        SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
                            
                                                                                    --Create Area Filering On Variable
                                                                                        DECLARE @areaFilteringOn VARCHAR(1)
                            
                                                                                    --Setup Area Filering Variable
                                                                                        IF ( ( SELECT   COUNT(dbo.UsersAreaFilter.RecordID)
                                                                                               FROM     dbo.UsersAreaFilter WITH ( NOLOCK )
                                                                                               WHERE    UsersAreaFilter.UserID = @UserID
                                                                                                        AND UsersAreaFilter.FilterActive = 'Y'
                                                                                             ) <> 0 )
                                                                                            BEGIN
                                                                                                SET @areaFilteringOn = 'Y'
                                                                                            END
                                                                                        ELSE
                                                                                            BEGIN
                                                                                                SET @areaFilteringOn = 'N'
                                                                                            END
                            
                                                                                    ;
                                                                                        WITH    cte_ObjectTasks
                                                                                                  AS ( SELECT   tbl_ObjectTasks.n_objtaskid ,
                                                                                                                tbl_MaintObj.n_objectid ,
                                                                                                                tbl_MaintObj.OBJECTID ,
                                                                                                                tbl_Tasks.n_taskid ,
                                                                                                                tbl_Tasks.TASKID ,
                                                                                                                tbl_Tasks.description ,
                                                                                                                tbl_ObjectTasks.b_disabled AS 'Disabled' ,
                                                                                                                tbl_ObjectTasks.b_seasonal AS 'Seasonal' ,
                                                                                                                tbl_MaintObj.n_areaid ,
                                                                                                                tbl_Areas.areaid ,
                                                                                                                tbl_ObjectTasks.n_laborclassid ,
                                                                                                                tbl_LaborClasses.laborclassid AS 'PriLaborID' ,
                                                                                                                tbl_ObjectTasks.n_groupid ,
                                                                                                                tbl_Groups.groupid ,
                                                                                                                tbl_ObjectTasks.n_supervisorid ,
                                                                                                                tbl_MPETUsers.Username AS 'SupervisorID' ,
                                                                                                                tbl_ObjectTasks.n_worktypeid ,
                                                                                                                tbl_WorkType.WorkOpID AS 'WorkTypeID' ,
                                                                                                                tbl_ObjectTasks.n_shiftid ,
                                                                                                                tbl_Shifts.ShiftID ,
                                                                                                                tbl_ObjectTasks.n_priorityid ,
                                                                                                                tbl_ObjectTasks.n_jobreasonid ,
                                                                                                                tbl_ObjectTasks.every ,
                                                                                                                tbl_ObjectTasks.time_units AS 'Freq'
                                                                                                       FROM     dbo.objecttasks tbl_ObjectTasks
                                                                                                                INNER JOIN ( SELECT tblObjects.n_objectid ,
                                                                                                                                    tblObjects.objectid ,
                                                                                                                                    tblObjects.n_areaid
                                                                                                                             FROM   dbo.MaintenanceObjects tblObjects
                                                                                                                           ) tbl_MaintObj ON tbl_ObjectTasks.n_objectid = tbl_MaintObj.n_objectid
                                                                                                                INNER JOIN ( SELECT tblTasks.n_taskid ,
                                                                                                                                    tblTasks.taskid ,
                                                                                                                                    tblTasks.description
                                                                                                                             FROM   dbo.tasks tblTasks
                                                                                                                           ) tbl_Tasks ON tbl_ObjectTasks.n_taskid = tbl_Tasks.n_taskid
                                                                                                                INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                                                                    tblAreas.areaid
                                                                                                                             FROM   dbo.Areas tblAreas
                                                                                                                             WHERE  ( ( @areaFilteringOn = 'Y'
                                                                                                                                        AND EXISTS ( SELECT recordMatches.AreaFilterID
                                                                                                                                                     FROM   dbo.UsersAreaFilter AS recordMatches
                                                                                                                                                     WHERE  tblAreas.n_areaid = recordMatches.AreaFilterID
                                                                                                                                                            AND recordMatches.UserID = @UserID
                                                                                                                                                            AND recordMatches.FilterActive = 'Y' )
                                                                                                                                      )
                                                                                                                                      OR ( @areaFilteringOn = 'N' )
                                                                                                                                    )
                                                                                                                           ) tbl_Areas ON tbl_MaintObj.n_areaid = tbl_Areas.n_areaid
                                                                                                                INNER JOIN ( SELECT tblLaborClasses.n_laborclassid ,
                                                                                                                                    tblLaborClasses.laborclassid
                                                                                                                             FROM   dbo.laborclasses tblLaborClasses
                                                                                                                           ) tbl_LaborClasses ON tbl_ObjectTasks.n_laborclassid = tbl_LaborClasses.n_laborclassid
                                                                                                                INNER JOIN ( SELECT tblSupervisor.UserID ,
                                                                                                                                    tblSupervisor.Username
                                                                                                                             FROM   dbo.MPetUsers tblSupervisor
                                                                                                                           ) tbl_MPETUsers ON tbl_ObjectTasks.n_supervisorid = tbl_MPETUsers.UserID
                                                                                                                INNER JOIN ( SELECT tblGroups.n_groupid ,
                                                                                                                                    tblGroups.groupid
                                                                                                                             FROM   dbo.groups tblGroups
                                                                                                                           ) tbl_Groups ON tbl_ObjectTasks.n_groupid = tbl_Groups.n_groupid
                                                                                                                INNER JOIN ( SELECT tblWorkOp.n_WorkOpID ,
                                                                                                                                    tblWorkOp.WorkOpID
                                                                                                                             FROM   dbo.WorkOperations tblWorkOp
                                                                                                                           ) tbl_WorkType ON tbl_ObjectTasks.n_worktypeid = tbl_WorkType.n_WorkOpID
                                                                                                                INNER JOIN ( SELECT tblShifts.n_shiftid ,
                                                                                                                                    tblShifts.ShiftID
                                                                                                                             FROM   dbo.Shifts tblShifts
                                                                                                                           ) tbl_Shifts ON tbl_ObjectTasks.n_shiftid = tbl_Shifts.n_shiftid
                                                                                                                INNER JOIN ( SELECT tblJobReasons.nJobReasonID ,
                                                                                                                                    tblJobReasons.jobreasonid
                                                                                                                             FROM   dbo.JobReasons tblJobreasons
                                                                                                                           ) tbl_JobReasons ON tbl_ObjectTasks.n_jobreasonid = tbl_JobReasons.nJobReasonID
                                                                                                                INNER JOIN ( SELECT tblPriorities.n_priorityid ,
                                                                                                                                    tblPriorities.priorityid
                                                                                                                             FROM   dbo.Priorities tblPriorities
                                                                                                                           ) tbl_Priorities ON tbl_ObjectTasks.n_priorityid = tbl_Priorities.n_priorityid
                                                                                                       WHERE    tbl_ObjectTasks.n_objtaskid > 0
                                                                                                     )
                                                                                            --Return Filtered Data
                                                            SELECT  cte_ObjectTasks.n_objtaskid ,
                                                                    cte_ObjectTasks.OBJECTID  + ' / ' + cte_ObjectTasks.TASKID AS TASKID,
                                                                    cte_ObjectTasks.description ,
                                                                    cte_ObjectTasks.Disabled AS 'D' ,
                                                                    cte_ObjectTasks.Seasonal AS 'S' ,
                                                                    cte_ObjectTasks.AreaID ,
                                                                    cte_ObjectTasks.PriLaborID ,
                                                                    cte_ObjectTasks.GroupID ,
                                                                    cte_ObjectTasks.SupervisorID ,
                                                                    cte_ObjectTasks.WorkTypeID ,
                                                                    cte_ObjectTasks.ShiftID ,
                                                                    cte_ObjectTasks.Every ,
                                                                    cte_ObjectTasks.Freq
                                                            FROM    cte_ObjectTasks">
                                                                                                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" />
                                                        </SelectParameters>
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton5" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="ProcessForcePmJob" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton6" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                                                                                    <ClientSideEvents Click="HidePopup" />
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                            </Items>
                                                        </dx:LayoutGroup>

                                                    </Items>
                                                </dx:ASPxFormLayout>
                                            </div>
                                        </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
        </FooterTemplate>
    </dx:ASPxPopupControl>
    <asp:SqlDataSource ID="OutcomeCodeSqlDatasource" runat="server" />
</asp:Content>
