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
            <asp:Repeater ID="rptJobMarkers" runat="server">
                <ItemTemplate>
                    {
                        
                        "jobid": '<%# Eval("jobID")%>',
                        "njobid": '<%# Eval("njobid")%>',

                      
                        "lat": '<%# Eval("Latitude") %>',
                        "lng": '<%# Eval("Longitude") %>',
                        "description": '<%# Eval("description") %>',
                    }
                </ItemTemplate>
                <SeparatorTemplate>
                    ,
        </SeparatorTemplate>
        </asp:Repeater>
    <asp:Repeater ID="rptJobStepMarkers" runat="server">
        <ItemTemplate>
            {                       
                "jobid": '<%# Eval("jobID")%>',
                "njobid": '<%# Eval("njobid")%>', 
                "jobstepid": '<%# Eval("jobstepid")%>',                           
                      
                "lat": '<%# Eval("Latitude") %>',
                "lng": '<%# Eval("Longitude") %>',
                "description": '<%# Eval("description") %>',
            }
        </ItemTemplate>
        <SeparatorTemplate>
            ,
        </SeparatorTemplate>
        </asp:Repeater>
    <asp:Repeater ID="rptObjectMarkers" runat="server">
        <ItemTemplate>
            {
                        
                        
                "nobjectid": '<%# Eval("nobjectid")%>',
                "objectid": '<%# Eval("objectid")%>',
                       
                      
                "lat": '<%# Eval("Latitude") %>',
                "lng": '<%# Eval("Longitude") %>',
                "objectDescription": '<%# Eval("objectDescription") %>',
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
            
            console.log("data values", [object, jobid, jobstepid, description]);
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.description,
                jobid: data.jobid,
                object: data.objectid,
                step: data.jobstepid,
                njobid: data.njobid,
                objectDescription: data.objectDescription

                
            });          
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {

                    if(data.njobid != null && data.njobid > 0){
                        mInfoWindow.setContent('<div>' + '<a href="/../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a Work Request' + '</a>' + '<br>' + '<a href="/../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a Planned Job' + '</a>' + '<br>' + '<a id="link" href="/../../Pages/WorkRequests/WorkRequestForm.aspx?n_jobid=' + data.njobid +'">' + data.jobid + '</a>' + '</div>')
                        mInfoWindow.open(map, marker);
                        return;
                    } else { };
                    if(data.objectid != null && jobstepid === "undefined" && jobid === "undefined"){
                        mInfoWindow.setContent('<div>' + '<a href="/../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a Work Request' + '</a>' + '<br>' + '<a href="/../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a Planned Job' + '</a>' + '<br>' + '<a runat="server" href="javascript:void(0);" Onclick="GoToWorkRequest">' + data.objectDescription + '</a>' + '</div>')
                        mInfoWindow.open(map, marker);
                        return;
                    }else{};
                    if(data.jobstepid > 0 && data.jobstepid != null){
                        mInfoWindow.setContent('<div>' + '<a href="/../../Pages/WorkRequests/WorkRequestForm.aspx">' + 'Make a Work Request' + '</a>' + '<br>' + '<a href="/../../Pages/PlannedJobs/PlannedJobs.aspx">' + 'Make a Planned Job' + '</a>' + '<br>' + '<a id="link" href="/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=' + data.jobstepid +'">' + data.description + '</a>' + '</div>')
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
            console.log("cluster markers", markers);
            
            var content = '';

            for (var i = 0; i < markers.length; i++){
                var marker = markers[i];
                    content += ('<H3>Items in Cluster</h3>')
                if(marker.jobid != null && marker.njobid > 0 &&  marker.step === "undefined" ){ 
                    content += ('<a href="/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobid=' + marker.njobid +'">' + marker.jobid + "</a>");
                    content += ("&nbsp");
                    content += marker.title;
                    content += ("<br>"); };
                if(marker.object != null && marker.step === "undefined" && marker.jobid === "undefined"){
                    content += ('<a href="/../../Pages/WorkRequests/WorkRequestForm.aspx">' + marker.object + "</a>");
                    content += ("&nbsp");
                    content += marker.objectDescription;
                    content += ("<br>"); 
                };
                if(marker.step > 0 && marker.step != null){
                    content += ('<a href="/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=' + marker.step +'">' + marker.jobid + "</a>");
                    content += ("&nbsp");
                    content += marker.title;
                    content += ("<br>"); 
                };
            }
            var infowindow = new google.maps.InfoWindow();
            infowindow.setPosition(cluster.getCenter());
            infowindow.close();
            infowindow.setContent(content);
            infowindow.open(map); 
            console.log("content", content);
            console.log("infowindow data", [markers.description, markers.jobid]);
            console.log("infoWindow Markers", markers);

            infowindow.addListener(infowindow, 'click', function(){
                infowindow.open(map);
            })

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
