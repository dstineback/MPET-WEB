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


</head>
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
                console.log("markers", markers);

        var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById("map"), mapOptions);
        
        var contentString = function(markers) {
            var showInInfoWindow = "";
            for(i = 0; i < markers.length; i++){
                //add ID's, Click handlers, etc
                showInInfoWindow +="<div>"
                showInInfoWindow +=markers[i].title
                showInInfoWindow +="</div>"
            }
            return '<div class="info-content">' +
                        '<h1>Title</h1>' +
                        '<div id="bodyContent">' +
                        showInInfoWindow +
                        '</div>' +
                    '</div>';
        }
        
   
        var infowindow = function(contentString) {
            return new google.maps.InfoWindow({
                content: contentString
            });
        }

        var mInfoWindow = new google.maps.InfoWindow();
        var markersArray = [];
        for (i = 0; i < markers.length; i++) {
            var data = markers[i]
            var url = "/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + data.title;
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.description               
            });          
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    mInfoWindow.setContent('<div>' + '<a href="/../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a Work Request' + '</a>' + '<br>' + '<a href="/../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a Planned Job' + '</a>' + '<br>' + '<a id="link" href="/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=' + data.title +'">' + data.title + '</a>' + '</div>')
                    mInfoWindow.open(map, marker)                   
                });
            })(marker, data);  
            document.getElementById("link").innerHTML = url;
            document.getElementById("link").setAttribute("href",url);
            markersArray.push(marker);
        }

        var markerCluster = new MarkerClusterer(map, markersArray, 
            {imagePath: '../../Content/v3-utility-library-master/markerclusterer/images/m',
                zoomOnClick: false,
                maxZoom: 15});

        var iw = infowindow(contentString(markersArray));

        google.maps.event.addListener(markerCluster, 'clusterclick', function(cluster) {          
                iw.open(map, markersArray);         
        });
    }
</script>
 


  
<body runat="server" >
    <div runat="server" id="backButton">
        
        <a id="link" runat="server" title="Go to Job"></a>
        <dx:ASPxHyperLink runat="server" ID="ObjectBack" Text="Back to Objects List" NavigateUrl="../Objects/ObjectsList.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="PlannedJobsBack" Text="Back to Planned Jobs List" NavigateUrl="~/Pages/PlannedJobs/PlannedJobsList.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="RequestJobsBack" Text="Back to Request List" NavigateUrl="~/Pages/WorkRequests/RequestsList.aspx"></dx:ASPxHyperLink>
    </div>
    <div runat="server" id="map"></div>  
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyARk0BJK-cnQ27jHObwdI4xtqsNY9n7z9E" async defer></script>
    <script  src="../../Content/v3-utility-library-master/markerclusterer/src/markerclusterer.js"></script>
</body>
</html>
