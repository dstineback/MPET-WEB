using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Web;

using MPETDSFactory;

public partial class Pages_History_MyJobsHistory : System.Web.UI.Page
{

    //settings for buttons
    private LogonObject _oLogon;

    private WorkOrder _oJob;
    private WorkOrderJobStep _oJobStep;
    private bool _userCanDelete;
    private bool _userCanAdd;
    private bool _userCanEdit;
    private bool _userCanView;
    private const int AssignedFormId = 55;
    private readonly DateTime _nullDate = Convert.ToDateTime("1/1/1960 23:59:59");
    private string _connectionString = "";
    private bool _useWeb;
    private JobPoolType _poolType = JobPoolType.Global;


    //Params that are set for Sql Stored Procedure
    private int UserID = 1;
    private string UserName = "";
    private int MatchJobType = 0;
    private int MatchJobAgainst = 0;
    private string JobIDLike = "";

    private DateTime StartingReqDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime EndingReqDate = new DateTime(1960, 01, 01, 23, 59, 59);

    private string TitleContains = "";

    private string PriorityMatch = "";
    private string ReasonCodeMatch = "";
    private string MatchArea = "";
    private string MatchObjectType = "";
    private string MachineIDContains = "";
    private string MatchOutcomeCode = "";
    private string ObjectDescr = "";
    private string WorkOpID = "";
    private string SubAssembly = "";
    private decimal MilepostStart = 0;
    private decimal MilepostEnd = 0;
    private string MilepostDirection = "";




    private string StateRouteMatch = "";
    private string ObjLocation = "";

    private string MatchWorkType = "";

    private string RequestedByMatch = "";
    private string RouteToID = "";
    private string Notes = "";
    private string MiscRef = "";
    private string ChargeCode = "";
    private string FundSource = "";
    private string WorkOrder = "";
    private string OrgCode = "";
    private string FundGroup = "";
    private string ControlSection = "";
    private string EquipNum = "";

    private string HasAttachments = "B";

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check For Logon Class
        if (HttpContext.Current.Session["LogonInfo"] != null)
        {
            //Get Logon Info From Session
            _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

            UserName = ((LogonObject)HttpContext.Current.Session["LogonInfo"]).Username;
            UserID = ((LogonObject)HttpContext.Current.Session["LogonInfo"]).UserID;

        }
        else
        {
            Response.Redirect("~/main.aspx", true);
        }

        //Get Control That Caused Post Back
        var controlName = Request.Params.Get("__EVENTTARGET");

        if (!string.IsNullOrEmpty(controlName))

        {
            switch (controlName.Replace("ctl00$Footer$", ""))
            {
                case "NewButton":
                    {   //Call Add Row
                        AddNewRow();
                        break;
                    }
                case "EditButton":
                    {
                        //Call Edit on Selection
                        EditSelectedRow();
                        break;
                    }
                default:
                    {
                        //Do Nothing
                        break;
                    }
            }
        }

        Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        ConnectionStringSettings strConnString = rootWebConfig.ConnectionStrings.ConnectionStrings["ClientConnectionString"];

        //String strConnString = ConfigurationManager.ConnectionStrings["ClientConnectionString"].ConnectionString;

        //Sql Stored Procedure connection
        // String strConnString = ConfigurationManager.ConnectionStrings["ClientConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString.ConnectionString))
        {
            SqlCommand cmd = new SqlCommand();
            //SqlDataReader reader;
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.Parameters.AddWithValue("@User", _oLogon.UserID);

                string sql = "SELECT    JobID ,Title ,CreationDate AS StartingDate ," + "'PENDING'" + " AS StatusID ,IsHistory ,null AS PostedDate ,Notes ,JobReasonID ,Priorities.priorityid" +
                    " FROM    dbo.Jobs" +
                    " INNER JOIN dbo.JobReasons ON JobReasons.nJobReasonID = n_jobreasonid" +
                    " INNER JOIN dbo.Priorities ON Priorities.n_priorityid = n_ActionPriority" +
                    " WHERE IsRequestOnly = " + "'Y'" + " AND n_requestor = @User AND IsHistory = " + "'Y'" +
                    " UNION ALL " +
                    " SELECT JobID, JobstepTitle, StartingDate, StatusID, " + "'N'" + " AS IsHistory, PostedDate, PostNotes, JobReasons.JobReasonID, Priorities.priorityid" +
                    " FROM    dbo.Jobsteps" +
                    " INNER JOIN dbo.JobReasons ON JobReasons.nJobReasonID = Jobsteps.ReasoncodeID" +
                    " INNER JOIN dbo.Priorities ON Priorities.n_priorityid = Jobsteps.PriorityID" +
                    " INNER JOIN dbo.Statuses ON Statuses.nStatusID = Jobsteps.n_statusid" +
                    " JOIN dbo.Jobs ON Jobsteps.n_jobid = Jobs.n_Jobid " +
                    " WHERE Jobs.n_requestor = @User AND Jobs.IsHistory = " + "'Y'";

                cmd.CommandText = sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            cmd.Connection = con;
            try
            {
                con.Open();
                //reader = cmd.ExecuteReader();
                // myWorkRequestGrid.DataSource = reader;
                //MUST have to bind Sql SP to GridView
                myWorkRequestGrid.DataSource = cmd.ExecuteReader();
                myWorkRequestGrid.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            con.Close();
        }


        //using (SqlConnection con = new SqlConnection(strConnString.ConnectionString))
        //{
        //    SqlCommand cmd = new SqlCommand();
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.CommandText = "filter_GetFilteredWorkRequestsList";
        //cmd.Parameters.Add("@MatchJobType", SqlDbType.Int).Value = MatchJobType;
        //cmd.Parameters.Add("@MatchJobAgainst", SqlDbType.Int).Value = MatchJobAgainst;
        //cmd.Parameters.Add("@JobIDLike", SqlDbType.VarChar).Value = JobIDLike;
        //cmd.Parameters.Add("@StartingReqDate", SqlDbType.DateTime).SqlValue = StartingReqDate;
        //cmd.Parameters.Add("@EndingReqDate", SqlDbType.DateTime).SqlValue = EndingReqDate;
        //cmd.Parameters.Add("@TitleContains", SqlDbType.VarChar).SqlValue = TitleContains;
        //cmd.Parameters.Add("@RequestedByMatch", SqlDbType.VarChar).SqlValue = RequestedByMatch;
        //cmd.Parameters.Add("@ReasonCodeMatch", SqlDbType.VarChar).SqlValue = ReasonCodeMatch;
        //cmd.Parameters.Add("@PriorityMatch", SqlDbType.VarChar).SqlValue = PriorityMatch;
        //cmd.Parameters.Add("@MatchArea", SqlDbType.VarChar).SqlValue = MatchArea;
        //cmd.Parameters.Add("@MatchObjectType", SqlDbType.VarChar).SqlValue = MatchObjectType;
        //cmd.Parameters.Add("@StateRouteMatch", SqlDbType.VarChar).SqlValue = StateRouteMatch;
        //cmd.Parameters.Add("@MachineIDContains", SqlDbType.VarChar).SqlValue = MachineIDContains;
        //cmd.Parameters.Add("@ObjectDescr", SqlDbType.VarChar).SqlValue = ObjectDescr;
        //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
        //cmd.Parameters.Add("@RouteToID", SqlDbType.VarChar).SqlValue = RouteToID;
        //cmd.Parameters.Add("@Notes", SqlDbType.VarChar).SqlValue = Notes;
        //cmd.Parameters.Add("@MiscRef", SqlDbType.VarChar).SqlValue = MiscRef;
        //cmd.Parameters.Add("@ObjLocation", SqlDbType.VarChar).SqlValue = ObjLocation;
        //cmd.Parameters.Add("@WorkOpID", SqlDbType.VarChar).SqlValue = WorkOpID;
        //cmd.Parameters.Add("@SubAssembly", SqlDbType.VarChar).SqlValue = SubAssembly;
        //cmd.Parameters.Add("@MilepostStart", SqlDbType.Decimal).SqlValue = MilepostStart;
        //cmd.Parameters.Add("@MilepostEnd", SqlDbType.Decimal).SqlValue = MilepostEnd;
        //cmd.Parameters.Add("@MilepostDirection", SqlDbType.VarChar).SqlValue = MilepostDirection;
        //cmd.Parameters.Add("@ChargeCode", SqlDbType.VarChar).SqlValue = ChargeCode;
        //cmd.Parameters.Add("@FundSource", SqlDbType.VarChar).SqlValue = FundSource;
        //cmd.Parameters.Add("@WorkOrder", SqlDbType.VarChar).SqlValue = WorkOrder;
        //cmd.Parameters.Add("@OrgCode", SqlDbType.VarChar).SqlValue = OrgCode;
        //cmd.Parameters.Add("@FundGroup", SqlDbType.VarChar).SqlValue = FundGroup;
        //cmd.Parameters.Add("@ControlSection", SqlDbType.VarChar).SqlValue = ControlSection;
        //cmd.Parameters.Add("@EquipNum", SqlDbType.VarChar).SqlValue = EquipNum;
        //cmd.Parameters.Add("@HasAttachments", SqlDbType.VarChar).SqlValue = HasAttachments;

        //cmd.Connection = con;
        //    try
        //    {
        //        con.Open();

        //        //MUST have to bind Sql SP to GridView
        //        myWorkRequestGrid.DataSource = cmd.ExecuteReader();
        //        myWorkRequestGrid.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    //Edit select logic for Edit button 
    private void EditSelectedRow()
    {

        var x = myWorkRequestGrid.GetSelectedFieldValues("n_Jobid");
        var tempurl = x[0].ToString();


        if (tempurl != null)
        {
            //Redirect To Edit Page With Job ID
            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + tempurl, true);
        }

        if (Selection.Contains("n_Jobid"))
        {
            //Redirect To Edit Page With Job ID
            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobid=" + Selection.Get("n_Jobid"), true);
        }
    }




    //Add new logic for Add button
    private void AddNewRow()
    {
        if (true)
        {
            //Redirect To Edit Page With Job ID
            Response.Redirect("~/Pages/WorkRequests/WorkRequestForm.aspx", true);
        }
        else
        {
            throw new NotImplementedException();
        }
    }



    //hyperlink the Jobs ID on GridView to actual Job

    public string GetUrl(GridViewDataItemTemplateContainer container)
    {
        var values = (int)container.Grid.GetRowValues(container.VisibleIndex, new[] { "n_Jobid" });
        return "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobid=" + values;
    }
}