<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="MyWorkRequestList.aspx.cs" Inherits="Pages_WorkRequests_MyWorkRequestList" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">My Jobs Request</asp:Content>
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
       <%-- <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" Width="100px" FixedStyle="Left" Visible="False">
                <CellStyle HorizontalAlign="Left" Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>
            
            <dx:GridViewDataTextColumn FieldName="n_jobid" VisibleIndex="4" FixedStyle="Left" Visible="False">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="T" ReadOnly="True" VisibleIndex="6" Caption="Type" Width="50px">

            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="A" 
                ReadOnly="True" VisibleIndex="45" 
                Caption="Job Against" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Object ID" VisibleIndex="8" Caption="Object ID">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
            </dx:GridViewDataTextColumn>
                       
            <dx:GridViewDataTextColumn FieldName="PriorityID" VisibleIndex="10" Caption="Priority">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataDateColumn FieldName="Requestor" 
                ReadOnly="True" VisibleIndex="12" 
                Caption="Requestor" Visible="False">
            </dx:GridViewDataDateColumn>
                       
            <dx:GridViewDataTextColumn FieldName="ReasonID" VisibleIndex="14" Caption="Reason">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></CellStyle>
                </dx:GridViewDataTextColumn>                      
            
            <dx:GridViewDataTextColumn FieldName="WorkOpID" 
                VisibleIndex="17" Caption="Work Op" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HwyRouteID" 
                ReadOnly="True" VisibleIndex="18" 
                Caption="Hwy. Route" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Milepost" 
                ReadOnly="True" VisibleIndex="19" 
                Caption="MM. Form" Visible="False">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="MilePostTo" 
                ReadOnly="True" VisibleIndex="20" 
                Caption="MM. To" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MilePostDirID" 
                ReadOnly="True" VisibleIndex="21" 
                Caption="MM. Direction" Visible="False">
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataTextColumn FieldName="SubAssemblyName" 
                VisibleIndex="26" Caption="Sub Assembly" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Area ID" 
                VisibleIndex="27" Caption="Area" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FlaggedRecordID" 
                ReadOnly="True" VisibleIndex="51" 
                Caption="Flagged Record" Visible="False">
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
            
            <dx:GridViewDataTextColumn FieldName="RouteToID" 
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
            
            <dx:GridViewDataTextColumn FieldName="Post Notes" 
                ReadOnly="True" VisibleIndex="57" 
                Caption="Post Notes" CellStyle-Wrap="False" 
                Width="200" Visible="False">
<CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Jobid" VisibleIndex="2" SortOrder="Descending" Caption="Job ID" FixedStyle="Left" Settings-AllowSort="True" Settings-AllowFilterBySearchPanel="True">
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
        </Columns>--%>
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
