<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaskScheduleList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.Tasks.TaskScheduleList" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Task Schedules</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">  
    <h1>TASK SCHEDULES</h1> 
    <script type="text/javascript">

        function OnGetRowId(idValue) {
            Selection.Set('taskid', idValue[0].toString());
            Selection.Set('n_objtaskid', idValue[1].toString());
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
    <dx:ASPxGridView 
        ID="TSGrid" 
        runat="server"
        Theme="Mulberry"
        KeyFieldName="n_objtaskid" 
        Width="98%"
        KeyboardSupport="True"
        ClientInstanceName="TSGrid" 
        AutoPostBack="True"
                Settings-HorizontalScrollBarMode="Auto"
                SettingsPager-Mode="ShowPager"
                SettingsBehavior-ProcessFocusedRowChangedOnServer="True"
                SettingsBehavior-AllowFocusedRow="True"
                SelectionMode="Multiple"
                DataSourceID="PMTaskDataSource">
        <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
            <Header CssClass="gridViewHeader"></Header>
            <Row CssClass="gridViewRow"></Row>
            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
            <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
        </Styles>
                        <ClientSideEvents RowClick="function(s, e) {
                        TSGrid.GetRowValues(e.visibleIndex, 'TASKID;n_objtaskid', OnGetRowId);
                    }" />
        <Columns>
            <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" Visible="false" VisibleIndex="0" />
            <dx:GridViewDataTextColumn FieldName="n_objtaskid" ReadOnly="True" Visible="false" VisibleIndex="1">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TASKID" 
                                        Caption="Object / Task ID" 
                                        Width="350px" 
                                        VisibleIndex="4"
                                        HeaderStyle-Font-Bold="True"
                                        SortOrder="Ascending"
                                        FixedStyle="Left">
                <CellStyle Wrap="False"></CellStyle>
                                <DataItemTemplate>
						<dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# GetUrl(Container) %>"
                            Text='<%# Eval("TASKID") %>' Width="100%" Theme="Mulberry" >
						</dxe:ASPxHyperLink>
					</DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="300px" VisibleIndex="5">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="D" FixedStyle="Left" Caption="Disabled" Width="75px" VisibleIndex="2">
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
                <SettingsBehavior 
                    EnableRowHotTrack="True" 
                    AllowFocusedRow="True" 
                    AllowClientEventsOnLoad="false" 
                    ColumnResizeMode="NextColumn" />
                <SettingsDataSecurity 
                    AllowDelete="False" 
                    AllowInsert="False" />
                <SettingsPopup HeaderFilter-Width="360" HeaderFilter-Height="360"></SettingsPopup>
                <Settings 
                    ShowFilterBar="Visible"
                    VerticalScrollBarMode="Visible" 
                    VerticalScrollBarStyle="Virtual" 
                    VerticalScrollableHeight="500" />
                <SettingsPager PageSize="20">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
    </dx:ASPxGridView>
             <%--TODO: Set up Sql DataSource--%>
            <asp:SqlDataSource ID="PMTaskDataSource" 
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
                                                </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="TSGrid"></dx:ASPxGridViewExporter>
</asp:Content>