<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestsList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.WorkRequests.RequestsList" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Requests</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>REQUESTS</h1>
    <script type="text/javascript">

        function OnGetRowId(idValue) {
            Selection.Set('Jobid', idValue[0].toString());
            Selection.Set('n_Jobid', idValue[1].toString());
            Selection.Set('n_worktypeid', idValue[2].toString());
            Selection.Set('n_priorityid', idValue[3].toString());
            Selection.Set('n_jobreasonid', idValue[4].toString());
            Selection.Set('SubAssemblyID', idValue[5].toString());
            Selection.Set('Title', idValue[6].toString());
            Selection.Set('Notes', idValue[7].toString());
            Selection.Set('n_requestor', idValue[8].toString());
            Selection.Set('Object ID', idValue[9].toString());
            Selection.Set('Latitude', idValue[10].toString());
            Selection.Set('Longitude', idValue[11].toString());
        }

        function HidePopup() {
            RoutineJobPopup.Hide();
            ForcePMPopup.Hide();
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
            <dx:ASPxGridView 
                ID="ReqGrid" 
                runat="server" 
                Theme="Mulberry" 
                KeyFieldName="Jobid" 
                Width="98%" 
                KeyboardSupport="True" 
                ClientInstanceName="ReqGrid" 
                AutoPostBack="True" 
                Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarStyle="Standard" Settings-VerticalScrollBarMode="Visible"
                SettingsPager-Mode="ShowPager" 
                SettingsBehavior-ProcessFocusedRowChangedOnServer="false" 
                SettingsBehavior-AllowFocusedRow="True"
                SelectionMode="Multiple"
                DataSourceID="RequestDataSource">
                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                    <Header CssClass="gridViewHeader"></Header>
                    <Row CssClass="gridViewRow"></Row>
                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                </Styles>
                <ClientSideEvents RowClick="function(s, e) {
                        ReqGrid.GetRowValues(e.visibleIndex, 'Jobid;n_Jobid;n_worktypeid;n_priorityid;n_jobreasonid;SubAssemblyID;Title;Notes;n_requestor;Object ID;Latitude;Longitude', OnGetRowId);
                    }" />
                <Columns>
                    <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" Visible="false" VisibleIndex="0" />
                    <dx:GridViewDataTextColumn FieldName="n_Jobid" ReadOnly="True" Visible="false" VisibleIndex="1">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn FixedStyle="Left" 
                                                    FieldName="Jobid" 
                                                    HeaderStyle-Font-Bold="True" 
                                                    Caption="Job ID" 
                                                    Width="100px" 
                                                    VisibleIndex="3">
                        <CellStyle Wrap="False"></CellStyle>
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="~/Pages/WorkRequests/WorkRequestForm.aspx?jobid={0}"></PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn FieldName="Object ID" Caption="Object ID" Width="150px" VisibleIndex="4">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Title" Caption="Description" Width="300px" VisibleIndex="5">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FixedStyle="Left" 
                                                FieldName="Request Date" 
                                                Caption="Requested" 
                                                ReadOnly="True" 
                                                VisibleIndex="2" 
                                                SortOrder="Descending" 
                                                Width="120px"
                                                Settings-AllowHeaderFilter="True">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="RouteToID" Caption="Route To" Width="100px" VisibleIndex="8">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOpID" Caption="Work Op" Width="100px" VisibleIndex="9">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PriorityID" Caption="Priority" Width="100px" VisibleIndex="10">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ReasonID" Caption="Reason" Width="100px" VisibleIndex="11">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Requestor" Caption="Requestor" Width="100px" VisibleIndex="16">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CostCodeID" Caption="Cost Code" Width="100px" ReadOnly="True" VisibleIndex="18">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" Caption="Fund. Source" Width="100px" ReadOnly="True" VisibleIndex="19">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" Caption="Work Order" Width="100px" ReadOnly="True" VisibleIndex="20">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" Caption="Org. Code" Width="100px" ReadOnly="True" VisibleIndex="21">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" Caption="Fund. Group" Width="100px" ReadOnly="True" VisibleIndex="22">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ControlSectionID" Caption="Ctl. Section" Width="100px" ReadOnly="True" VisibleIndex="23">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" Caption="Equip. #" Width="100px" ReadOnly="True" VisibleIndex="24">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_worktypeid" ReadOnly="True" Visible="false" VisibleIndex="25">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_priorityid" ReadOnly="True" Visible="false" VisibleIndex="26">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_jobreasonid" ReadOnly="True" Visible="false" VisibleIndex="27">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SubAssemblyID" ReadOnly="True" Visible="false" VisibleIndex="28">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Notes" ReadOnly="True" Visible="false" VisibleIndex="29">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_requestor" ReadOnly="True" Visible="false" VisibleIndex="30">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Latitude" Caption="Latitude" ReadOnly="true" Visible="true" VisibleIndex="31">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Longitude" Caption="Longitude" ReadOnly="true" Visible="true" VisibleIndex="32">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Object_Latitude" Caption="Object Latitude" ReadOnly="true" Visible="true" VisibleIndex="33">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Object_Latitude" Caption="Object Latitude" ReadOnly="true" Visible="true" VisibleIndex="34">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsSearchPanel Visible="true" />
                <SettingsBehavior 
                    EnableRowHotTrack="True" 
                    AllowFocusedRow="True" 
                    AllowClientEventsOnLoad="false" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="false"
                    ColumnResizeMode="NextColumn" />
                <SettingsDataSecurity 
                    AllowDelete="False" 
                    AllowInsert="False" />
                <SettingsPopup HeaderFilter-Width="360" HeaderFilter-Height="360"></SettingsPopup>
                <Settings 
                    ShowFilterBar="Visible"
                    VerticalScrollBarMode="Visible" 
                    VerticalScrollBarStyle="Standard" 
                    VerticalScrollableHeight="500" />
                <SettingsPager PageSize="20">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="RequestDataSource" 
                               runat="server" 
                               ConnectionString="<%$ ConnectionStrings:connection %>"  
                               SelectCommand="
                                                         --Create/Set Null Date
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
                                WITH    cte_Jobs
                                          AS ( SELECT   tbl_Jobs.n_Jobid ,
                                                        tbl_Jobs.Jobid ,
                                                        tbl_Jobs.Title ,
                                                        tbl_Jobs.TypeOfJob ,
                                                        CASE tbl_Jobs.TypeOfJob
                                                          WHEN 1 THEN 'P'
                                                          WHEN 2 THEN 'C'
                                                          WHEN 3 THEN 'R'
                                                          WHEN 4 THEN 'B'
                                                          ELSE 'U'
                                                        END AS JobTypeCode ,
                                                        tbl_Jobs.JobAgainstID ,
                                                        tbl_MaintObj.objectid ,
                                                        tbl_Jobs.n_MaintObjectID ,
                                                        tbl_Jobs.n_AreaObjectID ,
                                                        tbl_Jobs.n_LocationObjectID ,
                                                        tbl_Jobs.n_GPSObjectID ,
                                                        tbl_Jobs.n_ActionPriority ,
                                                        tbl_Jobs.n_jobreasonid ,
                                                        tbl_Jobs.EstimatedJobHours ,
                                                        tbl_Jobs.ActualJobHours ,
                                                        tbl_Jobs.IsRequestOnly ,
                                                        tbl_Jobs.n_RouteTo ,
                                                        tbl_Jobs.RequestDate ,
                                                        tbl_Jobs.n_requestPriority ,
                                                        tbl_Jobs.n_requestor ,
                                                        tbl_Jobs.pmcalc_startdate ,
                                                        tbl_Jobs.n_MobileOwner ,
                                                        tbl_Jobs.MobileDate ,
                                                        tbl_Jobs.GPS_X ,
                                                        tbl_Jobs.GPS_Y ,
                                                        tbl_Jobs.GPS_Z ,
                                                        tbl_Jobs.n_group ,
                                                        tbl_Jobs.SentToPDA ,
                                                        tbl_Jobs.JobOpen ,
                                                        tbl_Jobs.IsHistory ,
                                                        tbl_Jobs.ServicingEquipment ,
                                                        tbl_Jobs.completed_units1 ,
                                                        tbl_Jobs.completed_units2 ,
                                                        tbl_Jobs.completed_units3 ,
                                                        tbl_Jobs.n_OutcomeID ,
                                                        tbl_Jobs.n_OwnerID ,
                                                        tbl_Jobs.n_TaskID ,
                                                        tbl_Jobs.n_workeventid ,
                                                        tbl_Jobs.n_worktypeid ,
                                                        tbl_Jobs.PostedDate ,
                                                        tbl_Jobs.SubAssemblyID ,
                                                        tbl_Jobs.n_StateRouteID ,
                                                        tbl_Jobs.AssignedToArea ,
                                                        tbl_Jobs.AssignedObjectType ,
                                                        tbl_MaintObj.description ,
                                                        tbl_MPETUsers.Username AS 'RouteToID' ,
                                                        tbl_WorkOp.WorkOpID ,
                                                        tbl_Priorities.priorityid ,
                                                        tbl_JobReasons.JobReasonID AS 'ReasonID' ,
                                                        tbl_SubAssembly.SubAssemblyName ,
                                                        tbl_StateRoutes.StateRouteID AS 'HwyRouteID' ,
                                                        tbl_Jobs.Milepost ,
                                                        tbl_Jobs.IncreasingMP ,
                                                        tbl_MilePostDir.MilePostDirectionID AS 'MilePostDirID' ,
                                                        tbl_Requestor.Username AS 'Requestor' ,
                                                        tbl_Jobs.n_costcodeid ,
                                                        tbl_CostCodes.costcodeid ,
                                                        tbl_FundSource.n_FundSrcCodeID ,
                                                        tbl_FundSource.FundSrcCodeID ,
                                                        tbl_WorkOrders.n_WorkOrderCodeID ,
                                                        tbl_WorkOrders.WorkOrderCodeID ,
                                                        tbl_OrgCode.n_OrganizationCodeID ,
                                                        tbl_OrgCode.OrganizationCodeID ,
                                                        tbl_FundGroup.n_FundingGroupCodeID ,
                                                        tbl_FundGroup.FundingGroupCodeID ,
                                                        tbl_ControlSection.n_ControlSectionID ,
                                                        tbl_ControlSection.ControlSectionID ,
                                                        tbl_EquipNumber.n_EquipmentNumberID ,
                                                        tbl_EquipNumber.EquipmentNumberID ,
                                                        tbl_Jobs.MilepostTo ,
                                                        tbl_MaintObj.areaid AS 'Area ID',
                                                        tbl_Creator.Username AS 'CreatedBy' ,
                                                        CASE tbl_Jobs.CreationDate
                                                          WHEN @NullDate THEN NULL
                                                          ELSE tbl_Jobs.CreationDate
                                                        END AS createdon,
                                                        tbl_Modifier.Username AS 'ModifiedBy' ,
                                                        CASE tbl_Jobs.LastModified
                                                          WHEN @NullDate THEN NULL
                                                          ELSE tbl_Jobs.LastModified
                                                        END AS modifiedon,
                            							ROW_NUMBER() OVER ( ORDER BY tbl_Jobs.[n_Jobid] ) AS [rn],
                                                        tbl_Jobs.Notes,
                                                        tbl_MaintObj.X,
														tbl_MaintObj.Y
                                               FROM     [dbo].[Jobs] tbl_Jobs
                                                        INNER JOIN ( SELECT tbl_MO.n_objectid ,
                                                                            tbl_MO.objectid ,
                                                                            tbl_MO.description ,
                                                                            tbl_MO.GPS_X AS X,
																			tbl_MO.GPS_Y AS Y,
                                                                            tbl_Area.areaid
                                                                     FROM   dbo.MaintenanceObjects tbl_MO
                                                                            INNER JOIN ( SELECT tblArea.n_areaid ,
                                                                                                tblArea.areaid
                                                                                         FROM   dbo.Areas tblArea
                                                                                         WHERE ( ( @areaFilteringOn = 'Y'
                                                                                                        AND EXISTS ( SELECT recordMatches.AreaFilterID
                                                                                                                     FROM   dbo.UsersAreaFilter AS recordMatches
                                                                                                                     WHERE  tblArea.n_areaid = recordMatches.AreaFilterID
                                                                                                                            AND recordMatches.UserID = @UserID
                                                                                                                            AND recordMatches.FilterActive = 'Y' )
                                                                                                      )
                                                                                                      OR ( @areaFilteringOn = 'N' )
                                                                                                    )
                                                                                       ) tbl_Area ON tbl_MO.n_areaid = tbl_Area.n_areaid
                                                                            INNER JOIN ( SELECT tblObjectTypes.n_objtypeid ,
                                                                                                tblObjectTypes.objtypeid
                                                                                         FROM   dbo.objecttypes tblObjectTypes
                                                                                       ) tbl_ObjectTypes ON tbl_ObjectTypes.n_objtypeid = tbl_MO.n_objtypeid
                                                                            INNER JOIN ( SELECT tblLocations.n_locationid ,
                                                                                                tblLocations.locationid
                                                                                         FROM   dbo.Locations tblLocations
                                                                                       ) tbl_Locations ON tbl_Locations.n_locationid = tbl_MO.n_locationid
                                                                   ) tbl_MaintObj ON tbl_Jobs.n_MaintObjectID = tbl_MaintObj.n_objectid
                                                        INNER JOIN ( SELECT tblRouteTo.UserID ,
                                                                            tblRouteTo.Username
                                                                     FROM   dbo.MPetUsers tblRouteTo
                                                                   ) tbl_MPETUsers ON tbl_Jobs.n_OwnerID = tbl_MPETUsers.UserID
                                                        INNER JOIN ( SELECT tblWorkOp.n_WorkOpID ,
                                                                            tblWorkOp.WorkOpID
                                                                     FROM   dbo.WorkOperations tblWorkOp
                                                                   ) tbl_WorkOp ON tbl_Jobs.n_worktypeid = tbl_WorkOp.n_WorkOpID
                                                        INNER JOIN ( SELECT tblPriorities.n_priorityid ,
                                                                            tblPriorities.priorityid
                                                                     FROM   dbo.Priorities tblPriorities
                                                                   ) tbl_Priorities ON tbl_Jobs.n_requestPriority = tbl_Priorities.n_priorityid
                                                        INNER JOIN ( SELECT tblJobReasons.nJobReasonID ,
                                                                            tblJobReasons.JobReasonID
                                                                     FROM   dbo.JobReasons tblJobReasons
                                                                   ) tbl_JobReasons ON tbl_Jobs.n_jobreasonid = tbl_JobReasons.nJobReasonID
                                                        LEFT JOIN ( SELECT  tblSubAssembly.nSubAssemblyID ,
                                                                            tblSubAssembly.SubAssemblyName
                                                                    FROM    dbo.SubAssemblyNames tblSubAssembly
                                                                  ) tbl_SubAssembly ON tbl_Jobs.SubAssemblyID = tbl_SubAssembly.nSubAssemblyID
                                                        INNER JOIN ( SELECT tblStateRoutes.n_StateRouteID ,
                                                                            tblStateRoutes.StateRouteID
                                                                     FROM   dbo.StateRoutes tblStateRoutes
                                                                   ) tbl_StateRoutes ON tbl_Jobs.n_StateRouteID = tbl_StateRoutes.n_StateRouteID
                                                        INNER JOIN ( SELECT tblMilepostDir.n_MilePostDirectionID ,
                            												tblMilepostDir.MilePostDirectionID
                                                                     FROM   dbo.MilePostDirections tblMilepostDir
                                                                   ) tbl_MilePostDir ON tbl_Jobs.IncreasingMP = tbl_MilePostDir.n_MilePostDirectionID
                                                        INNER JOIN ( SELECT tblRequestor.UserID ,
                                                                            tblRequestor.Username
                                                                     FROM   dbo.MPetUsers tblRequestor
                                                                   ) tbl_Requestor ON tbl_Jobs.n_requestor = tbl_Requestor.UserID
                                                        INNER JOIN ( SELECT tblCostCodes.n_costcodeid ,
                                                                            tblCostCodes.costcodeid ,
                                                                            tblCostCodes.SupplementalCode
                                                                     FROM   dbo.CostCodes tblCostCodes
                                                                   ) tbl_CostCodes ON tbl_Jobs.n_costcodeid = tbl_CostCodes.n_costcodeid
                                                        INNER JOIN ( SELECT tblFundSource.n_FundSrcCodeID ,
                                                                            tblFundSource.FundSrcCodeID
                                                                     FROM   dbo.FundSrcCodes tblFundSource
                                                                   ) tbl_FundSource ON tbl_Jobs.n_FundSrcCodeID = tbl_FundSource.n_FundSrcCodeID
                                                        INNER JOIN ( SELECT tblWorkOrders.n_WorkOrderCodeID ,
                                                                            tblWorkOrders.WorkOrderCodeID
                                                                     FROM   dbo.WorkOrderCodes tblWorkOrders
                                                                   ) tbl_WorkOrders ON tbl_Jobs.n_WorkOrderCodeID = tbl_WorkOrders.n_WorkOrderCodeID
                                                        INNER JOIN ( SELECT tblOrgCode.n_OrganizationCodeID ,
                                                                            tblOrgCode.OrganizationCodeID
                                                                     FROM   dbo.OrganizationCodes tblOrgCode
                                                                   ) tbl_OrgCode ON tbl_Jobs.n_OrganizationCodeID = tbl_OrgCode.n_OrganizationCodeID
                                                        INNER JOIN ( SELECT tblFundGroup.n_FundingGroupCodeID ,
                                                                            tblFundGroup.FundingGroupCodeID
                                                                     FROM   dbo.FundingGroupCodes tblFundGroup
                                                                   ) tbl_FundGroup ON tbl_Jobs.n_FundingGroupCodeID = tbl_FundGroup.n_FundingGroupCodeID
                                                        INNER JOIN ( SELECT	tblEquipNumber.n_EquipmentNumberID ,
                                                                            tblEquipNumber.EquipmentNumberID
                                                                     FROM   dbo.EquipmentNumber tblEquipNumber
                                                                   ) tbl_EquipNumber ON tbl_Jobs.n_EquipmentNumberID = tbl_EquipNumber.n_EquipmentNumberID
                                                        INNER JOIN ( SELECT tblControlSection.n_ControlSectionID ,
                                                                            tblControlSection.ControlSectionID
                                                                     FROM   dbo.ControlSections tblControlSection
                                                                   ) tbl_ControlSection ON tbl_Jobs.n_ControlSectionID = tbl_ControlSection.n_ControlSectionID
                            									                               INNER JOIN ( SELECT dbo.MPetUsers.userid ,
                                                                            CASE dbo.MpetUsers.UserID
                                                                              WHEN -1 THEN 'N/A'
                                                                              WHEN 0 THEN 'IMPORTED'
                                                                              ELSE dbo.MPetUsers.Username
                                                                            END AS Username
                                                                     FROM   dbo.mpetusers
                                                                   ) tbl_Creator ON tbl_Jobs.CreatedBy = tbl_Creator.UserID
                                                        INNER JOIN ( SELECT dbo.MPetUsers.userid ,
                                                                            CASE dbo.MpetUsers.UserID
                                                                              WHEN -1 THEN 'N/A'
                                                                              WHEN 0 THEN 'IMPORTED'
                                                                              ELSE dbo.MPetUsers.Username
                                                                            END AS Username
                                                                     FROM   dbo.MPetUsers
                                                                   ) tbl_Modifier ON tbl_Jobs.LastModifiedBy = tbl_Modifier.UserID
                                               WHERE    ( tbl_Jobs.n_JobID > 0 )
                                                        AND ( tbl_Jobs.IsHistory = 'N' )
                                                        AND ( tbl_Jobs.IsRequestOnly = 'Y' )
                                             )
                                    --Return Data        
                                SELECT  cte_Jobs.n_Jobid ,
                                        cte_Jobs.Jobid ,
                                        cte_Jobs.ObjectID AS [Object ID] ,
                                        cte_Jobs.Title ,
                                        CASE cte_Jobs.RequestDate
                                          WHEN @NullDate THEN NULL
                                          ELSE CAST(CAST(RequestDate AS VARCHAR(11)) AS DATETIME)
                                        END AS [Request Date] ,
                                        cte_Jobs.RouteToID ,
                                        cte_Jobs.WorkOpID ,
                                        cte_Jobs.PriorityID ,
                                        cte_Jobs.ReasonID ,
                                        cte_Jobs.Requestor ,	
                                        CASE [CostCodeID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [CostCodeID]
                                        END AS [CostCodeID] ,
                                        CASE [FundSrcCodeID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [FundSrcCodeID]
                                        END AS [FundSrcCodeID] ,
                                        CASE [WorkOrderCodeID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [WorkOrderCodeID]
                                        END AS [WorkOrderCodeID] ,
                                        CASE [OrganizationCodeID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [OrganizationCodeID]
                                        END AS [OrganizationCodeID] ,
                                        CASE [FundingGroupCodeID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [FundingGroupCodeID]
                                        END AS [FundingGroupCodeID] ,
                                        CASE [ControlSectionID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [ControlSectionID]
                                        END AS [ControlSectionID] ,
                                        CASE [EquipmentNumberID]
                                          WHEN 'N/A' THEN NULL
                                          ELSE [EquipmentNumberID]
                                        END AS [EquipmentNumberID],
                                        cte_Jobs.n_worktypeid,
                                        cte_Jobs.n_requestPriority as [n_priorityid],
                                        cte_Jobs.n_jobreasonid,
                                        cte_Jobs.SubAssemblyID,
                                        cte_Jobs.Notes,
                                        cte_Jobs.n_requestor,
                                        cte_Jobs.GPS_X AS [Latitude],
										cte_Jobs.GPS_Y AS [Longitude],
										cte_Jobs.X AS [Object_Latitude],
										cte_Jobs.Y AS [Object_Longitude]
                                FROM    cte_Jobs">
                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                        </ContentTemplate>

    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="ReqGrid"></dx:ASPxGridViewExporter>
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
</asp:Content>