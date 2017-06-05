<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapForm.aspx.cs" Inherits="Pages_Map_MapForm" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta name="viewport" content="initial-scale=1.0" />
    <meta charset="utf-8" />
    <%-- Style for Map --%>
    <style>
        #map {
            height: 90%;
        }
        html, body {
            height: 100%;
        }
        h1 {
            margin-bottom: -10px;
        }
        hr {
            margin-top: -10px;
        }
        #mapInfoWindow {
    
        font-size: 12px;
        width: 100%;
        }
        
    </style>
    <link href="../../Content/Css/MpetWebStyle.css" rel="stylesheet" type="text/css" />
</head>
<%-- Script finds current location of the computer/Device. Not currently in use but here for future development --%>
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

<%-- Code for Google maps functions. This code takes in data from the code behinde and loops through markers to provide display --%>
  
<body runat="server" >
    <script type="text/javascript">
        var markers = [];
        
    </script>
    
  <div id="repeaters" runat="server" style="display:none;">
     <asp:Repeater ID="rptJobStepMarkers" runat="server">
        <ItemTemplate>
                                  
                "jobid": <%# Eval("jobID")%>,
                "njobid": <%# Eval("njobid")%>, 
                "jobstepid": <%# Eval("jobstepid")%>,                                                
                "lat": <%# Eval("Latitude") %>,
                "lng": <%# Eval("Longitude") %>,
                "description": <%# Eval("description") %>,
            
             <script type="text/javascript" >         
                
                 window.markers = window.markers || [];
                 markers.push({'jobid': '<%# Eval("jobID") %>','njobid': '<%# Eval("njobid") %>', 'jobstepid': '<%# Eval("jobstepid") %>','lat': '<%# Eval("Latitude") %>', 'lng': '<%# Eval("Longitude") %>','description': '<%# Eval("description") %>'});            
            </script>
        </ItemTemplate>
        <SeparatorTemplate>
            ,
        </SeparatorTemplate>
        </asp:Repeater>
    <asp:Repeater ID="rptJobMarkers" runat="server" >
                <ItemTemplate>
                                          
                        "jobid": '<%# Eval("jobID")%>',
                        "njobid": '<%# Eval("njobid")%>',                     
                        "lat": '<%# Eval("Latitude") %>',
                        "lng": '<%# Eval("Longitude") %>',
                        "description": '<%# Eval("description") %>',
                    
                    <script type="text/javascript">
                   
                     
                     window.markers = window.markers || [];
                     markers.push({'jobid': '<%# Eval("jobID") %>','njobid': '<%# Eval("njobid") %>','lat': '<%# Eval("Latitude") %>', 'lng': '<%# Eval("Longitude") %>','description': '<%# Eval("description") %>'});
                    </script>
                </ItemTemplate>
                <SeparatorTemplate>
                    ,
        </SeparatorTemplate>
        </asp:Repeater>
    <asp:Repeater ID="rptObjectMarkers" runat="server"  >
        <ItemTemplate>
            {
                "objectid": '<%# Eval("objectid")%>',               
                "nobjectid": '<%# Eval("nobjectid")%>',
                "lat": '<%# Eval("Latitude") %>',
                "lng": '<%# Eval("Longitude") %>',
                "objectDescription": '<%# Eval("objectDescription") %>', 
                "Area": '<%# Eval("Area") %>',
                "AssetNumber": '<%# Eval("AssetNumber") %>',
                "LocationID": '<%# Eval("LocationID") %>',
            }
            <script type="text/javascript" >
                
                window.markers = window.markers || [];
                markers.push({'nobjectid': '<%# Eval("nobjectid") %>','objectid': '<%# Eval("objectid") %>','lat': '<%# Eval("Latitude") %>', 'lng': '<%# Eval("Longitude") %>','objectDescription': '<%# Eval("objectDescription") %>', 'Area': '<%# Eval("Area") %>', 'AssetNumber': '<%# Eval("AssetNumber") %>', 'LocationID': '<%# Eval("LocationID") %>'});
            </script>
        </ItemTemplate>
        <SeparatorTemplate>
            ,
        </SeparatorTemplate>
        </asp:Repeater>
   </div>
    <script type="text/javascript">
     window.onload = function() {
         var getLat = markers[0].lat;
         var getLng = markers[0].lng;

        var mapOptions = {
            center: new google.maps.LatLng(getLat, getLng),
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById("map"), mapOptions);

        var mInfoWindow = new google.maps.InfoWindow();
        var markersArray = [];
        for (i = 0; i < markers.length; i++) {
            var data = markers[i];
            var object = data.objectid;
            var jobid = String(data.jobid);
            var jobstepid = String(data.jobstepid);
            var description = String(data.description);
            var objectDescription = String(data.objectDescription);
            var njobid = String(data.njobid);
            var lat = String(data.lat);
            var lng = String(data.lng);
            var nobjectid = String(data.nobjectid);
            var area = String(data.Area);
            var assetNumber = String(data.AssetNumber);
            var locationID = String(data.LocationID);
            
            var myLatlng = new google.maps.LatLng(lat, lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.description,
                jobid: data.jobid,
                object: data.objectid,
                step: data.jobstepid,
                njobid: data.njobid,
                objectDescription: data.objectDescription,
                nobjectid: data.nobjectid,
                area: data.Area,
                assetNumber: data.AssetNumber,
                locationID: data.LocationID,
            });          
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {

                    if(data.njobid != null && data.njobid > 0 && jobstepid === "undefined"){
                        mInfoWindow.setContent('<h1>Jobs</h1>' +  '<br>' + '<hr>' + '<div id="mapInfoWindow">' + '<a href="../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a New Work Request' + '</a>' + '<br>' + '<a href="../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a New Planned Job' + '</a>' + '<br>' + 'Job ID: ' + '<a id="link" href="../../Pages/WorkRequests/WorkRequestForm.aspx?n_jobid=' + data.njobid +'">'  + data.jobid + '</a>' + 'Description: ' + data.description + '</div>')
                        mInfoWindow.open(map, marker);
                        return;
                    } else { };
                    if(data.objectid != null && jobstepid === "undefined" && jobid === "undefined"){
                        
                        localStorage.setItem("nobjectid", data.nobjectid);
                        localStorage.setItem("objectid", data.object);
                        localStorage.setItem("description", data.objectDescription);
                        localStorage.setItem("area", data.Area);
                        localStorage.setItem("assetNumber", data.AssetNumber);
                        localStorage.setItem("locationID", data.LocationID);

                        mInfoWindow.setContent('<h1>Objects</h1>'  + '<br>' + '<hr>' + '<div id="mapInfoWindow">' + '<a href="../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a New Work Request' + '</a>' + '<br>' + '<a href="../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a New Planned Job' + '</a>' + '<br>'  + '<a href="../../Pages/QuickPost/QuickPost.aspx">' + 'Make a Quick Post' + '</a>' + '<br>' +'Object ID: ' +  data.objectid + 'Description: ' + data.objectDescription + '</div>')
                        mInfoWindow.open(map, marker);
                        return;
                    }else{};
                    if(data.jobstepid > 0 && data.jobstepid != null){
                        mInfoWindow.setContent('<h1>Planned Jobs</h1>' +  '<br>' + '<hr>' + '<div id="mapInfoWindow">' + '<a href="../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a New Work Request' + '</a>' + '<br>' + '<a href="../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a New Planned Job' + '</a>' + '<br>' + 'Job ID: ' + '<a id="link" href="../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=' + data.jobstepid +'">' + data.jobid + '</a>' + 'Description: ' + data.description + '</div>')
                        mInfoWindow.open(map, marker);
                        return
                    }else{};
                });
            })(marker, data);  
            markersArray.push(marker);
        }        

        var markerCluster = new MarkerClusterer(map, markersArray, 
            {imagePath: '../../Content/v3-utility-library-master/markerclusterer/images/m',
                zoomOnClick: false,
                maxZoom: 15});

        google.maps.event.addListener(markerCluster, 'clusterclick', function(cluster) {   
            var markers = cluster.getMarkers();            
            
            var content = '<h1>Jobs/Objects</h1>' + '<br>' + '<hr>' + '<div id="windowHeading">' + '<a href="../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a New Work Request' + '</a>' + '<br>' + '<a href="../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a New Planned Job' + '</a>' + '</div>' ;
            
            for (var i = 0; i < markers.length; i++){
                var marker = markers[i];
                var id = marker.nobjectid;
                localStorage.setItem("nobjectid", marker.nobjectid);
                localStorage.setItem("objectid", marker.object);
                localStorage.setItem("description", marker.objectDescription);
                localStorage.setItem("area", marker.area);
                localStorage.setItem("assetNumber", marker.assetNumber);
                localStorage.setItem("locationID", marker.locationID);

                if(marker.object != null){
                    content += ('<div id="mapInfoWindow">');
                    content += ('Object ID:' +  ' '  + marker.object);
                    content += ("&nbsp");
                    content += 'Description:' + ' ' + marker.objectDescription + '<a href="../../Pages/QuickPost/QuickPost.aspx">' + 'Quick Post' + '</a>';
                    content += ("<br>"); 
                    content += ('</div>');                                      
                };
                if(marker.jobid != null && marker.njobid > 0 && marker.step == null){ 
                    content += ('<div id="mapInfoWindow">');
                    content += ('Job ID:' + ' ' + '<a href="../../Pages/WorkRequests/WorkRequestForm.aspx?n_jobid=' + marker.njobid +'">' + marker.jobid + "</a>");
                    content += ("&nbsp");
                    content += 'Description:' + ' ' + marker.title;
                    content += ("<br>");
                    content += ('</div>');
                };
                if(marker.step > 0 && marker.step != null){
                    content += ('<div id="mapInfoWindow">');
                    content += ('Job ID:' + ' ' + '<a href="../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=' + marker.step +'">' + marker.jobid + "</a>");
                    content += ("&nbsp");
                    content += 'Description:' + ' ' + marker.title;
                    content += ("<br>"); 
                    content += ('</div>');
                };
            }
            var infowindow = new google.maps.InfoWindow();
            infowindow.setPosition(cluster.getCenter());
            infowindow.close();
            infowindow.setContent(content);
            infowindow.open(map); 
        });
    }
</script>

    <div runat="server" id="backButton">       
        <a id="link" runat="server" title="Go to Job"></a>
        <dx:ASPxHyperLink runat="server" ID="HomeBnt" CssClass="mapbutton" Text="Back to Main Page" ForeColor="White" NavigateUrl="~/main.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="ObjectBack" CssClass="mapbutton" Text="Back to Objects List" ForeColor="White" NavigateUrl="../Objects/ObjectsList.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="PlannedJobsBack" CssClass="mapbutton" Text="Back to Planned Jobs List" ForeColor="White" NavigateUrl="~/Pages/PlannedJobs/PlannedJobsList.aspx"></dx:ASPxHyperLink>
        <dx:ASPxHyperLink runat="server" ID="RequestJobsBack" CssClass="mapbutton" Text="Back to Request List" ForeColor="White" NavigateUrl="~/Pages/WorkRequests/RequestsList.aspx"></dx:ASPxHyperLink>  
    </div>
    <div runat="server" id="map" ></div>
    <script type="text/javascript">
        if (map) {
            
            document.getElementById("repeater").style.display = 'none';
        }
    </script>
      
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyARk0BJK-cnQ27jHObwdI4xtqsNY9n7z9E" async defer></script>
    <script  src="../../Content/v3-utility-library-master/markerclusterer/src/markerclusterer.js"></script>
</body>
</html>
