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
using System.Diagnostics;

public partial class Pages_PlannedJobs_myJobs : System.Web.UI.Page
{

    private int UserID = 1;
    private LogonObject _oLogon;


    private string UserName = "";
    private int MatchJobType = 0;
    private int MatchJobAgainst = 0;
    private string JobIDLike = "";
    private string FromHistoryYNB = "N";
    private string IsIssuedYNB = "B";
    private DateTime StartingReqDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime EndingReqDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime StartDateStart = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime StartDateEnd = new DateTime(1960, 01, 01, 23, 59, 59);
    private string TitleContains = "";
    private string MatchLaborClass = "";
    private string MatchStatus = "";
    private string MatchGroup = "";
    private string MatchPriority = "";
    private string MatchReason = "";
    private string MatchArea = "";
    private string MatchObjectType = "";
    private string MachineIDContains = "";
    private string MatchOutcomeCode = "";
    private string ObjIDDescrContains = "";
    private string MatchTaskID = "";
    private DateTime CompStartDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime CompEndDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private string JobCrew = "";
    private string JobSupervisor = "";
    private string MatchStateRoute = "";
    private string MatchLocation = "";
    private string PostedBy = "";
    private DateTime PostStartDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime PostEndDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private string MatchWorkType = "";
    private DateTime IssuedStartDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private DateTime IssuedEndDate = new DateTime(1960, 01, 01, 23, 59, 59);
    private string RequestedBy = "";
    private string RouteTo = "";
    private string Notes = "";
    private string MiscRef = "";
    private string CostCodeID = "";
    private string FundSourceID = "";
    private string WorkOrderCodeID = "";
    private string OrgCodeID = "";
    private string FundGroupID = "";
    private string ControlSectionID = "";
    private string EquipNmberID = "";
    private string IsBreakdownYNB = "B";
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

        //Enable/Disable Buttons
        Master.ShowNewButton = true;
        Master.ShowEditButton = true;

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

        Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/LindtTest");

        ConnectionStringSettings strConnString = rootWebConfig.ConnectionStrings.ConnectionStrings["connection"];

        //String strConnString = ConfigurationManager.ConnectionStrings["ClientConnectionString"].ConnectionString;


        using (SqlConnection con = new SqlConnection(strConnString.ConnectionString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "filter_GetFilteredPlannedJobstepsList";
            cmd.Parameters.Add("@MatchJobType", SqlDbType.Int).Value = MatchJobType;
            cmd.Parameters.Add("@MatchJobAgainst", SqlDbType.Int).Value = MatchJobAgainst;
            cmd.Parameters.Add("@JobIDLike", SqlDbType.VarChar).Value = JobIDLike;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            cmd.Parameters.Add("@JobCrew", SqlDbType.VarChar).SqlValue = UserName;
            cmd.Parameters.Add("@FromHistoryYNB", SqlDbType.VarChar).SqlValue = FromHistoryYNB;
            cmd.Parameters.Add("@IsIssuedYNB", SqlDbType.VarChar).SqlValue = IsIssuedYNB;
            cmd.Parameters.Add("@StartingReqDate", SqlDbType.DateTime).SqlValue = StartingReqDate;
            cmd.Parameters.Add("@EndingReqDate", SqlDbType.DateTime).SqlValue = EndingReqDate;
            cmd.Parameters.Add("@StartDateStart", SqlDbType.DateTime).SqlValue = StartDateStart;
            cmd.Parameters.Add("@StartDateEnd", SqlDbType.DateTime).SqlValue = StartDateEnd;
            cmd.Parameters.Add("@TitleContains", SqlDbType.VarChar).SqlValue = TitleContains;
            cmd.Parameters.Add("@MatchLaborClass", SqlDbType.VarChar).SqlValue = MatchLaborClass;
            cmd.Parameters.Add("@MatchStatus", SqlDbType.VarChar).SqlValue = MatchStatus;
            cmd.Parameters.Add("@MatchGroup", SqlDbType.VarChar).SqlValue = MatchGroup;
            cmd.Parameters.Add("@MatchPriority", SqlDbType.VarChar).SqlValue = MatchPriority;
            cmd.Parameters.Add("@MatchReason", SqlDbType.VarChar).SqlValue = MatchReason;
            cmd.Parameters.Add("@MatchArea", SqlDbType.VarChar).SqlValue = MatchArea;
            cmd.Parameters.Add("@MatchObjectType", SqlDbType.VarChar).SqlValue = MatchObjectType;
            cmd.Parameters.Add("@MachineIDContains", SqlDbType.VarChar).SqlValue = MachineIDContains;
            cmd.Parameters.Add("@MatchOutcomeCode", SqlDbType.VarChar).SqlValue = MatchOutcomeCode;
            cmd.Parameters.Add("@ObjIDDescrContains", SqlDbType.VarChar).SqlValue = ObjIDDescrContains;
            cmd.Parameters.Add("@MatchTaskID", SqlDbType.VarChar).SqlValue = MatchTaskID;
            cmd.Parameters.Add("@CompStartDate", SqlDbType.DateTime).SqlValue = CompStartDate;
            cmd.Parameters.Add("@CompEndDate", SqlDbType.DateTime).SqlValue = CompEndDate;
            cmd.Parameters.Add("@JobSupervisor", SqlDbType.VarChar).SqlValue = JobSupervisor;
            cmd.Parameters.Add("@MatchStateRoute", SqlDbType.VarChar).SqlValue = MatchStateRoute;
            cmd.Parameters.Add("@MatchLocation", SqlDbType.VarChar).SqlValue = MatchLocation;
            cmd.Parameters.Add("@PostedBy", SqlDbType.VarChar).SqlValue = PostedBy;
            cmd.Parameters.Add("@PostStartDate", SqlDbType.DateTime).SqlValue = PostStartDate;
            cmd.Parameters.Add("@PostEndDate", SqlDbType.DateTime).SqlValue = PostEndDate;
            cmd.Parameters.Add("@MatchWorkType", SqlDbType.VarChar).SqlValue = MatchWorkType;
            cmd.Parameters.Add("@IssuedStartDate", SqlDbType.DateTime).SqlValue = IssuedStartDate;
            cmd.Parameters.Add("@IssuedEndDate", SqlDbType.DateTime).SqlValue = IssuedEndDate;
            cmd.Parameters.Add("@RequestedBy", SqlDbType.VarChar).SqlValue = RequestedBy;
            cmd.Parameters.Add("@RouteTo", SqlDbType.VarChar).SqlValue = RouteTo;
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar).SqlValue = Notes;
            cmd.Parameters.Add("@MiscRef", SqlDbType.VarChar).SqlValue = MiscRef;
            cmd.Parameters.Add("@CostCodeID", SqlDbType.VarChar).SqlValue = CostCodeID;
            cmd.Parameters.Add("@FundSourceID", SqlDbType.VarChar).SqlValue = FundSourceID;
            cmd.Parameters.Add("@WorkOrderCodeID", SqlDbType.VarChar).SqlValue = WorkOrderCodeID;
            cmd.Parameters.Add("@OrgCodeID", SqlDbType.VarChar).SqlValue = OrgCodeID;
            cmd.Parameters.Add("@FundGroupID", SqlDbType.VarChar).SqlValue = FundGroupID;
            cmd.Parameters.Add("@ControlSectionID", SqlDbType.VarChar).SqlValue = ControlSectionID;
            cmd.Parameters.Add("@EquipNumberID", SqlDbType.VarChar).SqlValue = EquipNmberID;
            cmd.Parameters.Add("@IsBreakdownYNB", SqlDbType.VarChar).SqlValue = IsBreakdownYNB;
            cmd.Parameters.Add("@HasAttachments", SqlDbType.VarChar).SqlValue = HasAttachments;



            cmd.Connection = con;
            try
            {
               
                con.Open();
                
                myJobsGrid.DataSource = cmd.ExecuteReader();
                myJobsGrid.DataBind();
                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    //Edit select logic for Edit button 
    private void EditSelectedRow()
    {
        var i = myJobsGrid.FocusedRowIndex;
        var v = myJobsGrid.GetRowValues(i, new[] { "n_jobstepid" });

        if (v != null)
        {
            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + v);
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

    protected string GetUrl(GridViewDataItemTemplateContainer container)
    {
        var values = (int)container.Grid.GetRowValues(container.VisibleIndex, new[] { "n_jobstepid" });
        return "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + values;
    }
}
