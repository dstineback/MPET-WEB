using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Reflection;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using MPETDSFactory;
using Page = System.Web.UI.Page;

public partial class Pages_Map_MapForm : Page
{
    string[] mapPoints;
    private object MapSelected;
    decimal Latitude;
    decimal Longitude;
    int mapObject;

    protected void Page_Load(object sender, EventArgs e)
    {
        var session = HttpContext.Current.Session["MapSelected"];
        
        //Latitude = Convert.ToDecimal(HttpContext.Current.Session["Latitude"].ToString());
        //Longitude = Convert.ToDecimal(HttpContext.Current.Session["Longitude"].ToString());
        //mapObject = Convert.ToInt32(HttpContext.Current.Session["mapObject"].ToString());
        GetData();
       
    }

    protected void GetData()
    {
        
        //var Latitude = HttpContext.Current.Session["Latitude"].ToString();
        //var Longitude = HttpContext.Current.Session["Longitude"].ToString();
        //var jobID = HttpContext.Current.Session["n_jobid"].ToString();

        DataTable dt = new DataTable();
        dt.Columns.Add("mapObject");
        dt.Columns.Add("Latitude");
        dt.Columns.Add("Longitude");

        dt.Rows.Add(mapObject, Latitude, Longitude);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt);

        List<string> objList = new List<string>();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
            objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
            objList.Add(Convert.ToInt32(dr["mapObject"]).ToString());
        }
        mapPoints = objList.ToArray();
        //GridView.DataSource = ds;      
        
    }
}