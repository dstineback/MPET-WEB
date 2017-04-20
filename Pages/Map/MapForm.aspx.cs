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
    object MapSelected;
    decimal Latitude;
    decimal Longitude;
    int mapObject;
    string description;
    string jobID;
    string step;
    string n_jobid = "";
    string objectID;
    HttpContext context = HttpContext.Current;
    public DataTable dt { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {      
        if (HttpContext.Current.Session["MapSelected"] != null)
        {
           MapSelected = HttpContext.Current.Session["MapSelected"];          
        }      
        if(HttpContext.Current.Session["Latitude"] != null && HttpContext.Current.Session["Longitude"] != null)
        {

            Latitude = Convert.ToDecimal(HttpContext.Current.Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(HttpContext.Current.Session["Longitude"].ToString());
            mapObject = Convert.ToInt32(HttpContext.Current.Session["mapObject"].ToString());
            description = HttpContext.Current.Session["description"].ToString();
        }

        if (context.Session["jobid"] != null)
        {
            jobID = HttpContext.Current.Session["jobid"].ToString();
        }

        if (context.Session["step"] != null)
        {
            step = HttpContext.Current.Session["step"].ToString();           
        }

        Uri myUrl = Request.UrlReferrer;
        var currentURL = HttpContext.Current.Request.RawUrl.ToString();
        var query = myUrl.AbsolutePath.Split('/');

        if (myUrl.AbsolutePath != currentURL && query.Length > 2)
        {
            var page = query[3];

            if (page == "ObjectsList.aspx")
            {
                GetObjectData();
                PlannedJobsBack.Visible = false;
                RequestJobsBack.Visible = false;
            }
            if (page == "PlannedJobsList.aspx")
            {
                GetPlannedJobsData();
                RequestJobsBack.Visible = false;
                ObjectBack.Visible = false;
            }
            if( page == "RequestsList.aspx")
            {
                GetRequestData();
                ObjectBack.Visible = false;
                PlannedJobsBack.Visible = false;
            }
        }         
    }

    protected void GetObjectData()
    {
        var count = MapSelected as List<object>;
        
        if (MapSelected != null && count.Count > 0)
        {
            var mS = MapSelected as List<object>;

            dt = new DataTable();
            dt.Columns.Add("mapObject");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");

            foreach (object[] row in mS)
            {
                mapObject = Convert.ToInt32(row[0].ToString());
                Latitude = Convert.ToDecimal(row[1].ToString());
                Longitude = Convert.ToDecimal(row[2].ToString());
                description = row[3].ToString();

                dt.Rows.Add(mapObject, Latitude, Longitude, description);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToInt32(row["mapObject"]).ToString());
                objList.Add(Convert.ToDecimal(row["Latitude"]).ToString());
                objList.Add(Convert.ToDecimal(row["Longitude"]).ToString());
                objList.Add(Convert.ToString(row["description"]));
            }

            mapPoints = objList.ToArray();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        } else
        {
            mapObject = Convert.ToInt16(Session["mapObject"].ToString());
            Latitude = Convert.ToDecimal(Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(Session["Longitude"].ToString());
            description = Session["description"].ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("mapObject");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");

            dt.Rows.Add(mapObject, Latitude, Longitude, description);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
                objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
                objList.Add(Convert.ToInt32(dr["mapObject"]).ToString());
                objList.Add(dr["description"].ToString());
            }
            mapPoints = objList.ToArray();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }

        //GridView.DataSource = ds;      

    }
    protected void GetPlannedJobsData()
    {
        var count = MapSelected as List<object>;

        if (MapSelected != null && count.Count > 0)
        {
            var mS = MapSelected as List<object>;

            dt = new DataTable();
            dt.Columns.Add("jobID");
            dt.Columns.Add("n_Jobid");
            dt.Columns.Add("mapObject");         
            dt.Columns.Add("step");
            dt.Columns.Add("description");
            dt.Columns.Add("ObjectID");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            
            foreach (object[] row in mS)
            {
                jobID = row[0].ToString();
                n_jobid = row[1].ToString();
                mapObject = Convert.ToInt32(row[2].ToString());
                step = row[3].ToString();
                description = row[4].ToString();
                objectID = row[5].ToString();
                Latitude = Convert.ToDecimal(row[6].ToString());
                Longitude = Convert.ToDecimal(row[7].ToString());

                dt.Rows.Add(jobID, n_jobid, mapObject, description, step, objectID, Latitude, Longitude);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToInt32(row["mapObject"]).ToString());
                objList.Add(Convert.ToDecimal(row["Latitude"]).ToString());
                objList.Add(Convert.ToDecimal(row["Longitude"]).ToString());
                objList.Add(Convert.ToString(row["description"]));
                objList.Add(Convert.ToString(row["step"]));
                objList.Add(Convert.ToString(row["jobID"]));
                objList.Add(Convert.ToString(row["ObjectID"]));
                
            }

            mapPoints = objList.ToArray();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }
        else
        {
            mapObject = Convert.ToInt32(Session["mapObject"].ToString());
            Latitude = Convert.ToDecimal(Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(Session["Longitude"].ToString());
            description = Session["description"].ToString();
            step = Session["step"].ToString();
            jobID = Session["jobid"].ToString();


            DataTable dt = new DataTable();
            dt.Columns.Add("mapObject");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");
            dt.Columns.Add("step");
            dt.Columns.Add("jobID");


            dt.Rows.Add(mapObject, Latitude, Longitude, description, step, jobID);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
                objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
                objList.Add(Convert.ToInt32(dr["mapObject"]).ToString());
                objList.Add(dr["description"].ToString());
                objList.Add(dr["step"].ToString());
                objList.Add(dr["jobID"].ToString());
            }
            mapPoints = objList.ToArray();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }
    }
    protected void GetRequestData()
    {
        var count = MapSelected as List<object>;

        if (MapSelected != null && count.Count > 0)
        {
            var mS = MapSelected as List<object>;

            dt = new DataTable();
            dt.Columns.Add("mapObject");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");

            foreach (object[] row in mS)
            {
                mapObject = Convert.ToInt32(row[0].ToString());
                Latitude = Convert.ToDecimal(row[1].ToString());
                Longitude = Convert.ToDecimal(row[2].ToString());
                description = row[3].ToString();

                dt.Rows.Add(mapObject, Latitude, Longitude, description);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToInt32(row["mapObject"]).ToString());
                objList.Add(Convert.ToDecimal(row["Latitude"]).ToString());
                objList.Add(Convert.ToDecimal(row["Longitude"]).ToString());
                objList.Add(Convert.ToString(row["description"]));
            }

            mapPoints = objList.ToArray();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }
        else
        {
            mapObject = Convert.ToInt32(Session["mapObject"].ToString());
            Latitude = Convert.ToDecimal(Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(Session["Longitude"].ToString());
            description = Session["description"].ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("mapObject");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");

            dt.Rows.Add(mapObject, Latitude, Longitude, description);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
                objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
                objList.Add(Convert.ToInt32(dr["mapObject"]).ToString());
                objList.Add(dr["description"].ToString());
            }
            mapPoints = objList.ToArray();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }
    }
    private void backBtn_Click(object sender, EventArgs e)
    {
       
        Response.Redirect("~/Pages/Objects/ObjectsList.aspx");
    }
    protected string GetUrl()
    {
        var values = Session["mapObject"].ToString();
        return "/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + values;
    }
}