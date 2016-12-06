<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="UserControls.Common.Header" %>
<%@ Register Src="~/UserControls/Common/HeaderMenu.ascx" TagPrefix="uc" TagName="HeaderMenu" %>
<div style="float: left;">
    <a href="<%= ResolveClientUrl("~/") %>" class="top-logo"></a>
</div>
<div style="float: right;">
    <uc:HeaderMenu runat="server" ID="HeaderMenu" />
</div>
<div class="clear"></div>
