<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedJobs.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.PlannedJobs.PlannedJobs" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Planned Jobs</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <%--use this meta tag to stop double click zoom on Ipad--%>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/> 

    <script src="http://maps.google.com/maps/api/js?sensor=false"></script>  
   
     <script type="text/javascript">
        ASPx.TouchUIHelper.isNativeScrollingAllowed = false;
    </script>

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

        function getLocation() {

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                showError("Geolocation is not supported by this browser.");
            }
        }


        function showPosition(position) {

            var lat = position.coords.latitude;
            var lon = position.coords.longitude;
            var alt = position.coords.altitude;
            var latlon = new google.maps.LatLng(lat, lon);
            var mapholder = document.getElementById('map');
            mapholder.style.height = '300px';
            mapholder.style.width = '100%';

            window.GPSX.SetText(lat);
            window.GPSY.SetText(lon);
            window.GPSZ.SetText(alt);


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
            var reqLabel = window.Navigation.Get("RequestLabel");
            window.lblHeaderText.SetText(reqLabel);
        }

        function ActiveTabChanged() {

            var index = window.requestTab.GetActiveTabIndex();

            if (index == 2) {
                var x = window.GPSX.GetValue();
                var y = window.GPSY.GetValue();
                var latlng;
                var myOptions;
                if ((x != null) && (y != null)) {
                    latlng = new window.google.maps.LatLng(x, y);
                    myOptions = {
                        zoom: 15,
                        center: latlng,
                        mapTypeId: window.google.maps.MapTypeId.TERRAIN
                    };
                    var map = new window.google.maps.Map(document.getElementById("map"), myOptions);
                    var marker = new window.google.maps.Marker({ position: latlng, map: map, title: "Current Location" });
                } else {
                    latlng = new window.google.maps.LatLng(32.8470987, -117.2727893);
                    myOptions = {
                        zoom: 15,
                        center: latlng,
                        mapTypeId: window.google.maps.MapTypeId.TERRAIN
                    };
                    var map = new window.google.maps.Map(document.getElementById("map"), myOptions);

                }
            }
        }


        function OnGetCrewRowId(idValue) {
            Selection.Set('RecordID', idValue[0].toString());
            Selection.Set('DMRKEY', idValue[1].toString());
        }

        function OnGetMemberRowId(idValue) {
            Selection.Set('n_JobOtherID', idValue.toString());
        }

        function OnGetPartRowId(idValue) {
            Selection.Set('n_jobpartid', idValue[0].toString());
            Selection.Set('DMRKEY', idValue[1].toString());
            Selection.Set('n_masterpartid', idValue[2].toString());
        }

        function OnGetOtherRowId(idValue) {
            Selection.Set('n_JobOtherID', idValue.toString());
        }

        function OnGetEquipRowId(idValue) {
            Selection.Set('n_JobEquipmentID', idValue[0].toString());
            Selection.Set('DMRKEY', idValue[1].toString());
        }

        function OnRibbonChange(s, e) {
            var tab = e.tab.index;
            window.StepTab.SetActiveTabIndex(tab);
        }

        function HidePopup() {
            window.AddCrewPopup.Hide();
            window.AddEquipPopup.Hide();
            window.AddMemberPopup.Hide();
            window.AddPartPopup.Hide();
            window.CrewLaborClassPopup.Hide();
            window.CrewGroupPopup.Hide();
        }

        function ShowCrewPopup() {
            window.AddCrewPopup.Show();
        }

        function ShowMemberPopup() {
            window.AddMemberPopup.Show();
        }

        function ShowPartPopup() {
            window.AddPartPopup.Show();
        }

        function ShowEquipPopup() {
            window.AddEquipPopup.Show();
        }

        function ShowOtherPopup() {
            window.OtherEditGroup.Show();
        }

        function AddEditOtherRow() {
            window.StartEdit();
        }

        

        function OtherGrid_BatchEditRowValidating(s, e) {
            var otherDescColumn = s.GetColumnByField("OtherDescr");
            var cellValidationInfo = e.validationInfo[otherDescColumn.index];
            if (!cellValidationInfo) return;
            var value = cellValidationInfo.value;
            if (!window.ASPxClientUtils.IsExists(value) || window.ASPxClientUtils.Trim(value) === "") {
                cellValidationInfo.isValid = false;
                cellValidationInfo.errorText = "Other Description Is Required";
            }
        }

        function OnPartUpateClick() {
            window.PartGrid.UpdateEdit();
        }

        function OnPartCancelClick() {
            window.PartGrid.CancelEdit();
        }
        function OnPartDeleteClick() {
            window.PartGrid.DeletEdit();
        }

        function OnEquipUpateClick() {
            window.EquipGrid.UpdateEdit();
        }

        function OnEquipCancelClick() {
            window.EquipGrid.CancelEdit();
        }

        function OnOtherUpateClick() {
            window.OtherGrid.UpdateEdit();
        }

        function OnOtherCancelClick() {
            window.OtherGrid.CancelEdit();
        }

        function OnMemberUpateClick() {
            window.MemberGrid.UpdateEdit();
        }

        function OnMemberCancelClick() {
            window.MemberGrid.CancelEdit();
        }

        function OnCrewUpateClick() {
            window.CrewGrid.UpdateEdit();
        }

        function OnCrewCancelClick() {
            window.CrewGrid.CancelEdit();
        }

        function onHyperLinkClick(sender) {
            
            var s = sender.GetMainElement();
          
            var crewGrid = s.parentNode.parentNode;
            var click = new MouseEvent('dblclick', {
                'view': window,
                'bubbles': true,
                'cancelable': true
            });
            var dblClick = new MouseEvent('click', {
                'view': window,
                'bubbles': true,
                'cancelable': true
            });
            crewGrid.dispatchEvent(click);
            crewGrid.dispatchEvent(dblClick);
        }

      

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHyperLink ID="PlannedJobBackLink" runat="server" Font-size="20px" Theme="Mulberry" Text="PLANNED JOBS" NavigateUrl="~/Pages/PlannedJobs/PlannedJobsList.aspx"/> > <dx:ASPxLabel ID="lblHeader" Font-size="20px" Theme="Mulberry" runat="server" Text="ADD"></dx:ASPxLabel> > <dx:ASPxLabel ID="lblStep" Font-size="20px" Theme="Mulberry" runat="server" Text="Step: "></dx:ASPxLabel> <br />
    <dx:ASPxHyperLink ID="myJobsBackLink" runat="server" Font-Size="16" Theme="Mulberry" Text="MY JOBS" NavigateUrl="~/Pages/PlannedJobs/myJobs.aspx" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="MultiGrid" ViewStateMode="Enabled"  ClientInstanceName="MultiGrid" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxFormLayout ID="WorkRequestDescLayout" runat="server" Width="98%" Paddings="0,0" RequiredMarkDisplayMode="RequiredOnly" RequiredMark="" EnableViewState="True" >
        <Items>
            <dx:LayoutGroup Caption="Description Of Work" Width="98%" ColCount="2" SettingsItemCaptions-Location="Top" GroupBoxDecoration="HeadingLine">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" >
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
        <%--This label is set below the Description box--%>
    
    <dx:ASPxFormLayout ID="WorkReqDetailLayout" runat="server" Width="98%" Paddings="0,0" RequiredMarkDisplayMode="RequiredOnly" RequiredMark="" EnableViewState="True" >
        <Items>
            <dx:LayoutGroup Caption="" Width="98%" ColCount="2" SettingsItemCaptions-Location="Top" GroupBoxDecoration="None">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" HelpText="">
                        <LayoutItemNestedControlCollection >
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxPageControl ID="StepTab" Height="500px" Width="98%" ClientInstanceName="StepTab" TabPosition="Top" Theme="Mulberry" runat="server" ActiveTabIndex="6" EnableHierarchyRecreation="True">
                                    <TabPages>
                                        <dx:TabPage Text="JOB STEP" ToolTip="Allows Input Of Jobstep Details">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl8" runat="server">
                                                    <dx:ASPxPageControl ID="requestTab" Height="500px" Width="98%" ClientInstanceName="requestTab" TabPosition="Right" Theme="Mulberry" runat="server" ActiveTabIndex="0" EnableHierarchyRecreation="True">
                                                        <ClientSideEvents
                                                            ActiveTabChanged="function(s, e) { ActiveTabChanged(e); }"
                                                            />
                                                        <TabPages>                                      
                                                           <%--//Start of side tabs on the Job Step Page--%> 

                                                            <dx:TabPage Text="OBJECT/ASSET" ToolTip="Allows Input Of Jobstep Object/Asset Information">
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="ContentControl1" runat="server">
                                                                        <dx:ASPxFormLayout ID="ASPxFormLayout1" ColCount="2" runat="server">
                                                                            <Items>
                                                                                <dx:LayoutItem Caption="" CaptionSettings-Location="Top" >
                                                                                    <LayoutItemNestedControlCollection >
                                                                                        <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxFormLayout ID="WorkRequestObjectLayout" ColCount="3" runat="server">
                                                                                                <Items>
                                                                                                    <dx:LayoutItem Caption="Object/Asset:" ColSpan="3" CaptionSettings-Location="Top" >
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
                                                                                                    <dx:LayoutItem Caption="Description:" ColSpan="3"  CaptionSettings-Location="Top">
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
                               
                                                                    
                                                                                                    <dx:LayoutItem Caption="Area:" ColSpan="3"  CaptionSettings-Location="Top">
                                                                                                        <LayoutItemNestedControlCollection>
                                                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                                                <dx:ASPxTextBox ID="txtObjectArea" 
                                                                                                                                ClientInstanceName="txtObjectArea"
                                                                                                                                runat="server" Theme="Mulberry" ReadOnly="True" Width="600px">
                                                                                                                </dx:ASPxTextBox>
                                                                                                            </dx:LayoutItemNestedControlContainer>
                                                                                                        </LayoutItemNestedControlCollection>

                                                                                                    </dx:LayoutItem>

                                                                                                    <dx:LayoutItem Caption="Location:" ColSpan="3"  CaptionSettings-Location="Top">
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
                                                        
                                                                                                    <dx:LayoutItem Caption="Asset #:" ColSpan="3" CaptionSettings-Location="Top">
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
                                                                                                    
                                                                                                    <dx:LayoutItem Caption="Unit #1:" CaptionSettings-Location="Top">
                                                                                                        <LayoutItemNestedControlCollection>
                                                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                                                <dx:ASPxSpinEdit  ID="txtRunUnitOne" 
                                                                                                                          ClientInstanceName="txtRunUnitOne"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335"
                                                                                                                          ReadOnly="True"
                                                                                                                          AllowMouseWheel="False"
                                                                                                                          SpinButtons-ClientVisible="False">
<SpinButtons ClientVisible="False"></SpinButtons>

                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                            </dx:LayoutItemNestedControlContainer>
                                                                                                        </LayoutItemNestedControlCollection>

                                                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                                                    </dx:LayoutItem>
                                                                                                    
                                                                                                    <dx:LayoutItem Caption="Unit #2:" CaptionSettings-Location="Top">
                                                                                                        <LayoutItemNestedControlCollection>
                                                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                                                <dx:ASPxSpinEdit  ID="txtRunUnitTwo" 
                                                                                                                          ClientInstanceName="txtRunUnitTwo"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335"
                                                                                                                          ReadOnly="True"
                                                                                                                          AllowMouseWheel="False"
                                                                                                                          SpinButtons-ClientVisible="False">
<SpinButtons ClientVisible="False"></SpinButtons>

                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                            </dx:LayoutItemNestedControlContainer>
                                                                                                        </LayoutItemNestedControlCollection>

                                                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                                                    </dx:LayoutItem>

                                                                                                    <dx:LayoutItem Caption="Unit #3:" CaptionSettings-Location="Top">
                                                                                                        <LayoutItemNestedControlCollection>
                                                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                                                <dx:ASPxSpinEdit  ID="txtRunUnitThree" 
                                                                                                                          ClientInstanceName="txtRunUnitThree"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335"
                                                                                                                          ReadOnly="True"
                                                                                                                          AllowMouseWheel="False"
                                                                                                                          SpinButtons-ClientVisible="False">
<SpinButtons ClientVisible="False"></SpinButtons>

                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
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

                                                                                    <CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem>                                
                                                                            </Items>
                                                                        </dx:ASPxFormLayout>
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--OBJECTS & Assests--%>
                                                            <dx:TabPage Text="ORIGINATOR" ToolTip="Allows Input Of Jobstep Originator Information">
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
                                                                                                                     DisplayFormatString="D"
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
                                                                                    </Items>
                                                                                </dx:LayoutGroup>
                                                                            </Items>
                                                                        </dx:ASPxFormLayout>                            
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--Originators--%>
                                                            <dx:TabPage Text="GPS" ToolTip="Allows Input Of Jobstep GPS Coordinates">
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
                                                                                        <dx:LayoutItem Name="fldHwyRoute" Caption="Hwy. Route:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxComboBox ID="comboHwyRoute" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                                     ValueType="System.String" ValueField="n_StateRouteID"
                                                                                                                     OnItemsRequestedByFilterCondition="comboHwyRoute_OnItemsRequestedByFilterCondition_SQL"
                                                                                                                     OnItemRequestedByValue="comboHwyRoute_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                                     Width="200px" DropDownStyle="DropDown" Theme="Mulberry" TextField="StateRouteID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboHwyRoute">
                                                                                               
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
                                                                                                                    Width="200px" 
                                                                                                                    runat="server">
                                                                                                        <MaskSettings Mask="<0..99999g>.<0000..9999>" IncludeLiterals="DecimalSymbol" />
                                                                                                    </dx:ASPxTextBox>
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>

                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Caption="Milepost To:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxTextBox ID="txtMilepostTo" 
                                                                                                                    ClientInstanceName="txtMilepostTo"
                                                                                                                    Theme="Mulberry"
                                                                                                                    Width="200" 
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
                                                                                                                     Width="200px" DropDownStyle="DropDown" Theme="Mulberry" TextField="MilePostDirectionID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboMilePostDir">
                                                                                               
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
                                                            </dx:TabPage> <%--GPS--%>
                                                            <dx:TabPage Text="COSTING" ToolTip="Allows Input Of Jobstep Costing Information">
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
                                                                                                                     Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="CostCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboCostCode">
                                                                                               
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
                                                            </dx:TabPage> <%--Costing--%>
                                                            <dx:TabPage Text="ESTIMATES" ToolTip="Allows Input Of Jobstep Estimate Costs" Visible="false">
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="PlannedJobEstContent" runat="server">
                                                                    <dx:ASPxFormLayout ID="PlannedJobEstLayout" ColCount="2" runat="server">
                                                                            <Items>
                                                                                        <dx:LayoutItem Name="fldEstLength" Caption="Estimated Length:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldLaborEstimate" Caption="Labor Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldEstDowntime" Caption="Estimated Downtime:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldPartEstimate" Caption="Part(s) Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldProdUnitEstimate" Caption="Production Units:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldEquipEstimate" Caption="Equipment Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldUOM" Caption="U.O.M:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOtherEstimate" Caption="Other Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldManHrsEstimate" Caption="Man Hrs:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOverheadCostEst" Caption="Overhead Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOverheadPct" Caption="Overhead %:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldTotalCost" Caption="Total Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldBlank" Caption="" ShowCaption="False" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldPerUnitEstimate" Caption="Per Unit Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>                                                    
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                    </Items>
                                                                        </dx:ASPxFormLayout>                         
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--Estimates--%>
                                                            <dx:TabPage Text="ACTUALS" ToolTip="Allows Input Of Jobstep Actual Costs" Visible="false">
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="PlannedJobActContent" runat="server">
                                                                        <dx:ASPxFormLayout ID="PlannedJobActLayout" ColCount="2" runat="server">
                                                                                    <Items>
                                                                                        <dx:LayoutItem Name="fldActLength" Caption="Actual Length:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldActEstimate" Caption="Labor Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldActDowntime" Caption="Actual Downtime:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldPartActual" Caption="Part(s) Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldProdUnitActual" Caption="Production Units:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldEquipActual" Caption="Equipment Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldUOM" Caption="U.O.M:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOtherActual" Caption="Other Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldManHrsActual" Caption="Man Hrs:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOverheadCostAct" Caption="Overhead Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOverheadPctAct" Caption="Overhead %:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldTotalCostAct" Caption="Total Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldBlankAct" Caption="" ShowCaption="False" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                                                                                                                 
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldPerUnitAct" Caption="Per Unit Cost:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>                                                    
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings HorizontalAlign="Right" Location="Left"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                    </Items>
                                                                        </dx:ASPxFormLayout>                            
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--Actuals--%>
                                                            <dx:TabPage Text="FACILITY" ToolTip="Allows Input Of Jobstep Facility Information" Visible="false">
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
                                                            </dx:TabPage> <%--Facilty--%>
                                                            <dx:TabPage Text="TRACKING" ToolTip="Allows Input Of Jobstep Tracking Information" Visible="false">
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="PlannedJobTrackingCC" runat="server">
                                                                        <dx:ASPxFormLayout ID="PlannedJobTrackingLayout" runat="server">
                                                                            <Items>
                                                                                <dx:LayoutGroup Caption="" ColCount="2" Width="100%" GroupBoxDecoration="None">
                                                                                    <Items>
                                                                                    </Items>
                                                                                </dx:LayoutGroup>
                                                                            </Items>
                                                                        </dx:ASPxFormLayout>                            
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--Tracking--%>
                                                            <dx:TabPage Text="ASSIGNMENTS" ToolTip="Allows Input Of Jobstep Assignment Information" Visible="false">
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="PlannedJobAssignmentsCC" runat="server">
                                                                        <dx:ASPxFormLayout ID="PlannedJobAssignmentsLayout" runat="server">
                                                                            <Items>
                                                                                <dx:LayoutGroup Caption="" ColCount="2" Width="100%" GroupBoxDecoration="None">
                                                                                    <Items>
                                                                                    </Items>
                                                                                </dx:LayoutGroup>
                                                                            </Items>
                                                                        </dx:ASPxFormLayout>                            
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--Assignments--%>
                                                            <dx:TabPage Text="COMPLETION" ToolTip="Allows Input Of Jobstep Completion Information" >
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="PlannedJobCompCC" runat="server">
                                                                        <dx:ASPxFormLayout ID="PlannedJobCompLayout" ColCount="2" runat="server">
                                                                            <Items>
                                                                                        <dx:LayoutItem Name="fldStartDate" Caption="Start Date:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxDateEdit ID="TxtWorkStartDate" 
                                                                                                                     ClientInstanceName="TxtWorkStartDate"
                                                                                                                     DisplayFormatString="D"
                                                                                                                     Theme="Mulberry"
                                                                                                                     Width="450px" 
                                                                                                                     runat="server">
                                                                                                    </dx:ASPxDateEdit>                                                                                                    
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldReturnWithin" Caption="Return Within:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                <dx:ASPxSpinEdit  ID="txtReturnWithin" 
                                                                                                                          ClientInstanceName="txtReturnWithin"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          ValueType="System.Integer"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335"
                                                                                                                          Width="450px"
                                                                                                                          HorizontalAlign="Right"
                                                                                                                          AllowMouseWheel="False"
                                                                                                                          SpinButtons-ClientVisible="true" ToolTip="Number of Days" HelpTextSettings-DisplayMode="Inline" HelpText="Number off Days">
<SpinButtons ClientVisible="true"></SpinButtons>

                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>                                                                                                    
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>

                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldCompDate" Caption="Completed Date:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxDateEdit ID="TxtWorkCompDate" 
                                                                                                                     ClientInstanceName="TxtWorkCompDate"
                                                                                                                     DisplayFormatString="D"
                                                                                                                     Theme="Mulberry"
                                                                                                                     Width="450px" 
                                                                                                                     runat="server">
                                                                                                    </dx:ASPxDateEdit>                                                                                                                    
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>

                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldOutcome" Caption="Outcome Code:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxComboBox ID="ComboOutcome" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                                     ValueType="System.String" ValueField="n_outcomecodeid"
                                                                                                                     OnItemsRequestedByFilterCondition="ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL"
                                                                                                                     OnItemRequestedByValue="ComboOutcomeCode_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                                     Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="outcomecodeid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboOutcome">
                                                                                                        <Columns>
                                                                                                            <dx:ListBoxColumn FieldName="n_outcomecodeid" Visible="False" />
                                                                                                            <dx:ListBoxColumn FieldName="outcomecodeid" Caption="Outcome Code ID" Width="75px" ToolTip="M-PET.NET Outcome Code ID"/>
                                                                                                            <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Outcome Code Description"/>
                                                                                                        </Columns>
                                                                                                    </dx:ASPxComboBox>
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldUpdateObjects" Caption="Update Objects:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                            <dx:ASPxCheckBoxList ID="chkUpdateObjects" runat="server" 
                                                                                                        ValueField="ID" TextField="Name" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Horizontal">
                                                                                                    </dx:ASPxCheckBoxList>                                                                                                    
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>

                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>                                                         
                                                                                        <dx:LayoutItem Name="fldCompletedBy" Caption="Completed By:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxComboBox ID="ComboCompletedBy" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                                     ValueType="System.String" ValueField="UserID"
                                                                                                                     OnItemsRequestedByFilterCondition="ComboCompletedBy_OnItemsRequestedByFilterCondition_SQL"
                                                                                                                     OnItemRequestedByValue="ComboCompletedBy_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                                     Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboCompletedBy">
                                                                                               
                                                                                                        <Columns>
                                                                                                            <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                                            <dx:ListBoxColumn FieldName="username" Caption="User ID" Width="75px" ToolTip="M-PET.NET User ID"/>
                                                                                                            <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User Full Name"/>
                                                                                                        </Columns>
                                                                                                    </dx:ASPxComboBox>
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldPostedBy" Caption="Posted By:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxComboBox ID="ComboPostedBy" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                                     ValueType="System.String" ValueField="UserID"
                                                                                                                     OnItemsRequestedByFilterCondition="ComboPostedBy_OnItemsRequestedByFilterCondition_SQL"
                                                                                                                     OnItemRequestedByValue="ComboPostedBy_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                                     Width="450px" DropDownStyle="DropDown" Theme="Mulberry" TextField="username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboPostedBy">
                                                                                               
                                                                                                        <Columns>
                                                                                                            <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                                            <dx:ListBoxColumn FieldName="username" Caption="User ID" Width="75px" ToolTip="M-PET.NET User ID"/>
                                                                                                            <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User Full Name"/>
                                                                                                        </Columns>
                                                                                                    </dx:ASPxComboBox>
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>
                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                        <dx:LayoutItem Name="fldPostedOn" Caption="Posted On:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxDateEdit ID="TxtPostDate" 
                                                                                                                     ClientInstanceName="TxtPostDate"
                                                                                                                     DisplayFormatString="D"
                                                                                                                     Theme="Mulberry"
                                                                                                                     Width="450px" 
                                                                                                                     runat="server">
                                                                                                    </dx:ASPxDateEdit>                                                                                                           
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>

                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem>
                                                                                    </Items>
                                                                        </dx:ASPxFormLayout>                            
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage> <%--Completion--%>
                                                            <dx:TabPage Text="PROCEDURES" ToolTip="Allows Input Of Jobstep Notes" >
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
                                                            </dx:TabPage> <%--Procedures --%>                                                                                              
                                                            <dx:TabPage Text="POST NOTES" ToolTip="Allows Input Of Jobstep Post Notes">
                                                                <ContentCollection>
                                                                    <dx:ContentControl ID="PlannedJobPostCC" runat="server">
                                                                        <dx:ASPxFormLayout ID="PlannedJobPostLayout" Theme="Mulberry" runat="server" Width="98%" Paddings="0,0">
                                                                            <Items>
                                                                                <dx:LayoutGroup Width="100%" GroupBoxDecoration="None">
                                                                                    <Items>
                                                                                        <dx:LayoutItem Caption="" ShowCaption="False" HelpText="Please Enter Any Additional Information" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection >
                                                                                                <dx:LayoutItemNestedControlContainer>
                                                                                                    <dx:ASPxMemo ID="txtPostNotes" Native="True" Height="400px" Width="100%" MaxLength="8000"
                                                                                                                 ClientInstanceName="txtPostNotes"
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
                                                            </dx:TabPage> <%--Post Notes--%>
                                                        </TabPages> <%--Job Step side tabs--%>
                                                    </dx:ASPxPageControl>                                                        
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage>
                                        <dx:TabPage Text="MEMBERS" ToolTip="Allows Input Of Jobstep Members">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl9" runat="server">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" OnUnload="UpdatePanel_Unload">
                                                        <ContentTemplate>
                                                            <dx:ASPxGridView 
                                                                ID="MemberGrid" 
                                                                runat="server" 
                                                                Theme="Mulberry" 
                                                                KeyFieldName="n_JobOtherID" 
                                                                Width="98%"  
                                                                KeyboardSupport="True" 
                                                                ClientInstanceName="MemberGrid" 
                                                                AutoPostBack="True" 
                                                                EnableCallBacks="true" 
                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                SettingsPager-Mode="ShowPager" 
                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                DataSourceID="MemberDataSource"
                                                                OnRowUpdating="MemberGrid_RowUpdating"
                                                                OnDataBound="MemberGridBound">
                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                    <Row CssClass="gridViewRow"></Row>

                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                </Styles>
                                                                <ClientSideEvents RowClick="function(s, e) {
                                                                        MemberGrid.GetRowValues(e.visibleIndex, 'n_JobOtherID', OnGetMemberRowId);
                                                                    }"
                                                                                  RowDblClick="function(s, e) {
                                                                    s.StartEditRow(e.visibleIndex);
                                                                }" />
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowEditButton="True" Visible="false" VisibleIndex="0" />
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobOtherID" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobID" ReadOnly="True" Visible="false" VisibleIndex="2">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobStepID" ReadOnly="True" Visible="false" VisibleIndex="3">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_MaintenanceObjectID" ReadOnly="True" Visible="false" VisibleIndex="4">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ObjectID" ReadOnly="True" Caption="Object ID" SortOrder="Ascending" Width="250px" VisibleIndex="5">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                       <DataItemTemplate>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="javascript:void(0)"
                                                                                Text='<%# Eval("ObjectID") %>' Width="100%" Theme="Mulberry"> 
                                                                                <ClientSideEvents Click="onHyperLinkClick" />
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ObjectDescription" ReadOnly="True" Caption="Description" Width="450px" VisibleIndex="6">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataCheckColumn FieldName="b_Completed" Caption="Completed" Width="100px" VisibleIndex="7">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataCheckColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="WorkDate" Caption="Work Date" VisibleIndex="8"  Width="200px">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>                                                                                                                                                                     
                                                                    </dx:GridViewDataDateColumn>
                                                                </Columns>
                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn"  />
                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" AllowEdit="True"/>
                                                                <SettingsEditing Mode="PopupEditForm" 
                                                                                 PopupEditFormHorizontalAlign="WindowCenter"
                                                                                 PopupEditFormVerticalAlign="WindowCenter" 
                                                                                 PopupEditFormWidth="800px" 
                                                                                 PopupEditFormModal="True" 
                                                                                 PopupEditFormShowHeader="False"></SettingsEditing>
                                                                <Settings VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350" ShowFooter="true"  />
                                                                <SettingsPager PageSize="10">
                                                                    <PageSizeItemSettings Visible="true" />
                                                                </SettingsPager>
                                                                <SettingsPopup>
                                                                    <EditForm Width="800px" Modal="true" />
                                                                </SettingsPopup>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <div style="padding: 4px 4px 3px 4px">
                                                                            <dx:ASPxFormLayout Width="98%" Theme="Mulberry" ID="MemberEditLayout" runat="server">
                                                                                <Items>
                                                                                    <dx:LayoutGroup Name="MemberEditGroup" Caption="Item Edit"  ColCount="3">
                                                                                        <Items>
                                                                                            <dx:LayoutItem Name="liMemberID" Caption="Object ID:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtMemberID" 
                                                                                                                           Width="98%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtMemberID"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "ObjectID" 
                                                                                                                           ValueField = "ObjectID"
                                                                                                                           Value='<%# Bind("ObjectID") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           ReadOnly="True">
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liMemberCompleted" Caption="Completed:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxCheckbox ID="txtMemberCompletedEdit" 
                                                                                                                         ClientInstanceName="txtMemberCompletedEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         Checked='<%# Convert.ToBoolean(Eval("b_Completed")) %>'>
                                                                                                        </dx:ASPxCheckbox>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liMemberDate" Caption="Work Date:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxDateEdit ID="txtMemberDateUsedEdit" 
                                                                                                                         ClientInstanceName="txtMemberDateUsedEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "WorkDate" 
                                                                                                                         ValueField = "WorkDate"
                                                                                                                         Value='<%# Bind("WorkDate") %>'
                                                                                                                         ValueType="System.DateTime"
                                                                                                                         NullText="MM/DD/YYYY"
                                                                                                                         EditFormat="Custom"
                                                                                                                         EditFormatString="MM/dd/yyyy">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxDateEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liMemberDesc" Caption="Description:" ColSpan="3" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtMemberDesc" 
                                                                                                                           Height="50px" 
                                                                                                                           Width="98%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtMemberDesc"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "ObjectDescription" 
                                                                                                                           ValueField = "ObjectDescription"
                                                                                                                           Value='<%# Bind("ObjectDescription") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           ReadOnly="True">
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                        </Items>
                                                                                    </dx:LayoutGroup>
                                                                                </Items>
                                                                            </dx:ASPxFormLayout>                                                                                                        
                                                                                   
                                                                        </div>
                                                                        <div style="padding: 2px 2px 2px 2px; text-align: right;">
                                                                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" OnInit="HideDefaultEditButtons"  ReplacementType="EditFormUpdateButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" OnInit="HideDefaultEditButtons" ReplacementType="EditFormCancelButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxButton ID="btnUpdateMember" AutoPostBack="false" runat="server" CssClass="button" Text="Update" >
                                                                                <ClientSideEvents Click="function (s, e) { OnMemberUpateClick(s, e); }" />                                                                                
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>
                                                                            <dx:ASPxButton ID="btnCancelMember" AutoPostBack="False" runat="server" Text="Cancel" CssClass="button">
                                                                                <ClientSideEvents Click="function (s, e) { OnMemberCancelClick(s, e); }" />
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>     
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                                <Templates>
                                                                    <FooterRow>
                                                                        <dx:ASPxButton runat="server" ID="AddNewMemberButton" Theme="Mulberry" Text="Add New Member">
                                                                            <ClientSideEvents Click="ShowMemberPopup" />
                                                                        </dx:ASPxButton>
                                                                        <dx:ASPxButton runat="server" ID="DeleteMemberButton" OnClick="DeleteItems_Click" Theme="Mulberry" Text="Delete Member"></dx:ASPxButton>
                                                                    </FooterRow>
                                                                </Templates>
                                                            </dx:ASPxGridView>                                                    
                                                            <asp:SqlDataSource ID="MemberDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                         DECLARE @NullDate DATETIME
                                                            SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)

                                                        --Return Jobstep Member Records For Specified Job/Jobstep IDs
                                                            SELECT  tbl_JobMember.n_JobMembersID AS 'n_JobOtherID',
                                                                    tbl_JobMember.n_JobID AS 'n_JobID',
                                                                    tbl_JobMember.n_JobStepID AS 'n_JobStepID',
                                                                    tbl_JobMember.n_MaintenanceObjectID AS 'n_MaintenanceObjectID',
                                                                    CASE tbl_JobMember.b_Completed 
			                                                        WHEN 'Y' THEN 1
			                                                        ELSE 0
			                                                        END AS 'b_Completed',
			                                                        tbl_Objects.objectid AS 'ObjectID',
			                                                        tbl_Objects.DESCRIPTION AS 'ObjectDescription',
                                                                    CASE tbl_JobMember.WorkDate
                                                                      WHEN @NullDate THEN NULL
                                                                      ELSE tbl_JobMember.WorkDate
                                                                    END AS 'WorkDate'
                                                            FROM    dbo.JobMembers tbl_JobMember
	                                                        INNER JOIN (SELECT tblObjects.n_objectid,
					                                                           tblObjects.objectid,
					                                                           tblObjects.description
				                                                        FROM dbo.MaintenanceObjects tblObjects) tbl_Objects ON tbl_JobMember.n_MaintenanceObjectID = tbl_Objects.n_objectid
                                                            WHERE   ( tbl_JobMember.n_JobID = @JobID )
                                                                    AND ( tbl_JobMember.n_jobstepid = -1 ) ">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobID" SessionField="editingJobID" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>                                                    
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage> <%--Members--%>
                                        <dx:TabPage Name="StepCrew" Text="CREW" ToolTip="Allows Input Of Jobstep Crew">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl10" runat="server">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" OnUnload="UpdatePanel_Unload">
                                                        <ContentTemplate>
                                                            <dx:ASPxGridView 
                                                                ID="CrewGrid" 
                                                                runat="server" 
                                                                Theme="Mulberry" 
                                                                KeyFieldName="RecordID" 
                                                                Width="98%" 
                                                                KeyboardSupport="True" 
                                                                ClientInstanceName="CrewGrid" 
                                                                AutoPostBack="True" 
                                                                EnableCallBacks="true" 
                                                                Settings-HorizontalScrollBarMode="Auto" SettingsPager-Mode="ShowPager" SettingsBehavior-ProcessFocusedRowChangedOnServer="True" SettingsBehavior-AllowFocusedRow="False" 
                                                                SettingsBehavior-AllowSelectByRowClick="true" DataSourceID="CrewDataSource" OnDataBound="CrewGridBound" OnRowUpdating="CrewGrid_RowUpdating" >
                                                               
                                                                 <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                    <Row CssClass="gridViewRow"></Row>

                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                </Styles>
                                                                <ClientSideEvents RowClick="function(s, e) {
                                                                        CrewGrid.GetRowValues(e.visibleIndex, 'RecordID;DMRKEY', OnGetCrewRowId);
                                                                        
                                                                        window._myRowClickObject = window._myRowClickObject || {};
                                                                        _myRowClickObject.s = s;
                                                                        _myRowClickObject.e = e;
                                                                        
                                                                       
                                                                    }"
                                                                                RowDblClick="function(s, e) {
                                                                                window._myRowBblClickObject = window._myRowBblClickObject || {};
                                                                                _myRowBblClickObject.s = s;
                                                                                _myRowBblClickObject.e = e;
                                                                    
                                                                                s.StartEditRow(e.visibleIndex);
                                                                               
                                                                        
                                                                    }" />
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowEditButton="True" Visible="false" VisibleIndex="0" />
                                                                    <dx:GridViewDataTextColumn FieldName="RecordID" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="UserID" ReadOnly="True" Visible="false" VisibleIndex="2">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_skillid" ReadOnly="True" Visible="false" VisibleIndex="3">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_ShiftID" ReadOnly="True" Visible="false" VisibleIndex="4">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_PayCodeID" ReadOnly="True" Visible="false" VisibleIndex="5">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataComboBoxColumn FieldName="CrewMemberTextID" Caption="User ID" Width="100px" VisibleIndex="6">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink1" NavigateUrl="javascript:void(0)" runat="server" Text='<%# Eval("CrewMemberTextID") %>' Width="100%" Theme="Mulberry">
                                                                                <ClientSideEvents Click="onHyperLinkClick" />
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                        <PropertiesComboBox ClientInstanceName="gridComboCrewUser" TextField="Username" IncrementalFilteringMode="Contains" ValueField="UserID" DataSourceID="CrewIDGridLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="Username" Caption="Username" Width="75px" ToolTip="M-PET.NET User's Username"/>
                                                                                                                <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User's Full Name"/>
                                                                                                            </Columns>
                                                                        </PropertiesComboBox>
                                                                    </dx:GridViewDataComboBoxColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="CrewMemberName" ReadOnly="True" Caption="Full Name" Width="150px" VisibleIndex="7">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <PropertiesTextEdit ClientInstanceName="CrewMemberName"></PropertiesTextEdit>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ShiftIDText" ReadOnly="True" Caption="Shift ID" Width="100px" VisibleIndex="15">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ShiftIDDesc" ReadOnly="True" Visible="false">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataComboBoxColumn FieldName="SkillIDText" Caption="Skill ID" Width="100px" VisibleIndex="14">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <PropertiesComboBox ClientInstanceName="gridComboCrewSkill" TextField="skillid" IncrementalFilteringMode="Contains" ValueField="n_skillid" DataSourceID="CrewSkillGridLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_skillid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="skillid" Caption="Skill ID" Width="75px" ToolTip="M-PET.NET User's Skill ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Skill Description"/>
                                                                                                            </Columns>
                                                                        </PropertiesComboBox>
                                                                    </dx:GridViewDataComboBoxColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ShiftIDDesc" ReadOnly="True" Visible="false">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="EstHrs" Caption="Est. Hrs." Width="100px" VisibleIndex="10">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ActualHrs" Caption="Act. Hrs." Width="100px" VisibleIndex="11">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="PayRate" ReadOnly="True" Visible="false">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataComboBoxColumn FieldName="PayCodeText" Caption="Paycode ID" Width="100px" VisibleIndex="13">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <PropertiesComboBox ClientInstanceName="gridComboCrewPaycode" TextField="paycodeid" IncrementalFilteringMode="Contains" ValueField="n_paycodeid" DataSourceID="CrewPaycodeGridLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_paycodeid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="paycodeid" Caption="Paycode ID" Width="75px" ToolTip="M-PET.NET User's Paycode ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Paycode Description"/>
                                                                                                            </Columns>
                                                                        </PropertiesComboBox>
                                                                    </dx:GridViewDataComboBoxColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="WorkDate" Caption="Worked" VisibleIndex="9" SortOrder="Descending" Width="100px">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataDateColumn>     
                                                                    <dx:GridViewDataDateColumn FieldName="CertificationDate" Caption="Cert. Date" ReadOnly="True" VisibleIndex="16" Width="100px">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="CertificationDateExpires" Caption="Cert. Exp. Date" ReadOnly="True" VisibleIndex="17" Width="100px">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_laborclassid" ReadOnly="True" Visible="false">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="RateType" ReadOnly="True" Visible="false">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="DMRKEY" ReadOnly="True" Visible="false">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataComboBoxColumn FieldName="LaborClassID" Caption="Laborclass ID" Width="100px" VisibleIndex="8">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <PropertiesComboBox ClientInstanceName="gridComboCrewLabor" TextField="laborclassid" IncrementalFilteringMode="Contains" ValueField="n_laborclassid" DataSourceID="CrewLaborGridLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_laborclassid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="laborclassid" Caption="Labor Class" Width="75px" ToolTip="M-PET.NET User's Labor Class ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Labor Class Description"/>
                                                                                                            </Columns>
                                                                        </PropertiesComboBox>
                                                                    </dx:GridViewDataComboBoxColumn>                                                            
                                                                    <dx:GridViewDataComboBoxColumn FieldName="RateTypeStr" Caption="Rate" Width="100px" VisibleIndex="12">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <PropertiesComboBox ClientInstanceName="gridComboCrewRate" TextField="Rate" IncrementalFilteringMode="Contains" ValueField="RateType" DataSourceID="CrewRateGridLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="RateType" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="Rate" Caption="Rate ID" Width="75px" ToolTip="M-PET.NET User's Rate ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="Desc" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Rate Description"/>
                                                                                                            </Columns>
                                                                        </PropertiesComboBox>
                                                                    </dx:GridViewDataComboBoxColumn>                                                                                                                  
                                                                    <dx:GridViewDataTextColumn FieldName="LinkedDMR" ReadOnly="True" Caption="Linked DMR" Width="100px" VisibleIndex="18">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>                                                                                                                                                                              
                                                                </Columns>
                                                                <SettingsBehavior 
                                                                    EnableRowHotTrack="True" 
                                                                    AllowFocusedRow="True" 
                                                                    AllowClientEventsOnLoad="false" 
                                                                    ColumnResizeMode="NextColumn" />
                                                                <SettingsDataSecurity 
                                                                    AllowDelete="False" 
                                                                    AllowInsert="True" 
                                                                    AllowEdit="True"/>
                                                                <Settings 
                                                                    VerticalScrollBarMode="Visible" 
                                                                    VerticalScrollBarStyle="Virtual" 
                                                                    VerticalScrollableHeight="350"  />
                                                                <SettingsEditing Mode="PopupEditForm" 
                                                                                 PopupEditFormHorizontalAlign="WindowCenter"
                                                                                 PopupEditFormVerticalAlign="WindowCenter" 
                                                                                 PopupEditFormWidth="500px" 
                                                                                 PopupEditFormModal="True" 
                                                                                 PopupEditFormShowHeader="False"></SettingsEditing>
                                                                <SettingsPager PageSize="10">
                                                                    <PageSizeItemSettings Visible="true" />
                                                                </SettingsPager>
                                                                <Settings ShowFooter="True" />
                                                                <TotalSummary>
                                                                    <dx:ASPxSummaryItem FieldName="EstHrs" SummaryType="Sum" />
                                                                    <dx:ASPxSummaryItem FieldName="ActualHrs"  SummaryType="Sum" />
                                                                </TotalSummary>
                                                                <SettingsPopup>
                                                                    <EditForm Width="500px" Modal="false" AllowResize="true" />
                                                                </SettingsPopup>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <div style="padding: 4px 4px 3px 4px">
                                                                            <dx:ASPxFormLayout Width="98%" Theme="Mulberry" ID="CrewEditLayout" runat="server">
                                                                                <Items>
                                                                                    <dx:LayoutGroup Name="CrewEditGroup" Caption="Item Edit"  ColCount="4">
                                                                                        <Items>
                                                                                            <dx:LayoutItem Name="liCrewUserID" Caption="User ID:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxComboBox ID="comboCrewUser" runat="server" 
                                                                                                                         ValueType="System.Int32" 
                                                                                                                         ValueField="UserID" 
                                                                                                                         Value='<%# Bind("UserID") %>'
                                                                                                                         DropDownStyle="DropDown" 
                                                                                                                         Theme="Mulberry" 
                                                                                                                         TextField="Username" 
                                                                                                                         DisplayFormatString="{0}"
                                                                                                                         DropDownButton-Enabled="True" 
                                                                                                                         AutoPostBack="False" 
                                                                                                                         ClientInstanceName="comboCrewUser"
                                                                                                                         DataSourceID="CrewIDLookupDataSource">
                                                                                                            <ClientSideEvents ValueChanged="function(s, e) {
                                                                                                                var objectHasValue = comboCrewUser.GetValue();
                                                                                                                var selectedItem = s.GetSelectedItem();
                                                                                                                if(objectHasValue!=null)
                                                                                                                {
                                                                                                                    txtCrewNameEdit.SetText(selectedItem.GetColumnText('FullName'));
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    txtCrewNameEdit.SetText('');
                                                                                                                }

                                                                                                            }" />                                                                                               
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="Username" Caption="Username" Width="75px" ToolTip="M-PET.NET User's Username"/>
                                                                                                                <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User's Full Name"/>
                                                                                                            </Columns>
                                                                                                        </dx:ASPxComboBox>
                                                                                                        <asp:SqlDataSource ID="CrewIDLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                    SELECT    tblUsers.[UserID] ,
                                                                                                                tblUsers.[Username] ,
                                                                                                                tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                                                                                                ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                                                                                                      FROM      dbo.MPetUsers AS tblUsers
                                                                                                      WHERE     tblUsers.Active = 1
                                                                                                                AND tblUsers.UserID > 0">
                                                                                                        </asp:SqlDataSource>                                                                                                                                                                                                                
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewLaborID" Caption="Labor Class:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxComboBox ID="comboCrewLabor" runat="server" 
                                                                                                                         ValueType="System.Int32" 
                                                                                                                         ValueField="n_laborclassid" 
                                                                                                                         Value='<%# Bind("n_laborclassid") %>'
                                                                                                                         DropDownStyle="DropDown" 
                                                                                                                         Theme="Mulberry" 
                                                                                                                         TextField="laborclassid" 
                                                                                                                         DisplayFormatString="{0}"
                                                                                                                         DropDownButton-Enabled="True" 
                                                                                                                         AutoPostBack="False" 
                                                                                                                         ClientInstanceName="comboCrewLabor"
                                                                                                                         DataSourceID="CrewLaborLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_laborclassid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="laborclassid" Caption="Labor Class" Width="75px" ToolTip="M-PET.NET User's Labor Class ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Labor Class Description"/>
                                                                                                            </Columns>
                                                                                                        </dx:ASPxComboBox>
                                                                                                        <asp:SqlDataSource ID="CrewLaborLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  tblLC.n_laborclassid ,
                                                                                                                    tblLC.laborclassid ,
                                                                                                                    tblLC.description
                                                                                                            FROM    dbo.laborclasses tblLC
                                                                                                            WHERE   tblLC.b_IsActive = 'Y'
                                                                                                                    AND tblLC.n_laborclassid > 0
                                                                                                            ORDER BY tblLC.laborclassid ASC">
                                                                                                        </asp:SqlDataSource>                                                                                                                                                                                                                
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewEstHrs" Caption="Est. Hrs:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtCrewEstHrsEdit" 
                                                                                                                          ClientInstanceName="txtCrewEstHrsEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "EstHrs" 
                                                                                                                          ValueField = "EstHrs"
                                                                                                                          Value='<%# Bind("EstHrs") %>' 
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          NullText="Enter Est. Hours"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewActHrs" Caption="Act. Hrs:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit ID="txtCrewActHrsEdit" 
                                                                                                                         ClientInstanceName="txtCrewActHrsEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "ActualHrs" 
                                                                                                                         ValueField = "ActualHrs"
                                                                                                                         Value='<%# Bind("ActualHrs") %>'
                                                                                                                         ValueType="System.Decimal"
                                                                                                                         DisplayFormatString="F2"
                                                                                                                         NullText="Enter Act. Hours"
                                                                                                                         MinValue="0"
                                                                                                                         MaxValue="79228162514264337593543950335">
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewName" Caption="Name:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtCrewNameEdit" 
                                                                                                                           Width="98%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtCrewNameEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "CrewMemberName" 
                                                                                                                           ValueField = "CrewMemberName"
                                                                                                                           Value='<%# Bind("CrewMemberName") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           ReadOnly="True">
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>   
                                                                                            <dx:LayoutItem Name="liCrewRateID" Caption="Rate:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxComboBox ID="comboCrewRate" runat="server" 
                                                                                                                         ValueType="System.Int32" 
                                                                                                                         ValueField="RateType" 
                                                                                                                         Value='<%# Bind("RateType") %>'
                                                                                                                         DropDownStyle="DropDown" 
                                                                                                                         Theme="Mulberry" 
                                                                                                                         TextField="Rate" 
                                                                                                                         DisplayFormatString="{0}"
                                                                                                                         DropDownButton-Enabled="True" 
                                                                                                                         AutoPostBack="False" 
                                                                                                                         ClientInstanceName="comboCrewRate"
                                                                                                                         DataSourceID="CrewRateLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="RateType" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="Rate" Caption="Rate ID" Width="75px" ToolTip="M-PET.NET User's Rate ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="Desc" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Rate Description"/>
                                                                                                            </Columns>
                                                                                                        </dx:ASPxComboBox>
                                                                                                        <asp:SqlDataSource ID="CrewRateLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  0 AS RateType ,
                                                                                                                    'STANDARD' AS Rate ,
                                                                                                                    'STANDARD PAY RATE' AS 'Desc'
                                                                                                            UNION
                                                                                                            SELECT  1 AS RateType ,
                                                                                                                    'OVERTIME' AS Rate ,
                                                                                                                    'OVERTIME PAY RATE' AS 'Desc'
                                                                                                            UNION
                                                                                                            SELECT  2 AS RateType ,
                                                                                                                    'OTHER' AS Rate ,
                                                                                                                    'OTHER PAY RATE' AS 'DESC'">
                                                                                                        </asp:SqlDataSource>                                                                                                                                                                                                                
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewSkillID" Caption="Skill:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxComboBox ID="comboCrewSkill" runat="server" 
                                                                                                                         ValueType="System.Int32" 
                                                                                                                         ValueField="n_skillid" 
                                                                                                                         Value='<%# Bind("n_skillid") %>'
                                                                                                                         DropDownStyle="DropDown" 
                                                                                                                         Theme="Mulberry" 
                                                                                                                         TextField="skillid" 
                                                                                                                         DisplayFormatString="{0}"
                                                                                                                         DropDownButton-Enabled="True" 
                                                                                                                         AutoPostBack="False" 
                                                                                                                         ClientInstanceName="comboCrewSkill"
                                                                                                                         DataSourceID="CrewSkillLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_skillid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="skillid" Caption="Skill ID" Width="75px" ToolTip="M-PET.NET User's Skill ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Skill Description"/>
                                                                                                            </Columns>
                                                                                                        </dx:ASPxComboBox>
                                                                                                        <asp:SqlDataSource ID="CrewSkillLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  tblSkills.n_skillid ,
                                                                                                                    tblSkills.skillid ,
                                                                                                                    tblSkills.description
                                                                                                            FROM    dbo.skills tblSkills
                                                                                                            WHERE   tblSkills.b_IsActive = 'Y'
                                                                                                                    AND tblSkills.n_skillid > 0
                                                                                                            ORDER BY tblSkills.skillid ASC">
                                                                                                        </asp:SqlDataSource>                                                                                                                                                                                                                
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewPaycodeID" Caption="Pay Code:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxComboBox ID="comboCrewPaycode" runat="server" 
                                                                                                                         ValueType="System.Int32" 
                                                                                                                         ValueField="n_paycodeid" 
                                                                                                                         Value='<%# Bind("n_paycodeid") %>'
                                                                                                                         DropDownStyle="DropDown" 
                                                                                                                         Theme="Mulberry" 
                                                                                                                         TextField="paycodeid" 
                                                                                                                         DisplayFormatString="{0}"
                                                                                                                         DropDownButton-Enabled="True" 
                                                                                                                         AutoPostBack="False" 
                                                                                                                         ClientInstanceName="comboCrewPaycode"
                                                                                                                         DataSourceID="CrewPaycodeLookupDataSource">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_paycodeid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="paycodeid" Caption="Paycode ID" Width="75px" ToolTip="M-PET.NET User's Paycode ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET User's Paycode Description"/>
                                                                                                            </Columns>
                                                                                                        </dx:ASPxComboBox>
                                                                                                        <asp:SqlDataSource ID="CrewPaycodeLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  tblPC.n_paycodeid ,
                                                                                                                    tblPC.paycodeid ,
                                                                                                                    tblPC.description
                                                                                                            FROM    dbo.Paycodes tblPC
                                                                                                            WHERE   tblPC.b_IsActive = 'Y'
                                                                                                                    AND tblPC.n_paycodeid > 0
                                                                                                            ORDER BY tblPC.paycodeid ASC">
                                                                                                        </asp:SqlDataSource>                                                                                                                                                                                                                
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liCrewDate" Caption="Work Date:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxDateEdit ID="txtCrewDateWorkedEdit" 
                                                                                                                         ClientInstanceName="txtCrewDateWorkedEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "WorkDate" 
                                                                                                                         ValueField = "WorkDate"
                                                                                                                         Value='<%# Bind("WorkDate") %>'
                                                                                                                         ValueType="System.DateTime"
                                                                                                                         NullText="MM/DD/YYYY"
                                                                                                                         EditFormat="Custom"
                                                                                                                         EditFormatString="MM/dd/yyyy">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxDateEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                        </Items>
                                                                                    </dx:LayoutGroup>
                                                                                </Items>
                                                                            </dx:ASPxFormLayout>                                                                                                        
                                                                                   
                                                                        </div>
                                                                        <div style="padding: 2px 2px 2px 2px; text-align: right;">
                                                                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" OnInit="HideDefaultEditButtons"  ReplacementType="EditFormUpdateButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" OnInit="HideDefaultEditButtons" ReplacementType="EditFormCancelButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxButton ID="btnUpdateCrew" AutoPostBack="false" runat="server" CssClass="button" Text="Update" >
                                                                                <ClientSideEvents Click="function (s, e) { OnCrewUpateClick(s, e); }" />                                                                                
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>
                                                                            <dx:ASPxButton ID="btnCancelCrew" AutoPostBack="False" runat="server" Text="Cancel" CssClass="button">
                                                                                <ClientSideEvents Click="function (s, e) { OnCrewCancelClick(s, e); }" />
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>                                                                            
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                                <Templates>
                                                                    <FooterRow>
                                                                        <dx:ASPxButton runat="server" ID="AddNewCrewButton" Theme="Mulberry" Text="Add New Crew Member">
                                                                            <ClientSideEvents Click="ShowCrewPopup" />
                                                                        </dx:ASPxButton>
                                                                            
                                                                        <dx:ASPxButton runat="server" ID="DeleteCrewButton" OnClick="DeleteItems_Click" Theme="Mulberry" Text="Delete Crew Member"></dx:ASPxButton>
                                                                    </FooterRow>
                                                                </Templates>
                                                            </dx:ASPxGridView>
                                                            <asp:SqlDataSource ID="CrewPaycodeGridLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  tblPC.n_paycodeid ,
                                                                                                                    tblPC.paycodeid ,
                                                                                                                    tblPC.description
                                                                                                            FROM    dbo.Paycodes tblPC
                                                                                                            WHERE   tblPC.b_IsActive = 'Y'
                                                                                                                    AND tblPC.n_paycodeid > 0
                                                                                                            ORDER BY tblPC.paycodeid ASC">
                                                                                                        </asp:SqlDataSource>    
                                                            <asp:SqlDataSource ID="CrewSkillGridLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  tblSkills.n_skillid ,
                                                                                                                    tblSkills.skillid ,
                                                                                                                    tblSkills.description
                                                                                                            FROM    dbo.skills tblSkills
                                                                                                            WHERE   tblSkills.b_IsActive = 'Y'
                                                                                                                    AND tblSkills.n_skillid > 0
                                                                                                            ORDER BY tblSkills.skillid ASC">
                                                                                                        </asp:SqlDataSource> 
                                                            <asp:SqlDataSource ID="CrewRateGridLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  0 AS RateType ,
                                                                                                                    'STANDARD' AS Rate ,
                                                                                                                    'STANDARD PAY RATE' AS 'Desc'
                                                                                                            UNION
                                                                                                            SELECT  1 AS RateType ,
                                                                                                                    'OVERTIME' AS Rate ,
                                                                                                                    'OVERTIME PAY RATE' AS 'Desc'
                                                                                                            UNION
                                                                                                            SELECT  2 AS RateType ,
                                                                                                                    'OTHER' AS Rate ,
                                                                                                                    'OTHER PAY RATE' AS 'DESC'">
                                                                                                        </asp:SqlDataSource>  
                                                            <asp:SqlDataSource ID="CrewLaborGridLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                            SELECT  tblLC.n_laborclassid ,
                                                                                                                    tblLC.laborclassid ,
                                                                                                                    tblLC.description
                                                                                                            FROM    dbo.laborclasses tblLC
                                                                                                            WHERE   tblLC.b_IsActive = 'Y'
                                                                                                                    AND tblLC.n_laborclassid > 0
                                                                                                            ORDER BY tblLC.laborclassid ASC">
                                                                                                        </asp:SqlDataSource>  
                                                            <asp:SqlDataSource ID="CrewIDGridLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                                                    SELECT    tblUsers.[UserID] ,
                                                                                                                tblUsers.[Username] ,
                                                                                                                tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                                                                                                ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                                                                                                      FROM      dbo.MPetUsers AS tblUsers
                                                                                                      WHERE     tblUsers.Active = 1
                                                                                                                AND tblUsers.UserID > 0">
                                                                                                        </asp:SqlDataSource>                                                     
                                                            <asp:SqlDataSource ID="CrewDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        DECLARE @NullDate DATETIME
                                                        SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
                                                        --Return Data
                                                        SELECT  tbl_JC.n_jobcrewid AS 'RecordID' ,
                                                        tbl_JC.n_personid AS 'UserID' ,
                                                        case tbl_JC.n_skillid 
                                                            WHEN 0 THEN NULL
                                                            WHEN -1 THEN NULL
                                                            ELSE tbl_JC.n_skillid
                                                        END AS 'n_skillid' ,
                                                        tbl_JC.n_ShiftID AS 'n_ShiftID' ,
                                                        CASE tbl_JC.n_PayCodeID
                                                            WHEN -1 THEN NULL
                                                            WHEN 0 THEN NULL
                                                            ELSE tbl_JC.n_PayCodeID
                                                        END AS 'n_PayCodeID' ,
                                                        tbl_Users.Username AS 'CrewMemberTextID' ,
                                                        tbl_Users.Name AS 'CrewMemberName' ,
                                                        tbl_Shifts.ShiftID AS 'ShiftIDText' ,
                                                        tbl_Shifts.Description AS 'ShiftIDDesc' ,
                                                        tbl_Skills.skillid AS 'SkillIDText' ,
                                                        tbl_Skills.description AS 'SkillDesc' ,
                                                        tbl_JC.actuallen AS 'ActualHrs' ,
                                                        tbl_JC.estlen AS 'EstHrs' ,
                                                        tbl_JC.Payfactor AS 'PayRate' ,
                                                        tbl_JC.PayfactorText AS 'PayCodeText' ,
                                                        CASE tbl_JC.WorkDate
                                                          WHEN @NullDate THEN NULL
                                                          ELSE tbl_JC.WorkDate
                                                        END AS 'WorkDate' ,
                                                        CASE tbl_JC.CertificationDate
                                                          WHEN @NullDate THEN NULL
                                                          ELSE tbl_JC.CertificationDate
                                                        END AS 'CertificationDate' ,
                                                        CASE tbl_JC.CertificationDateExpires
                                                          WHEN @NullDate THEN NULL
                                                          ELSE tbl_JC.CertificationDateExpires
                                                        END AS 'CertificationDateExpires' ,
                                                        CASE tbl_JC.n_laborclassid
                                                            WHEN -1 THEN NULL
                                                            WHEN 0 THEN NULL
                                                            ELSE tbl_JC.n_laborclassid
                                                        END AS 'n_laborclassid' ,
                                                        tbl_JC.RateType AS 'RateType' ,
                                                        tbl_JC.DMRKEY AS 'DMRKEY' ,
                                                        tbl_LaborClasses.laborclassid AS 'LaborClassID' ,
                                                        CASE tbl_JC.RateType
                                                          WHEN 0 THEN 'STANDARD'
                                                          WHEN 1 THEN 'OVERTIME'
                                                          WHEN 2 THEN 'OTHER'
                                                        END AS 'RateTypeStr' ,
                                                        ISNULL(tbl_TimeBatches.time_batchid, 'N/A') AS 'LinkedDMR'
                                                        FROM    dbo.Jobcrews tbl_JC
                                                                INNER JOIN ( SELECT dbo.MPetUsers.UserID ,
                                                                                    dbo.MPetUsers.Username ,
                                                                                    LTRIM(RTRIM(FirstName + ' ' + LastName)) AS Name
                                                                             FROM   dbo.MPetUsers
                                                                           ) tbl_Users ON tbl_JC.n_personid = tbl_Users.UserID
                                                                INNER JOIN ( SELECT dbo.skills.n_skillid ,
                                                                                    dbo.skills.skillid ,
                                                                                    dbo.skills.description
                                                                             FROM   dbo.skills
                                                                           ) tbl_Skills ON tbl_JC.n_skillid = tbl_Skills.n_skillid
                                                                INNER JOIN ( SELECT dbo.Shifts.n_shiftid ,
                                                                                    dbo.Shifts.ShiftID ,
                                                                                    dbo.Shifts.Description
                                                                             FROM   dbo.Shifts
                                                                           ) tbl_Shifts ON tbl_JC.n_ShiftID = tbl_Shifts.n_shiftid
                                                                INNER JOIN ( SELECT dbo.laborclasses.n_laborclassid ,
                                                                                    dbo.laborclasses.laborclassid
                                                                             FROM   dbo.laborclasses
                                                                           ) tbl_LaborClasses ON tbl_JC.n_laborclassid = tbl_LaborClasses.n_laborclassid
                                                                LEFT JOIN ( SELECT  dbo.time_batches.RecordID ,
                                                                                    dbo.time_batches.time_batchid
                                                                            FROM    dbo.time_batches
                                                                          ) tbl_TimeBatches ON tbl_JC.DMRKey = tbl_TimeBatches.RecordID
                                                        WHERE   tbl_JC.JobstepID = @JobStepID 
                                                                AND tbl_JC.JobID = @JobID">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobID" SessionField="editingJobID" />
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobStepID" SessionField="editingJobStepID" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage> <%--Crew--%>
                                        <dx:TabPage Text="PARTS" ToolTip="Allows Input Of Jobstep Parts">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl11" runat="server">
                                                    <dx:ASPxGridView
                                                        ID="PartGrid"
                                                        runat="server"
                                                        Theme="Mulberry"
                                                        Width="98%"
                                                        KeyboardSupport="True"
                                                        ClientInstanceName="PartGrid"
                                                        AutoPostBack="True"
                                                        Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsPager-Mode="ShowPager"
                                                        SettingsBehavior-ProcessFocusedRowChangedOnServer="True"
                                                        SettingsBehavior-AllowFocusedRow="True"
                                                        DataSourceID="PartDataSource"
                                                        OnDataBound="PartGridBound"
                                                        OnRowUpdating="PartGrid_RowUpdating" AutoGenerateColumns="False" KeyFieldName="n_jobpartid">
                                                        <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"
                                                            RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow">
                                                            <Header CssClass="gridViewHeader"></Header>

                                                            <Row CssClass="gridViewRow"></Row>

                                                            <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                            <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                            <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                        </Styles>
                                                        <ClientSideEvents RowClick="function(s, e) {
                                                                        PartGrid.GetRowValues(e.visibleIndex, 'n_jobpartid;DMRKey;n_masterpartid', OnGetPartRowId);
                                                                    }"
                                                            RowDblClick="function(s, e) {
                                                                    s.StartEditRow(e.visibleIndex);
                                                                }" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowEditButton="True" Visible="false" VisibleIndex="0" />
                                                            <dx:GridViewDataTextColumn FieldName="n_jobpartid" ReadOnly="True" Visible="false" VisibleIndex="23">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_masterpartid" ReadOnly="True" Visible="false" VisibleIndex="24">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_storeroomid" ReadOnly="True" Visible="false" VisibleIndex="25">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="b_nonstocked" ReadOnly="True" Caption="N/S" ToolTip="Nonstocked Yes/No?" Width="50px" VisibleIndex="1">
                                                                <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                <DataItemTemplate>
                                                                    <dx:ASPxHyperLink ID="ASPxHyperLink1" NavigateUrl="javascript:void(0)" runat="server" Text='<%# Eval("b_nonstocked") %>' Width="100%" Theme="Mulberry">
                                                                        <ClientSideEvents Click="onHyperLinkClick" />
                                                                    </dx:ASPxHyperLink>
                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="miscrefnum" ReadOnly="False" Caption="Misc. Ref." ToolTip="M-PET.NET Jobstep Part Misc. Ref." Width="100px" VisibleIndex="9">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="nspartid" ReadOnly="False" Caption="Part ID" ToolTip="M-PET.NET Masterpart ID" Width="100px" VisibleIndex="1">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="nspartcost" ReadOnly="False" Caption="Cost" ToolTip="M-PET.NET Jobstep Part Cost" Width="100px" VisibleIndex="6">
                                                                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <PropertiesTextEdit DisplayFormatString="c4" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="NSMfgPartID" ReadOnly="True" Caption="Mfg. Part ID" ToolTip="M-PET.NET Jobstep Part Manufacturer Part ID" Width="100px" VisibleIndex="8">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="NSPartDescr" ReadOnly="False" Caption="Description" ToolTip="M-PET.NET Masterpart Description" Width="200px" VisibleIndex="2">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="qtyplanned" ReadOnly="False" Caption="Est. Qty." ToolTip="M-PET.NET Estimated Jobstep Part Quantity" Width="100px" VisibleIndex="4">
                                                                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <PropertiesTextEdit DisplayFormatString="f2" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="qtyused" ReadOnly="False" Caption="Act. Qty." ToolTip="M-PET.NET Actual Jobstep Part Quantity" Width="100px" VisibleIndex="5">
                                                                <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <PropertiesTextEdit DisplayFormatString="f2" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_MfgPartID" ReadOnly="True" Visible="false" VisibleIndex="26">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="DMRKey" ReadOnly="True" Visible="false" VisibleIndex="27">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn FieldName="WorkDate" ReadOnly="False" Caption="Used" ToolTip="M-PET.NET Jobstep Part Date Used" Width="150px" VisibleIndex="7">
                                                                <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn FieldName="storeroomid" ReadOnly="True" Caption="Storeroom" ToolTip="M-PET.NET Masterpart Storeroom" Width="100px" VisibleIndex="3">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="cWorkDate" ReadOnly="True" Visible="false" VisibleIndex="28">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="time_batchid" ReadOnly="True" Caption="Linked DMR" ToolTip="M-PET.NET Jobstep Part Linked DMR" Width="100px" VisibleIndex="10">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="xactionnum" ReadOnly="True" Caption="Linked Trans." ToolTip="M-PET.NET Jobstep Part Linked Transaction Number" Width="100px" VisibleIndex="11">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_partatlocid" ReadOnly="True" Visible="false" VisibleIndex="29">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_FundSrcCodeID" ReadOnly="True" Visible="false" VisibleIndex="30">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" ReadOnly="True" Caption="Fund. Src." ToolTip="M-PET.NET Jobstep Part Fund Source ID" Width="100px" VisibleIndex="12">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_WorkOrderCodeID" ReadOnly="True" Visible="false" VisibleIndex="31">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" ReadOnly="True" Caption="Work Order" ToolTip="M-PET.NET Jobstep Part Work Order ID" Width="100px" VisibleIndex="13">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_WorkOpID" ReadOnly="True" Visible="false" VisibleIndex="32">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="WorkOpID" ReadOnly="True" Caption="Work Op." ToolTip="M-PET.NET Jobstep Part Work Operation ID" Width="100px" VisibleIndex="14">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_OrganizationCodeID" ReadOnly="True" Visible="false" VisibleIndex="33">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" ReadOnly="True" Caption="Org. Code" ToolTip="M-PET.NET Jobstep Part Organization Code ID" Width="100px" VisibleIndex="15">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_FundingGroupCodeID" ReadOnly="True" Visible="false" VisibleIndex="34">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" ReadOnly="True" Caption="Fund. Grp." ToolTip="M-PET.NET Jobstep Part Funding Group ID" Width="100px" VisibleIndex="16">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_ObjectCodeID" ReadOnly="True" Visible="false" VisibleIndex="35">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ObjectCodeID" ReadOnly="True" Caption="Obj. Code" ToolTip="M-PET.NET Jobstep Part Object Code ID" Width="100px" VisibleIndex="17">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_ControlSectionID" ReadOnly="True" Visible="false" VisibleIndex="36">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ControlSectionID" ReadOnly="True" Caption="Ctl. Sect." ToolTip="M-PET.NET Jobstep Part Control Section ID" Width="100px" VisibleIndex="18">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_EquipmentNumberID" ReadOnly="True" Visible="false" VisibleIndex="37">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" ReadOnly="True" Caption="Equip. #" ToolTip="M-PET.NET Jobstep Part Equipment Number ID" Width="100px" VisibleIndex="19">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Entered From" ReadOnly="True" Visible="false" VisibleIndex="38">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_StoresIssueID" ReadOnly="True" Visible="false" VisibleIndex="39">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_StoresIssueItemid" ReadOnly="True" Visible="false" VisibleIndex="40">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="aisle" ReadOnly="True" Caption="Aisle" ToolTip="M-PET.NET Jobstep Part Aisle Location" Width="100px" VisibleIndex="20">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="shelf" ReadOnly="True" Caption="Shelf" ToolTip="M-PET.NET Jobstep Part Shelf Location" Width="100px" VisibleIndex="21">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="bin" ReadOnly="True" Caption="Bin" ToolTip="M-PET.NET Jobstep Part Bin Location" Width="100px" VisibleIndex="22">
                                                                <CellStyle Wrap="False"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior
                                                            EnableRowHotTrack="True"
                                                            AllowFocusedRow="True"
                                                            AllowClientEventsOnLoad="false"
                                                            ColumnResizeMode="NextColumn" ConfirmDelete="True" />

                                                        <SettingsCommandButton>
                                                            <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                                            <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                                                        </SettingsCommandButton>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="n_jobpartid" ReadOnly="True" VisibleIndex="1">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_masterpartid" VisibleIndex="2" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_storeroomid" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="b_nonstocked" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="miscrefnum" VisibleIndex="5"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="nspartid" VisibleIndex="6"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="nspartcost" VisibleIndex="7"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="NSMfgPartID" VisibleIndex="8"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="NSPartDescr" VisibleIndex="9"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="qtyplanned" VisibleIndex="10"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="qtyused" VisibleIndex="11"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_MfgPartID" VisibleIndex="12" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="DMRKey" VisibleIndex="13"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn FieldName="WorkDate" ReadOnly="True" VisibleIndex="14"></dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn FieldName="storeroomid" VisibleIndex="15"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="cWorkDate" ReadOnly="True" VisibleIndex="16"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="time_batchid" VisibleIndex="17"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="xactionnum" ReadOnly="True" VisibleIndex="18"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_partatlocid" VisibleIndex="19" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_FundSrcCodeID" ReadOnly="True" VisibleIndex="20" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" VisibleIndex="21"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_WorkOrderCodeID" ReadOnly="True" VisibleIndex="22" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" VisibleIndex="23"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_WorkOpID" ReadOnly="True" VisibleIndex="24" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="WorkOpID" VisibleIndex="25"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_OrganizationCodeID" ReadOnly="True" VisibleIndex="26" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" VisibleIndex="27"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_FundingGroupCodeID" ReadOnly="True" VisibleIndex="28" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" VisibleIndex="29"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_ObjectCodeID" ReadOnly="True" VisibleIndex="30" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ObjectCodeID" VisibleIndex="31"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_ControlSectionID" ReadOnly="True" VisibleIndex="32" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ControlSectionID" VisibleIndex="33"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_EquipmentNumberID" ReadOnly="True" VisibleIndex="34" Visible="false">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" VisibleIndex="35"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Entered From" VisibleIndex="36"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_StoresIssueID" VisibleIndex="37" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="n_StoresIssueItemid" VisibleIndex="38" Visible="false"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="aisle" VisibleIndex="39"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="shelf" VisibleIndex="40"></dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="bin" VisibleIndex="41"></dx:GridViewDataTextColumn>

                                                            <%--<dx:GridViewCommandColumn ShowNewButton="True" ShowEditButton="True" ShowDeleteButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>--%>
                                                        </Columns>
                                                        <Settings 
                                                            VerticalScrollBarMode="Visible" 
                                                            VerticalScrollBarStyle="Virtual" 
                                                            VerticalScrollableHeight="350" />
                                                        <SettingsEditing Mode="PopupEditForm" 
                                                            PopupEditFormHorizontalAlign="WindowCenter"
                                                            PopupEditFormVerticalAlign="WindowCenter" 
                                                            PopupEditFormWidth="800px" 
                                                            PopupEditFormModal="True" 
                                                            PopupEditFormShowHeader="False"></SettingsEditing>
                                                        <SettingsPager PageSize="20">
                                                            <PageSizeItemSettings Visible="true" />
                                                        </SettingsPager>
                                                        <Settings ShowFooter="True" />
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="nspartcost" SummaryType="Average" />
                                                            <dx:ASPxSummaryItem FieldName="qtyplanned" SummaryType="Sum" />
                                                            <dx:ASPxSummaryItem FieldName="qtyused"  SummaryType="Sum" />
                                                        </TotalSummary>
                                                                <SettingsPopup>
                                                                    <EditForm Width="600px" Modal="true" />
                                                                </SettingsPopup>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <div style="padding: 4px 4px 3px 4px; width: 600px">
                                                                            <dx:ASPxFormLayout Width="100%" Theme="Mulberry" ID="PartEditLayout" runat="server">
                                                                                <Items>
                                                                                    <dx:LayoutGroup Name="PartEditGroup" Caption="Item Edit"  ColCount="2">
                                                                                        <Items>
                                                                                            <dx:LayoutItem Name="liPartId" Caption="Part ID:" ColSpan="2" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxTextBox ID="txtPartIDEdit" 
                                                                                                                           Width="100%" 
                                                                                                                           MaxLength="50"
                                                                                                                           ClientInstanceName="txtPartIDEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "nspartid" 
                                                                                                                           ValueField = "nspartid"
                                                                                                                           Value='<%# Bind("nspartid") %>'
                                                                                                                           ValueType="System.String">
                                                                                                        </dx:ASPxTextBox>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>       
                                                                                            <dx:LayoutItem Name="liPartDesc" Caption="Description:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxMemo ID="txtPartDescriptionEdit" 
                                                                                                                           Width="100%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtPartDescriptionEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "NSPartDescr" 
                                                                                                                           ValueField = "NSPartDescr"
                                                                                                                           Value='<%# Bind("NSPartDescr") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           Height="75px">
                                                                                                        </dx:ASPxMemo>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>       
                                                                                            <dx:LayoutItem Name="liPartMiscRef" Caption="Misc. Ref:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxMemo ID="txtPartMiscRefEdit" 
                                                                                                                           Width="100%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtPartMiscRefEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "miscrefnum" 
                                                                                                                           ValueField = "miscrefnum"
                                                                                                                           Value='<%# Bind("miscrefnum") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           Height="75px">
                                                                                                        </dx:ASPxMemo>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>      
                                                                                            <dx:LayoutItem Name="liPartMfgPartID" Caption="Mfg. Part ID:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtPartMfgPartId" 
                                                                                                                           Width="100%" 
                                                                                                                           MaxLength="50"
                                                                                                                           ClientInstanceName="txtPartMfgPartId"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "NSMfgPartID" 
                                                                                                                           ValueField = "NSMfgPartID"
                                                                                                                           Value='<%# Bind("NSMfgPartID") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           ReadOnly="True">
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>  
                                                                                            <dx:LayoutItem Name="liPartEst" Caption="Est. Qty:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtPartEstQty" 
                                                                                                                          ClientInstanceName="txtPartEstQty"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "qtyplanned" 
                                                                                                                          ValueField = "qtyplanned"
                                                                                                                          Value='<%# Bind("qtyplanned") %>'
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          NullText="Enter Est. Units"
                                                                                                                          MinValue="0"
                                                                                                                          HorizontalAlign="Right"
                                                                                                                          MaxValue="79228162514264337593543950335"
                                                                                                                          AllowMouseWheel="False">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liStoreroomID" Caption="Storeroom:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxComboBox ID="comboSrParts" 
                                                                                                                         runat="server" 
                                                                                                                         Width="100%" 
                                                                                                                         ValueType="System.Int32" 
                                                                                                                         ValueField="n_partatlocid" 
                                                                                                                         DropDownStyle="DropDown" 
                                                                                                                         Theme="Mulberry" 
                                                                                                                         TextField="Storeroom" 
                                                                                                                         Value='<%# Bind("n_partatlocid") %>'
                                                                                                                         DisplayFormatString="{0}"
                                                                                                                         DropDownButton-Enabled="True" 
                                                                                                                         AutoPostBack="False" 
                                                                                                                         OnItemsRequestedByFilterCondition="ComboStoreroomPart_OnItemsRequestedByFilterCondition_SQL" 
                                                                                                                         OnItemRequestedByValue="ComboStoreroomPart_OnItemRequestedByValue_SQL" 
                                                                                                                         ClientInstanceName="comboSrParts">
                                                                                                            <Columns>
                                                                                                                <dx:ListBoxColumn FieldName="n_storeroomid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="n_masterpartid" Visible="False" />
                                                                                                                <dx:ListBoxColumn FieldName="Storeroom" Caption="Storeroom ID" Width="75px" ToolTip="M-PET.NET Storeroom ID"/>
                                                                                                                <dx:ListBoxColumn FieldName="Storeroom Desc" Caption="Description" Width="150px" ToolTip="M-PET.NET Storeroom Description"/>
                                                                                                                <dx:ListBoxColumn FieldName="Qty" Caption="Qty." Width="75px" ToolTip="M-PET.NET Storeroom Part Quantity On Hand"/>
                                                                                                                <dx:ListBoxColumn FieldName="aisle" Caption="Aisle" Width="75px" ToolTip="M-PET.NET Storeroom Part Aisle Location"/>
                                                                                                                <dx:ListBoxColumn FieldName="shelf" Caption="Shelf" Width="75px" ToolTip="M-PET.NET Storeroom Part Shelf Location"/>
                                                                                                                <dx:ListBoxColumn FieldName="bin" Caption="Bin" Width="75px" ToolTip="M-PET.NET Storeroom Part Bin Location"/>
                                                                                                                <dx:ListBoxColumn FieldName="n_partatlocid" Visible="False" />
                                                                                                            </Columns>
                                                                                                        </dx:ASPxComboBox>                                                                                                                                                                                                                
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>                                                                                            
                                                                                            <dx:LayoutItem Name="liPartAct" Caption="Act. Qty:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit ID="txtPartActQty" 
                                                                                                                         ClientInstanceName="txtPartActQty"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "qtyused" 
                                                                                                                         ValueField = "qtyused"
                                                                                                                         Value='<%# Bind("qtyused") %>'
                                                                                                                         ValueType="System.Decimal"
                                                                                                                         DisplayFormatString="F2"
                                                                                                                         NullText="Enter Act. Units"
                                                                                                                         MinValue="0"
                                                                                                                         HorizontalAlign="Right"
                                                                                                                         MaxValue="79228162514264337593543950335"
                                                                                                                         AllowMouseWheel="False">
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liPartDate" Caption="Work Date:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxDateEdit ID="txtPartDateUsedEdit" 
                                                                                                                         ClientInstanceName="txtPartDateUsedEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         Width="100%"        
                                                                                                                         runat="server" 
                                                                                                                         TextField = "WorkDate" 
                                                                                                                         ValueField = "WorkDate"
                                                                                                                         Value='<%# Bind("WorkDate") %>' 
                                                                                                                         ValueType="System.DateTime"
                                                                                                                         NullText="MM/DD/YYYY"
                                                                                                                         EditFormat="Custom"
                                                                                                                         HorizontalAlign="Center"
                                                                                                                         EditFormatString="MM/dd/yyyy">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxDateEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>                                                                                             
                                                                                            <dx:LayoutItem Name="liPartRate" Caption="Cost:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtPartRateEdit" 
                                                                                                                          ClientInstanceName="txtPartRateEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "nspartcost" 
                                                                                                                          ValueField = "nspartcost"
                                                                                                                          Value='<%# Bind("nspartcost") %>' 
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="C4"
                                                                                                                          NullText="Enter Equip. Rate"
                                                                                                                          MinValue="0"
                                                                                                                          HorizontalAlign="Right"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>                                                                                     

                                                                                            
                                                                                            
                                                                                        </Items>
                                                                                    </dx:LayoutGroup>
                                                                                </Items>
                                                                            </dx:ASPxFormLayout>                                                                                                        
                                                                        </div>
                                                                        <div style="padding: 2px 2px 2px 2px; text-align: right;">
                                                                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" OnInit="HideDefaultEditButtons"  ReplacementType="EditFormUpdateButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" OnInit="HideDefaultEditButtons" ReplacementType="EditFormCancelButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxButton ID="btnUpdatePart" AutoPostBack="false" runat="server" CssClass="button" Text="Update" >
                                                                                <ClientSideEvents Click="function (s, e) { OnPartUpateClick(s, e); }" />                                                                                
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>
                                                                            <dx:ASPxButton ID="btnCancelPart" AutoPostBack="False" runat="server" Text="Cancel" CssClass="button">
                                                                                <ClientSideEvents Click="function (s, e) { OnPartCancelClick(s, e); }" />
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton> 
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                        <Templates>
                                                                <FooterRow>
                                                                    <dx:ASPxButton runat="server" ID="AddNewPartButton" OnClick="AddNewItemButton_Click" Theme="Mulberry" Text="Add New Part">
                                                        
                                                                    </dx:ASPxButton>
                                                                    <dx:ASPxButton runat="server" ID="DeletePartButton" OnClick="DeleteItems_Click" Theme="Mulberry" Text="Delete Part"></dx:ASPxButton>
                                                                </FooterRow>
                                                         </Templates>
                                                    </dx:ASPxGridView>      
                                                    <asp:SqlDataSource ID="PartDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        DECLARE @NullDate DATETIME
                                                            SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)

                                                        --Return Jobstep Part Data
                                                            SELECT  tbl_JobParts.n_jobpartid,
                                                                    tbl_JobParts.n_masterpartid,
                                                                    tbl_JobParts.n_storeroomid,
                                                                    tbl_JobParts.b_nonstocked,
                                                                    tbl_JobParts.miscrefnum,
                                                                    tbl_JobParts.nspartid,
                                                                    tbl_JobParts.nspartcost,
                                                                    tbl_JobParts.NSMfgPartID,
                                                                    tbl_JobParts.NSPartDescr,
                                                                    tbl_JobParts.qtyplanned,
                                                                    tbl_JobParts.qtyused,
                                                                    tbl_JobParts.n_MfgPartID,
                                                                    tbl_JobParts.DMRKey,
                                                                    case tbl_JobParts.WorkDate
                                                                    when @NullDate THEN NULL
                                                                    ELSE tbl_JobParts.WorkDate
                                                                    END AS 'WorkDate',
                                                                    tbl_Storerooms.storeroomid,
                                                                    CASE tbl_JobParts.WorkDate
                                                                      WHEN @NullDate THEN ''
                                                                      ELSE CONVERT(VARCHAR, WorkDate, 101)
                                                                    END AS cWorkDate,
                                                                    tbl_TimeBatches.time_batchid,
                                                                    tbl_Transactions.xactionnum,
                                                                    tbl_JobParts.n_partatlocid,
                                                                    tbl_FndSrc.n_FundSrcCodeID,
                                                                    tbl_FndSrc.FundSrcCodeID,
                                                                    tbl_WorkOrderCodes.n_WorkOrderCodeID,
                                                                    tbl_WorkOrderCodes.WorkOrderCodeID,
                                                                    tbl_WorkOp.n_WorkOpID,
                                                                    tbl_WorkOp.WorkOpID,
                                                                    tbl_OrgCode.n_OrganizationCodeID,
                                                                    tbl_OrgCode.OrganizationCodeID,
                                                                    tbl_FundGrp.n_FundingGroupCodeID,
                                                                    tbl_FundGrp.FundingGroupCodeID,
                                                                    tbl_ObjectCodes.n_ObjectCodeID,
                                                                    tbl_ObjectCodes.ObjectCodeID,
                                                                    tbl_ControlSections.n_ControlSectionID,
                                                                    tbl_ControlSections.ControlSectionID,
                                                                    tbl_EquipNum.n_EquipmentNumberID,
                                                                    tbl_EquipNum.EquipmentNumberID,
                                                                    tbl_JobParts.PartType as 'Entered From',
                                                                    tbl_JobParts.n_StoresIssueID,
                                                                    tbl_JobParts.n_StoresIssueItemid,
			                                                        tbl_Locations.aisle,
			                                                        tbl_Locations.shelf,
			                                                        tbl_Locations.bin
                                                            FROM    [dbo].[JobParts] tbl_JobParts
                                                                    LEFT JOIN ( SELECT  dbo.Storerooms.n_storeroomid,
                                                                                        dbo.Storerooms.storeroomid
                                                                                FROM    dbo.Storerooms
                                                                              ) tbl_Storerooms ON tbl_Jobparts.n_storeroomid = tbl_Storerooms.n_storeroomid
                                                                    LEFT JOIN ( SELECT  dbo.time_batches.RecordID,
                                                                                        dbo.time_batches.time_batchid
                                                                                FROM    dbo.time_batches
                                                                              ) tbl_TimeBatches ON tbl_JobParts.DMRKey = tbl_TimeBatches.RecordID
                                                                    LEFT JOIN ( SELECT  tbl_XactionItems.n_jobpartid,
                                                                                        MIN(tbl_XactionItems.n_xactionid) AS n_xactionid,
                                                                                        MIN(tbl_PartXActions.xactionnum) AS xactionnum
                                                                                FROM    dbo.xactionitems tbl_XactionItems
                                                                                        INNER JOIN ( SELECT dbo.partxactions.n_xactionid,
                                                                                                            dbo.partxactions.xactionnum
                                                                                                     FROM   dbo.partxactions
                                                                                                     WHERE  dbo.partxactions.xactiontype = 'CHECKOUT'
                                                                                                   ) tbl_PartXActions ON tbl_XactionItems.n_xactionid = tbl_PartXActions.n_xactionid
                                                                                GROUP BY tbl_XactionItems.n_jobpartid
                                                                              ) tbl_Transactions ON tbl_JobParts.n_jobpartid = tbl_Transactions.n_jobpartid
                                                                    INNER JOIN ( SELECT dbo.FundSrcCodes.n_FundSrcCodeID,
                                                                                        dbo.FundSrcCodes.FundSrcCodeID
                                                                                 FROM   dbo.FundSrcCodes
                                                                               ) tbl_FndSrc ON tbl_JobParts.n_FundSrcCodeID = tbl_FndSrc.n_FundSrcCodeID
                                                                    INNER JOIN ( SELECT dbo.WorkOrderCodes.n_WorkOrderCodeID,
                                                                                        dbo.WorkOrderCodes.WorkOrderCodeID
                                                                                 FROM   dbo.WorkOrderCodes
                                                                               ) tbl_WorkOrderCodes ON tbl_JobParts.n_WorkOrderCodeID = tbl_WorkOrderCodes.n_WorkOrderCodeID
                                                                    INNER JOIN ( SELECT dbo.WorkOperations.n_WorkOpID,
                                                                                        dbo.WorkOperations.WorkOpID
                                                                                 FROM   dbo.WorkOperations
                                                                               ) tbl_WorkOp ON tbl_JobParts.n_WorkOpID = tbl_WorkOp.n_WorkOpID
                                                                    INNER JOIN ( SELECT dbo.OrganizationCodes.n_OrganizationCodeID,
                                                                                        dbo.OrganizationCodes.OrganizationCodeID
                                                                                 FROM   dbo.OrganizationCodes
                                                                               ) tbl_OrgCode ON tbl_JobParts.n_OrganizationCodeID = tbl_OrgCode.n_OrganizationCodeID
                                                                    INNER JOIN ( SELECT dbo.FundingGroupCodes.n_FundingGroupCodeID,
                                                                                        dbo.FundingGroupCodes.FundingGroupCodeID
                                                                                 FROM   dbo.FundingGroupCodes
                                                                               ) tbl_FundGrp ON tbl_JobParts.n_FundingGroupCodeID = tbl_FundGrp.n_FundingGroupCodeID
                                                                    INNER JOIN ( SELECT dbo.ObjectCodes.n_ObjectCodeID,
                                                                                        dbo.ObjectCodes.ObjectCodeID
                                                                                 FROM   dbo.ObjectCodes
                                                                               ) tbl_ObjectCodes ON tbl_JobParts.n_ObjectCodeID = tbl_ObjectCodes.n_ObjectCodeID
                                                                    INNER JOIN ( SELECT dbo.ControlSections.n_ControlSectionID,
                                                                                        dbo.ControlSections.ControlSectionID
                                                                                 FROM   dbo.ControlSections
                                                                               ) tbl_ControlSections ON tbl_JobParts.n_ControlSectionID = tbl_ControlSections.n_ControlSectionID
                                                                    INNER JOIN ( SELECT dbo.EquipmentNumber.n_EquipmentNumberID,
                                                                                        dbo.EquipmentNumber.EquipmentNumberID
                                                                                 FROM   dbo.EquipmentNumber
                                                                               ) tbl_EquipNum ON tbl_JobParts.n_EquipmentNumberID = tbl_EquipNum.n_EquipmentNumberID
                                                                    INNER JOIN ( SELECT dbo.PartsAtLocation.n_partatlocid,
                                                                                        dbo.PartsAtLocation.aisle,
								                                                        dbo.PartsAtLocation.shelf,
								                                                        dbo.PartsAtLocation.bin
                                                                                 FROM   dbo.PartsAtLocation
                                                                               ) tbl_Locations ON tbl_JobParts.n_partatlocid = tbl_Locations.n_partatlocid
                                                            WHERE   ( tbl_JobParts.n_jobid = @JobID )
                                                                    AND ( tbl_JobParts.n_jobstepid = @JobstepID )">
                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="JobID" SessionField="editingJobID" />
                                                            <asp:SessionParameter DefaultValue="-1" Name="JobstepID" SessionField="editingJobStepID" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>                                                                                                                                                           
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage> <%--Parts--%>
                                        <dx:TabPage Text="EQUIPMENT" ToolTip="Allows Input Of Jobstep Equipment">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl12" runat="server">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" OnUnload="UpdatePanel_Unload">
                                                        <ContentTemplate>
                                                            <dx:ASPxGridView 
                                                                ID="EquipGrid" 
                                                                runat="server" 
                                                                Theme="Mulberry" 
                                                                KeyFieldName="n_JobEquipmentID" 
                                                                Width="98%" 
                                                                KeyboardSupport="True" 
                                                                ClientInstanceName="EquipGrid" 
                                                                AutoPostBack="True" 
                                                                EnableCallBacks="true"
                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                SettingsPager-Mode="ShowPager" 
                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                DataSourceID="EquipDataSource"
                                                                OnRowUpdating="EquipGrid_RowUpdating"
                                                                OnDataBound="EquipGridBound">
                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                    <Row CssClass="gridViewRow"></Row>

                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                </Styles>
                                                                <ClientSideEvents RowClick="function(s, e) {
                                                                        EquipGrid.GetRowValues(e.visibleIndex, 'n_JobEquipmentID;DMRKey', OnGetEquipRowId);
                        
                                                                    }" 
                                                                                  RowDblClick="function(s, e) {
                                                                    s.StartEditRow(e.visibleIndex);
                                                                }" />
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowEditButton="True" Visible="false" VisibleIndex="0" />
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobEquipmentID" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobID" ReadOnly="True" Visible="false" VisibleIndex="2">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_jobstepid" ReadOnly="True" Visible="false" VisibleIndex="3">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_MaintObjectID" ReadOnly="True" Visible="false" VisibleIndex="4">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="JobEquipID" ReadOnly="True" Caption="Equip. ID" ToolTip="Jobstep Equipment ID" SortOrder="Ascending" Width="200px" VisibleIndex="5">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                            <DataItemTemplate>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink1" NavigateUrl="javascript:void(0)" runat="server" Text='<%# Eval("JobEquipID") %>' Width="100%" Theme="Mulberry">
                                                                                <ClientSideEvents Click="onHyperLinkClick" />
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="EquipDescr" ReadOnly="True" Caption="Description" ToolTip="Jobstep Equipment Description" Width="300px" VisibleIndex="6">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="EquipCost" Caption="Rate" ToolTip="Jobstep Equipment Rate" Width="150px" VisibleIndex="7">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="c4" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="qtyplanned" Caption="Est. Units" ToolTip="Jobstep Equipment Estimated Units" Width="150px" VisibleIndex="8">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="qtyused" Caption="Act. Units" ToolTip="Jobstep Equipment Actual Units" Width="150px" VisibleIndex="9">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="MiscellaneousReference" Caption="Misc. Ref." ToolTip="Jobstep Equipment Misc. Ref." Width="150px" VisibleIndex="10">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="DMRKey" ReadOnly="True" Visible="false" VisibleIndex="11">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="WorkDate" Caption="Work Date" VisibleIndex="12"  Width="150px">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>                                                                                                                                                                     
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="LinkedDMR" ReadOnly="True" Caption="Linked DMR" Width="100px" VisibleIndex="13">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="StartMeter" Visible="false" Caption="Start Mtr." ToolTip="Jobstep Equipment Starting Meter" Width="150px"  VisibleIndex="14">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="EndMeter" Visible="false" Caption="End Mtr." ToolTip="Jobstep Equipment Ending Meter" Width="150px"  VisibleIndex="15">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                <SettingsDataSecurity AllowDelete="False" AllowEdit="True" AllowInsert="True" />
                                                                <SettingsEditing Mode="PopupEditForm" 
                                                                                 PopupEditFormHorizontalAlign="WindowCenter"
                                                                                 PopupEditFormVerticalAlign="WindowCenter" 
                                                                                 PopupEditFormWidth="800px" 
                                                                                 PopupEditFormModal="True" 
                                                                                 PopupEditFormShowHeader="False"></SettingsEditing>
                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                <SettingsPager PageSize="10">
                                                                    <PageSizeItemSettings Visible="true" />
                                                                </SettingsPager>
                                                                <Settings ShowFooter="True" />
                                                                <TotalSummary>
                                                                    <dx:ASPxSummaryItem FieldName="EquipCost" SummaryType="Average" />
                                                                    <dx:ASPxSummaryItem FieldName="qtyplanned" SummaryType="Sum" />
                                                                    <dx:ASPxSummaryItem FieldName="qtyused"  SummaryType="Sum" />
                                                                </TotalSummary>
                                                                <SettingsPopup>
                                                                    <EditForm Width="800px" Modal="true" />
                                                                </SettingsPopup>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <div style="padding: 4px 4px 3px 4px; width: 600px">
                                                                            <dx:ASPxFormLayout Width="98%" Theme="Mulberry" ID="EquipEditLayout" runat="server">
                                                                                <Items>
                                                                                    <dx:LayoutGroup Name="EquipEditGroup" Caption="Item Edit"  ColCount="4">
                                                                                        <Items>
                                                                                            <dx:LayoutItem Name="liEquipId" Caption="Equip. ID:" ColSpan="1" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtEquipIDEdit" 
                                                                                                                           Width="98%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtEquipIDEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "JobEquipID" 
                                                                                                                           ValueField = "JobEquipID"
                                                                                                                           Value='<%# Bind("JobEquipID") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           ReadOnly="True">
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>       
                                                                                            <dx:LayoutItem Name="liEquipRate" Caption="Rate:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtOtherRateEdit" 
                                                                                                                          ClientInstanceName="txtOtherRateEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "EquipCost" 
                                                                                                                          ValueField = "EquipCost"
                                                                                                                          Value='<%# Bind("EquipCost") %>'
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="C4"
                                                                                                                          NullText="Enter Equip. Rate"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>                                                                                     
                                                                                            <dx:LayoutItem Name="liEquipStartMtr" Caption="Start Meter:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtOtherStartMtrEdit" 
                                                                                                                          ClientInstanceName="txtOtherStartMtrEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "StartMeter" 
                                                                                                                          ValueField = "StartMeter"
                                                                                                                          Value='<%# Bind("StartMeter") %>'
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          NullText="Enter Start Meter"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem> 
                                                                                            <dx:LayoutItem Name="liEquipEndMtr" Caption="End Meter:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtOtehrEndMtrEdit" 
                                                                                                                          ClientInstanceName="txtOtehrEndMtrEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "EndMeter" 
                                                                                                                          ValueField = "EndMeter"
                                                                                                                          Value='<%# Bind("EndMeter") %>'
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          NullText="Enter End Meter"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem> 
                                                                                            <dx:LayoutItem Name="liEquipDesc" Caption="Description:" ColSpan="2" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtEquipEdit" 
                                                                                                                           Width="98%"
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtEquipEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "EquipDescr" 
                                                                                                                           ValueField = "EquipDescr"
                                                                                                                           Value='<%# Bind("EquipDescr") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           ReadOnly="True">
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liEquipEstUnits" Caption="Est. Units:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtEquipEstUnitsEdit" 
                                                                                                                          ClientInstanceName="txtEquipEstUnitsEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "qtyplanned" 
                                                                                                                          ValueField = "qtyplanned"
                                                                                                                          Value='<%# Bind("qtyplanned") %>'
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          NullText="Enter Est. Units"
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liEquipActUnits" Caption="Act. Units:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit ID="txtEquipActUnitsEdit" 
                                                                                                                         ClientInstanceName="txtEquipActUnitsEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "qtyused" 
                                                                                                                         ValueField = "qtyused"
                                                                                                                         Value='<%# Bind("qtyused") %>'
                                                                                                                         ValueType="System.Decimal"
                                                                                                                         DisplayFormatString="F2"
                                                                                                                         NullText="Enter Act. Units"
                                                                                                                         MinValue="0"
                                                                                                                         MaxValue="79228162514264337593543950335">
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liEquipDate" Caption="Work Date:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxDateEdit ID="txtEquipDateUsedEdit" 
                                                                                                                         ClientInstanceName="txtEquipDateUsedEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "WorkDate" 
                                                                                                                         ValueField = "WorkDate"
                                                                                                                         Value='<%# Bind("WorkDate") %>' 
                                                                                                                         ValueType="System.DateTime"
                                                                                                                         NullText="MM/DD/YYYY"
                                                                                                                         EditFormat="Custom"
                                                                                                                         EditFormatString="MM/dd/yyyy">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxDateEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liEquipMisc" Caption="Misc. Ref:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtEquipMiscRefEdit" 
                                                                                                                           ClientInstanceName="txtEquipMiscRefEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server" 
                                                                                                                           TextField = "MiscellaneousReference" 
                                                                                                                           ValueField = "MiscellaneousReference"
                                                                                                                           Value='<%# Bind("MiscellaneousReference") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           NullText="Enter Misc. Ref.">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                        </Items>
                                                                                    </dx:LayoutGroup>
                                                                                </Items>
                                                                            </dx:ASPxFormLayout>                                                                                                        
                                                                        </div>
                                                                        <div style="padding: 2px 2px 2px 2px; text-align: right;">
                                                                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" OnInit="HideDefaultEditButtons"  ReplacementType="EditFormUpdateButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" OnInit="HideDefaultEditButtons" ReplacementType="EditFormCancelButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxButton ID="btnUpdateEquip" AutoPostBack="false" runat="server" CssClass="button" Text="Update" >
                                                                                <ClientSideEvents Click="function (s, e) { OnEquipUpateClick(s, e); }" />                                                                                
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>
                                                                            <dx:ASPxButton ID="btnCancelEquip" AutoPostBack="False" runat="server" Text="Cancel" CssClass="button">
                                                                                <ClientSideEvents Click="function (s, e) { OnEquipCancelClick(s, e); }" />
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>                                                                            
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                                <Templates>
                                                                <FooterRow>
                                                                    <dx:ASPxButton runat="server" ID="AddNewEquipButton" OnClick="AddNewItemButton_Click" Theme="Mulberry" Text="Add New Equipment">
                                                                        <ClientSideEvents Click="ShowEquipPopup" />
                                                                    </dx:ASPxButton>
                                                                    <dx:ASPxButton runat="server" ID="DeleteEquipButton" OnClick="DeleteItems_Click" Theme="Mulberry" Text="Delete Equipment"></dx:ASPxButton>
                                                                </FooterRow>
                                                                </Templates>
                                                            </dx:ASPxGridView>                                                    
                                                            <asp:SqlDataSource ID="EquipDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        --Create Null Date
                                                            DECLARE @NullDate DATETIME
                                                            SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
    
                                                        --Return Data
                                                            SELECT  tbl_JobEquip.n_JobEquipmentID AS 'n_JobEquipmentID',
                                                                    tbl_JobEquip.n_JobID AS 'n_JobID',
                                                                    tbl_JobEquip.n_jobstepid AS 'n_jobstepid',
                                                                    tbl_JobEquip.n_MaintObjectID AS 'n_MaintObjectID',
                                                                    tbl_MO.objectid AS 'JobEquipID',
                                                                    tbl_JobEquip.EquipCost AS 'EquipCost',
                                                                    tbl_JobEquip.EquipDescr AS 'EquipDescr',
                                                                    tbl_JobEquip.MiscellaneousReference AS 'MiscellaneousReference',
                                                                    tbl_JobEquip.qtyused AS 'qtyused',
                                                                    tbl_JobEquip.qtyplanned AS 'qtyplanned',
                                                                    tbl_JobEquip.DMRKey AS 'DMRKey',
                                                                    CASE tbl_JobEquip.WorkDate
                                                                      WHEN @NullDate THEN NULL
                                                                      ELSE tbl_JobEquip.WorkDate
                                                                    END AS 'WorkDate',
                                                                    ISNULL(tbl_TimeBatches.time_batchid, 'N/A') AS 'LinkedDMR',
			                                                        tbl_JobEquip.start_meter AS 'StartMeter',
			                                                        tbl_JobEquip.end_meter AS 'EndMeter'
                                                            FROM    dbo.JobEquipment tbl_JobEquip
                                                                    INNER JOIN ( SELECT dbo.MaintenanceObjects.n_objectid,
                                                                                        dbo.MaintenanceObjects.objectid
                                                                                 FROM   dbo.MaintenanceObjects
                                                                               ) tbl_MO ON tbl_JobEquip.n_MaintObjectID = tbl_MO.n_objectid
                                                                    LEFT JOIN ( SELECT  dbo.time_batches.RecordID,
                                                                                        dbo.time_batches.time_batchid
                                                                                FROM    dbo.time_batches
                                                                              ) tbl_TimeBatches ON tbl_JobEquip.DMRKey = tbl_TimeBatches.RecordID
                                                            WHERE   ( tbl_JobEquip.n_JobID = @JobID )
                                                                    AND ( tbl_JobEquip.n_jobstepid = @JobStep )    
                                                          ">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobID" SessionField="editingJobID" />
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobStep" SessionField="editingJobStepID" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>                                                              
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage> <%--Equipment--%>
                                        <dx:TabPage Text="OTHER" ToolTip="Allows Input Of Jobstep Other">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl13" runat="server">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
                                                        <ContentTemplate>
                                                            <dx:ASPxGridView 
                                                                ID="OtherGrid" 
                                                                runat="server" 
                                                                Theme="Mulberry" 
                                                                KeyFieldName="n_JobOtherID" 
                                                                Width="98%" 
                                                                KeyboardSupport="True" 
                                                                ClientInstanceName="OtherGrid" 
                                                                AutoPostBack="True" 
                                                                EnableCallBacks="true"
                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                SettingsPager-Mode="ShowPager" 
                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                DataSourceID="OtherDataSource"
                                                                OnRowInserting="OtherGrid_RowInserting"
                                                                OnRowUpdating="OtherGrid_RowUpdating"
                                                                OnDataBound="OtherGridBound">
                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                    <Row CssClass="gridViewRow"></Row>

                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                </Styles>
                                                                <ClientSideEvents RowClick="function(s, e) {
                                                                        OtherGrid.GetRowValues(e.visibleIndex, 'n_JobOtherID', OnGetOtherRowId);
                        
                                                                    }" RowDblClick="function(s, e) {
                                                                    s.StartEditRow(e.visibleIndex);
                                                                }" BatchEditRowValidating="OtherGrid_BatchEditRowValidating" />
                                                                
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowEditButton="True" Visible="false" VisibleIndex="0" />
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobOtherID" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobID" ReadOnly="True" Visible="false" VisibleIndex="2">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_JobStepID" ReadOnly="True" Visible="false" VisibleIndex="3">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="OtherDescr" ReadOnly="False" Caption="Description" SortOrder="Ascending" Width="250px" VisibleIndex="4">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                         <DataItemTemplate>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink1" NavigateUrl="javascript:void(0)" runat="server" Text='<%# Eval("OtherDescr") %>' Width="100%" Theme="Mulberry">
                                                                                <ClientSideEvents Click="onHyperLinkClick" />
                                                                            </dx:ASPxHyperLink>
                                                                         </DataItemTemplate>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="OtherCost" ReadOnly="False" Caption="Cost" Width="150px" VisibleIndex="5">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="c4" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="qtyplanned" ReadOnly="False" Caption="Est. Units" Width="150px" VisibleIndex="6">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="qtyused" ReadOnly="False" Caption="Act. Units" Width="150px" VisibleIndex="7">
                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <PropertiesTextEdit DisplayFormatString="f2" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="MiscellaneousReference" ReadOnly="False" Caption="Misc. Ref." Width="150px" VisibleIndex="8">
                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="WorkDate" Caption="Work Date" ReadOnly="False" VisibleIndex="9"  Width="150px">
                                                                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>                                                                                                                                                                     
                                                                    </dx:GridViewDataDateColumn>
                                                                </Columns>
                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />
                                                                <SettingsDataSecurity AllowDelete="False" AllowEdit="True" AllowInsert="True" />
                                                                <SettingsEditing Mode="PopupEditForm" 
                                                                                 PopupEditFormHorizontalAlign="WindowCenter"
                                                                                 PopupEditFormVerticalAlign="WindowCenter" 
                                                                                 PopupEditFormWidth="800px" 
                                                                                 PopupEditFormModal="True" 
                                                                                 PopupEditFormShowHeader="False"></SettingsEditing>
                                                                <Settings VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                <SettingsPager PageSize="10">
                                                                    <PageSizeItemSettings Visible="true" />
                                                                </SettingsPager>
                                                                <Settings ShowFooter="True" />
                                                                <TotalSummary>
                                                                    <dx:ASPxSummaryItem FieldName="OtherCost" SummaryType="Average" />
                                                                    <dx:ASPxSummaryItem FieldName="qtyplanned" SummaryType="Sum" />
                                                                    <dx:ASPxSummaryItem FieldName="qtyused"  SummaryType="Sum" />
                                                                </TotalSummary>
                                                                <SettingsPopup>
                                                                    <EditForm Width="800px" Modal="true" />
                                                                </SettingsPopup>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <div style="padding: 4px 4px 3px 4px">
                                                                            <dx:ASPxFormLayout Width="98%" Theme="Mulberry" ID="OtherEditLayout" runat="server">
                                                                                <Items>
                                                                                    <dx:LayoutGroup Name="OtherEditGroup" Caption="Item Edit"  ColCount="3">
                                                                                        <Items>
                                                                                            <dx:LayoutItem Name="liOtherDesc" Caption="Description:" ColSpan="3" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtOtherEdit" 
                                                                                                                           Height="50px" 
                                                                                                                           Width="98%" 
                                                                                                                           MaxLength="254"
                                                                                                                           ClientInstanceName="txtOtherEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server"
                                                                                                                           TextField = "OtherDescr" 
                                                                                                                           ValueField = "OtherDescr"
                                                                                                                           Value='<%# Bind("OtherDescr") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           NullText="Enter Other Description">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                            <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                                                                                                <RequiredField IsRequired="True"/>
                                                                                                            </ValidationSettings>
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liOtherEstUnits" Caption="Est. Units:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit  ID="txtOtherEstUnitsEdit" 
                                                                                                                          ClientInstanceName="txtOtherEstUnitsEdit"
                                                                                                                          Theme="Mulberry"
                                                                                                                          runat="server" 
                                                                                                                          TextField = "qtyplanned" 
                                                                                                                          ValueField = "qtyplanned"
                                                                                                                          Value='<%# Bind("qtyplanned") %>'
                                                                                                                          ValueType="System.Decimal"
                                                                                                                          DisplayFormatString="F2"
                                                                                                                          NullText="Enter Est. Qty."
                                                                                                                          MinValue="0"
                                                                                                                          MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liOtherActUnits" Caption="Act. Units:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit ID="txtOtherActUnitsEdit" 
                                                                                                                         ClientInstanceName="txtOtherActUnitsEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "qtyused" 
                                                                                                                         ValueField = "qtyused"
                                                                                                                         Value='<%# Bind("qtyused") %>'
                                                                                                                         ValueType="System.Decimal"
                                                                                                                         DisplayFormatString="F2"
                                                                                                                         NullText="Enter Act. Qty."
                                                                                                                         MinValue="0"
                                                                                                                         MaxValue="79228162514264337593543950335">
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liOtherCost" Caption="Cost:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxSpinEdit ID="txtOtherCostEdit" 
                                                                                                                         ClientInstanceName="txtOtherCostEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "OtherCost" 
                                                                                                                         ValueField = "OtherCost"
                                                                                                                         Value='<%# Bind("OtherCost") %>'
                                                                                                                         ValueType="System.Decimal"
                                                                                                                         DisplayFormatString="C4"
                                                                                                                         NullText="Enter Cost"
                                                                                                                         MinValue="0"
                                                                                                                         MaxValue="79228162514264337593543950335">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxSpinEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liOtherDate" Caption="Date Used:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxDateEdit ID="txtOtherDateUsedEdit" 
                                                                                                                         ClientInstanceName="txtOtherDateUsedEdit"
                                                                                                                         Theme="Mulberry"
                                                                                                                         runat="server" 
                                                                                                                         TextField = "WorkDate" 
                                                                                                                         ValueField = "WorkDate"
                                                                                                                         Value='<%# Bind("WorkDate") %>'
                                                                                                                         ValueType="System.DateTime"
                                                                                                                         NullText="MM/DD/YYYY"
                                                                                                                         EditFormat="Custom"
                                                                                                                         EditFormatString="MM/dd/yyyy">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxDateEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                            <dx:LayoutItem Name="liOtherMisc" Caption="Misc Ref:" CaptionSettings-Location="Top">
                                                                                                <LayoutItemNestedControlCollection >
                                                                                                    <dx:LayoutItemNestedControlContainer>
                                                                                                        <dx:ASPxButtonEdit ID="txtOtherMiscRefEdit" 
                                                                                                                           ClientInstanceName="txtOtherMiscRefEdit"
                                                                                                                           Theme="Mulberry"
                                                                                                                           runat="server" 
                                                                                                                           TextField = "MiscellaneousReference" 
                                                                                                                           ValueField = "MiscellaneousReference"
                                                                                                                           Value='<%# Bind("MiscellaneousReference") %>'
                                                                                                                           ValueType="System.String"
                                                                                                                           NullText="Enter Misc. Ref.">
                                                                                                            <ClearButton Visibility="True"></ClearButton>
                                                                                                        </dx:ASPxButtonEdit>
                                                                                                    </dx:LayoutItemNestedControlContainer>
                                                                                                </LayoutItemNestedControlCollection>
                                                                                            </dx:LayoutItem>
                                                                                        </Items>
                                                                                    </dx:LayoutGroup>
                                                                                </Items>
                                                                            </dx:ASPxFormLayout>                                                                                                        
                                                                                   
                                                                        </div>
                                                                        <div style="padding: 2px 2px 2px 2px; text-align: right;">
                                                                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" OnInit="HideDefaultEditButtons"  ReplacementType="EditFormUpdateButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" OnInit="HideDefaultEditButtons" ReplacementType="EditFormCancelButton"
                                                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                            <dx:ASPxButton ID="btnUpdateOther" AutoPostBack="false" runat="server" CssClass="button" Text="Update" >
                                                                                <ClientSideEvents Click="function (s, e) { OnOtherUpateClick(s, e); }" />                                                                                
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>
                                                                            <dx:ASPxButton ID="btnCancelOther" AutoPostBack="False" runat="server" Text="Cancel" CssClass="button">
                                                                                <ClientSideEvents Click="function (s, e) { OnOtherCancelClick(s, e); }" />
                                                                                <HoverStyle CssClass="hover"></HoverStyle>
                                                                            </dx:ASPxButton>                                                                            
                                                                        </div>
                                                                    </EditForm>
                                                                </Templates>
                                                                <Templates>
                                                                    <FooterRow>
                                                                        <dx:ASPxButton runat="server" ID="AddNewOtherButton" OnClick="AddNewItemButton_Click" Theme="Mulberry" Text="Add New Other">
                                                                            
                                                                        </dx:ASPxButton>
                                                                        <dx:ASPxButton runat="server" ID="DeleteOtherButton" OnClick="DeleteItems_Click" Theme="Mulberry" Text="Delete Other"></dx:ASPxButton>
                                                                    </FooterRow>
                                                                </Templates>
                                                            </dx:ASPxGridView>                                                    
                                                            <asp:SqlDataSource ID="OtherDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                         --Create Null Date Variable
                                                            DECLARE @NullDate DATETIME
                                                            SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)

                                                        --Return Jobstep Other Records For Specified Job/Jobstep IDs
                                                            SELECT  tbl_JobOtehr.n_JobOtherID AS 'n_JobOtherID',
                                                                    tbl_JobOtehr.n_JobID AS 'n_JobID',
                                                                    tbl_JobOtehr.n_JobStepID AS 'n_JobStepID',
                                                                    tbl_JobOtehr.OtherDescr AS 'OtherDescr',
                                                                    tbl_JobOtehr.OtherCost AS 'OtherCost',
                                                                    tbl_JobOtehr.MiscellaneousReference AS 'MiscellaneousReference',
                                                                    tbl_JobOtehr.qtyused AS 'qtyused',
                                                                    tbl_JobOtehr.qtyplanned AS 'qtyplanned',
                                                                    CASE tbl_JobOtehr.WorkDate
                                                                      WHEN @NullDate THEN NULL
                                                                      ELSE tbl_JobOtehr.WorkDate
                                                                    END AS 'WorkDate'
                                                            FROM    dbo.JobOther tbl_JobOtehr
                                                            WHERE   ( tbl_JobOtehr.n_JobID = @JobID )
                                                                    AND ( tbl_JobOtehr.n_jobstepid = @JobStep )  
                                                          "
                                                                               UpdateCommand="    UPDATE  [dbo].[JobOther]
                                                            SET     [n_JobID] = @n_JobID,
                                                                    [n_JobStepID] = @n_JobStepID,
                                                                    [OtherDescr] = @OtherDescr,
                                                                    [OtherCost] = @OtherCost,
                                                                    [MiscellaneousReference] = @MiscellaneousReference,
                                                                    [qtyused] = @qtyused,
                                                                    [qtyplanned] = @qtyplanned,
                                                                    [WorkDate] = @WorkDate,
                                                                    [ModifiedBy] = @ModifiedBy,
                                                                    [ModifiedOn] = GETDATE()
                                                            WHERE   [n_JobOtherID] = @n_JobOtherID" >
                                                                <SelectParameters>
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobID" SessionField="editingJobID" />
                                                                    <asp:SessionParameter DefaultValue="-1" Name="JobStep" SessionField="editingJobStepID" />
                                                                </SelectParameters>
                                                                <UpdateParameters>
                                                                    <asp:SessionParameter DefaultValue="-1" Type="Int32" Name="n_JobID" SessionField="editingJobID" />
                                                                    <asp:SessionParameter DefaultValue="-1" Type="Int32" Name="n_JobStepID" SessionField="editingJobStepID" />
                                                                    <asp:Parameter Name="OtherDescr" Type="String"  />
                                                                    <asp:Parameter Name="OtherCost" Type="Decimal" />
                                                                    <asp:Parameter Name="MiscellaneousReference" Type="String" />
                                                                    <asp:Parameter Name="qtyused" Type="Decimal" />
                                                                    <asp:Parameter Name="qtyplanned" Type="Decimal" />
                                                                    <asp:Parameter Name="WorkDate" Type="DateTime" />
                                                                    <asp:SessionParameter DefaultValue="-1" Type="Int32" Name="ModifiedBy" SessionField="UserID" />
                                                                    <asp:Parameter Name="n_JobOtherID" Type="Int32" />
                                                                </UpdateParameters>
                                                            </asp:SqlDataSource>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>                                                                                                        
                                                </dx:ContentControl>
                                            </ContentCollection>
                                        </dx:TabPage> <%--Other--%>
                                        <dx:TabPage Text="ATTACHMENTS" ToolTip="Allows Input Of Jobstep Attachments">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControl14" runat="server">
                                                    <dx:ASPxFormLayout ID="ASPxFormLayout2"  Theme="Mulberry" runat="server" Width="98%" Paddings="0,0">
                                                        <Items>
                                                            <dx:LayoutGroup Width="100%" ColCount="2" GroupBoxDecoration="None">
                                                                <Items>
                                                                    <dx:LayoutItem Caption="" ShowCaption="False" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <div id="PhotoContainer" runat="server">
                                                                                    <div class="uploadContainer">
                                                                                        <dx:ASPxUploadControl 
                                                                                            Theme="Mulberry"
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
                                                                                            
                                                                                            <BrowseButton Text="Browse">
                                                                                            </BrowseButton>
                                                                                            <AdvancedModeSettings 
                                                                                                EnableDragAndDrop="True" 
                                                                                                EnableFileList="false" 
                                                                                                EnableMultiSelect="true">
                                                                                                <FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
                                                                                            </AdvancedModeSettings>
                                                                                            <ValidationSettings 
                                                                                                MaxFileSize="4194304" 
                                                                                                AllowedFileExtensions=".jpg,.jpeg,.gif,.png">
                                                                                            </ValidationSettings>
                                                                                            <ClientSideEvents 
                                                                                                FileUploadComplete="onFileUploadComplete"/>
                                                                                        </dx:ASPxUploadControl>
                                                                                    </div>
                                                                                </div>
                                                                            </dx:LayoutItemNestedControlContainer>
                                                                        </LayoutItemNestedControlCollection>

                                                                        <CaptionSettings Location="Top"></CaptionSettings>
                                                                    </dx:LayoutItem>                                                                    
                                                                    <dx:LayoutItem Caption="" ShowCaption="False" ColSpan="2" CaptionSettings-Location="Top">
                                                                        <LayoutItemNestedControlCollection >
                                                                            <dx:LayoutItemNestedControlContainer>
                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" OnUnload="UpdatePanel_Unload">
                                                                                    <ContentTemplate>
                                                                                        <dx:ASPxGridView 
                                                                                            ID="AttachmentGrid" 
                                                                                            runat="server" 
                                                                                            Theme="Mulberry" 
                                                                                            KeyFieldName="LocationOrURL" 
                                                                                            Width="98%" 
                                                                                            KeyboardSupport="True" 
                                                                                            ClientInstanceName="AttachmentGrid" 
                                                                                            AutoPostBack="true" 
                                                                                            OnDataBound="AttachmentGridBound"
                                                                                            Settings-HorizontalScrollBarMode="Auto" 
                                                                                            SettingsPager-Mode="ShowPager" 
                                                                                            SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                            SettingsBehavior-AllowFocusedRow="True"
                                                                                            EnableCallBacks="true" AutoGenerateColumns="False" DataSourceID="AttachmentDataSource">
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
                                                                                                     <DataItemTemplate>
                                                                                                        <dx:ASPxHyperLink ID="ASPxHyperLink1" NavigateUrl="javascript:void(0)" runat="server" Text='<%# Eval("ShortName") %>' Width="100%" Theme="Mulberry">
                                                                                                            <ClientSideEvents Click="onHyperLinkClick" />
                                                                                                        </dx:ASPxHyperLink>
                                                                                                     </DataItemTemplate>
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
                                                                                            </SettingsPager>
                                                                                            <Templates>
                                                                                                <FooterRow>
                                                                                                    
                                                                                                    <dx:ASPxButton runat="server" ID="DeleteAttachmentButton" OnClick="DeleteAttachmentButton_Click" Theme="Mulberry" Text="Delete Crew Member"></dx:ASPxButton>
                                                                                                </FooterRow>
                                                                                            </Templates>
                                                                                        </dx:ASPxGridView>      
                                                                                        <asp:SqlDataSource ID="AttachmentDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="SELECT [ID], [nJobID], [nJobstepID], [DocType], [Description], [LocationOrURL], [ShortName] FROM [Attachments] WHERE (([nJobID] = @nJobID) AND ([nJobstepID] = @nJobstepID))">
                                                                                            <SelectParameters>
                                                                                                <asp:SessionParameter DefaultValue="0" Name="nJobID" SessionField="editingJobID" Type="Int32" />
                                                                                                <asp:SessionParameter DefaultValue="-1" Name="nJobstepID" SessionField="editingJobStepID" Type="Int32" />
                                                                                            </SelectParameters>
                                                                                        </asp:SqlDataSource>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
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
                                        </dx:TabPage> <%--Attachments--%>
                                    </TabPages> <%--Planned Job tabs--%>
                                </dx:ASPxPageControl>
                                <dx:ASPxPopupControl ID="AddCrewPopup" ClientInstanceName="AddCrewPopup" ShowCloseButton="true" ShowHeader="false" HeaderText=""
                                                     CloseAnimationType="Fade" PopupAnimationType="Fade" runat="server" ShowShadow="true" ShowFooter="true"
                                                     CloseAction="None" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="600px">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxLogonLayout" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Crew Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="CrewLookupGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="nUserID" 
                                                                                Width="98%" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="CrewLookupGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="CrewLookupDataSource"
                                                                                SelectionMode="Multiple">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="nUserID" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UserID" ReadOnly="True" Caption="Username" SortOrder="Ascending" Width="100px" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Name" ReadOnly="True" Caption="Full Name"  Width="200px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Area" ReadOnly="True" Caption="Area ID"  Width="100px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Position Code" ReadOnly="True"  Caption="Position"  Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Person Class" ReadOnly="True" Caption="Person Class"  Width="100px" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Location" ReadOnly="True"  Caption="Location"  Width="100px" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="LaborClassID" ReadOnly="True"  Visible="False"  Width="100px" VisibleIndex="7">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="LaborClass" ReadOnly="True" Caption="Laborclass ID"  Width="100px" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="GroupID" ReadOnly="True" Visible="False"  Width="100px" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Group" ReadOnly="True" Caption="Group ID"  Width="100px" VisibleIndex="10">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>

                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="CrewLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                        ;
                                                            WITH    cte_Personnel
                                                                      AS ( SELECT   tbl_MPETUsers.UserID AS nUserID ,
                                                                                    tbl_MPETUsers.Username AS UserID ,
                                                                                    tbl_MPETUsers.FirstName AS Firstname ,
                                                                                    tbl_MPETUsers.LastName AS LastName ,
                                                                                    LTRIM(RTRIM(LTRIM(RTRIM(tbl_MPETUsers.LastName)) + ', ' + LTRIM(RTRIM(tbl_MPETUsers.FirstName)))) AS [Name] ,
                                                                                    tbl_Areas.areaid AS 'Area' ,
                                                                                    tbl_PositionCodes.positioncodeid AS 'Position Code' ,
                                                                                    tbl_PersonClasses.personclassid AS 'Person Class' ,
                                                                                    tbl_Locations.locationid AS 'Location' ,
                                                                                    tbl_MPETUsers.AreaID ,
                                                                                    tbl_MPETUsers.PositionCodeID ,
                                                                                    tbl_MPETUsers.PersonClassID ,
                                                                                    tbl_MPETUsers.LocationID ,
                                                                                    tbl_MPETUsers.Active ,
                                                                                    tbl_MPETUsers.LaborClassID AS 'laborclassid' ,
                                                                                    tbl_LaborClasses.laborclassid AS 'LaborClass',
                                                                                    tbl_MPETUsers.GroupID ,
                                                                                    tbl_Groups.groupid AS 'Group' 
                                                                           FROM     dbo.MPetUsers tbl_MPETUsers
                                                                                    INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                                                                        tblAreas.areaid
                                                                                                 FROM   dbo.Areas tblAreas
                                                                                               ) tbl_Areas ON tbl_MPETUsers.AreaID = tbl_Areas.n_areaid
                                                                                    INNER JOIN ( SELECT tblLocations.n_locationid ,
                                                                                                        tblLocations.locationid
                                                                                                 FROM   dbo.locations tblLocations
                                                                                               ) tbl_Locations ON tbl_MPETUsers.LocationID = tbl_Locations.n_locationid
                                                                                    INNER JOIN ( SELECT tblPositionCode.n_positioncodeid ,
                                                                                                        tblPositionCode.positioncodeid
                                                                                                 FROM   dbo.positioncodes tblPositionCode
                                                                                               ) tbl_PositionCodes ON tbl_MPETUsers.PositionCodeID = tbl_PositionCodes.n_positioncodeid
                                                                                    INNER JOIN ( SELECT tblPersonClasses.n_personclassid ,
                                                                                                        tblPersonClasses.personclassid
                                                                                                 FROM   dbo.personclasses tblPersonClasses
                                                                                               ) tbl_PersonClasses ON tbl_MPETUsers.PersonClassID = tbl_PersonClasses.n_personclassid
                                                                                    INNER JOIN ( SELECT tblLaborClasses.n_laborclassid ,
                                                                                                        tblLaborClasses.laborclassid
                                                                                                 FROM   dbo.laborclasses tblLaborClasses
                                                                                               ) tbl_LaborClasses ON tbl_MPETUsers.LaborClassID = tbl_LaborClasses.n_laborclassid
                                                                                    INNER JOIN ( SELECT tblGroups.n_groupid ,
                                                                                                        tblGroups.groupid
                                                                                                 FROM   dbo.groups tblGroups
                                                                                               ) tbl_Groups ON tbl_MPETUsers.GroupID = tbl_Groups.n_groupid
                                                                           WHERE    ( tbl_MPETUsers.UserID > 0 AND tbl_MPETUsers.Active=1)
							
                                                                         )
                                                                --Return Filtered Data
                                                            SELECT  cte_Personnel.nUserID ,
                                                                    cte_Personnel.UserID ,
                                                                    cte_Personnel.Name ,
                                                                    cte_Personnel.Area ,
                                                                    cte_Personnel.[Position Code] ,
                                                                    cte_Personnel.[Person Class] ,
                                                                    cte_Personnel.Location ,
                                                                    cte_Personnel.LaborClassID ,
                                                                    cte_Personnel.LaborClass ,
                                                                    cte_Personnel.GroupID ,
                                                                    cte_Personnel.[Group] 
                                                            FROM    cte_Personnel
                                                          ">
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="LogonButton" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddCrew_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="OkButton" AutoPostBack="True" runat="server" Text="Close" CssClass="button">
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
                                <dx:ASPxPopupControl ID="AddEquipPopup" ClientInstanceName="AddEquipPopup" ShowCloseButton="true" ShowHeader="false" HeaderText=""
                                                     CloseAnimationType="Fade" PopupAnimationType="Fade" runat="server" ShowShadow="true" ShowFooter="true"
                                                     CloseAction="None" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="600px">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Equipment Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="EquipLookupGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_objectid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="EquipLookupGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="EquipLookupDataSource"
                                                                                SelectionMode="Multiple">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>
                                                                                    <Row CssClass="gridViewRow"></Row>
                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_objectid" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="objectid" Caption="Object" Width="150px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="200px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Area" Caption="Area" Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectType" Caption="Obj. Type" Width="100px" ReadOnly="True" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectClass" Caption="Obj. Class" Width="100px" ReadOnly="True" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ChargeRate" Caption="Charge Rate" Width="100px" ReadOnly="True" VisibleIndex="7">
                                                                                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>

                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>
                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="EquipLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
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
                                                                            WITH    cte_MaintenanceObjects
                                                          AS ( SELECT   tbl_MaintObj.n_objectid ,
                                                                        tbl_MaintObj.objectid ,
                                                                        tbl_MaintObj.description ,
                                                                        tbl_Areas.areaid ,
                                                                        tbl_ObjectTypes.objtypeid ,
                                                                        tbl_ObjectClasses.objclassid ,
                                                                        tbl_MaintObj.charge_rate 
                                                               FROM     dbo.MaintenanceObjects tbl_MaintObj
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
                                                                        INNER JOIN ( SELECT tblObjectTypes.n_objtypeid ,
                                                                                            tblObjectTypes.objtypeid
                                                                                     FROM   dbo.objecttypes tblObjectTypes
                                                                                   ) tbl_ObjectTypes ON tbl_MaintObj.n_objtypeid = tbl_ObjectTypes.n_objtypeid
                                                                        INNER JOIN ( SELECT tblObjectClasses.n_objclassid ,
                                                                                            tblObjectClasses.objclassid
                                                                                     FROM   dbo.objectclasses tblObjectClasses
                                                                                   ) tbl_ObjectClasses ON tbl_MaintObj.n_objclassid = tbl_ObjectClasses.n_objclassid
                                                               WHERE    tbl_MaintObj.n_objectid > 0
						                                                AND tbl_MaintObj.b_chargeable = 'Y'
							                                            AND tbl_MaintObj.b_active = 'Y'
                                                             )
                                                    SELECT  cte_MaintenanceObjects.n_objectid AS 'n_objectid' ,
                                                            cte_MaintenanceObjects.objectid AS 'objectid' ,
                                                            cte_MaintenanceObjects.description AS 'description' ,
                                                            cte_MaintenanceObjects.areaid AS 'Area' ,
                                                            cte_MaintenanceObjects.objtypeid AS 'ObjectType' ,
                                                            cte_MaintenanceObjects.objclassid AS 'ObjectClass' ,
                                                            cte_MaintenanceObjects.charge_rate AS 'ChargeRate' 
                                                    FROM    cte_MaintenanceObjects
                                                          ">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" Type="Int32" />
                                                                                </SelectParameters>                                                        
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="btnAddEquip" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddEquip_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="btnCancelEquipAdd" AutoPostBack="True" runat="server" Text="Close" CssClass="button">
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
                                <dx:ASPxPopupControl ID="AddMemberPopup" ClientInstanceName="AddMemberPopup" ShowCloseButton="true" ShowHeader="false" HeaderText=""
                                                     CloseAnimationType="Fade" PopupAnimationType="Fade" runat="server" ShowShadow="true" ShowFooter="true"
                                                     CloseAction="None" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="600px">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout4" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Member Selection" Width="98%">
                                                            <Items>              
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="MemberLookupGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_objectid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="MemberLookupGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="MemberLookupDataSource"
                                                                                SelectionMode="Multiple">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>
                                                                                    <Row CssClass="gridViewRow"></Row>
                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_objectid" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="objectid" Caption="Object" Width="150px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="200px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Area" Caption="Area" Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectType" Caption="Obj. Type" Width="100px" ReadOnly="True" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectClass" Caption="Obj. Class" Width="100px" ReadOnly="True" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>

                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>
                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="MemberLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
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
                                                                            WITH    cte_MaintenanceObjects
                                                          AS ( SELECT   tbl_MaintObj.n_objectid ,
                                                                        tbl_MaintObj.objectid ,
                                                                        tbl_MaintObj.description ,
                                                                        tbl_Areas.areaid ,
                                                                        tbl_ObjectTypes.objtypeid ,
                                                                        tbl_ObjectClasses.objclassid 
                                                               FROM     dbo.MaintenanceObjects tbl_MaintObj
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
                                                                        INNER JOIN ( SELECT tblObjectTypes.n_objtypeid ,
                                                                                            tblObjectTypes.objtypeid
                                                                                     FROM   dbo.objecttypes tblObjectTypes
                                                                                   ) tbl_ObjectTypes ON tbl_MaintObj.n_objtypeid = tbl_ObjectTypes.n_objtypeid
                                                                        INNER JOIN ( SELECT tblObjectClasses.n_objclassid ,
                                                                                            tblObjectClasses.objclassid
                                                                                     FROM   dbo.objectclasses tblObjectClasses
                                                                                   ) tbl_ObjectClasses ON tbl_MaintObj.n_objclassid = tbl_ObjectClasses.n_objclassid
                                                               WHERE    tbl_MaintObj.n_objectid > 0
							                                            AND tbl_MaintObj.b_active = 'Y'
                                                             )
                                                    SELECT  cte_MaintenanceObjects.n_objectid AS 'n_objectid' ,
                                                            cte_MaintenanceObjects.objectid AS 'objectid' ,
                                                            cte_MaintenanceObjects.description AS 'description' ,
                                                            cte_MaintenanceObjects.areaid AS 'Area' ,
                                                            cte_MaintenanceObjects.objtypeid AS 'ObjectType' ,
                                                            cte_MaintenanceObjects.objclassid AS 'ObjectClass' 
                                                    FROM    cte_MaintenanceObjects
                                                          ">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" Type="Int32" />
                                                                                </SelectParameters>                                                        
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="btnAddMember" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddMember_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="btnCancelMemberAdd" AutoPostBack="True" runat="server" Text="Close" CssClass="button">
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
                                <dx:ASPxPopupControl ID="AddPartPopup" ClientInstanceName="AddPartPopup" ShowCloseButton="true" ShowHeader="false" HeaderText=""
                                                     CloseAnimationType="Fade" PopupAnimationType="Fade" runat="server" ShowShadow="true" ShowFooter="true"
                                                     CloseAction="None" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="600px" AllowDragging="true">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl>
                                            <div class="popup-text">
                                                <dx:ASPxFormLayout ID="ASPxFormLayout5" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Part Selection" Width="98%">
                                                            <Items>              
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="PartLookupGrid" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_masterpartid" 
                                                                                Width="600px" 
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="PartLookupGrid" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="PartLookupDataSource"
                                                                                SelectionMode="Multiple">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>
                                                                                    <Row CssClass="gridViewRow"></Row>
                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_masterpartid" ReadOnly="True" Visible="false" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="masterpartid" Caption="Masterpart" SortOrder="Ascending" ToolTip="M-PET.NET Masterpart ID" Width="100px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="Description" Caption="Description" ToolTip="M-PET.NET Masterpart Description" Width="200px" VisibleIndex="3">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="listcost" Caption="List $" ToolTip="M-PET.NET Masterpart List Cost" Width="100px" VisibleIndex="4">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="lastcost" Caption="Last $" ToolTip="M-PET.NET Masterpart Last Cost" Width="100px" ReadOnly="True" VisibleIndex="5">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="avgcost" Caption="Avg. $" ToolTip="M-PET.NET Masterpart Average Cost" Width="100px" ReadOnly="True" VisibleIndex="6">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_buyerid" ReadOnly="True" Visible="false" VisibleIndex="7">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_parttypeid" ReadOnly="True" Visible="false" VisibleIndex="8">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_prefmfgid" ReadOnly="True" Visible="false" VisibleIndex="9">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_prefvndid" ReadOnly="True" Visible="false" VisibleIndex="10">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_PrefMFGPartID" ReadOnly="True" Visible="false" VisibleIndex="11">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="n_PrefVndPartID" ReadOnly="True" Visible="false" VisibleIndex="12">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                                                                                    <dx:GridViewDataTextColumn FieldName="SpecialHandlingNotes" Caption="Notes" ToolTip="M-PET.NET Masterpart Special Handling Notes" Width="200px" VisibleIndex="13">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="BuyerComment" Caption="Comments" ToolTip="M-PET.NET Masterpart Buyers Comments" Width="200px" VisibleIndex="14">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="b_tool" Caption="Tool?" ToolTip="M-PET.NET Masterpart Tool Status Yes/No?" Width="75px" ReadOnly="True" VisibleIndex="15">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="b_SpecHandling" Caption="Spec. Hndl?" ToolTip="M-PET.NET Masterpart Special Handling Status Yes/No?" Width="75px" ReadOnly="True" VisibleIndex="16">
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="chkoutunits" ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="MRPRuleType"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="YieldPct"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ShelfLife"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="CountCycle"  ReadOnly="True" Visible="false">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="cPrefMfg" Caption="Pref. Mfg."  ToolTip="M-PET.NET Masterpart Preferred Manufacturer ID" Width="100px" VisibleIndex="17">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="PrefMFGPartID" Caption="Pref. Mfg. Part"  ToolTip="M-PET.NET Masterpart Preferred Manufacturer Part ID" Width="100px" VisibleIndex="18">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="cVendor" Caption="Pref. Vnd."  ToolTip="M-PET.NET Masterpart Preferred Vendor ID" Width="100px" VisibleIndex="19">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="PrefVndPartID" Caption="Pref. Vnd. Part"  ToolTip="M-PET.NET Masterpart Preferred Vendor Part ID" Width="100px" VisibleIndex="20">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UOI" Caption="UOI"  ToolTip="M-PET.NET Masterpart Units Of Issue" Width="100px" VisibleIndex="21">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="UOM" Caption="UOM"  ToolTip="M-PET.NET Masterpart Units Of Measure" Width="100px" VisibleIndex="22">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ObjectCodeID" Caption="Object Code"  ToolTip="M-PET.NET Masterpart Object Code ID" Width="100px" VisibleIndex="23">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="FlaggedRecordID" ReadOnly="True" Visible="false" VisibleIndex="12">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>

                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>
                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="PartLookupDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" SelectCommand="
                                                                            ;WITH    cte_Masterparts
                                                                              AS ( SELECT   tbl_Masterparts.n_masterpartid ,
                                                                                            tbl_Masterparts.masterpartid ,
                                                                                            tbl_Masterparts.Description ,
                                                                                            tbl_Masterparts.listcost ,
                                                                                            tbl_Masterparts.lastcost ,
                                                                                            tbl_Masterparts.avgcost ,
                                                                                            tbl_Masterparts.n_buyerid ,
                                                                                            tbl_Masterparts.n_parttypeid ,
                                                                                            tbl_Masterparts.n_prefmfgid ,
                                                                                            tbl_Masterparts.n_prefvndid ,
                                                                                            tbl_Masterparts.n_PrefMFGPartID ,
                                                                                            tbl_Masterparts.n_PrefVndPartID ,
                                                                                            tbl_Masterparts.SpecialHandlingNotes ,
                                                                                            tbl_Masterparts.BuyerComment ,
                                                                                            tbl_Masterparts.b_tool ,
                                                                                            tbl_Masterparts.b_SpecHandling ,
                                                                                            tbl_Masterparts.chkoutunits ,
                                                                                            tbl_Masterparts.MRPRuleType ,
                                                                                            tbl_Masterparts.YieldPct ,
                                                                                            tbl_Masterparts.ShelfLife ,
                                                                                            tbl_Masterparts.CountCycle ,
                                                                                            tbl_Manufacturer.mfgid ,
                                                                                            tbl_Vendors.vendorid ,
                                                                                            tbl_PartTypes.parttypeid ,
                                                                                            tbl_Masterparts.b_active ,
                                                                                            tbl_Masterparts.UnitOfIssue ,
                                                                                            tbl_Masterparts.UnitOfMeasure ,
                                                                                            tbl_MfgParts.mfgpartid ,
                                                                                            tbl_VendorParts.VendorPartID ,
                                                                                            tbl_ObjectCodes.n_ObjectCodeID ,
                                                                                            tbl_ObjectCodes.ObjectCodeID ,
                                                                                            ISNULL(tbl_IsFlaggedRecord.RecordID, -1) AS FlaggedRecordID
                                                                                   FROM     dbo.Masterparts tbl_Masterparts
                                                                                            INNER JOIN ( SELECT tbl_Mfg.n_mfgid ,
                                                                                                                tbl_Mfg.mfgid
                                                                                                         FROM   dbo.Manufacturers tbl_Mfg
                                                                                                       ) tbl_Manufacturer ON tbl_Masterparts.n_prefmfgid = tbl_Manufacturer.n_mfgid
                                                                                            INNER JOIN ( SELECT tbl_Vendors.n_vendorid ,
                                                                                                                tbl_Vendors.vendorid
                                                                                                         FROM   dbo.Vendors tbl_Vendors
                                                                                                       ) tbl_Vendors ON tbl_Masterparts.n_prefvndid = tbl_Vendors.n_vendorid
                                                                                            INNER JOIN ( SELECT tbl_PartTypes.n_parttypeid ,
                                                                                                                tbl_PartTypes.parttypeid
                                                                                                         FROM   dbo.PartTypes tbl_PartTypes
                                                                                                       ) tbl_PartTypes ON tbl_Masterparts.n_parttypeid = tbl_PartTypes.n_parttypeid
                                                                                            INNER JOIN ( SELECT tbl_VendorParts.n_VendorPartID ,
                                                                                                                tbl_VendorParts.VendorPartID
                                                                                                         FROM   dbo.VendorParts tbl_VendorParts
                                                                                                       ) tbl_VendorParts ON tbl_Masterparts.n_PrefVndPartID = tbl_VendorParts.n_VendorPartID
                                                                                            INNER JOIN ( SELECT tbl_MfgParts.n_mfgpartid ,
                                                                                                                tbl_MfgParts.mfgpartid
                                                                                                         FROM   dbo.MfgParts tbl_MfgParts
                                                                                                       ) tbl_MfgParts ON tbl_Masterparts.n_PrefMFGPartID = tbl_MfgParts.n_mfgpartid
                                                                                            INNER JOIN ( SELECT tbl_ObjectCodes.n_ObjectCodeID ,
                                                                                                                tbl_ObjectCodes.ObjectCodeID
                                                                                                         FROM   dbo.ObjectCodes tbl_ObjectCodes
                                                                                                       ) tbl_ObjectCodes ON tbl_Masterparts.n_ObjectCodeID = tbl_ObjectCodes.n_ObjectCodeID
                                                                                            LEFT JOIN ( SELECT  dbo.UsersFlaggedRecords.RecordID ,
                                                                                                                dbo.UsersFlaggedRecords.n_masterpartid
                                                                                                        FROM    dbo.UsersFlaggedRecords
                                                                                                        WHERE   dbo.UsersFlaggedRecords.UserID = @UserID
                                                                                                                AND dbo.UsersFlaggedRecords.n_masterpartid > 0
                                                                                                      ) tbl_IsFlaggedRecord ON tbl_Masterparts.n_masterpartid = tbl_IsFlaggedRecord.n_masterpartid
                                                                                   WHERE    ( tbl_Masterparts.b_active = 'Y'
                                                                                              AND tbl_Masterparts.n_masterpartid > 0
                                                                                            )
                                                                                 )
                                                                        --Return Data
                                                                        SELECT  cte_Masterparts.n_masterpartid ,
                                                                                cte_Masterparts.masterpartid ,
                                                                                cte_Masterparts.Description ,
                                                                                cte_Masterparts.listcost ,
                                                                                cte_Masterparts.lastcost ,
                                                                                cte_Masterparts.avgcost ,
                                                                                cte_Masterparts.n_buyerid ,
                                                                                cte_Masterparts.n_parttypeid ,
                                                                                cte_Masterparts.n_prefmfgid ,
                                                                                cte_Masterparts.n_prefvndid ,
                                                                                cte_Masterparts.n_PrefMFGPartID ,
                                                                                cte_Masterparts.n_PrefVndPartID ,
                                                                                cte_Masterparts.SpecialHandlingNotes ,
                                                                                cte_Masterparts.BuyerComment ,
                                                                                cte_Masterparts.b_tool ,
                                                                                cte_Masterparts.b_SpecHandling ,
                                                                                cte_Masterparts.chkoutunits ,
                                                                                cte_Masterparts.MRPRuleType ,
                                                                                cte_Masterparts.UnitOfIssue ,
                                                                                cte_Masterparts.UnitOfMeasure ,
                                                                                cte_Masterparts.YieldPct ,
                                                                                cte_Masterparts.ShelfLife ,
                                                                                cte_Masterparts.CountCycle ,
                                                                                cte_Masterparts.mfgid AS cPrefMfg ,
                                                                                cte_Masterparts.vendorid AS cVendor ,
                                                                                cte_Masterparts.parttypeid AS cPartType ,
                                                                                cte_Masterparts.UnitOfIssue AS UOI ,
                                                                                cte_Masterparts.UnitOfMeasure AS UOM ,
                                                                                cte_Masterparts.mfgpartid AS PrefMFGPartID ,
                                                                                cte_Masterparts.VendorPartID AS PrefVndPartID ,
                                                                                cte_Masterparts.ObjectCodeID ,
                                                                                cte_Masterparts.FlaggedRecordID
                                                                        FROM    cte_Masterparts">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" Type="Int32" />
                                                                                </SelectParameters>                                                        
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="btnAddPart" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddPart_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="btnCancelPartAdd" AutoPostBack="True" runat="server" Text="Close" CssClass="button">
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
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>

                <SettingsItemCaptions Location="Top"></SettingsItemCaptions>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>  
    <dx:ASPxPopupControl ID="CrewLaborClassPopup"
        ClientInstanceName="CrewLaborClassPopup"
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
                                                <dx:ASPxFormLayout ID="ASPxFormLayout6" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Crew By Labor Class Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="CrewLaborGridLookup" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_laborclassid" 
                                                                                Width="600px"
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="CrewLaborGridLookup" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="CrewLaborClassLookup"
                                                                                SelectionMode="Multiple">
                                                                                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused" 
                                                                                        RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" >
                                                                                    <Header CssClass="gridViewHeader"></Header>

                                                                                    <Row CssClass="gridViewRow"></Row>

                                                                                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>

                                                                                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>

                                                                                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                                                                                </Styles>
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                                                    <dx:GridViewDataTextColumn FieldName="n_laborclassid" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="laborclassid" ReadOnly="True" Caption="Labor Class" ToolTip="M-PET.NET Labor Class ID" SortOrder="Ascending" Width="200px" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" ReadOnly="True" Caption="Description" ToolTip="M-PET.NET Labor Class Description"   Width="300px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>

                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="CrewLaborClassLookup" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" 
                                                                                SelectCommand="SELECT  tblLC.n_laborclassid ,
                                                                                              tblLC.laborclassid ,
                                                                                              tblLC.description
                                                                                              FROM    dbo.laborclasses tblLC
                                                                                              WHERE   tblLC.b_IsActive = 'Y'
                                                                                              AND tblLC.n_laborclassid > 0
                                                                                              ORDER BY tblLC.laborclassid ASC">
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton1" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddCrewByLabor_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton3" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
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
    <dx:ASPxPopupControl ID="CrewGroupPopup"
        ClientInstanceName="CrewGroupPopup"
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
                                                <dx:ASPxFormLayout ID="ASPxFormLayout7" runat="server" Font-Size="Medium">
                                                    <Items>
                                                        <dx:LayoutGroup ColCount="4" Caption="Crew By Group Selection" Width="98%">
                                                            <Items>
                                                                    
                                                                <dx:LayoutItem ColSpan="4" Caption="">
                                                                    <LayoutItemNestedControlCollection >
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxGridView 
                                                                                ID="CrewGroupGridLookup" 
                                                                                runat="server" 
                                                                                Theme="Mulberry" 
                                                                                KeyFieldName="n_groupid" 
                                                                                Width="600px"
                                                                                KeyboardSupport="True" 
                                                                                ClientInstanceName="CrewGroupGridLookup" 
                                                                                AutoPostBack="True" 
                                                                                EnableCallBacks="true"
                                                                                Settings-HorizontalScrollBarMode="Auto" 
                                                                                SettingsPager-Mode="ShowPager" 
                                                                                SettingsBehavior-ProcessFocusedRowChangedOnServer="True" 
                                                                                SettingsBehavior-AllowFocusedRow="True" 
                                                                                DataSourceID="CrewGroupAddDs"
                                                                                SelectionMode="Multiple">
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
                                                                                    <dx:GridViewDataTextColumn FieldName="n_groupid" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="groupid" ReadOnly="True" Caption="Group" ToolTip="M-PET.NET Group ID" SortOrder="Ascending" Width="200px" VisibleIndex="1">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="description" ReadOnly="True" Caption="Description" ToolTip="M-PET.NET Group Description"   Width="300px" VisibleIndex="2">
                                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsSearchPanel Visible="true" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" AllowFocusedRow="True" AllowClientEventsOnLoad="false" ColumnResizeMode="NextColumn" />

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>

                                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" />
                                                                                <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" VerticalScrollableHeight="350"  />
                                                                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                                                                <SettingsPager PageSize="20">
                                                                                    <PageSizeItemSettings Visible="true" />
                                                                                </SettingsPager>

                                                                            </dx:ASPxGridView>                                                    
                                                                            <asp:SqlDataSource ID="CrewGroupAddDs" runat="server" ConnectionString="<%$ ConnectionStrings:connection %>" 
                                                                                SelectCommand="SELECT  tblGrp.n_groupid ,
                                                                                              tblGrp.groupid ,
                                                                                              tblGrp.description
                                                                                              FROM    dbo.groups tblGrp
                                                                                              WHERE   tblGrp.Active = 'Y'
                                                                                              AND tblGrp.n_groupid > 0
                                                                                              ORDER BY tblGrp.groupid ASC">
                                                                            </asp:SqlDataSource>
                                                                            <div class="popup-buttons-centered">
                                                                                <dx:ASPxButton ID="ASPxButton4" AutoPostBack="True" runat="server" CssClass="button" Text="Add" OnClick="btnAddCrewByGroup_Click" >
                                                                                    <HoverStyle CssClass="hover"></HoverStyle>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton5" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
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
    <dx:ASPxPopupControl ID="PostPopup" ClientInstanceName="PostPopup" ShowCloseButton="false" HeaderText=""
        CloseAnimationType="Fade" PopupAnimationType="Fade" runat="server" ShowShadow="true" ShowFooter="true"
        CssClass="popup" CloseAction="None" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <div class="popup-text">
                    <dx:ASPxFormLayout ID="flError" runat="server" Font-Size="Medium">
                        <Items>
                            <dx:LayoutGroup Caption="Post">
                                <Items>
                                    <dx:LayoutItem Caption="Completion Date:">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxDateEdit ID="jobPostDate"
                                                    ClientInstanceName="jobPostDate"
                                                    Theme="iOS"
                                                    Width="300px"
                                                    runat="server">
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Outcome Code:" HorizontalAlign="Right">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="ComboOutcomeCode"
                                                    runat="server"
                                                    EnableCallbackMode="true"
                                                    CallbackPageSize="10"
                                                    ValueType="System.String"
                                                    ValueField="n_outcomecodeid"
                                                    OnItemsRequestedByFilterCondition="ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ComboOutcomeCode_OnItemRequestedByValue_SQL"
                                                    TextFormatString="{0} - {1}"
                                                    Width="300px"
                                                    DropDownStyle="DropDown"
                                                    Theme="iOS"
                                                    TextField="outcomecodeid"
                                                    DropDownButton-Enabled="True"
                                                    AutoPostBack="False"
                                                    ClientInstanceName="ComboOutcomeCode">
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="n_outcomecodeid" Visible="False" />
                                                        <dx:ListBoxColumn FieldName="outcomecodeid" Caption="Outcome ID" Width="75px" ToolTip="M-PET.NET Outcome Code ID" />
                                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Outcome Code Description" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Name="PostDefaults" Caption="Post Defaults:" HorizontalAlign="Center" ShowCaption="False">
                                            <LayoutItemNestedControlCollection >
                                                <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxCheckBox ID="chkPostDefaults" runat="server" Text="Post Defaults"
                                                            ValueField="ID" TextField="Post Defaults" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Horizontal">
                                                        </dx:ASPxCheckBox>
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
            <div class="popup-buttons-centered">
                <dx:ASPxButton ID="ASPxButton1" AutoPostBack="True" runat="server" CssClass="button" Text="Post" OnClick="PostPlanJob">
                    <HoverStyle CssClass="hover"></HoverStyle>
                </dx:ASPxButton>
                <dx:ASPxButton ID="ASPxButton2" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
                    <ClientSideEvents Click="HidePopup" />
                    <HoverStyle CssClass="hover"></HoverStyle>
                </dx:ASPxButton>
            </div>
        </FooterTemplate>
    </dx:ASPxPopupControl>
        

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
    <asp:SqlDataSource ID="StoreroomPartDS" runat="server" />
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
    <asp:SqlDataSource ID="CrewUserDataSource" runat="server" />
    <asp:SqlDataSource ID="CompletedByDataSource" runat="server" />
    <asp:SqlDataSource ID="PostedByDataSource" runat="server" />
    <asp:SqlDataSource ID="OutcomeCodeDS" runat="server" />
</asp:Content>