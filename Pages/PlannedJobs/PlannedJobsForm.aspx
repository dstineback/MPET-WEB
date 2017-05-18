 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedJobsForm.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.PlannedJobs.PlannedJobsForm" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Planned Jobs</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

    
    <%-- JavaScript Code --%>
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

    <%-- Heading Script Manager, HyperLinks, and Hidden Fields --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHyperLink ID="PlannedJobBackLink" runat="server" Font-size="20px" Theme="Mulberry" Text="PLANNED JOBS" NavigateUrl="~/Pages/PlannedJobs/PlannedJobsList.aspx"/> > <dx:ASPxLabel ID="lblHeader" Font-size="20px" Theme="Mulberry" runat="server" Text="ADD"></dx:ASPxLabel> > <dx:ASPxLabel ID="lblStep" Font-size="20px" Theme="Mulberry" runat="server" Text="Step: "></dx:ASPxLabel> <br />
    <dx:ASPxHyperLink ID="myJobsBackLink" runat="server" Font-Size="16" Theme="Mulberry" Text="MY JOBS" NavigateUrl="~/Pages/PlannedJobs/myJobs.aspx" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled"  ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="MultiGrid" ViewStateMode="Enabled"  ClientInstanceName="MultiGrid" runat="server"></dx:ASPxHiddenField>
    
    <dx:ASPxFormLayout ID="objectFormLayout" runat="server" Width="98%" >
        <Items>
    <%-- Form Layout --%>
             <dx:LayoutItem Caption="Breakdown">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxCheckBox ID="BreakDownCheckBox" ClientInstanceName="BreakDownCheckBox" Theme="iOS" 
                            runat="server">
                        </dx:ASPxCheckBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>    <%--Breakdown--%>
      <%-- Breakdown --%>
      <%-- Object/Asset --%>
             <dx:LayoutGroup Caption="Object/Asset" 
                ColCount="4" SettingsItemCaptions-Location="Top">
                <Items>
                    <dx:LayoutItem Caption="Object/Asset">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ObjectIDCombo" runat="server"  EnableCallbackMode="true"  CallbackPageSize="10" 
                                                                                                                    ValueType="System.String" 
                                                                                                                    ValueField="n_objectid" 
                                                                                                                    OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" 
                                                                                                                    OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" 
                                                                                                                    TextFormatString="{0}" 
                                                                                                                    Width="90%" 
                                                                                                                    DropDownStyle="DropDown" 
                                                                                                                    Theme="iOS" 
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
                    </dx:LayoutItem>     <%--Object/Asset--%>
                    <dx:LayoutItem Caption="Location" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="txtObjectLocation" ClientInstanceName="txtObjectLocation" Theme="iOS" ReadOnly="true" 
                                    runat="server" Width="90%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>     <%--Location--%>           
                    <dx:LayoutItem Name="fldFundGrp" Caption="Line" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ComboFundGroup" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                        ValueType="System.String" ValueField="n_FundingGroupCodeID"
                                                        OnItemsRequestedByFilterCondition="ComboFundGroup_OnItemsRequestedByFilterCondition_SQL"
                                                        OnItemRequestedByValue="ComboFundGroup_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                        Width="90%" DropDownStyle="DropDown" Theme="iOS" TextField="FundingGroupCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboFundGroup">
                                                                                               
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="n_FundingGroupCodeID" Visible="False" />
                                            <dx:ListBoxColumn FieldName="FundingGroupCodeID" Caption="Fund. Group ID" Width="75px" ToolTip="M-PET.NET Funding Group ID"/>
                                            <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Funding Group Description"/>
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionSettings Location="Top"></CaptionSettings>                                                                                        
                    </dx:LayoutItem> <%--Line--%>
                    <dx:LayoutItem Name="fldCostCode" Caption="Cost Code:" CaptionSettings-Location="Top">
                                                                                            <LayoutItemNestedControlCollection>
                                                                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                                                                    <dx:ASPxComboBox ID="ComboCostCode" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                                                                                     ValueType="System.String" ValueField="n_costcodeid"
                                                                                                                     OnItemsRequestedByFilterCondition="ComboCostCode_OnItemsRequestedByFilterCondition_SQL"
                                                                                                                     OnItemRequestedByValue="ComboCostCode_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                                                                                     Width="90%" DropDownStyle="DropDown" Theme="iOS" TextField="CostCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboCostCode">
                                                                                               
                                                                                                        <Columns>
                                                                                                            <dx:ListBoxColumn FieldName="n_costcodeid" Visible="False" />
                                                                                                            <dx:ListBoxColumn FieldName="CostCodeID" Caption="Cost Code ID" Width="75px" ToolTip="M-PET.NET Cost Code ID"/>
                                                                                                            <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Cost Code Description"/>
                                                                                                        </Columns>
                                                                                                    </dx:ASPxComboBox>
                                                                                                </dx:LayoutItemNestedControlContainer>
                                                                                            </LayoutItemNestedControlCollection>

                                                                                            <CaptionSettings Location="Top"></CaptionSettings>
                                                                                        </dx:LayoutItem> <%--Cost Code--%>
                    <dx:LayoutItem Caption="Description" 
                                                                                    CaptionSettings-Location="Top" ColSpan="2">
                                                                                    <LayoutItemNestedControlCollection>
                                                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                                                            <dx:ASPxTextBox ID="txtObjectDescription" 
                                                                                                runat="server" 
                                                                                                ClientInstanceName="txtObjectDescription" 
                                                                                                Height="100px" MaxLength="254" ReadOnly="True" 
                                                                                                Theme="iOS" Width="90%">
                                                                                            </dx:ASPxTextBox>
                                                                                        </dx:LayoutItemNestedControlContainer>
                                                                                    </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                                                                                </dx:LayoutItem> <%--Description--%>
                    <dx:LayoutItem Caption="" HelpText="" 
                                CaptionSettings-Location="Top" ColSpan="2">
                                <CaptionSettings Location="Top"></CaptionSettings>
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxImage ID="objectImg" runat="server" 
                                            AlternateText="No Picture Associated" 
                                            ClientInstanceName="objectImg" ImageAlign="Left" 
                                            ImageUrl="~/Content/Images/noImage.png" 
                                            ShowLoadingImage="True" Theme="Mulberry" 
                                            Width="100px" Height="100px">
                                        </dx:ASPxImage>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                                <CaptionSettings Location="Top" />
                    </dx:LayoutItem> <%--Object Image  --%>                    
                </Items>

<SettingsItemCaptions Location="Top"></SettingsItemCaptions>
            </dx:LayoutGroup>
      <%-- Description of Work --%>
            <dx:LayoutItem Name="DescLable" Caption="Description Of Work" CaptionSettings-Location="Top">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="txtWorkDescription" Height="75px" Width="95%" ClientInstanceName="txtWorkDescription" Theme="iOS" runat="server">
                            <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                        <RequiredField IsRequired="True"/>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>

                <CaptionSettings Location="Top"></CaptionSettings>
            </dx:LayoutItem> <%--Description of Work--%>
      <%-- Requestor Group --%>
            <dx:LayoutGroup Caption="Requestor" ColCount="2">
                <Items>
                    <dx:LayoutItem Caption="Request Date" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxDateEdit ID="TxtWorkRequestDate" ClientInstanceName="TxtWorkRequestDate" Theme="IOS" DisplayFormatString="D" Width="90%" runat="server">
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

                    <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem> <%--Request Date--%>
                    <dx:LayoutItem Caption="Requestor" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboRequestor" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                ValueType="System.String" ValueField="UserID"
                                                OnItemsRequestedByFilterCondition="ComboRequestor_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="ComboRequestor_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                Width="90%" DropDownStyle="DropDown" Theme="IOS" TextField="Username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboRequestor">
                                     <Columns>
                                        <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                        <dx:ListBoxColumn FieldName="Username" Caption="Username" Width="75px" ToolTip="M-PET.NET User's Username"/>
                                        <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User's Full Name"/>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem> <%--Requestor--%>
                    <dx:LayoutItem Caption="Priority" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboPriority" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                ValueType="System.String" ValueField="n_priorityid"
                                                OnItemsRequestedByFilterCondition="ComboPriority_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="ComboPriority_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                Width="90%" DropDownStyle="DropDown" Theme="Mulberry" TextField="priorityid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboPriority">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_priorityid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="priorityid" Caption="Priority ID" Width="75px" ToolTip="M-PET.NET Priority ID"/>
                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Priority Description"/>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem> <%--Priority--%>
                    <dx:LayoutItem Caption="Reason" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="comboReason" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                ValueType="System.String" ValueField="n_reasonid"
                                                OnItemsRequestedByFilterCondition="comboReason_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="comboReason_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                Width="90%" DropDownStyle="DropDown" Theme="IOS" TextField="reasonid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboReason">
                                                                                               
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_reasonid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="reasonid" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Reason ID"/>
                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Reason Description"/>
                                    </Columns> 
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem> <%--Reason--%>
                </Items>
            </dx:LayoutGroup>
      <%-- Scheduled Dates Group --%>
            <dx:LayoutGroup Caption="Scheduled Dates" 
                ColCount="2">
                <Items>
                    <dx:LayoutItem Caption="Start Date" Name="fldStartDate" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxDateEdit ID="TxtWorkStartDate" 
                                                ClientInstanceName="TxtWorkStartDate"
                                                DisplayFormatString="D"
                                                Theme="Mulberry"
                                                Width="90%" 
                                                runat="server" >
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>     <%--Start Date--%>
                    <dx:LayoutItem Caption="Due Date" Name="fldReturnWithin" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxSpinEdit ID="txtReturnWithin" 
                                                ClientInstanceName="txtReturnWithin"
                                                Theme="IOS"
                                                runat="server" 
                                                ValueType="System.Integer"
                                                MinValue="0"
                                                MaxValue="79228162514264337593543950335"
                                                Width="90%"
                                                HorizontalAlign="Right"
                                                AllowMouseWheel="False" 
                                                SpinButtons-ClientVisible="False">
                                <SpinButtons ClientVisible="True"></SpinButtons>
                                <ClearButton Visibility="True"></ClearButton>
                                </dx:ASPxSpinEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>     <%--Return with in (Due Date)--%>
                    <dx:LayoutItem Caption="Completion Date" Name="fldCompDate" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxDateEdit ID="TxtWorkCompDate" 
                                                ClientInstanceName="TxtWorkCompDate"
                                                DisplayFormatString="D"
                                                Theme="iOS"
                                                Width="90%" 
                                                runat="server" >
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>     <%--Completion Date--%>
                    <dx:LayoutItem Caption="Outcome Code" Name="fldOutcome" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboOutcome" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                    ValueType="System.String" ValueField="n_outcomecodeid"
                                                    OnItemsRequestedByFilterCondition="ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ComboOutcomeCode_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                    Width="90%" DropDownStyle="DropDown" Theme="iOS" TextField="outcomecodeid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboOutcome">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_outcomecodeid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="outcomecodeid" Caption="Outcome Code ID" Width="75px" ToolTip="M-PET.NET Outcome Code ID"/>
                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Outcome Code Description"/>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>     <%--Outcome Code--%>
                </Items>
            </dx:LayoutGroup>
        
        <dx:LayoutItem Name="CrewGridViewLayoutItem" 
                Caption="">
            <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                         <dx:ASPxGridView 
                                                                ID="CrewGrid" 
                                                                runat="server" 
                                                                Theme="iOS" 
                                                                KeyFieldName="RecordID" 
                                                                Width="90%" 
                                                                KeyboardSupport="True" 
                                                                ClientInstanceName="CrewGrid" 
                                                                AutoPostBack="false" 
                                                                EnableCallBacks="true"  
                                                                Settings-HorizontalScrollBarMode="Auto" SettingsPager-Mode="ShowPager" SettingsBehavior-ProcessFocusedRowChangedOnServer="True" SettingsBehavior-AllowFocusedRow="False" 
                                                                SettingsBehavior-AllowSelectByRowClick="true" DataSourceID="CrewDataSource" OnDataBound="CrewGridBound" OnRowUpdating="CrewGrid_RowUpdating" Border-BorderStyle="Solid" Border-BorderColor="Gray" >
                                                               
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
                                                                    <dx:GridViewCommandColumn VisibleIndex="0" />
                                                                    <dx:GridViewDataTextColumn FieldName="RecordID" 
                                                                        ReadOnly="True" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="UserID" 
                                                                        VisibleIndex="2">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_skillid" 
                                                                        ReadOnly="True" VisibleIndex="3">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_ShiftID" 
                                                                        VisibleIndex="4">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_PayCodeID" 
                                                                        ReadOnly="True" VisibleIndex="5">
                                                                    </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="CrewMemberTextID" ShowInCustomizationForm="True" VisibleIndex="6">
</dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="CrewMemberName" 
                                                                        ReadOnly="True" VisibleIndex="7">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ShiftIDText" 
                                                                        VisibleIndex="8">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="ShiftIDDesc" 
                                                                        VisibleIndex="9">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="SkillIDText" 
                                                                        VisibleIndex="10">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="SkillDesc" 
                                                                        VisibleIndex="11">
                                                                    </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="ActualHrs" ShowInCustomizationForm="True" VisibleIndex="12">
</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="EstHrs" ShowInCustomizationForm="True" VisibleIndex="13">
</dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="PayRate" 
                                                                        VisibleIndex="14">
                                                                    </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="PayCodeText" ShowInCustomizationForm="True" VisibleIndex="15">
</dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="WorkDate" 
                                                                        VisibleIndex="16" ReadOnly="True">
                                                                    </dx:GridViewDataDateColumn>     
                                                                    <dx:GridViewDataDateColumn FieldName="CertificationDate" 
                                                                        ReadOnly="True" VisibleIndex="17">
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="CertificationDateExpires" 
                                                                        ReadOnly="True" VisibleIndex="18">
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="n_laborclassid" 
                                                                        ReadOnly="True" VisibleIndex="19">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="RateType" 
                                                                        VisibleIndex="20">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="DMRKEY" 
                                                                        VisibleIndex="21">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="LaborClassID" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="22">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="RateTypeStr" 
                                                                        ReadOnly="True" VisibleIndex="23">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="LinkedDMR" 
                                                                        ReadOnly="True" VisibleIndex="24">
                                                                    </dx:GridViewDataTextColumn>   
                                                                </Columns>
                                    
                                                                <SettingsBehavior 
                                                                    EnableRowHotTrack="True" 
                                                                    AllowFocusedRow="True" 
                                                                    AllowClientEventsOnLoad="false" 
                                                                    ColumnResizeMode="NextColumn" />
                                        

<SettingsCommandButton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</SettingsCommandButton>
                                                                
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
                                                                    <FooterRow>
                                                                        
                                                                    <dx:ASPxButton runat="server" AutoPostBack="false" 
                                                                        ID="AddNewCrewButton" 
                                                                        OnClick="btnAddCrew_Click" Theme="iOS" 
                                                                        Text="Add New Crew Member"></dx:ASPxButton>
                                                                    <dx:ASPxButton runat="server" 
                                                                        ID="DeleteCrewButton" 
                                                                        OnClick="btnDeleteCrew_Click" Theme="iOS" 
                                                                        Text="Delete Crew Member"></dx:ASPxButton>
                                                                
</FooterRow>
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
                                                             
                                                            <Border BorderColor="Gray" BorderStyle="Solid"></Border>
                                                            <Templates>
                                                                <FooterRow>
                                                                    <dx:ASPxButton runat="server" ID="AddNewCrewButton" AutoPostBack="false"  Theme="iOS" Text="Add New Crew Member">
                                                                        <ClientSideEvents Click="ShowCrewPopup" />
                                                                    </dx:ASPxButton>
                                                                    <dx:ASPxButton runat="server" ID="DeleteCrewButton" OnClick="btnDeleteCrew_Click" Theme="iOS" Text="Delete Crew Member"></dx:ASPxButton>
                                                                </FooterRow>

                                                            </Templates>
                                                            </dx:ASPxGridView>                            
                            <div>
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
                            </div> <%--CrewGrid DataSource--%>                    
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
        </dx:LayoutItem> <%--Gridview for Crew--%>

        </Items>
    </dx:ASPxFormLayout>
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
                                                                                AutoPostBack="False" 
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
                                                                                <dx:ASPxButton ID="OkButton" AutoPostBack="True" runat="server" Text="Cancel" CssClass="button">
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


