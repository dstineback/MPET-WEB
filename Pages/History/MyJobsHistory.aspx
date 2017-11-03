<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="MyJobsHistory.aspx.cs" Inherits="Pages_History_MyJobsHistory" %>

<%@ MasterType VirtualPath="~/SiteBase.master" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">My Jobs History</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>My Jobs</h1>
    <script type="text/javascript">
        function OnGetRowID(idValue) {
              
            Selection.Set('Jobid', idValue[0].toString());
            //Selection.Set('n_jobid', idValue[1].toString());                  
        }
    </script>
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField> 
    <dx:ASPxGridView ID="myWorkRequestGrid" runat="server" Theme="Mulberry" Width="100%" 
        AutoGenerateColumns="False" EnableTheming="True" SettingsBehavior-AllowFocusedRow="false" 
        Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollBarMode="Visible" Settings-VerticalScrollBarStyle="Standard" 
        SettingsPager-Mode="ShowPager" SettingsAdaptivity-AdaptivityMode="HideDataCells" 
        SettingsBehavior-AllowSelectByRowClick="false" KeyFieldName="JobID" SettingsText-EmptyDataRow="No Data" 
        SettingsText-EmptyHeaders="No Data" SettingsPager-PageSize="30" SettingsBehavior-AllowEllipsisInText="true" SettingsResizing-ColumnResizeMode="NextColumn">
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
        <ClientSideEvents RowClick="function(s, e) {                       
                        myWorkRequestGrid.GetRowValues(e.visibleIndex, 'Jobid;n_jobid', OnGetRowID);                                           
                    }" />   
        <StylesContextMenu>
            <Row>
                <Link HoverColor="#66FFFF"></Link>
            </Row>
        </StylesContextMenu>
        <SettingsPager Position="Top"></SettingsPager>
        <Settings ShowFilterRow="True" ShowGroupPanel="True" AutoFilterCondition="Equals" EnableFilterControlPopupMenuScrolling="True" HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollableHeight="500" VerticalScrollBarMode="Visible"></Settings>
        <SettingsBehavior ColumnResizeMode="NextColumn" />

        <SettingsPager Position="Top"></SettingsPager>

        <Settings AutoFilterCondition="Equals" EnableFilterControlPopupMenuScrolling="True" HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollableHeight="450" VerticalScrollBarMode="Visible"></Settings>


        <SettingsCommandButton>
            <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>
            <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
        </SettingsCommandButton>

        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False"></SettingsDataSecurity>
        <SettingsSearchPanel ShowApplyButton="True" ShowClearButton="True" Visible="True"></SettingsSearchPanel>

        <Columns>
            <dx:GridViewDataTextColumn FieldName="JobID" Caption="Job ID" Width="100" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Description" Width="300" VisibleIndex="2" CellStyle-Wrap="True">
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataDateColumn FieldName="StartingDate" Caption="Start Date" FixedStyle="Left" VisibleIndex="0" Width="150"></dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="IsHistory" Caption="History" VisibleIndex="8" Width="100">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StatusID" Caption="Status" VisibleIndex="3" Width="100" >
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PostedDate" Caption="Date Posted" VisibleIndex="7"  Width="150">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Notes" Caption="Notes" VisibleIndex="6" Width="600" Settings-AllowEllipsisInText="True" AllowTextTruncationInAdaptiveMode="true" CellStyle-Wrap="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="JobReasonID" Caption="Reason" VisibleIndex="4" Width="100">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="priorityid" Caption="Priority" VisibleIndex="5" Width="100">
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

</asp:Content>

