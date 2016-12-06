<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JobsHistoryList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.History.JobsHistoryList" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Job History</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>JOB HISTORY</h1>
    <script type="text/javascript">

        function OnGetRowId(idValue) {
            Selection.Set('Jobid', idValue[0].toString());
            Selection.Set('n_jobid', idValue[1].toString());
            Selection.Set('n_jobstepid', idValue[2].toString());
            Selection.Set('step', idValue[3].toString());
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled" ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
            <dx:ASPxGridView
                ID="HistoryGrid"
                runat="server"
                Theme="Mulberry"
                KeyFieldName="n_jobstepid"
                Width="98%"
                KeyboardSupport="True"
                ClientInstanceName="HistoryGrid"
                AutoPostBack="False" 
                SettingsPager-Mode="ShowPager" 
                Settings-HorizontalScrollBarMode="Auto"
                SettingsBehavior-AllowFocusedRow="True"
                DataSourceID="ObjectGridDataSource">
                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"
                    RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow">
                    <Header CssClass="gridViewHeader"></Header>
                    <Row CssClass="gridViewRow"></Row>
                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                </Styles>
                                        <ClientSideEvents FocusedRowChanged="function(s, e) {
                        HistoryGrid.GetRowValues(s.GetFocusedRowIndex(), 'Jobid;n_jobid;n_jobstepid;step', OnGetRowId);
                    }" />
                <Columns>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" Visible="false" VisibleIndex="0" />
                    <dx:GridViewDataTextColumn FieldName="n_jobstepid" ReadOnly="True" Visible="false" VisibleIndex="1">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_jobid" ReadOnly="True" Visible="false" VisibleIndex="4">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="T" Caption="Type" Width="50px" VisibleIndex="7">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Jobid"
                                                Caption="Job ID" 
                                                Width="140px" 
                                                VisibleIndex="7"
                                                HeaderStyle-Font-Bold="True" 
                                                FixedStyle="Left" >
                        <CellStyle Wrap="False"></CellStyle>
                                                        <DataItemTemplate>
						<dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# GetUrl(Container) %>"
                            Text='<%# Eval("Jobid") %>' Width="100%" Theme="Mulberry" >
						</dxe:ASPxHyperLink>
					</DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Object ID" Caption="Object ID" Width="140px" VisibleIndex="2">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="step" Caption="Step #" Width="50px" VisibleIndex="6">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Step Title" Caption="Description" Width="300px" VisibleIndex="5">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Priority" Caption="Priority" Width="100px" VisibleIndex="8">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Status" Caption="Status" Width="100px" VisibleIndex="9">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="Requested"
                        Caption="Requested"
                        Width="120px"
                        ReadOnly="True"
                        VisibleIndex="10"
                        Settings-AllowHeaderFilter="True">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn FieldName="Starting Date"
                        Caption="Starting"
                        Width="120px"
                        SortOrder="Descending"
                        ReadOnly="True"
                        VisibleIndex="11"
                        Settings-AllowHeaderFilter="True">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="Labor Class" Caption="Labor Class" Width="100px" ReadOnly="True" VisibleIndex="12">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="I" Caption="Issued" Width="100px" ReadOnly="True" VisibleIndex="13">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cReasoncode" Caption="Reason" Width="100px" ReadOnly="True" VisibleIndex="14">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AssignedTaskID" Caption="Task" Width="100px" ReadOnly="True" VisibleIndex="15">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="Completion Date"
                        Caption="Completed"
                        Width="120px"
                        ReadOnly="True"
                        VisibleIndex="6"
                        SortOrder="Descending" 
                        Settings-AllowHeaderFilter="True" 
                        FixedStyle="Left">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOpID" Caption="Work Op" Width="100px" ReadOnly="True" VisibleIndex="17">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="HighwayRouteID" Caption="Hwy. Route" Width="100px" ReadOnly="True" VisibleIndex="18">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MileMarker" Caption="MM. From" Width="100px" ReadOnly="True" VisibleIndex="19">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MileMarkerTo" Caption="MM. To" Width="100px" ReadOnly="True" VisibleIndex="37">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AssetNumber" Caption="Asset #" Width="100px" ReadOnly="True" VisibleIndex="20">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ResponsiblePersonID" Caption="Resp. Person" Width="100px" ReadOnly="True" VisibleIndex="21">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="nMaintObjectID" ReadOnly="True" Visible="false" VisibleIndex="22">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="BreakdownJob" Caption="Breakdown" Width="100px" ReadOnly="True" VisibleIndex="23">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DowntimeHrs" Caption="Dt. Hours" Width="100px" ReadOnly="True" VisibleIndex="24">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Ontime" Caption="Ontime" Width="100px" ReadOnly="True" VisibleIndex="24">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ReturnWithin" Caption="Rtn W/in" Width="100px" ReadOnly="True" VisibleIndex="26">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SubAssembly" Caption="Sub. Asmbly" Width="100px" ReadOnly="True" VisibleIndex="27">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AreaID" Caption="Area" Width="100px" ReadOnly="True" VisibleIndex="28">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ObjectDescr" Caption="Obj. Descr." Width="100px" ReadOnly="True" VisibleIndex="29">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CostCodeID" Caption="Cost Code" Width="100px" ReadOnly="True" VisibleIndex="30">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" Caption="Fund. Src." Width="100px" ReadOnly="True" VisibleIndex="31">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" Caption="Work Order" Width="100px" ReadOnly="True" VisibleIndex="32">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" Caption="Org. Code" Width="100px" ReadOnly="True" VisibleIndex="33">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" Caption="Fund. Group" Width="100px" ReadOnly="True" VisibleIndex="34">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ControlSectionID" Caption="Ctl. Section" Width="100px" ReadOnly="True" VisibleIndex="35">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" Caption="Equip. #" Width="100px" ReadOnly="True" VisibleIndex="36">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EstimatedUnits" Caption="Est. Units" Width="100px" ReadOnly="True" VisibleIndex="38">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ActualUnits" Caption="Act. Units" Width="100px" ReadOnly="True" VisibleIndex="39">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ShiftID" Caption="Shift" Width="100px" ReadOnly="True" VisibleIndex="40">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Crew Assigned" Caption="Crew Asgd." Width="100px" ReadOnly="True" VisibleIndex="41">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Parts Assigned" Caption="Parts Asgd." Width="100px" ReadOnly="True" VisibleIndex="42">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Equip. Assigned" Caption="Equip. Asgd." Width="100px" ReadOnly="True" VisibleIndex="43">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Other Assigned" Caption="Other Asgd." Width="100px" ReadOnly="True" VisibleIndex="44">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Route To ID" Caption="Route To" Width="100px" ReadOnly="True" VisibleIndex="45">
                        <CellStyle Wrap="False"></CellStyle>
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
                <SettingsPopup HeaderFilter-Width="360"
                    HeaderFilter-Height="360">
                </SettingsPopup>
                <Settings ShowFilterBar="Visible"
                    VerticalScrollBarMode="Visible"
                    VerticalScrollBarStyle="Virtual"
                    VerticalScrollableHeight="400"
                    ShowGroupPanel="True"
                    ShowFooter="True"
                    ShowGroupFooter="VisibleIfExpanded" />
                <SettingsPager PageSize="20">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="ObjectGridDataSource" 
                       runat="server" 
                               ConnectionString="<%$ ConnectionStrings:connection %>"  
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
							tbl_OtherExists.[OtherExists]
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
                                                tbl_Objects.areaid
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
                                                                    tbl_Areas.areaid
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
										WHERE tblJobs.IsHistory = 'Y'
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
                cte_Jobsteps.RouteToID AS 'Route To ID' 
        FROM    cte_Jobsteps">
        
                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="HistoryGrid"></dx:ASPxGridViewExporter>
</asp:Content>
