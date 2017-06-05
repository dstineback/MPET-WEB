<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceRequests.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.ServiceRequests.ServiceRequest"%>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Service Request</asp:Content>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceRequests.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.ServiceRequests.ServiceRequests"%>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Requests</asp:Content>




<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHyperLink ID="PlannedJobBackLink" runat="server" Font-Size="20px" Theme="Mulberry" Text="Service Requests" NavigateUrl="~/Pages/ServiceRequests/ServiceRequests.aspx" />
    >
    <dx:ASPxLabel ID="lblHeader" Font-Size="20px" Theme="Mulberry" runat="server" Text="ADD"></dx:ASPxLabel>
    <dx:ASPxHiddenField ID="Navigation" ViewStateMode="Enabled" ClientInstanceName="Navigation" runat="server"></dx:ASPxHiddenField>
    <dx:ASPxFormLayout ID="WorkRequestDescLayout" runat="server" Width="600px" Paddings="0,0" RequiredMarkDisplayMode="RequiredOnly" RequiredMark="" EnableViewState="True">
        <Items>
            <dx:LayoutGroup Caption="Requested Action" Width="600px" ColCount="2" SettingsItemCaptions-Location="Top" GroupBoxDecoration="HeadingLine">
                <Items>
                    <dx:LayoutItem Name="DescLabel" Caption="" HelpText="Please Enter Additional Details Below" ColSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo ID="txtWorkDescription" Height="150px" Width="100%" MaxLength="254"
                                    ClientInstanceName="txtWorkDescription"
                                    runat="server" Theme="Mulberry">
                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic" ErrorDisplayMode="Text">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxMemo>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Request Date:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="TxtWorkRequestDate"
                                    ClientInstanceName="TxtWorkRequestDate"
                                    Theme="Mulberry"
                                    Width="300px"
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
                    <dx:LayoutItem Name="fldHwyRoute" Caption="Road Name:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="comboHwyRoute" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                    ValueType="System.String" ValueField="n_StateRouteID"
                                    OnItemsRequestedByFilterCondition="comboHwyRoute_OnItemsRequestedByFilterCondition_SQL"
                                    OnItemRequestedByValue="comboHwyRoute_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                    Width="300px" DropDownStyle="DropDown" Theme="Mulberry" TextField="StateRouteID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboHwyRoute">

                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_StateRouteID" Visible="False" />
                                        <dx:ListBoxColumn FieldName="StateRouteID" Caption="Road Name ID" Width="75px" ToolTip="M-PET.NET Road Name ID" />
                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Road Name Description" />
                                    </Columns>
                                </dx:ASPxComboBox>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Visible="False" Caption="Milepost From:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtMilepost"
                                    ClientInstanceName="txtMilepost"
                                    Theme="Mulberry"
                                    Width="200"
                                    runat="server"
                                    HorizontalAlign="Right"
                                    ClientVisible="False">
                                    <MaskSettings Mask="<0..99999g>.<0000..9999>" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Visible="False" Caption="Milepost To:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtMilepostTo"
                                    ClientInstanceName="txtMilepostTo"
                                    Theme="Mulberry"
                                    Width="200"
                                    runat="server"
                                    HorizontalAlign="Right"
                                    ClientVisible="False">
                                    <MaskSettings Mask="<0..99999g>.<0000..9999>" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Visible="False" Caption="Milepost Direction:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="comboMilePostDir" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                    ValueType="System.String" ValueField="n_MilePostDirectionID"
                                    OnItemsRequestedByFilterCondition="comboMilePostDir_OnItemsRequestedByFilterCondition_SQL"
                                    OnItemRequestedByValue="comboMilePostDir_OnItemRequestedByValue_SQL" TextFormatString="{0} - {1}"
                                    Width="200" DropDownStyle="DropDown" Theme="Mulberry" TextField="MilePostDirectionID" DropDownButton-Enabled="True" AutoPostBack="False" ClientInstanceName="comboMilePostDir" ClientVisible="False">

                                    <Columns>
                                        <dx:ListBoxColumn FieldName="n_MilePostDirectionID" Visible="False" />
                                        <dx:ListBoxColumn FieldName="MilePostDirectionID" Caption="Reason ID" Width="75px" ToolTip="M-PET.NET Mile Post Direction ID" />
                                        <dx:ListBoxColumn FieldName="Description" Caption="Description" Width="150px" ToolTip="M-PET.NET Mile Post Direction Description" />
                                    </Columns>
                                </dx:ASPxComboBox>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="First Name:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtFN" Width="300" MaxLength="100"
                                    ClientInstanceName="txtFN"
                                    runat="server" Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Last Name:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtLN" Width="300" MaxLength="100"
                                    ClientInstanceName="txtLN"
                                    runat="server" Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Email Address:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtEmail" Width="300" MaxLength="254"
                                    ClientInstanceName="txtEmail"
                                    runat="server" Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Phone Number:" CaptionSettings-Location="Top" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtPhone" Width="300"
                                    ClientInstanceName="txtPhone"
                                    runat="server" Theme="Mulberry" HorizontalAlign="Right">
                                    <MaskSettings Mask="+1 (999) 000-0000" IncludeLiterals="None" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="ASPxTextBox1" Width="100" MaxLength="10"
                                    ClientInstanceName="txtExt"
                                    runat="server" Theme="Mulberry" ClientVisible="False">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Ext:" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtExt" Width="100" MaxLength="10"
                                    ClientInstanceName="txtExt"
                                    runat="server" Theme="Mulberry">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Any Additional Information (Addresses/Names):" HelpText="Please Don't Forget To Click The Submit Button Below To Submit This Service Request" ColSpan="2" CaptionSettings-Location="Top">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxMemo ID="txtAdditionalInfo" Native="True" Height="100px" Width="100%" MaxLength="8000"
                                    ClientInstanceName="txtAdditionalInfo"
                                    runat="server" Theme="Mulberry">
                                </dx:ASPxMemo>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>

                        <CaptionSettings Location="Top"></CaptionSettings>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
    <asp:SqlDataSource ID="HwyRouteSqlDatasource" runat="server" />
    <asp:SqlDataSource ID="MilePostDirSqlDatasource" runat="server" />
</asp:Content>
