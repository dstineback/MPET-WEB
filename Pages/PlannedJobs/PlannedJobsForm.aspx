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
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E3" 
                                    runat="server">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Object/Asset--%>

                    <dx:LayoutItem Caption="Description">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="ASPxFormLayout1_E4" 
                                    runat="server">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Description--%>
                    <dx:LayoutItem Caption="Location">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E5" 
                                    runat="server">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Location--%>
                    <dx:LayoutItem Caption="Line">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E6" 
                                    runat="server">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Line--%>
                </Items>
            </dx:LayoutGroup>
      <%-- Requestor Group --%>
            <dx:LayoutGroup Caption="Requestor" ColCount="2">
                <Items>
                    <dx:LayoutItem Caption="Request Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxCalendar ID="ASPxFormLayout1_E7" 
                                    runat="server">
                                </dx:ASPxCalendar>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Request Date--%>
                    <dx:LayoutItem Caption="Requestor">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E8" 
                                    runat="server">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Requestor--%>
                    <dx:LayoutItem Caption="Priority">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E9" 
                                    runat="server">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem> <%--Priority--%>
                    <dx:LayoutItem Caption="Reason">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E10" 
                                    runat="server">
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
                    <dx:LayoutItem Caption="Start Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxCalendar ID="ASPxFormLayout1_E11" 
                                    runat="server">
                                </dx:ASPxCalendar>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>     <%--Start Date--%>
                    <dx:LayoutItem Caption="Due Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxCalendar ID="ASPxFormLayout1_E12" 
                                    runat="server">
                                </dx:ASPxCalendar>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>     <%--Return with in (Due Date)--%>
                    <dx:LayoutItem Caption="Completion Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxCalendar ID="ASPxFormLayout1_E13" 
                                    runat="server">
                                </dx:ASPxCalendar>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>     <%--Completion Date--%>
                    <dx:LayoutItem Caption="Outcome Code">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ASPxFormLayout1_E14" 
                                    runat="server">
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


   
</asp:Content>


