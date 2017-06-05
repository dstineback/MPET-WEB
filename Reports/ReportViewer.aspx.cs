using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reports_ReportViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Uri myUrl = Request.UrlReferrer;
        var currentURL = HttpContext.Current.Request.RawUrl.ToString();
        var query = myUrl.AbsolutePath.Split('/');

        if (myUrl.AbsolutePath != currentURL && query.Length > 2)
        {
            var page = query[3];

            if (page == "RequestsList.aspx")
            {
              plannedJobDocumentviewer.Visible = false;
                plannedJob.Visible = false;
                PlanLink.Visible = false;            
            }
            if (page == "PlannedJobsList.aspx")
            {               
                workRequestDocumentViewer.Visible = false;
                workRequest.Visible = false;
                RequestLink.Visible = false;             
            }
        }        
    }

    protected void plannedJobDocumentviewer_Load(object sender, EventArgs e)
    {
        
  
    }
}