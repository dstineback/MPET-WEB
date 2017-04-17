<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="Map.aspx.cs" Inherits="Pages_Map_Map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

<script type="text/javascript" src="http://www.bing.com/api/maps/mapcontrol?callback=GetMap" ></script>

    <script type="text/javascript">
        var lat;
        var long;
var x = document.getElementById("noGEO");

function GetMap() {
    var map = new Microsoft.Maps.Map(document.getElementById('MyMap'), {
        credentials: 'ZjhJ67W9I5S4DRLTSr3X~78OEInuF4CzjVqlDRnVLcg~ArUL4UQBAGeTRRpiGRnn04XH-s0YhdPs6ESDtiArfH-8xv0fmq4DXY5Mv7KHAoM1',
        center: new Microsoft.Maps.Location(51.50632, -0.12714),
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        zoom: 10
    });
}
    </script>
   <%-- <script>

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else { 
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}
        function displayInfobox(e) {
            if (e.targetType == 'pushpin') {
                infobox.setLocation(e.target.getLocation());
                infobox.setOptions({ visible: true, title: e.target.Title, description: e.target.Description });
            }
        }



            function showPosition(position) {
                lat = position.coords.latitude;
                long = position.coords.longitude;
            }

</script>--%>
   
   <dx:ASPxPanel ID="MyMap" runat="server" style="position:absolute; width:600px; height:600px;" OnLoad="Page_Load"></dx:ASPxPanel>
     
   
   
</asp:Content>
