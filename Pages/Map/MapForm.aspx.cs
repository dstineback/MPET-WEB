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
using System.Web.UI.HtmlControls;

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
    int njobid;
    int jobstepid;
    int nobjectid;
    string objectID;
    HttpContext context = HttpContext.Current;
    public DataTable dt { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlAnchor anchor = new HtmlAnchor();
        anchor.ServerClick += new EventHandler(GoToWorkRequest);

        if (HttpContext.Current.Session["MapSelected"] != null)
        {
           MapSelected = HttpContext.Current.Session["MapSelected"];          
        }      
        if(HttpContext.Current.Session["Latitude"] != null && HttpContext.Current.Session["Longitude"] != null && HttpContext.Current.Session["description"] != null)
        {
            Latitude = Convert.ToDecimal(HttpContext.Current.Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(HttpContext.Current.Session["Longitude"].ToString());
            description = HttpContext.Current.Session["description"].ToString();
        }

        if (context.Session["jobid"] != null)
        {
            jobID = (HttpContext.Current.Session["jobid"].ToString());
        }
        if (context.Session["n_Jobid"] != null)
        {
            njobid = Convert.ToInt32(HttpContext.Current.Session["n_Jobid"].ToString());
        }

        if(context.Session["jobstepid"] != null)
        {
            jobstepid =  Convert.ToInt32(HttpContext.Current.Session["jobstepid"].ToString());
        }
        if (context.Session["step"] != null)
        {
            step = HttpContext.Current.Session["step"].ToString();           
        }

        if(context.Session["objectid"] != null)
        {
            objectID = HttpContext.Current.Session["objectid"].ToString();
        }
        if(context.Session["n_objectid"] != null)
        {
            nobjectid = Convert.ToInt32(HttpContext.Current.Session["n_objectid"].ToString());
            HttpContext.Current.Session.Add("nobjectid", nobjectid);
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
            dt.Columns.Add("nobjectid");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");
            dt.Columns.Add("objectid");

            foreach (object[] row in mS)
            {
                objectID = row[0].ToString();
                nobjectid = Convert.ToInt32(row[1].ToString());
                Latitude = Convert.ToDecimal(row[2].ToString());
                Longitude = Convert.ToDecimal(row[3].ToString());
                description = row[4].ToString();

                dt.Rows.Add(mapObject, Latitude, Longitude, description, objectID);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToInt32(row["nobjectid"]).ToString());
                objList.Add(Convert.ToDecimal(row["Latitude"]).ToString());
                objList.Add(Convert.ToDecimal(row["Longitude"]).ToString());
                objList.Add(Convert.ToString(row["description"]));
                objList.Add(Convert.ToString(row["objectid"]).ToString());
            }

            mapPoints = objList.ToArray();
            rptObjectMarkers.DataSource = dt;
            rptObjectMarkers.DataBind();
        } else
        {
            nobjectid = Convert.ToInt32(Session["n_objectid"].ToString());
            Latitude = Convert.ToDecimal(Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(Session["Longitude"].ToString());
            description = Session["description"].ToString();
            objectID = Session["objectid"].ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("nobjectid");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");
            dt.Columns.Add("objectid").ToString();

            dt.Rows.Add(nobjectid, Latitude, Longitude, description, objectID);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
                objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
                objList.Add(Convert.ToInt32(dr["nobjectid"]).ToString());
                objList.Add(dr["description"].ToString());
                objList.Add(dr["objectid"].ToString());
            }
            mapPoints = objList.ToArray();
            rptObjectMarkers.DataSource = dt;
            rptObjectMarkers.DataBind();
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
            dt.Columns.Add("jobstepid");         
            dt.Columns.Add("step");
            dt.Columns.Add("description");
            dt.Columns.Add("ObjectID");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            
            
            foreach (object[] row in mS)
            {
                jobID = row[0].ToString();
                njobid = Convert.ToInt32(row[1].ToString());
                jobstepid = Convert.ToInt32(row[2].ToString());
                step = row[3].ToString();
                description = row[4].ToString();
                objectID = row[5].ToString();
                Latitude = Convert.ToDecimal(row[6].ToString());
                Longitude = Convert.ToDecimal(row[7].ToString());

                dt.Rows.Add(jobID, njobid, jobstepid, description, step, objectID, Latitude, Longitude);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToInt32(row["jobstepid"]).ToString());
                objList.Add(Convert.ToDecimal(row["Latitude"]).ToString());
                objList.Add(Convert.ToDecimal(row["Longitude"]).ToString());
                objList.Add(Convert.ToInt32(row["njobid"]).ToString());
                objList.Add(Convert.ToString(row["description"]));
                objList.Add(Convert.ToString(row["step"]));
                objList.Add(Convert.ToString(row["jobID"]));
                objList.Add(Convert.ToString(row["ObjectID"]));
                
            }

            mapPoints = objList.ToArray();
            rptJobStepMarkers.DataSource = dt;
            rptJobStepMarkers.DataBind();
        }
        else
        {
            jobstepid = Convert.ToInt32(Session["jobstepid"].ToString());
            Latitude = Convert.ToDecimal(Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(Session["Longitude"].ToString());
            description = Session["description"].ToString();
            step = Session["step"].ToString();
            jobID = Session["jobid"].ToString();
            njobid = Convert.ToInt32(Session["n_Jobid"].ToString());


            DataTable dt = new DataTable();
            dt.Columns.Add("jobstepid");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");
            dt.Columns.Add("step");
            dt.Columns.Add("jobID");


            dt.Rows.Add(jobstepid, Latitude, Longitude, description, step, jobID);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
                objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
                objList.Add(dr["jobstepid"].ToString());
                objList.Add(dr["description"].ToString());
                objList.Add(dr["step"].ToString());
                objList.Add(dr["jobID"].ToString());
            }
            mapPoints = objList.ToArray();
            rptJobStepMarkers.DataSource = dt;
            rptJobStepMarkers.DataBind();
        }
    }
    protected void GetRequestData()
    {
        var count = MapSelected as List<object>;

        if (MapSelected != null && count.Count > 0)
        {
            var mS = MapSelected as List<object>;

            dt = new DataTable();
            dt.Columns.Add("njobid");
            dt.Columns.Add("jobID");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");

            foreach (object[] row in mS)
            {
                njobid = Convert.ToInt32(row[0].ToString());
                jobID = row[1].ToString();
                Latitude = Convert.ToDecimal(row[2].ToString());
                Longitude = Convert.ToDecimal(row[3].ToString());
                description = row[4].ToString();

                dt.Rows.Add(njobid, jobID, Latitude, Longitude, description);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToInt32(row["njobid"]).ToString());
                objList.Add(Convert.ToString(row["jobID"]));
                objList.Add(Convert.ToDecimal(row["Latitude"]).ToString());
                objList.Add(Convert.ToDecimal(row["Longitude"]).ToString());
                objList.Add(Convert.ToString(row["description"]));
            }

            mapPoints = objList.ToArray();
            rptJobMarkers.DataSource = dt;
            rptJobMarkers.DataBind();
        }
        else
        {
            njobid = Convert.ToInt32(Session["n_jobid"].ToString());
            jobID = Session["jobID"].ToString();
            Latitude = Convert.ToDecimal(Session["Latitude"].ToString());
            Longitude = Convert.ToDecimal(Session["Longitude"].ToString());
            description = Session["description"].ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("njobid");
            dt.Columns.Add("jobID");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("description");

            dt.Rows.Add(njobid, jobID, Latitude, Longitude, description);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            List<string> objList = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                objList.Add(Convert.ToDecimal(dr["Longitude"]).ToString());
                objList.Add(Convert.ToDecimal(dr["Latitude"]).ToString());
                objList.Add(Convert.ToInt32(dr["njobid"]).ToString());
                objList.Add(dr["jobID"].ToString());
                objList.Add(dr["description"].ToString());
            }
            mapPoints = objList.ToArray();
            rptJobMarkers.DataSource = dt;
            rptJobMarkers.DataBind();
        }
    }
    private void backBtn_Click()
    {
       
        Response.Redirect("~/Pages/Objects/ObjectsList.aspx");
    }
    public void GoToWorkRequest(object sender, System.EventArgs e)
    {
        Response.Redirect("www.google.com");
    }
    protected string GetUrl()
    {
        var values = Session["mapObject"].ToString();
        return "/../../Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + values;
    }
}