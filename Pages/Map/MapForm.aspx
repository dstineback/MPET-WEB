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
    var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
            <ItemTemplate>
                {
                "title": '<%# Eval("mapObject") %>',
                "lat": '<%# Eval("Latitude") %>',
                "lng": '<%# Eval("Longitude") %>'
                <%--"description": '<%# Eval("Description") %>'--%>
                }
            </ItemTemplate>
        <SeparatorTemplate>
        ,
        </SeparatorTemplate>
        </asp:Repeater>
    ];
    console.log(markers);
</script>
<script type="text/javascript">
    window.onload = function () {
        var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 14,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var infoWindow = new google.maps.InfoWindow();
        var map = new google.maps.Map(document.getElementById("map"), mapOptions);
        for (i = 0; i < markers.length; i++) {
            var data = markers[i]
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title
            });
            
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    infoWindow.setContent(data.description);
                    infoWindow.open(map, marker);
                });
            })(marker, data);
            
        }
    }
</script>
  
<body runat="server" >
    <div runat="server" id="backButton">
        
        <a href="../Objects/ObjectsList.aspx">Back</a>
        <dx:ASPxHyperLink runat="server" Text="Back" NavigateUrl="../Objects/ObjectsList.aspx"></dx:ASPxHyperLink>
    </div>
    <div runat="server" id="map"></div>  
  <%--  <script>
      var map;
      function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
          center: {lat: -34.397, lng: 150.644},
          zoom: 8
        });

        var marker = new google.maps.Marker({
            position: { lat: -34.398, lng: 150.64 },
            map: map
        });
      }
    </script>--%>
    <script src="https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/markerclusterer.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyARk0BJK-cnQ27jHObwdI4xtqsNY9n7z9E"
    async defer></script>
</body>
</html>
