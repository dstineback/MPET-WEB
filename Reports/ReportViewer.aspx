<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportViewer.aspx.cs" Inherits="Reports_ReportViewer" %>

<%@ Register assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>


<!DOCTYPE html>
<script src="../Content/Css/MpetWebStyle.css"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 608px">
    <form id="form1" runat="server">
    <div>
    <dx:ASPxHyperLink runat="server" NavigateUrl="../Pages/PlannedJobs/PlannedJobsList.aspx" Text="<---Back to Planned Jobs List"></dx:ASPxHyperLink>
    </div>
     <div>
         <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" 
            runat="server" ReportTypeName="JobReport" 
            Theme="iOS">
        </dx:ASPxDocumentViewer>

     </div>  
    </form>
</body>
</html>
