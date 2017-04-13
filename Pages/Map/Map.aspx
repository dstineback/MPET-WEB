<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteBase.master" CodeFile="Map.aspx.cs" Inherits="Pages_Map_Map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

<script type="text/javascript" src="https://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&amp;s=1"></script>

    <script type="text/javascript">
        var lat;
        var long;
var x = document.getElementById("noGEO");

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else { 
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}
function LoadMap() {
        var map = (function () {
            var bingMap = null;
            var mapElementID = "myMap";
            function getMapElement() { return document.getElementById(mapElementID); }
            function showMap() {
                if (!bingMap) {
                    getLocation();
                    createMap();
                }
            }
            function createMap() {
                if(typeof Microsoft.Maps.Map === "undefined") return;
                var mapOptions = {
                    credentials: "As0ozajCG_1t1JtHFrJC3zbv76ESjZypBRHHuaSqM1k2sbFOMij2W4tDrPaInwjB",
                    mapTypeId: Microsoft.Maps.MapTypeId.road,
                    zoom: 12,
                    center: new Microsoft.Maps.Location(lat, long),
                    enableSearchLogo: false,
                    showScalebar: false,
                    useInertia: false,
                    disableKeyboardInput: true
                };
                bingMap = new Microsoft.Maps.Map(getMapElement(), mapOptions);
            }
            return {
                showMap: showMap,
                createMap: createMap
            };
        })();
}
    </script>
    <script>

function showPosition(position) {
     lat = position.coords.latitude; 
     long = position.coords.longitude;
}
</script>
   
   <dx:ASPxPanel ID="pnlMap" runat="server" style="position:absolute; width:600px; height:600px;" OnLoad="Page_Load"></dx:ASPxPanel>
     
    <%--<asp:Panel ID="pnlMap" runat="server" style="position:absolute; width:600px; height:600px;"></asp:Panel>--%>
    
     <%-- <dx:ASPxPopupControl ID="MapPopup" runat="server" Width="600px" Height="600px" Modal="true"
        ShowPinButton="True" ShowRefreshButton="True" ShowCollapseButton="True" ShowMaximizeButton="True" 
        ClientInstanceName="popup" PopupElementID="popupArea" ShowOnPageLoad="True" 
        PopupVerticalAlign="TopSides" PopupHorizontalAlign="LeftSides"
        AllowDragging="True" AllowResize="true" CloseAction="CloseButton" 
        ScrollBars="None" HeaderText="Map" ShowFooter="true" FooterText="" PopupAnimationType="Fade" >
            <ContentStyle Paddings-Padding="0">
        </ContentStyle>
        <ClientSideEvents Shown="map.showMap" EndCallback="map.createMap"></ClientSideEvents>
        <ContentCollection>
            <dx:PopupControlContentControl>
                <div id='myMap' style="position: relative; width:100%; height:100%">
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        </dx:ASPxPopupContr--%>ol>
   
</asp:Content>
