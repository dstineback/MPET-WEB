<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FacilityRequests.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.FacilityRequests.FacilityRequests"%>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Service Request</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHyperLink ID="PlannedJobBackLink" runat="server" Font-Size="20px" Theme="Mulberry" Text="Facility Requests List" NavigateUrl="~/Pages/WorkRequests/RequestsList.aspx" />
    >
    <dx:ASPxLabel ID="lblHeader" Font-Size="20px" Theme="Mulberry" runat="server" Text="ADD">
        
    </dx:ASPxLabel>
    
    <asp:LinkButton Width="35px" ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click">Submit
        <style>
           
            #submitButton {
    padding: 5px 20px;
    color: white;
    align-items:center;
    margin-left: 200px;
    border-radius: 5px;
    border: solid 1px black;
    background-color: #f55656;
   
}
#submitButton:hover {
    border: solid 1px Black;
    background-color: crimson;
}
        </style>
    </asp:LinkButton>
       
    <dx:ASPxHiddenField ID="Navigation" ViewStateMode="Enabled" ClientInstanceName="Navigation" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxFormLayout ID="WorkRequestDescLayout" runat="server"
        Width="600px" Paddings="0,0" RequiredMark="">
        <Items>
            <dx:LayoutGroup Caption="Requested Action" Width="600px" ColCount="3" SettingsItemCaptions-Location="Top" GroupBoxDecoration="HeadingLine">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" HelpText="Please Enter Additional Details Below" ColSpan="3">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo ID="txtWorkDescription" 
                                                Height="150px"
                                                Width="95%" 
                                                MaxLength="254"
                                                ClientInstanceName="txtWorkDescription"
                                                runat="server" 
                                                Theme="iOS" Border-BorderStyle="Solid" Border-BorderColor="#cccccc" Border-BorderWidth="1px" >
                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxMemo>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        

                    </dx:LayoutItem>

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
                    <dx:LayoutItem Caption="Object Description" CaptionSettings-Location="Top"
                        ColSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="txtObjectDescription" runat="server"
                                    ClientInstanceName="txtObjectDescription" Width="100%"
                                    ReadOnly="false" Theme="iOS">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Request Date:" CaptionSettings-Location="Top"
                        ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit runat="server" HorizontalAlign="Center"
                                    Width="200px" DisplayFormatString="D" ClientInstanceName="TxtWorkRequestDate"
                                    Theme="iOS" ID="TxtWorkRequestDate">
                                    <ValidationSettings ErrorDisplayMode="Text" Display="Dynamic"
                                        SetFocusOnError="True">
                                        <RequiredField IsRequired="True"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>






                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Visible="true" Caption="Reason:"
                        CaptionSettings-Location="Top" ColSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox runat="server" DropDownStyle="DropDown"
                                    CallbackPageSize="10" EnableCallbackMode="True"
                                    TextField="JobReasonID" ValueField="nJobReasonID"
                                    TextFormatString="{0} - {1}" Width="100%" ClientInstanceName="comboJobReason"
                                    Theme="iOS" ID="comboJobReason" OnItemsRequestedByFilterCondition="comboJobReason_OnItemsRequestedByFilterCondition_SQL"
                                    OnItemRequestedByValue="comboJobReason_OnItemRequestedByValue_SQL">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="nJobReasonID" Visible="False">
                                        </dx:ListBoxColumn>
                                        <dx:ListBoxColumn FieldName="JobReasonID" Width="75px"
                                            Caption="Reason ID" ToolTip="M-PET.NET Job Reason ID">
                                        </dx:ListBoxColumn>
                                        <dx:ListBoxColumn FieldName="Description" Width="150px"
                                            Caption="Description" ToolTip="M-PET.NET Job Reason Description">
                                        </dx:ListBoxColumn>
                                    </Columns>
                                </dx:ASPxComboBox>




                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Service Office:" CaptionSettings-Location="Top"
                        ColSpan="1" Name="fldHwyRoute">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="comboServiceOffice"
                                    runat="server"
                                    EnableCallbackMode="true"
                                    CallbackPageSize="10"
                                    ValueType="System.String"
                                    ValueField="n_areaid"
                                    OnItemsRequestedByFilterCondition="comboServiceOffice_OnItemsRequestedByFilterCondition_SQL"
                                    OnItemRequestedByValue="comboServiceOffice_OnItemRequestedByValue_SQL"
                                    TextFormatString="{0} - {1}"
                                    Width="100%"
                                    DropDownStyle="DropDown"
                                    Theme="iOS"
                                    TextField="areaid"
                                    DropDownButton-Enabled="True"
                                    AutoPostBack="False"
                                    ClientInstanceName="comboServiceOffice">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_areaid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="areaid" Caption="Service Office ID"
                                            Width="75px" ToolTip="M-PET.NET Service Office ID" />
                                        <dx:ListBoxColumn FieldName="n_areaid" Caption="Description"
                                            Width="150px" ToolTip="M-PET.NET Service Office Description" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Building:" CaptionSettings-Location="Top"
                        ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox runat="server" Width="100%" MaxLength="10"
                                    HorizontalAlign="Right" ClientInstanceName="txtBuildingNum"
                                    Theme="iOS" ID="txtBuildingNum">
                                </dx:ASPxTextBox>




                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Room:" CaptionSettings-Location="Top"
                        ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtRoomNum"
                                    Width="100%"
                                    MaxLength="10"
                                    ClientInstanceName="txtRoomNum"
                                    runat="server"
                                    Theme="iOS" HorizontalAlign="Right">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Mail Code:" CaptionSettings-Location="Top"
                        ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtMailCode"
                                    Width="100%"
                                    MaxLength="10"
                                    ClientInstanceName="txtExt"
                                    runat="server"
                                    Theme="iOS">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="" CaptionSettings-Location="Top"
                        ClientVisible="False" ColSpan="2" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="ASPxTextBox1"
                                    Width="100%"
                                    runat="server"
                                    Theme="iOS" MaxLength="10" ClientVisible="False"
                                    HorizontalAlign="Right">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="First Name:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtFN"
                                    Width="100%"
                                    MaxLength="100"
                                    ClientInstanceName="txtFN"
                                    runat="server"
                                    Theme="iOS">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Last Name:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtLN"
                                    Width="100%"
                                    MaxLength="100"
                                    runat="server"
                                    Theme="iOS" ClientInstanceName="txtLN">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem CaptionSettings-Location="Top"
                        Caption="Email Address:">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox runat="server" Width="100%" MaxLength="254"
                                    Theme="iOS"
                                    ID="txtEmail" ClientInstanceName="txtEmail">
                                </dx:ASPxTextBox>



                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Phone Number:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtPhone"
                                    Width="100%"
                                    ClientInstanceName="txtPhone"
                                    runat="server"
                                    Theme="iOS" HorizontalAlign="Right">
                                    <MaskSettings Mask="(999) 000-0000" IncludeLiterals="None">
                                    </MaskSettings>

                                    <ValidationSettings Display="Dynamic"></ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Ext:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtExt"
                                    Width="100%"
                                    ClientInstanceName="txtExt"
                                    runat="server"
                                    Theme="iOS" MaxLength="10">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Any Additional Information (Addresses/Names):"
                        CaptionSettings-Location="Top" ColSpan="3" HelpText="Please Don&#39;t Forget To Click The Submit Button Below To Submit This Facility Request">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo runat="server" MaxLength="8000" Height="100px"
                                    Native="True" Width="100%" ClientInstanceName="txtAdditionalInfo"
                                    Theme="iOS" ID="txtAdditionalInfo"></dx:ASPxMemo>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
    <asp:SqlDataSource ID="ObjectDataSource" runat="server" />
    <asp:SqlDataSource ID="HwyRouteSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="MilePostDirSqlDatasource" runat="server" />
</asp:Content>
