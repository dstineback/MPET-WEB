<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkRequestForm.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.WorkRequests.WorkRequestForm" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Request</asp:Content>

<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

    <dx:ASPxLabel ID="lblHeader" Font-size="20px" Theme="Mulberry" runat="server" Text="ADD"></dx:ASPxLabel>
    <dx:ASPxHiddenField ID="Navigation" ViewStateMode="Enabled"  ClientInstanceName="Navigation" runat="server"></dx:ASPxHiddenField>
        
    
    
        <dx:ASPxFormLayout ID="ASPxFormLayout1" 
            runat="server" EnableTheming="True" 
            Theme="iOS" Width="1438px" Height="400px">
            <Items>
                <dx:LayoutItem Caption="Description of Work" 
                    RowSpan="2">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="ASPxFormLayout1_E6" 
                                runat="server" Height="84px" Width="946px">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutGroup ColCount="2" 
                    GroupBoxDecoration="Box" Caption="">
                    <Items>
                        <dx:LayoutItem Caption="Object/Asset:" 
                            RequiredMarkDisplayMode="Hidden">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E10" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Object Description" 
                            RequiredMarkDisplayMode="Optional">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E9" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Reason:">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ASPxFormLayout1_E3" 
                                        runat="server">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Priority:">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E4" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutItem Caption="Attachments:">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxUploadControl runat="server" ID="UploadControl" Theme="iOS" ClientInstanceName="Uploadcontrol" Width="95%" UploadMode="Auto" UploadStorage="Azure" FileUploadMode="OnPageLoad" ShowUploadButton="true" ShowProgressPanel="true" OnFileUploadComplete="UploadControl_FileUploadComplete" ShowAddRemoveButtons="true">
                            <AdvancedModeSettings EnableDragAndDrop="true" EnableFileList="false" EnableMultiSelect="true">
                                <FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
                            </AdvancedModeSettings>
                            <ValidationSettings MaxFileSize="4194304"  AllowedFileExtensions=".jpg,.jpeg,.gif,.png"> </ValidationSettings>
                            <ClientSideEvents FileUploadComplete="onFileUploadComplete"/>
                            </dx:ASPxUploadControl>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="" ShowCaption="False" ColSpan="2" CaptionSettings-Location="Top">
                   <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>'
                     <asp:UpdatePanel runat="server">
                         <ContentTemplate>
                             <dx:ASPxGridview runat="server" Id="AttachmentGrid" Theme="iOS" KeyFieldName="LocationOrURL" Width="95%" KeyboardSupport="true" ClientInstanceName="AttachmentGrid" AutoPostBack="true" Settings-HorizontalScrollBarMode="Auto" SettingsPager-Mode="ShowPager" SettingsBehavior-ProcessFocusedRowChangedOnServer="true" SettingsBehavior-AllowFocusedRow="true" EnableCallBacks="true" AutoGenerateColumns="false" DataSourceID="AttachmentDataSource">
                                 <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                                    RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                    <Header CssClass="gridViewHeader"></Header>

                                    <Row CssClass="gridViewRow"></Row>

                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                </Styles>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="false" VisibleIndex="0">
                                        <CellStyle Wrap="False"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="nJobID" ReadOnly="True" Visible="false" VisibleIndex="1">
                                        <CellStyle Wrap="False"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="nJobstepID" ReadOnly="True" Visible="false" VisibleIndex="2">
                                        <CellStyle Wrap="False"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="ShortName" Caption="Name" Width="200px" VisibleIndex="3">
                                        <CellStyle Wrap="False"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="DocType" Caption="Name" Width="100px" VisibleIndex="4">
                                        <CellStyle Wrap="False"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Description" Visible="false" Caption="Description" Width="400px" VisibleIndex="5">
                                        <CellStyle Wrap="False"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataHyperLinkColumn FieldName="LocationOrURL" Caption="Location/URL" Width="600px" VisibleIndex="6">
                                        <CellStyle Wrap="False"></CellStyle>
                                        <PropertiesHyperLinkEdit ></PropertiesHyperLinkEdit>
                                    </dx:GridViewDataHyperLinkColumn>
                                </Columns>
                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350" />
                                <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" />
                             </dx:ASPxGridview>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                    </dx:LayoutItemNestedControlContainer>

                   </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
        </dx:ASPxFormLayout>
  </asp:Content>

