<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JobHistoryCostList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.History.JobHistoryCostList" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Job Cost History</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">  
    <h1>JOB COST HISTORY</h1> 
        <script type="text/javascript">

            function OnGetRowId(idValue) {
                Selection.Set('Jobid', idValue[0].toString());
                Selection.Set('n_jobid', idValue[1].toString());
                Selection.Set('n_jobstepid', idValue[2].toString());
            }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
    <dx:ASPxGridView 
        ID="HistoryCostGrid" 
        runat="server"
        Theme="Mulberry"
        KeyFieldName="n_jobid" 
        Width="98%"
        KeyboardSupport="True"
        ClientInstanceName="HistoryCostGrid" 
        AutoPostBack="False" 
        SettingsPager-Mode="ShowPager" 
        Settings-HorizontalScrollBarMode="Auto"
        SettingsBehavior-AllowFocusedRow="True"
        DataSourceID="ObjectGridDataSource">
        <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
            <Header CssClass="gridViewHeader"></Header>
            <Row CssClass="gridViewRow"></Row>
            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
            <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
        </Styles>
                        <ClientSideEvents FocusedRowChanged="function(s, e) {
                        HistoryCostGrid.GetRowValues(s.GetFocusedRowIndex(), 'Jobid;n_jobid;n_jobstepid', OnGetRowId);
                    }" />
        <Columns>
           <dx:GridViewCommandColumn ShowSelectCheckbox="True" Visible="false" VisibleIndex="0" />
            <dx:GridViewDataTextColumn FieldName="n_jobid" ReadOnly="True" Visible="false" VisibleIndex="1">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="JobGUID" ReadOnly="True" Visible="false" VisibleIndex="2">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Jobid" 
                                        HeaderStyle-Font-Bold="True" 
                                        FixedStyle="Left" 
                                        Caption="Job ID" 
                                        Width="140px" 
                                        VisibleIndex="4">
                <CellStyle Wrap="False"></CellStyle>
                                <DataItemTemplate>
						<dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# GetUrl(Container) %>"
                            Text='<%# Eval("Jobid") %>' Width="100%" Theme="Mulberry" >
						</dxe:ASPxHyperLink>
					</DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn  FieldName="CompletedDate" 
                                        Caption="Completed" 
                                        SortOrder="Descending" 
                                        Width="120px"
                                        Settings-AllowHeaderFilter="True" 
                                        FixedStyle="Left" 
                                        ReadOnly="True" 
                                        VisibleIndex="3">
                <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="TotalParts" Caption="Ttl. Parts" Width="150px" VisibleIndex="5">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalLabor" Caption="Ttl. Labor" Width="150px" VisibleIndex="6">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalEquip" Caption="Ttl. Equip." Width="150px" VisibleIndex="7">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalOther" Caption="Ttl. Other" Width="150" VisibleIndex="8">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalCost" Caption="Ttl. Cost" Width="150" VisibleIndex="9">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Description" Width="300px" VisibleIndex="10">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Object ID" Caption="Object ID" Width="200px" VisibleIndex="11">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AreaID" Caption="Area" Width="100px" ReadOnly="True" VisibleIndex="12">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="WorkOpID" Caption="Work Op" Width="100px" ReadOnly="True" VisibleIndex="13">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Prod. Units" Caption="Prod. Units" Width="100px" ReadOnly="True" VisibleIndex="14">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Labor Hrs." Caption="Labor Hrs." Width="100px" ReadOnly="True" VisibleIndex="15">
                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Job Reason ID" Caption="Reason" Width="100px" ReadOnly="True" VisibleIndex="16">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>      
            <dx:GridViewDataTextColumn FieldName="n_jobstepid" ReadOnly="True" Visible="false" VisibleIndex="17">
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
                                HeaderFilter-Height="360"></SettingsPopup>
        <Settings ShowFilterBar="Visible" 
                    VerticalScrollBarMode="Visible" 
                    VerticalScrollBarStyle="Virtual" 
                    VerticalScrollableHeight="400" 
                    ShowGroupPanel="True" 
                    ShowFooter="True" 
                    ShowGroupFooter="VisibleIfExpanded"/>
        <SettingsPager PageSize="20">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="TotalParts" SummaryType="sum" />
            <dx:ASPxSummaryItem FieldName="TotalLabor" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalEquip" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalOther" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" />
        </TotalSummary>
        <GroupSummary>
            <dx:ASPxSummaryItem FieldName="TotalParts" ShowInGroupFooterColumn="TotalParts" SummaryType="sum" />
            <dx:ASPxSummaryItem FieldName="TotalLabor" ShowInGroupFooterColumn="TotalLabor" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalEquip" ShowInGroupFooterColumn="TotalEquip" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalOther" ShowInGroupFooterColumn="TotalOther" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalCost" ShowInGroupFooterColumn="TotalCost" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="TotalCost"  SummaryType="Sum" />
        </GroupSummary>
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
WITH    cte_JobCosts
              AS ( SELECT   tbl_Jobs.n_jobid ,
                            tbl_Jobs.AssignedGUID AS 'JobGUID' ,
                            tbl_Jobs.TypeOfJob ,
                            tbl_Jobs.JobAgainstID ,
                            CASE tbl_Jobs.TypeOfJob
                              WHEN 1 THEN 'P'
                              WHEN 2 THEN 'C'
                              WHEN 3 THEN 'R'
                              WHEN 4 THEN 'B'
                              ELSE 'U'
                            END AS JobTypeCode ,
                            CASE tbl_Jobs.JobAgainstID
                              WHEN 1 THEN 'M'
                              WHEN 2 THEN 'L'
                              WHEN 3 THEN 'A'
                              WHEN 4 THEN 'G'
                              ELSE 'U'
                            END AS JobAgainstTypeCode ,
                            tbl_Jobs.Jobid ,
                            tbl_Jobs.n_MaintObjectID AS n_MaintObjectID ,
                            tbl_MaintObj.objectid AS 'Object ID' ,
                            tbl_Jobs.Title ,
                            tbl_Jobs.n_OutcomeID ,
                            tbl_Jobs.n_requestPriority ,
                            tbl_MaintObj.objectid ,
                            CASE tbl_Jobs.JobAgainstID
                              WHEN 1 THEN tbl_Jobs.n_MaintObjectID
                              WHEN 2 THEN -1
                              WHEN 3 THEN -1
                              WHEN 4 THEN -1
                              ELSE -1
                            END AS nMaintObjectID ,
                            tbl_Jobs.IsHistory ,
                            tbl_Jobs.IssuedDate ,
                            CASE tbl_Jobs.IssuedDate
                              WHEN @NullDate THEN 'N'
                              ELSE 'Y'
                            END AS IsIssued ,
                            tbl_Jobs.AssignedToArea ,
                            tbl_Areas.areaid ,
                            tbl_Jobs.AssignedObjectType ,
                            tbl_Jobs.n_TaskID ,
                            tbl_MaintObj.description ,
                            tbl_HistCost.TTLParts AS 'TotalParts' ,
                            tbl_HistCost.TTLLabor AS 'TotalLabor' ,
                            tbl_HistCost.TTLEquipment AS 'TotalEquip' ,
                            tbl_HistCost.TTLParts + tbl_HistCost.TTLLabor + tbl_HistCost.TTLEquipment + tbl_HistCost.TTLOther AS 'TotalCost' ,
                            tbl_Jobs.RequestDate ,
                            tbl_StartDate.StartingDate AS 'StartDate' ,
                            CASE tbl_CompletedDate.DateTimeCompleted
                              WHEN @NullDate THEN NULL
                              ELSE CAST(CAST(tbl_CompletedDate.DateTimeCompleted AS VARCHAR(11)) AS DATETIME)
                            END AS 'CompletedDate' ,
                            tbl_Jobs.PostedDate ,
                            tbl_Jobs.n_worktypeid ,
                            tbl_WorkOp.WorkOpID ,
                            tbl_HistCost.TTLOther AS 'TotalOther' ,
                            tbl_Jobs.ActualUnits AS 'Prod. Units' ,
                            ISNULL(tbl_JC.Hours, 0) AS 'Labor Hrs.' ,
                            tbl_Jobs.n_jobreasonid ,
                            tbl_JobReasons.JobReasonID AS 'Job Reason ID',
                            tbl_MinStepId.n_jobstepid
                   FROM     dbo.Jobs tbl_Jobs
                            INNER JOIN ( SELECT tblObjects.n_objectid ,
                                                tblObjects.objectid ,
                                                tblObjects.description ,
                                                tblObjects.n_areaid
                                         FROM   dbo.MaintenanceObjects tblObjects
                                       ) tbl_MaintObj ON tbl_Jobs.n_MaintObjectID = tbl_MaintObj.n_objectid
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
                                       ) tbl_Areas ON tbl_Jobs.AssignedToArea = tbl_Areas.n_areaid
                            INNER JOIN ( SELECT tblWorkOp.n_WorkOpID ,
                                                tblWorkOp.WorkOpID
                                         FROM   dbo.WorkOperations tblWorkOp
                                       ) tbl_WorkOp ON tbl_Jobs.n_worktypeid = tbl_WorkOp.n_WorkOpID
                            INNER JOIN ( SELECT dbo.HistCostSumm.n_HistoryID ,
                                                dbo.HistCostSumm.TTLParts ,
                                                dbo.HistCostSumm.TTLLabor ,
                                                dbo.HistCostSumm.TTLEquipment ,
                                                dbo.HistCostSumm.TTLOther
                                         FROM   dbo.HistCostSumm
                                       ) tbl_HistCost ON tbl_Jobs.n_Jobid = tbl_HistCost.n_HistoryID
                            OUTER APPLY ( SELECT    MIN(tblStartingJobsteps.StartingDate) AS 'StartingDate'
                                          FROM      dbo.Jobsteps tblStartingJobsteps
                                          WHERE     tblStartingJobsteps.n_jobid = tbl_Jobs.n_jobid
                                        ) AS tbl_StartDate
                            OUTER APPLY ( SELECT    MAX(Jobsteps.DateTimeCompleted) AS 'DateTimeCompleted'
                                          FROM      dbo.Jobsteps
                                          WHERE     dbo.Jobsteps.n_jobid = tbl_Jobs.n_jobid
                                        ) AS tbl_CompletedDate
                            OUTER APPLY ( SELECT    MIN(Jobsteps.n_jobstepid) AS 'n_jobstepid'
                                          FROM      dbo.Jobsteps
                                          WHERE     dbo.Jobsteps.n_jobid = tbl_Jobs.n_jobid
                                        ) AS tbl_MinStepId                            
                            LEFT JOIN ( SELECT  dbo.Jobcrews.JobID ,
                                                SUM(dbo.Jobcrews.actuallen) AS 'Hours'
                                        FROM    dbo.Jobcrews
                                        GROUP BY Jobcrews.JobID
                                      ) tbl_JC ON tbl_Jobs.n_Jobid = tbl_JC.JobID
                            INNER JOIN ( SELECT tblJobReasons.nJobReasonID ,
                                                tblJobReasons.JobReasonID
                                         FROM   dbo.JobReasons tblJobReasons
                                       ) tbl_JobReasons ON tbl_Jobs.n_jobreasonid = tbl_JobReasons.nJobReasonID
                   WHERE    ( tbl_Jobs.IsHistory = 'Y' )
                 )
        --Select Filtered Information
    SELECT  cte_JobCosts.n_jobid ,
            cte_JobCosts.JobGUID ,
            cte_JobCosts.Jobid ,
            cte_JobCosts.[Object ID] ,
            cte_JobCosts.Title ,
            cte_JobCosts.TotalParts ,
            cte_JobCosts.TotalLabor ,
            cte_JobCosts.TotalEquip ,
            cte_JobCosts.TotalCost ,
            cte_JobCosts.CompletedDate ,
            cte_JobCosts.AreaID ,
            cte_JobCosts.WorkOpID ,
            cte_JobCosts.TotalOther ,
            cte_JobCosts.[Prod. Units] ,
            cte_JobCosts.[Labor Hrs.] ,
            cte_JobCosts.[Job Reason ID] ,
            cte_JobCosts.[n_jobstepid]
    FROM    cte_JobCosts">
        
                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
            </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="HistoryCostGrid"></dx:ASPxGridViewExporter>
</asp:Content>