<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FacilityRequests.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.FacilityRequests.FacilityRequests"%>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Service Request</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHyperLink ID="PlannedJobBackLink" runat="server" Font-Size="20px" Theme="Mulberry" Text="Facility Requests" NavigateUrl="~/Pages/FacilityRequests/FacilityRequests.aspx" />
    >
    <dx:ASPxLabel ID="lblHeader" Font-Size="20px" Theme="Mulberry" runat="server" Text="ADD"></dx:ASPxLabel>
    <dx:ASPxHiddenField ID="Navigation" ViewStateMode="Enabled" ClientInstanceName="Navigation" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxFormLayout ID="WorkRequestDescLayout" runat="server" Width="600px" Paddings="0,0" RequiredMarkDisplayMode="RequiredOnly" RequiredMark="" EnableViewState="True">
        <Items>
            <dx:LayoutGroup Caption="Requested Action" Width="600px" ColCount="3" SettingsItemCaptions-Location="Top" GroupBoxDecoration="HeadingLine">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" HelpText="Please Enter Additional Details Below" ColSpan="3">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo ID="txtWorkDescription" 
                                                Height="150px"
                                                Width="100%" 
                                                MaxLength="254"
                                                ClientInstanceName="txtWorkDescription"
                                                runat="server" 
                                                Theme="Mulberry">
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
                    <dx:LayoutItem Caption="Object Description" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="txtObjectDescription" runat="server" ClientInstanceName="txtObjectDescription" Width="90%" Height="100px" ReadOnly="false"  Theme="iOS">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

<CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Request Date:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="TxtWorkRequestDate"
                                                    ClientInstanceName="TxtWorkRequestDate"
                                                    Theme="Mulberry"
                                                    Width="200"
                                                    runat="server"
                                                    HorizontalAlign="Center"
                                                    DisplayFormatString="D">
                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Visible="true" Caption="Reason:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="comboJobReason" 
                                                    runat="server" 
                                                    EnableCallbackMode="true" 
                                                    CallbackPageSize="10"
                                                    ValueType="System.String" 
                                                    ValueField="nJobReasonID"
                                                    OnItemsRequestedByFilterCondition="comboJobReason_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="comboJobReason_OnItemRequestedByValue_SQL" 
                                                    TextFormatString="{0} - {1}"
                                                    Width="200" 
                                                    DropDownStyle="DropDown" 
                                                    Theme="Mulberry" 
                                                    TextField="JobReasonID" 
                                                    DropDownButton-Enabled="True" 
                                                    AutoPostBack="False" 
                                                    ClientInstanceName="comboJobReason" 
                                                    ClientVisible="True">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="nJobReasonID" Visible="False" />
                                        <dx:ListBoxColumn FieldName="JobReasonID" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Job Reason ID" />
                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Job Reason Description" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Name="fldHwyRoute" Caption="Service Office:" CaptionSettings-Location="Top" ColSpan="1">
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
                                                    Width="200" 
                                                    DropDownStyle="DropDown" 
                                                    Theme="Mulberry" 
                                                    TextField="areaid" 
                                                    DropDownButton-Enabled="True" 
                                                    AutoPostBack="False" 
                                                    ClientInstanceName="comboServiceOffice">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_areaid" Visible="False" />
                                        <dx:ListBoxColumn FieldName="areaid" Caption="Service Office ID" Width="75px" ToolTip="M-PET.NET Service Office ID" />
                                        <dx:ListBoxColumn FieldName="n_areaid" Caption="Description" Width="150px" ToolTip="M-PET.NET Service Office Description" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="First Name:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtFN"  
                                                Width="200" 
                                                MaxLength="100"
                                                ClientInstanceName="txtFN"                                            
                                                runat="server" 
                                                Theme="Mulberry"  >
                                    
                                    
                                    
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Last Name:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtLN" 
                                                Width="200" 
                                                MaxLength="100"
                                                ClientInstanceName="txtLN"
                                                runat="server" 
                                                Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Email Address:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtEmail" 
                                                Width="200" 
                                                MaxLength="254"
                                                ClientInstanceName="txtEmail"
                                                runat="server" 
                                                Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Phone Number:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtPhone" 
                                                Width="200"
                                                ClientInstanceName="txtPhone"
                                                runat="server" 
                                                Theme="Mulberry"
                                                HorizontalAlign="Right">
                                    <MaskSettings ShowHints="False" Mask="+1 (999) 000-0000" IncludeLiterals="None" />
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
                                                Width="100" 
                                                MaxLength="10"
                                                ClientInstanceName="txtExt"
                                                runat="server" 
                                                Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="" ClientVisible="False" ShowCaption="False" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="ASPxTextBox1" 
                                                Width="100" 
                                                MaxLength="10"
                                                runat="server" 
                                                Theme="Mulberry"
                                                HorizontalAlign="Right"
                                                ClientVisible="False">
                                </dx:ASPxTextBox>                                
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Building:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtBuildingNum" 
                                                Width="100" 
                                                MaxLength="10"
                                                ClientInstanceName="txtBuildingNum"
                                                runat="server" 
                                                Theme="Mulberry"
                                                HorizontalAlign="Right">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Room:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtRoomNum" 
                                                Width="100" 
                                                MaxLength="10"
                                                ClientInstanceName="txtRoomNum"
                                                runat="server" 
                                                Theme="Mulberry"
                                                HorizontalAlign="Right">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Mail Code:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtMailCode" 
                                                Width="100" 
                                                MaxLength="10"
                                                ClientInstanceName="txtExt"
                                                runat="server" 
                                                Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Any Additional Information (Addresses/Names):" 
                                    HelpText="Please Don't Forget To Click The Submit Button Below To Submit This Facility Request" ColSpan="3" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo ID="txtAdditionalInfo" 
                                                Native="True" 
                                                Height="100px" 
                                                Width="100%" 
                                                MaxLength="8000"
                                                ClientInstanceName="txtAdditionalInfo"
                                                runat="server" 
                                    Theme="Mulberry">
                                </dx:ASPxMemo>
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
