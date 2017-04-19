<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapForm.aspx.cs" Inherits="Pages_Map_MapForm" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta name="viewport" content="initial-scale=1.0" />
    <meta charset="utf-8" />
    <style>
        #map {
            height: 90%;
        }
        html, body {
            height: 100%;
        }
    </style>

    <!-- Reference to the Bing Maps SDK -->

<script type='text/javascript'>
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
     function showPosition(position) {
     lat = position.coords.latitude; 
     long = position.coords.longitude;
     }
</script>

</head>
<script type="text/javascript">
</script>
<script type="text/javascript">
    window.onload = function () {
    var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
            <ItemTemplate>
                {
                "title": '<%# Eval("mapObject") %>',
                "lat": '<%# Eval("Latitude") %>',
                "lng": '<%# Eval("Longitude") %>',
                "description": '<%# Eval("description") %>'
                }
            </ItemTemplate>
        <SeparatorTemplate>
        ,
        </SeparatorTemplate>
        </asp:Repeater>
    ];
    console.log(markers);
        var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 14,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var infoWindow = new google.maps.InfoWindow();
        var map = new google.maps.Map(document.getElementById("map"), mapOptions);
        var markersArray = [];
        for (i = 0; i < markers.length; i++) {
            var data = markers[i]
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.description               
            });
            
            
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    infoWindow.setContent(data.description);
                    infoWindow.open(map, marker);
                });
            })(marker, data);
            markersArray.push(marker);
            console.log(markersArray);
            
        }
            var cluster = new MarkerClusterer(map, markersArray, 
                {imagePath: '../../Content/v3-utility-library-master/markerclusterer/images/m',
                gridSize: 5,
                maxZoom: 15});
            console.log(cluster);
    }
</script>
  
<body runat="server" >
    <div runat="server" id="backButton">
        
        
        <dx:ASPxHyperLink runat="server" ID="ObjectBack" Text="Back to Objects List" NavigateUrl="../Objects/ObjectsList.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="PlannedJobsBack" Text="Back to Planned Jobs List" NavigateUrl="~/Pages/PlannedJobs/PlannedJobsList.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="RequestJobsBack" Text="Back to Request List" NavigateUrl="~/Pages/WorkRequests/RequestsList.aspx"></dx:ASPxHyperLink>
    </div>
    <div runat="server" id="map"></div>  
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyARk0BJK-cnQ27jHObwdI4xtqsNY9n7z9E" async defer></script>
    <script  src="../../Content/v3-utility-library-master/markerclusterer/src/markerclusterer.js"></script>
</body>
</html>
