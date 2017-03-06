<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkRequestForm.aspx.cs" Inherits="Pages_WorkRequests_WorkRequestForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Request &gt;
        <br />
    
    </div>
        <dx:ASPxFormLayout ID="ASPxFormLayout1" 
            runat="server" EnableTheming="True" 
            Theme="iOS" Width="1438px" Height="400px">
            <Items>
                <dx:LayoutItem Caption="Description of Work" 
                    RowSpan="2">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="ASPxFormLayout1_E6" 
                                runat="server" Height="84px" Width="946px">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutGroup ColCount="2" 
                    GroupBoxDecoration="Box" Caption="">
                    <Items>
                        <dx:LayoutItem Caption="Object/Asset:" 
                            RequiredMarkDisplayMode="Hidden">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E10" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Object Description" 
                            RequiredMarkDisplayMode="Optional">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="ASPxFormLayout1_E9" 
                                        runat="server">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Reason:">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ASPxFormLayout1_E3" 
                                        runat="server">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Priority:">
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
                <dx:LayoutItem Caption="Attachments:">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxUploadControl ID="ASPxFormLayout1_E8" 
                                runat="server">
                            </dx:ASPxUploadControl>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
        </dx:ASPxFormLayout>
    </form>
</body>
</html>
