<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestsOld.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages_WorkRequests_RequestsOld" %>
<%--<%@ Import Namespace="DevExpress.Snap.Core" %>--%>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<%@ Register Src="~/UserControls/UploadedFilesContainer.ascx" TagName="UploadedFilesContainer" TagPrefix="dx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Requests</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <script src="http://maps.google.com/maps/api/js?sensor=false"></script>  
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
                FilesRoundPanel.SetContentHtml(contentHtml);
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
                DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
            }
        }

        function getLocation() {

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                showError("Geolocation is not supported by this browser.");
            }
        }

        function showPosition(position) {

            lat = position.coords.latitude;
            lon = position.coords.longitude;
            alt = position.coords.altitude;
            latlon = new google.maps.LatLng(lat, lon);
            mapholder = document.getElementById('map');
            mapholder.style.height = '300px';
            mapholder.style.width = '100%';

            GPSX.SetText(lat);
            GPSY.SetText(lon);
            GPSZ.SetText(alt);

            var myOptions = {
                center: latlon,
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                mapTypeControl: false,
                navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL }
            };
            var map = new google.maps.Map(document.getElementById("map"), myOptions);
            var marker = new google.maps.Marker({ position: latlon, map: map, title: "Current Location" });
        }

        function showError(error) {
            alert(error.code);

        }

        function updateLabel() {
            var reqLabel = Navigation.Get("RequestLabel");
            lblHeaderText.SetText(reqLabel);
        }

        function ActiveTabChanged(e) {
            
            var index = requestTab.GetActiveTabIndex();

            if (index == 2) {
                var x = GPSX.GetValue();
                var y = GPSY.GetValue();

                if ((x != null) && (y != null)) {
                    var latlng = new google.maps.LatLng(x, y);
                    var myOptions = {
                        zoom: 15,
                        center: latlng,
                        mapTypeId: google.maps.MapTypeId.TERRAIN
                    };
                    var map = new google.maps.Map(document.getElementById("map"), myOptions);
                    var marker = new google.maps.Marker({ position: latlng, map: map, title: "Current Location" });
                } else {

                    var latlng = new google.maps.LatLng(32.8470987, -117.2727893);
                    var myOptions = {
                        zoom: 15,
                        center: latlng,
                        mapTypeId: google.maps.MapTypeId.TERRAIN
                    };
                    var map = new google.maps.Map(document.getElementById("map"), myOptions);

                }
            }
        }

    </script>
    <dx:ASPxLabel ID="lblHeader" Font-size="20px" Theme="Mulberry" runat="server" Text="NEW REQUEST"></dx:ASPxLabel>
    <dx:ASPxHiddenField ID="Navigation" ViewStateMode="Enabled"  ClientInstanceName="Navigation" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxFormLayout ID="WorkRequestDescLayout" runat="server" Width="98%" Paddings="0,0" RequiredMarkDisplayMode="RequiredOnly" RequiredMark="" EnableViewState="True" >
        <Items>
            <dx:LayoutGroup Caption="Description Of Work" Width="98%" ColCount="2" SettingsItemCaptions-Location="Top" GroupBoxDecoration="HeadingLine">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" HelpText="Please Enter Additional Details Below">
                        <LayoutItemNestedControlCollection >
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo ID="txtWorkDescription" Height="50px" Width="98%" MaxLength="254"
                                             ClientInstanceName="txtWorkDescription"
                                             runat="server"  Theme="Mulberry" >
                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                        <RequiredField IsRequired="True"/>
                                    </ValidationSettings>
                                </dx:ASPxMemo>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>  
    <dx:ASPxFormLayout ID="WorkReqDetailLayout" runat="server" Width="98%" Paddings="0,0" RequiredMarkDisplayMode="RequiredOnly" RequiredMark="" EnableViewState="True" >
        <Items>
            <dx:LayoutGroup Caption="" Width="98%" ColCount="2" SettingsItemCaptions-Location="Top" GroupBoxDecoration="None">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" HelpText="">
                        <LayoutItemNestedControlCollection >
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxPageControl ID="requestTab" Enabled="False" Height="500px" Width="98%" ClientInstanceName="requestTab" TabPosition="Right" Theme="Mulberry" runat="server" ActiveTabIndex="0" EnableHierarchyRecreation="True">
                                    <ClientSideEvents
                                        ActiveTabChanged="function(s, e) { ActiveTabChanged(e); }"
                                        />
                                    <TabPages>                                      
                                        <dx:TabPage Text="OBJECT/ASSET">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl1" runat="server">
                                                    <dx:ASPxFormLayout ID="ASPxFormLayout1" ColCount="2" runat="server">
                                                        <Items>
                                                            <dx:LayoutItem Caption="" CaptionSettings-Location="Top" >
                                                                <LayoutItemNestedControlCollection >
                                                                    <dx:LayoutItemNestedControlContainer>
                                                                        <dx:ASPxFormLayout ID="WorkRequestObjectLayout" runat="server">
                                                                            <Items>
                                                                                <dx:LayoutItem Caption="Object/Asset:" CaptionSettings-Location="Top" >
                                                                                    <LayoutItemNestedControlCollection >
                                                                                        <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxComboBox 
                                                                                                ID="ObjectIDCombo" 
                                                                                                runat="server" 
                                                                                                EnableCallbackMode="true" 
                                                                                                CallbackPageSize="10" 
                                                                                                ValueType="System.String" 
                                                                                                ValueField="n_objectid" 
                                                                                                OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" 
                                                                                                OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" 
                                                                                                TextFormatString="{0} - {1} - {2} - {3} - {4}" 
                                                                                                Width="600px" 
                                                                                                DropDownStyle="DropDown" 
                                                                                                Theme="Mulberry" 
                                                                                                TextField="objectid" 
                                                                                                DropDownButton-Enabled="True" 
                                                                                                AutoPostBack="False" 
                                                                                                ClientInstanceName="ObjectIDCombo">
                                                                                                <ClientSideEvents ValueChanged="function(s, e) {
                                                                                                    var objectHasValue = ObjectIDCombo.GetValue();
                                                                                                    var selectedItem = s.GetSelectedItem();
                                                                                                    if(objectHasValue!=null)
                                                                                                    {
                                                                                                        txtObjectDescription.SetText(selectedItem.GetColumnText('description'));
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

}" />
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn FieldName="n_objectid" Visible="False" />
                                                                                                    <dx:ListBoxColumn FieldName="objectid" Caption="Object ID" Width="150px" ToolTip="M-PET.NET Maintenance Object ID"/>
                                                                                                    <dx:ListBoxColumn FieldName="description" Caption="Description" Width="250px" ToolTip="M-PET.NET Maintenance Object Description"/>
                                                                                                    <dx:ListBoxColumn FieldName="areaid" Caption="Area ID" Width="75px" ToolTip="M-PET.NET Maintenance Object Assigned Area ID" />
                                                                                                    <dx:ListBoxColumn FieldName="locationid" Caption="Location ID" Width="75px" ToolTip="M-PET.NET Maintenance Object Assigned Location ID" />
                                                                                                    <dx:ListBoxColumn FieldName="assetnumber" Caption="Asset #" Width="50px" ToolTip="M-PET.NET Maintenance Object Asset Number"/>
                                                                                                    <dx:ListBoxColumn FieldName="Following" Caption="Following" Width="50px" ToolTip="M-PET.NET Maintenance Object Following Yes/No?"/>
                                                                                                    <dx:ListBoxColumn FieldName="LocationOrURL" Caption="Photo" Width="50px" ToolTip="M-PET.NET Maintenance Object Photo"/>
                                                                                                </Columns>
                                                                                            </dx:ASPxComboBox>                              
                                                                                        </dx:LayoutItemNestedControlContainer>
                                                                                    </LayoutItemNestedControlCollection>
                                                                                    <CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem>
                                                                                <dx:LayoutItem Caption="Description:" CaptionSettings-Location="Top">
                                                                                    <LayoutItemNestedControlCollection>
                                                                                        <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxMemo ID="txtObjectDescription" Height="100px" Width="600px" MaxLength="254"
                                                                                                         ClientInstanceName="txtObjectDescription"
                                                                                                         runat="server" ReadOnly="True"  Theme="Mulberry" >
                                                                                            </dx:ASPxMemo>
                                                                                        </dx:LayoutItemNestedControlContainer>
                                                                                    </LayoutItemNestedControlCollection>

                                                                                    <CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem>
                               
                                                                    
                                                                                <dx:LayoutItem Caption="Area:" CaptionSettings-Location="Top">
                                                                                    <LayoutItemNestedControlCollection>
                                                                                        <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxTextBox ID="txtObjectArea" 
                                                                                                            ClientInstanceName="txtObjectArea"
                                                                                                            runat="server" Theme="Mulberry" ReadOnly="True" Width="600px">
                                                                                            </dx:ASPxTextBox>
                                                                                        </dx:LayoutItemNestedControlContainer>
                                                                                    </LayoutItemNestedControlCollection>

                                                                                    <CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem>

                                                                                <dx:LayoutItem Caption="Location:" CaptionSettings-Location="Top">
                                                                                    <LayoutItemNestedControlCollection>
                                                                                        <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxTextBox ID="txtObjectLocation" 
                                                                                                            ClientInstanceName="txtObjectLocation"
                                                                                                            runat="server" Theme="Mulberry" ReadOnly="True" Width="600px">
                                                                                            </dx:ASPxTextBox>
                                                                                        </dx:LayoutItemNestedControlContainer>
                                                                                    </LayoutItemNestedControlCollection>

                                                                                    <CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem>
                                                        
                                                                                <dx:LayoutItem Caption="Asset #:" CaptionSettings-Location="Top">
                                                                                    <LayoutItemNestedControlCollection>
                                                                                        <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxTextBox ID="txtObjectAssetNumber" 
                                                                                                            ClientInstanceName="txtObjectAssetNumber"
                                                                                                            runat="server" Theme="Mulberry" ReadOnly="True" Width="600px">
                                                                                            </dx:ASPxTextBox>
                                                                                        </dx:LayoutItemNestedControlContainer>
                                                                                    </LayoutItemNestedControlCollection>

                                                                                    <CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem>
                                                                            </Items>
                                                                        </dx:ASPxFormLayout>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                                <CaptionSettings Location="Top"></CaptionSettings>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="" HelpText="" CaptionSettings-Location="Top">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer>
                                                                        <dx:ASPxImage ID="objectImg" ImageAlign="Left" ImageUrl="~/Content/Images/noImage.png" AlternateText="No Picture Associated" Theme="Mulberry" Width="200px" ClientInstanceName="objectImg" runat="server" ShowLoadingImage="true"></dx:ASPxImage>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>                                
                                                        </Items>
                                                    </dx:ASPxFormLayout>
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                        <dx:TabPage Text="ORIGINATOR">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl2" runat="server">
                                                    <dx:ASPxFormLayout ID="WorkReqeustRequestorInfoLayout" runat="server" Font-Size="Medium" >
                                                        <Items>
                                                            <dx:LayoutGroup Caption="" ColCount="2" GroupBoxDecoration="None" >
                                                                <Items>
                                                                    <dx:LayoutItem Caption="Request Date:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxDateEdit ID="TxtWorkRequestDate" 
                                                                                                 ClientInstanceName="TxtWorkRequestDate"
                                                                                                 Theme="Mulberry"
                                                                                                 Width="400px" 
                                                                                                 runat="server">
                                                                                </dx:ASPxDateEdit>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>

                                                                    <dx:LayoutItem Caption="Requestor:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboRequestor" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="UserID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboRequestor_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboRequestor_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="Username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboRequestor">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="Username" Caption="Username" Width="75px" ToolTip="M-PET.NET User's Username"/>
                                                                                        <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User's Full Name"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                                            
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                    
                                                                    <dx:LayoutItem Caption="Priority:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboPriority" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_priorityid"
                                                                                                 OnItemsRequestedByFilterCondition="ComboPriority_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboPriority_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="priorityid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboPriority">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_priorityid" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="priorityid" Caption="Priority ID" Width="75px" ToolTip="M-PET.NET Priority ID"/>
                                                                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Priority Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                                            
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>

                                                                    <dx:LayoutItem Caption="Reason:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="comboReason" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_reasonid"
                                                                                                 OnItemsRequestedByFilterCondition="comboReason_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="comboReason_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="reasonid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboReason">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_reasonid" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="reasonid" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Reason ID"/>
                                                                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Reason Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                                            
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                        
                                                                    <dx:LayoutItem Caption="Route To:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="comboRouteTo" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="UserID"
                                                                                                 OnItemsRequestedByFilterCondition="comboRouteTo_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="comboRouteTo_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="Username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboRouteTo">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="Username" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Route To ID"/>
                                                                                        <dx:ListBoxColumn FieldName="FullName" Caption="Description" Width="150px" ToolTip="M-PET.NET Route To User's Full Name"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                                            
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                
                                                                    <dx:LayoutItem Name="fldHwyRoute" Caption="Hwy. Route:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="comboHwyRoute" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_StateRouteID"
                                                                                                 OnItemsRequestedByFilterCondition="comboHwyRoute_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="comboHwyRoute_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="StateRouteID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboHwyRoute">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_StateRouteID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="StateRouteID" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Highway Route ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Highway Route Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                                            
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                
                                                                    <dx:LayoutItem Caption="Milepost:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtMilepost" 
                                                                                                ClientInstanceName="txtMilepost"
                                                                                                Theme="Mulberry"
                                                                                                Width="400px" 
                                                                                                runat="server">
                                                                                    <MaskSettings Mask="<0..99999g>.<0000..9999>" IncludeLiterals="DecimalSymbol" />
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                
                                                                    <dx:LayoutItem Caption="Milepost Dir:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="comboMilePostDir" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_MilePostDirectionID"
                                                                                                 OnItemsRequestedByFilterCondition="comboMilePostDir_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="comboMilePostDir_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="MilePostDirectionID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboMilePostDir">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_MilePostDirectionID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="MilePostDirectionID" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Mile Post Direction ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Mile Post Direction Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                                            
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                </Items>
                                                            </dx:LayoutGroup>
                                                        </Items>
                                                    </dx:ASPxFormLayout>                            
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                        <dx:TabPage Text="GPS">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl3" runat="server">
                                                    <div id="map" style="height: 300px; width: 98%;"></div>
                                                    <dx:ASPxButton ID="ASPxButton2" VerticalAlign="Middle" ClientInstanceName="refreshButton"
                                                                   runat="server" AutoPostBack="False" Text="Show Location">
                                                        <Image IconID="actions_show_16x16gray">
                                                        </Image>
                                                        <ClientSideEvents Click="getLocation" />
                                                    </dx:ASPxButton>
                                                    <dx:ASPxFormLayout ID="GPSLayout" runat="server" Width="98%" Paddings="0,0">
                                                        <Items>
                                                            <dx:LayoutGroup Caption="Current Coordinates" ColCount="3" Width="100%" GroupBoxDecoration="HeadingLine">
                                                                <Items>
                                                                    <dx:LayoutItem Caption="GPS X:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="GPSX"  Width="200" MaxLength="8000"
                                                                                                ClientInstanceName="GPSX"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="GPS Y" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="GPSY"  Width="200" MaxLength="8000"
                                                                                                ClientInstanceName="GPSY"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="GPS Z" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="GPSZ"  Width="200" MaxLength="8000"
                                                                                                ClientInstanceName="GPSZ"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                </Items>
                                                            </dx:LayoutGroup>
                                                        </Items>
                                                    </dx:ASPxFormLayout>                           
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                        <dx:TabPage Text="COSTING">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl4" runat="server">
                                                    <dx:ASPxFormLayout ID="WRCFL" runat="server">
                                                        <Items>
                                                            <dx:LayoutGroup Caption="" ColCount="2" Width="100%" GroupBoxDecoration="None">
                                                                <Items>
                                                                    <dx:LayoutItem Name="fldCostCode" Caption="Cost Code:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboCostCode" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_costcodeid"
                                                                                                 OnItemsRequestedByFilterCondition="ComboCostCode_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboCostCode_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="CostCodeID" DropDownButton-Enabled="True" ClientInstanceName="ComboCostCode">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_costcodeid" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="CostCodeID" Caption="Cost Code ID" Width="75px" ToolTip="M-PET.NET Cost Code ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Cost Code Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                
                                                                    <dx:LayoutItem Name="fldFundSrc" Caption="Fund Source:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboFundSource" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_FundSrcCodeID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboFundSource_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboFundSource_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="FundSrcCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboFundSource">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_FundSrcCodeID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="FundSrcCodeID" Caption="Cost Code ID" Width="75px" ToolTip="M-PET.NET Fund Source ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Fund Source Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem> 
                                                                    <dx:LayoutItem Name="fldWorkOrder" Caption="Work Order Code:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboWorkOrder" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_WorkOrderCodeID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboWorkOrder_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboWorkOrder_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="WorkOrderCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboWorkOrder">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_WorkOrderCodeID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="WorkOrderCodeID" Caption="Work Order ID" Width="75px" ToolTip="M-PET.NET Work Order Code ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Work Order Code Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>
                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                    <dx:LayoutItem Name="fldWorkOp" Caption="Work Op:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboWorkOp" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_WorkOpID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboWorkOp_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboWorkOp_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="WorkOpID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboWorkOp">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_WorkOpID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="WorkOpID" Caption="Work Op. ID" Width="75px" ToolTip="M-PET.NET Work Operation ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Work Operation Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>
                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                    <dx:LayoutItem Name="fldOrgCode" Caption="Org. Code:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboOrgCode" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_OrganizationCodeID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboOrgCode_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboOrgCode_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="OrganizationCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboOrgCode">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_OrganizationCodeID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="OrganizationCodeID" Caption="Org. Code ID" Width="75px" ToolTip="M-PET.NET Organization Code ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Organization Code Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>
                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                    <dx:LayoutItem Name="fldFundGrp" Caption="Fund. Group:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboFundGroup" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_FundingGroupCodeID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboFundGroup_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboFundGroup_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="FundingGroupCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboFundGroup">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_FundingGroupCodeID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="FundingGroupCodeID" Caption="Fund. Group ID" Width="75px" ToolTip="M-PET.NET Funding Group ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Funding Group Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>
                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem> 
                                                                    <dx:LayoutItem Name="fldCtlSection" Caption="Control Section:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboCtlSection" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_ControlSectionID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboCtlSection_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboCtlSection_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="ControlSectionID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboCtlSection">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_ControlSectionID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="ControlSectionID" Caption="Ctl. Section" Width="75px" ToolTip="M-PET.NET Control Section ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Control Section Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>
                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                    <dx:LayoutItem Name="fldEquipNum" Caption="Equipment Number:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboEquipNum" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_EquipmentNumberID"
                                                                                                 OnItemsRequestedByFilterCondition="ComboEquipNum_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboEquipNum_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="EquipmentNumberID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboEquipNum">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_EquipmentNumberID" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="EquipmentNumberID" Caption="Equip. #" Width="75px" ToolTip="M-PET.NET Equipment Number ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Equipment Number /Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>
                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                </Items>
                                                            </dx:LayoutGroup>
                                                        </Items>
                                                    </dx:ASPxFormLayout>                            
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                
                                        <dx:TabPage Text="FACILITY">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl5" runat="server">
                                                    <dx:ASPxFormLayout ID="WorkReqFrmLayout" Width="98%" runat="server" Font-Size="Medium" >
                                                        <Items>
                                                            <dx:LayoutGroup Caption="" ColCount="2" Width="100%" GroupBoxDecoration="None">
                                                                <Items>
                                                                    <dx:LayoutItem Caption="First Name:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtFN" Width="450" MaxLength="100"
                                                                                                ClientInstanceName="txtFN"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Last Name:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtLN" Width="450" MaxLength="100"
                                                                                                ClientInstanceName="txtLN"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Email Address:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtEmail" Width="450" MaxLength="254"
                                                                                                ClientInstanceName="txtEmail"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Phone Number:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtPhone" Width="450"
                                                                                                ClientInstanceName="txtPhone"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                    <MaskSettings Mask="+1 (999) 000-0000" IncludeLiterals="None" />
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Ext:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtExt" Width="450" MaxLength="10"
                                                                                                ClientInstanceName="txtExt"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Mail Code:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtMail" Width="450" MaxLength="10"
                                                                                                ClientInstanceName="txtMail"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                        
                                                                    <dx:LayoutItem Caption="Service Office:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection>
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxComboBox ID="ComboServiceOffice" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                 ValueType="System.String" ValueField="n_areaid"
                                                                                                 OnItemsRequestedByFilterCondition="ComboServiceOffice_OnItemsRequestedByFilterCondition_SQL"
                                                                                                 OnItemRequestedByValue="ComboServiceOffice_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                 Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="AreaID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboServiceOffice">
                                                                                               
                                                                                    <Columns>
                                                                                        <dx:ListBoxColumn FieldName="n_areaid" Visible="False" />
                                                                                        <dx:ListBoxColumn FieldName="areaid" Caption="Area ID" Width="75px" ToolTip="M-PET.NET Area ID"/>
                                                                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Area Description"/>
                                                                                    </Columns>
                                                                                </dx:ASPxComboBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Building:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtBuilding" Width="450" MaxLength="10"
                                                                                                ClientInstanceName="txtBuilding"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                                                            
                                                                    <dx:LayoutItem Caption="Room #:" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxTextBox ID="txtRoomNum" Width="450" MaxLength="10"
                                                                                                ClientInstanceName="txtRoomNum"
                                                                                                runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxTextBox>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                </Items>
                                                            </dx:LayoutGroup>
                                                        </Items>
                                                    </dx:ASPxFormLayout>                              
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                
                                        <dx:TabPage Text="ATTACHMENTS">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl6" runat="server">
                                                    <div id="PhotoContainer" runat="server">
                                                        <div class="uploadContainer">
                                                            <dx:ASPxUploadControl 
                                                                ID="UploadControl" 
                                                                runat="server" 
                                                                ClientInstanceName="UploadControl" 
                                                                Width="98%" 
                                                                UploadMode="Auto" 
                                                                UploadStorage="Azure" 
                                                                FileUploadMode="OnPageLoad" 
                                                                ShowUploadButton="True" 
                                                                ShowProgressPanel="True" 
                                                                OnFileUploadComplete="UploadControl_FileUploadComplete" 
                                                                ShowAddRemoveButtons="True">
                                                                <AzureSettings 
                                                                    StorageAccountName="aspdemo" 
                                                                    ContainerName="uploadcontroldemo"/>
                                                        
                                                                <BrowseButton Text="Select Attachment(s) To Upload..."></BrowseButton>
                                                                <AdvancedModeSettings 
                                                                    EnableDragAndDrop="True" 
                                                                    EnableFileList="False" 
                                                                    EnableMultiSelect="False">
                                                                    <FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
                                                                </AdvancedModeSettings>
                                                                <ValidationSettings 
                                                                    MaxFileSize="4194304" 
                                                                    AllowedFileExtensions=".jpg,.jpeg,.gif,.png">
                                                                </ValidationSettings>
                                                                <ClientSideEvents 
                                                                    FileUploadStart="function(s, e) { DXUploadedFilesContainer.Clear(); }" 
                                                                    FileUploadComplete="onFileUploadComplete"/>
                                                            </dx:ASPxUploadControl>
                                                        </div>
                                                        <div>
                                                            <dx:UploadedFilesContainer ID="FileContainer" runat="server" Width="400" Height="180" 
                                                                                       NameColumnWidth="240" SizeColumnWidth="70" HeaderText="Uploaded Files" UseExtendedPopup="True" />
                                                        </div>
                                                    </div>

                                                    <dx:ASPxUploadControl 
                                                        ID="ASPxUploadControl1" 
                                                        ClientInstanceName="UploadControl" 
                                                        Visible="False"
                                                        runat="server" 
                                                        UploadMode="Auto" 
                                                        AutoStartUpload="True" 
                                                        Width="98%" 
                                                        Theme="Mulberry"
                                                        ShowProgressPanel="True" 
                                                        CssClass="uploadControl" 
                                                        OnFileUploadComplete="UploadControl_FileUploadComplete">
                                                        <AdvancedModeSettings EnableDragAndDrop="True" EnableFileList="False" EnableMultiSelect="True" ExternalDropZoneID="externalDropZone" DropZoneText="" >
                                                            <FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
                                                        </AdvancedModeSettings>
                                                        <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" />
                                                        <BrowseButton Text="Select Attachment(s) To Upload..." />
                                                
                                                        <ProgressBarStyle CssClass="uploadControlProgressBar" />
                                                        <ClientSideEvents 
                                                            FileUploadStart="function(s, e) { DXUploadedFilesContainer.Clear(); }" 
                                                            FileUploadComplete="onFileUploadComplete">
                                                        </ClientSideEvents>
                                                    </dx:ASPxUploadControl>                       
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                        <dx:TabPage Text="NOTES">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl7" runat="server">
                                                    <dx:ASPxFormLayout ID="AdditionalContentLayoutGroup" Theme="Mulberry" runat="server" Width="98%" Paddings="0,0">
                                                        <Items>
                                                            <dx:LayoutGroup Width="100%" GroupBoxDecoration="None">
                                                                <Items>
                                                                    <dx:LayoutItem Caption="" ShowCaption="False" HelpText="Please Enter Any Additional Information" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <dx:ASPxMemo ID="txtAdditionalInfo" Native="True" Height="400px" Width="100%" MaxLength="8000"
                                                                                             ClientInstanceName="txtAdditionalInfo"
                                                                                             runat="server"  Theme="Mulberry" >
                                                                                                                            
                                                                                </dx:ASPxMemo>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>
                                                                </Items>
                                                            </dx:LayoutGroup>
                                                        </Items>
                                                    </dx:ASPxFormLayout>  
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>                                                                                                

                                    </TabPages>
                                </dx:ASPxPageControl>  
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>  
    <%--    

    
    

    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" ClientInstanceName="WorkReqFacPnl" runat="server" LoadContentViaCallback="true"
                       HeaderText="Facility Information" Theme="Mulberry" Visible="true" ClientVisible="True" Collapsed="False" ShowCollapseButton="False"
                       Width="100%" Font-Bold="True" Font-Size="Large" AllowCollapsingByHeaderClick="False">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="400px" Paddings="0,0">
                    <Items>
                        <dx:LayoutGroup Caption="Enter Facility Information" Width="100%" GroupBoxDecoration="HeadingLine">
                            <Items>
                                <dx:LayoutItem Caption="First Name:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox1" Width="400" MaxLength="100"
                                                            ClientInstanceName="txtFN"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Last Name:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox2" Width="400" MaxLength="100"
                                                            ClientInstanceName="txtLN"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Email Address:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox3" Width="400" MaxLength="254"
                                                            ClientInstanceName="txtEmail"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Phone Number:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox4" Width="400"
                                                            ClientInstanceName="txtPhone"
                                                            runat="server"  Theme="Mulberry" >
                                                <MaskSettings Mask="+1 (999) 000-0000" IncludeLiterals="None" />
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Ext:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox5" Width="400" MaxLength="10"
                                                            ClientInstanceName="txtExt"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Mail Code:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox6" Width="400" MaxLength="10"
                                                            ClientInstanceName="txtMail"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                        
                                <dx:LayoutItem Caption="Service Office:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                             ValueType="System.String" ValueField="n_areaid"
                                                             OnItemsRequestedByFilterCondition="ComboServiceOffice_OnItemsRequestedByFilterCondition_SQL"
                                                             OnItemRequestedByValue="ComboServiceOffice_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                             Width="400px" DropDownStyle="DropDown" Theme="Mulberry" TextField="AreaID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboServiceOffice">
                                                                                               
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="n_areaid" Visible="False" />
                                                    <dx:ListBoxColumn FieldName="areaid" Caption="Area ID" Width="75px" ToolTip="M-PET.NET Area ID"/>
                                                    <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Area Description"/>
                                                </Columns>
                                            </dx:ASPxComboBox>
                                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" />
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Building:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox7" Width="400" MaxLength="10"
                                                            ClientInstanceName="txtBuilding"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                                                                                                            
                                <dx:LayoutItem Caption="Room #:" CaptionSettings-Location="Top">
                                    <LayoutItemNestedControlCollection >
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="ASPxTextBox8" Width="400" MaxLength="10"
                                                            ClientInstanceName="txtRoomNum"
                                                            runat="server"  Theme="Mulberry" >
                                                                                                                            
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>

                                    <CaptionSettings Location="Top"></CaptionSettings>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                    </Items>
                </dx:ASPxFormLayout>  
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    --%>
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

<%--<asp:Content ID="BottomContent" runat="server" ContentPlaceHolderID="BottomContentPlaceHolder">
    <ReqDetail:ReqDetail runat="server" ID="RequestDetails" />
    
</asp:Content>--%>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="FooterRangeControlPlaceHolder" runat="Server">
    <div class="contentBox salesDateRangeContainer">
        <uc:FooterRangeControl runat="server" ID="FooterRangeControl" />
    </div>
</asp:Content>--%>

<%--<asp:Content ID="HelpMenu" ContentPlaceHolderID="HelpMenuDescriptionPlaceHolder" runat="server">
    <p><a target="_blank" href="https://documentation.devexpress.com/#AspNet/CustomDocument5823">Grid View</a> - used to display products stored in the database. Sort values by clicking individual column headers.</p>
    <p><a target="_blank" href="https://documentation.devexpress.com/#AspNet/clsDevExpressXtraChartsWebWebChartControltopic">Pie Charts</a> - used to communicate the state of revenues by sector, region and sales channel.</p>
    <p><a target="_blank" href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxEditorsASPxTrackBartopic">Track Bar</a> – used to visually specify a date range for sales information displayed within the charts.</p>
</asp:Content>--%>