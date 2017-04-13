using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Pages_Map_MapForm : Page
{
        decimal Latitude = 122;/*HttpContext.Current.Session["Latitude"].ToString();*/
        decimal Longitude = 45;/*HttpContext.Current.Session["Longitude"].ToString();*/
        int jobID = 1;/*HttpContext.Current.Session["n_jobid"].ToString();*/
    protected void Page_Load(object sender, EventArgs e)
    {
        GetData();
       
    }

    protected void GetData()
    {
        //var Latitude = HttpContext.Current.Session["Latitude"].ToString();
        //var Longitude = HttpContext.Current.Session["Longitude"].ToString();
        //var jobID = HttpContext.Current.Session["n_jobid"].ToString();

        DataTable dt = new DataTable();
        dt.Columns.Add("JobId");
        dt.Columns.Add("Latitude");
        dt.Columns.Add("Longitude");

        dt.Rows.Add(jobID, Latitude, Longitude);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        //GridView.DataSource = ds;      
        
    }
}