<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkRequestForm.aspx.cs" Inherits="Pages_WorkRequests_WorkRequestForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <dx:ASPxFormLayout ID="ASPxFormLayout1" 
            runat="server" EnableTheming="True" 
            Theme="Mulberry" Width="934px">
            <Items>
                <dx:LayoutGroup ColCount="3" 
                    GroupBoxDecoration="Box">
                    <Items>
                        <dx:LayoutItem>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                        runat="server" 
                                        ErrorMessage="Need to have First Name"></asp:RequiredFieldValidator>
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E1" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                                        runat="server" 
                                        ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E2" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ASPxFormLayout1_E3" 
                                        runat="server">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem>
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
                <dx:LayoutItem>
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
        </dx:ASPxFormLayout>
    </form>
</body>
</html>
