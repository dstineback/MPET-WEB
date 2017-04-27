<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuickPost.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.QuickPost.QuickPost" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Quick Post</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

<dx:ASPxFormLayout runat="server">
    <Items>
        <dx:LayoutItem>
            <LayoutItemNestedControlCollection>
                <dx:LayoutItemNestedControlContainer runat="server">
                    <dx:ASPxTextBox runat="server" Theme="iOS">
                        
                    </dx:ASPxTextBox>
                </dx:LayoutItemNestedControlContainer>
            </LayoutItemNestedControlCollection>
        </dx:LayoutItem>
    </Items>
</dx:ASPxFormLayout>
</asp:Content>