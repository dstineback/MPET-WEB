<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TasksList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.Tasks.TasksList" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">PM Tasks</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">  
    <h1>PM TASKS</h1> 
    <script type="text/javascript">

        function OnGetRowId(idValue) {
            Selection.Set('taskid', idValue[0].toString());
            Selection.Set('n_taskid', idValue[1].toString());
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
            <dx:ASPxGridView
                ID="PMTaskGrid"
                runat="server"
                Theme="Mulberry"
                KeyFieldName="taskid"
                Width="98%"
                KeyboardSupport="True"
                ClientInstanceName="PMTaskGrid"
                AutoPostBack="True"
                Settings-HorizontalScrollBarMode="Auto"
                SettingsPager-Mode="ShowPager"
                SettingsBehavior-ProcessFocusedRowChangedOnServer="True"
                SettingsBehavior-AllowFocusedRow="True"
                SelectionMode="Multiple"
                DataSourceID="PMTaskDataSource">
                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"
                    RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow">
                    <Header CssClass="gridViewHeader"></Header>
                    <Row CssClass="gridViewRow"></Row>
                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                </Styles>
                <ClientSideEvents RowClick="function(s, e) {
                        PMTaskGrid.GetRowValues(e.visibleIndex, 'taskid;n_taskid', OnGetRowId);
                    }" />
                <Columns>
                    <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" Visible="false" VisibleIndex="0" />
                    <dx:GridViewDataTextColumn FieldName="n_taskid" ReadOnly="True" Visible="false" VisibleIndex="2">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn FixedStyle="Left"
                        FieldName="taskid"
                        Caption="Task ID"
                        Width="200px"
                        VisibleIndex="3"
                        HeaderStyle-Font-Bold="True"
                        SortOrder="Ascending">
                        <CellStyle Wrap="False"></CellStyle>
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="~/Pages/Tasks/Tasks.aspx?taskid={0}"></PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="400px" VisibleIndex="4">
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
                    <dx:GridViewDataTextColumn FieldName="AssignedAreaID" Caption="Area ID" Width="200px" VisibleIndex="9">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FixedStyle="Left" FieldName="IsActive" Caption="Active" Width="100px" ReadOnly="True" VisibleIndex="1">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
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
            <%-- TODO: Set up SQL command--%>
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
                                               WHERE    ( tbl_Tasks.n_taskid > 0 )
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
                                                    </ContentTemplate>

    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="PMTaskGrid"></dx:ASPxGridViewExporter>
</asp:Content>