<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapForm.aspx.cs" Inherits="Pages_Map_MapForm" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="utf-8" />

    <!-- Reference to the Bing Maps SDK -->
    <script type='text/javascript'
            src='http://www.bing.com/api/maps/mapcontrol?callback=GetMap' 
            async defer></script>
    
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
    function GetMap()
    {

        var map = new Microsoft.Maps.Map('#myMap', {
            credentials: 'ZjhJ67W9I5S4DRLTSr3X~78OEInuF4CzjVqlDRnVLcg~ArUL4UQBAGeTRRpiGRnn04XH-s0YhdPs6ESDtiArfH-8xv0fmq4DXY5Mv7KHAoM1',
            //center: new Microsoft.Maps.Location(0, 0),
            mapTypeId: Microsoft.Maps.MapTypeId.aerial,
            zoom: 10,
           

        });

        //Add your post map load code here.
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

        map.setView({
            zoom: 15,
            
            mapTypeId: Microsoft.Maps.MapTypeId.road
        })
    }
    </script>
</head>
<body runat="server">
    <form runat="server">

    <div runat="server">

        <dx:ASPxGridView ID="GridView" runat="server" AutoGenerateColumns="False"
            DataSourceID="MapGridDataSource">


            <Columns>
                <dx:GridViewDataTextColumn FieldName="n_jobstepid"
                    ReadOnly="True" VisibleIndex="0">
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nJobid" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cJobID" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="stepnumber" VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="concurwithstep"
                    VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="followstep" VisibleIndex="5">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="JobTitle" VisibleIndex="6">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="JobstepTitle"
                    VisibleIndex="7"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_statusid" VisibleIndex="8">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cStatusID" VisibleIndex="9">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_laborclassid"
                    VisibleIndex="10"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cLaborClassId"
                    VisibleIndex="11"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CompletionNotes"
                    VisibleIndex="12"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="JobstepNotes"
                    VisibleIndex="13"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_ServicingEquipment"
                    VisibleIndex="14"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cServiceingEquipment"
                    VisibleIndex="15"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_groupid" VisibleIndex="16">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cGroupID" VisibleIndex="17">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_OutcomeCode"
                    VisibleIndex="18"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cOutcomeCode"
                    VisibleIndex="19"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_shiftID" VisibleIndex="20">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cShiftID" VisibleIndex="21">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_supervisorid"
                    VisibleIndex="22"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cSupervisorID"
                    VisibleIndex="23"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cSupervisorName"
                    ReadOnly="True" VisibleIndex="24"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="actualdowntime"
                    VisibleIndex="25"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="actuallength"
                    VisibleIndex="26"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="estimateddowntime"
                    VisibleIndex="27"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="estimatedlength"
                    VisibleIndex="28"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="OriginalStartingDate"
                    VisibleIndex="29"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="cOriginalStartingDate"
                    ReadOnly="True" VisibleIndex="30"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="StartingDate"
                    VisibleIndex="31"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="cStartingDate"
                    ReadOnly="True" VisibleIndex="32"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="DateTimeCompleted"
                    VisibleIndex="33"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="cDateTimeCompleted"
                    ReadOnly="True" VisibleIndex="34"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RemainingLength"
                    VisibleIndex="35"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RemainingDowntime"
                    VisibleIndex="36"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="return_within"
                    VisibleIndex="37"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_FundSrcCodeID"
                    VisibleIndex="38"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cFundSrcCodeID"
                    VisibleIndex="39"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="FundSrcCodeDesc"
                    VisibleIndex="40"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SubAssemblyID"
                    VisibleIndex="41"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cSubAssemblyID"
                    VisibleIndex="42"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PriorityID" VisibleIndex="43">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cPriorityID"
                    VisibleIndex="44"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ReasoncodeID"
                    VisibleIndex="45"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cReasoncodeID"
                    VisibleIndex="46"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PostNotes" VisibleIndex="47">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="HistoryAuditTrail"
                    VisibleIndex="48"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="Last_modified"
                    VisibleIndex="49"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="cLast_modified"
                    ReadOnly="True" VisibleIndex="50"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LastModifiedBy"
                    VisibleIndex="51"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cLastModifiedBy"
                    VisibleIndex="52"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cLastModifiedByName"
                    ReadOnly="True" VisibleIndex="53"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="CreatedOn" VisibleIndex="54">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="cCreatedOn" ReadOnly="True"
                    VisibleIndex="55"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CreatedBy" VisibleIndex="56">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cCreatedBy" VisibleIndex="57">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="cCreatedByName"
                    ReadOnly="True" VisibleIndex="58"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_AssignedArea"
                    VisibleIndex="59"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AssignedArea"
                    VisibleIndex="60"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AssignedAreaDesc"
                    VisibleIndex="61"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ObjectID" VisibleIndex="62">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ObjectDesc" VisibleIndex="63">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nObj" VisibleIndex="64">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nTaskID" VisibleIndex="65">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="TaskID" VisibleIndex="66">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nLocation" ReadOnly="True"
                    VisibleIndex="67">
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Location" VisibleIndex="68">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LocationDesc"
                    VisibleIndex="69"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nRequestor" VisibleIndex="70">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Requestor" VisibleIndex="71">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="RequestDate"
                    VisibleIndex="72"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="nWorkType" VisibleIndex="73">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="WorkType" VisibleIndex="74">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nTypeOfJob" VisibleIndex="75">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="TypeOfJob" ReadOnly="True"
                    VisibleIndex="76"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="IsHistory" VisibleIndex="77">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_HistoryTtlCrew"
                    ReadOnly="True" VisibleIndex="78"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_HistoryTtlParts"
                    ReadOnly="True" VisibleIndex="79"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_HistoryTtlEquip"
                    ReadOnly="True" VisibleIndex="80"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_PostedBy" VisibleIndex="81">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PostedBy" VisibleIndex="82">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_CompletedBy"
                    VisibleIndex="83"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CompletedBy"
                    VisibleIndex="84"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_TotalSteps"
                    ReadOnly="True" VisibleIndex="85"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="GroupDesc" VisibleIndex="86">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PriorityDesc"
                    VisibleIndex="87"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="GPSX" VisibleIndex="88">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="GPSY" VisibleIndex="89">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="GPSZ" VisibleIndex="90">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_AssignedTo"
                    VisibleIndex="91"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AssignedTo" VisibleIndex="92">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AssignedToFull"
                    ReadOnly="True" VisibleIndex="93"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="assetnumber"
                    VisibleIndex="94"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="TTLOther" VisibleIndex="95">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="b_AdditionalDamage"
                    VisibleIndex="96"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PercentOverage"
                    VisibleIndex="97"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Milepost" VisibleIndex="98">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="MilePostDirectionID"
                    VisibleIndex="99"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="StateRouteID"
                    VisibleIndex="100"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID"
                    VisibleIndex="101"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="n_HistoryTtlOther"
                    ReadOnly="True" VisibleIndex="102"></dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
        <asp:SqlDataSource runat="server" ID="MapGridDataSource"
            ConnectionString='<%$ ConnectionStrings:ClientConnectionString %>'
            SelectCommand="GetSimpleJobStep" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter SessionField="n_jobid" Name="JobID"
                    Type="Int32"></asp:SessionParameter>
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div id="myMap" style="position:relative;width:600px;height:400px;"></div>
    </form>
</body>
</html>
