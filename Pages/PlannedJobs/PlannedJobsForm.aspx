<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedJobsForm.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages_PlannedJobs_PlannedJobsForm" %>
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
            //console.log('sender', sender);
            //window._xyz = sender.GetMainElement();
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
    
    <%-- Form Layout --%>
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server">
        <Items>
      <%-- Object/Asset --%>
             <dx:LayoutGroup Caption="Object/Asset" 
                ColCount="4">
                <Items>
                    <dx:LayoutItem Caption="Object/Asset">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ObjectIDCombo" runat="server"  EnableCallbackMode="true"  CallbackPageSize="10" 
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
                    </dx:LayoutItem> <%--Object/Asset--%>

                    <dx:LayoutItem Caption="Description" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="txtObjectDescription" Height="100px" Width="600px" MaxLength="254" ClientInstanceName="txtObjectDescription" runat="server" ReadOnly="True"  Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Description--%>
                    <dx:LayoutItem Caption="Location" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="txtObjectLocation" ClientInstanceName="txtObjectLocation" Theme="iOS" ReadOnly="true" 
                                    runat="server">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Location--%>
                    <dx:LayoutItem Caption="Line" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboFundSource" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                    ValueType="System.String" ValueField="n_FundSrcCodeID"
                                                    OnItemsRequestedByFilterCondition="ComboFundSource_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ComboFundSource_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                    Width="iOS" DropDownStyle="DropDown" Theme="iOS" TextField="FundSrcCodeID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboFundSource">                                                           
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_FundSrcCodeID" Visible="False" />
                                        <dx:ListBoxColumn FieldName="FundSrcCodeID" Caption="Cost Code ID" Width="75px" ToolTip="M-PET.NET Fund Source ID"/>
                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Fund Source Description"/>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Line--%>
                </Items>
            </dx:LayoutGroup>
      <%-- Requestor Group --%>
            <dx:LayoutGroup Caption="Requestor" ColCount="2">
                <Items>
                    <dx:LayoutItem Caption="Request Date" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxDateEdit ID="TxtWorkRequestDate" ClientInstanceName="TxtWorkRequestDate"Theme="IOS"DisplayFormatString="D" Width="50%" runat="server">
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Request Date--%>
                    <dx:LayoutItem Caption="Requestor" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboRequestor" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                ValueType="System.String" ValueField="UserID"
                                                OnItemsRequestedByFilterCondition="ComboRequestor_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="ComboRequestor_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                Width="50%" DropDownStyle="DropDown" Theme="IOS" TextField="Username" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboRequestor">
                                     <Columns>
                                        <dx:ListBoxColumn FieldName="UserID" Visible="False" />
                                        <dx:ListBoxColumn FieldName="Username" Caption="Username" Width="75px" ToolTip="M-PET.NET User's Username"/>
                                        <dx:ListBoxColumn FieldName="FullName" Caption="Full Name" Width="150px" ToolTip="M-PET.NET User's Full Name"/>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Requestor--%>
                    <dx:LayoutItem Caption="Priority" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
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
                    </dx:LayoutItem> <%--Priority--%>
                    <dx:LayoutItem Caption="Reason" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="comboReason" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                ValueType="System.String" ValueField="n_reasonid"
                                                OnItemsRequestedByFilterCondition="comboReason_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="comboReason_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                Width="400px" DropDownStyle="DropDown" Theme="IOS" TextField="reasonid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboReason">
                                                                                               
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_reasonid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="reasonid" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Reason ID"/>
                                        <dx:ListBoxColumn FieldName="description" Caption="Description" Width="150px" ToolTip="M-PET.NET Reason Description"/>
                                    </Columns> 
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Reason--%>
                </Items>
            </dx:LayoutGroup>
      <%-- Description of Work --%>
            <dx:LayoutItem Name="DescLable" Caption="Description Of Work">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="txtWorkDescription" Height="75px" Width="95%" ClientInstanceName="txtWorkDescription" Theme="iOS" runat="server">
                            <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                        <RequiredField IsRequired="True"/>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem> <%--Description of Work--%>
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
                                                Width="450px" 
                                                runat="server" >
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
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
                                                Width="50%"
                                                HorizontalAlign="Right"
                                                AllowMouseWheel="False" 
                                                SpinButtons-ClientVisible="False">
                                <SpinButtons ClientVisible="True"></SpinButtons>
                                <ClearButton Visibility="True"></ClearButton>
                                </dx:ASPxSpinEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>     <%--Return with in (Due Date)--%>
                    <dx:LayoutItem Caption="Completion Date" Name="fldCompDate" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxCalendar ID="TxtWorkCompDate" 
                                                ClientInstanceName="TxtWorkCompDate"
                                                DisplayFormatString="D"
                                                Theme="iOS"
                                                Width="50%" 
                                                runat="server" >
                                </dx:ASPxCalendar>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>     <%--Completion Date--%>
                    <dx:LayoutItem Caption="Outcome Code" Name="fldOutcome" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboOutcome" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                                    ValueType="System.String" ValueField="n_outcomecodeid"
                                                    OnItemsRequestedByFilterCondition="ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ComboOutcomeCode_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                                    Width="50%" DropDownStyle="DropDown" Theme="iOS" TextField="outcomecodeid" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="ComboOutcome">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_outcomecodeid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="outcomecodeid" Caption="Outcome Code ID" Width="75px" ToolTip="M-PET.NET Outcome Code ID"/>
                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Outcome Code Description"/>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>     <%--Outcome Code--%>
                </Items>
            </dx:LayoutGroup>
      <%-- Breakdown --%>
            <dx:LayoutItem Caption="Breakdown">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="ASPxFormLayout1_E18" 
                            runat="server">
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>    <%--Breakdown--%>
        </Items>
    </dx:ASPxFormLayout>


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


