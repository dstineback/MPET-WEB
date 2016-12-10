<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderMenu.ascx.cs" Inherits="UserControls.Common.HeaderMenu" %>
    <script type="text/javascript">
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
        }
    </script> 
<div class="mainMenu">
    <dx:ASPxMenu ID="mainMenu" runat="server" Theme="Moderno" AutoPostBack="True" DataSourceID="siteMapDataSource" SelectParentItem="True" Width="539px">
        <ClientSideEvents ItemClick="OnItemClick" />
        <Items>
            <dx:MenuItem GroupName="LogOut_Click" Name="LogOut_Click" NavigateUrl="~/Pages/LogOut.aspx">
            </dx:MenuItem>
        </Items>
        <RootItemSubMenuOffset FirstItemY="1" />

        
    </dx:ASPxMenu>
&nbsp;</div>
<dx:ASPxSiteMapDataSource ID="siteMapDataSource" runat="server" SiteMapFileName="~/web.sitemap">
</dx:ASPxSiteMapDataSource>
