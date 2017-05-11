<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkRequestForm.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.WorkRequests.WorkRequestForm" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<%@ Register Src="~/UserControls/UploadedFilesContainer.ascx" TagName="UploadedFilesContainer" TagPrefix="dx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Request</asp:Content>

<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
<script type="text/javascript">
        DXUploadedFilesContainer = {
            nameCellStyle: "",
            sizeCellStyle: "",
            useExtendedPopup: false,

            AddFile: function(fileName, fileUrl, fileSize) {
                var self = DXUploadedFilesContainer;
                var builder = ["<tr>"];

                builder.push("<td class='nameCell'");
                if (self.nameCellStyle)
                    builder.push(" style='" + self.nameCellStyle + "'");
                builder.push(">");
                self.BuildLink(builder, fileName, fileUrl);
                builder.push("</td>");

                builder.push("<td class='sizeCell'");
                if (self.sizeCellStyle)
                    builder.push(" style='" + self.sizeCellStyle + "'");
                builder.push(">");
                builder.push(fileSize);
                builder.push("</td>");

                builder.push("</tr>");

                var html = builder.join("");
                DXUploadedFilesContainer.AddHtml(html);
            },
            Clear: function() {
                DXUploadedFilesContainer.ReplaceHtml("");
            },
            BuildLink: function(builder, text, url) {
                builder.push("<a target='blank' onclick='return DXDemo.ShowScreenshotWindow(event, this, " + this.useExtendedPopup + ");'");
                builder.push(" href='" + url + "'>");
                builder.push(text);
                builder.push("</a>");
            },
            AddHtml: function(html) {
                var fileContainer = document.getElementById("uploadedFilesContainer"),
                    fullHtml = html;
                if (fileContainer) {
                    var containerBody = fileContainer.tBodies[0];
                    fullHtml = containerBody.innerHTML + html;
                }
                DXUploadedFilesContainer.ReplaceHtml(fullHtml);
            },
            ReplaceHtml: function(html) {
                var builder = ["<table id='uploadedFilesContainer' class='uploadedFilesContainer'><tbody>"];
                builder.push(html);
                builder.push("</tbody></table>");
                var contentHtml = builder.join("");
                window.FilesRoundPanel.SetContentHtml(contentHtml);
            },
            ApplySettings: function(nameCellStyle, sizeCellStyle, useExtendedPopup) {
                var self = DXUploadedFilesContainer;
                self.nameCellStyle = nameCellStyle;
                self.sizeCellStyle = sizeCellStyle;
                self.useExtendedPopup = useExtendedPopup;
            }
        };

        function onFileUploadComplete(s, e) {
            if (e.callbackData) {
                var fileData = e.callbackData.split('|');
                var fileName = fileData[0],
                    fileUrl = fileData[1],
                    fileSize = fileData[2];
                //DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
                window.AttachmentGrid.Refresh();
            }
        }

        //function getLocation() {

        //    if (navigator.geolocation) {
        //        navigator.geolocation.getCurrentPosition(showPosition, showError);
        //    } else {
        //        showError("Geolocation is not supported by this browser.");
        //    }
        //}

        //function showPosition(position) {

        //    var lat = position.coords.latitude;
        //    var lon = position.coords.longitude;
        //    var alt = position.coords.altitude;
        //    var latlon = new window.google.maps.LatLng(lat, lon);
        //    var mapholder = document.getElementById('map');
        //    mapholder.style.height = '225px';
        //    mapholder.style.width = '100%';

        //    window.GPSX.SetText(lat);
        //    window.GPSY.SetText(lon);
        //    window.GPSZ.SetText(alt);


        //    var myOptions = {
        //        center: latlon,
        //        zoom: 15,
        //        mapTypeId: window.google.maps.MapTypeId.TERRAIN,
        //        mapTypeControl: false,
        //        navigationControlOptions: { style: window.google.maps.NavigationControlStyle.SMALL }
        //    };
        //    var map = new window.google.maps.Map(document.getElementById("map"), myOptions);
        //    var marker = new window.google.maps.Marker({ position: latlon, map: map, title: "Current Location" });
        //}

        function showError(error) {
            alert(error.code);

        }

        function updateLabel() {
            var reqLabel = window.Navigation.Get("RequestLabel");
            window.lblHeaderText.SetText(reqLabel);
        }

        //function ActiveTabChanged(e) {

        //    var index = window.requestTab.GetActiveTabIndex();

        //    if (index == 2) {
        //        var x = window.GPSX.GetValue();
        //        var y = window.GPSY.GetValue();

        //        if ((x != null) && (y != null)) {
        //            var latlng = new window.google.maps.LatLng(x, y);
        //            var myOptions = {
        //                zoom: 15,
        //                center: latlng,
        //                mapTypeId: window.google.maps.MapTypeId.TERRAIN
        //            };
        //            var map = new window.google.maps.Map(document.getElementById("map"), myOptions);
        //            var marker = new window.google.maps.Marker({ position: latlng, map: map, title: "Current Location" });
        //        } else {

        //            var latlng = new window.google.maps.LatLng(32.8470987, -117.2727893);
        //            var myOptions = {
        //                zoom: 15,
        //                center: latlng,
        //                mapTypeId: window.google.maps.MapTypeId.TERRAIN
        //            };
        //            var map = new window.google.maps.Map(document.getElementById("map"), myOptions);

        //        }
        //    }
    //}
        function OnClickButtonDel(s, e) {
            grid.PerformCallback('Delete');
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHyperLink ID="WorkRequestListBackLink" runat="server" Font-Size="20px" Theme="iOS" Text="WORK REQUEST" NavigateUrl="RequestsList.aspx" />
    <dx:ASPxLabel ID="lblHeader" Font-size="20px" Theme="iOS" runat="server" Text="ADD"></dx:ASPxLabel>
    <dx:ASPxHiddenField ID="Navigation" ViewStateMode="Enabled"  ClientInstanceName="Navigation" runat="server"></dx:ASPxHiddenField>
        
    
    
    <dx:ASPxFormLayout ID="WorkResquestFormLayout" runat="server"
        EnableTheming="True" Theme="iOS" Width="95%"
        Height="400px" ColCount="2" Colspan="2">

        <%-- Description of work--%>
        <Items>
            <dx:LayoutItem Caption="Description of Work" RowSpan="2"
                Width="95%" CaptionSettings-Location="Top" ColSpan="2">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="txtWorkDescription" runat="server"
                            Height="100px" Width="100%" ClientInstanceName="txtWorkDescription"
                            Theme="iOS" AutoPostBack="false" MaxLength="254" >
                            <ValidationSettings SetFocusOnError="true" Display="Dynamic"
                                ErrorDisplayMode="Text">
                                <RequiredField IsRequired="true" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>

        <CaptionSettings Location="Top"></CaptionSettings>
            </dx:LayoutItem>
            <dx:LayoutGroup ColSpan="2" GroupBoxDecoration="Box"
                Caption="" ColCount="2">
                <CellStyle>
                    <Paddings PaddingTop="10px" PaddingBottom="10px"></Paddings>
                </CellStyle>
                <Items>
                        <%--Object Combo Box--%>
                        <dx:LayoutItem Caption="Object/Asset:" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ObjectIDCombo" TextFormatString="{0}" runat="server" EnableCallbackMode="true" CallbackPageSize="10" ValueType="System.String" ValueField="n_objectid" OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" Width="90%" DropDownStyle="DropDown" Theme="iOS" TextField="objectid" DropDownButton-Enabled="true" AutoPostBack="false" ClientInstanceName="ObjectIDCombo">
                                        <ClientSideEvents ValueChanged="function(s, e) { 
                                            var objectHasValue = ObjectIDCombo.GetValue();
                                                                                                    var selectedItem = s.GetSelectedItem();
                                                                                                    if(objectHasValue!=null)
                                                                                                    {
                                                                                                        txtObjectDescription.SetText(selectedItem.GetColumnText('description'));
                                                                                                        ObjectIDComboText.SetText(selectedItem.GetColumnText('objectid'));
                                                                                                        txtObjectArea.SetText(selectedItem.GetColumnText('areaid'));
                                                                                                        txtObjectLocation.SetText(selectedItem.GetColumnText('locationid'));
                                                                                                        txtObjectAssetNumber.SetText(selectedItem.GetColumnText('assetnumber'));
                                                                                                        objectImg.SetImageUrl(selectedItem.GetColumnText('LocationOrURL'));
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        txtObjectDescription.SetText('');
                                                                                                        txtObjectArea.SetText('');
                                                                                                        txtObjectLocation.SetText('');
                                                                                                        txtObjectAssetNumber.SetText('');
                                                                                                    }
                                             }"  />
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="n_objectid" Visible="False" />
                                            <dx:ListBoxColumn FieldName="objectid" Caption="Object ID" Width="150px" ToolTip="M-PET.NET Maintenance Object ID"/>
                                            <dx:ListBoxColumn FieldName="description" Caption="Description" Width="250px" ToolTip="M-PET.NET Maintenance Object Description"/>
                                            <dx:ListBoxColumn FieldName="areaid" Caption="Area ID" Width="75px" ToolTip="M-PET.NET Maintenance Object Assigned Area ID" />
                                            <dx:ListBoxColumn FieldName="locationid" Caption="Location ID" Width="75px" ToolTip="M-PET.NET Maintenance Object Assigned Location ID" />
                                            <dx:ListBoxColumn FieldName="assetnumber" Caption="Asset #" Width="50px" ToolTip="M-PET.NET Maintenance Object Asset Number"/>
                                            <dx:ListBoxColumn FieldName="Following" Caption="Following" Width="50px" ToolTip="M-PET.NET Maintenance Object Following Yes/No?"/>
                                            <dx:ListBoxColumn FieldName="LocationOrURL" Caption="Photo" Width="50px" ToolTip="M-PET.NET Maintenance Object Photo"/>
                                            <dx:ListBoxColumn FieldName="OrganizationCodeID" Caption="Org. Code ID" Width="100px" ToolTip="M-PET.NET Maintenance Object Assigned Org. Code ID" />
                                            <dx:ListBoxColumn FieldName="FundingGroupCodeID" Caption="Fund. Group Code ID" Width="100px" ToolTip="M-PET.NET Maintenance Object Assigned Funding Group Code ID" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                        </dx:LayoutItem>
                        <%--Object Description--%>
                        <dx:LayoutItem Caption="Object Description" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtObjectDescription" runat="server" ClientInstanceName="txtObjectDescription" Width="90%" Height="100px" ReadOnly="false"  Theme="iOS">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Priority:" HelpText="" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxComboBox runat="server" DropDownStyle="DropDown"
                                        CallbackPageSize="10" EnableCallbackMode="True"
                                        TextField="priorityid" ValueField="n_priorityid"
                                        TextFormatString="{0} - {1}" ClientInstanceName="ComboPriority"
                                        Theme="iOS" ID="ComboPriority" OnItemsRequestedByFilterCondition="ComboPriority_OnItemsRequestedByFilterCondition_SQL"
                                        OnItemRequestedByValue="ComboPriority_OnItemRequestedByValue_SQL" Width="90%">
                                        
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="n_priorityid" Visible="False">
                                            </dx:ListBoxColumn>
                                            <dx:ListBoxColumn FieldName="priorityid" Width="75px"
                                                Caption="Priority ID" ToolTip="M-PET.NET Priority ID">
                                            </dx:ListBoxColumn>
                                            <dx:ListBoxColumn FieldName="description" Width="150px"
                                                Caption="Description" ToolTip="M-PET.NET Priority Description">
                                            </dx:ListBoxColumn>
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionSettings Location="Top"></CaptionSettings>
                        </dx:LayoutItem>
                        <%--Work Request Priority--%>
                        <dx:LayoutItem Caption="Reason:" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ComboReason" runat="server" EnableCallbackMode="true"
                                        CallbackPageSize="10" ValueType="System.String"
                                        ValueField="n_reasonid" OnItemsRequestedByFilterCondition="comboReason_OnItemsRequestedByFilterCondition_SQL"
                                        OnItemRequestedByValue="comboReason_OnItemRequestedByValue_SQL"
                                        TextFormatString="{0} - {1}" Width="90%" DropDownStyle="DropDown"
                                        Theme="iOS" TextField="reasonid" DropDownButton-Enabled="true"
                                        AutoPostBack="false" ClientInstanceName="comboReason">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="n_reasonid" Visible="False" />
                                            <dx:ListBoxColumn FieldName="reasonid" Caption="Reason ID"
                                                Width="75px" ToolTip="M-PET.NET Reason ID" />
                                            <dx:ListBoxColumn FieldName="description" Caption="Description"
                                                Width="150px" ToolTip="M-PET.NET Reason Description" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionSettings Location="Top"></CaptionSettings>
                        </dx:LayoutItem>
                        <%-- Work Request Reason--%>
                        <dx:LayoutItem Caption="" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxImage runat="server" ImageUrl="~/Content/Images/noImage.png"
                                        AlternateText="No Picture Associated" ShowLoadingImage="True"
                                        ImageAlign="Left" Width="200px" ClientInstanceName="objectImg"
                                        Theme="Mulberry" ID="objectImg"></dx:ASPxImage>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>

                        <CaptionSettings Location="Top"></CaptionSettings>
                       
                            <%--Work Request Requester--%>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Requester" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ComboRequestor" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="UserID" 
                                                                                                 OnItemsRequestedByFilterCondition="ComboRequestor_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboRequestor_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="90%" DropDownStyle="DropDown" Theme="Mulberry" TextField="Username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboRequestor">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="Username" Caption="Username" Width="75px" ToolTip="M-PET.NET User's Username"/>
                                                                                        <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User's Full Name"/>
                                                                                    </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                <SettingsItems VerticalAlign="Top"></SettingsItems>
            </dx:LayoutGroup>            
             <dx:LayoutItem Caption="" HelpText="Save work Request allow attachments" HelpTextSettings-Position="Bottom" HelpTextStyle-Font-Italic="true" HelpTextStyle-ForeColor="LightGray" >
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxUploadControl runat="server" ID="UploadControl" Theme="iOS" ClientInstanceName="UploadControl" Width="95%" UploadMode="Auto" UploadStorage="Azure" FileUploadMode="OnPageLoad" ShowUploadButton="true" ShowProgressPanel="true" OnFileUploadComplete="UploadControl_FileUploadComplete" ShowAddRemoveButtons="true">
                                <AdvancedModeSettings EnableDragAndDrop="true" EnableFileList="false" EnableMultiSelect="true">
                                    <FileListItemStyle CssClass="pending dxucFileListItem">
                                    </FileListItemStyle>
                                </AdvancedModeSettings>
                                <ValidationSettings MaxFileSize="4194304"  AllowedFileExtensions=".jpg,.jpeg,.gif,.png">
                                </ValidationSettings>
                                <ClientSideEvents FileUploadComplete="onFileUploadComplete"/>
                            </dx:ASPxUploadControl>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>

<HelpTextSettings Position="Bottom"></HelpTextSettings>

<HelpTextStyle Font-Italic="True" ForeColor="LightGray"></HelpTextStyle>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="" ShowCaption="False" ColSpan="2" CaptionSettings-Location="Top">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <dx:ASPxGridView SettingsDataSecurity-AllowDelete="true" runat="server" Id="AttachmentGrid" Theme="iOS" KeyFieldName="LocationOrURL" Width="95%" KeyboardSupport="true" ClientInstanceName="AttachmentGrid" AutoPostBack="true" Settings-HorizontalScrollBarMode="Auto" SettingsPager-Mode="ShowPager" SettingsBehavior-ProcessFocusedRowChangedOnServer="true" SettingsBehavior-AllowFocusedRow="true" EnableCallBacks="true" AutoGenerateColumns="false" DataSourceID="AttachmentDataSource">
                                        <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"                                                                                                   RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                            <Header CssClass="gridViewHeader">
                                            </Header>
                                            <Row CssClass="gridViewRow">
                                            </Row>
                                            <RowHotTrack CssClass="gridViewRow">
                                            </RowHotTrack>
                                            <FocusedRow CssClass="gridViewRowFocused">
                                            </FocusedRow>
                                            <FilterRow CssClass="gridViewFilterRow">
                                            </FilterRow>
                                        </Styles>
                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" ShowDeleteButton="true" ShowSelectCheckbox="true">
                                                <FooterTemplate>
                                                    <dx:ASPxButton ID="buttonDel" AutoPostBack="false" runat="server" Text="Delete">
                                                        <ClientSideEvents Click="onClickButtonDel()" />
                                                    </dx:ASPxButton>
                                                </FooterTemplate>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="nJobID" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="nJobstepID" ReadOnly="True" Visible="false" VisibleIndex="2">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ShortName" Caption="Name" Width="200px" VisibleIndex="3">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="DocType" Caption="Name" Width="100px" VisibleIndex="4">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="Description" Visible="false" Caption="Description" Width="400px" VisibleIndex="5">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataHyperLinkColumn FieldName="LocationOrURL" Caption="Location/URL" Width="600px" VisibleIndex="6">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                                <PropertiesHyperLinkEdit >
                                                </PropertiesHyperLinkEdit>
                                            </dx:GridViewDataHyperLinkColumn>
                                        </Columns>
                                        <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
                                        <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350" />
                                        <SettingsPager PageSize="10">
                                            <PageSizeItemSettings Visible="true" />
                                        </SettingsPager>
                                    </dx:ASPxGridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                </dx:LayoutItem>                
        </Items>
    </dx:ASPxFormLayout>
    <asp:SqlDataSource ID="AttachmentDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" 
        SelectCommand="SELECT [ID], [nJobID], [nJobstepID], [DocType], [Description], [LocationOrURL], [ShortName] 
        FROM [Attachments] 
        WHERE (([nJobID] = @nJobID) AND ([nJobstepID] = @nJobstepID))">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="0" Name="nJobID" SessionField="editingJobID" Type="Int32" />
            <asp:Parameter DefaultValue="-1" Name="nJobstepID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ObjectDataSource" runat="server" />
    <asp:SqlDataSource ID="HwyRouteSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="CostCodeSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="FundSourceSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="WorkOrderSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="WorkOpSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="OrgCodeSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="FundGroupSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="CtlSectionSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="EquipNumSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="AreaSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="MilePostDirSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="PrioritySqlDatasource" runat="server" />
    <asp:SqlDataSource ID="ReasonSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="RequestorSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="RouteToSqlDatasource" runat="server" />
  </asp:Content>

