using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using DevExpress.Data.Filtering;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Blob;
using MPETDSFactory;

namespace Pages.WorkRequests
{
    public partial class RequestsList : System.Web.UI.Page
    {
        private LogonObject _oLogon;
        private WorkOrder _oJob;
        private AttachmentObject _oAttachments;
        private bool _userCanDelete;
        private bool _userCanAdd;
        private bool _userCanEdit;
        private bool _userCanView;
        private const int AssignedFormId = 3;
        private readonly DateTime _nullDate = Convert.ToDateTime("1/1/1960 23:59:59");
        private string _connectionString = "";
        private bool _useWeb;
        private JobPoolType _poolType = JobPoolType.Global;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check For Logon Class
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //Add Use ID TO Session
                HttpContext.Current.Session.Add("UserID", _oLogon.UserID);

                //Load Form Permissions
                if (FormSetup(_oLogon.UserID))
                {
                    //Setup Buttons 
                    Master.ShowSaveButton = false;
                    Master.ShowNewButton = _userCanAdd;
                    Master.ShowEditButton = _userCanEdit;
                    Master.ShowDeleteButton = _userCanDelete;
                    Master.ShowViewButton = _userCanView;
                    Master.ShowPlanButton = (_userCanEdit && _userCanAdd);
                    Master.ShowCopyJobButton = _userCanAdd;
                    Master.ShowRoutineJobButton = (_userCanEdit && _userCanAdd); 
                    Master.ShowForcePmButton = (_userCanEdit && _userCanAdd);
                    Master.ShowPrintButton = true;
                    Master.ShowMapDisplayButton = _userCanEdit;
                    Master.ShowPdfButton = false;
                    Master.ShowXlsButton = true;
                    Master.ShowMultiSelectButton = _userCanDelete;
                }
            }

            //Check For Post Back
            if (!IsPostBack)
            {
                //ReqGrid.DataSource = GetData(ReqGrid.FilterExpression);
                ReqGrid.DataBind();
            }
            else
            {
                //Get Script Manager
                var scriptManager = ScriptManager.GetCurrent(Page);

                //Check For Null & Post
                if (scriptManager != null && scriptManager.IsInAsyncPostBack)
                {

                }

                //Get Control That Caused Post Back
                var controlName = Request.Params.Get("__EVENTTARGET");

                //Check For Null
                if (!string.IsNullOrEmpty(controlName))
                {
                    //Determing What To Do
                    switch (controlName.Replace("ctl00$Footer$", ""))
                    {
                        case "NewButton":
                        {
                            //Call View Routine
                            AddNewRow();
                            break;
                        }
                        case "EditButton":
                        {
                            //Call View Routine
                            EditSelectedRow();
                            break;
                        }
                        case "DeleteButton":
                        {
                            //Call View Routine
                            DeleteSelectedRow();
                            break;
                        }
                        case "ViewButton":
                        {
                            //Call View Routine
                            ViewSelectedRow();
                            break;
                        }
                        case "PrintButton":
                        {
                            //Call Print Routine
                            PrintSelectedRow();
                            break;
                        }
                        case "MapDisplay":
                            {
                                //Map Request Items
                                MapRequestItem();
                                break;
                            }
                        case "ExportPDF":
                        {
                            //Call Export PDF Option
                            ExportPdf();
                            break;
                        }
                        case "ExportXLS":
                        {
                            //Call Export XLS Option
                            ExportXls();
                            break;
                        }
                        case"MultiSelect":
                        {
                            //Enable/Disable MultiSelect
                            EnableMultiSelect(!(ReqGrid.Columns[0].Visible));
                            break;
                        }
                        case "PlanJob":
                        {   
                            //Call Plan Event
                            PlanJobRoutine();

                            //Break
                            break;
                        }
                        case "CopyJob":
                        {
                            //Call Copy Event
                            CopyJobRoutine();

                            //break
                            break;
                        }
                        case "RoutineJob":
                        {
                            //Bind Grid
                            RoutineTaskGrid.DataBind();

                            //Show Popup
                            RoutineJobPopup.ShowOnPageLoad = true;

                            //break
                            break;
                        }
                        case "ForcePM":
                        {
                            //Bind Grid
                            ForcePMGrid.DataBind();

                            //Show Popup
                            ForcePMPopup.ShowOnPageLoad = true;

                            //break
                            break;
                        }
                        default:
                        {
                            //Do Nothing
                            break;
                        }
                    }
                }
                else
                {
                    //Hide Popups
                    RoutineJobPopup.ShowOnPageLoad = false;
                    ForcePMPopup.ShowOnPageLoad = false;
                }
            }

            //Enable/Disable Buttons
            Master.ShowNewButton = !(ReqGrid.Columns[0].Visible);
            Master.ShowEditButton = !(ReqGrid.Columns[0].Visible);
            Master.ShowViewButton = !(ReqGrid.Columns[0].Visible);
            Master.ShowPrintButton = !(ReqGrid.Columns[0].Visible);
            Master.ShowPlanButton = !(ReqGrid.Columns[0].Visible);
            Master.ShowCopyJobButton = !(ReqGrid.Columns[0].Visible);
            Master.ShowForcePmButton = !((ReqGrid.Columns[0].Visible) && _userCanEdit);
            Master.ShowRoutineJobButton = !((ReqGrid.Columns[0].Visible) && _userCanEdit);

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(ReqGrid.Columns[0].Visible))
            {
                //Uncheck All
                ReqGrid.Selection.UnselectAll();
            }

            //Bind Grid
            ReqGrid.DataBind();
        }

        protected void EnableMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            ReqGrid.Columns[0].Visible = showMultiSelect;
        }

        protected void grid_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        {
            if (e.Value == "|")
                return;

            if (e.Column.FieldName == "Request Date")
            {
                if (e.Kind == GridViewAutoFilterEventKind.CreateCriteria)
                {
                    String[] dates = e.Value.Split('|');
                    Session["RequestDateText"] = dates[0] + " - " + dates[1];
                    DateTime dateFrom = Convert.ToDateTime(dates[0]), dateTo = Convert.ToDateTime(dates[1]);
                    e.Criteria = (new OperandProperty("Request Date") >= dateFrom) & (new OperandProperty("Request Date") <= dateTo);
                }
                else if (e.Kind == GridViewAutoFilterEventKind.ExtractDisplayText)
                {
                    e.Value = Session["RequestDateText"].ToString();
                }
            }
        }

        private void AddNewRow()
        {
            //Redirect To Edit Page With Job ID
            Response.Redirect("~/Pages/WorkRequests/WorkRequestForm.aspx",true);
        }

        private void EditSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("Jobid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/WorkRequests/WorkRequestForm.aspx?jobid=" + Selection.Get("Jobid"), true);
            }
        }

        public bool FormSetup(int userId)
        {
            //Create Flag
            bool rightsLoaded;

            //Get Security Settings
            using (
                var oSecurity = new UserSecurityTemplate(_connectionString, _useWeb))
            {
                //Get Rights
                rightsLoaded = oSecurity.GetUserFormRights(userId, AssignedFormId,
                    ref _userCanEdit, ref _userCanAdd,
                    ref _userCanDelete, ref _userCanView);
            }

            //Return Flag
            return rightsLoaded;
        }

       

        private void DeleteSelectedRow()
        {
            //Check Permissions
            if (_userCanDelete)
            {
                //Create Deletion Bool
                var deletionDone = false;

                //Create Deltion Continue Bool
                bool continueDeletion;

                //Create Deletion Key
                var recordToDelete = -1;

              
                //Check For Multi Selection
                if ((ReqGrid.Columns[0].Visible))
                {
                    //Get Selections
                    var recordIdSelection = ReqGrid.GetSelectedFieldValues("n_Jobid");

                    

                    //Process Multi Selection
                    foreach (var record in recordIdSelection)
                    {
                        //Get ID
                        recordToDelete = Convert.ToInt32(record.ToString());

                        //Set Continue Bool
                        continueDeletion = (recordToDelete > 0);

                        //Check Continue Bool
                        if (continueDeletion)
                        {
                            //Clear Errors
                            _oJob.ClearErrors();

                            //Delete Jobstep
                            if (_oJob.Delete(recordToDelete))
                            {
                                //Set Deletion Done
                                deletionDone = true;
                            }
                        }

                        //Check Deletion Done
                        if (deletionDone)
                        {
                            //Perform Refresh
                            ReqGrid.DataBind();
                        }
                    }
                }
                else
                {
                    //Check For Job ID
                    if (Selection.Contains("n_Jobid"))
                    {
                        //Get ID
                        recordToDelete = Convert.ToInt32(Selection.Get("n_Jobid"));
                    }

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJob.ClearErrors();

                        //Delete Jobstep
                        if (_oJob.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        ReqGrid.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// Views Selected Row
        /// </summary>
        private void ViewSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("Jobid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/WorkRequests/WorkRequestForm.aspx?jobid=" + Selection.Get("Jobid"), true);
            }
        }

        private void PrintSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("n_Jobid"))
            {
                //Check For Previous Session Report Parm ID
                if (HttpContext.Current.Session["ReportParm"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportParm");
                }

                //Add Session Report Parm ID
                HttpContext.Current.Session.Add("ReportParm", Selection.Get("n_Jobid"));

                 var param = Convert.ToInt32(HttpContext.Current.Session["ReportParm"]);
                

                //Check For Previous Report Name
                if (HttpContext.Current.Session["ReportToDisplay"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportToDisplay");
                }

                //Add Report To Display
                HttpContext.Current.Session.Add("ReportToDisplay", "simplewo.rpt");

                //Redirect To Report Page
                Response.Redirect("~/Reports/ReportViewer.aspx", true);
            }
        }

        private void MapRequestItem()
        {
            var sel = Selection.Count;
            var MapSelected = ReqGrid.GetSelectedFieldValues("Jobid", "n_Jobid", "Object ID", "Title", "Latitude", "Longitude");

            if(sel > 0 || MapSelected != null) {  
            
                if (HttpContext.Current.Session["MapSelected"] != null)
                {
                    HttpContext.Current.Session.Remove("MapSelected");
                }
                if (MapSelected.Count > 0)
                {
                    HttpContext.Current.Session.Add("MapSelected", MapSelected);
                }
            

                //Check For Row Value In Hidden Field (Set Via JS)
                if (Selection.Contains("n_Jobid"))
                {
                    //Check For Previous Session Report Parm ID
                    if (HttpContext.Current.Session["n_Jobid"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("n_Jobid");
                    }

                    //Add Session Report Parm ID
                    HttpContext.Current.Session.Add("n_Jobid", Selection.Get("n_Jobid"));
                
                    if(Session["n_objectid"] != null)
                    {
                        Session.Remove("n_objectid");
                    }
                    Session.Add("objectid", Selection.Get("Object ID"));

                    if(Session["Latitude"] != null)
                    {
                        Session.Remove("Latitude");
                    }
                    Session.Add("Latitude", Selection.Get("Latitude"));
                    if(Session["Longitude"] != null)
                    {
                        Session.Remove("Longitude");
                    }
                    Session.Add("Longitude", Selection.Get("Longitude"));
                    if(Session["description"] != null)
                    {
                        Session.Remove("description");
                    }
                    Session.Add("description", Selection.Get("Title"));
                    if(Session["Jobid"] != null)
                    {
                        Session.Remove("Jobid");
                    }
                    Session.Add("jobID", Selection.Get("Jobid"));

                }
                    //Redirect To Report Page
                    Response.Redirect("~/Pages/Map/MapForm.aspx", true);
            }
            else { System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('Error trying to Map Items, No rows were selected.');</script>"); }
        }
        /// <summary>
        /// Export Grid To PDF
        /// </summary>
        private void ExportPdf()
        {
            //Call Export Routine
            gridExport.WritePdfToResponse();
        }

        //Export Grid To XLS
        private void ExportXls()
        {
            //Call Export Routine
            gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            var urlEncode = HttpUtility.UrlEncode(fileName);
            if (urlEncode != null)
                Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, urlEncode.Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            Page.Response.End();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //Set Connection Info
            _connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
            _useWeb = (ConfigurationManager.AppSettings["UsingWebService"] == "Y");

            //Initialize Classes
            _oJob = new WorkOrder(_connectionString, _useWeb);

        }

        protected void ReqGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //Redirect To Edit Page With Job ID
            ASPxWebControl.RedirectOnCallback("~/Pages/WorkRequests/WorkRequestForm.aspx?jobid=" + e.EditingKeyValue);
        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            //MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            //    .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            //methodInfo.Invoke(ScriptManager.GetCurrent(Page),
            //    new object[] { sender as UpdatePanel });

            RegisterUpdatePanel((UpdatePanel)sender);
        }

        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }

        /// <summary>
        /// Plans Selected Jobs
        /// </summary>
        protected void PlanJobRoutine()
        {
            //Plan Selected Jobs
            try
            {
                //Check For Multi Selection
                if ((ReqGrid.Columns[0].Visible))
                {
                    //Future Implimentation 
                }
                else
                {
                    //Check For Job ID
                    if (Selection.Contains("n_Jobid"))
                    {
                        //Get ID
                        var recordToPlan = Convert.ToInt32(Selection.Get("n_Jobid"));

                        //Validate Work Operation Selection
                        //Approver Must Be Allowed To Approve For Specified Work Operation
                        if (_oLogon.ValidateWorkOperations)
                        {
                            //Check For Work Op/Type ID
                            if (Selection.Contains("n_worktypeid"))
                            {
                                //Get ID
                                var workOpId = Convert.ToInt32(Selection.Get("n_worktypeid"));

                                //Check Work Op Selection
                                if ((workOpId.ToString(CultureInfo.InvariantCulture) != "") &&
                                    (workOpId > 0))
                                {
                                    //Create Found Flag
                                    var foundIt = false;

                                    //Check User's Work Operations To See If Specified One Exists
                                    for (var i = 0; i < _oLogon.UsersWorkOperations.Rows.Count; i++)
                                    {
                                        //Check Value
                                        if (_oLogon.UsersWorkOperations.Rows[i][0].ToString() ==
                                            workOpId.ToString(CultureInfo.InvariantCulture))
                                        {
                                            //Set Flag
                                            foundIt = true;

                                            //Break Loop
                                            break;
                                        }
                                    }

                                    //Check Flag
                                    if (!foundIt)
                                    {
                                        //Throw Error
                                        throw new SystemException(
                                            @"Insufficient Permissions To Approve Request For Specified Work Operation.");
                                    }
                                }
                            }
                        }

                        //Create ID
                        var plannerdJobStepId = -1;

                        //Create Class
                        var oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

                        //Get Priority
                        var priority = -1;
                        if (Selection.Contains("n_priorityid"))
                        {
                            //Set Value
                            priority = Convert.ToInt32(Selection.Get("n_priorityid"));
                        }

                        //ReasonCode
                        var reasonCode = -1;
                        if (Selection.Contains("n_jobreasonid"))
                        {
                            //Set Value
                            reasonCode = Convert.ToInt32(Selection.Get("n_jobreasonid"));
                        }

                        //Mobile Equipment
                        const int mobileEquip = -1;

                        //Sub Assembly
                        var subAssemblyId = -1;
                        if (Selection.Contains("SubAssemblyID"))
                        {
                            //Set Value
                            subAssemblyId = Convert.ToInt32(Selection.Get("SubAssemblyID"));
                        }

                        //Title
                        var jobTitle = "";
                        if (Selection.Contains("Title"))
                        {
                            //Set Value
                            jobTitle = Selection.Get("Title").ToString();
                        }

                        //Additional Details
                        var jobAdditionalInfo = "";
                        if (Selection.Contains("Notes"))
                        {
                            //Set Value
                            jobAdditionalInfo = Selection.Get("Notes").ToString();
                        }

                        if (oJobStep.InsertDefaultJobStep(recordToPlan,
                            JobType.Corrective,
                            jobTitle,
                            jobAdditionalInfo,
                            mobileEquip,
                            subAssemblyId,
                            priority,
                            reasonCode,
                            _oLogon.UserID,
                            ref plannerdJobStepId))
                        {   
                            #region Set Default Group, Supervisor, Labor & Shift

                            //Get User's Default Group And Group's Supervisor
                            try
                            {
                            

                                var loaded = true;
                                var groupId = -1;
                                var supervisorId = -1;

                                //Check Requestor Field
                                var userId = Selection.Contains("n_requestor") ? Convert.ToInt32(Selection.Get("n_requestor")) : _oLogon.UserID;

                                //Create Group Class
                                using (var oGroup = new MaintenanceGroup(_connectionString, _useWeb, _oLogon.UserID))
                                {
                                    //Get Users Group
                                    using (var dt = oGroup.GetFilteredGroupList("B", "", "", -1, userId, ref loaded))
                                    {
                                        //Check Flag
                                        if (loaded)
                                        {
                                            //Set Group
                                            if (dt.Rows.Count > 0)
                                            {
                                                //Get Group ID
                                                groupId = Convert.ToInt32(dt.Rows[0][0].ToString());

                                                //Get Supervisor
                                                if (oGroup.LoadHeaderData(groupId))
                                                {
                                                    //Get Supervisor ID
                                                    supervisorId = oGroup.SupervisorID;
                                                }
                                                else
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Loading Group/Supervisor Defaults");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Throw Error
                                            throw new SystemException(
                                                @"Error Loading Group/Supervisor Defaults");
                                        }

                                        //Update Defaults
                                        if (
                                            !oJobStep.UpdateUserDefaults(plannerdJobStepId, groupId, supervisorId,
                                                _oLogon.LaborClassID, _oLogon.ShiftID, _oLogon.UserID))
                                        {
                                            //Throw Error
                                            throw new SystemException(
                                                @"Error Saving User Defaults - " + oJobStep.LastError);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Getting User's Default Group And Supervisor - " + ex.Message);
                            }

                            #endregion

                            //Forward User To Planned Job
                            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + plannerdJobStepId, true);
                        }
                        else
                        {
                            //Throw Error
                            throw new SystemException(
                                @"Error Planning Job - " + oJobStep.LastError);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Copies Current Job
        /// </summary>
        protected void CopyJobRoutine()
        {
            //Copy Selected Job
            try
            {
                //Make Sure Multi Select Is Disabled
                if (!(ReqGrid.Columns[0].Visible))
                {
                    //Check For Job ID
                    if (Selection.Contains("n_Jobid"))
                    {
                        if (Selection.Contains("Jobid"))
                        {
                            //Create Cloning Object
                            var oCloner = new WorkOrder(_connectionString, _useWeb);

                            //Create Temp Variables
                            var clonedJobKey = -1;
                            var newCloneJobId = "";
                            var cloneErr = "";
                            var cloneGuid = "";

                            //Get Clone Job ID(Int)
                            var cloneId = Convert.ToInt32(Selection.Get("n_Jobid"));

                            //Clear Errors
                            oCloner.ClearErrors();

                            //Copy/Clone The Work Request
                            if (!oCloner.CloneJobRequest(cloneId,
                                ref clonedJobKey,
                                ref newCloneJobId,
                                ref cloneErr,
                                ref cloneGuid,
                                _oLogon.UserID))
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Copying Job Request - "
                                    + oCloner.LastError);
                            }

                            //Check For Errors
                            if (cloneErr != "")
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Copying Job Request - "
                                    + cloneErr);
                            }

                            //Forward User To New Job
                            Response.Redirect("~//Pages/WorkRequests/WorkRequestForm.aspx?jobid=" + newCloneJobId, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Processes Routine Job Selection
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ProcessRoutineJob(object sender, EventArgs e)
        {
            //Process Routine Job Selection
            try
            {
                //Check For Multi Selection
                if (!(ReqGrid.Columns[0].Visible))
                {
                    //Get Job Selections
                    var jobSelections =
                        RoutineTaskGrid.GetSelectedFieldValues("n_taskid");

                    //Create Variables Here

                    //Job ID
                    var jobId = -1;

                    //Date To Schedule
                    var dtToSchedule = DateTime.Now;

                    //Job Class
                    _oJob = new WorkOrder(_connectionString, _useWeb);

                    //Jobstep Class
                    var oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

                    //Job Step Crew Class
                    var oJobCrew = new JobstepJobCrew(_connectionString, _useWeb);

                    //Job Step Parts Class
                    var oJobParts = new JobstepJobParts(_connectionString, _useWeb);

                    //Job Attachments Class
                    var oAttachments = new AttachmentObject(_connectionString, _useWeb);

                    //Job Equipment Class
                    var oJobEquip = new JobEquipment(_connectionString, _useWeb);

                    //Task Class
                    var oTask = new Tasks(_connectionString, _useWeb);

                    //Task Parts Class
                    var oTaskParts = new TaskPartsDb(_connectionString, _useWeb);

                    //Task Steps Class
                    var oTaskSteps = new TaskStepDb(_connectionString, _useWeb);

                    //Task Crews Class
                    var oTaskCrews = new TaskCrewsDb(_connectionString, _useWeb);

                    //Task Equipment Class
                    var oTaskEquip = new TaskEquipDb(_connectionString, _useWeb);

                    //Task Attachment Class
                    var oTaskAttachments = new TaskAttachmentObject(_connectionString, _useWeb);

                    //Masterparts Class
                    var oMp = new Masterparts(_connectionString, _useWeb);

                    //Task Process Class
                    var oTaskProcessLinks = new TaskProcessLinks(_connectionString, _useWeb);

                    //Job Processes
                    var oJobProcesses = new JobProcesses(_connectionString, _useWeb);

                    //Job Id Generator
                    var oJobIdGenerator = new JobIdGenerator(_connectionString,
                        _useWeb,
                        _oLogon.UserID,
                        -1);

                    //Task Dataset
                    var dsTasks = new DataSet();

                    //Task Steps Dataset
                    var dsTaskSteps = new DataSet();

                    //Task Crews Dataset
                    var dsTaskCrews = new DataSet();

                    //Task Parts Dataset
                    var dsTaskParts = new DataSet();

                    //Task Equipment Dataset
                    var dsTaskEquip = new DataSet();

                    //Task Attachments Dataset
                    var dsTaskAttachments = new DataSet();

                    //Task Process Dataset
                    var dsTaskProcesses = new DataSet();

                    //Process Multi Selection
                    foreach (var selected in jobSelections)
                    {
                        #region Reset Variables

                        //Reset Bool
                        var success = true;

                        //Reset Job Variables
                        var assignedGuid = "";
                        var newJobId = "";
                        var jobErrorReturned = "";
                        var jobstepId = -1;
                        decimal partCost = 0;
                        var errorInfo = "";

                        //Reset Task Datasets
                        dsTasks.Clear();
                        dsTasks.Tables.Clear();
                        dsTaskSteps.Clear();
                        dsTaskSteps.Tables.Clear();
                        dsTaskCrews.Clear();
                        dsTaskCrews.Tables.Clear();
                        dsTaskParts.Clear();
                        dsTaskParts.Tables.Clear();
                        dsTaskAttachments.Clear();
                        dsTaskAttachments.Tables.Clear();

                        #endregion

                        #region Load Task Data

                        //Get Task ID
                        var editingTaskId = Convert.ToInt32(selected.ToString());

                        //Load Task Header Info
                        if (oTask.LoadData(editingTaskId))
                        {
                            //Extract Dataset
                            dsTasks = oTask.Ds.Copy();

                            //Load Task Attachment Info
                            if (oTaskAttachments.LoadData(editingTaskId))
                            {
                                //Extract Dataset
                                dsTaskAttachments = oTaskAttachments.Ds.Copy();

                                //Load Task Steps Info
                                if (oTaskSteps.LoadData(editingTaskId))
                                {
                                    //Extract Dataset
                                    dsTaskSteps = oTaskSteps.Ds.Copy();

                                    //Check Table Count
                                    if (dsTaskSteps.Tables.Count > 0)
                                    {
                                        //Check Row COunt
                                        if (dsTaskSteps.Tables[0].Rows.Count > 0)
                                        {
                                            //Loop Through Task Steps To Get Crew/Attachment Information
                                            for (var a = 0; a < dsTaskSteps.Tables[0].Rows.Count; a++)
                                            {
                                                //Load Task Crew Info
                                                if (oTaskCrews.GetPmData(editingTaskId,
                                                    Convert.ToInt32(
                                                        dsTaskSteps.Tables[0].Rows[a][0].
                                                            ToString())))
                                                {
                                                    //Check Table Count
                                                    if (oTaskCrews.Ds.Tables.Count > 0)
                                                    {
                                                        //Set Table Name Before Copying It
                                                        oTaskCrews.Ds.Tables[0].Namespace =
                                                            a.ToString(CultureInfo.InvariantCulture);

                                                        //Extract Dataset
                                                        dsTaskCrews.Tables.Add(oTaskCrews.Ds.Tables[0].Copy());
                                                    }
                                                }
                                                else
                                                {
                                                    //Error Set Flag
                                                    success = false;

                                                    //Add Error
                                                    errorInfo = "Error Loading Task Crew Information";
                                                }

                                                //Load Task Parts Info
                                                if (oTaskParts.LoadData(editingTaskId,
                                                    Convert.ToInt32(
                                                        dsTaskSteps.Tables[0].Rows[a][0].
                                                            ToString())))
                                                {
                                                    //Check Table Count
                                                    if (oTaskParts.Ds.Tables.Count > 0)
                                                    {
                                                        //Set Table Name Before Copying It
                                                        oTaskParts.Ds.Tables[0].Namespace =
                                                            a.ToString(CultureInfo.InvariantCulture);

                                                        //Extract Dataset
                                                        dsTaskParts.Tables.Add(oTaskParts.Ds.Tables[0].Copy());
                                                    }
                                                }
                                                else
                                                {
                                                    //Error Set Flag
                                                    success = false;

                                                    //Add Error
                                                    errorInfo = "Error Loading Task Part Information";
                                                }

                                                //Load Task Equipment Info
                                                if (oTaskEquip.LoadData(editingTaskId,
                                                    Convert.ToInt32(
                                                        dsTaskSteps.Tables[0].Rows[a][0].
                                                            ToString())))
                                                {
                                                    //Check Table Count
                                                    if (oTaskEquip.Ds.Tables.Count > 0)
                                                    {
                                                        //Set Table Name Before Copying It
                                                        oTaskEquip.Ds.Tables[0].Namespace =
                                                            a.ToString(CultureInfo.InvariantCulture);

                                                        //Extract Dataset
                                                        dsTaskEquip.Tables.Add(oTaskEquip.Ds.Tables[0].Copy());
                                                    }
                                                }
                                                else
                                                {
                                                    //Error Set Flag
                                                    success = false;

                                                    //Add Error
                                                    errorInfo = "Error Loading Task Equipment Information";
                                                }
                                            }

                                            //Load Task Process Info
                                            if (oTaskProcessLinks.LoadFilteredData(editingTaskId))
                                            {
                                                //Check Table Count
                                                if (oTaskProcessLinks.Ds.Tables.Count > 0)
                                                {
                                                    //Set Table Name Before Copying It
                                                    oTaskEquip.Ds.Tables[0].Namespace = "TaskProcesses";

                                                    //Extract Dataset
                                                    dsTaskProcesses.Tables.Add(oTaskProcessLinks.Ds.Tables[0].Copy());
                                                }
                                            }
                                            else
                                            {
                                                //Error Set Flag
                                                success = false;

                                                //Add Error
                                                errorInfo = "Error Loading Task Process Information";
                                            }
                                        }
                                        else
                                        {
                                            //Error
                                            //No Task Steps
                                            //By Definition 1 Step Is Required
                                            //Set Flag
                                            success = false;

                                            //Add Error
                                            errorInfo = "Error Loading Task Steps (No Task Steps Returned).";
                                        }
                                    }
                                    else
                                    {
                                        //Error
                                        //No Task Steps
                                        //By Definition 1 Step Is Required
                                        //Set Flag
                                        success = false;

                                        //Add Error
                                        errorInfo = "Error Loading Task Steps (No Table Returned).";
                                    }
                                }
                                else
                                {
                                    //Error Set Flag
                                    success = false;

                                    //Add Error
                                    errorInfo = "Error Loading Task Steps.";
                                }
                            }
                            else
                            {
                                //Error Set Flag
                                success = false;

                                //Add Error
                                errorInfo = "Error Loading Task Attachments.";
                            }
                        }
                        else
                        {
                            //Error Set Flag
                            success = false;

                            //Set Error
                            errorInfo = "Error Loading Task Header Information.";
                        }

                        #endregion

                        #region Schedules Job For Specified Object Task

                        //Check Flag
                        if (success)
                        {
                            //Get Job Pool In Use
                            if (oJobIdGenerator.GetJobPoolInUse(ref _poolType))
                            {
                                //Create Job Header
                                if (_oJob.AddNonIssuedPm(true,
                                    JobType.Routine,
                                    JobAgainstType.MaintenanceObjects,
                                    -1,
                                    "ROUTINE JOB. " + dsTasks.Tables[0].Rows[0][2],
                                    -1,
                                    -1,
                                    dsTasks.Tables[0].Rows[0][5].ToString(),
                                    Convert.ToDecimal(
                                        dsTasks.Tables[0].Rows[0][4].ToString()),
                                    0,
                                    false,
                                    -1,
                                    DateTime.Now,
                                    -1,
                                    _oLogon.UserID,
                                    dtToSchedule,
                                    0,
                                    0,
                                    0,
                                    -1,
                                    -1,
                                    0,
                                    0,
                                    0,
                                    -1,
                                    -1,
                                    editingTaskId,
                                    -1,
                                    -1,
                                    -1,
                                    ref newJobId,
                                    ref jobErrorReturned,
                                    ref assignedGuid,
                                    _oLogon.UserID))
                                {
                                    //Get Job ID
                                    jobId = _oJob.RecordID;

                                    //Loop Through Task Steps And Create Job Steps
                                    for (var a = 0; a < dsTaskSteps.Tables[0].Rows.Count; a++)
                                    {
                                        //Create Job Step
                                        if (oJobStep.Add(jobId,
                                            "ROUTINE JOB. " + dsTaskSteps.Tables[0].Rows[a][1],
                                            Convert.ToInt32(
                                                dsTaskSteps.Tables[0].Rows[a][3].ToString()),
                                            -1,
                                            503,
                                            -1,
                                            "",
                                            dsTasks.Tables[0].Rows[0][5].ToString(),
                                            -1,
                                            -1,
                                            -1,
                                            -1,
                                            -1,
                                            0,
                                            0,
                                            Convert.ToDecimal(
                                                dsTaskSteps.Tables[0].Rows[a][5].ToString()),
                                            Convert.ToDecimal(
                                                dsTaskSteps.Tables[0].Rows[a][6].ToString()),
                                            Convert.ToDecimal(
                                                dsTaskSteps.Tables[0].Rows[a][5].ToString()),
                                            Convert.ToDecimal(
                                                dsTaskSteps.Tables[0].Rows[a][6].ToString()),
                                            dtToSchedule,
                                            _nullDate,
                                            0,
                                            -1,
                                            -1,
                                            -1,
                                            -1,
                                            _oLogon.UserID))
                                        {
                                            jobstepId = oJobStep.RecordID;

                                            #region Add All Job Step Crew

                                            //Check For Crews Table Count
                                            if (dsTaskCrews.Tables.Count > 0)
                                            {
                                                //Check For Crew Row Count
                                                if (dsTaskCrews.Tables[a].Rows.Count > 0)
                                                {
                                                    //Loop Through Task Crews And Create Step Crews
                                                    for (var b = 0; b < dsTaskCrews.Tables[a].Rows.Count; b++)
                                                    {
                                                        //Create Job Crew Member
                                                        if (!oJobCrew.Add(jobId,
                                                            jobstepId,
                                                            Convert.ToInt32(
                                                                dsTaskCrews.Tables[a].Rows[b][0].
                                                                    ToString()),
                                                            Convert.ToInt32(
                                                                dsTaskCrews.Tables[a].Rows[b][1].
                                                                    ToString()),
                                                            -1,
                                                            -1,
                                                            Convert.ToInt32(
                                                                dsTaskCrews.Tables[a].Rows[b][2].
                                                                    ToString()),
                                                            Convert.ToDecimal(
                                                                dsTaskCrews.Tables[a].Rows[b][4].
                                                                    ToString()),
                                                            Convert.ToDecimal(
                                                                dsTaskCrews.Tables[a].Rows[b][3].
                                                                    ToString()),
                                                            0,
                                                            0,
                                                            _nullDate,
                                                            _oLogon.UserID,
                                                            -1,
                                                            -1))
                                                        {
                                                            //Error Hit
                                                            //Set Flag
                                                            success = false;

                                                            //Set Error Information
                                                            errorInfo = "Error Adding Routine Job Crew - " +
                                                                        oJobCrew.LastError;
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Add Job Step Parts

                                            //Check For Parts Table Count
                                            if (dsTaskParts.Tables.Count > 0)
                                            {
                                                //Loop Through Task Parts And Create Step Parts
                                                for (var c = 0;
                                                    c < dsTaskParts.Tables[a].Rows.Count;
                                                    c++)
                                                {
                                                    //Get Part Cost
                                                    if (
                                                        oMp.GetMasterpartListCost(
                                                            Convert.ToInt32(
                                                                dsTaskParts.Tables[a].Rows[c][7]
                                                                    .ToString()), ref partCost))
                                                    {
                                                        //Set Job Step Info
                                                        oJobParts.SetJobstepInfo(jobId,
                                                            jobstepId);

                                                        //Add Job Part
                                                        if (
                                                            !oJobParts.Add(
                                                                Convert.ToInt32(
                                                                    dsTaskParts.Tables[a].Rows[c
                                                                        ][7].ToString()),
                                                                Convert.ToInt32(
                                                                    dsTaskParts.Tables[a].Rows[c
                                                                        ][8].ToString()),
                                                                false,
                                                                false,
                                                                "",
                                                                dsTaskParts.Tables[a].Rows[c][3]
                                                                    .ToString(),
                                                                partCost,
                                                                "",
                                                                dsTaskParts.Tables[a].Rows[c][4]
                                                                    .ToString(),
                                                                Convert.ToDecimal(
                                                                    dsTaskParts.Tables[a].Rows[c
                                                                        ][9].ToString()),
                                                                0,
                                                                _nullDate,
                                                                -1,
                                                                -1,
                                                                _oLogon.UserID,
                                                                -1,
                                                                -1))
                                                        {
                                                            //Error Hit
                                                            //Set Flag
                                                            success = false;

                                                            //Set Error Information
                                                            errorInfo =
                                                                "Error Adding Routine Job Part - " +
                                                                oJobParts.LastError;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Error Hit
                                                        //Set Flag
                                                        success = false;

                                                        //Set Error Information
                                                        errorInfo =
                                                            "Error Getting Routine Job Part Cost - " +
                                                            oMp.LastError;
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Add All Job Step Equipment

                                            //Check For Job Equipment
                                            if (dsTaskEquip.Tables.Count > 0)
                                            {
                                                //Loop Through Equipment And Add Them
                                                for (var je = 0;
                                                    je < dsTaskEquip.Tables[0].Rows.Count;
                                                    je++)
                                                {
                                                    //Expected Schema:
                                                    //0 [n_taskequipmentid],
                                                    //1 [EquipmentID],
                                                    //2 [EquipmentDesc],
                                                    //3 [n_taskid],
                                                    //4 [n_taskstepid],
                                                    //5 [n_objectid],
                                                    //6 [estqty],
                                                    //7 CurrentRate,
                                                    //8 TotalEstCost

                                                    //Extract Values
                                                    var maintId =
                                                        (int)dsTaskEquip.Tables[0].Rows[je][5];
                                                    var equipmentCost =
                                                        (decimal)dsTaskEquip.Tables[0].Rows[je][7];
                                                    var desc =
                                                        (string)dsTaskEquip.Tables[0].Rows[je][2];
                                                    var qtyPlanned =
                                                        (decimal)
                                                            dsTaskEquip.Tables[0].Rows[je][6];

                                                    //Clear Errors
                                                    oJobEquip.ClearErrors();

                                                    //Add Equipment
                                                    if (!oJobEquip.Add(jobId,
                                                        jobstepId,
                                                        maintId,
                                                        equipmentCost,
                                                        desc,
                                                        "",
                                                        0,
                                                        qtyPlanned,
                                                        _nullDate,
                                                        -1,
                                                        _oLogon.UserID,
                                                        -1,
                                                        -1))
                                                    {
                                                        //Error Hit
                                                        //Set Flag
                                                        success = false;

                                                        //Set Error Information
                                                        errorInfo =
                                                            "Error Adding Routine Job Equipment - " +
                                                            oJobEquip.LastError;
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Add All Job Processes

                                            //Check For Process Table Count
                                            if (dsTaskProcesses.Tables.Count > 0)
                                            {
                                                if (dsTaskProcesses.Tables[0].Rows.Count > 0)
                                                {
                                                    //Only Process On First Step
                                                    if (a == 0)
                                                    {
                                                        //Loop Through Task Processes And Create Jop Processes
                                                        for (var b = 0;
                                                            b < dsTaskProcesses.Tables[0].Rows.Count;
                                                            b++)
                                                        {
                                                            //Add Process
                                                            if (!oJobProcesses.Add(jobId,
                                                                Convert.ToInt32(dsTaskProcesses.Tables[0]
                                                                    .Rows[b]["TaskProcessId"]
                                                                    .ToString()),
                                                                dsTaskProcesses.Tables[0]
                                                                    .Rows[b]["TaskProcess"]
                                                                    .ToString(),
                                                                dsTaskProcesses.Tables[0]
                                                                    .Rows[b]["Description"]
                                                                    .ToString(),
                                                                _nullDate,
                                                                0,
                                                                -1,
                                                                _oLogon.UserID))
                                                            {
                                                                //Error Hit
                                                                //Set Flag
                                                                success = false;

                                                                //Set Error Information
                                                                errorInfo =
                                                                    "Error Adding Routine Job Process - " +
                                                                    oAttachments.LastError;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Add Attachments

                                            //Check For Attachment Table Count
                                            if (dsTaskAttachments.Tables.Count > 0)
                                            {
                                                if (dsTaskAttachments.Tables[0].Rows.Count > 0)
                                                {
                                                    //Only Add To First Step
                                                    //Curretnly Task Attachments Are At Header Level Not Step Level
                                                    if (a == 0)
                                                    {
                                                        //Create System Setting Class
                                                        using (
                                                            var oSettings = new MpetSystemSettings(
                                                                _connectionString, _useWeb))
                                                        {
                                                            //Create Directory Variable
                                                            var dirToUse = "";

                                                            //Get Attachement Directory
                                                            if (!oSettings.GetAttachmentDirectory(ref dirToUse))
                                                            {
                                                                //Set Flag
                                                                success = false;

                                                                //Set Error Information
                                                                errorInfo =
                                                                    "Error Getting Default Attachment Directory - " +
                                                                    oSettings.LastError;
                                                            }

                                                            //Check Value
                                                            if (dirToUse.Trim() == "")
                                                            {
                                                                //Set Flag
                                                                success = false;

                                                                //Set Error Information
                                                                errorInfo = "Attachment Directory Not Set";
                                                            }

                                                            #region Create Temp Variables

                                                            var fn = "";

                                                            #endregion

                                                            //Loop Through Task Attachments And Create Step Attachments
                                                            for (var b = 0;
                                                                b < dsTaskAttachments.Tables[0].Rows.Count;
                                                                b++)
                                                            {
                                                                //Check For Azure Hosted Attachments
                                                                var fileName = "";

                                                                if (_oLogon.AzureHostedAttachments)
                                                                {
                                                                    //Get Full File Name
                                                                    fileName =
                                                                        dsTaskAttachments.Tables[0]
                                                                            .Rows[b][4].ToString();

                                                                    // Get Storage Account From Connection String
                                                                    var storageAccount = Microsoft.WindowsAzure.Storage
                                                                        .CloudStorageAccount
                                                                        .Parse(
                                                                            CloudConfigurationManager
                                                                                .GetSetting(
                                                                                    "StorageConnectionString"));

                                                                    // Create Blob Client
                                                                    var blobClient =
                                                                        storageAccount
                                                                            .CreateCloudBlobClient();

                                                                    //Get Attachment Blob Reference
                                                                    var container =
                                                                        blobClient.GetContainerReference
                                                                            (
                                                                                "attachments" +
                                                                                @"/Work Order Attachments/Un-Issued Jobs/" +
                                                                                jobId + @"/");

                                                                    //Check Name
                                                                    fn =
                                                                        fileName.Substring(
                                                                            fileName.LastIndexOf(@"/",
                                                                                StringComparison.Ordinal) +
                                                                            1);

                                                                    // Retrieve Refrence To Blob
                                                                    CloudBlockBlob blockBlob =
                                                                        container.GetBlockBlobReference(
                                                                            fn);

                                                                    //Check For Duplicate File
                                                                    if (!blockBlob.Exists())
                                                                    {
                                                                        try
                                                                        {
                                                                            var httpRequest =
                                                                                (HttpWebRequest)
                                                                                    WebRequest.Create(
                                                                                        fileName);

                                                                            //Set Method
                                                                            httpRequest.Method = "GET";

                                                                            // if the URI doesn't exist, an exception will be thrown here...
                                                                            using (
                                                                                var httpResponse =
                                                                                    (HttpWebResponse)
                                                                                        httpRequest
                                                                                            .GetResponse
                                                                                            ())
                                                                            {
                                                                                using (
                                                                                    var responseStream =
                                                                                        httpResponse
                                                                                            .GetResponseStream
                                                                                            ())
                                                                                {
                                                                                    //Upload File
                                                                                    blockBlob
                                                                                        .UploadFromStream
                                                                                        (responseStream);
                                                                                }
                                                                            }

                                                                            //Get File Path
                                                                            fileName =
                                                                                blockBlob.Uri.ToString();
                                                                        }
                                                                            // ReSharper disable once EmptyGeneralCatchClause
                                                                        catch (Exception)
                                                                        {
                                                                            //Do Nothing With Error-> File No Longer Exists
                                                                        }
                                                                    }
                                                                }


                                                                //Clear Errors
                                                                oAttachments.ClearErrors();

                                                                //Create Max Length Variable
                                                                var maxLen = 20;

                                                                //Check To See If FN Is Shorter
                                                                if (maxLen > fn.Length)
                                                                {
                                                                    //Change Max Length
                                                                    maxLen = fn.Length;
                                                                }

                                                                //Make Sure Short Name Version Of FN Is Only 20 Characters
                                                                fn = fn.Substring(0, maxLen);

                                                                //Add Attachment
                                                                if (!oAttachments.Add(jobId,
                                                                    jobstepId,
                                                                    _oLogon.UserID,
                                                                    fileName.Trim(),
                                                                    dsTaskAttachments.Tables[0]
                                                                        .Rows[b][2]
                                                                        .ToString(),
                                                                    dsTaskAttachments.Tables[0]
                                                                        .Rows[b][3]
                                                                        .ToString(),
                                                                    dsTaskAttachments.Tables[0]
                                                                        .Rows[b][5]
                                                                        .ToString()))
                                                                {
                                                                    //Error Hit
                                                                    //Set Flag
                                                                    success = false;

                                                                    //Set Error Information
                                                                    errorInfo =
                                                                        "Error Adding Routine Job Attachment - " +
                                                                        oAttachments
                                                                            .LastError;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            //Error Hit
                                            //Set Flag
                                            success = false;

                                            //Set Error Information
                                            errorInfo = "Error Adding Routine Job Step - " + oJobStep.LastError;
                                        }
                                    }

                                    //Check Flag
                                    if (success)
                                    {
                                    }
                                }
                                else
                                {
                                    //Error Hit
                                    //Set Flag
                                    success = false;

                                    //Set Error Information
                                    errorInfo = "Error Adding Routine Job - " + _oJob.LastError;
                                }
                            }
                            else
                            {
                                //Error Hit
                                //Set Flag
                                success = false;

                                //Set Error Information
                                errorInfo = "Error Determining Job Pool Type - " + oJobIdGenerator.LastError;
                            }
                        }

                        #endregion

                        #region Show Job

                        //Check For Success
                        if (success)
                        {
                            //Create New Job ID Variable
                            var newStandardJobId = "";

                            //Clear Errors
                            _oJob.ClearErrors();

                            //Convert Job ID
                            if (_oJob.ConvertJobToStandardJob(jobId,
                                _oLogon.UserID,
                                ref newStandardJobId))
                            {
                                //Forward User To Copied Work Order
                                Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + jobstepId, true);
                            }
                            else
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Generating Routine Job - " + _oJob.LastError);
                            }
                        }
                        else
                        {
                            //Throw Error
                            throw new SystemException(
                                @"Error Generating Routine Job - " + errorInfo);

                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Processes Force PM Selections
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ProcessForcePmJob(object sender, EventArgs e)
        {
            //Process Force PM Selection
            try
            {
                //Check For Multi Selection
                if (!(ReqGrid.Columns[0].Visible))
                {
                    //Get Job Selections
                    var jobSelections =
                        ForcePMGrid.GetSelectedFieldValues("n_objtaskid");

                    //Create Variables Here
                    //The PM Scheduler Class Creates Them Globally
                    //This Routine Does It Here Because This Isn't A Common Procedure And Creating It Globally Would Be A Waste

                    //Date To Schedule
                    var dtToSchedule = DateTime.Now;

                    //Job Class
                    _oJob = new WorkOrder(_connectionString, _useWeb);

                    //Jobstep Class
                    var oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

                    //Job Step Crew Class
                    var oJobCrew = new JobstepJobCrew(_connectionString, _useWeb);

                    //Job Step Parts Class
                    var oJobParts = new JobstepJobParts(_connectionString, _useWeb);

                    //Job Attachments Class
                    var oAttachments = new AttachmentObject(_connectionString, _useWeb);

                    //Task Class
                    var oTask = new Tasks(_connectionString, _useWeb);

                    //Task Parts Class
                    var oTaskParts = new TaskPartsDb(_connectionString, _useWeb);

                    //Task Steps Class
                    var oTaskSteps = new TaskStepDb(_connectionString, _useWeb);

                    //Task Crews Class
                    var oTaskCrews = new TaskCrewsDb(_connectionString, _useWeb);

                    //Task Attachment Class
                    var oTaskAttachments = new TaskAttachmentObject(_connectionString, _useWeb);

                    //Masterparts Class
                    var oMp = new Masterparts(_connectionString, _useWeb);

                    //Object Tasks Class
                    var oObjTasks = new ObjectTasksDb(_connectionString, _useWeb);

                    //Maintenance Object Class
                    var oMo = new MaintenanceObject(_connectionString, _useWeb);

                    //Task Process Class
                    var oTaskProcessLinks = new TaskProcessLinks(_connectionString, _useWeb);

                    //Job Processes
                    var oJobProcesses = new JobProcesses(_connectionString, _useWeb);

                    //Job Members
                    var oJobMembers = new JobMembers(_connectionString, _useWeb);

                    //Task Dataset
                    var dsTasks = new DataSet();

                    //Task Steps Dataset
                    var dsTaskSteps = new DataSet();

                    //Task Crews Dataset
                    var dsTaskCrews = new DataSet();

                    //Task Parts Dataset
                    var dsTaskParts = new DataSet();

                    //Task Attachments Dataset
                    var dsTaskAttachments = new DataSet();

                    //Task Process Dataset
                    var dsTaskProcesses = new DataSet();

                    //Job Member Dataset
                    var dsJobMembers = new DataSet();

                    //Process Multi Selection
                    foreach (var selected in jobSelections)
                    {

                        #region Reset Variables

                        //Reset Bool
                        var success = true;

                        //Reset Job Variables
                        var assignedGuid = "";
                        var newJobId = "";
                        var jobErrorReturned = "";
                        var jobstepId = -1;
                        decimal partCost = 0;
                        var errorInfo = "";

                        //Reset Task Variables
                        var editingTaskId = -1;
                        var editingMoid = -1;

                        //Reset Task Datasets
                        dsTasks.Clear();
                        dsTasks.Tables.Clear();
                        dsTaskSteps.Clear();
                        dsTaskSteps.Tables.Clear();
                        dsTaskCrews.Clear();
                        dsTaskCrews.Tables.Clear();
                        dsTaskParts.Clear();
                        dsTaskParts.Tables.Clear();
                        dsTaskAttachments.Clear();
                        dsTaskAttachments.Tables.Clear();

                        //Create/Set Costing Variables
                        var costCode = -1;
                        var fundingSource = -1;
                        var workOrder = -1;
                        var workOp = -1;
                        var orgCode = -1;
                        var fundingGroup = -1;
                        var equipNumber = -1;
                        var controlSection = -1;

                        #endregion

                        #region Load Task Data

                        //Get Object Task ID
                        var editingObjTaskId = Convert.ToInt32(selected.ToString());

                        //Load Object Task Data
                        if (oObjTasks.LoadData(editingObjTaskId))
                        {
                            //Expected Schema
                            //[n_objtaskid], 
                            //[n_groupid], 
                            //[n_jobreasonid], 
                            //[n_laborclassid], 
                            //[n_objectid], 
                            //[n_objtypetaskid], 
                            //[n_ownerid], 
                            //[n_priorityid], 
                            //[n_shiftid], 
                            //[n_supervisorid], 
                            //[n_taskid], 
                            //[n_worktypeid], 
                            //[begindate], 
                            //[b_disabled], 
                            //[b_fixedinterval], 
                            //[b_seasonal], 
                            //[datelastdone], 
                            //[endday], 
                            //[endmo], 
                            //[every], 
                            //[mm_dd_of_yr], 
                            //[on_day_of_month], 
                            //[on_day_of_week], 
                            //[point1last], 
                            //[point1trigger], 
                            //[point2last], 
                            //[point2trigger], 
                            //[point3last], 
                            //[point3trigger], 
                            //[shutdown_code], 
                            //[defer_days], 
                            //[return_within], 
                            //[startday], 
                            //[startmo], 
                            //[starttime], 
                            //[time_units], 
                            //[pending_delete], 
                            //nSubAssemblyID

                            //Check For Returned Table
                            if (oObjTasks.Ds.Tables.Count > 0)
                            {
                                //Check For Rows
                                if (oObjTasks.Ds.Tables[0].Rows.Count > 0)
                                {
                                    //Get Task ID
                                    editingTaskId =
                                        Convert.ToInt32(oObjTasks.Ds.Tables[0].Rows[0][10].ToString());

                                    //Get Maintenance Object ID
                                    editingMoid = Convert.ToInt32(oObjTasks.Ds.Tables[0].Rows[0][4].ToString());

                                    //Load Maintenance Object Information
                                    if (oMo.LoadData(editingMoid))
                                    {
                                        //Check For Returned Table
                                        if (oMo.Ds.Tables.Count > 0)
                                        {
                                            //Check For Rows
                                            if (oMo.Ds.Tables[0].Rows.Count == 0)
                                            {
                                                //Set Error Flag
                                                success = false;

                                                //Set Error
                                                errorInfo =
                                                    "Error Loading Maintenance Object Information - No Rows Returned";
                                            }
                                            else
                                            {
                                                using (
                                                    var oMoCosting = new MaintenanceObject(_connectionString,
                                                        _useWeb))
                                                {
                                                    //Create Flag
                                                    var loaded = false;

                                                    //Load Data
                                                    using (
                                                        var moDs =
                                                            oMoCosting.GetMaintObjRecordInfo2(editingMoid,
                                                                _oLogon.UserID,
                                                                ref loaded))
                                                    {
                                                        //Check Flag
                                                        if (loaded)
                                                        {
                                                            //Check Table Count
                                                            if (moDs.Tables.Count > 0)
                                                            {
                                                                //Check Row COunt
                                                                if (moDs.Tables[0].Rows.Count > 0)
                                                                {
                                                                    //Get Values

                                                                    //Check/Get Cost Code
                                                                    costCode =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_costcodeid"].ToString());

                                                                    //Check/Get Funding Source
                                                                    fundingSource =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_FundSrcCodeID"].ToString());

                                                                    //Check/Get Work Order
                                                                    workOrder =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_WorkOrderCodeID"].ToString());

                                                                    //Check/Get Work Op
                                                                    workOp =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0]["n_WorkOpID"]
                                                                                .ToString());

                                                                    //Check/Get Org Code
                                                                    orgCode =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_OrganizationCodeID"].ToString
                                                                                ());

                                                                    //Check/Get Funding Group
                                                                    fundingGroup =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_FundingGroupCodeID"].ToString
                                                                                ());

                                                                    //Check/Get Equipment Number
                                                                    equipNumber =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_EquipmentNumberID"].ToString());

                                                                    //Check/Get Control Section
                                                                    controlSection =
                                                                        Convert.ToInt32(
                                                                            moDs.Tables[0].Rows[0][
                                                                                "n_ControlSectionID"].ToString());
                                                                }
                                                                else
                                                                {
                                                                    //Set Error Flag
                                                                    success = false;

                                                                    //Set Error
                                                                    errorInfo =
                                                                        "Error Loading Maintenance Object Information - No Cost Row Information Returned";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Set Error Flag
                                                                success = false;

                                                                //Set Error
                                                                errorInfo =
                                                                    "Error Loading Maintenance Object Information - No Cost Table Information Returned";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Set Error Flag
                                                            success = false;

                                                            //Set Error
                                                            errorInfo =
                                                                "Error Loading Maintenance Object Information - No Cost Information Returned";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Set Error Flag
                                            success = false;

                                            //Set Error
                                            errorInfo =
                                                "Error Loading Maintenance Object Information - No Table Information Returned";
                                        }
                                    }
                                    else
                                    {
                                        //Set Error Flag
                                        success = false;

                                        //Set Error
                                        errorInfo = "Error Loading Maintenance Object Information - " +
                                                    oMo.LastError;
                                    }
                                }
                                else
                                {
                                    //Set Success Bool
                                    success = false;

                                    //Set Error
                                    errorInfo = "Error Loading Object Task Data - No Rows Returned";
                                }
                            }
                            else
                            {
                                //Set Success Bool
                                success = false;

                                //Set Error
                                errorInfo = "Error Loading Object Task Data - No Table Information Returned";
                            }
                        }
                        else
                        {
                            //Set Success Bool
                            success = false;

                            //Set Error
                            errorInfo = "Error Loading Object Task Data - " + oObjTasks.LastError;
                        }

                        //Load Task Header Info
                        if (oTask.LoadData(editingTaskId))
                        {
                            //Extract Dataset
                            dsTasks = oTask.Ds.Copy();

                            //Load Task Attachment Info
                            if (oTaskAttachments.LoadData(editingTaskId))
                            {
                                //Extract Dataset
                                dsTaskAttachments = oTaskAttachments.Ds.Copy();

                                //Load Task Process Info
                                if (oTaskProcessLinks.LoadFilteredData(editingTaskId))
                                {
                                    //Check Table Count
                                    if (oTaskProcessLinks.Ds.Tables.Count > 0)
                                    {
                                        //Set Table Name Before Copying It
                                        oTaskProcessLinks.Ds.Tables[0].Namespace = "TaskProcesses";

                                        //Extract Dataset
                                        dsTaskProcesses.Tables.Add(oTaskProcessLinks.Ds.Tables[0].Copy());
                                    }
                                }
                                else
                                {
                                    //Error Set Flag
                                    success = false;

                                    //Add Error
                                    errorInfo = "Error Loading Task Process Information";
                                }

                                //Check Flag
                                if (success)
                                {
                                    //Load Job Member Info
                                    using (var tmpData = oJobMembers.GetRouteMembersForJobMembers(editingMoid, ref success))
                                    {
                                        //Check Flag
                                        if (success)
                                        {
                                            //Set Table Name Before Copying It
                                            tmpData.Namespace = "JobMembers";

                                            //Extract Dataset
                                            dsJobMembers.Tables.Add(tmpData.Copy());
                                        }
                                        else
                                        {
                                            //Error Set Flag
                                            success = false;

                                            //Add Error
                                            errorInfo = "Error Loading Task Process Information";
                                        }
                                    }
                                }

                                //Check Flag
                                if (success)
                                {
                                    //Load Task Steps Info
                                    if (oTaskSteps.LoadData(editingTaskId))
                                    {
                                        //Extract Dataset
                                        dsTaskSteps = oTaskSteps.Ds.Copy();

                                        //Check Table Count
                                        if (dsTaskSteps.Tables.Count > 0)
                                        {
                                            //Check Row COunt
                                            if (dsTaskSteps.Tables[0].Rows.Count > 0)
                                            {
                                                //Loop Through Task Steps To Get Crew/Attachment Information
                                                for (var a = 0; a < dsTaskSteps.Tables[0].Rows.Count; a++)
                                                {
                                                    //Load Task Crew Info
                                                    if (oTaskCrews.GetPmData(editingTaskId,
                                                        Convert.ToInt32(
                                                            dsTaskSteps.Tables[0].Rows[a][0].
                                                                ToString())))
                                                    {
                                                        //Check Table Count
                                                        if (oTaskCrews.Ds.Tables.Count > 0)
                                                        {
                                                            //Set Table Name Before Copying It
                                                            oTaskCrews.Ds.Tables[0].Namespace =
                                                                a.ToString(CultureInfo.InvariantCulture);

                                                            //Extract Dataset
                                                            dsTaskCrews.Tables.Add(
                                                                oTaskCrews.Ds.Tables[0].Copy());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Error Set Flag
                                                        success = false;

                                                        //Add Error
                                                        errorInfo = "Error Loading Task Crew Information";
                                                    }

                                                    //Load Task Parts Info
                                                    if (oTaskParts.LoadData(editingTaskId,
                                                        Convert.ToInt32(
                                                            dsTaskSteps.Tables[0].Rows[a][0].
                                                                ToString())))
                                                    {
                                                        //Check Table Count
                                                        if (oTaskParts.Ds.Tables.Count > 0)
                                                        {
                                                            //Set Table Name Before Copying It
                                                            oTaskParts.Ds.Tables[0].Namespace =
                                                                a.ToString(CultureInfo.InvariantCulture);

                                                            //Extract Dataset
                                                            dsTaskParts.Tables.Add(
                                                                oTaskParts.Ds.Tables[0].Copy());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Error Set Flag
                                                        success = false;

                                                        //Add Error
                                                        errorInfo = "Error Loading Task Part Information";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //Error
                                                //No Task Steps
                                                //By Definition 1 Step Is Required
                                                //Set Flag
                                                success = false;

                                                //Add Error
                                                errorInfo = "Error Loading Task Steps (No Task Steps Returned).";
                                            }
                                        }
                                        else
                                        {
                                            //Error
                                            //No Task Steps
                                            //By Definition 1 Step Is Required
                                            //Set Flag
                                            success = false;

                                            //Add Error
                                            errorInfo = "Error Loading Task Steps (No Table Returned).";
                                        }
                                    }
                                    else
                                    {
                                        //Error Set Flag
                                        success = false;

                                        //Add Error
                                        errorInfo = "Error Loading Task Steps.";
                                    }
                                }
                            }
                            else
                            {
                                //Error Set Flag
                                success = false;

                                //Add Error
                                errorInfo = "Error Loading Task Attachments.";
                            }
                        }
                        else
                        {
                            //Error Set Flag
                            success = false;

                            //Set Error
                            errorInfo = "Error Loading Task Header Information.";
                        }

                        #endregion

                        #region Schedules Job For Specified Object Task

                        //Check Flag
                        if (success)
                        {
                            //Generate The Next Job ID
                            var oJobIdGenerator = new JobIdGenerator(_connectionString,
                                _useWeb,
                                _oLogon.UserID,
                                Convert.ToInt32(
                                    oMo.Ds.Tables[0].Rows[0][6].ToString()));

                            //Get Job Pool In Use
                            if (oJobIdGenerator.GetJobPoolInUse(ref _poolType))
                            {
                                //Create Job Header
                                if (_oJob.AddNonIssuedPm(true,
                                    JobType.Preventive,
                                    JobAgainstType.MaintenanceObjects,
                                    editingMoid,
                                    "FORCED PM JOB. " + dsTasks.Tables[0].Rows[0][2],
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][7].ToString()),
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][2].ToString()),
                                    dsTasks.Tables[0].Rows[0][5].ToString(),
                                    Convert.ToDecimal(
                                        dsTasks.Tables[0].Rows[0][4].ToString()),
                                    0,
                                    false,
                                    -1,
                                    DateTime.Now,
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][7].ToString()),
                                    _oLogon.UserID,
                                    dtToSchedule,
                                    0,
                                    0,
                                    0,
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][1].ToString()),
                                    -1,
                                    0,
                                    0,
                                    0,
                                    -1,
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][6].ToString()),
                                    editingTaskId,
                                    -1,
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][11].ToString()),
                                    Convert.ToInt32(
                                        oObjTasks.Ds.Tables[0].Rows[0][37].ToString()),
                                    ref newJobId,
                                    ref jobErrorReturned,
                                    ref assignedGuid,
                                    _oLogon.UserID))
                                {
                                    //Get Job ID
                                    var jobId = _oJob.RecordID;

                                    //Update Costing Information
                                    if (_oJob.UpdateJobCosting(jobId,
                                        costCode,
                                        fundingSource,
                                        workOrder,
                                        workOp,
                                        orgCode,
                                        fundingGroup,
                                        equipNumber,
                                        controlSection,
                                        _oLogon.UserID))
                                    {
                                        //Loop Through Task Steps And Create Job Steps
                                        for (var a = 0; a < dsTaskSteps.Tables[0].Rows.Count; a++)
                                        {
                                            //Create Job Step
                                            if (oJobStep.Add(jobId,
                                                "FORCED PM JOB. " +
                                                dsTaskSteps.Tables[0].Rows[a][1],
                                                Convert.ToInt32(
                                                    dsTaskSteps.Tables[0].Rows[a][3].ToString()),
                                                Convert.ToInt32(
                                                    dsTaskSteps.Tables[0].Rows[a][4].ToString()),
                                                503,
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][3].ToString()),
                                                "",
                                                dsTasks.Tables[0].Rows[0][5].ToString(),
                                                -1,
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][1].ToString()),
                                                -1,
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][8].ToString()),
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][9].ToString()),
                                                0,
                                                0,
                                                Convert.ToDecimal(
                                                    dsTaskSteps.Tables[0].Rows[a][5].ToString()),
                                                Convert.ToDecimal(
                                                    dsTaskSteps.Tables[0].Rows[a][6].ToString()),
                                                Convert.ToDecimal(
                                                    dsTaskSteps.Tables[0].Rows[a][5].ToString()),
                                                Convert.ToDecimal(
                                                    dsTaskSteps.Tables[0].Rows[a][6].ToString()),
                                                dtToSchedule,
                                                _nullDate,
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][31].ToString()),
                                                -1,
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][37].ToString()),
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][7].ToString()),
                                                Convert.ToInt32(
                                                    oObjTasks.Ds.Tables[0].Rows[0][2].ToString()),
                                                _oLogon.UserID))
                                            {
                                                jobstepId = oJobStep.RecordID;

                                                //Update Jobstep Costing
                                                if (oJobStep.UpdateJobstepCosting(jobId,
                                                    jobstepId,
                                                    costCode,
                                                    fundingSource,
                                                    workOrder,
                                                    workOp,
                                                    orgCode,
                                                    fundingGroup,
                                                    equipNumber,
                                                    controlSection,
                                                    _oLogon.UserID))
                                                {
                                                    //Check For Crews Table Count
                                                    if (dsTaskCrews.Tables.Count > 0)
                                                    {
                                                        //Check For Crew Row Count
                                                        if (dsTaskCrews.Tables[a].Rows.Count > 0)
                                                        {
                                                            //Loop Through Task Crews And Create Step Crews
                                                            for (var b = 0;
                                                                b < dsTaskCrews.Tables[a].Rows.Count;
                                                                b++)
                                                            {
                                                                //Create Job Crew Member
                                                                if (oJobCrew.Add(jobId,
                                                                    jobstepId,
                                                                    Convert.ToInt32(
                                                                        dsTaskCrews.Tables[a].Rows
                                                                            [b][0].
                                                                            ToString()),
                                                                    Convert.ToInt32(
                                                                        dsTaskCrews.Tables[a].Rows
                                                                            [b][1].
                                                                            ToString()),
                                                                    -1,
                                                                    -1,
                                                                    Convert.ToInt32(
                                                                        dsTaskCrews.Tables[a].Rows
                                                                            [b][2].
                                                                            ToString()),
                                                                    Convert.ToDecimal(
                                                                        dsTaskCrews.Tables[a].Rows
                                                                            [b][4].
                                                                            ToString()),
                                                                    Convert.ToDecimal(
                                                                        dsTaskCrews.Tables[a].Rows
                                                                            [b][3].
                                                                            ToString()),
                                                                    0,
                                                                    0,
                                                                    _nullDate,
                                                                    _oLogon.UserID,
                                                                    -1,
                                                                    -1))
                                                                {
                                                                    //Check For Parts Table Count
                                                                    if (dsTaskParts.Tables.Count > 0)
                                                                    {
                                                                        //Loop Through Task Parts And Create Step Parts
                                                                        for (var c = 0;
                                                                            c <
                                                                            dsTaskParts.Tables[a].Rows.Count;
                                                                            c++)
                                                                        {
                                                                            //Get Part Cost
                                                                            if (
                                                                                oMp.GetMasterpartListCost(
                                                                                    Convert.ToInt32(
                                                                                        dsTaskParts.Tables[a]
                                                                                            .Rows[c][7]
                                                                                            .ToString()),
                                                                                    ref partCost))
                                                                            {
                                                                                //Set Job Step Info
                                                                                oJobParts.SetJobstepInfo(jobId,
                                                                                    jobstepId);

                                                                                //Add Job Part
                                                                                if (
                                                                                    !oJobParts.Add(
                                                                                        Convert.ToInt32(
                                                                                            dsTaskParts.Tables[a
                                                                                                ].Rows[c
                                                                                                ][7].ToString()),
                                                                                        Convert.ToInt32(
                                                                                            dsTaskParts.Tables[a
                                                                                                ].Rows[c
                                                                                                ][8].ToString()),
                                                                                        false,
                                                                                        false,
                                                                                        "",
                                                                                        dsTaskParts.Tables[a]
                                                                                            .Rows[c][3]
                                                                                            .ToString(),
                                                                                        partCost,
                                                                                        "",
                                                                                        dsTaskParts.Tables[a]
                                                                                            .Rows[c][4]
                                                                                            .ToString(),
                                                                                        Convert.ToDecimal(
                                                                                            dsTaskParts.Tables[a
                                                                                                ].Rows[c
                                                                                                ][9].ToString()),
                                                                                        0,
                                                                                        _nullDate,
                                                                                        -1,
                                                                                        -1,
                                                                                        _oLogon.UserID,
                                                                                        -1,
                                                                                        -1))
                                                                                {
                                                                                    //Error Hit
                                                                                    //Set Flag
                                                                                    success = false;

                                                                                    //Set Error Information
                                                                                    errorInfo =
                                                                                        "Error Adding PM Job Part - " +
                                                                                        oJobParts.LastError;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                //Error Hit
                                                                                //Set Flag
                                                                                success = false;

                                                                                //Set Error Information
                                                                                errorInfo =
                                                                                    "Error Getting PM Job Part Cost - " +
                                                                                    oMp.LastError;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //Error Hit
                                                                    //Set Flag
                                                                    success = false;

                                                                    //Set Error Information
                                                                    errorInfo = "Error Adding PM Job Crew - " +
                                                                                oJobCrew.LastError;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //Error Hit
                                                    //Set Flag
                                                    success = false;

                                                    //Show Error
                                                    errorInfo =
                                                        @"Error Saving Costing Information - " +
                                                        oJobStep.LastError;
                                                }
                                            }
                                            else
                                            {
                                                //Error Hit
                                                //Set Flag
                                                success = false;

                                                //Set Error Information
                                                errorInfo = "Error Adding PM Job Step - " + oJobStep.LastError;
                                            }
                                        }

                                        //Check Flag
                                        if (success)
                                        {
                                            //Check For Process Table Count
                                            if (dsTaskProcesses.Tables.Count > 0)
                                            {
                                                //Check Row Count
                                                if (dsTaskProcesses.Tables[0].Rows.Count > 0)
                                                {
                                                    //Loop Through Task Processes And Create Jop Processes
                                                    for (var b = 0;
                                                        b < dsTaskProcesses.Tables[0].Rows.Count;
                                                        b++)
                                                    {
                                                        //Add Process
                                                        if (!oJobProcesses.Add(jobId,
                                                            Convert.ToInt32(dsTaskProcesses.Tables[0]
                                                                .Rows[b]["TaskProcessId"]
                                                                .ToString()),
                                                            dsTaskProcesses.Tables[0]
                                                                .Rows[b]["TaskProcess"]
                                                                .ToString(),
                                                            dsTaskProcesses.Tables[0]
                                                                .Rows[b]["Description"]
                                                                .ToString(),
                                                            _nullDate,
                                                            0,
                                                            -1,
                                                            _oLogon.UserID))
                                                        {
                                                            //Error Hit
                                                            //Set Flag
                                                            success = false;

                                                            //Set Error Information
                                                            errorInfo =
                                                                "Error Adding Routine Job Process - " +
                                                                oAttachments.LastError;
                                                        }
                                                    }
                                                }
                                            }

                                            //Check For Member Table Count
                                            if (dsJobMembers.Tables.Count > 0)
                                            {
                                                //Check Row Count
                                                if (dsJobMembers.Tables[0].Rows.Count > 0)
                                                {
                                                    //Loop Through Job Members & Add Them
                                                    for (var b = 0;
                                                        b < dsJobMembers.Tables[0].Rows.Count;
                                                        b++)
                                                    {
                                                        //Add Job Member
                                                        if (!oJobMembers.Add(jobId,
                                                            -1,
                                                            Convert.ToInt32(dsJobMembers.Tables[0].Rows[b]["n_memberobjectid"].ToString()),
                                                            true,
                                                            _nullDate,
                                                            _oLogon.UserID))
                                                        {
                                                            //Error Hit
                                                            //Set Flag
                                                            success = false;

                                                            //Set Error Information
                                                            errorInfo = "Error Adding PM Job Member - " + oJobMembers.LastError;
                                                        }
                                                    }
                                                }
                                            }

                                            //Check For Attachment Table Count
                                            if (dsTaskAttachments.Tables.Count > 0)
                                            {
                                                //Check Row Count
                                                if (dsTaskAttachments.Tables[0].Rows.Count > 0)
                                                {
                                                    //Create System Setting Class
                                                    using (
                                                        var oSettings = new MpetSystemSettings(
                                                            _connectionString, _useWeb))
                                                    {
                                                        //Create Directory Variable
                                                        var dirToUse = "";

                                                        //Get Attachement Directory
                                                        if (!oSettings.GetAttachmentDirectory(ref dirToUse))
                                                        {
                                                            //Set Flag
                                                            success = false;

                                                            //Set Error Information
                                                            errorInfo =
                                                                "Error Getting Default Attachment Directory - " +
                                                                oSettings.LastError;
                                                        }

                                                        //Check Value
                                                        if (dirToUse.Trim() == "")
                                                        {
                                                            //Set Flag
                                                            success = false;

                                                            //Set Error Information
                                                            errorInfo = "Attachment Directory Not Set";
                                                        }

                                                        #region Create Temp Variables

                                                        var fn = "";

                                                        const bool continueProcessing = true;

                                                        #endregion

                                                        //Loop Through Task Attachments And Create Step Attachments
                                                        for (var b = 0;
                                                            b < dsTaskAttachments.Tables[0].Rows.Count;
                                                            b++)
                                                        {
                                                            //Check Flag
                                                            if (continueProcessing)
                                                            {
                                                                //Reset Processing Error
                                                                const bool processingError = false;

                                                                //Check For Processing Erorrs
                                                                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                                                                if (!processingError)
                                                                    // ReSharper restore ConditionIsAlwaysTrueOrFalse
                                                                {

                                                                    //Check For Azure Hosted Attachments
                                                                    string fileName = "";

                                                                    if (_oLogon.AzureHostedAttachments)
                                                                    {
                                                                        //Get Full File Name
                                                                        fileName =
                                                                            dsTaskAttachments.Tables[0]
                                                                                .Rows[b][4].ToString();

                                                                        // Get Storage Account From Connection String
                                                                        var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount
                                                                            .Parse(
                                                                                CloudConfigurationManager
                                                                                    .GetSetting(
                                                                                        "StorageConnectionString"));

                                                                        // Create Blob Client
                                                                        var blobClient =
                                                                            storageAccount
                                                                                .CreateCloudBlobClient();

                                                                        //Get Attachment Blob Reference
                                                                        var container =
                                                                            blobClient.GetContainerReference
                                                                                (
                                                                                    "attachments" +
                                                                                    @"/Work Order Attachments/Un-Issued Jobs/" +
                                                                                    jobId + @"/");

                                                                        //Check Name
                                                                        fn =
                                                                            fileName.Substring(
                                                                                fileName.LastIndexOf(@"/", StringComparison.Ordinal) +
                                                                                1);

                                                                        // Retrieve Refrence To Blob
                                                                        CloudBlockBlob blockBlob =
                                                                            container.GetBlockBlobReference(
                                                                                fn);

                                                                        //Check For Duplicate File
                                                                        if (!blockBlob.Exists())
                                                                        {
                                                                            try
                                                                            {
                                                                                var httpRequest =
                                                                                    (HttpWebRequest)
                                                                                        WebRequest.Create(
                                                                                            fileName);

                                                                                //Set Method
                                                                                httpRequest.Method = "GET";

                                                                                // if the URI doesn't exist, an exception will be thrown here...
                                                                                using (
                                                                                    var httpResponse =
                                                                                        (HttpWebResponse)
                                                                                            httpRequest
                                                                                                .GetResponse
                                                                                                ())
                                                                                {
                                                                                    using (
                                                                                        var responseStream =
                                                                                            httpResponse
                                                                                                .GetResponseStream
                                                                                                ())
                                                                                    {
                                                                                        //Upload File
                                                                                        blockBlob
                                                                                            .UploadFromStream
                                                                                            (responseStream);
                                                                                    }
                                                                                }

                                                                                //Get File Path
                                                                                fileName =
                                                                                    blockBlob.Uri.ToString();
                                                                            }
                                                                                // ReSharper disable once EmptyGeneralCatchClause
                                                                            catch
                                                                            {
                                                                                //Do Nothing -> File No Longer Exists
                                                                            }
                                                                        }
                                                                    }

                                                                    //Check Processing Error
                                                                    if (!processingError)
                                                                    {
                                                                        //Check Flag
                                                                        if (!processingError)
                                                                        {
                                                                            //Clear Errors
                                                                            oAttachments.ClearErrors();

                                                                            //Create Max Length Variable
                                                                            var maxLen = 20;

                                                                            //Check To See If FN Is Shorter
                                                                            if (maxLen > fn.Length)
                                                                            {
                                                                                //Change Max Length
                                                                                maxLen = fn.Length;
                                                                            }

                                                                            //Make Sure Short Name Version Of FN Is Only 20 Characters
                                                                            fn = fn.Substring(0, maxLen);

                                                                            //Add Attachment
                                                                            if (!oAttachments.Add(jobId,
                                                                                jobstepId,
                                                                                _oLogon.UserID,
                                                                                fileName.Trim(),
                                                                                dsTaskAttachments.Tables[0]
                                                                                    .Rows[b][2]
                                                                                    .ToString(),
                                                                                dsTaskAttachments.Tables[0]
                                                                                    .Rows[b][3]
                                                                                    .ToString(),
                                                                                dsTaskAttachments.Tables[0]
                                                                                    .Rows[b][5]
                                                                                    .ToString()))
                                                                            {
                                                                                //Error Hit
                                                                                //Set Flag
                                                                                success = false;

                                                                                //Set Error Information
                                                                                errorInfo =
                                                                                    "Error Adding Routine Job Attachment - " +
                                                                                    oAttachments
                                                                                        .LastError;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Error Hit
                                        //Set Flag
                                        success = false;

                                        //Show Error
                                        errorInfo =
                                            @"Error Saving Costing Information - " + _oJob.LastError;
                                    }
                                }
                                else
                                {
                                    //Error Hit
                                    //Set Flag
                                    success = false;

                                    //Set Error Information
                                    errorInfo = "Error Adding PM Job - " + _oJob.LastError;
                                }
                            }
                            else
                            {
                                //Error Hit
                                //Set Flag
                                success = false;

                                //Set Error Information
                                errorInfo = "Error Determining Job Pool Type - " + oJobIdGenerator.LastError;
                            }
                        }

                        #endregion

                        #region Update Task Information And Show Job

                        //Check For Success
                        if (success)
                        {
                            //Update Object Task Last DT Scheduled
                            if (!oObjTasks.UpdateDateLastDone(editingObjTaskId, dtToSchedule, _oLogon.UserID))
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Updating Object Task Last Date Scheduled - " +
                                    oObjTasks.LastError);
                            }

                            //Forward User To Copied Work Order
                            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + jobstepId, true);
                        }
                        else
                        {
                            //Throw Error
                            throw new SystemException(
                                @"Error Forcing PM - " + errorInfo);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }
    }
}