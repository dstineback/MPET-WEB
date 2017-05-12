using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using DevExpress.Web;
using MPETDSFactory;
using Page = System.Web.UI.Page;

namespace Pages.PlannedJobs
{
    public partial class PlannedJobs : Page
    {
        private const int _navigationIndex = 0;
        private WorkOrder _oJob;
        private WorkOrderJobStep _oJobStep;
        private JobstepJobParts _oJobParts;
        private JobstepJobCrew _oJobCrew;
        private JobOther _oJobOther;
        private JobEquipment _oJobEquipment;
        private JobMembers _oJobMembers;
        private LogonObject _oLogon;
        private JobIdGenerator _oJobIDGenerator;
        private AttachmentObject _oAttachments;
        private MpetUserDbClass _oMpetUser;
        private MaintObjectRunUnit _oRunUnit;
        public string AssignedGuid = "";
        public string AssignedJobID = "";
        private bool _userCanDelete;
        private bool _userCanAdd;
        private bool _userCanEdit;
        private bool _userCanView;
        private bool _userCanPost;
        private bool _jobIsHistory;
        private const int AssignedFormID = 55;
        private readonly DateTime _nullDate = Convert.ToDateTime("1/1/1960 23:59:59");
        private object newOtherRecordId;
        PlannedJobOtherTemplate otherEditTemplate = new PlannedJobOtherTemplate();
        private string _connectionString = "";
        private bool _useWeb;
        private const int EditingTimeBachId = -1;
        private const int EditingTiemBatchItemId = -1;
        private int activeTab = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.RegisterBaseScript(Page);

            #region Attempt To Load Logon Info

            //Check For Logon Class
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //Add Use ID TO Session
                HttpContext.Current.Session.Add("UserID", _oLogon.UserID);

                var jobStepIdToLoad = Convert.ToInt32(Request.QueryString["n_jobstepid"]);
                if (jobStepIdToLoad > 0)
                {
                    HttpContext.Current.Session.Add("editingJobStepID", jobStepIdToLoad);
                }

                //Load Form Permissions
                if (FormSetup(_oLogon.UserID))
                {
                    //Setup Buttons
                }

                //Enable/Disable DOT Specific Tab
                requestTab.TabPages[2].ClientVisible = _oLogon.IsDotIndustry;
                Master.ShowLocationButton = true;

                //Enable/Disable Facility Specific Tab
                requestTab.TabPages[4].ClientVisible = _oLogon.IsFacIndustry;

                //Show Job Step Specific Buttons
                //Master.ShowPostButton = true;
                //Master.ShowUnPostButton = true;
                //Master.EnableUnPostButton = false;
                //Master.ShowIssueButton = _userCanEdit;
                //Master.ShowSetDefaultsButton = _userCanEdit;
                //Master.ShowSetStartDateButton = _userCanEdit;
                //Master.ShowSetEndDateButton = _userCanEdit;
                Master.ShowAddCrewGroupButton = _userCanEdit;
                Master.ShowAddCrewLaborButton = _userCanEdit;
            }

            #endregion

            #region Attempt To Load Azure Details

            //Check For Null Azure Account
            if (!string.IsNullOrEmpty(AzureAccount))
            {
                UploadControl.AzureSettings.StorageAccountName = AzureAccount;
            }

            //Check For Null Access Key
            if (!string.IsNullOrEmpty(AzureAccessKey))
            {
                UploadControl.AzureSettings.AccessKey = AzureAccessKey;
            }

            //Check For Null Container
            if (!string.IsNullOrEmpty(AzureContainer))
            {
                UploadControl.AzureSettings.ContainerName = AzureContainer;
            }

            #endregion

            #region Check For Post To Setup Form
            //Check For Post To Setup Form
            if (!IsPostBack)
            {
                #region Set up for Editing or Adding
                //Check For Session Variable To Distinguish Previous Edit
                if (HttpContext.Current.Session["editingJobStepID"] != null)
                {
                    //Reset Session
                    ResetSession();

                    //Setup For Editing -> Checks Later For Viewing Only 
                    SetupForEditing();

                    if (HttpContext.Current.Session["activeTab"] != null)
                    {
                        StepTab.ActiveTabIndex = Convert.ToInt32(HttpContext.Current.Session["activeTab"].ToString());
                    } else
                    {
                        StepTab.ActiveTabIndex = 0;
                    }
                }
                else
                {
                    //Setup For Adding
                    SetupForAdding();
                    StepTab.ActiveTabIndex = 0;

                    //Check Tab
                    if (requestTab.ActiveTabIndex == 1)
                    {
                        //Set Focus
                        txtWorkDescription.Focus();
                        StepTab.ActiveTabIndex = 0;
                    }
                }
                #endregion

                #region Setup Navitation Checkboxes

                //Check For Navigation Items
                if (chkUpdateObjects.Items.Count > 0)
                {
                    //Clear Items
                    chkUpdateObjects.Items.Clear();
                }

                //Add Items
                chkUpdateObjects.Items.Add("Warranty Exp.", "1");
                chkUpdateObjects.Items.Add("Life Cycle", "2");
                chkUpdateObjects.Items.Add("In-Service", "3");

                #endregion
            }
            else
            {
                if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
                {

                }

                //Get Control That Caused Post Back
                var controlName = this.Request.Params.Get("__EVENTTARGET");

                #region Set up Footer Buttons
                //Check For Null
                if (!string.IsNullOrEmpty(controlName))
                {
                    //Determing What To Do
                    switch (controlName.Replace("ctl00$Footer$", ""))
                    {
                        case "NewButton":
                            {
                                //Call Add Routine
                                AddItems();
                                break;
                            }
                        case "EditButton":
                            {
                                //Call Edit Routine
                                EditItems();
                                break;
                            }
                        case "DeleteButton":
                            {
                                //Call Delete Routine
                                DeleteItems();
                                break;
                            }
                        case "SaveButton":
                            {
                                var jobStepIdToLoad = -1;
                                var editJobStepID = -1;
                                var editJobID = -1;

                                //Check For Job ID                           
                                jobStepIdToLoad = Convert.ToInt32(Request.QueryString["n_jobstepid"]);
                                if (jobStepIdToLoad > 0)
                                {
                                    HttpContext.Current.Session.Add("editingJobStepID", jobStepIdToLoad);
                                }

                                if (HttpContext.Current.Session["editingJobStepID"] != null)
                                {
                                    editJobStepID = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                                }
                                if (HttpContext.Current.Session["editingJobID"] != null)
                                {
                                    editJobID = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                                }

                                if (editJobID > 0 && editJobStepID > 0)
                                {
                                    //Save Session Data
                                    SaveSessionData();
                                    UpdateRoutine();
                                }
                                else
                                {
                                    if (editJobID < 1)
                                    {
                                        SaveSessionData();
                                        AddRequest();
                                        PlanJobRoutine();

                                        HttpContext.Current.Session.Add("editingJobStepID", _oJobStep.RecordID);

                                        editJobStepID = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                                        if (editJobStepID > 0)
                                        {
                                            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()), true);
                                        }
                                    }
                                    else
                                    {
                                        if (editJobStepID > 0)
                                        {
                                            SaveSessionData();
                                            UpdateRoutine();
                                        } else
                                        {
                                            SaveSessionData();
                                            AddJobStep();
                                            HttpContext.Current.Session.Add("editingJobStepID", _oJobStep.RecordID);
                                            UpdateRoutine();
                                        }
                                    }

                                }
                                break;
                            }
                        case "PrintButton":
                            {
                                //Call Print Routine
                                PrintSelectedRow();
                                break;
                            }
                        case "MultiSelect":
                            {
                                //Determine Grid
                                switch (StepTab.ActiveTabIndex)
                                {
                                    case 0:
                                        {
                                            //Steps
                                            break;
                                        }
                                    case 1:
                                        {
                                            //Enable/Disable Member MultiSelect
                                            EnableMemberMultiSelect(!((MemberGrid.Columns[0] as GridViewColumn).Visible));
                                            break;
                                        }
                                    case 2:
                                        {
                                            //Enable/Disable Crew MultiSelect
                                            EnableCrewMultiSelect(!((CrewGrid.Columns[0] as GridViewColumn).Visible));
                                            break;
                                        }
                                    case 3:
                                        {
                                            //Enable/Disable Part MultiSelect
                                            EnablePartMultiSelect(!((PartGrid.Columns[0] as GridViewColumn).Visible));
                                            break;
                                        }
                                    case 4:
                                        {
                                            //Enable/Disable Part MultiSelect
                                            EnableEquipMultiSelect(!((EquipGrid.Columns[0] as GridViewColumn).Visible));
                                            break;
                                        }
                                    case 5:
                                        {
                                            //Enable/Disable Other MultiSelect
                                            EnableOtherMultiSelect(!((OtherGrid.Columns[0] as GridViewColumn).Visible));
                                            break;
                                        }
                                    default:
                                        {
                                            //Do Nothing
                                            break;
                                        }
                                }

                                //Break
                                break;
                            }
                        case "SetToStartDate":
                            {
                                SetMembersToStartDate();
                                break;
                            }
                        case "SetToEndDate":
                            {
                                SetMembersToEndDate();
                                break;
                            }
                        case "CopyJob":
                            {
                                //Call Copy Routine
                                CopyJobRoutine();

                                //break
                                break;
                            }
                        case "AddCrewByLabor":
                            {
                                //Bind Grid
                                CrewLaborGridLookup.DataBind();

                                //Show Popup
                                CrewLaborClassPopup.ShowOnPageLoad = true;

                                //break
                                break;
                            }
                        case "AdCrewByGroup":
                            {
                                //Bind Grid
                                CrewGroupGridLookup.DataBind();

                                //Show Popup
                                CrewGroupPopup.ShowOnPageLoad = true;

                                //break
                                break;
                            }
                        case "IssueJob":
                            {
                                //Call Issue Routine
                                IssueRoutine();

                                //break
                                break;
                            }
                        case "NewNonStockPart":
                            {
                                //Call NS Part Routine
                                NonStockPartRoutine();

                                //break
                                break;
                            }
                        case "PreviousStep":
                            {
                                //Check For Previous Step
                                if (HttpContext.Current.Session["PreviousStep"] != null)
                                {
                                    //Reset Variables
                                    ResetSession();

                                    //Go To Previous Step
                                    Response.Redirect(
                                        "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" +
                                        Convert.ToInt32(HttpContext.Current.Session["PreviousStep"].ToString()), true);
                                }

                                //break
                                break;
                            }
                        case "NextStep":
                            {
                                //Check For Next Step
                                if (HttpContext.Current.Session["NextStep"] != null)
                                {
                                    //Reset Variables
                                    ResetSession();

                                    //Go To Next Step
                                    Response.Redirect(
                                        "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" +
                                        Convert.ToInt32(HttpContext.Current.Session["NextStep"].ToString()), true);
                                }

                                //break
                                break;
                            }
                        case "Post":
                            {
                                //Clear Fields
                                
                                jobPostDate.Value = null;
                                ComboOutcomeCode.Value = null;
                                chkPostDefaults.Checked = false;

                                //Show Popup
                                PostPopup.ShowOnPageLoad = true;

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
                    AddCrewPopup.ShowOnPageLoad = false;
                    AddEquipPopup.ShowOnPageLoad = false;
                    AddMemberPopup.ShowOnPageLoad = false;
                    CrewLaborClassPopup.ShowOnPageLoad = false;
                    CrewGroupPopup.ShowOnPageLoad = false;
                    AddPartPopup.ShowOnPageLoad = false;
                    PostPopup.ShowOnPageLoad = false;

                }
                #endregion
            }
            #endregion

            #region Setup Fields on page load
            //Check For Query String
            if (!String.IsNullOrEmpty(Request.QueryString["n_jobstepid"]))
            {
                //Setup For Editing -> Checks Later For Viewing Only 
                SetupForEditing();

                //Check For Editing Job ID
                if (HttpContext.Current.Session["editingJobStepID"] == null)
                {
                    //Check For Editing Job ID
                    if (HttpContext.Current.Session["editingJobID"] == null)
                    {
                        //Get Step ID
                        var jobStepIdToLoad = Convert.ToInt32(Request.QueryString["n_jobstepid"]);

                        //Get Logon
                        _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                        //Clear Errors
                        _oJobStep.ClearErrors();

                        //Load Job Step Data
                        if (_oJobStep.LoadJobStepData(jobStepIdToLoad, _oLogon.UserID))
                        {
                            //Check Table Count 
                            if (_oJobStep.Ds.Tables.Count > 0)
                            {
                                //Check Table Row Count
                                if (_oJobStep.Ds.Tables[0].Rows.Count > 0)
                                {
                                    //Clear Error
                                    _oJob.ClearErrors();

                                    //Set Job ID
                                    var nJobId = Convert.ToInt32(_oJobStep.Ds.Tables[0].Rows[0]["n_Jobid"]);

                                    //Load Job Data
                                    if (_oJob.LoadJobData(nJobId, _oLogon.UserID))
                                    {
                                        //Check Table Count
                                        if (_oJob.Ds.Tables.Count > 0)
                                        {
                                            //Check Row Count
                                            if (_oJob.Ds.Tables[0].Rows.Count > 0)
                                            {
                                                #region Expected Schema
                                                //Expected Job Schema
                                                //1	tbl_Jobs.n_Jobid,
                                                //2	tbl_Jobs.Jobid,
                                                //3	tbl_Jobs.Title,
                                                //4	tbl_Jobs.TypeOfJob,
                                                //5	tbl_Jobs.AssignedGUID,
                                                //6	tbl_Jobs.JobAgainstID,
                                                //7	tbl_Jobs.n_MaintObjectID,
                                                //8	tbl_Jobs_MO.objectid,
                                                //9	tbl_Jobs.n_jobreasonid as 'nJobReasonID',
                                                //10	tbl_Jobs_Reasons.JobReasonID,
                                                //11	tbl_Jobs.Notes,
                                                //12	tbl_Jobs.EstimatedJobHours,
                                                //13	tbl_Jobs.ActualJobHours,
                                                //14	tbl_Jobs.IsRequestOnly,
                                                //15	tbl_Jobs.n_RouteTo,
                                                //16	tbl_Jobs_RouteTo.Username as 'RouteToUsername',
                                                //17	tbl_Jobs.RequestDate,
                                                //18	tbl_Jobs.n_requestPriority as 'n_priorityid',
                                                //19	tbl_Jobs_Priorities.priorityid,
                                                //20	tbl_Jobs.n_requestor,
                                                //21	tbl_Jobs_Requestor.Username as 'RequestorUsername',
                                                //22	tbl_Jobs.pmcalc_startdate,
                                                //23	tbl_Jobs.n_MobileOwner,
                                                //24	tbl_Jobs.MobileDate,
                                                //25	tbl_Jobs.IssuedDate,
                                                //26	tbl_Jobs.GPS_X,
                                                //27	tbl_Jobs.GPS_Y,
                                                //28	tbl_Jobs.GPS_Z,
                                                //29	tbl_Jobs.n_group,
                                                //30	tbl_Jobs_Groups.groupid,
                                                //31	tbl_Jobs.SentToPDA,
                                                //32	tbl_Jobs.JobOpen,
                                                //33	tbl_Jobs.IsHistory,
                                                //34	tbl_Jobs.ServicingEquipment,
                                                //35	tbl_Jobs.completed_units1,
                                                //36	tbl_Jobs.completed_units2,
                                                //37	tbl_Jobs.completed_units3,
                                                //38	tbl_Jobs.n_OutcomeID,
                                                //39	tbl_Jobs_OutcomeCodes.outcomecodeid,
                                                //40	tbl_Jobs.n_OwnerID,
                                                //41	tbl_Jobs.n_TaskID,
                                                //42	tbl_Jobs_Tasks.taskid,
                                                //43	tbl_Jobs.n_workeventid,
                                                //44	tbl_Jobs_WorkEvents.workeventid,
                                                //45	tbl_Jobs.n_worktypeid as 'n_WorkOpID',
                                                //46	tbl_Jobs_WorkOps.WorkOpID,
                                                //47	tbl_Jobs.PostedDate,
                                                //48	tbl_Jobs.SubAssemblyID as 'SubAssemblyID',
                                                //49	tbl_Jobs_SubAssy.SubAssemblyName,
                                                //50	tbl_Jobs.n_StateRouteID,
                                                //51	tbl_Jobs_StateRoutes.StateRouteID,
                                                //52	tbl_Jobs.Milepost,
                                                //53	tbl_Jobs.IncreasingMP as 'n_MilePostDirectionID',
                                                //54	tbl_Jobs.EstimatedUnits,
                                                //55	tbl_Jobs.ActualUnits,
                                                //56	tbl_Jobs.b_AdditionalDamage,
                                                //57	tbl_Jobs.PercentOverage,
                                                //58	tbl_Jobs_Tasks.ProceduresText,
                                                //59	tbl_Jobs_PostedBy.Username as 'JobPoster',
                                                //60	@CurrentUnits1 as 'CurrentUnits1',
                                                //61	@CurrentUnits2 as 'CurrentUnits2',
                                                //62	@CurrentUnits3 as 'CurrentUnits3',
                                                //63	tbl_Jobs_UOM.UnitsOfMeasure,
                                                //64	tbl_Jobs_MilePostDir.MilePostDirectionID
                                                //65    tbl_Jobs.n_OwningGroupID,
                                                //66    tbl_OwningGroup.groupid as 'OwningGroupID'
                                                //67    'OwningGroupUser'  
                                                //68    tbl_Jobs.MilepostTo
                                                //69    MemberCount

                                                //Expected Job Step Schema
                                                //1	tbl_JS.n_Jobid,
                                                //2	tbl_JS.n_jobstepid,
                                                //3	tbl_JS.stepnumber,
                                                //4	tbl_JS.concurwithstep,
                                                //5	tbl_JS.followstep,
                                                //6	tbl_JS.JobstepTitle,
                                                //7	tbl_JS.n_statusid as 'nStatusID',
                                                //8	tbl_JS_Statuses.StatusID,
                                                //9	tbl_JS.n_laborclassid,
                                                //10	tbl_JS_LC.laborclassid,
                                                //11	tbl_JS.CompletionNotes,
                                                //12	tbl_JS.JobstepNotes,
                                                //13	tbl_JS.n_ServicingEquipment,
                                                //14	tbl_JS.n_groupid,
                                                //15	tbl_JS_Groups.groupid,
                                                //16	tbl_JS.n_OutcomeCode as 'n_outcomecodeid',
                                                //17	tbl_JS_OC.outcomecodeid,
                                                //18	tbl_JS.n_shiftID,
                                                //19	tbl_JS_Shifts.ShiftID,
                                                //20	tbl_JS.n_supervisorid as 'UserID',
                                                //21	tbl_JS_Supervisors.Username,
                                                //22	tbl_JS.actualdowntime,
                                                //23	tbl_JS.actuallength,
                                                //24	tbl_JS.estimateddowntime,
                                                //25	tbl_JS.estimatedlength,
                                                //26	tbl_JS.OriginalStartingDate,
                                                //27	tbl_JS.StartingDate,
                                                //28	tbl_JS.DateTimeCompleted,
                                                //29	tbl_JS.RemainingLength,
                                                //30	tbl_JS.RemainingDowntime,
                                                //31	tbl_JS.return_within,
                                                //32	tbl_JS.n_FundSrcCodeID,
                                                //33	tbl_JS_FSC.FundSrcCodeID,
                                                //34	tbl_JS.SubAssemblyID,
                                                //35	tbl_JS_SubAss.SubAssemblyName,
                                                //36	tbl_JS.PriorityID as 'n_priorityid',
                                                //37	tbl_JS_Priority.priorityid,
                                                //38	tbl_JS.ReasoncodeID as 'nJobReasonID',
                                                //39	tbl_JS_Reasons.JobReasonID,
                                                //40	tbl_JS.PostNotes,
                                                //41	tbl_JS.Chargeto,
                                                //42	tbl_JS.n_IncidentLogID as 'RecordID',
                                                //43	tbl_JS_Incidents.IncidentNum,
                                                //44	tbl_JS.n_WorkOrderCodeID,
                                                //45	tbl_JS_WOCodes.WorkOrderCodeID,
                                                //46	tbl_JS.n_WorkOpID,
                                                //47	tbl_JS_WorkOp.WorkOpID,
                                                //48	tbl_JS.n_OrganizationCodeID,
                                                //49	tbl_JS_OrgCodes.OrganizationCodeID,
                                                //50	tbl_JS.n_FundingGroupCodeID,
                                                //51	tbl_JS_FundGroup.FundingGroupCodeID,
                                                //52	tbl_JS.n_ControlSectionID,
                                                //53	tbl_JS_CtlSections.ControlSectionID,
                                                //54	tbl_JS.n_EquipmentNumberID,
                                                //55	tbl_JS_EquipNum.EquipmentNumberID,
                                                //56	tbl_JS.n_CostCodeID,
                                                //57	tbl_JS_CostCodes.costcodeid,
                                                //58	'CrewCount',
                                                //59	'PartCount',
                                                //60	'EquipCount',
                                                //61	'OtherCount',
                                                //62	'AttachCount',
                                                //63	tbl_JS.CompletedBy,
                                                //64	tbl_JS_CompletedBy.Username as 'CompletedByID'
                                                //65    tbl_JS_GroupSupervisor.UserID as 'n_SupervisorID',
                                                //66    tbl_JS_GroupSupervisor.Username as 'SupervisorID'
                                                //67    tbl_JS_UOM.UnitsOfMeasure,
                                                //68    isnull(tbl_IsFlaggedRecord.RecordID,-1) as 'FlaggedRecordID',
                                                //69    tbl_JS_CostCodes.SupplementalCode,
                                                //70    tbl_JS_OC.PostNotesRequired,
                                                //71    tbl_JS_OC.WorkNotDone,
                                                //72    tbl_JS.n_RouteToID AS 'n_RouteToID',
                                                //73    tbl_JS_RouteTo.Username AS 'RouteToID'
                                                //74    ISNULL(tbl_PreviousStep.n_jobstepid, -1) AS 'PreviousStep',
                                                //75    ISNULL(tblNextStep.n_jobstepid,-1) AS 'NextStep'

                                                #endregion

                                                #region Setup History Flag

                                                //Get History Flag
                                                _jobIsHistory = ((_oJob.Ds.Tables[0].Rows[0]["IsHistory"].ToString().ToUpper()) == "Y");

                                                #endregion

                                                #region Setup Job Data

                                                //Add Job ID Class
                                                HttpContext.Current.Session.Add("oJob", _oJob);

                                                //Add Editing Job ID
                                                HttpContext.Current.Session.Add("editingJobID",
                                                    ((int)_oJob.Ds.Tables[0].Rows[0]["n_Jobid"]));

                                                //Add Editing Job Step ID
                                                HttpContext.Current.Session.Add("editingJobStepID", jobStepIdToLoad);
                                                HttpContext.Current.Session.Add("editingJobStepID",
                                                    ((int)_oJobStep.Ds.Tables[0].Rows[0]["n_jobstepid"]));


                                                //Add Job Step 
                                                HttpContext.Current.Session.Add("editingJobStepNum",
                                                    ((int)_oJobStep.Ds.Tables[0].Rows[0]["stepnumber"]));

                                                //Check For Valid Previous Number
                                                if (((int)_oJobStep.Ds.Tables[0].Rows[0]["PreviousStep"]) > 0)
                                                {
                                                    //Add Job Step Previous ID
                                                    HttpContext.Current.Session.Add("PrevousStep",
                                                        ((int)_oJobStep.Ds.Tables[0].Rows[0]["PreviousStep"]));

                                                    //Enable Button
                                                    Master.ShowPrevStepButton = true;
                                                }
                                                else
                                                {
                                                    //Disable & Clear Nav
                                                    Master.ShowPrevStepButton = false;

                                                    //Check For Prior Value
                                                    if (HttpContext.Current.Session["PreviousStep"] != null)
                                                    {
                                                        //Remove Old One
                                                        HttpContext.Current.Session.Remove("PreviousStep");
                                                    }
                                                }

                                                //Check For Valid Next Number
                                                if (((int)_oJobStep.Ds.Tables[0].Rows[0]["NextStep"]) > 0)
                                                {
                                                    //Add Job Step Next ID
                                                    HttpContext.Current.Session.Add("NextStep",
                                                        ((int)_oJobStep.Ds.Tables[0].Rows[0]["NextStep"]));

                                                    //Enable Label
                                                    Master.ShowNextStepButton = true;
                                                }
                                                else
                                                {

                                                    //Disable & Clear Nav
                                                    Master.ShowNextStepButton = false;

                                                    //Check For Prior Value
                                                    if (HttpContext.Current.Session["NextStep"] != null)
                                                    {
                                                        //Remove Old One
                                                        HttpContext.Current.Session.Remove("NextStep");
                                                    }
                                                }

                                                //Add Job String ID
                                                HttpContext.Current.Session.Add("AssignedJobID",
                                                    _oJob.Ds.Tables[0].Rows[0]["Jobid"]);

                                                //Add Description

                                                if (jobStepIdToLoad > 0)
                                                {
                                                    HttpContext.Current.Session.Add("txtWorkDescription",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["JobStepTitle"]);
                                                } else
                                                {

                                                    HttpContext.Current.Session.Add("txtWorkDescription",
                                                        _oJob.Ds.Tables[0].Rows[0]["Title"]);
                                                }

                                                //Add Request Date
                                                HttpContext.Current.Session.Add("TxtWorkRequestDate",
                                                    _oJob.Ds.Tables[0].Rows[0]["RequestDate"]);

                                                //Add Start Date
                                                if (Convert.ToDateTime(_oJobStep.Ds.Tables[0].Rows[0]["StartingDate"]) !=
                                                    _nullDate)
                                                {
                                                    //Set Value
                                                    HttpContext.Current.Session.Add("TxtWorkStartDate",
                                                        _oJobStep.Ds.Tables[0].Rows[0]["StartingDate"]);
                                                }

                                                //Add Comp Date
                                                if (Convert.ToDateTime(_oJobStep.Ds.Tables[0].Rows[0]["DateTimeCompleted"]) !=
                                                    _nullDate)
                                                {
                                                    //Set Value
                                                    HttpContext.Current.Session.Add("TxtWorkCompDate",
                                                        _oJobStep.Ds.Tables[0].Rows[0]["DateTimeCompleted"]);
                                                }

                                                HttpContext.Current.Session.Add("ComboOutcome", _oJobStep.Ds.Tables[0].Rows[0]["n_outcomecodeid"]);
                                                HttpContext.Current.Session.Add("txtReturnWithin", _oJobStep.Ds.Tables[0].Rows[0]["return_within"]);
                                                HttpContext.Current.Session.Add("ComboCompletedBy", _oJobStep.Ds.Tables[0].Rows[0]["CompletedBy"]);

                                                //Add Step Number
                                                HttpContext.Current.Session.Add("stepnumber",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["stepnumber"]);

                                                //Add Concur Step Number
                                                HttpContext.Current.Session.Add("concurwithstep",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["concurwithstep"]);

                                                //Add Follow Step Number
                                                HttpContext.Current.Session.Add("followstep",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["followstep"]);

                                                #endregion

                                                #region Setup Object Info

                                                HttpContext.Current.Session.Add("ObjectIDCombo",
                                                    _oJob.Ds.Tables[0].Rows[0]["n_MaintObjectID"]);
                                                HttpContext.Current.Session.Add("ObjectIDComboText",
                                                    _oJob.Ds.Tables[0].Rows[0]["ObjectID"]);
                                                HttpContext.Current.Session.Add("txtObjectDescription",
                                                    _oJob.Ds.Tables[0].Rows[0]["ObjectDesc"]);
                                                HttpContext.Current.Session.Add("txtObjectArea",
                                                    _oJob.Ds.Tables[0].Rows[0]["ObjectArea"]);
                                                HttpContext.Current.Session.Add("txtObjectLocation",
                                                    _oJob.Ds.Tables[0].Rows[0]["ObjectLoc"]);
                                                HttpContext.Current.Session.Add("txtObjectAssetNumber",
                                                    _oJob.Ds.Tables[0].Rows[0]["ObjectAsset"]);

                                                #endregion

                                                #region Setup Priority

                                                HttpContext.Current.Session.Add("ComboPriority",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_priorityid"]);
                                                HttpContext.Current.Session.Add("ComboPriorityText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["priorityid"]);

                                                #endregion

                                                #region Setup Reason

                                                HttpContext.Current.Session.Add("comboReason",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["nJobReasonID"]);
                                                HttpContext.Current.Session.Add("comboReasonText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["JobReasonID"]);

                                                #endregion

                                                #region Setup Route To

                                                HttpContext.Current.Session.Add("comboRouteTo",
                                                    _oJob.Ds.Tables[0].Rows[0]["n_RouteTo"]);
                                                HttpContext.Current.Session.Add("comboRouteToText",
                                                    _oJob.Ds.Tables[0].Rows[0]["RouteToUsername"]);

                                                #endregion

                                                #region Setup Hwy Route

                                                HttpContext.Current.Session.Add("comboHwyRoute",
                                                    _oJob.Ds.Tables[0].Rows[0]["n_StateRouteID"]);
                                                HttpContext.Current.Session.Add("comboHwyRouteText",
                                                    _oJob.Ds.Tables[0].Rows[0]["StateRouteID"]);

                                                #endregion

                                                #region Setup Milepost

                                                HttpContext.Current.Session.Add("txtMilepost",
                                                    _oJob.Ds.Tables[0].Rows[0]["Milepost"]);
                                                HttpContext.Current.Session.Add("txtMilepostTo", _oJob.Ds.Tables[0].Rows[0]["MilepostTo"]);
                                                HttpContext.Current.Session.Add("comboMilePostDir",
                                                    _oJob.Ds.Tables[0].Rows[0]["n_MilePostDirectionID"]);
                                                HttpContext.Current.Session.Add("comboMilePostDirText",
                                                    _oJob.Ds.Tables[0].Rows[0]["MilePostDirectionID"]);

                                                #endregion

                                                #region Setup Cost Code

                                                HttpContext.Current.Session.Add("ComboCostCode",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_CostCodeID"]);
                                                HttpContext.Current.Session.Add("ComboCostCodeText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["costcodeid"]);

                                                #endregion

                                                #region Setup Fund Source

                                                HttpContext.Current.Session.Add("ComboFundSource",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_FundSrcCodeID"]);
                                                HttpContext.Current.Session.Add("ComboFundSourceText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["FundSrcCodeID"]);

                                                #endregion

                                                #region Setup Work Order

                                                HttpContext.Current.Session.Add("ComboWorkOrder",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_WorkOrderCodeID"]);
                                                HttpContext.Current.Session.Add("ComboWorkOrderText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["WorkOrderCodeID"]);

                                                #endregion

                                                #region Setup Work Op

                                                HttpContext.Current.Session.Add("ComboWorkOp",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_WorkOpID"]);
                                                HttpContext.Current.Session.Add("ComboWorkOpText",
                                                    _oJob.Ds.Tables[0].Rows[0]["WorkOpID"]);

                                                #endregion

                                                #region Setup Org Code

                                                HttpContext.Current.Session.Add("ComboOrgCode",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_OrganizationCodeID"]);
                                                HttpContext.Current.Session.Add("ComboOrgCodeText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["OrganizationCodeID"]);

                                                #endregion

                                                #region Setup Fund Group

                                                HttpContext.Current.Session.Add("ComboFundGroup",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_FundingGroupCodeID"]);
                                                HttpContext.Current.Session.Add("ComboFundGroupText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["FundingGroupCodeID"]);

                                                #endregion

                                                #region Setup Control Section

                                                HttpContext.Current.Session.Add("ComboCtlSection",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_ControlSectionID"]);
                                                HttpContext.Current.Session.Add("ComboCtlSectionText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["ControlSectionID"]);

                                                #endregion

                                                #region Setup Equip Num

                                                HttpContext.Current.Session.Add("ComboEquipNum",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["n_EquipmentNumberID"]);
                                                HttpContext.Current.Session.Add("ComboEquipNumText",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["EquipmentNumberID"]);

                                                #endregion

                                                #region Setup Run Units

                                                //Check History Bool
                                                if (!_jobIsHistory)
                                                {
                                                    //Setup Run Units
                                                    SetupRunUnits(
                                                        Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["n_MaintObjectID"]));

                                                    //Check Current Units 1
                                                    HttpContext.Current.Session.Add("txtRunUnitOne",
                                                        Convert.ToDecimal(
                                                            _oJob.Ds.Tables[0].Rows[0]["CurrentUnits1"].ToString()) > 0
                                                            ? _oJob.Ds.Tables[0].Rows[0]["CurrentUnits1"].ToString()
                                                            : "0.00");

                                                    //Check Current Units 2
                                                    HttpContext.Current.Session.Add("txtRunUnitTwo",
                                                        Convert.ToDecimal(
                                                            _oJob.Ds.Tables[0].Rows[0]["CurrentUnits2"].ToString()) > 0
                                                            ? _oJob.Ds.Tables[0].Rows[0]["CurrentUnits2"].ToString()
                                                            : "0.00");

                                                    //Check Current Units 3
                                                    HttpContext.Current.Session.Add("txtRunUnitThree",
                                                        Convert.ToDecimal(
                                                            _oJob.Ds.Tables[0].Rows[0]["CurrentUnits3"].ToString()) > 0
                                                            ? _oJob.Ds.Tables[0].Rows[0]["CurrentUnits3"].ToString()
                                                            : "0.00");
                                                }
                                                else
                                                {
                                                    //Get History Run Units

                                                    //Check For Run Unit 1
                                                    if (_oJob.Ds.Tables[0].Rows[0]["completed_units1"].ToString() != "")
                                                    {
                                                        //Set Value
                                                        HttpContext.Current.Session.Add("txtRunUnitOne",
                                                            _oJob.Ds.Tables[0].Rows[0]["completed_units1"].ToString());
                                                    }

                                                    //Check For Run Unit 2
                                                    if (_oJob.Ds.Tables[0].Rows[0]["completed_units2"].ToString() != "")
                                                    {
                                                        //Set Value
                                                        HttpContext.Current.Session.Add("txtRunUnitTwo",
                                                            _oJob.Ds.Tables[0].Rows[0]["completed_units2"].ToString());
                                                    }

                                                    //Check For Run Unit 3
                                                    if (_oJob.Ds.Tables[0].Rows[0]["completed_units3"].ToString() != "")
                                                    {
                                                        //Set Value
                                                        HttpContext.Current.Session.Add("txtRunUnitThree",
                                                            _oJob.Ds.Tables[0].Rows[0]["completed_units3"].ToString());
                                                    }
                                                }

                                                #endregion

                                                #region Facility Values
                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtFN"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtFN");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtLN"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtLN");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtEmail"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtEmail");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtPhone"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtPhone");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtExt"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtExt");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtMail"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtMail");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtBuilding"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtBuilding");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["txtRoomNum"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("txtRoomNum");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["ComboServiceOffice"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("ComboServiceOffice");
                                                }

                                                //Check For Prior Value
                                                if (HttpContext.Current.Session["ComboServiceOfficeText"] != null)
                                                {
                                                    //Remove Old One
                                                    HttpContext.Current.Session.Remove("ComboServiceOfficeText");
                                                }
                                                #endregion

                                                #region Setup Location

                                                //Check X
                                                if (Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["GPS_X"]) != 0)
                                                {
                                                    HttpContext.Current.Session.Add("GPSX",
                                                        _oJob.Ds.Tables[0].Rows[0]["GPS_X"]);
                                                }

                                                //Check Y
                                                if (Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["GPS_Y"]) != 0)
                                                {
                                                    HttpContext.Current.Session.Add("GPSY",
                                                        _oJob.Ds.Tables[0].Rows[0]["GPS_Y"]);
                                                }

                                                //Check Z
                                                if (Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["GPS_Z"]) != 0)
                                                {
                                                    HttpContext.Current.Session.Add("GPSZ",
                                                        _oJob.Ds.Tables[0].Rows[0]["GPS_Z"]);
                                                }


                                                #endregion

                                                #region Setup Additional Info

                                                HttpContext.Current.Session.Add("txtAddDetail",
                                                    _oJobStep.Ds.Tables[0].Rows[0]["JobstepNotes"]);

                                                HttpContext.Current.Session.Add("txtPostNotes",
                                                _oJobStep.Ds.Tables[0].Rows[0]["PostNotes"]);

                                                #endregion

                                                //Set Tab Counts

                                                //Refresh Crew
                                                CrewGrid.DataBind();

                                                //Refresh Attachments
                                                AttachmentGrid.DataBind();

                                                //Refresh Members
                                                MemberGrid.DataBind();

                                                //Refresh Other
                                                OtherGrid.DataBind();

                                                //Refresh Equipment
                                                EquipGrid.DataBind();

                                                //Refresh Parts Grid
                                                PartGrid.DataBind();
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
                    //Check For Photo
                    if (HttpContext.Current.Session["ObjectPhoto"] != null)
                    {
                        //Set Image
                        objectImg.ImageUrl = HttpContext.Current.Session["ObjectPhoto"].ToString();
                    }
                }
            }
            #endregion

            //Setup User Defined Fields
            //SetupUserDefinedFields();
            #region Is not a Post-Back
            if (!IsPostBack)
            {
                //Check For Previous Step
                if (HttpContext.Current.Session["PreviousStep"] != null)
                {
                    //Show Button
                    Master.ShowPrevStepButton = true;
                }


                //Check For Next Step
                if (HttpContext.Current.Session["NextStep"] != null)
                {
                    //Show Button
                    Master.ShowNextStepButton = true;
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtWorkDescription"] != null)
                {
                    //Get Additional Info From Session
                    txtWorkDescription.Text = (HttpContext.Current.Session["txtWorkDescription"].ToString());
                }

                //Job ID
                if (HttpContext.Current.Session["editingJobStepNum"] != null)
                {
                    //Get Additional Info From Session
                    lblStep.Text = @"STEP #" + (HttpContext.Current.Session["editingJobStepID"]);
                }

                //Step Number
                if (HttpContext.Current.Session["AssignedJobID"] != null)
                {
                    //Get Additional Info From Session
                    lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());
                }

                if (HttpContext.Current.Session["ComboCompletedBy"] != null)
                {
                    ComboCompletedBy.Value = HttpContext.Current.Session["ComboCompletedBy"].ToString();
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["ComboOutcome"] != null)

                {
                    //Get Info From Session
                    ComboOutcome.Value = (HttpContext.Current.Session["ComboOutcome"].ToString());

                }

                if (HttpContext.Current.Session["txtReturnWithin"] != null)
                {
                    txtReturnWithin.Value = Convert.ToInt32(HttpContext.Current.Session["txtReturnWithin"].ToString());
                }

                if (HttpContext.Current.Session["TxtWorkStartDate"] != null)
                {
                    TxtWorkStartDate.Value = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkStartDate"].ToString());
                }

                if (HttpContext.Current.Session["TxtWorkCompDate"] != null)
                {
                    TxtWorkCompDate.Value = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkCompDate"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboCostCode"] != null) &&
                    (HttpContext.Current.Session["ComboCostCodeText"] != null))
                {
                    //Get Info From Session
                    ComboCostCode.Value = Convert.ToInt32((HttpContext.Current.Session["ComboCostCode"].ToString()));
                    ComboCostCode.Text = (HttpContext.Current.Session["ComboCostCodeText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboFundSource"] != null) &&
                    (HttpContext.Current.Session["ComboFundSourceText"] != null))
                {
                    //Get Info From Session
                    ComboFundSource.Value = Convert.ToInt32((HttpContext.Current.Session["ComboFundSource"].ToString()));
                    ComboFundSource.Text = (HttpContext.Current.Session["ComboFundSourceText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboWorkOrder"] != null) &&
                    (HttpContext.Current.Session["ComboWorkOrderText"] != null))
                {
                    //Get Info From Session
                    ComboWorkOrder.Value = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOrder"].ToString()));
                    ComboWorkOrder.Text = (HttpContext.Current.Session["ComboWorkOrderText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboWorkOp"] != null) &&
                    (HttpContext.Current.Session["ComboWorkOpText"] != null))
                {
                    //Get Info From Session
                    ComboWorkOp.Value = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
                    ComboWorkOp.Text = (HttpContext.Current.Session["ComboWorkOpText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboOrgCode"] != null) &&
                    (HttpContext.Current.Session["ComboOrgCodeText"] != null))
                {
                    //Get Info From Session
                    ComboOrgCode.Value = Convert.ToInt32((HttpContext.Current.Session["ComboOrgCode"].ToString()));
                    ComboOrgCode.Text = (HttpContext.Current.Session["ComboOrgCodeText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboFundGroup"] != null) &&
                    (HttpContext.Current.Session["ComboFundGroupText"] != null))
                {
                    //Get Info From Session
                    ComboFundGroup.Value = Convert.ToInt32((HttpContext.Current.Session["ComboFundGroup"].ToString()));
                    ComboFundGroup.Text = (HttpContext.Current.Session["ComboFundGroupText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboCtlSection"] != null) &&
                    (HttpContext.Current.Session["ComboCtlSectionText"] != null))
                {
                    //Get Info From Session
                    ComboCtlSection.Value = Convert.ToInt32((HttpContext.Current.Session["ComboCtlSection"].ToString()));
                    ComboCtlSection.Text = (HttpContext.Current.Session["ComboCtlSectionText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboEquipNum"] != null) &&
                    (HttpContext.Current.Session["ComboEquipNumText"] != null))
                {
                    //Get Info From Session
                    ComboEquipNum.Value = Convert.ToInt32((HttpContext.Current.Session["ComboEquipNum"].ToString()));
                    ComboEquipNum.Text = (HttpContext.Current.Session["ComboEquipNumText"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtAddDetail"] != null)
                {
                    //Get Additional Info From Session
                    txtAdditionalInfo.Text = (HttpContext.Current.Session["txtAddDetail"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtPostNotes"] != null)
                {
                    //Get Additional Info From Session
                    txtPostNotes.Text = (HttpContext.Current.Session["txtPostNotes"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboServiceOffice"] != null) &&
                    (HttpContext.Current.Session["ComboServiceOfficeText"] != null))
                {
                    //Get Info From Session
                    ComboServiceOffice.Value =
                        Convert.ToInt32((HttpContext.Current.Session["ComboServiceOffice"].ToString()));
                    ComboServiceOffice.Text = (HttpContext.Current.Session["ComboServiceOfficeText"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtFN"] != null)
                {
                    //Get Info From Session
                    txtFN.Value = (HttpContext.Current.Session["txtFN"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtLN"] != null)
                {
                    //Get Info From Session
                    txtLN.Value = (HttpContext.Current.Session["txtLN"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtEmail"] != null)
                {
                    //Get Info From Session
                    txtEmail.Value = (HttpContext.Current.Session["txtEmail"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtPhone"] != null)
                {
                    //Get Info From Session
                    txtPhone.Value = (HttpContext.Current.Session["txtPhone"].ToString());
                }
                else
                {
                    //Set Default
                    txtPhone.Value = 1111111111;
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtExt"] != null)
                {
                    //Get Info From Session
                    txtExt.Value = (HttpContext.Current.Session["txtExt"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtMail"] != null)
                {
                    //Get Info From Session
                    txtMail.Value = (HttpContext.Current.Session["txtMail"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtBuilding"] != null)
                {
                    //Get Info From Session
                    txtBuilding.Value = (HttpContext.Current.Session["txtBuilding"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtRoomNum"] != null)
                {
                    //Get Info From Session
                    txtRoomNum.Value = (HttpContext.Current.Session["txtRoomNum"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["GPSX"] != null)
                {
                    //Get Info From Session
                    GPSX.Value = (HttpContext.Current.Session["GPSX"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["GPSY"] != null)
                {
                    //Get Info From Session
                    GPSY.Value = (HttpContext.Current.Session["GPSY"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["GPSZ"] != null)
                {
                    //Get Info From Session
                    GPSZ.Value = (HttpContext.Current.Session["GPSZ"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["ObjectIDCombo"] != null)
                {
                    //Get Info From Session
                    ObjectIDCombo.Value = Convert.ToInt32((HttpContext.Current.Session["ObjectIDCombo"].ToString()));
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["ObjectIDComboText"] != null)
                {
                    //Get Info From Session
                    ObjectIDCombo.Text = (HttpContext.Current.Session["ObjectIDComboText"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtObjectDescription"] != null)
                {
                    //Get Info From Session
                    txtObjectDescription.Text = (HttpContext.Current.Session["txtObjectDescription"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtObjectArea"] != null)
                {
                    //Get Info From Session
                    txtObjectArea.Text = (HttpContext.Current.Session["txtObjectArea"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtObjectLocation"] != null)
                {
                    //Get Info From Session
                    txtObjectLocation.Text = (HttpContext.Current.Session["txtObjectLocation"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtObjectAssetNumber"] != null)
                {
                    //Get Info From Session
                    txtObjectAssetNumber.Text = (HttpContext.Current.Session["txtObjectAssetNumber"].ToString());
                }

                //Check For Previous Session Variables
                TxtWorkRequestDate.Value = HttpContext.Current.Session["TxtWorkRequestDate"] != null
                    ? Convert.ToDateTime((HttpContext.Current.Session["TxtWorkRequestDate"].ToString()))
                    : DateTime.Now;

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboRequestor"] != null) &&
                    (HttpContext.Current.Session["ComboRequestorText"] != null))
                {
                    //Get Info From Session
                    ComboRequestor.Value = Convert.ToInt32((HttpContext.Current.Session["ComboRequestor"].ToString()));
                    ComboRequestor.Text = (HttpContext.Current.Session["ComboRequestorText"].ToString());
                }
                else if (HttpContext.Current.Session["LogonInfo"] != null)
                {
                    //Get Logon Info
                    _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                    //Set Requestor
                    ComboRequestor.Value = _oLogon.UserID;
                    ComboRequestor.Text = _oLogon.Username;

                    //Add Session Variables
                    HttpContext.Current.Session.Add("ComboRequestor", _oLogon.UserID);
                    HttpContext.Current.Session.Add("ComboRequestorText", _oLogon.Username);
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["ComboPriority"] != null) &&
                    (HttpContext.Current.Session["ComboPriorityText"] != null))
                {
                    //Get Info From Session
                    ComboPriority.Value = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
                    ComboPriority.Text = (HttpContext.Current.Session["ComboPriorityText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["comboReason"] != null) &&
                    (HttpContext.Current.Session["comboReasonText"] != null))
                {
                    //Get Info From Session
                    comboReason.Value = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
                    comboReason.Text = (HttpContext.Current.Session["comboReasonText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["comboRouteTo"] != null) &&
                    (HttpContext.Current.Session["comboRouteToText"] != null))
                {
                    //Get Info From Session
                    comboRouteTo.Value = Convert.ToInt32((HttpContext.Current.Session["comboRouteTo"].ToString()));
                    comboRouteTo.Text = (HttpContext.Current.Session["comboRouteToText"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["comboHwyRoute"] != null) &&
                    (HttpContext.Current.Session["comboHwyRouteText"] != null))
                {
                    //Get Info From Session
                    comboHwyRoute.Value = Convert.ToInt32((HttpContext.Current.Session["comboHwyRoute"].ToString()));
                    comboHwyRoute.Text = (HttpContext.Current.Session["comboHwyRouteText"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtMilepost"] != null)
                {
                    //Get Info From Session
                    txtMilepost.Value = (HttpContext.Current.Session["txtMilepost"].ToString());
                }

                //Check For Previous Session Variables
                if (HttpContext.Current.Session["txtMilepostTo"] != null)
                {
                    //Get Info From Session
                    txtMilepostTo.Value = (HttpContext.Current.Session["txtMilepostTo"].ToString());
                }

                //Check For Previous Session Variables
                if ((HttpContext.Current.Session["comboMilePostDir"] != null) &&
                    (HttpContext.Current.Session["comboMilePostDirText"] != null))
                {
                    //Get Info From Session
                    comboMilePostDir.Value =
                        Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
                    comboMilePostDir.Text = (HttpContext.Current.Session["comboMilePostDirText"].ToString());
                }

                #region Process Unit One

                //Check For Value
                if (HttpContext.Current.Session["txtRunUnitOne"] != null)
                {
                    //Check Value
                    if (Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitOne"]) > 0)
                    {
                        //Set Value
                        txtRunUnitOne.Value =
                            Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitOne"]);

                        //Enable Fields
                        txtRunUnitOne.ReadOnly = _jobIsHistory;
                    }
                    else
                    {
                        ///Disable Fields & Put Defaults In Them
                        txtRunUnitOne.Value = 0.00;
                        txtRunUnitOne.ReadOnly = true;
                    }
                }
                else
                {
                    ///Disable Fields & Put Defaults In Them
                    txtRunUnitOne.Value = 0.00;
                    txtRunUnitOne.ReadOnly = true;
                }

                #endregion

                #region Process Unit Two

                //Check For Value
                if (HttpContext.Current.Session["txtRunUnitTwo"] != null)
                {
                    //Check Value
                    if (Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitTwo"]) > 0)
                    {
                        //Set Value
                        txtRunUnitTwo.Value =
                            Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitTwo"]);

                        //Enable Fields
                        txtRunUnitTwo.ReadOnly = _jobIsHistory;
                    }
                    else
                    {
                        ///Disable Fields & Put Defaults In Them
                        txtRunUnitTwo.Value = 0.00;
                        txtRunUnitTwo.ReadOnly = true;
                    }
                }
                else
                {
                    ///Disable Fields & Put Defaults In Them
                    txtRunUnitTwo.Value = 0.00;
                    txtRunUnitTwo.ReadOnly = true;
                }

                #endregion

                #region Process Unit Three

                //Check For Value
                if (HttpContext.Current.Session["txtRunUnitThree"] != null)
                {
                    //Check Value
                    if (Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitThree"]) > 0)
                    {
                        //Set Value
                        txtRunUnitThree.Value =
                            Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitThree"]);

                        //Enable Fields
                        txtRunUnitThree.ReadOnly = _jobIsHistory;
                    }
                    else
                    {
                        ///Disable Fields & Put Defaults In Them
                        txtRunUnitThree.Value = 0.00;
                        txtRunUnitThree.ReadOnly = true;
                    }
                }
                else
                {
                    ///Disable Fields & Put Defaults In Them
                    txtRunUnitThree.Value = 0.00;
                    txtRunUnitThree.ReadOnly = true;
                }

                #endregion

                #region Process Run Unit Class

                if (HttpContext.Current.Session["RunUnit"] != null)
                {
                    //Get From Session
                    _oRunUnit = ((MaintObjectRunUnit)HttpContext.Current.Session["RunUnit"]);
                }

                #endregion

            }
            #endregion

            #region Bind Grids
            //Bind Grids
            AttachmentGrid.DataBind();

            //Refresh Crew
            CrewGrid.DataBind();

            //Refresh Members
            MemberGrid.DataBind();

            //Refresh Other
            OtherGrid.DataBind();

            //Refresh Equipment
            EquipGrid.DataBind();

            //Refresh Parts Grid
            PartGrid.DataBind();

            #endregion

            #region Button status
            //Get Button Status
            var showButtons = !((CrewGrid.Columns[0].Visible)
                                || (MemberGrid.Columns[0].Visible)
                                || (PartGrid.Columns[0].Visible)
                                || (EquipGrid.Columns[0].Visible)
                                || (OtherGrid.Columns[0].Visible));

            //Enable/Disable Buttons
            Master.ShowNewButton = false;
            Master.ShowEditButton = (showButtons && (HttpContext.Current.Session[""] != null));
            Master.ShowViewButton = false;
            Master.ShowSaveButton = showButtons;
            Master.ShowPrintButton = false;
            Master.ShowPostButton = showButtons;
            Master.ShowIssueButton = showButtons;
            Master.ShowCopyJobButton = showButtons;
            Master.ShowAddCrewGroupButton = (!showButtons && (CrewGrid.Columns[0].Visible));
            Master.ShowAddCrewLaborButton = (!showButtons && (CrewGrid.Columns[0].Visible));
            Master.ShowSetStartDateButton = (!showButtons
                                             && (HttpContext.Current.Session["TxtStartingDate"] != null)
                                             && (MemberGrid.Columns[0].Visible));
            Master.ShowSetEndDateButton = (!showButtons
                                           && (HttpContext.Current.Session["TxtCompletionDate"] != null)
                                           && (MemberGrid.Columns[0].Visible));
            Master.ShowNonStockAddButton = (!showButtons
                                           && (PartGrid.Columns[0].Visible));

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(CrewGrid.Columns[0].Visible))
            {
                //Uncheck All
                CrewGrid.Selection.UnselectAll();
            }
            else
            {
                //Make Sure Settings Are Right
                CrewGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            }

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(MemberGrid.Columns[0].Visible))
            {
                //Uncheck All
                MemberGrid.Selection.UnselectAll();
            }
            else
            {
                //Make Sure Settings Are Right
                MemberGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            }

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(PartGrid.Columns[0].Visible))
            {
                //Uncheck All
                PartGrid.Selection.UnselectAll();
            }
            else
            {
                //Make Sure Settings Are Right
                PartGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            }

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(EquipGrid.Columns[0].Visible))
            {
                //Uncheck All
                EquipGrid.Selection.UnselectAll();
            }
            else
            {
                //Make Sure Settings Are Right
                EquipGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            }

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(OtherGrid.Columns[0].Visible))
            {
                //Uncheck All
                OtherGrid.Selection.UnselectAll();
            }
            else
            {
                //Make Sure Settings Are Right
                OtherGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            }
            #endregion

            #region Check for MultiGrid
            //Check For MultiGrid
            if (MultiGrid.Contains("Grid"))
            {
                //Determine Current Grid In Multi Mode
                switch (MultiGrid.Get("Grid").ToString())
                {
                    case "MemberGrid":
                        {
                            //Disable Other Tabs
                            StepTab.TabPages[0].ClientEnabled = false;  //Step
                            StepTab.TabPages[2].ClientEnabled = false;  //Crew
                            StepTab.TabPages[3].ClientEnabled = false;  //Parts
                            StepTab.TabPages[4].ClientEnabled = false;  //Equip
                            StepTab.TabPages[5].ClientEnabled = false;  //Other
                            StepTab.TabPages[6].ClientEnabled = false;  //Attachments
                            break;
                        }
                    case "CrewGrid":
                        {
                            //Disable Other Tabs
                            StepTab.TabPages[0].ClientEnabled = false;  //Step
                            StepTab.TabPages[1].ClientEnabled = false;  //Members
                            StepTab.TabPages[3].ClientEnabled = false;  //Parts
                            StepTab.TabPages[4].ClientEnabled = false;  //Equip
                            StepTab.TabPages[5].ClientEnabled = false;  //Other
                            StepTab.TabPages[6].ClientEnabled = false;  //Attachments
                            break;
                        }
                    case "PartGrid":
                        {
                            //Disable Other Tabs
                            StepTab.TabPages[0].ClientEnabled = false;  //Step
                            StepTab.TabPages[1].ClientEnabled = false;  //Members
                            StepTab.TabPages[2].ClientEnabled = false;  //Crew
                            StepTab.TabPages[4].ClientEnabled = false;  //Equip
                            StepTab.TabPages[5].ClientEnabled = false;  //Other
                            StepTab.TabPages[6].ClientEnabled = false;  //Attachments
                            break;
                        }
                    case "EquipGrid":
                        {
                            //Disable Other Tabs
                            StepTab.TabPages[0].ClientEnabled = false;  //Step
                            StepTab.TabPages[1].ClientEnabled = false;  //Members
                            StepTab.TabPages[2].ClientEnabled = false;  //Crew
                            StepTab.TabPages[3].ClientEnabled = false;  //Parts
                            StepTab.TabPages[5].ClientEnabled = false;  //Other
                            StepTab.TabPages[6].ClientEnabled = false;  //Attachments
                            break;
                        }
                    case "OtherGrid":
                        {
                            StepTab.TabPages[0].ClientEnabled = false;  //Step
                            StepTab.TabPages[1].ClientEnabled = false;  //Members
                            StepTab.TabPages[2].ClientEnabled = false;  //Crew
                            StepTab.TabPages[3].ClientEnabled = false;  //Parts
                            StepTab.TabPages[4].ClientEnabled = false;  //Equip
                            StepTab.TabPages[6].ClientEnabled = false;  //Attachments
                            break;
                        }
                    default:
                        {
                            //Make Sure All Tabs Are Client Enabled
                            StepTab.TabPages[0].ClientEnabled = true;  //Step
                            StepTab.TabPages[1].ClientEnabled = true;  //Members
                            StepTab.TabPages[2].ClientEnabled = true;  //Crew
                            StepTab.TabPages[3].ClientEnabled = true;  //Parts
                            StepTab.TabPages[4].ClientEnabled = true;  //Equip
                            StepTab.TabPages[5].ClientEnabled = true;  //Other
                            StepTab.TabPages[6].ClientEnabled = true;  //Attachments
                            break;
                        }
                }
            }
            else
            {
                //Make Sure All Tabs Are Client Enabled
                StepTab.TabPages[0].ClientEnabled = true;  //Step
                StepTab.TabPages[1].ClientEnabled = true;  //Members
                StepTab.TabPages[2].ClientEnabled = true;  //Crew
                StepTab.TabPages[3].ClientEnabled = true;  //Parts
                StepTab.TabPages[4].ClientEnabled = true;  //Equip
                StepTab.TabPages[5].ClientEnabled = true;  //Other
                StepTab.TabPages[6].ClientEnabled = true;  //Attachments
            }
            #endregion
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //Set Connection Info
            _connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
            _useWeb = (ConfigurationManager.AppSettings["UsingWebService"] == "Y");


            //Initialize Classes
            _oJob = new WorkOrder(_connectionString, _useWeb);
            _oRunUnit = new MaintObjectRunUnit(_connectionString, _useWeb);
            _oAttachments = new AttachmentObject(_connectionString, _useWeb);
            _oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);
            _oJobCrew = new JobstepJobCrew(_connectionString, _useWeb);
            _oJobParts = new JobstepJobParts(_connectionString, _useWeb);
            _oJobEquipment = new JobEquipment(_connectionString, _useWeb);
            _oJobOther = new JobOther(_connectionString, _useWeb);
            _oJobMembers = new JobMembers(_connectionString, _useWeb);
            _oMpetUser = new MpetUserDbClass(_connectionString, _useWeb);
            //Set Datasources
            StoreroomPartDS.ConnectionString = _connectionString;
            AreaSqlDatasource.ConnectionString = _connectionString;
            ObjectDataSource.ConnectionString = _connectionString;
            CostCodeSqlDatasource.ConnectionString = _connectionString;
            FundSourceSqlDatasource.ConnectionString = _connectionString;
            WorkOrderSqlDatasource.ConnectionString = _connectionString;
            WorkOpSqlDatasource.ConnectionString = _connectionString;
            OrgCodeSqlDatasource.ConnectionString = _connectionString;
            FundGroupSqlDatasource.ConnectionString = _connectionString;
            CtlSectionSqlDatasource.ConnectionString = _connectionString;
            EquipNumSqlDatasource.ConnectionString = _connectionString;
            RequestorSqlDatasource.ConnectionString = _connectionString;
            PrioritySqlDatasource.ConnectionString = _connectionString;
            ReasonSqlDatasource.ConnectionString = _connectionString;
            RouteToSqlDatasource.ConnectionString = _connectionString;
            HwyRouteSqlDatasource.ConnectionString = _connectionString;
            MilePostDirSqlDatasource.ConnectionString = _connectionString;
            CrewUserDataSource.ConnectionString = _connectionString;
            CompletedByDataSource.ConnectionString = _connectionString;
            PostedByDataSource.ConnectionString = _connectionString;
            OutcomeCodeDS.ConnectionString = _connectionString;

            //Setup Fields
            TxtWorkRequestDate.Value = DateTime.Now;
            txtPhone.Value = 1111111111;

        }

        public bool FormSetup(int userId)
        {
            //Create Flag
            var rightsLoaded = false;

            //Get Security Settings
            using (
                var oSecurity = new UserSecurityTemplate(_connectionString, _useWeb))
            {
                //Get Rights
                rightsLoaded = oSecurity.GetUserFormRights(userId, AssignedFormID,
                    ref _userCanEdit, ref _userCanAdd,
                    ref _userCanDelete, ref _userCanView);
            }

            //Return Flag
            return rightsLoaded;
        }

        #region Setup for Viewing, Editing, Adding
        /// <summary>
        /// Enables Form Buttons For Viewing
        /// </summary>
        private void SetupForViewing()
        {
            //Setup Buttons
            Master.ShowSaveButton = (_userCanAdd || _userCanEdit);
            Master.ShowNewButton = false;
            Master.ShowDeleteButton = false;
            Master.ShowPrintButton = false;

        }

        /// <summary>
        /// Enables Form Optiosn For Editing
        /// </summary>
        private void SetupForEditing()
        {
            //Setup Buttons
            Master.ShowSaveButton = (_userCanAdd || _userCanEdit);
            Master.ShowCopyJobButton = _userCanAdd;
            Master.ShowIssueButton = _userCanEdit;
            Master.ShowEditButton = _userCanEdit;
            Master.ShowNewButton = false;
            Master.ShowDeleteButton = false;
            Master.ShowPrintButton = false;
            Master.ShowMultiSelectButton = _userCanEdit;

            //Enable Tabs
            requestTab.Enabled = true;
        }

        /// <summary>
        /// Enables Form Options For Adding
        /// </summary>
        private void SetupForAdding()
        {
            //Setup Buttons
            Master.ShowSaveButton = (_userCanAdd || _userCanEdit);
            Master.ShowNewButton = false;
            Master.ShowDeleteButton = false;

            //Disable Tabs
            requestTab.Enabled = false;
        }
        #endregion

        #region Enable Multi Select for tab Pages
        protected void EnableCrewMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            CrewGrid.Columns[0].Visible = showMultiSelect;

            //Set Multi Grid Text
            if (showMultiSelect)
            {
                //Add Grid
                MultiGrid.Add("Grid", "CrewGrid");
            }
            else
            {
                //Clear Collection
                MultiGrid.Clear();
            }
        }

        protected void EnableMemberMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            MemberGrid.Columns[0].Visible = showMultiSelect;

            //Set Edit Mode
            MemberGrid.SettingsEditing.Mode = showMultiSelect ? GridViewEditingMode.Inline : GridViewEditingMode.PopupEditForm;

            //Set Multi Grid Text
            if (showMultiSelect)
            {
                //Add Grid
                MultiGrid.Add("Grid", "MemberGrid");
            }
            else
            {
                //Clear Collection
                MultiGrid.Clear();
            }
        }

        protected void EnablePartMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            PartGrid.Columns[0].Visible = showMultiSelect;

            //Set Edit Mode
            PartGrid.SettingsEditing.Mode = showMultiSelect
                ? GridViewEditingMode.Inline
                : GridViewEditingMode.PopupEditForm;

            //Set Multi Grid Text
            if (showMultiSelect)
            {
                //Add Grid
                MultiGrid.Add("Grid", "PartGrid");
            }
            else
            {
                //Clear Collection
                MultiGrid.Clear();
            }
        }

        protected void EnableEquipMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            EquipGrid.Columns[0].Visible = showMultiSelect;

            //Set Edit Mode
            EquipGrid.SettingsEditing.Mode = showMultiSelect ? GridViewEditingMode.Inline : GridViewEditingMode.PopupEditForm;

            //Set Multi Grid Text
            if (showMultiSelect)
            {
                //Add Grid
                MultiGrid.Add("Grid", "EquipGrid");
            }
            else
            {
                //Clear Collection
                MultiGrid.Clear();
            }
        }

        protected void EnableOtherMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            OtherGrid.Columns[0].Visible = showMultiSelect;

            //Set Edit Mode
            OtherGrid.SettingsEditing.Mode = showMultiSelect ? GridViewEditingMode.Inline : GridViewEditingMode.PopupEditForm;

            //Set Multi Grid Text
            if (showMultiSelect)
            {
                //Add Grid
                MultiGrid.Add("Grid", "OtherGrid");
            }
            else
            {
                //Clear Collection
                MultiGrid.Clear();
            }
        }
        #endregion

        #region Azure Setup
        string AzureAccount
        {
            get
            {
                return WebConfigurationManager.AppSettings["StorageAccount"];
            }
        }

        string AzureAccessKey
        {
            get
            {
                return WebConfigurationManager.AppSettings["StorageKey"];
            }
        }

        string AzureContainer
        {
            get
            {
                return WebConfigurationManager.AppSettings["StorageContainer"];
            }
        }
        #endregion

        #region File Upload Set up
        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            // RemoveFileWithDelay(e.UploadedFile.FileNameInStorage, 5);

            string name = e.UploadedFile.FileName;
            string url = GetImageUrl(e.UploadedFile.FileNameInStorage);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;

            //INSERT JOB ATTACHMENT ROUTINE HERE!!!!

            //Check For Job ID
            if (HttpContext.Current.Session["editingJobID"] != null)
            {
                //Check For Previous Session Variable
                if (HttpContext.Current.Session["LogonInfo"] != null)
                {
                    //Get Logon Info From Session
                    _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
                    var jobStepID = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());

                    if (_oAttachments.Add(Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()),
                        jobStepID,
                        _oLogon.UserID,
                        url,
                        "JPG",
                        "Mobile Web Attachment",
                        name.Trim()))
                    {
                        //Check For Prior Value
                        if (HttpContext.Current.Session["HasAttachments"] != null)
                        {
                            //Remove Old One
                            HttpContext.Current.Session.Remove("HasAttachments");
                        }

                        //Add New Value
                        HttpContext.Current.Session.Add("HasAttachments", true);

                        //Refresh Attachments
                        AttachmentGrid.DataBind();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "refreshAttachments", "refreshAttachments();", true);

                    }
                }
            }
        }

        protected void fileManager_FileUploading(object sender, FileManagerFileUploadEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemRenaming(object sender, FileManagerItemRenameEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemMoving(object sender, FileManagerItemMoveEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemDeleting(object sender, FileManagerItemDeleteEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_FolderCreating(object sender, FileManagerFolderCreateEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemCopying(object sender, FileManagerItemCopyEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        void ValidateSiteEdit(FileManagerActionEventArgsBase e)
        {
            //e.Cancel = Utils.IsSiteMode;
            //e.ErrorText = Utils.GetReadOnlyMessageText();
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

        string GetImageUrl(string fileName)
        {
                AzureFileSystemProvider provider = new AzureFileSystemProvider("");
                
            if (WebConfigurationManager.AppSettings["StorageAccount"] != null)
            {
                provider.StorageAccountName = UploadControl.AzureSettings.StorageAccountName;
                provider.AccessKey = UploadControl.AzureSettings.AccessKey;
                provider.ContainerName = UploadControl.AzureSettings.ContainerName;
            } else
            {
                   
            }
                FileManagerFile file = new FileManagerFile(provider, fileName);
                FileManagerFile[] files = new FileManagerFile[] { file };
                return provider.GetDownloadUrl(files);

        }

        protected string GetUrl(GridViewDataItemTemplateContainer container)
        {
            var values = (int)container.Grid.GetRowValues(container.VisibleIndex, new[] { "n_jobstepid" });
            return "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + values;
        }
        #endregion

        #region Printing Selected Row
        private void PrintSelectedRow()
        {
            //Check For Row Value 
            if (HttpContext.Current.Session["editingJobID"] != null)
            {
                //Check For Previous Session Report Parm ID
                if (HttpContext.Current.Session["ReportParm"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportParm");
                }

                //Add Session Report Parm ID
                HttpContext.Current.Session.Add("ReportParm", HttpContext.Current.Session["editingJobID"]);

                //Check For Previous Report Name
                if (HttpContext.Current.Session["ReportToDisplay"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportToDisplay");
                }

                //Add Report To Display
                HttpContext.Current.Session.Add("ReportToDisplay", "mltstpwo.rpt");

                //Redirect To Report Page
                Response.Redirect("~/Reports/ViewReport.aspx", true);
            }
        }
        #endregion

        #region Setup User Defined Fields
        // <summary>
        // Checks For User Defined Fields And Labels Accordingly
        // Changes the default field names to CUSTOM Names by User
        // THIS is LINDT USAGE ONLY CURRENTLY DS 3/31/17
        // </summary>
        private void SetupUserDefinedFields()
        {
            //Get Logon Info
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
            }

            //Check Group Count
            if (WRCFL.Items.Count > 0)
            {
                //Get Layout Group
                var layoutGroup = ((LayoutGroup)WRCFL.Items[0]);

                for (var rowIndex = 0; rowIndex < layoutGroup.Items.Count; rowIndex++)
                {
                    //Determine Current Item
                    switch (layoutGroup.Items[rowIndex].Name)
                    {
                        case "fldCostCode":
                            {
                                //Check Cost Code Flag
                                if ((_oLogon.RenameCostCode != null) && (_oLogon.RenameCostCode))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameCostCodeTo;

                                    //Update Combo Header
                                    ComboCostCode.Columns[1].Caption = _oLogon.RenameCostCodeTo;
                                }

                                break;
                            }

                        case "fldFundSrc":
                            {
                                //Check FUnd Source Flag
                                if ((_oLogon.RenameFundSource != null) && (_oLogon.RenameFundSource))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameFundSourceTo;

                                    //Update Combo Header
                                    ComboFundSource.Columns[1].Caption = _oLogon.RenameFundSourceTo;
                                }

                                break;
                            }

                        case "fldWorkOrder":
                            {
                                //Check Work Order Flag
                                if ((_oLogon.RenameWorkOrder != null) && (_oLogon.RenameWorkOrder))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameWorkOrderTo;

                                    //Update Combo Header
                                    ComboWorkOrder.Columns[1].Caption = _oLogon.RenameWorkOrderTo;
                                }

                                break;
                            }

                        case "fldWorkOp":
                            {
                                //Check WOrk Op Flag
                                if ((_oLogon.RenameWorkOp != null) && (_oLogon.RenameWorkOp))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameWorkOpTo;

                                    //Update Combo Header
                                    ComboWorkOp.Columns[1].Caption = _oLogon.RenameWorkOpTo;
                                }

                                break;
                            }

                        case "fldOrgCode":
                            {
                                //Check Org Code Flag
                                if ((_oLogon.RenameOrgCode != null) && (_oLogon.RenameOrgCode))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameOrgCodeTo;

                                    //Update Combo Header
                                    ComboOrgCode.Columns[1].Caption = _oLogon.RenameOrgCodeTo;
                                }

                                break;
                            }

                        case "fldFundGrp":
                            {
                                //Check Fund Group Flag
                                if ((_oLogon.RenameFundGroup != null) && (_oLogon.RenameFundGroup))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameFundGroupTo;

                                    //Update Combo Header
                                    ComboFundGroup.Columns[1].Caption = _oLogon.RenameFundGroupTo;
                                }

                                break;
                            }

                        case "fldCtlSection":
                            {
                                //Check Ctl Section Flag
                                if ((_oLogon.RenameControlSection != null) && (_oLogon.RenameControlSection))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameControlSectionTo;

                                    //Update Combo Header
                                    ComboCtlSection.Columns[1].Caption = _oLogon.RenameControlSectionTo;
                                }

                                break;
                            }

                        case "fldEquipNum":
                            {
                                //Check Equip Flag
                                if ((_oLogon.RenameEquipNumber != null) && (_oLogon.RenameEquipNumber))
                                {
                                    //Set Caption
                                    layoutGroup.Items[rowIndex].Caption = _oLogon.RenameEquipNumberTo;

                                    //Update Combo Header
                                    ComboEquipNum.Columns[1].Caption = _oLogon.RenameEquipNumberTo;
                                }

                                break;
                            }
                        default:
                            {
                                //Do Nothing
                                break;
                            }
                    }
                }
            }
        }
        #endregion

        #region Combo Loading Events

        protected void ComboCostCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CostCodeSqlDatasource.SelectCommand =
                @"SELECT  n_costcodeid ,
                            costcodeid ,
                            [Description]
                    FROM    ( SELECT    tblCostCode.n_costcodeid ,
                                        tblCostCode.costcodeid ,
                                        tblCostCode.[Descr] AS [Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblCostCode.n_costcodeid ) AS [rn]
                              FROM      dbo.CostCodes AS tblCostCode
                              WHERE     ( ( costcodeid + ' ' + [Descr] ) LIKE @filter )
                                        AND tblCostCode.b_IsActive = 'Y'
                                        AND tblCostCode.n_costcodeid > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            CostCodeSqlDatasource.SelectParameters.Clear();
            CostCodeSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            CostCodeSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            CostCodeSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = CostCodeSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboCostCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CostCodeSqlDatasource.SelectCommand = @"SELECT  tblCostCode.n_costcodeid ,
                                                            tblCostCode.costcodeid ,
                                                            tblCostCode.[Descr] AS [Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblCostCode.n_costcodeid ) AS [rn]
                                                    FROM    dbo.CostCodes AS tblCostCode
                                                    WHERE   ( n_costcodeid = @ID )
                                                    ORDER BY costcodeid";

            CostCodeSqlDatasource.SelectParameters.Clear();
            CostCodeSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = CostCodeSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboRequestor_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            RequestorSqlDatasource.SelectCommand =
                @"SELECT  [UserID] ,
                            [username] ,
                            [FullName] 
                    FROM    ( SELECT    tblUsers.[UserID] ,
                                        tblUsers.[Username] ,
                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                              FROM      dbo.MPetUsers AS tblUsers
                              WHERE     ( ( [Username] + ' ' + [firstname] + ' ' + [lastname] ) LIKE @filter )
                                        AND tblUsers.Active = 1
                                        AND tblUsers.UserID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            RequestorSqlDatasource.SelectParameters.Clear();
            RequestorSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            RequestorSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            RequestorSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = RequestorSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboRequestor_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            RequestorSqlDatasource.SelectCommand = @"SELECT  tblUsers.[UserID] ,
                                                        tblUsers.[Username] ,
                                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                                                FROM    dbo.MPetUsers AS tblUsers
                                                WHERE   ( UserID = @ID )
                                                ORDER BY Username";

            RequestorSqlDatasource.SelectParameters.Clear();
            RequestorSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = RequestorSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboCompletedBy_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CompletedByDataSource.SelectCommand =
                @"SELECT  [UserID] ,
                            [username] ,
                            [FullName] 
                    FROM    ( SELECT    tblUsers.[UserID] ,
                                        tblUsers.[Username] ,
                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                              FROM      dbo.MPetUsers AS tblUsers
                              WHERE     ( ( [Username] + ' ' + [firstname] + ' ' + [lastname] ) LIKE @filter )
                                        AND tblUsers.Active = 1
                                        AND tblUsers.UserID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            CompletedByDataSource.SelectParameters.Clear();
            CompletedByDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            CompletedByDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            CompletedByDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = CompletedByDataSource;
            comboBox.DataBind();
        }

        protected void ComboCompletedBy_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CompletedByDataSource.SelectCommand = @"SELECT  tblUsers.[UserID] ,
                                                        tblUsers.[Username] ,
                                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                                                FROM    dbo.MPetUsers AS tblUsers
                                                WHERE   ( UserID = @ID )
                                                ORDER BY Username";

            CompletedByDataSource.SelectParameters.Clear();
            CompletedByDataSource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = CompletedByDataSource;
            comboBox.DataBind();
        }

        protected void ComboPostedBy_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            PostedByDataSource.SelectCommand =
                @"SELECT  [UserID] ,
                            [username] ,
                            [FullName] 
                    FROM    ( SELECT    tblUsers.[UserID] ,
                                        tblUsers.[Username] ,
                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                              FROM      dbo.MPetUsers AS tblUsers
                              WHERE     ( ( [Username] + ' ' + [firstname] + ' ' + [lastname] ) LIKE @filter )
                                        AND tblUsers.Active = 1
                                        AND tblUsers.UserID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            PostedByDataSource.SelectParameters.Clear();
            PostedByDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            PostedByDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            PostedByDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = PostedByDataSource;
            comboBox.DataBind();
        }

        protected void ComboPostedBy_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            PostedByDataSource.SelectCommand = @"SELECT  tblUsers.[UserID] ,
                                                        tblUsers.[Username] ,
                                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                                                FROM    dbo.MPetUsers AS tblUsers
                                                WHERE   ( UserID = @ID )
                                                ORDER BY Username";

            PostedByDataSource.SelectParameters.Clear();
            PostedByDataSource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = PostedByDataSource;
            comboBox.DataBind();
        }

        protected void ComboPriority_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            PrioritySqlDatasource.SelectCommand =
                @"SELECT  [n_priorityid] ,
                            [priorityid] ,
                            [description]
                    FROM    ( SELECT    tblPriority.[n_priorityid] ,
                                        tblPriority.[priorityid] ,
                                        tblPriority.[description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblPriority.[n_priorityid] ) AS [rn]
                              FROM      dbo.Priorities AS tblPriority
                              WHERE     ( ( [priorityid] + ' ' + [description] ) LIKE @filter )
                                        AND tblPriority.Active = 'Y'
                                        AND tblPriority.n_priorityid > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            PrioritySqlDatasource.SelectParameters.Clear();
            PrioritySqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            PrioritySqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            PrioritySqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = PrioritySqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboPriority_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            PrioritySqlDatasource.SelectCommand = @"SELECT  tblPriority.[n_priorityid] ,
                                                            tblPriority.[priorityid] ,
                                                            tblPriority.[description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblPriority.[n_priorityid] ) AS [rn]
                                                    FROM    dbo.Priorities AS tblPriority
                                                    WHERE   ( n_priorityid = @ID )
                                                    ORDER BY priorityid";

            PrioritySqlDatasource.SelectParameters.Clear();
            PrioritySqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = PrioritySqlDatasource;
            comboBox.DataBind();
        }

        protected void comboReason_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            ReasonSqlDatasource.SelectCommand =
                @"SELECT  [n_reasonid] ,
                            [reasonid] ,
                            [description]
                    FROM    ( SELECT    tblReasons.nJobReasonID AS 'n_reasonid' ,
                                        tblReasons.JobReasonID AS 'reasonid' ,
                                        tblReasons.Description AS 'description' ,
                                        ROW_NUMBER() OVER ( ORDER BY tblReasons.[nJobReasonID] ) AS [rn]
                              FROM      dbo.JobReasons AS tblReasons
                              WHERE     ( ( [JobReasonID] + ' ' + [description] ) LIKE @filter )
                                        AND tblReasons.b_IsActive = 'Y'
                                        AND tblReasons.nJobReasonID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            ReasonSqlDatasource.SelectParameters.Clear();
            ReasonSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            ReasonSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            ReasonSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = ReasonSqlDatasource;
            comboBox.DataBind();
        }

        protected void comboReason_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            ReasonSqlDatasource.SelectCommand = @"SELECT  tblReason.nJobReasonID AS 'n_reasonid' ,
                                                        tblReason.JobReasonID AS 'reasonid' ,
                                                        tblReason.[description] ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblReason.nJobReasonID ) AS [rn]
                                                FROM    dbo.JobReasons AS tblReason
                                                WHERE   ( nJobReasonID = @ID )
                                                ORDER BY JobReasonID";

            ReasonSqlDatasource.SelectParameters.Clear();
            ReasonSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = ReasonSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            OutcomeCodeDS.SelectCommand =
                @"SELECT  [n_outcomecodeid] ,
                            [outcomecodeid] ,
                            [description]
                    FROM    ( SELECT    tblOutcomes.n_outcomecodeid AS 'n_outcomecodeid' ,
                                        tblOutcomes.outcomecodeid AS 'outcomecodeid' ,
                                        tblOutcomes.Description AS 'description' ,
                                        ROW_NUMBER() OVER ( ORDER BY tblOutcomes.[n_outcomecodeid] ) AS [rn]
                              FROM      dbo.OutcomeCodes AS tblOutcomes
                              WHERE     ( ( [outcomecodeid] + ' ' + [description] ) LIKE @filter )
                                        AND tblOutcomes.b_IsActive = 'Y'
                                        AND tblOutcomes.n_outcomecodeid > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            OutcomeCodeDS.SelectParameters.Clear();
            OutcomeCodeDS.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            OutcomeCodeDS.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            OutcomeCodeDS.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = OutcomeCodeDS;
            comboBox.DataBind();
        }

        protected void ComboOutcomeCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            OutcomeCodeDS.SelectCommand = @"SELECT  tblOutcomes.n_outcomecodeid AS 'n_outcomecodeid' ,
                                                        tblOutcomes.outcomecodeid AS 'outcomecodeid' ,
                                                        tblOutcomes.[description] ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblOutcomes.n_outcomecodeid ) AS [rn]
                                                FROM    dbo.OutcomeCodes AS tblOutcomes
                                                WHERE   ( n_outcomecodeid = @ID )
                                                ORDER BY outcomecodeid";

            OutcomeCodeDS.SelectParameters.Clear();
            OutcomeCodeDS.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = OutcomeCodeDS;
            comboBox.DataBind();
        }

        protected void comboRouteTo_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            RouteToSqlDatasource.SelectCommand =
                @"SELECT  [UserID] ,
                            [username] ,
                            [FullName] 
                    FROM    ( SELECT    tblUsers.[UserID] ,
                                        tblUsers.[Username] ,
                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                              FROM      dbo.MPetUsers AS tblUsers
                              WHERE     ( ( [Username] + ' ' + [firstname] + ' ' + [lastname] ) LIKE @filter )
                                        AND tblUsers.Active = 1
                                        AND tblUsers.UserID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            RouteToSqlDatasource.SelectParameters.Clear();
            RouteToSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            RouteToSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            RouteToSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = RouteToSqlDatasource;
            comboBox.DataBind();
        }

        protected void comboRouteTo_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            RouteToSqlDatasource.SelectCommand = @"SELECT  tblUsers.[UserID] ,
                                                        tblUsers.[Username] ,
                                                        tblUsers.[lastname] + ', ' + tblUsers.[firstname] AS 'FullName' ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblUsers.[UserID] ) AS [rn]
                                                FROM    dbo.MPetUsers AS tblUsers
                                                WHERE   ( UserID = @ID )
                                                ORDER BY Username";

            RouteToSqlDatasource.SelectParameters.Clear();
            RouteToSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = RouteToSqlDatasource;
            comboBox.DataBind();
        }

        protected void comboHwyRoute_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            HwyRouteSqlDatasource.SelectCommand =
                @"SELECT  [n_StateRouteID] ,
                            [StateRouteID] ,
                            [Description]
                    FROM    ( SELECT    StateRoutes.n_StateRouteID ,
                                        StateRoutes.StateRouteID ,
                                        StateRoutes.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY StateRoutes.[n_StateRouteID] ) AS [rn]
                              FROM      dbo.StateRoutes AS StateRoutes
                              WHERE     ( ( [StateRouteID] + ' ' + [Description]) LIKE @filter )
                                        AND StateRoutes.b_IsActive = 'Y'
                                        AND StateRoutes.n_StateRouteID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            HwyRouteSqlDatasource.SelectParameters.Clear();
            HwyRouteSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            HwyRouteSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            HwyRouteSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = HwyRouteSqlDatasource;
            comboBox.DataBind();
        }

        protected void comboHwyRoute_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            HwyRouteSqlDatasource.SelectCommand = @"SELECT  tblStateRoutes.n_StateRouteID ,
                                                            tblStateRoutes.StateRouteID ,
                                                            tblStateRoutes.[Description],
                                                            ROW_NUMBER() OVER ( ORDER BY tblStateRoutes.[n_StateRouteID] ) AS [rn]
                                                    FROM    dbo.StateRoutes AS tblStateRoutes
                                                    WHERE   ( n_StateRouteID = @ID )
                                                    ORDER BY StateRouteID";

            HwyRouteSqlDatasource.SelectParameters.Clear();
            HwyRouteSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = HwyRouteSqlDatasource;
            comboBox.DataBind();
        }

        protected void comboMilePostDir_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            MilePostDirSqlDatasource.SelectCommand =
                @"SELECT  n_MilePostDirectionID ,
                            MilePostDirectionID ,
                            [Description]
                    FROM    ( SELECT    MilePostDir.n_MilePostDirectionID ,
                                        MilePostDir.MilePostDirectionID ,
                                        MilePostDir.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY MilePostDir.n_MilePostDirectionID ) AS [rn]
                              FROM      dbo.MilePostDirections AS MilePostDir
                              WHERE     ( ( MilePostDirectionID + ' ' + [Description] ) LIKE @filter )
                                        AND MilePostDir.b_IsActive = 'Y'
                                        AND MilePostDir.n_MilePostDirectionID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            MilePostDirSqlDatasource.SelectParameters.Clear();
            MilePostDirSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            MilePostDirSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            MilePostDirSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = MilePostDirSqlDatasource;
            comboBox.DataBind();
        }

        protected void comboMilePostDir_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            MilePostDirSqlDatasource.SelectCommand = @"SELECT  tblMilePostDir.n_MilePostDirectionID ,
                                                            tblMilePostDir.MilePostDirectionID ,
                                                            tblMilePostDir.[Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblMilePostDir.[n_MilePostDirectionID] ) AS [rn]
                                                    FROM    dbo.MilePostDirections AS tblMilePostDir
                                                    WHERE   ( n_MilePostDirectionID = @ID )
                                                    ORDER BY MilePostDirectionID";

            MilePostDirSqlDatasource.SelectParameters.Clear();
            MilePostDirSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = MilePostDirSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboFundSource_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //Check For Simple Costing
            if (_oLogon.UseSimpleCostLinking)
            {
                //Create Cost Code ID Var
                var costCodeId = -1;

                //Get Current Cost Code (If Applicable)
                if (ComboCostCode.Value != null)
                {
                    //Enable Combo
                    ComboFundSource.Enabled = true;

                    //Get ID
                    costCodeId = Convert.ToInt32(ComboCostCode.Value.ToString());

                    //Get Combo
                    var comboBox = (ASPxComboBox)source;
                    FundSourceSqlDatasource.SelectCommand =
                        string.Format(@"SELECT  n_FundSrcCodeID ,
                            FundSrcCodeID ,
                            [Description]
                    FROM    ( SELECT    tblFundSrc.n_FundSrcCodeID ,
                                        tblFundSrc.FundSrcCodeID ,
                                        tblFundSrc.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblFundSrc.n_FundSrcCodeID ) AS [rn]
                              FROM      dbo.FundSrcCodes AS tblFundSrc
		                      INNER JOIN ( SELECT dbo.CostCodeLinks.n_FundSrcCodeID
                                 FROM   dbo.CostCodeLinks
                                 WHERE  dbo.CostCodeLinks.n_CostCodeID = {0}
                               ) tbl_CostCodeLinks ON tblFundSrc.n_FundSrcCodeID = tbl_CostCodeLinks.n_FundSrcCodeID
                              WHERE     ( ( FundSrcCodeID + ' ' + [Description] ) LIKE @filter )
                                        AND tblFundSrc.b_IsActive = 'Y'
                                        AND tblFundSrc.n_FundSrcCodeID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex", costCodeId);

                    FundSourceSqlDatasource.SelectParameters.Clear();
                    FundSourceSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                    FundSourceSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                    FundSourceSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                    comboBox.DataSource = FundSourceSqlDatasource;
                    comboBox.DataBind();
                }
                else
                {
                    //Disable Combo
                    ComboFundSource.Enabled = false;
                }
            }
            else
            {
                //Get Combo
                var comboBox = (ASPxComboBox)source;
                FundSourceSqlDatasource.SelectCommand =
                    @"SELECT  n_FundSrcCodeID ,
                            FundSrcCodeID ,
                            [Description]
                    FROM    ( SELECT    tblFundSrc.n_FundSrcCodeID ,
                                        tblFundSrc.FundSrcCodeID ,
                                        tblFundSrc.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblFundSrc.n_FundSrcCodeID ) AS [rn]
                              FROM      dbo.FundSrcCodes AS tblFundSrc
                              WHERE     ( ( FundSrcCodeID + ' ' + [Description] ) LIKE @filter )
                                        AND tblFundSrc.b_IsActive = 'Y'
                                        AND tblFundSrc.n_FundSrcCodeID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

                FundSourceSqlDatasource.SelectParameters.Clear();
                FundSourceSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                FundSourceSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                FundSourceSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = FundSourceSqlDatasource;
                comboBox.DataBind();
            }
        }

        protected void ComboFundSource_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            FundSourceSqlDatasource.SelectCommand = @"SELECT  tblFundSrc.n_FundSrcCodeID ,
                                                            tblFundSrc.FundSrcCodeID ,
                                                            tblFundSrc.[Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblFundSrc.n_FundSrcCodeID ) AS [rn]
                                                    FROM    dbo.FundSrcCodes AS tblFundSrc
                                                    WHERE   ( n_FundSrcCodeID = @ID )
                                                    ORDER BY FundSrcCodeID";

            FundSourceSqlDatasource.SelectParameters.Clear();
            FundSourceSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = FundSourceSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboWorkOrder_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            WorkOrderSqlDatasource.SelectCommand =
                @"SELECT  n_WorkOrderCodeID ,
                            WorkOrderCodeID ,
                            [Description]
                    FROM    ( SELECT    tblWoC.n_WorkOrderCodeID ,
                                        tblWoC.WorkOrderCodeID ,
                                        tblWoC.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblWoC.n_WorkOrderCodeID ) AS [rn]
                              FROM      dbo.WorkOrderCodes AS tblWoC
                              WHERE     ( ( WorkOrderCodeID + ' ' + [Description] ) LIKE @filter )
                                        AND tblWoC.b_IsActive = 'Y'
                                        AND tblWoC.n_WorkOrderCodeID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            WorkOrderSqlDatasource.SelectParameters.Clear();
            WorkOrderSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            WorkOrderSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            WorkOrderSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = WorkOrderSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboWorkOrder_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            WorkOrderSqlDatasource.SelectCommand = @"SELECT  tblWoC.n_WorkOrderCodeID ,
                                                            tblWoC.WorkOrderCodeID ,
                                                            tblWoC.[Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblWoC.n_WorkOrderCodeID ) AS [rn]
                                                    FROM    dbo.WorkOrderCodes AS tblWoC
                                                    WHERE   ( n_WorkOrderCodeID = @ID )
                                                    ORDER BY WorkOrderCodeID";

            WorkOrderSqlDatasource.SelectParameters.Clear();
            WorkOrderSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = WorkOrderSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboWorkOp_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            WorkOpSqlDatasource.SelectCommand =
                @"SELECT  n_WorkOpID ,
                            WorkOpID ,
                            [Description]
                    FROM    ( SELECT    tblWorkOp.n_WorkOpID ,
                                        tblWorkOp.WorkOpID ,
                                        tblWorkOp.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblWorkOp.n_WorkOpID ) AS [rn]
                              FROM      dbo.WorkOperations AS tblWorkOp
                              WHERE     ( ( WorkOpID + ' ' + [Description] ) LIKE @filter )
                                        AND tblWorkOp.b_IsActive = 'Y'
                                        AND tblWorkOp.n_WorkOpID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            WorkOpSqlDatasource.SelectParameters.Clear();
            WorkOpSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            WorkOpSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            WorkOpSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = WorkOpSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboWorkOp_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            WorkOpSqlDatasource.SelectCommand = @"SELECT  tblWorkOp.n_WorkOpID ,
                                                            tblWorkOp.WorkOpID ,
                                                            tblWorkOp.[Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblWorkOp.n_WorkOpID ) AS [rn]
                                                    FROM    dbo.WorkOperations AS tblWorkOp
                                                    WHERE   ( n_WorkOpID = @ID )
                                                    ORDER BY WorkOpID";

            WorkOpSqlDatasource.SelectParameters.Clear();
            WorkOpSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = WorkOpSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboOrgCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            OrgCodeSqlDatasource.SelectCommand =
                @"SELECT  n_OrganizationCodeID ,
                                OrganizationCodeID ,
                                [Description]
                        FROM    ( SELECT    tblOrgCode.n_OrganizationCodeID ,
                                            tblOrgCode.OrganizationCodeID ,
                                            tblOrgCode.[Description] ,
                                            ROW_NUMBER() OVER ( ORDER BY tblOrgCode.n_OrganizationCodeID ) AS [rn]
                                  FROM      dbo.OrganizationCodes AS tblOrgCode
                                  WHERE     ( ( OrganizationCodeID + ' ' + [Description] ) LIKE @filter )
                                            AND tblOrgCode.b_IsActive = 'Y'
                                            AND tblOrgCode.n_OrganizationCodeID > 0
                                ) AS st
                        WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            OrgCodeSqlDatasource.SelectParameters.Clear();
            OrgCodeSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            OrgCodeSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            OrgCodeSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = OrgCodeSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboOrgCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            OrgCodeSqlDatasource.SelectCommand = @"SELECT  tblOrgCode.n_OrganizationCodeID ,
                                                        tblOrgCode.OrganizationCodeID ,
                                                        tblOrgCode.[Description] ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblOrgCode.n_OrganizationCodeID ) AS [rn]
                                                FROM    dbo.OrganizationCodes AS tblOrgCode
                                                WHERE   ( n_OrganizationCodeID = @ID )
                                                ORDER BY OrganizationCodeID";

            OrgCodeSqlDatasource.SelectParameters.Clear();
            OrgCodeSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = OrgCodeSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboFundGroup_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            FundGroupSqlDatasource.SelectCommand =
                @"SELECT  n_FundingGroupCodeID ,
                            FundingGroupCodeID ,
                            [Description]
                    FROM    ( SELECT    tblFGC.n_FundingGroupCodeID ,
                                        tblFGC.FundingGroupCodeID ,
                                        tblFGC.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblFGC.n_FundingGroupCodeID ) AS [rn]
                              FROM      dbo.FundingGroupCodes AS tblFGC
                              WHERE     ( ( FundingGroupCodeID + ' ' + [Description] ) LIKE @filter )
                                        AND tblFGC.b_IsActive = 'Y'
                                        AND tblFGC.n_FundingGroupCodeID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            FundGroupSqlDatasource.SelectParameters.Clear();
            FundGroupSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            FundGroupSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            FundGroupSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = FundGroupSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboFundGroup_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            FundGroupSqlDatasource.SelectCommand = @"SELECT  tblFGC.n_FundingGroupCodeID ,
                                                            tblFGC.FundingGroupCodeID ,
                                                            tblFGC.[Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblFGC.n_FundingGroupCodeID ) AS [rn]
                                                    FROM    dbo.FundingGroupCodes AS tblFGC
                                                    WHERE   ( n_FundingGroupCodeID = @ID )
                                                    ORDER BY FundingGroupCodeID";

            FundGroupSqlDatasource.SelectParameters.Clear();
            FundGroupSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = FundGroupSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboCtlSection_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CtlSectionSqlDatasource.SelectCommand =
                @"SELECT  n_ControlSectionID ,
                            ControlSectionID ,
                            [Description]
                    FROM    ( SELECT    tblCtlSec.n_ControlSectionID ,
                                        tblCtlSec.ControlSectionID ,
                                        tblCtlSec.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblCtlSec.n_ControlSectionID ) AS [rn]
                              FROM      dbo.ControlSections AS tblCtlSec
                              WHERE     ( ( ControlSectionID + ' ' + [Description] ) LIKE @filter )
                                        AND tblCtlSec.b_IsActive = 'Y'
                                        AND tblCtlSec.n_ControlSectionID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            CtlSectionSqlDatasource.SelectParameters.Clear();
            CtlSectionSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            CtlSectionSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            CtlSectionSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = CtlSectionSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboCtlSection_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CtlSectionSqlDatasource.SelectCommand = @"SELECT  tblCtlSec.n_ControlSectionID ,
                                                        tblCtlSec.ControlSectionID ,
                                                        tblCtlSec.[Description] ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblCtlSec.n_ControlSectionID ) AS [rn]
                                                FROM    dbo.ControlSections AS tblCtlSec
                                                WHERE   ( n_ControlSectionID = @ID )
                                                ORDER BY ControlSectionID";

            CtlSectionSqlDatasource.SelectParameters.Clear();
            CtlSectionSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = CtlSectionSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboEquipNum_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            EquipNumSqlDatasource.SelectCommand =
                @"SELECT  n_EquipmentNumberID ,
                            EquipmentNumberID ,
                            [Description]
                    FROM    ( SELECT    tblEquipNum.n_EquipmentNumberID ,
                                        tblEquipNum.EquipmentNumberID ,
                                        tblEquipNum.[Description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblEquipNum.n_EquipmentNumberID ) AS [rn]
                              FROM      dbo.EquipmentNumber AS tblEquipNum
                              WHERE     ( ( EquipmentNumberID + ' ' + [Description] ) LIKE @filter )
                                        AND tblEquipNum.b_IsActive = 'Y'
                                        AND tblEquipNum.n_EquipmentNumberID > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            EquipNumSqlDatasource.SelectParameters.Clear();
            EquipNumSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            EquipNumSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            EquipNumSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = EquipNumSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboEquipNum_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            EquipNumSqlDatasource.SelectCommand = @"SELECT  tblEquipNum.n_EquipmentNumberID ,
                                                        tblEquipNum.EquipmentNumberID ,
                                                        tblEquipNum.[Description] ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblEquipNum.n_EquipmentNumberID ) AS [rn]
                                                FROM    dbo.EquipmentNumber AS tblEquipNum
                                                WHERE   ( n_EquipmentNumberID = @ID )
                                                ORDER BY EquipmentNumberID";

            EquipNumSqlDatasource.SelectParameters.Clear();
            EquipNumSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = EquipNumSqlDatasource;
            comboBox.DataBind();
        }

        protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //Get Requestor
            var requestor = -1;
            if ((HttpContext.Current.Session["LogonInfo"] != null))
            {
                //Get Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
                requestor = _oLogon.UserID;
            }

            ASPxComboBox comboBox = (ASPxComboBox)source;
            ObjectDataSource.SelectCommand =
                @"DECLARE @areaFilteringOn VARCHAR(1)
                --Setup Area Filering Variable
                IF ( ( SELECT   COUNT(dbo.UsersAreaFilter.RecordID)
                       FROM     dbo.UsersAreaFilter WITH ( NOLOCK )
                       WHERE    UsersAreaFilter.UserID = " + requestor + @"
                                AND UsersAreaFilter.FilterActive = 'Y'
                     ) <> 0 )
                    BEGIN
                        SET @areaFilteringOn = 'Y'
                    END
                ELSE
                    BEGIN
                        SET @areaFilteringOn = 'N'
                    END

                SELECT  [n_objectid] ,
                        [objectid] ,
                        [description] ,
                        [areaid] ,
                        [locationid] ,
                        [assetnumber],
                        CASE ISNULL(RecordID, -1)
                          WHEN -1 THEN 'N'
                          ELSE 'Y'
                        END AS [Following] ,
                        isnull(LocationOrURL, '') AS LocationOrURL
                FROM    ( SELECT    tblmo.[n_objectid] ,
                                    tblmo.[objectid] ,
                                    tblmo.[description] ,
                                    tblarea.[areaid] ,
                                    tbllocation.[locationid] ,
                                    tblmo.[assetnumber] ,
                                    ROW_NUMBER() OVER ( ORDER BY tblmo.[n_objectid] ) AS [rn],
                                    tbl_IsFlaggedRecord.RecordID,
                                   tblFirstPhoto.LocationOrURL
                          FROM      dbo.MaintenanceObjects AS tblmo
                                    JOIN ( SELECT   tbl_Area.n_areaid ,
                                                    tbl_Area.areaid
                                           FROM     dbo.Areas tbl_Area
                                           WHERE    ( ( @areaFilteringOn = 'Y'
                                                        AND EXISTS ( SELECT recordMatches.AreaFilterID
                                                                     FROM   dbo.UsersAreaFilter AS recordMatches
                                                                     WHERE  tbl_Area.n_areaid = recordMatches.AreaFilterID
                                                                            AND recordMatches.UserID = " + requestor + @"
                                                                            AND recordMatches.FilterActive = 'Y' )
                                                      )
                                                      OR ( @areaFilteringOn = 'N' )
                                                    )
                                         ) tblarea ON tblmo.n_areaid = tblarea.n_areaid
                                    JOIN ( SELECT   n_locationid ,
                                                    locationid
                                           FROM     dbo.locations
                                         ) tbllocation ON tblmo.n_locationid = tbllocation.n_locationid
                                    LEFT JOIN ( SELECT  dbo.UsersFlaggedRecords.RecordID ,
                                                        dbo.UsersFlaggedRecords.n_objectid
                                                FROM    dbo.UsersFlaggedRecords
                                                WHERE   dbo.UsersFlaggedRecords.UserID = " + requestor + @"
                                                        AND dbo.UsersFlaggedRecords.n_objectid > 0
                                              ) tbl_IsFlaggedRecord ON tblmo.n_objectid = tbl_IsFlaggedRecord.n_objectid
                                    LEFT JOIN ( SELECT TOP 1 dbo.Attachments.LocationOrURL ,
                                                        dbo.Attachments.n_MaintObjectID
                                                FROM    dbo.Attachments
                                              ) tblFirstPhoto ON tblmo.n_objectid = tblFirstPhoto.n_MaintObjectID                                
                          WHERE     ( ( [objectid] + ' ' + [description] + ' ' + [areaid] + ' ' + [locationid] + ' ' + [assetnumber] ) LIKE @filter )
                                    AND tblmo.b_active = 'Y'
                                    AND tblmo.n_objectid > 0
                        ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            ObjectDataSource.SelectParameters.Clear();
            ObjectDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            ObjectDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            ObjectDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = ObjectDataSource;
            comboBox.DataBind();
        }

        protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            //Get Requestor
            var requestor = -1;
            if ((HttpContext.Current.Session["LogonInfo"] != null))
            {
                //Get Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
                requestor = _oLogon.UserID;
            }


            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            ObjectDataSource.SelectCommand = @"SELECT    tblmo.[n_objectid] ,
                                                            tblmo.[objectid] ,
                                                            tblmo.[description] ,
                                                            tblarea.[areaid] ,
					                                        tbllocation.[locationid] ,
					                                        tblmo.[assetnumber] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblmo.[n_objectid] ) AS [rn],
                                                            CASE ISNULL(RecordID, -1)
                                                                                      WHEN -1 THEN 'N'
                                                                                      ELSE 'Y'
                                                                                    END AS [Following],
                                                            isnull(LocationOrURL, '') AS LocationOrURL
                                                  FROM      dbo.MaintenanceObjects AS tblmo
			                                        JOIN ( SELECT   n_areaid ,
							                                        areaid
				                                           FROM     dbo.Areas
				                                         ) tblarea ON tblmo.n_areaid = tblarea.n_areaid
			                                        JOIN ( SELECT   n_locationid ,
							                                        locationid
				                                           FROM     dbo.locations
				                                         ) tbllocation ON tblmo.n_locationid = tbllocation.n_locationid
                                                    LEFT JOIN ( SELECT  dbo.UsersFlaggedRecords.RecordID ,
                                                            dbo.UsersFlaggedRecords.n_objectid
                                                    FROM    dbo.UsersFlaggedRecords
                                                    WHERE   dbo.UsersFlaggedRecords.UserID = " + requestor + @"
                                                            AND dbo.UsersFlaggedRecords.n_objectid > 0
                                                  ) tbl_IsFlaggedRecord ON tblmo.n_objectid = tbl_IsFlaggedRecord.n_objectid
                                                    LEFT JOIN ( SELECT TOP 1 dbo.Attachments.LocationOrURL ,
                                                            dbo.Attachments.n_MaintObjectID
                                                    FROM    dbo.Attachments
                                                  ) tblFirstPhoto ON tblmo.n_objectid = tblFirstPhoto.n_MaintObjectID                                                    
                                        WHERE (tblmo.n_objectid = @ID) ORDER BY objectid";

            ObjectDataSource.SelectParameters.Clear();
            ObjectDataSource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = ObjectDataSource;
            comboBox.DataBind();
        }

        protected void ComboServiceOffice_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            AreaSqlDatasource.SelectCommand =
                @"SELECT  n_areaid ,
                                areaid ,
                                [Description]
                        FROM    ( SELECT    tblAreas.n_areaid ,
                                            tblAreas.areaid ,
                                            tblAreas.descr AS [Description] ,
                                            ROW_NUMBER() OVER ( ORDER BY tblAreas.n_areaid ) AS [rn]
                                  FROM      dbo.Areas AS tblAreas
                                  WHERE     ( ( areaid + ' ' + [descr] ) LIKE @filter )
                                            AND tblAreas.b_IsActive = 'Y'
                                            AND tblAreas.n_areaid > 0
                                ) AS st
                        WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            AreaSqlDatasource.SelectParameters.Clear();
            AreaSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            AreaSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            AreaSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = AreaSqlDatasource;
            comboBox.DataBind();
        }

        protected void ComboServiceOffice_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            AreaSqlDatasource.SelectCommand = @"SELECT  tblAreas.n_areaid ,
                                                    tblAreas.areaid ,
                                                    tblAreas.descr AS [Description] ,
                                                    ROW_NUMBER() OVER ( ORDER BY tblAreas.n_areaid ) AS [rn]
                                            FROM    dbo.Areas AS tblAreas
                                            WHERE   ( n_areaid = @ID )
                                            ORDER BY areaid";

            AreaSqlDatasource.SelectParameters.Clear();
            AreaSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = AreaSqlDatasource;
            comboBox.DataBind();
        }

        /// <summary>
        /// Load Storeroom Parts Dropdown On Filter Criteria
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ComboStoreroomPart_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //Load Storeroom Parts Dropdown
            try
            {
                //Get Current Record
                var recordId = Convert.ToInt32(PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "n_masterpartid"));

                //Check Value
                if (recordId > 0)
                {
                    //Get Combo
                    var comboBox = (ASPxComboBox)source;

                    //Enable Combo
                    comboBox.Enabled = true;

                    //Set Command
                    StoreroomPartDS.SelectCommand =
                        string.Format(@"SELECT  n_storeroomid ,
                                                n_masterpartid ,
                                                Qty,
		                                        aisle,
		                                        shelf,
		                                        bin,
		                                        Storeroom,
		                                        [Storeroom Desc],
		                                        n_partatlocid
                                        FROM    ( SELECT    tbl_SR.n_storeroomid AS 'n_storeroomid' ,
					                                        tbl_PartsAtLoc.n_masterpartid AS 'n_masterpartid' ,
					                                        tbl_PartsAtLoc.qtyonhand AS 'Qty' ,
					                                        tbl_PartsAtLoc.aisle AS 'aisle' ,
					                                        tbl_PartsAtLoc.shelf AS 'shelf' ,
					                                        tbl_PartsAtLoc.bin AS 'bin' ,
					                                        tbl_SR.storeroomid AS 'Storeroom' ,
					                                        tbl_SR.descr AS 'Storeroom Desc' ,
					                                        tbl_PartsAtLoc.n_partatlocid AS 'n_partatlocid',
                                                            ROW_NUMBER() OVER ( ORDER BY tbl_SR.n_storeroomid ) AS [rn]
                                                  FROM    dbo.Storerooms tbl_SR
			                                        INNER JOIN ( SELECT dbo.PartsAtLocation.n_partatlocid ,
								                                        dbo.PartsAtLocation.n_masterpartid ,
								                                        dbo.PartsAtLocation.n_storeroomid ,
								                                        dbo.PartsAtLocation.qtyonhand ,
								                                        dbo.PartsAtLocation.aisle ,
								                                        dbo.PartsAtLocation.shelf ,
								                                        dbo.PartsAtLocation.bin ,
								                                        dbo.PartsAtLocation.IntLeadTime ,
								                                        dbo.PartsAtLocation.demand
						                                         FROM   dbo.PartsAtLocation
					                                           ) tbl_PartsAtLoc ON tbl_SR.n_storeroomid = tbl_PartsAtLoc.n_storeroomid
			                                        INNER JOIN ( SELECT dbo.Masterparts.n_masterpartid ,
								                                        dbo.Masterparts.masterpartid ,
								                                        dbo.Masterparts.Description ,
								                                        dbo.Masterparts.b_tool ,
								                                        dbo.Masterparts.b_active
						                                         FROM   dbo.Masterparts
						                                         WHERE dbo.Masterparts.n_masterpartid= {0}
					                                           ) tbl_MP ON tbl_PartsAtLoc.n_masterpartid = tbl_MP.n_masterpartid
                                                  WHERE     ( ( storeroomid + ' ' + [Description] + ' ' + [aisle] + ' ' + [shelf] + ' ' + [bin] ) LIKE @filter )
                                                            AND ( tbl_SR.n_storeroomid > 0 )
                                                            AND ( tbl_MP.b_tool = 'N' )
					                                        AND ( tbl_MP.b_active = 'Y' )
                                                ) AS st
                                        WHERE   st.[rn] BETWEEN @startIndex AND @endIndex", recordId);

                    StoreroomPartDS.SelectParameters.Clear();
                    StoreroomPartDS.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                    StoreroomPartDS.SelectParameters.Add("startIndex", TypeCode.Int64,
                        (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));
                    StoreroomPartDS.SelectParameters.Add("endIndex", TypeCode.Int64,
                        (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));
                    comboBox.DataSource = StoreroomPartDS;
                    comboBox.DataBind();
                }
                else
                {
                    //Disable Combo
                    ((ASPxComboBox)source).Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Loads Storeroom Parts Combo By ID
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ComboStoreroomPart_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            //Load Storeroom Part Combo BY ID
            try
            {
                long value;
                if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                    return;
                var comboBox = (ASPxComboBox)source;
                StoreroomPartDS.SelectCommand = @"SELECT    tbl_SR.n_storeroomid AS 'n_storeroomid' ,
					                                    tbl_PartsAtLoc.n_masterpartid AS 'n_masterpartid' ,
					                                    tbl_PartsAtLoc.qtyonhand AS 'Qty' ,
					                                    tbl_PartsAtLoc.aisle AS 'aisle' ,
					                                    tbl_PartsAtLoc.shelf AS 'shelf' ,
					                                    tbl_PartsAtLoc.bin AS 'bin' ,
					                                    tbl_SR.storeroomid AS 'Storeroom' ,
					                                    tbl_SR.descr AS 'Storeroom Desc' ,
					                                    tbl_PartsAtLoc.n_partatlocid AS 'n_partatlocid',
                                                        ROW_NUMBER() OVER ( ORDER BY tbl_SR.n_storeroomid ) AS [rn]
                                              FROM    dbo.Storerooms tbl_SR
			                                    INNER JOIN ( SELECT dbo.PartsAtLocation.n_partatlocid ,
								                                    dbo.PartsAtLocation.n_masterpartid ,
								                                    dbo.PartsAtLocation.n_storeroomid ,
								                                    dbo.PartsAtLocation.qtyonhand ,
								                                    dbo.PartsAtLocation.aisle ,
								                                    dbo.PartsAtLocation.shelf ,
								                                    dbo.PartsAtLocation.bin ,
								                                    dbo.PartsAtLocation.IntLeadTime ,
								                                    dbo.PartsAtLocation.demand
						                                     FROM   dbo.PartsAtLocation
						                                     WHERE dbo.Partsatlocation.n_partatlocid = @ID
					                                       ) tbl_PartsAtLoc ON tbl_SR.n_storeroomid = tbl_PartsAtLoc.n_storeroomid
			                                    INNER JOIN ( SELECT dbo.Masterparts.n_masterpartid ,
								                                    dbo.Masterparts.masterpartid ,
								                                    dbo.Masterparts.Description ,
								                                    dbo.Masterparts.b_tool ,
								                                    dbo.Masterparts.b_active
						                                     FROM   dbo.Masterparts
					                                       ) tbl_MP ON tbl_PartsAtLoc.n_masterpartid = tbl_MP.n_masterpartid";

                StoreroomPartDS.SelectParameters.Clear();
                StoreroomPartDS.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
                comboBox.DataSource = StoreroomPartDS;
                comboBox.DataBind();
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }
        #endregion

        #region Delete Setup for tab Grids
        /// <summary>
        /// Determines What Grid To Delete Items From
        /// </summary>
        private void DeleteItems()
        {
            //Determine Grid
            switch (StepTab.ActiveTabIndex)
            {
                case 0:
                    {
                        //Call Step Deletion Routine
                        JobStepDeletionRoutine();
                        break;
                    }
                case 1:
                    {
                        //Members
                        DeleteSelectedMembers();
                        break;
                    }
                case 2:
                    {
                        //Crew
                        DeleteSelectedCrew();
                        break;
                    }
                case 3:
                    {
                        //Parts
                        DeleteSelectedParts();
                        break;
                    }
                case 4:
                    {
                        //Equip
                        DeleteSelectedEquip();
                        break;
                    }
                case 5:
                    {
                        //Other
                        DeleteSelectedOther();
                        break;
                    }
                default:
                    {
                        //Do Nothing
                        break;
                    }
            }
        }

        /// <summary>
        /// Deletes Selected Crew Row
        /// </summary>
        private void DeleteSelectedCrew()
        {
            //Check For Multi Select Option
            if (((CrewGrid.Columns[0] as GridViewColumn).Visible))
            {
                //Get Selections
                var recordIdSelection = CrewGrid.GetSelectedFieldValues("RecordID");

                //Create Deletion Key
                var recordToDelete = -1;

                //Create Control Flags
                var continueDeletion = true;
                var deletionDone = false;

                //Process Multi Selection
                foreach (var selection in recordIdSelection)
                {
                    //Get ID
                    recordToDelete = Convert.ToInt32(selection.ToString());

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJobCrew.ClearErrors();

                        //Delete Jobstep
                        if (_oJobCrew.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                        else
                        {
                            //Set Flag
                            continueDeletion = false;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        CrewGrid.DataBind();
                    }
                }
            }
            else
            {
                //Check Selection
                if (Selection.Contains("RecordID"))
                {
                    //Get Selected Row
                    var recordIdSelection = Convert.ToInt32(Selection.Get("RecordID"));

                    //Check Permissions
                    if (_userCanEdit)
                    {
                        //Get Job Step ID
                        var jobStepId = -1;
                        if ((HttpContext.Current.Session["editingJobStepID"] != null))
                        {
                            //Get Info From Session
                            jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                        }

                        //Get Job ID
                        var jobId = -1;
                        if ((HttpContext.Current.Session["editingJobID"] != null))
                        {
                            //Get Info From Session
                            jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                        }

                        //Get DMR Key
                        var dmrKey = Convert.ToInt32(Selection.Get("DMRKEY"));

                        //Make Sure It Isn't Linked
                        if (dmrKey < 0)
                        {
                            //Clear Errors
                            _oJobCrew.ClearErrors();

                            //Delete Crew
                            if (_oJobCrew.Delete(recordIdSelection))
                            {
                                //Refresh Grid
                                CrewGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes Selected Parts
        /// </summary>
        private void DeleteSelectedParts()
        {
            //Check For Multi Select Option
            if ((PartGrid.Columns[0].Visible))
            {
                //Get Selections
                var recordIdSelection = PartGrid.GetSelectedFieldValues("n_jobpartid");

                //Create Deletion Key
                var recordToDelete = -1;

                //Create Control Flags
                var continueDeletion = true;
                var deletionDone = false;

                //Process Multi Selection
                foreach (var selection in recordIdSelection)
                {
                    //Get ID
                    recordToDelete = Convert.ToInt32(selection.ToString());

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJobParts.ClearErrors();

                        //Delete Jobstep
                        if (_oJobParts.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                        else
                        {
                            //Set Flag
                            continueDeletion = false;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        PartGrid.DataBind();
                    }
                }
            }
            else
            {
                //Check Selection
                if (Selection.Contains("n_jobpartid"))
                {
                    //Get Selected Row
                    var recordIdSelection = Convert.ToInt32(Selection.Get("n_jobpartid"));

                    //Check Permissions
                    if (_userCanEdit)
                    {
                        //Get Job Step ID
                        var jobStepId = -1;
                        if ((HttpContext.Current.Session["editingJobStepID"] != null))
                        {
                            //Get Info From Session
                            jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                        }

                        //Get Job ID
                        var jobId = -1;
                        if ((HttpContext.Current.Session["editingJobID"] != null))
                        {
                            //Get Info From Session
                            jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                        }

                        //Get DMR Key
                        var dmrKey = Convert.ToInt32(Selection.Get("DMRKEY"));

                        //Make Sure It Isn't Linked
                        if (dmrKey < 0)
                        {
                            //Clear Errors
                            _oJobParts.ClearErrors();

                            //Delete Part
                            if (_oJobParts.Delete(recordIdSelection))
                            {
                                //Refresh Grid
                                PartGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes Selected Equip
        /// </summary>
        private void DeleteSelectedEquip()
        {
            //Check For Multi Select Option
            if (((EquipGrid.Columns[0] as GridViewColumn).Visible))
            {
                //Get Selections
                var recordIdSelection = EquipGrid.GetSelectedFieldValues("n_JobEquipmentID");

                //Create Deletion Key
                var recordToDelete = -1;

                //Create Control Flags
                var continueDeletion = true;
                var deletionDone = false;

                //Process Multi Selection
                foreach (var selection in recordIdSelection)
                {
                    //Get ID
                    recordToDelete = Convert.ToInt32(selection.ToString());

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJobEquipment.ClearErrors();

                        //Delete Jobstep
                        if (_oJobEquipment.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                        else
                        {
                            //Set Flag
                            continueDeletion = false;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        EquipGrid.DataBind();
                    }
                }
            }
            else
            {
                //Check Selection
                if (Selection.Contains("n_JobEquipmentID"))
                {
                    //Get Selected Row
                    var recordIdSelection = Convert.ToInt32(Selection.Get("n_JobEquipmentID"));

                    //Check Permissions
                    if (_userCanEdit)
                    {
                        //Get DMR Key
                        var dmrKey = Convert.ToInt32(Selection.Get("DMRKEY"));

                        //Make Sure It Isn't Linked
                        if (dmrKey < 0)
                        {
                            //Clear Errors
                            _oJobEquipment.ClearErrors();

                            //Delete Part
                            if (_oJobEquipment.Delete(recordIdSelection))
                            {
                                //Refresh Grid
                                EquipGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes Selected Other
        /// </summary>
        private void DeleteSelectedOther()
        {
            //Check For Multi Select Option
            if (((OtherGrid.Columns[0] as GridViewColumn).Visible))
            {
                //Get Selections
                var recordIdSelection = OtherGrid.GetSelectedFieldValues("n_JobOtherID");

                //Create Deletion Key
                var recordToDelete = -1;

                //Create Control Flags
                var continueDeletion = true;
                var deletionDone = false;

                //Process Multi Selection
                foreach (var selection in recordIdSelection)
                {
                    //Get ID
                    recordToDelete = Convert.ToInt32(selection.ToString());

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJobOther.ClearErrors();

                        //Delete Jobstep
                        if (_oJobOther.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                        else
                        {
                            //Set Flag
                            continueDeletion = false;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        OtherGrid.DataBind();
                    }
                }
            }
            else
            {
                //Check Selection
                if (Selection.Contains("n_JobOtherID"))
                {
                    //Get Selected Row
                    var recordIdSelection = Convert.ToInt32(Selection.Get("n_JobOtherID"));

                    //Check Permissions
                    if (_userCanEdit)
                    {
                        //Clear Errors
                        _oJobOther.ClearErrors();

                        //Delete Part
                        if (_oJobOther.Delete(recordIdSelection))
                        {
                            //Refresh Grid
                            OtherGrid.DataBind();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes Selected Members
        /// </summary>
        private void DeleteSelectedMembers()
        {
            //Check For Multi Select Option
            if (((MemberGrid.Columns[0] as GridViewColumn).Visible))
            {
                //Get Selections
                var recordIdSelection = MemberGrid.GetSelectedFieldValues("n_JobOtherID");

                //Create Deletion Key
                var recordToDelete = -1;

                //Create Control Flags
                var continueDeletion = true;
                var deletionDone = false;

                //Process Multi Selection
                foreach (var selection in recordIdSelection)
                {
                    //Get ID
                    recordToDelete = Convert.ToInt32(selection.ToString());

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJobMembers.ClearErrors();

                        //Delete Jobstep
                        if (_oJobMembers.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                        else
                        {
                            //Set Flag
                            continueDeletion = false;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        MemberGrid.DataBind();
                    }
                }
            }
            else
            {
                //Check Selection
                if (Selection.Contains("n_JobOtherID"))
                {
                    //Get Selected Row
                    var recordIdSelection = Convert.ToInt32(Selection.Get("n_JobOtherID"));

                    //Check Permissions
                    if (_userCanEdit)
                    {
                        //Clear Errors
                        _oJobMembers.ClearErrors();

                        //Delete Crew
                        if (_oJobMembers.Delete(recordIdSelection))
                        {
                            //Refresh Grid
                            MemberGrid.DataBind();
                        }
                    }
                }
            }
        }

        public void DeleteGridViewAttachment()
        {
            for (int i = 0; i < AttachmentGrid.VisibleRowCount; i++)
            {
                if (AttachmentGrid.GetRowLevel(i) == AttachmentGrid.GroupCount)
                {
                    object keyValue = AttachmentGrid.GetRowValues(i, new string[] { "ID" });
                    var id = Convert.ToInt32(keyValue.ToString());
                    if (keyValue != null)

                        _oAttachments.Delete(id);
                }
            }
        }

        #endregion

        #region Add Setup for tab Grids
        /// <summary>
        /// Determins Grid Calling And Opens Up Popup For Selection
        /// </summary>
        private void AddItems()
        {
            //Determine Grid
            switch (StepTab.ActiveTabIndex)
            {
                case 0:
                    {
                        //Steps

                        //Call Add New Step Routine
                        NewJobStepRoutine();
                        break;
                    }
                case 1:
                    {
                        //Members

                        //Bind Grid
                        MemberLookupGrid.DataBind();

                        //Show Popup
                        AddMemberPopup.ShowOnPageLoad = true;
                        break;
                    }
                case 2:
                    {
                        //Crew Lookup

                        //Bind Grid
                        CrewLookupGrid.DataBind();

                        //Show Popup
                        AddCrewPopup.ShowOnPageLoad = true;
                        break;
                    }
                case 3:
                    {
                        //Part Lookup

                        //Bind Grid
                        PartLookupGrid.DataBind();

                        //Show Popup
                        AddPartPopup.ShowOnPageLoad = true;
                        break;
                    }
                case 4:
                    {
                        //Equipment Lookup

                        //Bind Grid
                        EquipLookupGrid.DataBind();

                        //Show Popup
                        AddEquipPopup.ShowOnPageLoad = true;
                        break;
                    }
                case 5:
                    {
                        //Other

                        //Show Edit/Add Template
                        OtherGrid.AddNewRow();
                        break;
                    }
                default:
                    {
                        //Do Nothing
                        break;
                    }
            }
        }

        /// <summary>
        /// Adds Selected Crew For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddCrew_Click(object sender, EventArgs e)
        {
            //Add Selected Crew For Job Step
            var recordIdSelection = CrewLookupGrid.GetSelectedFieldValues("nUserID");

            //Check Count
            if (recordIdSelection.Count > 0)
            {
                //Check Permissions
                if (_userCanEdit)
                {
                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["editingJobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                    }

                    //Get Job ID
                    var jobId = -1;
                    if ((HttpContext.Current.Session["editingJobID"] != null))
                    {
                        //Get Info From Session
                        jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    }

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Create Control Variable
                        var addedCrewMember = false;

                        //Loop Selections
                        for (var rowIndex = 0; rowIndex < recordIdSelection.Count; rowIndex++)
                        {
                            //Get ID
                            var crewMemberId = Convert.ToInt32(recordIdSelection[rowIndex].ToString());
                            var defaultLaborId = -1;
                            var defaultPayCodeId = -1;
                            var defaultShiftId = -1;


                            #region Load Crew Specific Information

                            var userLaborId = "";
                            var userLaborDesc = "";
                            var tmpLaborClass = 0;
                            var gotRates = true;

                            //Get Default Labor
                            if (_oMpetUser.GetUsersLaborSettingText(crewMemberId, ref tmpLaborClass, ref userLaborId,
                                ref userLaborDesc))
                            {
                                defaultLaborId = tmpLaborClass;
                            }

                            //Load Users
                            if (_oMpetUser.LoadData())
                            {
                                //Check Table
                                if (_oMpetUser.Ds.Tables.Count > 0)
                                {
                                    //  0    [UserID], 
                                    //  1    [Username], 
                                    //  2    [FirstName], 
                                    //  3    [LastName], 
                                    //  4    [Password], 
                                    //  5    [AreaID], 
                                    //  6    [WorkPhone], 
                                    //  7    [CellPhone], 
                                    //  8    [PayrollID], 
                                    //  9    [PositionCodeID], 
                                    //  10   [PersonClassID], 
                                    //  11   [LocationID], 
                                    //  12   [PasswordExpireDate], 
                                    //  13   [PasswordExpires], 
                                    //  14   [PasswordDayCount], 
                                    //  15   [Notes], 
                                    //  16   [Active], 
                                    //  17   [LaborClassID], 
                                    //  18   [GroupID], 
                                    //  19   [FundID], 
                                    //  20   [CompanyDate], 
                                    //  21   [PlantDate], 
                                    //  22   [EntryDate],
                                    //  23   [CanLogon],
                                    //  24   n_shiftid,
                                    //  25   MondayHrs,
                                    //  26   TuesdayHrs,
                                    //  27   WednesdayHrs,
                                    //  28   ThursdayHrs,
                                    //  29   FridayHrs,
                                    //  31   SaturdayHrs,
                                    //  32   SundayHrs
                                    var dv = new DataView(_oMpetUser.Ds.Tables[0]) { RowFilter = "UserID=" + crewMemberId };
                                    if (dv.Count == 1)
                                    {
                                        var drv = dv[0];
                                        var row = drv.Row;
                                        defaultShiftId = (int)row[24];
                                    }
                                }
                            }

                            #endregion

                            //Create Rate Variables
                            decimal straightTimeRate = 0;
                            decimal overtimeRate = 0;
                            decimal otherRate = 0;

                            //Clear Errors
                            _oJobCrew.ClearErrors();

                            //Get Adjusted Rates
                            if (_oJobCrew.GetAdjustedUserRate(crewMemberId,
                                defaultShiftId,
                                defaultLaborId,
                                ref straightTimeRate,
                                ref overtimeRate,
                                ref otherRate))
                            {
                                //Create Date Worked
                                var dateWorked = _nullDate;

                                //If Completion Date Is Set Use It Instead
                                //if ((txtJobstepDateCompleted.Value != null) &&
                                //    (txtJobstepDateCompleted.Value.ToString() != ""))
                                //{
                                //    _editingJcWorkDate = Convert.ToDateTime(txtJobstepDateCompleted.Value.ToString());
                                //}

                                ////Get Job Hour Estimates From First Tab
                                //decimal EstHrs = 0;

                                //if ((this.txtEstimatedJobLen.Value != null) && (this.txtEstimatedJobLen.Value.ToString() != ""))
                                //{
                                //    EstHrs = Convert.ToDecimal(this.txtEstimatedJobLen.Value.ToString());
                                //}

                                _oJobCrew.ClearErrors();
                                if (_oJobCrew.Add(jobId,
                                    jobStepId,
                                    crewMemberId,
                                    defaultLaborId,
                                    -1,
                                    defaultShiftId,
                                    0,
                                    straightTimeRate,
                                    0,
                                    0,
                                    0,
                                    dateWorked,
                                    _oLogon.UserID,
                                    -1,
                                    -1))
                                {
                                    addedCrewMember = true;
                                }
                            }
                        }

                        //Check Flag
                        if (addedCrewMember)
                        {
                            //Refresh Crew Grid
                            CrewGrid.DataBind();

                            //Clear Selection
                            CrewLookupGrid.Selection.UnselectAll();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds Selected Equipment For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddEquip_Click(object sender, EventArgs e)
        {
            //Add Selected Crew For Job Step
            var recordIdSelection = EquipLookupGrid.GetSelectedFieldValues("n_objectid");

            //Check Count
            if (recordIdSelection.Count > 0)
            {
                //Check Permissions
                if (_userCanEdit)
                {
                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["editingJobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                    }

                    //Get Job ID
                    var jobId = -1;
                    if ((HttpContext.Current.Session["editingJobID"] != null))
                    {
                        //Get Info From Session
                        jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    }

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Create Control Variable
                        var addedEquipment = false;

                        //Loop Selections
                        for (var rowIndex = 0; rowIndex < recordIdSelection.Count; rowIndex++)
                        {
                            //Get ID
                            var maintId = Convert.ToInt32(recordIdSelection[rowIndex].ToString());

                            //Set Temp Date
                            var tmpJobEquipDate = DateTime.Now;

                            //Get Cost
                            var equipmentCost =
                                Convert.ToDecimal(
                                    EquipLookupGrid.GetFilteredSelectedValues("ChargeRate")[rowIndex].ToString());

                            //Get Description
                            var equipDesc =
                                EquipLookupGrid.GetFilteredSelectedValues("description")[rowIndex].ToString();

                            //Create Date Worked
                            var dateWorked = _nullDate;

                            //If Completion Date Is Set Use It Instead
                            //if ((txtJobstepDateCompleted.Value != null) &&
                            //    (txtJobstepDateCompleted.Value.ToString() != ""))
                            //{
                            //    tmpJobEquipDate = Convert.ToDateTime(txtJobstepDateCompleted.Value.ToString());
                            //}

                            //Clear Errors
                            _oJobEquipment.ClearErrors();

                            //Add Item
                            if (_oJobEquipment.Add(jobId,
                                jobStepId,
                                maintId,
                                equipmentCost,
                                equipDesc,
                                "",
                                0,
                                0,
                                tmpJobEquipDate,
                                -1,
                                _oLogon.UserID,
                                -1,
                                -1))
                            {
                                //Equipment Added Set Flag
                                addedEquipment = true;
                            }
                        }

                        //Check Flag
                        if (addedEquipment)
                        {
                            //Refresh Crew Grid
                            EquipGrid.DataBind();

                            //Clear Selection
                            EquipLookupGrid.Selection.UnselectAll();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds Selected Members For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddMember_Click(object sender, EventArgs e)
        {
            //Add Selected Crew For Job Step
            var recordIdSelection = MemberLookupGrid.GetSelectedFieldValues("n_objectid");

            //Check Count
            if (recordIdSelection.Count > 0)
            {
                //Check Permissions
                if (_userCanEdit)
                {
                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["editingJobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                    }

                    //Get Job ID
                    var jobId = -1;
                    if ((HttpContext.Current.Session["editingJobID"] != null))
                    {
                        //Get Info From Session
                        jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    }

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Create Control Variable
                        var addedMember = false;

                        //Loop Selections
                        for (var rowIndex = 0; rowIndex < recordIdSelection.Count; rowIndex++)
                        {
                            //Get ID
                            var tmpObjectId = Convert.ToInt32(recordIdSelection[rowIndex].ToString());

                            //Set Temp Date
                            var tmpWorkDate = DateTime.Now;

                            //Check If Job Completion Date
                            //if ((txtJobstepDateCompleted.Value != null) &&
                            //    (txtJobstepDateCompleted.Value.ToString() != ""))
                            //{
                            //    //Set Date
                            //    tmpWorkDate = Convert.ToDateTime(txtJobstepDateCompleted.Value.ToString());
                            //}
                            //else if ((txtJSStartDate.Value != null) && (txtJSStartDate.Value.ToString() != ""))
                            //{
                            //    //Check If Job Start Date Exists
                            //    tmpWorkDate = Convert.ToDateTime(txtJSStartDate.Value.ToString());
                            //}


                            //Clear Errors
                            _oJobMembers.ClearErrors();

                            //Add Record
                            if (
                                _oJobMembers.Add(jobId, -1, tmpObjectId, true, tmpWorkDate,
                                    _oLogon.UserID))
                            {
                                //Member Added Set Flag
                                addedMember = true;
                            }
                        }

                        //Check Flag
                        if (addedMember)
                        {
                            //Refresh Member Grid
                            MemberGrid.DataBind();

                            //Clear Selection
                            MemberLookupGrid.Selection.UnselectAll();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds Selected Parts For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddPart_Click(object sender, EventArgs e)
        {
            try
            {
                //Check Permissions
                if (_userCanEdit)
                {
                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["editingJobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                    }

                    //Get Job ID
                    var jobId = -1;
                    if ((HttpContext.Current.Session["editingJobID"] != null))
                    {
                        //Get Info From Session
                        jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    }

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Get Equipment Selection
                        var recordIdSelection = PartLookupGrid.GetSelectedFieldValues(new[] { "n_masterpartid", "masterpartid", "Description", "listcost", "cPrefMfg", "n_PrefMFGPartID" });

                        //Set Job ID/Step
                        _oJobParts.SetJobstepInfo(jobId, jobStepId);

                        //Process Multi Selection
                        if ((from object[] partSelections in recordIdSelection
                             let partId = Convert.ToInt32(partSelections[0].ToString())
                             let editingJpnsPartId = partSelections[1].ToString()
                             let editingJpnsPartDesc = partSelections[2].ToString()
                             let editingJpnsPartCost = Convert.ToDecimal(partSelections[3].ToString())
                             let cPreferedMfgPartId = partSelections[4].ToString()
                             let editingJpMfgPartId = Convert.ToInt32(partSelections[5].ToString())
                             where !_oJobParts.Add(partId,
                                                     -1,
                                                     false,
                                                     false,
                                                     "",
                                                     editingJpnsPartId,
                                                     editingJpnsPartCost,
                                                     cPreferedMfgPartId,
                                                     editingJpnsPartDesc,
                                                     0,
                                                     0,
                                                     DateTime.Now.Date,
                                                     editingJpMfgPartId,
                                                     -1,
                                                     _oLogon.UserID,
                                                     -1,
                                                     -1)
                             select partId).Any())
                        {
                            //Throw Error
                            throw new SystemException(@"Error Adding Stock Master Part  - " + _oJobParts.LastError);
                        }

                        //Perform Refresh
                        PartGrid.DataBind();
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
        /// Adds New Nonstock Part
        /// </summary>
        protected void NonStockPartRoutine()
        {
            //Add New Nonstock Part
            try
            {
                //Check Permissions
                if (_userCanAdd)
                {
                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["editingJobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                    }

                    //Get Job ID
                    var jobId = -1;
                    if ((HttpContext.Current.Session["editingJobID"] != null))
                    {
                        //Get Info From Session
                        jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    }

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Set Job ID/Step
                        _oJobParts.SetJobstepInfo(jobId, jobStepId);

                        //Clear Errors
                        _oJobParts.ClearErrors();

                        //Add Non Stock Part
                        if (_oJobParts.Add(-1,
                            -1,
                            false,
                            true,
                            "",
                            "N/A",
                            0,
                            "",
                            "",
                            0,
                            0,
                            DateTime.Now.Date,
                            -1,
                            -1,
                            _oLogon.UserID,
                            -1,
                            -1))
                        {
                            //Refresh Parts Grid
                            PartGrid.DataBind();
                        }
                        else
                        {
                            //Throw Error
                            throw new SystemException(
                                @"Error Adding Non-Stock Part - " + _oJobParts.LastError);
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
        #endregion

        #region Updating Setup for tab Grids
        /// <summary>
        /// Determins Grid Calling And Opens Up Edit Template For Edits
        /// </summary>
        private void EditItems()
        {
            //Determine Grid
            switch (StepTab.ActiveTabIndex)
            {
                case 0:
                    {
                        //Steps
                        break;
                    }
                case 1:
                    {
                        //Members
                        //Check Selection
                        if (MemberGrid.FocusedRowIndex >= 0)
                        {
                            //Get Record ID
                            var recordId = Convert.ToInt32(MemberGrid.FocusedRowIndex);

                            //Show Edit/Add Template
                            MemberGrid.StartEdit(recordId);
                        }
                        break;
                    }
                case 2:
                    {
                        //Crew Lookup
                        //Check Selection
                        if (CrewGrid.FocusedRowIndex >= 0)
                        {
                            //Get Record ID
                            var recordId = Convert.ToInt32(CrewGrid.FocusedRowIndex);

                            //Show Edit/Add Template
                            CrewGrid.StartEdit(recordId);
                        }
                        break;

                    }
                case 3:
                    {
                        //Parts
                        //Check Selection
                        if (PartGrid.FocusedRowIndex >= 0)
                        {
                            //Get Record ID
                            var recordId = Convert.ToInt32(PartGrid.FocusedRowIndex);

                            //Show Edit/Add Template
                            PartGrid.StartEdit(recordId);
                        }
                        break;
                    }
                case 4:
                    {
                        //Equipment Lookup
                        //Check Selection
                        if (EquipGrid.FocusedRowIndex >= 0)
                        {
                            //Get Record ID
                            var recordId = Convert.ToInt32(EquipGrid.FocusedRowIndex);

                            //Show Edit/Add Template
                            EquipGrid.StartEdit(recordId);
                        }
                        break;
                    }
                case 5:
                    {
                        //Other

                        //Check Selection
                        if (OtherGrid.FocusedRowIndex >= 0)
                        {
                            //Get Record ID
                            var recordId = Convert.ToInt32(OtherGrid.FocusedRowIndex);

                            //Show Edit/Add Template
                            OtherGrid.StartEdit(recordId);
                        }
                        break;
                    }
                default:
                    {
                        //Do Nothing
                        break;
                    }
            }
        }

        /// <summary>
        /// Saves Other Job Record Changes From Row Edit Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OtherGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Check Permissions
            if (_userCanEdit)
            {
                //Get Job Step ID
                var jobStepId = -1;
                if ((HttpContext.Current.Session["editingJobStepID"] != null))
                {
                    //Get Info From Session
                    jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                }

                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Make Sure We Have A Selection
                if (OtherGrid.EditingRowVisibleIndex >= 0)
                {
                    //Set Cancel Flag
                    e.Cancel = true;

                    //Get Editing Record ID
                    var recordId = -1;
                    recordId = Convert.ToInt32(OtherGrid.GetRowValues(OtherGrid.EditingRowVisibleIndex, "n_JobOtherID"));

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Check Editing Mode
                        if (OtherGrid.SettingsEditing.Mode == GridViewEditingMode.PopupEditForm)
                        {
                            //Check For Valid Description
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherDesc")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherDesc")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != ""))
                            {
                                //Get Description
                                var descToEdit =
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherDesc")
                                        as LayoutItem).GetNestedControl() as ASPxButtonEdit).Text;

                                //Check/Get Est Units
                                decimal estToEdit = 0;
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherEstUnits")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherEstUnits")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                                {

                                    estToEdit = Convert.ToDecimal(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                                "liOtherEstUnits")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                                }

                                //Check/Get Act Units
                                decimal actToEdit = 0;
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherActUnits")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherActUnits")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                                {

                                    actToEdit = Convert.ToDecimal(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                                "liOtherActUnits")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                                }

                                //Get Cost
                                decimal costToEdit = 0;
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherCost")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherCost")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                                {

                                    costToEdit = Convert.ToDecimal(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                                "liOtherCost")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                                }

                                //Get Date Worked
                                var dateWorkedToEdit = _nullDate;
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text != ""))
                                {

                                    dateWorkedToEdit = Convert.ToDateTime(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                                "liOtherDate")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxDateEdit).Text);
                                }

                                //Get Misc Ref
                                var miscRefToEdit = "";
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherMisc")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liOtherMisc")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != ""))
                                {

                                    miscRefToEdit =
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                                "liOtherMisc")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxButtonEdit).Text;
                                }

                                //Save Changes
                                _oJobOther.ClearErrors();

                                //Add Other Record
                                if (_oJobOther.Update(recordId,
                                    jobId,
                                    jobStepId,
                                    descToEdit,
                                    costToEdit,
                                    miscRefToEdit,
                                    actToEdit,
                                    estToEdit,
                                    dateWorkedToEdit,
                                    _oLogon.UserID))
                                {
                                    //Cacnel Edit (Hides Template)
                                    OtherGrid.CancelEdit();

                                    //Refresh Part Grid                        
                                    OtherGrid.DataBind();
                                }
                            }
                        }
                        else
                        {

                            //Get Description
                            var descToEdit = e.NewValues["OtherDescr"].ToString();

                            //Get Est Units
                            var estToEdit = Convert.ToDecimal(e.NewValues["qtyplanned"]);

                            //Get Act Units
                            var actToEdit = Convert.ToDecimal(e.NewValues["qtyused"]);

                            //Get Cost
                            var costToEdit = Convert.ToDecimal(e.NewValues["OtherCost"]);

                            //Get Work Date
                            var dateWorkedToEdit = _nullDate;
                            if (e.NewValues["WorkDate"].ToString() != "")
                            {
                                //Get Date
                                dateWorkedToEdit = Convert.ToDateTime(e.NewValues["WorkDate"]);
                            }

                            //Get Misc Ref
                            var miscRefToEdit = e.NewValues["MiscellaneousReference"].ToString();

                            //Save Changes
                            _oJobOther.ClearErrors();

                            //Add Other Record
                            if (_oJobOther.Update(recordId,
                                jobId,
                                jobStepId,
                                descToEdit,
                                costToEdit,
                                miscRefToEdit,
                                actToEdit,
                                estToEdit,
                                dateWorkedToEdit,
                                _oLogon.UserID))
                            {
                                //Cacnel Edit (Hides Template)
                                OtherGrid.CancelEdit();

                                //Refresh Part Grid                        
                                OtherGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// Saves Other Job Record Changes From Row Edit Template
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void OtherGrid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        //{
        //    //Check Permissions
        //    if (_userCanEdit)
        //    {
        //        //Set Handled Flag
        //        e.Handled = true;

        //        //Get Job Step ID
        //        var jobStepId = -1;
        //        if ((HttpContext.Current.Session["editingJobStepID"] != null))
        //        {
        //            //Get Info From Session
        //            jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
        //        }

        //        //Get Job ID
        //        var jobId = -1;
        //        if ((HttpContext.Current.Session["editingJobID"] != null))
        //        {
        //            //Get Info From Session
        //            jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
        //        }

        //        //Check IDs
        //        if ((jobStepId > 0) && (jobId > 0))
        //        {
        //            //Make Sure We Have Updated Values
        //            if (e.UpdateValues.Count > 0)
        //            {
        //                //Loop Items
        //                foreach (ASPxDataUpdateValues item in e.UpdateValues)
        //                {
        //                    //Get Record ID
        //                    var recordId = Convert.ToInt32(item.Keys["n_JobOtherID"]);

        //                    //Get Description
        //                    var descToEdit = item.NewValues["OtherDescr"].ToString();

        //                    //Get Est Units
        //                    var estToEdit  = Convert.ToDecimal(item.NewValues["qtyplanned"]);

        //                    //Get Act Units
        //                    var actToEdit  = Convert.ToDecimal(item.NewValues["qtyused"]);

        //                    //Get Cost
        //                    var costToEdit  = Convert.ToDecimal(item.NewValues["OtherCost"]);

        //                    //Get Work Date
        //                    var dateWorkedToEdit  = Convert.ToDateTime(item.NewValues["WorkDate"]);

        //                    //Get Misc Ref
        //                    var miscRefToEdit = item.NewValues["MiscellaneousReference"].ToString();

        //                    //Save Changes
        //                    _oJobOther.ClearErrors();

        //                        //Add Other Record
        //                    if (_oJobOther.Update(recordId,
        //                        jobId,
        //                        jobStepId,
        //                        descToEdit,
        //                        costToEdit,
        //                        miscRefToEdit,
        //                        actToEdit,
        //                        estToEdit,
        //                        dateWorkedToEdit,
        //                        _oLogon.UserID))
        //                    {
        //                        //Cacnel Edit (Hides Template)
        //                        OtherGrid.CancelEdit();

        //                        //Refresh Part Grid                        
        //                        OtherGrid.DataBind();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Adds New Jobstep Other Record From Row Edit Template
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void OtherGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //Check Permissions
            if (_userCanEdit)
            {
                //Set Cancel Flag
                e.Cancel = true;


                //Get Job Step ID
                var jobStepId = -1;
                if ((HttpContext.Current.Session["editingJobStepID"] != null))
                {
                    //Get Info From Session
                    jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                }

                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Check IDs
                if ((jobStepId > 0) && (jobId > 0))
                {
                    //Check For Valid Description
                    if (
                        (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName("liOtherDesc") as
                            LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != null)
                        &&
                        (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                            .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName("liOtherDesc") as
                            LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != ""))
                    {
                        //Get Description
                        var descToAdd =
                            ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName("liOtherDesc")
                                as LayoutItem).GetNestedControl() as ASPxButtonEdit).Text;

                        //Check/Get Est Units
                        decimal estToAdd = 0;
                        if (
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName("liOtherEstUnits")
                                as
                                LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                            &&
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName("liOtherEstUnits")
                                as
                                LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                        {

                            estToAdd = Convert.ToDecimal(
                                ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherEstUnits")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                        }

                        //Check/Get Act Units
                        decimal actToAdd = 0;
                        if (
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherActUnits")
                                as
                                LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                            &&
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherActUnits")
                                as
                                LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                        {

                            actToAdd = Convert.ToDecimal(
                                ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherActUnits")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                        }

                        //Get Cost
                        decimal costToAdd = 0;
                        if (
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherCost")
                                as
                                LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                            &&
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherCost")
                                as
                                LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                        {

                            costToAdd = Convert.ToDecimal(
                                ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherCost")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                        }

                        //Get Date Worked
                        var dateWorkedToAdd = _nullDate;
                        if (
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherDate")
                                as
                                LayoutItem).GetNestedControl() as ASPxDateEdit).Text != null)
                            &&
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherDate")
                                as
                                LayoutItem).GetNestedControl() as ASPxDateEdit).Text != ""))
                        {

                            dateWorkedToAdd = Convert.ToDateTime(
                                ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text);
                        }

                        //Get Misc Ref
                        var miscRefToAdd = "";
                        if (
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherMisc")
                                as
                                LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != null)
                            &&
                            (((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as ASPxFormLayout)
                                .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                    "liOtherMisc")
                                as
                                LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != ""))
                        {

                            miscRefToAdd =
                                ((((((ASPxGridView)sender).FindEditFormTemplateControl("OtherEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("OtherEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liOtherMisc")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxButtonEdit).Text;
                        }

                        //Save Changes
                        _oJobOther.ClearErrors();

                        //Add Other Record
                        if (_oJobOther.Add(jobId,
                            jobStepId,
                            descToAdd,
                            costToAdd,
                            miscRefToAdd,
                            actToAdd,
                            estToAdd,
                            dateWorkedToAdd,
                            -1,
                            _oLogon.UserID,
                            -1,
                            -1))
                        {
                            //Cacnel Edit (Hides Template)
                            OtherGrid.CancelEdit();

                            //Refresh Part Grid                        
                            OtherGrid.DataBind();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves Member Job Record Changes From Row Edit Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MemberGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Check Permissions
            if (_userCanEdit)
            {
                //Get Job Step ID
                var jobStepId = -1;
                if ((HttpContext.Current.Session["editingJobStepID"] != null))
                {
                    //Get Info From Session
                    jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                }

                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Make Sure We Have A Selection
                if (MemberGrid.EditingRowVisibleIndex >= 0)
                {
                    //Set Cancel Flag
                    e.Cancel = true;

                    //Get Editing Record ID
                    var recordId = -1;
                    recordId = Convert.ToInt32(MemberGrid.GetRowValues(MemberGrid.EditingRowVisibleIndex, "n_JobOtherID"));

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Check Editing Mode
                        if (MemberGrid.SettingsEditing.Mode == GridViewEditingMode.PopupEditForm)
                        {
                            var objectId = Convert.ToInt32(MemberGrid.GetRowValues(MemberGrid.EditingRowVisibleIndex, "n_MaintenanceObjectID"));

                            //Get Completed
                            var isCompleted = false;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("MemberEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("MemberEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liMemberCompleted")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxCheckBox) != null))
                            {

                                isCompleted =
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("MemberEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("MemberEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liMemberCompleted")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxCheckBox).Checked;
                            }

                            //Get Date Worked
                            var dateWorkedToEdit = _nullDate;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("MemberEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("MemberEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liMemberDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("MemberEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("MemberEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liMemberDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text != ""))
                            {

                                dateWorkedToEdit = Convert.ToDateTime(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("MemberEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("MemberEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liMemberDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text);
                            }

                            //Save Changes
                            _oJobMembers.ClearErrors();

                            //Add Other Record
                            if (_oJobMembers.Update(recordId,
                                jobId,
                                -1,
                                objectId,
                                isCompleted,
                                dateWorkedToEdit,
                                _oLogon.UserID))
                            {
                                //Cacnel Edit (Hides Template)
                                MemberGrid.CancelEdit();

                                //Refresh Member Grid                        
                                MemberGrid.DataBind();
                            }
                        }
                        else
                        {

                            //Get Completed
                            var isCompleted = Convert.ToBoolean(e.NewValues["b_Completed"]);

                            //Get Object ID
                            var objectId = Convert.ToInt32(MemberGrid.GetRowValues(MemberGrid.EditingRowVisibleIndex, "n_MaintenanceObjectID"));

                            var dateWorkedToEdit = _nullDate;
                            if ((e.NewValues["WorkDate"] != null)
                                && (e.NewValues["WorkDate"].ToString() != ""))
                            {
                                //Get Date
                                dateWorkedToEdit = Convert.ToDateTime(e.NewValues["WorkDate"]);
                            }

                            //Save Changes
                            _oJobMembers.ClearErrors();

                            //Add Other Record
                            if (_oJobMembers.Update(recordId,
                                jobId,
                                -1,
                                objectId,
                                isCompleted,
                                dateWorkedToEdit,
                                _oLogon.UserID))
                            {
                                //Cacnel Edit (Hides Template)
                                MemberGrid.CancelEdit();

                                //Refresh Member Grid                        
                                MemberGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves Equip Job Record Changes From Row Edit Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EquipGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Check Permissions
            if (_userCanEdit)
            {
                //Get Job Step ID
                var jobStepId = -1;
                if ((HttpContext.Current.Session["editingJobStepID"] != null))
                {
                    //Get Info From Session
                    jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                }

                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Make Sure We Have A Selection
                if (EquipGrid.EditingRowVisibleIndex >= 0)
                {
                    //Set Cancel Flag
                    e.Cancel = true;

                    //Get Editing Record ID
                    var recordId = -1;
                    recordId = Convert.ToInt32(EquipGrid.GetRowValues(EquipGrid.EditingRowVisibleIndex, "n_JobEquipmentID"));

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Check Editing Mode
                        if (EquipGrid.SettingsEditing.Mode == GridViewEditingMode.PopupEditForm)
                        {
                            var objectId = Convert.ToInt32(EquipGrid.GetRowValues(EquipGrid.EditingRowVisibleIndex, "n_MaintObjectID"));

                            //Get Rate
                            decimal costToAdd = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipRate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipRate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                costToAdd = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipRate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get Desc
                            var equipDesc =
                                ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName("liEquipDesc")
                                    as LayoutItem).GetNestedControl() as ASPxButtonEdit).Text;

                            //Get Misc Ref
                            var miscRefToAdd = "";
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipMisc")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipMisc")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxButtonEdit).Text != ""))
                            {

                                miscRefToAdd =
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipMisc")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxButtonEdit).Text;
                            }

                            //Get Used
                            decimal actToAdd = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipActUnits")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipActUnits")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                actToAdd = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipActUnits")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get Est
                            decimal estToAdd = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipEstUnits")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipEstUnits")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                estToAdd = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipEstUnits")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get Start Meter
                            decimal equipStartMtr = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipStartMtr")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipStartMtr")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                equipStartMtr = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipStartMtr")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get End Meter
                            decimal equipEndMtr = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipEndMtr")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipEndMtr")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                equipEndMtr = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipEndMtr")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get Date Worked
                            var dateWorkedToEdit = _nullDate;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liEquipDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text != ""))
                            {

                                dateWorkedToEdit = Convert.ToDateTime(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("EquipEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("EquipEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liEquipDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text);
                            }

                            //Save Changes
                            _oJobEquipment.ClearErrors();

                            //Update Equipment Record
                            if (_oJobEquipment.Update(recordId,
                                jobId,
                                jobStepId,
                                objectId,
                                costToAdd,
                                equipDesc,
                                miscRefToAdd,
                                actToAdd,
                                estToAdd,
                                equipStartMtr,
                                equipEndMtr,
                                dateWorkedToEdit,
                                _oLogon.UserID))
                            {
                                //Cacnel Edit (Hides Template)
                                EquipGrid.CancelEdit();

                                //Refresh Equip Grid                        
                                EquipGrid.DataBind();
                            }
                        }
                        else
                        {
                            //Get Object ID
                            var objectId = Convert.ToInt32(EquipGrid.GetRowValues(EquipGrid.EditingRowVisibleIndex, "n_MaintObjectID"));

                            //Get Rate
                            decimal costToAdd = 0;
                            if ((e.NewValues["EquipCost"] != null)
                                && (e.NewValues["EquipCost"].ToString() != ""))
                            {

                                costToAdd = Convert.ToDecimal(e.NewValues["EquipCost"]);
                            }

                            //Get Desc
                            var equipDesc = EquipGrid.GetRowValues(EquipGrid.EditingRowVisibleIndex, "EquipDescr").ToString();

                            //Get Misc Ref
                            var miscRefToAdd = "";
                            if ((e.NewValues["MiscellaneousReference"] != null)
                                && (e.NewValues["MiscellaneousReference"].ToString() != ""))
                            {

                                miscRefToAdd = e.NewValues["MiscellaneousReference"].ToString();
                            }

                            //Get Used
                            decimal actToAdd = 0;
                            if ((e.NewValues["qtyused"] != null)
                                && (e.NewValues["qtyused"].ToString() != ""))
                            {

                                actToAdd = Convert.ToDecimal(e.NewValues["qtyused"]);
                            }

                            //Get Est
                            decimal estToAdd = 0;
                            if ((e.NewValues["qtyplanned"] != null)
                                && (e.NewValues["qtyplanned"].ToString() != ""))
                            {

                                estToAdd = Convert.ToDecimal(e.NewValues["qtyplanned"]);
                            }

                            //Get Start Meter
                            decimal equipStartMtr = 0;
                            if ((e.NewValues["StartMeter"] != null)
                                && (e.NewValues["StartMeter"].ToString() != ""))
                            {

                                equipStartMtr = Convert.ToDecimal(e.NewValues["StartMeter"]);
                            }

                            //Get End Meter
                            decimal equipEndMtr = 0;
                            if ((e.NewValues["EndMeter"] != null)
                                && (e.NewValues["EndMeter"].ToString() != ""))
                            {

                                equipEndMtr = Convert.ToDecimal(e.NewValues["EndMeter"]);
                            }

                            //Get Work Date
                            var dateWorkedToEdit = _nullDate;
                            if ((e.NewValues["WorkDate"] != null)
                                && (e.NewValues["WorkDate"].ToString() != ""))
                            {
                                //Get Date
                                dateWorkedToEdit = Convert.ToDateTime(e.NewValues["WorkDate"]);
                            }

                            //Save Changes
                            _oJobEquipment.ClearErrors();

                            //Update Equipment Record
                            if (_oJobEquipment.Update(recordId,
                                jobId,
                                jobStepId,
                                objectId,
                                costToAdd,
                                equipDesc,
                                miscRefToAdd,
                                actToAdd,
                                estToAdd,
                                equipStartMtr,
                                equipEndMtr,
                                dateWorkedToEdit,
                                _oLogon.UserID))
                            {
                                //Cacnel Edit (Hides Template)
                                EquipGrid.CancelEdit();

                                //Refresh Equip Grid                        
                                EquipGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves Crew Job Record Changes From Row Edit Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PartGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Save Job Part Record Changes
            try
            {
                //Check Permissions
                if (_userCanEdit)
                {
                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["editingJobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                    }

                    //Get Job ID
                    var jobId = -1;
                    if ((HttpContext.Current.Session["editingJobID"] != null))
                    {
                        //Get Info From Session
                        jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    }

                    //Make Sure We Have A Selection
                    if (PartGrid.EditingRowVisibleIndex >= 0)
                    {
                        //Set Cancel Flag
                        e.Cancel = true;

                        //Get Editing Record ID
                        var recordId = -1;
                        recordId = Convert.ToInt32(PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "n_jobpartid"));

                        //Get Non-Stocked Flag
                        var nonStocked = ((string)PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "b_nonstocked") == "Y");

                        //Get Masterpart Record Id
                        var masterpartId = -1;
                        masterpartId = Convert.ToInt32(PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "n_masterpartid"));

                        //ID/Desc
                        var nsPartId = "";
                        var nsPartDesc = "";

                        //Get Mfg Part Record Id
                        var mfgPartId = -1;
                        mfgPartId = Convert.ToInt32(PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "n_MfgPartID"));

                        //Get Mfg Part ID
                        var nsMfgPartId = "";
                        nsMfgPartId = PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "NSMfgPartID").ToString();

                        //Check IDs
                        if ((jobStepId > 0) && (jobId > 0))
                        {
                            //Check Editing Mode
                            if (PartGrid.SettingsEditing.Mode == GridViewEditingMode.PopupEditForm)
                            {
                                //Check Stocked Flag
                                if (nonStocked)
                                {
                                    #region Get Part ID

                                    //Get Part Edit Form Layout
                                    var partEditIdFormLayout = ((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout;

                                    //Check For Null
                                    if (partEditIdFormLayout != null)
                                    {
                                        //Get Part Edit Group
                                        var partEditIdLayoutGroup = partEditIdFormLayout
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup;

                                        //Check For Null
                                        if (partEditIdLayoutGroup != null)
                                        {
                                            //Get Layout Item
                                            var partEditIdLayoutItem = partEditIdLayoutGroup.FindItemOrGroupByName(
                                                "liPartId")
                                                as
                                                LayoutItem;

                                            //Check For Null
                                            if (partEditIdLayoutItem != null)
                                            {
                                                //Get Textbox
                                                var partIdTextBox = partEditIdLayoutItem.GetNestedControl() as ASPxTextBox;

                                                //Check For Null/Blank
                                                if (
                                                    partIdTextBox != null && (!string.IsNullOrEmpty(partIdTextBox.Text)))
                                                {
                                                    //Get Value
                                                    nsPartId = partIdTextBox.Text;
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    #region Get Part Description

                                    //Get Part Edit Form Layout
                                    var partEditDescFormLayout = ((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout;

                                    //Check For Null
                                    if (partEditDescFormLayout != null)
                                    {
                                        //Get Part Edit Group
                                        var partEditDescLayoutGroup = partEditDescFormLayout
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup;

                                        //Check For Null
                                        if (partEditDescLayoutGroup != null)
                                        {
                                            //Get Layout Item
                                            var partEditDescLayoutItem = partEditDescLayoutGroup.FindItemOrGroupByName(
                                                "liPartDesc")
                                                as
                                                LayoutItem;

                                            //Check For Null
                                            if (partEditDescLayoutItem != null)
                                            {
                                                //Get Memo Box
                                                var partDescMemoBox = partEditDescLayoutItem.GetNestedControl() as ASPxMemo;

                                                //Check For Null/Blank
                                                if (
                                                    partDescMemoBox != null && (!string.IsNullOrEmpty(partDescMemoBox.Text)))
                                                {
                                                    //Get Value
                                                    nsPartDesc = partDescMemoBox.Text;
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    //Set ID/Desc
                                    nsPartId = PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "nspartid").ToString();
                                    nsPartDesc = PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "NSPartDescr").ToString();
                                }

                                #region Get Misc Ref

                                //Create Variable
                                var miscRef = "";

                                //Check Value
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartMiscRef")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxMemo).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartMiscRef")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxMemo).Text != ""))
                                {
                                    //Get Value
                                    miscRef =
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liPartMiscRef")
                                            as LayoutItem).GetNestedControl() as ASPxMemo).Text;
                                }

                                #endregion

                                #region Get Part At Location ID & Storeroom

                                //Create Variables
                                var partAtLocationId = -1;
                                var storeroomId = -1;

                                //Check Value
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liStoreroomID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liStoreroomID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Text != ""))
                                {
                                    //Get Part At Location Value
                                    partAtLocationId = Convert.ToInt32(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liStoreroomID")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxComboBox).Value);

                                    //Get Storeroom Value
                                    storeroomId = Convert.ToInt32(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liStoreroomID")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxComboBox).SelectedItem.GetValue("n_storeroomid").ToString());
                                }

                                #endregion

                                #region Get Cost

                                //Create Variable
                                var partCost = 0;

                                //Check Value
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartRate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartRate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                                {
                                    //Get Value
                                    partCost = Convert.ToInt32(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liPartRate")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxSpinEdit).Value);
                                }

                                #endregion

                                #region Get Actual

                                //Create Variable
                                decimal actToAdd = 0;

                                //Check Value
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartAct")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartAct")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                                {
                                    //Get Value
                                    actToAdd = Convert.ToDecimal(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liPartAct")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                                }

                                #endregion

                                #region Get Estimated

                                //Create Variable
                                decimal estToAdd = 0;

                                //Check Value
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartEst")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartEst")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                                {
                                    //Get Value
                                    estToAdd = Convert.ToDecimal(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liPartEst")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                                }

                                #endregion

                                #region Get Date Worked

                                //Create Variable
                                var dateWorkedToEdit = _nullDate;

                                //Check Value
                                if (
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text != null)
                                    &&
                                    (((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("PartEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liPartDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text != ""))
                                {
                                    //Get Value
                                    dateWorkedToEdit = Convert.ToDateTime(
                                        ((((((ASPxGridView)sender).FindEditFormTemplateControl("PartEditLayout") as
                                            ASPxFormLayout)
                                            .FindItemOrGroupByName("PartEditGroup") as LayoutGroup)
                                            .FindItemOrGroupByName(
                                                "liPartDate")
                                            as
                                            LayoutItem).GetNestedControl() as ASPxDateEdit).Text);
                                }

                                #endregion

                                //Set Job ID/Step
                                _oJobParts.SetJobstepInfo(jobId, jobStepId);

                                //Save Changes
                                _oJobParts.ClearErrors();

                                //Update Record
                                if (_oJobParts.Update(recordId,
                                                               masterpartId,
                                                               storeroomId,
                                                               partAtLocationId,
                                                               false,
                                                               nonStocked,
                                                               miscRef,
                                                               nsPartId,
                                                               partCost,
                                                               nsMfgPartId,
                                                               nsPartDesc,
                                                               estToAdd,
                                                               actToAdd,
                                                               dateWorkedToEdit,
                                                               mfgPartId,
                                                               _oLogon.UserID))
                                {
                                    //Cacnel Edit (Hides Template)
                                    PartGrid.CancelEdit();

                                    //Refresh Part Grid                        
                                    PartGrid.DataBind();
                                }
                                else
                                {
                                    //Cacnel Edit (Hides Template)
                                    PartGrid.CancelEdit();

                                    //Refresh Part Grid                        
                                    PartGrid.DataBind();

                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Updating Job Part - "
                                        + _oJobParts.LastError);
                                }
                            }
                            else
                            {
                                #region Get Part ID/Desc

                                //Check Stocked Flag
                                if (nonStocked)
                                {
                                    #region Get Part ID

                                    if ((e.NewValues["nspartid"] != null)
                                        && (e.NewValues["nspartid"].ToString() != ""))
                                    {
                                        //Get Value
                                        nsPartId = e.NewValues["nspartid"].ToString();
                                    }

                                    #endregion

                                    #region Get Part Description

                                    if ((e.NewValues["NSPartDescr"] != null)
                                        && (e.NewValues["NSPartDescr"].ToString() != ""))
                                    {
                                        //Get Value
                                        nsPartDesc = e.NewValues["NSPartDescr"].ToString();
                                    }

                                    #endregion
                                }
                                else
                                {
                                    //Set ID/Desc
                                    nsPartId = PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "nspartid").ToString();
                                    nsPartDesc = PartGrid.GetRowValues(PartGrid.EditingRowVisibleIndex, "NSPartDescr").ToString();
                                }

                                #endregion

                                #region Get Part Cost

                                //Create Variable
                                decimal partCost = 0;

                                //Check Input
                                if ((e.NewValues["nspartcost"] != null)
                                    && (e.NewValues["nspartcost"].ToString() != ""))
                                {
                                    //Get Value
                                    partCost = Convert.ToDecimal(e.NewValues["nspartcost"]);
                                }

                                #endregion

                                #region Misc. Ref

                                //Create Variable
                                var miscRef = "";

                                //Check Input
                                if ((e.NewValues["miscrefnum"] != null)
                                    && (e.NewValues["miscrefnum"].ToString() != ""))
                                {
                                    //Get Value
                                    miscRef = e.NewValues["miscrefnum"].ToString();
                                }

                                #endregion

                                #region Get Storeroom ID

                                //Create Variable
                                var storeroomId = -1;

                                //Check Input
                                if ((e.NewValues["n_storeroomid"] != null)
                                    && (e.NewValues["n_storeroomid"].ToString() != ""))
                                {
                                    //Get Value
                                    storeroomId = Convert.ToInt32(e.NewValues["n_storeroomid"]);
                                }

                                #endregion

                                #region Get Part At Location ID

                                //Create Variable
                                var partAtLocationId = -1;

                                //Check Input
                                if ((e.NewValues["n_partatlocid"] != null)
                                    && (e.NewValues["n_partatlocid"].ToString() != ""))
                                {
                                    //Get Value
                                    partAtLocationId = Convert.ToInt32(e.NewValues["n_partatlocid"]);
                                }

                                #endregion

                                #region Get Actual

                                //Create Variable
                                decimal actToAdd = 0;

                                //Check Input
                                if ((e.NewValues["qtyused"] != null)
                                    && (e.NewValues["qtyused"].ToString() != ""))
                                {
                                    //Get Value
                                    actToAdd = Convert.ToDecimal(e.NewValues["qtyused"]);
                                }

                                #endregion

                                #region Get Estimated

                                //Create Variable
                                decimal estToAdd = 0;

                                //Check Input
                                if ((e.NewValues["qtyplanned"] != null)
                                    && (e.NewValues["qtyplanned"].ToString() != ""))
                                {
                                    //Get Valure
                                    estToAdd = Convert.ToDecimal(e.NewValues["qtyplanned"]);
                                }

                                #endregion

                                #region Get Work Date

                                //Create Variable
                                var dateWorkedToEdit = _nullDate;

                                //Check Values
                                if ((e.NewValues["WorkDate"] != null)
                                    && (e.NewValues["WorkDate"].ToString() != ""))
                                {
                                    //Get Date
                                    dateWorkedToEdit = Convert.ToDateTime(e.NewValues["WorkDate"]);
                                }

                                #endregion

                                //Set Job ID/Step
                                _oJobParts.SetJobstepInfo(jobId, jobStepId);

                                //Save Changes
                                _oJobParts.ClearErrors();

                                //Update Record
                                if (_oJobParts.Update(recordId,
                                                               masterpartId,
                                                               storeroomId,
                                                               partAtLocationId,
                                                               false,
                                                               nonStocked,
                                                               miscRef,
                                                               nsPartId,
                                                               partCost,
                                                               nsMfgPartId,
                                                               nsPartDesc,
                                                               estToAdd,
                                                               actToAdd,
                                                               dateWorkedToEdit,
                                                               mfgPartId,
                                                               _oLogon.UserID))
                                {
                                    //Cacnel Edit (Hides Template)
                                    PartGrid.CancelEdit();

                                    //Refresh Part Grid                        
                                    PartGrid.DataBind();
                                }
                                else
                                {
                                    //Cacnel Edit (Hides Template)
                                    PartGrid.CancelEdit();

                                    //Refresh Part Grid                        
                                    PartGrid.DataBind();

                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Updating Job Part - "
                                        + _oJobParts.LastError);
                                }
                            }
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
        /// Saves Part Job Record Changes From Row Edit Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CrewGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Check Permissions
            if (_userCanEdit)
            {
                //Get Job Step ID
                var jobStepId = -1;
                if ((HttpContext.Current.Session["editingJobStepID"] != null))
                {
                    //Get Info From Session
                    jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"]);
                }

                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Make Sure We Have A Selection
                if (CrewGrid.EditingRowVisibleIndex >= 0)
                {
                    //Set Cancel Flag
                    e.Cancel = true;

                    //Get Editing Record ID
                    var recordId = -1;
                    recordId = Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "RecordID"));

                    //Check IDs
                    if ((jobStepId > 0) && (jobId > 0))
                    {
                        //Check Editing Mode
                        if (CrewGrid.SettingsEditing.Mode == GridViewEditingMode.PopupEditForm)
                        {
                            //Get Shift
                            var shiftId = Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_ShiftID"));

                            //Get Crew ID
                            var crewId = -1;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewUserID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewUserID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != ""))
                            {

                                crewId = Convert.ToInt32(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewUserID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Value);
                            }

                            //Get Labor ID
                            var laborClassId = -1;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewLaborID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewLaborID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != ""))
                            {

                                laborClassId = Convert.ToInt32(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewLaborID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Value);
                            }

                            //Get Paycode
                            var paycodeId = -1;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewPaycodeID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewPaycodeID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != ""))
                            {

                                paycodeId = Convert.ToInt32(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewPaycodeID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Value);
                            }

                            //Get Skill
                            var skillId = -1;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewSkillID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewSkillID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != ""))
                            {

                                skillId = Convert.ToInt32(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewSkillID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Value);
                            }

                            //Get Rate Type
                            var rateType = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewRateID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewRateID")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxComboBox).Text != ""))
                            {

                                rateType = Convert.ToInt32(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewRateID")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxComboBox).Value);
                            }

                            //Get Used
                            decimal actToAdd = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewActHrs")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewActHrs")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                actToAdd = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewActHrs")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get Est
                            decimal estToAdd = 0;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewEstHrs")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewEstHrs")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxSpinEdit).Text != ""))
                            {

                                estToAdd = Convert.ToDecimal(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewEstHrs")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxSpinEdit).Text);
                            }

                            //Get Date Worked
                            var dateWorkedToEdit = _nullDate;
                            if (
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text != null)
                                &&
                                (((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                    ASPxFormLayout)
                                    .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                        "liCrewDate")
                                    as
                                    LayoutItem).GetNestedControl() as ASPxDateEdit).Text != ""))
                            {

                                dateWorkedToEdit = Convert.ToDateTime(
                                    ((((((ASPxGridView)sender).FindEditFormTemplateControl("CrewEditLayout") as
                                        ASPxFormLayout)
                                        .FindItemOrGroupByName("CrewEditGroup") as LayoutGroup).FindItemOrGroupByName(
                                            "liCrewDate")
                                        as
                                        LayoutItem).GetNestedControl() as ASPxDateEdit).Text);
                            }

                            //Rate
                            var rate = GetRateToUse(crewId, shiftId, laborClassId,
                                rateType);

                            //Save Changes
                            _oJobCrew.ClearErrors();

                            //Update Crew Record
                            if (_oJobCrew.Update(recordId,
                                crewId,
                                laborClassId,
                                paycodeId,
                                shiftId,
                                skillId,
                                rate,
                                estToAdd,
                                actToAdd,
                                dateWorkedToEdit,
                                rateType,
                                _oLogon.UserID,
                                -1,
                                -1))
                            {
                                //Cacnel Edit (Hides Template)
                                CrewGrid.CancelEdit();

                                //Refresh Crew Grid                        
                                CrewGrid.DataBind();
                            }
                        }
                        else
                        {
                            //Get Shift ID
                            var shiftId = -1;
                            if ((CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_ShiftID") != null)
                                && (CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_ShiftID").ToString() != ""))
                            {
                                shiftId =
                                    Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_ShiftID"));
                            }

                            //Get Crew
                            var crewId = -1;
                            if ((e.NewValues["CrewMemberTextID"] != null)
                                && (e.NewValues["CrewMemberTextID"].ToString() != ""))
                            {
                                try
                                {
                                    //Get Crew ID
                                    crewId = Convert.ToInt32(e.NewValues["CrewMemberTextID"]);
                                }
                                catch (Exception)
                                {
                                    if ((CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "UserID") != null)
                                        && (CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "UserID").ToString() != ""))
                                    {
                                        crewId =
                                            Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "UserID"));
                                    }
                                }
                            }

                            //Get Labor Class
                            var laborClassId = -1;
                            if ((e.NewValues["LaborClassID"] != null)
                                && (e.NewValues["LaborClassID"].ToString() != ""))
                            {
                                try
                                {
                                    //Get Labor ID
                                    laborClassId = Convert.ToInt32(e.NewValues["LaborClassID"]);
                                }
                                catch (Exception)
                                {
                                    if ((CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_laborclassid") != null)
                                        && (CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_laborclassid").ToString() != ""))
                                    {
                                        laborClassId =
                                            Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_laborclassid"));
                                    }
                                }
                            }

                            //Get Paycode
                            var paycodeId = -1;
                            if ((e.NewValues["PayCodeText"] != null)
                                && (e.NewValues["PayCodeText"].ToString() != ""))
                            {
                                try
                                {
                                    //Get Paycode ID
                                    paycodeId = Convert.ToInt32(e.NewValues["PayCodeText"]);
                                }
                                catch (Exception)
                                {
                                    if ((CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_PayCodeID") != null)
                                        && (CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_PayCodeID").ToString() != ""))
                                    {
                                        paycodeId =
                                            Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_PayCodeID"));
                                    }
                                }
                            }

                            //Get Skill
                            var skillId = -1;
                            if ((e.NewValues["SkillIDText"] != null)
                                && (e.NewValues["SkillIDText"].ToString() != ""))
                            {
                                try
                                {
                                    //Get Skill ID
                                    skillId = Convert.ToInt32(e.NewValues["SkillIDText"]);
                                }
                                catch (Exception)
                                {
                                    if ((CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_skillid") != null)
                                        && (CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_skillid").ToString() != ""))
                                    {
                                        skillId =
                                            Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "n_skillid"));
                                    }
                                }
                            }

                            //Get Rate Type
                            var rateType = -1;
                            if ((e.NewValues["RateTypeStr"] != null)
                                && (e.NewValues["RateTypeStr"].ToString() != ""))
                            {
                                try
                                {
                                    //Get Rate Type ID
                                    rateType = Convert.ToInt32(e.NewValues["RateTypeStr"]);
                                }
                                catch (Exception)
                                {
                                    if ((CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "RateType") != null)
                                        && (CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "RateType").ToString() != ""))
                                    {
                                        rateType =
                                            Convert.ToInt32(CrewGrid.GetRowValues(CrewGrid.EditingRowVisibleIndex, "RateType"));
                                    }
                                }
                            }

                            //Get Used
                            decimal actToAdd = 0;
                            if ((e.NewValues["qtyused"] != null)
                                && (e.NewValues["qtyused"].ToString() != ""))
                            {

                                actToAdd = Convert.ToDecimal(e.NewValues["qtyused"]);
                            }

                            //Get Est
                            decimal estToAdd = 0;
                            if ((e.NewValues["qtyplanned"] != null)
                                && (e.NewValues["qtyplanned"].ToString() != ""))
                            {

                                estToAdd = Convert.ToDecimal(e.NewValues["qtyplanned"]);
                            }

                            //Get Work Date
                            var dateWorkedToEdit = _nullDate;
                            if ((e.NewValues["WorkDate"] != null)
                                && (e.NewValues["WorkDate"].ToString() != ""))
                            {
                                //Get Date
                                dateWorkedToEdit = Convert.ToDateTime(e.NewValues["WorkDate"]);
                            }

                            //Rate
                            var rate = GetRateToUse(crewId, shiftId, laborClassId,
                                rateType);

                            //Save Changes
                            _oJobCrew.ClearErrors();

                            //Update Crew Record
                            if (_oJobCrew.Update(recordId,
                                crewId,
                                laborClassId,
                                paycodeId,
                                shiftId,
                                skillId,
                                rate,
                                estToAdd,
                                actToAdd,
                                dateWorkedToEdit,
                                rateType,
                                _oLogon.UserID,
                                -1,
                                -1))
                            {
                                //Cacnel Edit (Hides Template)
                                CrewGrid.CancelEdit();

                                //Refresh Crew Grid                        
                                CrewGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets Selected Members Work Dates To Start Date If Exists
        /// </summary>
        protected void SetMembersToStartDate()
        {
            //Check For Multi Select Option
            if (((MemberGrid.Columns[0] as GridViewColumn).Visible))
            {
                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Get Start Date
                var startDate = _nullDate;
                if ((HttpContext.Current.Session["TxtStartingDate"] != null))
                {
                    //Get Info From Session
                    startDate = Convert.ToDateTime(HttpContext.Current.Session["TxtStartingDate"]);
                }

                //Check Date
                if (startDate != _nullDate)
                {
                    //Check Job ID
                    if (jobId > 0)
                    {
                        //Get Selections
                        var recordIdSelection = MemberGrid.GetSelectedFieldValues("n_JobOtherID");

                        //Create Control Flags
                        var continueUpdate = true;
                        var updateDone = false;

                        //Process Multi Selection
                        foreach (var selection in recordIdSelection)
                        {
                            //Get ID
                            var recordId = Convert.ToInt32(selection.ToString());

                            //Get Index
                            var rowIndex = MemberGrid.FindVisibleIndexByKeyValue(recordId);

                            //Get Object ID
                            var objectId = Convert.ToInt32(MemberGrid.GetRowValues(rowIndex, "n_MaintenanceObjectID"));

                            //Get Is Completed
                            var isCompleted = (MemberGrid.GetRowValues(rowIndex, "b_Completed").ToString() == "1");

                            //Set Continue Bool
                            continueUpdate = (recordId > 0);

                            //Check Continue Bool
                            if (continueUpdate)
                            {
                                //Clear Errors
                                _oJobMembers.ClearErrors();

                                //Update Member
                                if (_oJobMembers.Update(recordId,
                                    jobId,
                                    -1,
                                    objectId,
                                    isCompleted,
                                    startDate,
                                    _oLogon.UserID))
                                {
                                    //Set Flag
                                    updateDone = true;
                                }
                                else
                                {
                                    //Set Flag
                                    continueUpdate = false;
                                }
                            }

                            //Check Deletion Done
                            if (updateDone)
                            {
                                //Refresh Member Grid                        
                                MemberGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets Selected Members Work Dates To End Date If Exists
        /// </summary>
        protected void SetMembersToEndDate()
        {
            //Check For Multi Select Option
            if (((MemberGrid.Columns[0] as GridViewColumn).Visible))
            {
                //Get Job ID
                var jobId = -1;
                if ((HttpContext.Current.Session["editingJobID"] != null))
                {
                    //Get Info From Session
                    jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                }

                //Get Comp Date
                var compDate = _nullDate;
                if ((HttpContext.Current.Session["TxtCompletionDate"] != null))
                {
                    //Get Info From Session
                    compDate = Convert.ToDateTime(HttpContext.Current.Session["TxtCompletionDate"]);
                }

                //Check Date
                if (compDate != _nullDate)
                {
                    //Check Job ID
                    if (jobId > 0)
                    {
                        //Get Selections
                        var recordIdSelection = MemberGrid.GetSelectedFieldValues("n_JobOtherID");

                        //Create Control Flags
                        var continueUpdate = true;
                        var updateDone = false;

                        //Process Multi Selection
                        foreach (var selection in recordIdSelection)
                        {
                            //Get ID
                            var recordId = Convert.ToInt32(selection.ToString());

                            //Get Index
                            var rowIndex = MemberGrid.FindVisibleIndexByKeyValue(recordId);

                            //Get Object ID
                            var objectId = Convert.ToInt32(MemberGrid.GetRowValues(rowIndex, "n_MaintenanceObjectID"));

                            //Get Is Completed
                            var isCompleted = (MemberGrid.GetRowValues(rowIndex, "b_Completed").ToString() == "1");

                            //Set Continue Bool
                            continueUpdate = (recordId > 0);

                            //Check Continue Bool
                            if (continueUpdate)
                            {
                                //Clear Errors
                                _oJobMembers.ClearErrors();

                                //Update Member
                                if (_oJobMembers.Update(recordId,
                                    jobId,
                                    -1,
                                    objectId,
                                    isCompleted,
                                    compDate,
                                    _oLogon.UserID))
                                {
                                    //Set Flag
                                    updateDone = true;
                                }
                                else
                                {
                                    //Set Flag
                                    continueUpdate = false;
                                }
                            }

                            //Check Deletion Done
                            if (updateDone)
                            {
                                //Refresh Member Grid                        
                                MemberGrid.DataBind();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns Users Rate For Specified Shift/Laborclass
        /// </summary>
        /// <param name="crewMemberId">User ID To Get Rate For</param>
        /// <param name="shiftId">Shift To Use</param>
        /// <param name="laborClass">Labor Class To Use</param>
        /// <param name="stanOverOtherRateId">Overhead Rate ID</param>
        /// <returns>Adjusted Users Rate</returns>
        private decimal GetRateToUse(int crewMemberId, int shiftId, int laborClass, int stanOverOtherRateId)
        {
            //Set Defaults
            decimal rateUsed = 0;
            decimal straightTimeRate = 0;
            decimal overtimeRate = 0;
            decimal otherRate = 0;

            //Get Adjusted Rate
            if (_oMpetUser.GetAdjustedUserRate(crewMemberId,
                shiftId,
                laborClass,
                ref straightTimeRate,
                ref overtimeRate,
                ref otherRate))
            {
                //Determine What Rate To Use
                switch (stanOverOtherRateId)
                {
                    case 0: //Standard
                        {
                            rateUsed = straightTimeRate;
                            break;
                        }
                    case 1: //Overtime
                        {
                            rateUsed = overtimeRate;
                            break;
                        }
                    case 2: //Other
                        {
                            rateUsed = otherRate;
                            break;
                        }
                    default:
                        {
                            rateUsed = straightTimeRate;
                            break;
                        }
                }
            }

            //Return Rate
            return rateUsed;
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void MemberGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            StepTab.TabPages[1].Text = @"MEMBERS (" + MemberGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void CrewGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            StepTab.TabPages[2].Text = @"CREW (" + CrewGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void PartGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            StepTab.TabPages[3].Text = @"PARTS (" + PartGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void EquipGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            StepTab.TabPages[4].Text = @"EQUIPMENT (" + EquipGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void OtherGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            StepTab.TabPages[5].Text = @"OTHER (" + OtherGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void AttachmentGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            StepTab.TabPages[6].Text = @"ATTACHMENTS (" + AttachmentGrid.VisibleRowCount + @")";
        }
        #endregion

        #region Set up Run Units
        private void SetupRunUnits(int objTypeAgainst)
        {
            try
            {
                //
                //Reset the Run Units to Zero and disable them
                //
                txtRunUnitOne.Value = 0.00;
                txtRunUnitTwo.Value = 0.00;
                txtRunUnitThree.Value = 0.00;
                txtRunUnitOne.ReadOnly = true;
                txtRunUnitTwo.ReadOnly = true;
                txtRunUnitThree.ReadOnly = true;

                //Clear Errors
                _oRunUnit.ClearErrors();

                //Load Data
                if (_oRunUnit.LoadData(objTypeAgainst))
                {
                    //Check Table
                    if (_oRunUnit.Ds.Tables.Count > 0)
                    {
                        //Check Rows
                        if (_oRunUnit.Ds.Tables[0].Rows.Count > 0)
                        {
                            //Loop Data
                            for (var I = 0; I < _oRunUnit.Ds.Tables[0].Rows.Count; I++)
                            {
                                //0[RecordID], 
                                //1[n_objectID], 
                                //2[CurrentReading], 
                                //3[Description], 
                                //4[DisplayDecimals]

                                //Determine Unit
                                switch (I)
                                {
                                    case 0:
                                        {
                                            //Set Value
                                            txtRunUnitOne.Value = (decimal)_oRunUnit.Ds.Tables[0].Rows[I][2];
                                            txtRunUnitOne.ReadOnly = false;
                                            break;
                                        }
                                    case 1:
                                        {
                                            //Set Value
                                            txtRunUnitTwo.Value = (decimal)_oRunUnit.Ds.Tables[0].Rows[I][2];
                                            txtRunUnitTwo.ReadOnly = false;
                                            break;
                                        }
                                    case 2:
                                        {
                                            //Set Value
                                            txtRunUnitThree.Value = (decimal)_oRunUnit.Ds.Tables[0].Rows[I][2];
                                            txtRunUnitThree.ReadOnly = false;
                                            break;
                                        }
                                }
                            }
                        }

                        //Add To Session
                        HttpContext.Current.Session.Add("RunUnit",
                            _oRunUnit);
                    }
                    else
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Job Step - No Table Data Returned");
                    }
                }
                else
                {
                    //Throw Error
                    throw new SystemException(
                        @"Error Loading Run Units -" + _oRunUnit.LastError);
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }
        #endregion

        #region Button Click Set up For ASPX page
        /// <summary>
        /// Adds Crew For Job By Group
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddCrewByGroup_Click(object sender, EventArgs e)
        {
            //Add Crew For Job By Group
            try
            {
                //Check For Multi Selection
                if ((CrewGrid.Columns[0].Visible))
                {
                    //Check For Job ID
                    if (Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()) > 0)
                    {
                        //Check For Job Step ID
                        if (Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()) > 0)
                        {
                            //Get Job/Step Keys
                            var jobstepKey = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());
                            var jobKey = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

                            //Get Group Selections
                            var groupSelections =
                                CrewGroupGridLookup.GetSelectedFieldValues("n_groupid");

                            //Create Group Member Class
                            var oGroupMembers = new MaintenanceGroupDetail(_connectionString, _useWeb, _oLogon.UserID);

                            //Process Multi Selection
                            foreach (var selected in groupSelections)
                            {
                                //Get Group ID
                                var groupId = Convert.ToInt32(selected.ToString());

                                //Load Group Members
                                if (oGroupMembers.LoadDesiredGroupMembers(groupId))
                                {
                                    //Check Table Count
                                    if (oGroupMembers.Ds.Tables.Count > 0)
                                    {
                                        //Loop Through Members & Add To Job
                                        for (var a = 0; a < oGroupMembers.Ds.Tables[0].Rows.Count; a++)
                                        {
                                            #region Reset Editing Variables

                                            var editingJcShiftId = -1;
                                            var editingJcLaborClassId = -1;
                                            var editingJcSkillId = -1;
                                            var editingJcRateChoice = -1;
                                            var editingJcEstHrs = 0;
                                            var editingJcActHrs = 0;
                                            decimal straightTimeRate = 0;
                                            decimal overtimeRate = 0;
                                            decimal otherRate = 0;

                                            #endregion

                                            //Set Editing Crew ID
                                            var editingCrewMember =
                                                Convert.ToInt32(oGroupMembers.Ds.Tables[0].Rows[a][2].ToString());

                                            #region Get User Shift Info

                                            //Check ID
                                            if (editingCrewMember > 0)
                                            {
                                                //Create Temp Variables
                                                var userLaborId = "";
                                                var userLaborDesc = "";
                                                var tmpLaborClass = 0;

                                                //Get User Labor Class Settings
                                                if (!_oMpetUser.GetUsersLaborSettingText(editingCrewMember, ref tmpLaborClass, ref userLaborId, ref userLaborDesc))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Getting Labor Class - " + _oMpetUser.LastError);
                                                }

                                                //Get Labor Class
                                                editingJcLaborClassId = tmpLaborClass;

                                                //Create Flag
                                                var gotRates = true;

                                                //Clear Errors
                                                _oMpetUser.ClearErrors();

                                                //Get Rates
                                                using (var dtRates = _oMpetUser.GetUserDetailInfo(editingCrewMember, ref gotRates))
                                                {
                                                    //Check Flag
                                                    if (!gotRates)
                                                        //Throw Error
                                                        throw new SystemException(
                                                            @"Error Getting Crew Rates - " + _oMpetUser.LastError);
                                                }
                                            }

                                            //Load User Data
                                            if (_oMpetUser.LoadData())
                                            {
                                                //Chceck Table Count
                                                if (_oMpetUser.Ds.Tables.Count > 0)
                                                {
                                                    //  0    [UserID], 
                                                    //  1    [Username], 
                                                    //  2    [FirstName], 
                                                    //  3    [LastName], 
                                                    //  4    [Password], 
                                                    //  5    [AreaID], 
                                                    //  6    [WorkPhone], 
                                                    //  7    [CellPhone], 
                                                    //  8    [PayrollID], 
                                                    //  9    [PositionCodeID], 
                                                    //  10   [PersonClassID], 
                                                    //  11   [LocationID], 
                                                    //  12   [PasswordExpireDate], 
                                                    //  13   [PasswordExpires], 
                                                    //  14   [PasswordDayCount], 
                                                    //  15   [Notes], 
                                                    //  16   [Active], 
                                                    //  17   [LaborClassID], 
                                                    //  18   [GroupID], 
                                                    //  19   [FundID], 
                                                    //  20   [CompanyDate], 
                                                    //  21   [PlantDate], 
                                                    //  22   [EntryDate],
                                                    //  23   [CanLogon],
                                                    //  24   n_shiftid,
                                                    //  25   MondayHrs,
                                                    //  26   TuesdayHrs,
                                                    //  27   WednesdayHrs,
                                                    //  28   ThursdayHrs,
                                                    //  29   FridayHrs,
                                                    //  31   SaturdayHrs,
                                                    //  32   SundayHrs
                                                    var dv = new DataView(_oMpetUser.Ds.Tables[0]) { RowFilter = "UserID=" + editingCrewMember };

                                                    //Check Count
                                                    if (dv.Count == 1)
                                                    {
                                                        var drv = dv[0];
                                                        var row = drv.Row;
                                                        editingJcShiftId = (int)row[24];
                                                    }
                                                }
                                                else
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"User Table Expected");
                                                }
                                            }
                                            else
                                            {
                                                //Throw Error
                                                throw new SystemException(
                                                    @"Error Getting Crew Member Rates - " + _oMpetUser.LastError);
                                            }


                                            #endregion

                                            //Clear Errors
                                            _oJobCrew.ClearErrors();

                                            //Get User Rate
                                            if (!_oJobCrew.GetAdjustedUserRate(editingCrewMember,
                                                                              editingJcShiftId,
                                                                              editingJcLaborClassId,
                                                                              ref straightTimeRate,
                                                                              ref overtimeRate,
                                                                              ref otherRate))
                                            {
                                                //Throw Error
                                                throw new SystemException(
                                                    @"Possible Bad Pay Rate - " + _oJobCrew.LastError);
                                            }

                                            //Set Devault Work Date
                                            var editingJcWorkDate = _nullDate;

                                            //If Completion Date Is Set Use It Instead
                                            if ((HttpContext.Current.Session["TxtCompletionDate"] != null) &&
                                                (HttpContext.Current.Session["TxtCompletionDate"].ToString() != ""))
                                            {
                                                //Set New Work Date
                                                editingJcWorkDate = Convert.ToDateTime(HttpContext.Current.Session["TxtCompletionDate"].ToString());
                                            }

                                            //Clear Errors
                                            _oJobCrew.ClearErrors();

                                            //Add Crew Member
                                            if (!_oJobCrew.Add(jobKey,
                                                jobstepKey,
                                                editingCrewMember,
                                                editingJcLaborClassId,
                                                -1,
                                                editingJcShiftId,
                                                0,
                                                straightTimeRate,
                                                0,
                                                0,
                                                0,
                                                editingJcWorkDate,
                                                _oLogon.UserID,
                                                -1,
                                                -1))
                                            {
                                                //Throw Error
                                                throw new SystemException(
                                                    @"Error Adding Job Crew Member - " + _oJobCrew.LastError);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Adding Job Crew Member By Group - "
                                                        + oGroupMembers.LastError);
                                }
                            }

                            //Perform Refresh
                            CrewGrid.DataBind();
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
        /// Adds Crew For Job By Labor Class
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddCrewByLabor_Click(object sender, EventArgs e)
        {
            //Add Crew For Job By Labor Class
            try
            {
                //Check For Multi Selection
                if ((CrewGrid.Columns[0].Visible))
                {
                    //Check For Job ID
                    if (Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()) > 0)
                    {
                        //Check For Job Step ID
                        if (Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()) > 0)
                        {
                            //Get Job/Step Keys
                            var jobstepKey = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());
                            var jobKey = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

                            //Get Labor Selections
                            var laborSelections =
                                CrewLaborGridLookup.GetSelectedFieldValues("n_laborclassid");

                            //Create Editing Variables
                            const int editingJcCrewId = -1;
                            const int editingJcPaycodeId = -1;
                            const int editingJcShiftId = -1;
                            const int editingJcSkillId = -1;
                            const int editingJcRateChoice = -1;
                            const int editingJcEstHrs = 0;
                            const int editingJcActHrs = 0;

                            //Process Multi Selection
                            foreach (var selected in laborSelections)
                            {
                                //Get & Set Laborclass
                                var editingJcLaborClassId = Convert.ToInt32(selected.ToString());

                                //Create Null Date
                                var editingJcWorkDate = _nullDate;

                                #region Determine Rate

                                //Get Adjusted Rate
                                var rateUsed = GetRateToUse(editingJcCrewId,
                                    editingJcShiftId,
                                    editingJcLaborClassId,
                                    editingJcRateChoice);

                                #endregion

                                //If Completion Date Is Set Use It Instead
                                if ((HttpContext.Current.Session["TxtCompletionDate"] != null) &&
                                    (HttpContext.Current.Session["TxtCompletionDate"].ToString() != ""))
                                {
                                    //Set New Work Date
                                    editingJcWorkDate = Convert.ToDateTime(HttpContext.Current.Session["TxtCompletionDate"].ToString());
                                }

                                //Clear Errors
                                _oJobCrew.ClearErrors();

                                //Add Crew By Laborclass
                                if (!_oJobCrew.Add(jobKey,
                                    jobstepKey,
                                    -1,
                                    editingJcLaborClassId,
                                    editingJcPaycodeId,
                                    editingJcShiftId,
                                    editingJcSkillId,
                                    rateUsed,
                                    editingJcEstHrs,
                                    editingJcActHrs,
                                    editingJcRateChoice,
                                    editingJcWorkDate,
                                    _oLogon.UserID,
                                    -1,
                                    -1))
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Adding Job Crew Member - " + _oJobCrew.LastError);
                                }
                            }

                            //Perform Refresh
                            CrewGrid.DataBind();
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

        protected void AddNewItemButton_Click(object sender, EventArgs e)
        {
            AddItems();
        }

        protected void DeleteItems_Click(object sender, EventArgs e)
        {
            DeleteItems();
            var jobStepKey = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());
            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + jobStepKey);
        }

        protected void DeleteAttachmentButton_Click(object sender, EventArgs e)
        {
            DeleteGridViewAttachment();
        }
        #endregion        

        #region Add Save Update Job & JobStep Routines
        /// <summary>
        /// Resets Session Variables
        /// </summary>
        protected void ResetSession()
        {
            //Clear Session & Fields
            if (HttpContext.Current.Session["navObject"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("navObject");
            }

            //Clear Session & Fields
            if (HttpContext.Current.Session["RunUnit"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("RunUnit");
            }

            //Check For Prior Job Step ID
            if (HttpContext.Current.Session["editingJobStepID"] != null)
            {
                //Remove Old ONe
                HttpContext.Current.Session.Remove("editingJobStepID");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["editingJobID"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("editingJobID");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["HasAttachments"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("HasAttachments");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["AssignedJobID"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("AssignedJobID");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtWorkDescription");
            }
            //Check For Prior Value
            if (HttpContext.Current.Session["ObjectIDCombo"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ObjectIDCombo");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ObjectIDComboText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ObjectIDComboText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtObjectDescription"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtObjectDescription");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtObjectArea"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtObjectArea");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtObjectLocation"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtObjectLocation");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtObjectAssetNumber"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtObjectAssetNumber");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["TxtWorkRequestDate"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("TxtWorkRequestDate");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboRequestor"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboRequestor");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboRequestorText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboRequestorText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboPriority"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboPriority");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboPriorityText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboPriorityText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboReason"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboReason");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboReasonText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboReasonText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboRouteTo"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboRouteTo");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboRouteToText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboRouteToText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboHwyRoute"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboHwyRoute");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboHwyRouteText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboHwyRouteText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtMilepost"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtMilepost");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtMilepostTo"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtMilepostTo");
            }


            //Check For Prior Value
            if (HttpContext.Current.Session["comboMilePostDir"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboMilePostDir");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["comboMilePostDirText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("comboMilePostDirText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboCostCode"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboCostCode");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboCostCodeText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboCostCodeText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboFundSource"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboFundSource");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboFundSourceText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboFundSourceText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboWorkOrder"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboWorkOrder");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboWorkOrderText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboWorkOrderText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboWorkOp"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboWorkOp");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboWorkOpText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboWorkOpText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboOrgCode"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboOrgCode");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboOrgCodeText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboOrgCodeText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboFundGroup"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboFundGroup");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboFundGroupText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboFundGroupText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboCtlSection"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboCtlSection");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboCtlSectionText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboCtlSectionText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboEquipNum"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboEquipNum");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboEquipNumText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboEquipNumText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtFN"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtFN");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtLN"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtLN");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtEmail"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtEmail");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtPhone"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtPhone");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtExt"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtExt");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtMail"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtMail");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtBuilding"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtBuilding");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRoomNum"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRoomNum");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboServiceOffice"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboServiceOffice");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ComboServiceOfficeText"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ComboServiceOfficeText");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["GPSX"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("GPSX");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["GPSY"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("GPSY");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("GPSZ");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtAddDetail"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtAddDetail");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtPostNotes"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtPostNotes");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["PreviousStep"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("PreviousStep");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["NextStep"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("NextStep");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRunUnitOne"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRunUnitOne");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRunUnitTwo"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRunUnitTwo");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRunUnitThree"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRunUnitThree");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["stepnumber"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("stepnumber");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["concurwithstep"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("concurwithstep");
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["followstep"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("followstep");
            }
        }

        /// <summary>
        /// Updates Work Reqeust
        /// </summary>
        /// <returns>True/False For Success</returns>
        protected bool UpdateRequest()
        {
            var jobAgainstArea = 0;
            const bool requestOnly = true;
            const int actionPriority = -1;
            const int mobileEquip = -1;
            const int subAssemblyID = -1;
            const bool additionalDamage = false;
            const decimal percentOverage = 0;

            #region Get Logon Info

            //Get Value
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
            }

            #endregion

            #region Get Object

            var objectAgainstId = -1;
            if (HttpContext.Current.Session["ObjectIDCombo"] != null)
            {
                //Get Info From Session
                objectAgainstId = Convert.ToInt32((HttpContext.Current.Session["ObjectIDCombo"].ToString()));
            }

            #endregion

            #region Get GPS X

            decimal gpsX = 0;
            if (HttpContext.Current.Session["GPSX"] != null)
            {
                //Get Info From Session
                gpsX = Convert.ToDecimal((HttpContext.Current.Session["GPSX"].ToString()));
            }

            #endregion

            #region Get GPS Y

            decimal gpsY = 0;
            if (HttpContext.Current.Session["GPSY"] != null)
            {
                //Get Info From Session
                gpsY = Convert.ToDecimal((HttpContext.Current.Session["GPSY"].ToString()));
            }

            #endregion

            #region Get GPS Z

            decimal gpsZ = 0;
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Get Info From Session
                gpsZ = Convert.ToDecimal((HttpContext.Current.Session["GPSZ"].ToString()));
            }

            #endregion

            #region Get Description

            var workDesc = "";
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Get Additional Info From Session
                workDesc = (HttpContext.Current.Session["txtWorkDescription"].ToString());
            }

            #endregion

            #region Get Work Date

            var requestDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkRequestDate"] != null)
            {
                //Get Info From Session
                requestDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkRequestDate"].ToString());
            }

            #endregion

            #region Get Start Date

            var startDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkStartDate"] != null)
            {
                //Get Info From Session
                startDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkStartDate"].ToString());
            }

            #endregion

            #region Get Comp Date

            var compDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkCompDate"] != null)
            {
                //Get Info From Session
                compDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkCompDate"].ToString());
            }

            #endregion

            #region Get Priority

            var requestPriority = -1;
            if ((HttpContext.Current.Session["ComboPriority"] != null))
            {
                //Get Info From Session
                requestPriority = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
            }

            #endregion

            #region Get State Route

            var stateRouteId = -1;
            if ((HttpContext.Current.Session["comboHwyRoute"] != null))
            {
                //Get Info From Session
                stateRouteId = Convert.ToInt32((HttpContext.Current.Session["comboHwyRoute"].ToString()));
            }

            #endregion

            #region Get Milepost

            decimal milepost = 0;
            if (HttpContext.Current.Session["txtMilepost"] != null)
            {
                //Get Info From Session
                milepost = Convert.ToDecimal(HttpContext.Current.Session["txtMilepost"].ToString());
            }

            #endregion

            #region Get Milepost To

            decimal milepostTo = 0;
            if (HttpContext.Current.Session["txtMilepostTo"] != null)
            {
                //Get Info From Session
                milepostTo = Convert.ToDecimal(HttpContext.Current.Session["txtMilepostTo"].ToString());
            }

            #endregion

            #region Get Milepost Direction

            var mpIncreasing = -1;
            if ((HttpContext.Current.Session["comboMilePostDir"] != null))
            {
                //Get Info From Session
                mpIncreasing = Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
            }

            #endregion

            #region Get Work Op

            var workOp = -1;
            if ((HttpContext.Current.Session["ComboWorkOp"] != null))
            {
                //Get Info From Session
                workOp = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
            }

            #endregion

            #region Get Requestor

            var requestor = -1;
            if ((HttpContext.Current.Session["ComboRequestor"] != null))
            {
                //Get Info From Session
                requestor = Convert.ToInt32((HttpContext.Current.Session["ComboRequestor"].ToString()));
            }
            else if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //Set Requestor
                requestor = _oLogon.UserID;
            }

            #endregion

            #region Get Job Reason

            var reasonCode = -1;
            if ((HttpContext.Current.Session["comboReason"] != null))
            {
                //Get Info From Session
                reasonCode = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
            }

            #endregion

            #region Get Route To

            var routeTo = -1;
            if ((HttpContext.Current.Session["comboRouteTo"] != null))
            {
                //Get Info From Session
                routeTo = Convert.ToInt32((HttpContext.Current.Session["comboRouteTo"].ToString()));
            }

            #endregion

            #region Get Cost Code

            var costCodeId = -1;
            if ((HttpContext.Current.Session["ComboCostCode"] != null))
            {
                //Get Info From Session
                costCodeId = Convert.ToInt32((HttpContext.Current.Session["ComboCostCode"].ToString()));
            }

            #endregion

            #region Get Fund Source

            var fundSource = -1;
            if ((HttpContext.Current.Session["ComboFundSource"] != null))
            {
                //Get Info From Session
                fundSource = Convert.ToInt32((HttpContext.Current.Session["ComboFundSource"].ToString()));
            }

            #endregion

            #region Get Work Order

            var workOrder = -1;
            if ((HttpContext.Current.Session["ComboWorkOrder"] != null))
            {
                //Get Info From Session
                workOrder = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOrder"].ToString()));
            }

            #endregion

            #region Get Org Code

            var orgCode = -1;
            if ((HttpContext.Current.Session["ComboOrgCode"] != null))
            {
                //Get Info From Session
                orgCode = Convert.ToInt32((HttpContext.Current.Session["ComboOrgCode"].ToString()));
            }

            #endregion

            #region Get Fund Group

            var fundingGroup = -1;
            if ((HttpContext.Current.Session["ComboFundGroup"] != null))
            {
                //Get Info From Session
                fundingGroup = Convert.ToInt32((HttpContext.Current.Session["ComboFundGroup"].ToString()));
            }

            #endregion

            #region Get Equip Num

            var equipNumber = -1;
            if ((HttpContext.Current.Session["ComboEquipNum"] != null))
            {
                //Get Info From Session
                equipNumber = Convert.ToInt32((HttpContext.Current.Session["ComboEquipNum"].ToString()));
            }

            #endregion

            #region Get Ctl Section

            var controlSection = -1;
            if ((HttpContext.Current.Session["ComboCtlSection"] != null))
            {
                //Get Info From Session
                controlSection = Convert.ToInt32((HttpContext.Current.Session["ComboCtlSection"].ToString()));
            }

            #endregion

            #region Get Notes

            var notes = "";
            if (HttpContext.Current.Session["txtAddDetail"] != null)
            {
                //Get Additional Info From Session
                notes = (HttpContext.Current.Session["txtAddDetail"].ToString());
            }

            #endregion

            #region Get Post Notes

            var postNotes = "";
            if (HttpContext.Current.Session["txtPostNotes"] != null)
            {
                //Get Post Notes From Session
                postNotes = (HttpContext.Current.Session["txtPostNotes"].ToString());
            }

            #endregion

            #region Get Run Units

            //Create Variables
            decimal unitOne = 0;
            decimal unitTwo = 0;
            decimal unitThree = 0;

            //Check For First Unit
            if (HttpContext.Current.Session["txtRunUnitOne"] != null)
            {
                //Get From Session
                unitOne = Convert.ToDecimal((HttpContext.Current.Session["txtRunUnitOne"].ToString()));
            }

            //Check For Second Unit
            if (HttpContext.Current.Session["txtRunUnitTwo"] != null)
            {
                //Get From Session
                unitTwo = Convert.ToDecimal((HttpContext.Current.Session["txtRunUnitTwo"].ToString()));
            }

            //Check For Third Unit
            if (HttpContext.Current.Session["txtRunUnitThree"] != null)
            {
                //Get From Session
                unitThree = Convert.ToDecimal((HttpContext.Current.Session["txtRunUnitThree"].ToString()));
            }

            #endregion

            #region Get Job ID

            var jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

            #endregion

            #region Get Job Step ID

            var jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());

            #endregion

            #region Get Step #

            var jobStepNumber = Convert.ToInt32(HttpContext.Current.Session["stepnumber"].ToString());

            #endregion

            #region Get Concur Step Number

            var jobStepConcurNumber = Convert.ToInt32(HttpContext.Current.Session["concurwithstep"].ToString());

            #endregion

            #region Get Follow Step Number

            var jobStepFollowStepNumber = Convert.ToInt32(HttpContext.Current.Session["followstep"].ToString());

            #endregion

            #region Get Status

            var jobStatus = -1;
            if ((HttpContext.Current.Session["ComboStatus"] != null))
            {
                //Get Info From Session
                jobStatus = Convert.ToInt32((HttpContext.Current.Session["ComboStatus"].ToString()));
            }

            #endregion

            #region Get Laborclass

            var jobLaborClass = -1;
            if ((HttpContext.Current.Session["ComboLaborClass"] != null))
            {
                //Get Info From Session
                jobLaborClass = Convert.ToInt32((HttpContext.Current.Session["ComboLaborClass"].ToString()));
            }

            #endregion

            #region Get Group

            var jobGroup = -1;
            if ((HttpContext.Current.Session["ComboGroup"] != null))
            {
                //Get Info From Session
                jobGroup = Convert.ToInt32((HttpContext.Current.Session["ComboGroup"].ToString()));
            }

            #endregion

            #region Get Outcome

            var jobOutcome = -1;
            if ((HttpContext.Current.Session["ComboOutcome"] != null))
            {
                //Get Info From Session
                jobOutcome = Convert.ToInt32((HttpContext.Current.Session["ComboOutcome"].ToString()));
            }

            #endregion

            #region Get Shift

            var jobShift = -1;
            if ((HttpContext.Current.Session["ComboShift"] != null))
            {
                //Get Info From Session
                jobShift = Convert.ToInt32((HttpContext.Current.Session["ComboShift"].ToString()));
            }

            #endregion

            #region Get Supervisor

            var jobSupervisor = -1;
            if ((HttpContext.Current.Session["ComboSupervisor"] != null))
            {
                //Get Info From Session
                jobSupervisor = Convert.ToInt32((HttpContext.Current.Session["ComboSupervisor"].ToString()));
            }

            #endregion

            #region Get Actual Downtime

            decimal jobActualDt = 0;
            if ((HttpContext.Current.Session["txtActualDownTime"] != null))
            {
                //Get Info From Session
                jobActualDt = Convert.ToDecimal((HttpContext.Current.Session["txtActualDownTime"].ToString()));
            }

            #endregion

            #region Get Actual Length

            decimal jobActualLen = 0;
            if ((HttpContext.Current.Session["txtActualJobLen"] != null))
            {
                //Get Info From Session
                jobActualLen = Convert.ToDecimal((HttpContext.Current.Session["txtActualJobLen"].ToString()));
            }

            #endregion

            #region Get Estimated Downtime

            decimal jobEstimatedDt = 0;
            if ((HttpContext.Current.Session["txtEstimatedDownTime"] != null))
            {
                //Get Info From Session
                jobEstimatedDt = Convert.ToDecimal((HttpContext.Current.Session["txtEstimatedDownTime"].ToString()));
            }

            #endregion

            #region Get Estimated Length

            decimal jobEstimatedLen = 0;
            if ((HttpContext.Current.Session["txtEstimatedJobLen"] != null))
            {
                //Get Info From Session
                jobEstimatedLen = Convert.ToDecimal((HttpContext.Current.Session["txtEstimatedJobLen"].ToString()));
            }

            #endregion

            #region Get Remaining Downtime

            decimal jobRemainingDt = 0;
            if ((HttpContext.Current.Session["txtRemainingDownTime"] != null))
            {
                //Get Info From Session
                jobRemainingDt = Convert.ToDecimal((HttpContext.Current.Session["txtRemainingDownTime"].ToString()));
            }

            #endregion

            #region Get Estimated Length

            decimal jobRemainingLen = 0;
            if ((HttpContext.Current.Session["txtRemainingJobLength"] != null))
            {
                //Get Info From Session
                jobRemainingLen = Convert.ToDecimal((HttpContext.Current.Session["txtRemainingJobLength"].ToString()));
            }

            #endregion

            #region Get Estimated Units

            decimal jobEstimatedUnits = 0;
            if ((HttpContext.Current.Session["txtEstimagedUnits"] != null))
            {
                //Get Info From Session
                jobEstimatedUnits = Convert.ToDecimal((HttpContext.Current.Session["txtEstimagedUnits"].ToString()));
            }

            #endregion

            #region Get Actual Units

            decimal jobActualUnits = 0;
            if ((HttpContext.Current.Session["txtActualUnits"] != null))
            {
                //Get Info From Session
                jobActualUnits = Convert.ToDecimal((HttpContext.Current.Session["txtActualUnits"].ToString()));
            }

            #endregion

            #region Get Return Within

            var jobReturnWithin = 0;
            if ((HttpContext.Current.Session["txtReturnWithin"] != null))
            {
                //Get Info From Session
                jobReturnWithin = Convert.ToInt32((HttpContext.Current.Session["txtReturnWithin"].ToString()));
            }

            #endregion

            #region Get Route To

            var jobRouteTo = -1;
            if ((HttpContext.Current.Session["ComboRouteTo"] != null))
            {
                //Get Info From Session
                jobRouteTo = Convert.ToInt32((HttpContext.Current.Session["ComboRouteTo"].ToString()));
            }

            #endregion

            #region Get Completed By

            var jobCompletedBy = -1;
            if ((HttpContext.Current.Session["ComboCompletedBy"] != null))
            {
                //Get Info From Session
                jobCompletedBy = Convert.ToInt32((HttpContext.Current.Session["ComboCompletedBy"].ToString()));
            }

            #endregion

            #region Get Charge To

            var jobChargeTo = "";
            if ((HttpContext.Current.Session["txtChargeTo"] != null))
            {
                //Get Info From Session
                jobChargeTo = (HttpContext.Current.Session["txtChargeTo"].ToString());
            }

            #endregion

            #region Get Incident Log

            var jobIncidentLog = -1;
            if ((HttpContext.Current.Session["ComboIncidentLog"] != null))
            {
                //Get Info From Session
                jobIncidentLog = Convert.ToInt32((HttpContext.Current.Session["ComboIncidentLog"].ToString()));
            }

            #endregion

            //Clear Errors
            _oJob.ClearErrors();

            try
            {
                //Save Job Details
                if (_oJob.Update(jobId,
                    workDesc,
                    JobType.Corrective,
                    JobAgainstType.MaintenanceObjects,
                    objectAgainstId,
                    actionPriority,
                    reasonCode,
                    notes,
                    routeTo,
                    true,
                    requestDate,
                    requestPriority,
                    requestor,
                    0,
                    0,
                    gpsX,
                    gpsY,
                    gpsZ,
                    unitOne,
                    unitTwo,
                    unitThree,
                    -1,
                    mobileEquip,
                    _oJob.NullDate,
                    routeTo,
                    -1,
                    -1,
                    workOp,
                    -1,
                    subAssemblyID,
                    stateRouteId,
                    milepost,
                    milepostTo,
                    mpIncreasing,
                    additionalDamage,
                    percentOverage,
                    _oLogon.UserID,
                    ref AssignedJobID))
                {
                    //Update Job Step
                    if (!_oJobStep.Update(jobStepId,
                        jobStepNumber,
                        jobStepConcurNumber,
                        jobStepFollowStepNumber,
                        workDesc,
                        jobStatus,
                        jobLaborClass,
                        postNotes,
                        notes,
                        -1,
                        jobGroup,
                        jobOutcome,
                        jobShift,
                        jobSupervisor,
                        jobActualDt,
                        jobActualLen,
                        jobEstimatedDt,
                        jobEstimatedLen,
                        jobRemainingDt,
                        jobRemainingLen,
                        startDate,
                        compDate,
                        jobReturnWithin,
                        fundSource,
                        subAssemblyID,
                        requestPriority,
                        reasonCode,
                        _oLogon.UserID,
                        EditingTimeBachId,
                        EditingTiemBatchItemId))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Job Step -" + _oJobStep.LastError);
                    }

                    //Save Route & Completion Information
                    if (!_oJobStep.UpdateRouteAndCompletionInfo(jobStepId, jobRouteTo, jobCompletedBy, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Route To And Completion Information -" + _oJobStep.LastError);
                    }

                    //Check Run Unit Table
                    if (_oRunUnit.Ds.Tables.Count > 0)
                    {
                        //Check Row Count
                        if (_oRunUnit.Ds.Tables[0].Rows.Count > 0)
                        {
                            //Loop Rows
                            for (var i = 0; i < _oRunUnit.Ds.Tables[0].Rows.Count; i++)
                            {
                                //Determine Unit
                                switch (i)
                                {
                                    case 0:
                                        {
                                            //Check If Not Read Only
                                            if (txtRunUnitOne.ReadOnly == false)
                                            {
                                                //Update Run Unit
                                                if (!_oRunUnit.Update((int)_oRunUnit.Ds.Tables[0].Rows[i][0],
                                                    Convert.ToDecimal(
                                                        txtRunUnitOne.Value.ToString()),
                                                    _oRunUnit.Ds.Tables[0].Rows[i][3].
                                                        ToString(),
                                                    (int)_oRunUnit.Ds.Tables[0].Rows[i][4],
                                                    _oLogon.UserID))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Updating Run Units -" + _oRunUnit.LastError);
                                                }
                                            }
                                            break;
                                        }
                                    case 1:
                                        {
                                            //Check If Not Read Only
                                            if (txtRunUnitTwo.ReadOnly == false)
                                            {
                                                //Update Run Unit
                                                if (!_oRunUnit.Update((int)_oRunUnit.Ds.Tables[0].Rows[i][0],
                                                    Convert.ToDecimal(
                                                        txtRunUnitTwo.Value.ToString()),
                                                    _oRunUnit.Ds.Tables[0].Rows[i][3].
                                                        ToString(),
                                                    (int)_oRunUnit.Ds.Tables[0].Rows[i][4],
                                                    _oLogon.UserID))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Updating Run Units -" + _oRunUnit.LastError);
                                                }
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            //Check If Not Read Only
                                            if (txtRunUnitThree.ReadOnly == false)
                                            {
                                                //Update Run Unit
                                                if (!_oRunUnit.Update((int)_oRunUnit.Ds.Tables[0].Rows[i][0],
                                                    Convert.ToDecimal(
                                                        txtRunUnitThree.Value.ToString()),
                                                    _oRunUnit.Ds.Tables[0].Rows[i][3].
                                                        ToString(),
                                                    (int)_oRunUnit.Ds.Tables[0].Rows[i][4],
                                                    _oLogon.UserID))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Updating Run Units -" + _oRunUnit.LastError);
                                                }
                                            }
                                            break;
                                        }
                                }
                            }

                            //Update Completed Units
                            _oJob.ClearErrors();

                            //Create Temp Variables
                            decimal tmpUnit1 = 0;
                            decimal tmpUnit2 = 0;
                            decimal tmpUnit3 = 0;

                            //Check For Valid Input
                            if ((HttpContext.Current.Session["txtRunUnitOne"] != null) && (HttpContext.Current.Session["txtRunUnitOne"].ToString() != ""))
                            {
                                //Get Unit 1
                                tmpUnit1 = Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitOne"]);
                            }

                            //Check For Valid Input
                            if ((HttpContext.Current.Session["txtRunUnitTwo"] != null) && (HttpContext.Current.Session["txtRunUnitTwo"].ToString() != ""))
                            {
                                //Get Unit 2
                                tmpUnit2 = Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitTwo"]);
                            }

                            //Check For Valid Input
                            if ((HttpContext.Current.Session["txtRunUnitThree"] != null) && (HttpContext.Current.Session["txtRunUnitThree"].ToString() != ""))
                            {
                                //Get Unit 3
                                tmpUnit3 = Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitThree"]);
                            }

                            //Update Completed Units
                            if (!_oJob.UpdateCompletedUnits(jobId, tmpUnit1, tmpUnit2, tmpUnit3))
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Updating Completed Units -" + _oJob.LastError);
                            }
                        }
                    }

                    //Update Charge To
                    if (!_oJobStep.UpdateChargeTo(jobId, jobStepId, jobChargeTo, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Charge To -" + _oJobStep.LastError);
                    }

                    //Update Incident Log Link
                    if (!_oJobStep.UpdateIncidentLogLink(jobId, jobStepId, jobIncidentLog, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Incident Log Link -" + _oJobStep.LastError);
                    }

                    //Update Production Units
                    if (!_oJob.UpdateProductionUnits(jobId, jobEstimatedUnits, jobActualUnits, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Production Units -" + _oJob.LastError);
                    }

                    //Update GPS Coordinates
                    if (
                        !_oJob.UpdateGpsCoordinates(jobId,
                                                    gpsX,
                                                    gpsY,
                                                    gpsZ))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating GPS Coordinates -" + _oJob.LastError);
                    }

                    //Update Costing Information
                    if (
                        !_oJob.UpdateJobCosting(
                            Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()),
                            costCodeId,
                            fundSource,
                            workOrder,
                            workOp,
                            orgCode,
                            fundingGroup,
                            equipNumber,
                            controlSection,
                            _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Job Costing -" + _oJob.LastError);
                    }
                    //Check For Value
                    if (HttpContext.Current.Session["AssignedJobID"] != null)
                    {
                        //Get Additional Info From Session
                        lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                        //Check For Step
                        if (HttpContext.Current.Session["editingJobStepNum"] != null)
                        {
                            //Set Text
                            lblStep.Text = @"STEP #" +
                                           (HttpContext.Current.Session["editingJobStepNum"]);
                        }

                        //Setup For Editing
                        SetupForEditing();
                    }

                    //Success Return True
                    return true;
                }

                //Throw Error
                throw new SystemException(
                    @"Error Updating Job -" + _oJob.LastError);
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                //Return False To Prevent Navigation
                return false;
            }
        }

        /// <summary>
        /// Adds New Work Request
        /// </summary>
        /// <returns>True/False For Success</returns>
        protected bool AddRequest()
        {
            //Set Defaults
            const bool requestOnly = true;
            const int actionPriority = -1;
            const int mobileEquip = -1;
            const bool additionalDamage = false;
            const decimal percentOverage = 0;
            const int subAssemblyID = -1;
            var newJobID = "";
            var errorFromJobIDGeneration = "";
            var poolTypeForJob = JobPoolType.Global;

            //Get Requestor
            var requestor = -1;
            if ((HttpContext.Current.Session["ComboRequestor"] != null))
            {
                //Get Info From Session
                requestor = Convert.ToInt32((HttpContext.Current.Session["ComboRequestor"].ToString()));
            }
            else if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //Set Requestor
                requestor = _oLogon.UserID;
            }

            //Get Object ID
            var objectAgainstId = -1;
            if (HttpContext.Current.Session["ObjectIDCombo"] != null)
            {
                //Get Info From Session
                objectAgainstId = Convert.ToInt32((HttpContext.Current.Session["ObjectIDCombo"].ToString()));
            }

            //Get Description
            var workDesc = "";
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Get Additional Info From Session
                workDesc = (HttpContext.Current.Session["txtWorkDescription"].ToString());
            }

            //Get Work Date
            var requestDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkRequestDate"] != null)
            {
                //Get Info From Session
                requestDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkRequestDate"].ToString());
            }

            //Get Priority
            var requestPriority = -1;
            if ((HttpContext.Current.Session["ComboPriority"] != null))
            {
                //Get Info From Session
                requestPriority = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
            }

            //Get State Route
            var stateRouteId = -1;
            if ((HttpContext.Current.Session["comboHwyRoute"] != null))
            {
                //Get Info From Session
                stateRouteId = Convert.ToInt32((HttpContext.Current.Session["comboHwyRoute"].ToString()));
            }

            //Get Milepost
            decimal milepost = 0;
            if (HttpContext.Current.Session["txtMilepost"] != null)
            {
                //Get Info From Session
                milepost = Convert.ToDecimal(HttpContext.Current.Session["txtMilepost"].ToString());
            }

            //Get Milepost To
            decimal milepostTo = 0;
            if (HttpContext.Current.Session["txtMilepostTo"] != null)
            {
                //Get Info From Session
                milepostTo = Convert.ToDecimal(HttpContext.Current.Session["txtMilepostTo"].ToString());
            }

            //Get Milepost Direction
            var mpIncreasing = -1;
            if ((HttpContext.Current.Session["comboMilePostDir"] != null))
            {
                //Get Info From Session
                mpIncreasing = Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
            }

            //Get Work Op
            var workOp = -1;
            if ((HttpContext.Current.Session["ComboWorkOp"] != null))
            {
                //Get Info From Session
                workOp = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
            }

            //Get Job Reason
            var reasonCode = -1;
            if ((HttpContext.Current.Session["comboReason"] != null))
            {
                //Get Info From Session
                reasonCode = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
            }

            //Get Route To
            var routeTo = -1;
            if ((HttpContext.Current.Session["comboRouteTo"] != null))
            {
                //Get Info From Session
                routeTo = Convert.ToInt32((HttpContext.Current.Session["comboRouteTo"].ToString()));
            }

            //Get Cost Code
            var costCodeId = -1;
            if ((HttpContext.Current.Session["ComboCostCode"] != null))
            {
                //Get Info From Session
                costCodeId = Convert.ToInt32((HttpContext.Current.Session["ComboCostCode"].ToString()));
            }

            //Get Fund Source
            var fundSource = -1;
            if ((HttpContext.Current.Session["ComboFundSource"] != null))
            {
                //Get Info From Session
                fundSource = Convert.ToInt32((HttpContext.Current.Session["ComboFundSource"].ToString()));
            }

            //Get Work Order
            var workOrder = -1;
            if ((HttpContext.Current.Session["ComboWorkOrder"] != null))
            {
                //Get Info From Session
                workOrder = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOrder"].ToString()));
            }

            //Get Org Code
            var orgCode = -1;
            if ((HttpContext.Current.Session["ComboOrgCode"] != null))
            {
                //Get Info From Session
                orgCode = Convert.ToInt32((HttpContext.Current.Session["ComboOrgCode"].ToString()));
            }

            //Get Fund Group
            var fundingGroup = -1;
            if ((HttpContext.Current.Session["ComboFundGroup"] != null))
            {
                //Get Info From Session
                fundingGroup = Convert.ToInt32((HttpContext.Current.Session["ComboFundGroup"].ToString()));
            }

            //Get Equip Num
            var equipNumber = -1;
            if ((HttpContext.Current.Session["ComboEquipNum"] != null))
            {
                //Get Info From Session
                equipNumber = Convert.ToInt32((HttpContext.Current.Session["ComboEquipNum"].ToString()));
            }

            //Get Ctl Section
            var controlSection = -1;
            if ((HttpContext.Current.Session["ComboCtlSection"] != null))
            {
                //Get Info From Session
                controlSection = Convert.ToInt32((HttpContext.Current.Session["ComboCtlSection"].ToString()));
            }

            //Get Notes
            var notes = "";
            if (HttpContext.Current.Session["txtAddDetail"] != null)
            {
                //Get Additional Info From Session
                notes = (HttpContext.Current.Session["txtAddDetail"].ToString());
            }

            //Get GPS X
            decimal gpsX = 0;
            if (HttpContext.Current.Session["GPSX"] != null)
            {
                //Get Info From Session
                gpsX = Convert.ToDecimal((HttpContext.Current.Session["GPSX"].ToString()));
            }

            //Get GPS Y
            decimal gpsY = 0;
            if (HttpContext.Current.Session["GPSY"] != null)
            {
                //Get Info From Session
                gpsY = Convert.ToDecimal((HttpContext.Current.Session["GPSX"].ToString()));
            }

            //Get GPS Z
            decimal gpsZ = 0;
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Get Info From Session
                gpsZ = Convert.ToDecimal((HttpContext.Current.Session["GPSX"].ToString()));
            }

            //Clear Errors
            _oJob.ClearErrors();

            try
            {
                if (HttpContext.Current.Session["LogonInfo"] != null)
                {
                    //Get Logon Info From Session
                    _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                    //Setup Job ID Generator For Logon
                    _oJobIDGenerator =
                        new JobIdGenerator(_connectionString, _useWeb, _oLogon.UserID, _oLogon.AreaID);
                }
                else
                {
                    //Setup For Non Logged In User
                    _oJobIDGenerator =
                        new JobIdGenerator(_connectionString, _useWeb, -1, -1);
                }

                //Clear Errors
                _oJobIDGenerator.ClearErrors();

                //Get Pool
                if (!_oJobIDGenerator.GetJobPoolInUse(ref poolTypeForJob))
                {
                    //Return False
                    return false;
                }

                //Add Job
                if (_oJob.Add(true,
                    JobType.Corrective,
                    JobAgainstType.MaintenanceObjects,
                    objectAgainstId,
                    workDesc,
                    actionPriority,
                    reasonCode,
                    notes,
                    0, 0, true,
                    -1,
                    requestDate,
                    requestPriority,
                    requestor,
                    _oJob.NullDate,
                    gpsX, gpsY, gpsZ,
                    -1,
                    mobileEquip,
                    0, 0, 0, -1,
                    routeTo,
                    -1, -1,
                    workOp,
                    subAssemblyID,
                    stateRouteId,
                    milepost,
                    milepostTo,
                    mpIncreasing,
                    additionalDamage,
                    percentOverage,
                    ref AssignedJobID,
                    ref errorFromJobIDGeneration,
                    ref AssignedGuid,
                    requestor))
                {
                    //Add Job To Session
                    HttpContext.Current.Session.Add("oJob", _oJob);
                    HttpContext.Current.Session.Add("editingJobID", _oJob.RecordID);
                    HttpContext.Current.Session.Add("AssignedJobID", AssignedJobID);

                    //Set Text
                    lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                    //Set Text
                    lblStep.Text = @"STEP #1";

                    //Update Costing Information
                    if (!_oJob.UpdateJobCosting(_oJob.RecordID,
                        costCodeId,
                        fundSource,
                        workOrder,
                        workOp,
                        orgCode,
                        fundingGroup,
                        equipNumber,
                        controlSection,
                        _oLogon.UserID))
                    {
                        //Return False To Prevent Navigation
                        return false;
                    }

                    //Setup For Editing
                    SetupForEditing();

                    //Return True
                    return true;
                }

                //Return False To Prevent Navigation
                return false;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                //Return False To Prevent Navigation
                return false;
            }
        }

        /// <summary>
        /// Saves Session Data
        /// </summary>
        protected void SaveSessionData()
        {
            #region Request Description 

            //Check For Input
            if (txtWorkDescription.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtWorkDescription"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtWorkDescription");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtWorkDescription", txtWorkDescription.Text.Trim());
            }

            #endregion

            #region Additional Details

            //Check For Input
            if (txtAdditionalInfo.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtAddDetail"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtAddDetail");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtAddDetail", txtAdditionalInfo.Text.Trim());
            }

            #endregion

            #region Post Notes

            //Check For Input
            if (txtPostNotes.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtPostNotes"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtPostNotes");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtPostNotes", txtPostNotes.Text.Trim());
            }

            #endregion

            #region First Name

            if (txtFN.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtFN"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtFN");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtFN", txtFN.Value.ToString());
            }

            #endregion

            #region Last Name

            if (txtLN.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtLN"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtLN");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtLN", txtLN.Value.ToString());
            }

            #endregion

            #region Email

            if (txtEmail.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtEmail"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtEmail");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtEmail", txtEmail.Value.ToString());
            }

            #endregion

            #region Phone

            if (txtPhone.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtPhone"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtPhone");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtPhone", txtPhone.Value.ToString());
            }

            #endregion

            #region Ext

            if (txtExt.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtExt"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtExt");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtExt", txtExt.Value.ToString());
            }

            #endregion

            #region Mail

            if (txtMail.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtMail"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtMail");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtMail", txtMail.Value.ToString());
            }

            #endregion

            #region Building

            if (txtBuilding.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtBuilding"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtBuilding");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtBuilding", txtBuilding.Value.ToString());
            }

            #endregion

            #region Room

            if (txtRoomNum.Text.Length > 0)
            {
                //Check For Prior Value
                if (HttpContext.Current.Session["txtRoomNum"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtRoomNum");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtRoomNum", txtRoomNum.Value.ToString());
            }

            #endregion

            #region Service Office

            if (ComboServiceOffice.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboServiceOffice"] != null)
                {
                    if (HttpContext.Current.Session["ComboServiceOfficeText"].ToString() != ComboServiceOffice.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboServiceOffice");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboServiceOffice", ComboServiceOffice.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboServiceOffice", ComboServiceOffice.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboServiceOfficeText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboServiceOfficeText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboServiceOfficeText", ComboServiceOffice.Text.Trim());

                #endregion
            }

            #endregion

            #region Cost Code

            if (ComboCostCode.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboCostCode"] != null)
                {
                    //Check For Change
                    if (HttpContext.Current.Session["ComboCostCodeText"].ToString() != ComboCostCode.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboCostCode");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboCostCode", ComboCostCode.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboCostCode", ComboCostCode.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboCostCodeText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboCostCodeText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboCostCodeText", ComboCostCode.Text.Trim());

                #endregion
            }

            #endregion

            #region Fund Source

            if (ComboFundSource.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboFundSource"] != null)
                {
                    //Check For Change
                    if (HttpContext.Current.Session["ComboFundSourceText"].ToString() != ComboFundSource.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboFundSource");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboFundSource", ComboFundSource.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboFundSource", ComboFundSource.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboFundSourceText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboFundSourceText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboFundSourceText", ComboFundSource.Text.Trim());

                #endregion
            }

            #endregion

            #region Work Order

            if (ComboWorkOrder.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboWorkOrder"] != null)
                {
                    if (HttpContext.Current.Session["ComboWorkOrderText"].ToString() != ComboWorkOrder.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboWorkOrder");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboWorkOrder", ComboWorkOrder.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboWorkOrder", ComboWorkOrder.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboWorkOrderText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboWorkOrderText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboWorkOrderText", ComboWorkOrder.Text.Trim());

                #endregion
            }

            #endregion

            #region Work Op

            if (ComboWorkOp.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboWorkOp"] != null)
                {
                    if (HttpContext.Current.Session["ComboWorkOpText"].ToString() != ComboWorkOp.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboWorkOp");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboWorkOp", ComboWorkOp.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboWorkOp", ComboWorkOp.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboWorkOpText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboWorkOpText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboWorkOpText", ComboWorkOp.Text.Trim());

                #endregion
            }

            #endregion

            #region Org Code

            if (ComboOrgCode.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboOrgCode"] != null)
                {
                    if (HttpContext.Current.Session["ComboOrgCodeText"].ToString() != ComboOrgCode.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboOrgCode");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboOrgCode", ComboOrgCode.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboOrgCode", ComboOrgCode.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboOrgCodeText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboOrgCodeText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboOrgCodeText", ComboOrgCode.Text.Trim());

                #endregion
            }

            #endregion

            #region Fund Group

            if (ComboFundGroup.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboFundGroup"] != null)
                {
                    if (HttpContext.Current.Session["ComboFundGroupText"].ToString() != ComboFundGroup.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboFundGroup");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboFundGroup", ComboFundGroup.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboFundGroup", ComboFundGroup.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboFundGroupText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboFundGroupText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboFundGroupText", ComboFundGroup.Text.Trim());

                #endregion
            }

            #endregion

            #region Ctl Section

            if (ComboCtlSection.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboCtlSection"] != null)
                {
                    if (HttpContext.Current.Session["ComboCtlSectionText"].ToString() != ComboCtlSection.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboCtlSection");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboCtlSection", ComboCtlSection.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboCtlSection", ComboCtlSection.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboCtlSectionText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboCtlSectionText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboCtlSectionText", ComboCtlSection.Text.Trim());

                #endregion
            }

            #endregion

            #region Equip Num

            if (ComboEquipNum.Value != null)
            {
                #region Combo Value
                //Check For Prior Value
                if (HttpContext.Current.Session["ComboEquipNum"] != null)
                {
                    if (HttpContext.Current.Session["ComboEquipNumText"].ToString() != ComboEquipNum.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboEquipNum");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboEquipNum", ComboEquipNum.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboEquipNum", ComboEquipNum.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboEquipNumText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboEquipNumText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboEquipNumText", ComboEquipNum.Text.Trim());

                #endregion
            }

            #endregion

            #region GPS X

            //Check For Prior Value
            if (HttpContext.Current.Session["GPSX"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("GPSX");
            }

            //Check For Value
            if (GPSX.Value != null)
            {
                //Add New Value
                HttpContext.Current.Session.Add("GPSX", GPSX.Value.ToString());
            }

            #endregion

            #region GPS Y

            //Check For Prior Value
            if (HttpContext.Current.Session["GPSY"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("GPSY");
            }

            //Check For Value
            if (GPSY.Value != null)
            {
                //Add New Value
                HttpContext.Current.Session.Add("GPSY", GPSY.Value.ToString());
            }

            #endregion

            #region GPS Z

            //Check For Prior Value
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("GPSZ");
            }

            //Check For Value
            if (GPSZ.Value != null)
            {
                //Add New Value
                HttpContext.Current.Session.Add("GPSZ", GPSZ.Value.ToString());
            }

            #endregion

            #region Object Info

            //Check For Input
            if (ObjectIDCombo.Value != null)
            {
                //See If Selection Changed

                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["ObjectIDCombo"] != null)
                {
                    //See If Value Changed
                    if (HttpContext.Current.Session["ObjectIDComboText"].ToString() != ObjectIDCombo.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ObjectIDCombo");

                        //Add New Value
                        HttpContext.Current.Session.Add("ObjectIDCombo", ObjectIDCombo.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ObjectIDCombo", ObjectIDCombo.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ObjectIDComboText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ObjectIDComboText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ObjectIDComboText", ObjectIDCombo.Text.Trim());

                #endregion

                #region Description

                //Check For Prior Value
                if (HttpContext.Current.Session["txtObjectDescription"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtObjectDescription");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtObjectDescription", txtObjectDescription.Text.Trim());

                #endregion

                #region Area

                //Check For Prior Value
                if (HttpContext.Current.Session["txtObjectArea"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtObjectArea");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtObjectArea", txtObjectArea.Text.Trim());

                #endregion

                #region Location

                //Check For Prior Value
                if (HttpContext.Current.Session["txtObjectLocation"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtObjectLocation");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtObjectLocation", txtObjectLocation.Text.Trim());

                #endregion

                #region Asset

                //Check For Prior Value
                if (HttpContext.Current.Session["txtObjectAssetNumber"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtObjectAssetNumber");
                }

                //Add New Value
                HttpContext.Current.Session.Add("txtObjectAssetNumber", txtObjectAssetNumber.Text.Trim());

                #endregion
            }

            #endregion

            #region Request Date

            //Check For Prior Value
            if (HttpContext.Current.Session["TxtWorkRequestDate"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("TxtWorkRequestDate");
            }

            //Add New Value
            HttpContext.Current.Session.Add("TxtWorkRequestDate", TxtWorkRequestDate.Value.ToString());

            #endregion

            #region Requestor

            if (ComboRequestor.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboRequestor"] != null)
                {
                    //See If Value Changed
                    if (HttpContext.Current.Session["ComboRequestorText"].ToString() != ComboRequestor.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboRequestor");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboRequestor", ComboRequestor.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboRequestor", ComboRequestor.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboRequestorText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboRequestorText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboRequestorText", ComboRequestor.Text.Trim());

                #endregion
            }

            #endregion

            #region Priority

            if (ComboPriority.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboPriority"] != null)
                {
                    //See If Value Changed
                    if (HttpContext.Current.Session["ComboPriorityText"].ToString() != ComboPriority.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ComboPriority");

                        //Add New Value
                        HttpContext.Current.Session.Add("ComboPriority", ComboPriority.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("ComboPriority", ComboPriority.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["ComboPriorityText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("ComboPriorityText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("ComboPriorityText", ComboPriority.Text.Trim());

                #endregion
            }

            #endregion

            #region Reason

            if (comboReason.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["comboReason"] != null)
                {
                    //See If Value Changed
                    if (HttpContext.Current.Session["comboReasonText"].ToString() != comboReason.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("comboReason");

                        //Add New Value
                        HttpContext.Current.Session.Add("comboReason", comboReason.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("comboReason", comboReason.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["comboReasonText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("comboReasonText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("comboReasonText", comboReason.Text.Trim());

                #endregion
            }

            #endregion

            #region Route To

            if (comboRouteTo.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["comboRouteTo"] != null)
                {
                    //See If Value Changed
                    if (HttpContext.Current.Session["comboRouteToText"].ToString() != comboRouteTo.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("comboRouteTo");

                        //Add New Value
                        HttpContext.Current.Session.Add("comboRouteTo", comboRouteTo.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("comboRouteTo", comboRouteTo.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["comboRouteToText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("comboRouteToText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("comboRouteToText", comboRouteTo.Text.Trim());

                #endregion
            }

            #endregion

            #region Hwy Route

            if (comboHwyRoute.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["comboHwyRoute"] != null)
                {
                    if (HttpContext.Current.Session["comboHwyRouteText"].ToString() != comboHwyRoute.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("comboHwyRoute");

                        //Add New Value
                        HttpContext.Current.Session.Add("comboHwyRoute", comboHwyRoute.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("comboHwyRoute", comboHwyRoute.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["comboHwyRouteText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("comboHwyRouteText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("comboHwyRouteText", comboHwyRoute.Text.Trim());

                #endregion
            }

            #endregion

            #region Milepost

            //Check For Prior Value
            //if (HttpContext.Current.Session["txtMilepost"] != null)
            //{
            //    //Remove Old One
            //    HttpContext.Current.Session.Remove("txtMilepost");
            //}

            ////Add New Value
            //HttpContext.Current.Session.Add("txtMilepost", txtMilepost.Value.ToString());

            #endregion

            #region Milepost To

            //Check For Prior Value
            if (HttpContext.Current.Session["txtMilepostTo"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtMilepostTo");
            }

            //Check Value
            if (txtMilepost.Value != null)
            {
                //Add New Value
                HttpContext.Current.Session.Add("txtMilepostTo", txtMilepostTo.Value.ToString());

            }

            #endregion

            #region Milepost Direction

            if (comboMilePostDir.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["comboMilePostDir"] != null)
                {
                    if (HttpContext.Current.Session["comboMilePostDirText"].ToString() != comboMilePostDir.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("comboMilePostDir");

                        //Add New Value
                        HttpContext.Current.Session.Add("comboMilePostDir", comboMilePostDir.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("comboMilePostDir", comboMilePostDir.Value.ToString());
                }

                #endregion

                #region Combo Text

                //Check For Prior Value
                if (HttpContext.Current.Session["comboMilePostDirText"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("comboMilePostDirText");
                }

                //Add New Value
                HttpContext.Current.Session.Add("comboMilePostDirText", comboMilePostDir.Text.Trim());

                #endregion
            }

            #endregion

            #region Run Unit One

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRunUnitOne"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRunUnitOne");
            }

            //Add New Value
            //HttpContext.Current.Session.Add("txtRunUnitOne", txtRunUnitOne.Value.ToString());

            #endregion

            #region Run Unit Two

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRunUnitTwo"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRunUnitTwo");
            }

            //Add New Value
            //HttpContext.Current.Session.Add("txtRunUnitTwo", txtRunUnitTwo.Value.ToString());

            #endregion

            #region Run Unit Three

            //Check For Prior Value
            if (HttpContext.Current.Session["txtRunUnitThree"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtRunUnitThree");
            }

            //Add New Value
            //HttpContext.Current.Session.Add("txtRunUnitThree", txtRunUnitThree.Value.ToString());

            #endregion

            #region Get Return Within


            //var jobReturnWithin = Convert.ToInt32((HttpContext.Current.Session["txtReturnWithin"].ToString()));
            if ((HttpContext.Current.Session["txtReturnWithin"] != null))
            {
                HttpContext.Current.Session.Add("txtReturnWithin", txtReturnWithin.Value.ToString());
                //Get Info From Session
            }

            #endregion

            if (HttpContext.Current.Session["ComboOutcome"] != null)
            {
                HttpContext.Current.Session.Add("ComboOutcome", ComboOutcome.Value);
            }

            #region Start and Completion Dates
            if (HttpContext.Current.Session["TxtWorkStartDate"] != null)
            {
                //Set Value
                HttpContext.Current.Session.Add("TxtWorkStartDate", TxtWorkStartDate.Value.ToString());

            }

            //Add Comp Date
            if (HttpContext.Current.Session["TxtWorkCompDate"] != null)
            {

                //Set Value
                HttpContext.Current.Session.Add("TxtWorkCompDate", TxtWorkCompDate.Value.ToString());

            }
            #endregion

            #region Completed By
            if (HttpContext.Current.Session["ComboCompletedBy"] != null)
            {
                HttpContext.Current.Session.Add("ComboCompletedBy", ComboCompletedBy.Value);
            }
            #endregion

            GetStepTab();
        }

        /// <summary>
        /// Sets Up Run Units For Specified Object
        /// </summary>
        /// <param name="objTypeAgainst">Sending Object</param>
        /// <summary>
        /// Deletes Current Job Step
        /// </summary>
        protected void JobStepDeletionRoutine()
        {
            //Delete Current Step
            try
            {
                //Check Permissions
                if (_userCanDelete)
                {
                    //Check For Job Step ID
                    if (Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()) > 0)
                    {
                        //Delete Step
                        if (_oJobStep.Delete(Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()), _oLogon.UserID))
                        {
                            //Send Back To Planned Jobs List
                            Response.Redirect("~/Pages/PlannedJobs/PlannedJobsList.aspx", true);
                        }
                        else
                        {
                            //Throw Error
                            throw new SystemException(
                                @"Jobstep Deletion Error - " + _oJobStep.LastError);
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
        /// Copies Selected Job
        /// </summary>
        protected void CopyJobRoutine()
        {
            //Copy Selected Job
            try
            {
                //Check For Job ID
                if (Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()) > 0)
                {
                    //Check For Job Step ID
                    if (Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()) > 0)
                    {
                        //Check For Step Number
                        if (Convert.ToInt32(HttpContext.Current.Session["editingJobStepNum"].ToString()) > 0)
                        {
                            //Get Values
                            var jobstepKey = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());
                            var jobKey = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());
                            var jobStepNum = Convert.ToInt32(HttpContext.Current.Session["editingJobStepNum"].ToString());
                            var newJobKey = -1;
                            var newJobStep = -1;
                            var newRequestJobId = "";
                            var newJobRequestGuid = "";
                            var cloneError = "";
                            var found = false;
                            var jobtype = JobType.Unknown;

                            //Copy Job
                            if (_oJob.ClonePlannedWorkOrder(jobKey,
                                ref newJobKey,
                                ref newJobStep,
                                ref newRequestJobId,
                                ref cloneError,
                                ref newJobRequestGuid,
                                _oLogon.UserID))
                            {
                                //Issue New Job
                                if (_oJob.SetJobIssuedStatus(newJobKey, true, _oLogon.UserID))
                                {
                                    //Get Job Info
                                    if (_oJob.GetJobstepKeyFromJobAndStep(newJobKey, jobStepNum, ref found,
                                        ref newJobStep,
                                        ref newJobRequestGuid,
                                        ref newRequestJobId, ref jobtype))
                                    {
                                        //Check Flag
                                        if (found)
                                        {
                                            //Check Error
                                            if (cloneError != "")
                                            {
                                                //Throw Error
                                                throw new SystemException(
                                                    @"Error Copying Job Request - "
                                                    + cloneError);
                                            }

                                            //Reset Session
                                            ResetSession();

                                            //Forward User To Copied Work Order
                                            Response.Redirect(
                                                "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + newJobStep, true);
                                        }
                                        else
                                        {
                                            //Throw Error
                                            throw new SystemException(
                                                @"Error Copying Jobstep - " + _oJob.LastError);
                                        }
                                    }
                                    else
                                    {
                                        //Throw Error
                                        throw new SystemException(
                                            @"Error Copying Jobstep - " + _oJob.LastError);
                                    }
                                }
                                else
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Issuing Jobstep - " + _oJob.LastError);
                                }
                            }
                            else
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Copying Jobstep - " + _oJob.LastError);
                            }
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
        /// Issues Selected Jobs
        /// </summary>
        protected void IssueRoutine()
        {
            //Issue Selected Jobs
            try
            {
                //Check Permissions
                if (_userCanEdit)
                {
                    //Check For Job ID
                    if (Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()) > 0)
                    {
                        //Check For Job Step ID
                        if (Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString()) > 0)
                        {
                            //Get Record IDs
                            var jobKey = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());
                            var jobstepKey = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());

                            //Clear Errors
                            _oJob.ClearErrors();

                            //Load Job Data
                            if (_oJob.LoadData(jobKey))
                            {
                                //Check Table Cont
                                if (_oJob.Ds.Tables.Count > 0)
                                {
                                    //Check Row COunt
                                    if (_oJob.Ds.Tables[0].Rows.Count == 1)
                                    {
                                        //Get History Flag
                                        var isHist = (_oJob.Ds.Tables[0].Rows[0][30].ToString().ToUpper() == "Y");

                                        //Check Flag
                                        if (isHist == false)
                                        {
                                            //Issue Job
                                            if (!_oJob.SetJobIssuedStatus(jobKey, true, _oLogon.UserID))
                                            {
                                                //Throw Error
                                                throw new SystemException(
                                                    @"Error Issuing Job - " + _oJob.LastError);
                                            }
                                            else
                                            {
                                                //Reset Session
                                                ResetSession();

                                                //Redirect Page To Reload Data
                                                Response.Redirect(
                                                    "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + jobstepKey, true);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Issuing Job - " + _oJob.LastError);
                            }
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

        protected void NewJobStepRoutine()
        {
            //NJobstepId = 0;

            ////Load Job Step Detail
            //FillWorkOrderScreen(NJobstepId);
            ////
            ////Must enable the procedures tab in case user defined
            ////value entries are used, so that we can save the record.
            ////Otherwise, if the user has a required entry they will
            ////not be able to save (adding new jobstep). 
            ////
            ////6/13/08 CW version 1.0.0.8
            ////
            ////this.JSP5_JobstepDetailNotes.Enabled = false;
            //JSP5_JobstepDetailNotes.Enabled = true;
            //JSP2_JobParts.Enabled = false;
            //JSP6_JobstepCompletionNotes.Enabled = false;
            //JSP3_JobCrews.Enabled = false;
            //JSP7_Attachments.Enabled = false;
            //JSP4_JobEquipment.Enabled = false;
        }

        protected void PlanJobRoutine()
        {
            //Plan Selected Jobs
            try
            {
                //Get Logon Info
                if (HttpContext.Current.Session["LogonInfo"] != null)
                {
                    //Get Logon Info From Session
                    _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
                }

                //Check For Job ID
                if (Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()) > 0)
                {
                    //Get ID
                    var recordToPlan = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

                    //Validate Work Operation Selection
                    //Approver Must Be Allowed To Approve For Specified Work Operation
                    if (_oLogon.ValidateWorkOperations)
                    {
                        //Check For Work Op/Type ID
                        if ((HttpContext.Current.Session["ComboWorkOp"] != null))
                        {
                            //Get ID
                            var workOpId = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));

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
                    if ((HttpContext.Current.Session["ComboPriority"] != null))
                    {
                        //Set Value
                        priority = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
                    }

                    //ReasonCode
                    var reasonCode = -1;
                    if ((HttpContext.Current.Session["comboReason"] != null))
                    {
                        //Set Value
                        reasonCode = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
                    }

                    //Mobile Equipment
                    const int mobileEquip = -1;

                    //Sub Assembly
                    const int subAssemblyId = -1;

                    //Title
                    var jobTitle = "";
                    if (HttpContext.Current.Session["txtWorkDescription"] != null)
                    {
                        //Set Value
                        jobTitle = (HttpContext.Current.Session["txtWorkDescription"].ToString());
                    }

                    //Additional Details
                    var jobAdditionalInfo = "";
                    if (HttpContext.Current.Session["txtAddDetail"] != null)
                    {
                        //Set Value
                        jobAdditionalInfo = (HttpContext.Current.Session["txtAddDetail"].ToString());
                    }

                    //Add Default Step
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
                            var userId = ((HttpContext.Current.Session["ComboRequestor"] != null))
                                ? Convert.ToInt32((HttpContext.Current.Session["ComboRequestor"].ToString()))
                                : _oLogon.UserID;

                            //Create Group Class
                            using (
                                var oGroup =
                                    new MaintenanceGroup(_connectionString, _useWeb, _oLogon.UserID))
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
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        public void AddJobStep()
        {
            #region Get Job ID

            var jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

            #endregion

            #region Get Start Date

            var startDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkStartDate"] != null)
            {
                //Get Info From Session
                startDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkStartDate"].ToString());
            }

            #endregion

            #region Get Comp Date

            var compDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkCompDate"] != null)
            {
                //Get Info From Session
                compDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkCompDate"].ToString());
            }

            #endregion

            var workDesc = "";
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Get Additional Info From Session
                workDesc = (HttpContext.Current.Session["txtWorkDescription"].ToString());
            }

            var priority = -1;

            if ((HttpContext.Current.Session["ComboPriority"] != null))
            {
                //Get Info From Session
                priority = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
            }

            var status = -1;
            if ((HttpContext.Current.Session["ComboStatus"] != null))
            {
                //Get Info From Session
                status = Convert.ToInt32((HttpContext.Current.Session["ComboStatus"].ToString()));
            }

            var reasonCodeId = -1;
            if ((HttpContext.Current.Session["ComboReason"] != null))
            {
                //Get Info From Session
                reasonCodeId = Convert.ToInt32((HttpContext.Current.Session["ComboReason"].ToString()));
            }


            var fundSource = -1;
            if ((HttpContext.Current.Session["ComboFundSource"] != null))
            {
                //Get Info From Session
                fundSource = Convert.ToInt32((HttpContext.Current.Session["ComboFundSource"].ToString()));
            }




            var subAssemblyCodeId = -1;


            //TODO: Possibly added Element stuff here
            int elementId = -1;

            var completedById = -1;

            #region Get Route To

            var jobRouteTo = -1;
            if ((HttpContext.Current.Session["ComboRouteTo"] != null))
            {
                //Get Info From Session
                jobRouteTo = Convert.ToInt32((HttpContext.Current.Session["ComboRouteTo"].ToString()));
            }

            #endregion

            #region Get Notes

            var notes = "";
            if (HttpContext.Current.Session["txtAddDetail"] != null)
            {
                //Get Additional Info From Session
                notes = (HttpContext.Current.Session["txtAddDetail"].ToString());
            }

            #endregion

            #region Get Post Notes

            var postNotes = "";
            if (HttpContext.Current.Session["txtPostNotes"] != null)
            {
                //Get Post Notes From Session
                postNotes = (HttpContext.Current.Session["txtPostNotes"].ToString());
            }

            #endregion

            #region Get Laborclass

            var jobLaborClass = -1;
            if ((HttpContext.Current.Session["ComboLaborClass"] != null))
            {
                //Get Info From Session
                jobLaborClass = Convert.ToInt32((HttpContext.Current.Session["ComboLaborClass"].ToString()));
            }

            #endregion

            #region Get Group

            var jobGroup = -1;
            if ((HttpContext.Current.Session["ComboGroup"] != null))
            {
                //Get Info From Session
                jobGroup = Convert.ToInt32((HttpContext.Current.Session["ComboGroup"].ToString()));
            }

            #endregion

            #region Get Outcome

            var jobOutcome = -1;
            if ((HttpContext.Current.Session["ComboOutcome"] != null))
            {
                //Get Info From Session
                jobOutcome = Convert.ToInt32((HttpContext.Current.Session["ComboOutcome"].ToString()));
            }

            #endregion

            #region Get Remaining Downtime

            decimal jobRemainingDt = 0;
            if ((HttpContext.Current.Session["txtRemainingDownTime"] != null))
            {
                //Get Info From Session
                jobRemainingDt = Convert.ToDecimal((HttpContext.Current.Session["txtRemainingDownTime"].ToString()));
            }

            #endregion

            #region Get Estimated Length

            decimal jobRemainingLen = 0;
            if ((HttpContext.Current.Session["txtRemainingJobLength"] != null))
            {
                //Get Info From Session
                jobRemainingLen = Convert.ToDecimal((HttpContext.Current.Session["txtRemainingJobLength"].ToString()));
            }

            #endregion

            #region Get Actual Downtime

            decimal jobActualDt = 0;
            if ((HttpContext.Current.Session["txtActualDownTime"] != null))
            {
                //Get Info From Session
                jobActualDt = Convert.ToDecimal((HttpContext.Current.Session["txtActualDownTime"].ToString()));
            }

            #endregion

            #region Get Actual Length

            decimal jobActualLen = 0;
            if ((HttpContext.Current.Session["txtActualJobLen"] != null))
            {
                //Get Info From Session
                jobActualLen = Convert.ToDecimal((HttpContext.Current.Session["txtActualJobLen"].ToString()));
            }

            #endregion

            #region Get Shift

            var jobShift = -1;
            if ((HttpContext.Current.Session["ComboShift"] != null))
            {
                //Get Info From Session
                jobShift = Convert.ToInt32((HttpContext.Current.Session["ComboShift"].ToString()));
            }

            #endregion

            #region Get Supervisor

            var jobSupervisor = -1;
            if ((HttpContext.Current.Session["ComboSupervisor"] != null))
            {
                //Get Info From Session
                jobSupervisor = Convert.ToInt32((HttpContext.Current.Session["ComboSupervisor"].ToString()));
            }

            #endregion

            #region Get Estimated Length

            decimal jobEstimatedLen = 0;
            if ((HttpContext.Current.Session["txtEstimatedJobLen"] != null))
            {
                //Get Info From Session
                jobEstimatedLen = Convert.ToDecimal((HttpContext.Current.Session["txtEstimatedJobLen"].ToString()));
            }

            #endregion

            decimal jobEstimatedDt = 0;
            if ((HttpContext.Current.Session["txtEstimatedDownTime"] != null))
            {
                //Get Info From Session
                jobEstimatedDt = Convert.ToDecimal((HttpContext.Current.Session["txtEstimatedDownTime"].ToString()));
            }

            var jobReturnWithin = 0;
            if ((HttpContext.Current.Session["txtReturnWithin"] != null))
            {
                //Get Info From Session
                jobReturnWithin = Convert.ToInt32((HttpContext.Current.Session["txtReturnWithin"].ToString()));
            }

            //                    int jobId,
            //                    string jobTitle,
            //                    int followsStep,
            //                    int concurWithStep,
            //                    int statusID,
            //                    int laborClassID,
            //                    string completionNotes,
            //                    string notes,
            //                    int servicingEquipment,
            //                    int groupID,
            //                    int outcomeCode,
            //                    int shiftID,
            //                    int supervisorID,
            //                    decimal actualDowntime,
            //                    decimal actualLength,
            //                    decimal estDowntime,
            //                    decimal estLength,
            //                    decimal remaingDowntime,
            //                    decimal remainingLength,
            //                    DateTime startingDate,
            //                    DateTime dateCompleted,
            //                    int returnWithinDays,
            //                    int fundSourceID,
            //                    int subAssemblyCodeID,
            //                    int elementID,
            //                    int priorityID,
            //                    int reasonCodeID,
            //                    int createdBy)

            var results = _oJobStep.Add(jobId,
                                               workDesc,
                                               0, 0,
                                               status,
                                               jobLaborClass,
                                               postNotes,
                                               notes,
                                               -1,
                                               jobGroup,
                                               jobOutcome,
                                               jobShift,
                                               jobSupervisor,
                                               jobActualDt,
                                               jobActualLen,
                                               jobEstimatedDt,
                                               jobEstimatedLen,
                                               jobRemainingDt,
                                               jobRemainingLen,
                                               startDate, compDate,
                                               jobReturnWithin,
                                               fundSource,
                                               subAssemblyCodeId,
                                               //elementId,
                                               priority,
                                               reasonCodeId,
                                               _oLogon.UserID);


        }

        protected void UpdateRoutine()
        {
            var jobAgainstArea = 0;
            const bool requestOnly = true;
            const int actionPriority = -1;
            const int mobileEquip = -1;
            const int subAssemblyID = -1;
            const bool additionalDamage = false;
            const decimal percentOverage = 0;

            #region Get Logon Info

            //Get Value
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
            }

            if (HttpContext.Current.Session[""] != null)
            {

            }

            #endregion
            #region Job Step info
            #region Get Object

            var objectAgainstId = -1;
            if (HttpContext.Current.Session["ObjectIDCombo"] != null)
            {
                //Get Info From Session
                objectAgainstId = Convert.ToInt32((HttpContext.Current.Session["ObjectIDCombo"].ToString()));
            }

            #endregion

            #region Get GPS X

            decimal gpsX = 0;
            if (HttpContext.Current.Session["GPSX"] != null)
            {
                //Get Info From Session
                gpsX = Convert.ToDecimal((HttpContext.Current.Session["GPSX"].ToString()));
            }

            #endregion

            #region Get GPS Y

            decimal gpsY = 0;
            if (HttpContext.Current.Session["GPSY"] != null)
            {
                //Get Info From Session
                gpsY = Convert.ToDecimal((HttpContext.Current.Session["GPSY"].ToString()));
            }

            #endregion

            #region Get GPS Z

            decimal gpsZ = 0;
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Get Info From Session
                gpsZ = Convert.ToDecimal((HttpContext.Current.Session["GPSZ"].ToString()));
            }

            #endregion

            #region Get Description

            var workDesc = "";
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Get Additional Info From Session
                workDesc = (HttpContext.Current.Session["txtWorkDescription"].ToString());
            }

            #endregion

            #region Get Work Date

            var requestDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkRequestDate"] != null)
            {
                //Get Info From Session
                requestDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkRequestDate"].ToString());
            }

            #endregion

            #region Get Start Date

            var startDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkStartDate"] != null)
            {
                //Get Info From Session
                startDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkStartDate"].ToString());
            }

            #endregion

            #region Get Comp Date

            var compDate = DateTime.Now;
            if (HttpContext.Current.Session["TxtWorkCompDate"] != null)
            {
                //Get Info From Session
                compDate = Convert.ToDateTime(HttpContext.Current.Session["TxtWorkCompDate"].ToString());
            }

            #endregion

            #region Get Priority

            var requestPriority = -1;
            if ((HttpContext.Current.Session["ComboPriority"] != null))
            {
                //Get Info From Session
                requestPriority = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
            }

            #endregion

            #region Get State Route

            var stateRouteId = -1;
            if ((HttpContext.Current.Session["comboHwyRoute"] != null))
            {
                //Get Info From Session
                stateRouteId = Convert.ToInt32((HttpContext.Current.Session["comboHwyRoute"].ToString()));
            }

            #endregion

            #region Get Milepost

            decimal milepost = 0;
            if (HttpContext.Current.Session["txtMilepost"] != null)
            {
                //Get Info From Session
                milepost = Convert.ToDecimal(HttpContext.Current.Session["txtMilepost"].ToString());
            }

            #endregion

            #region Get Milepost To

            decimal milepostTo = 0;
            if (HttpContext.Current.Session["txtMilepostTo"] != null)
            {
                //Get Info From Session
                milepostTo = Convert.ToDecimal(HttpContext.Current.Session["txtMilepostTo"].ToString());
            }

            #endregion

            #region Get Milepost Direction

            var mpIncreasing = -1;
            if ((HttpContext.Current.Session["comboMilePostDir"] != null))
            {
                //Get Info From Session
                mpIncreasing = Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
            }

            #endregion

            #region Get Work Op

            var workOp = -1;
            if ((HttpContext.Current.Session["ComboWorkOp"] != null))
            {
                //Get Info From Session
                workOp = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
            }

            #endregion

            #region Get Requestor

            var requestor = -1;
            if ((HttpContext.Current.Session["ComboRequestor"] != null))
            {
                //Get Info From Session
                requestor = Convert.ToInt32((HttpContext.Current.Session["ComboRequestor"].ToString()));
            }
            else if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //Set Requestor
                requestor = _oLogon.UserID;
            }

            #endregion

            #region Get Job Reason

            var reasonCode = -1;
            if ((HttpContext.Current.Session["comboReason"] != null))
            {
                //Get Info From Session
                reasonCode = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
            }

            #endregion

            #region Get Route To

            var routeTo = -1;
            if ((HttpContext.Current.Session["comboRouteTo"] != null))
            {
                //Get Info From Session
                routeTo = Convert.ToInt32((HttpContext.Current.Session["comboRouteTo"].ToString()));
            }

            #endregion

            #region Get Cost Code

            var costCodeId = -1;
            if ((HttpContext.Current.Session["ComboCostCode"] != null))
            {
                //Get Info From Session
                costCodeId = Convert.ToInt32((HttpContext.Current.Session["ComboCostCode"].ToString()));
            }

            #endregion

            #region Get Fund Source

            var fundSource = -1;
            if ((HttpContext.Current.Session["ComboFundSource"] != null))
            {
                //Get Info From Session
                fundSource = Convert.ToInt32((HttpContext.Current.Session["ComboFundSource"].ToString()));
            }

            #endregion

            #region Get Work Order

            var workOrder = -1;
            if ((HttpContext.Current.Session["ComboWorkOrder"] != null))
            {
                //Get Info From Session
                workOrder = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOrder"].ToString()));
            }

            #endregion

            #region Get Org Code

            var orgCode = -1;
            if ((HttpContext.Current.Session["ComboOrgCode"] != null))
            {
                //Get Info From Session
                orgCode = Convert.ToInt32((HttpContext.Current.Session["ComboOrgCode"].ToString()));
            }

            #endregion

            #region Get Fund Group

            var fundingGroup = -1;
            if ((HttpContext.Current.Session["ComboFundGroup"] != null))
            {
                //Get Info From Session
                fundingGroup = Convert.ToInt32((HttpContext.Current.Session["ComboFundGroup"].ToString()));
            }

            #endregion

            #region Get Equip Num

            var equipNumber = -1;
            if ((HttpContext.Current.Session["ComboEquipNum"] != null))
            {
                //Get Info From Session
                equipNumber = Convert.ToInt32((HttpContext.Current.Session["ComboEquipNum"].ToString()));
            }

            #endregion

            #region Get Ctl Section

            var controlSection = -1;
            if ((HttpContext.Current.Session["ComboCtlSection"] != null))
            {
                //Get Info From Session
                controlSection = Convert.ToInt32((HttpContext.Current.Session["ComboCtlSection"].ToString()));
            }

            #endregion

            #region Get Notes

            var notes = "";
            if (HttpContext.Current.Session["txtAddDetail"] != null)
            {
                //Get Additional Info From Session
                notes = (HttpContext.Current.Session["txtAddDetail"].ToString());
            }

            #endregion

            #region Get Post Notes

            var postNotes = "";
            if (HttpContext.Current.Session["txtPostNotes"] != null)
            {
                //Get Post Notes From Session
                postNotes = (HttpContext.Current.Session["txtPostNotes"].ToString());
            }

            #endregion

            #region Get Run Units

            //Create Variables
            decimal unitOne = 0;
            decimal unitTwo = 0;
            decimal unitThree = 0;

            //Check For First Unit
            if (HttpContext.Current.Session["txtRunUnitOne"] != null)
            {
                //Get From Session
                unitOne = Convert.ToDecimal((HttpContext.Current.Session["txtRunUnitOne"].ToString()));
            }

            //Check For Second Unit
            if (HttpContext.Current.Session["txtRunUnitTwo"] != null)
            {
                //Get From Session
                unitTwo = Convert.ToDecimal((HttpContext.Current.Session["txtRunUnitTwo"].ToString()));
            }

            //Check For Third Unit
            if (HttpContext.Current.Session["txtRunUnitThree"] != null)
            {
                //Get From Session
                unitThree = Convert.ToDecimal((HttpContext.Current.Session["txtRunUnitThree"].ToString()));
            }

            #endregion

            #region Get Job ID

            var jobID = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());
            var jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

            #endregion

            #region Get Job Step ID

            var jobStepId = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());

            #endregion

            #region Get Step #
            var jobStepNumber = 1;
            if (HttpContext.Current.Session["stepnumber"] != null)
            {
                jobStepNumber = Convert.ToInt32(HttpContext.Current.Session["stepnumber"].ToString());

            }


            #endregion

            #region Get Concur Step Number


            var jobStepConcurNumber = 1;
            if (HttpContext.Current.Session["concurwithstep"] != null)
            {

                jobStepConcurNumber = Convert.ToInt32(HttpContext.Current.Session["concurwithstep"].ToString());
            }

            #endregion

            #region Get Follow Step Number
            //Add Follow Step Number

            var jobStepFollowStepNumber = 1;
            if (HttpContext.Current.Session["followstep"] != null)
            {

                jobStepFollowStepNumber = Convert.ToInt32(HttpContext.Current.Session["followstep"].ToString());
            }


            #endregion

            #region Get Status

            var jobStatus = -1;
            if ((HttpContext.Current.Session["ComboStatus"] != null))
            {
                //Get Info From Session
                jobStatus = Convert.ToInt32((HttpContext.Current.Session["ComboStatus"].ToString()));
            }

            #endregion

            #region Get Laborclass

            var jobLaborClass = -1;
            if ((HttpContext.Current.Session["ComboLaborClass"] != null))
            {
                //Get Info From Session
                jobLaborClass = Convert.ToInt32((HttpContext.Current.Session["ComboLaborClass"].ToString()));
            }

            #endregion

            #region Get Group

            var jobGroup = -1;
            if ((HttpContext.Current.Session["ComboGroup"] != null))
            {
                //Get Info From Session
                jobGroup = Convert.ToInt32((HttpContext.Current.Session["ComboGroup"].ToString()));
            }

            #endregion

            #region Get Outcome

            var jobOutcome = -1;
            if ((HttpContext.Current.Session["ComboOutcome"] != null))
            {
                var x = Convert.ToInt32(ComboOutcome.Value.ToString());
                var y = ComboOutcome.Text.ToString();

                jobOutcome = x;
                //Get Info From Session
            }

            #endregion

            #region Get Shift

            var jobShift = -1;
            if ((HttpContext.Current.Session["ComboShift"] != null))
            {
                //Get Info From Session
                jobShift = Convert.ToInt32((HttpContext.Current.Session["ComboShift"].ToString()));
            }

            #endregion

            #region Get Supervisor

            var jobSupervisor = -1;
            if ((HttpContext.Current.Session["ComboSupervisor"] != null))
            {
                //Get Info From Session
                jobSupervisor = Convert.ToInt32((HttpContext.Current.Session["ComboSupervisor"].ToString()));
            }

            #endregion

            #region Get Actual Downtime

            decimal jobActualDt = 0;
            if ((HttpContext.Current.Session["txtActualDownTime"] != null))
            {
                //Get Info From Session
                jobActualDt = Convert.ToDecimal((HttpContext.Current.Session["txtActualDownTime"].ToString()));
            }

            #endregion

            #region Get Actual Length

            decimal jobActualLen = 0;
            if ((HttpContext.Current.Session["txtActualJobLen"] != null))
            {
                //Get Info From Session
                jobActualLen = Convert.ToDecimal((HttpContext.Current.Session["txtActualJobLen"].ToString()));
            }

            #endregion

            #region Get Estimated Downtime

            decimal jobEstimatedDt = 0;
            if ((HttpContext.Current.Session["txtEstimatedDownTime"] != null))
            {
                //Get Info From Session
                jobEstimatedDt = Convert.ToDecimal((HttpContext.Current.Session["txtEstimatedDownTime"].ToString()));
            }

            #endregion

            #region Get Estimated Length

            decimal jobEstimatedLen = 0;
            if ((HttpContext.Current.Session["txtEstimatedJobLen"] != null))
            {
                //Get Info From Session
                jobEstimatedLen = Convert.ToDecimal((HttpContext.Current.Session["txtEstimatedJobLen"].ToString()));
            }

            #endregion

            #region Get Remaining Downtime

            decimal jobRemainingDt = 0;
            if ((HttpContext.Current.Session["txtRemainingDownTime"] != null))
            {
                //Get Info From Session
                jobRemainingDt = Convert.ToDecimal((HttpContext.Current.Session["txtRemainingDownTime"].ToString()));
            }

            #endregion

            #region Get Estimated Length

            decimal jobRemainingLen = 0;
            if ((HttpContext.Current.Session["txtRemainingJobLength"] != null))
            {
                //Get Info From Session
                jobRemainingLen = Convert.ToDecimal((HttpContext.Current.Session["txtRemainingJobLength"].ToString()));
            }

            #endregion

            #region Get Estimated Units

            decimal jobEstimatedUnits = 0;
            if ((HttpContext.Current.Session["txtEstimagedUnits"] != null))
            {
                //Get Info From Session
                jobEstimatedUnits = Convert.ToDecimal((HttpContext.Current.Session["txtEstimagedUnits"].ToString()));
            }

            #endregion

            #region Get Actual Units

            decimal jobActualUnits = 0;
            if ((HttpContext.Current.Session["txtActualUnits"] != null))
            {
                //Get Info From Session
                jobActualUnits = Convert.ToDecimal((HttpContext.Current.Session["txtActualUnits"].ToString()));
            }

            #endregion

            #region Get Return Within

            var jobReturnWithin = 0;
            if (HttpContext.Current.Session["txtReturnWithin"] != null)
            {
                jobReturnWithin = Convert.ToInt32((HttpContext.Current.Session["txtReturnWithin"].ToString()));
                //Get Info From Session
            }

            #endregion

            #region Completed By
            var completedBy = "";
            if (HttpContext.Current.Session["ComboCompletedBy"] != null)
            {
                completedBy = HttpContext.Current.Session["ComboCompletedBy"].ToString();
            }
            #endregion

            #region Get Route To

            var jobRouteTo = -1;
            if ((HttpContext.Current.Session["ComboRouteTo"] != null))
            {
                //Get Info From Session
                jobRouteTo = Convert.ToInt32((HttpContext.Current.Session["ComboRouteTo"].ToString()));
            }

            #endregion

            #region Get Completed By

            var jobCompletedBy = -1;
            if ((HttpContext.Current.Session["ComboCompletedBy"] != null))
            {
                //Get Info From Session
                jobCompletedBy = Convert.ToInt32((HttpContext.Current.Session["ComboCompletedBy"].ToString()));
            }

            #endregion

            #region Get Charge To

            var jobChargeTo = "";
            if ((HttpContext.Current.Session["txtChargeTo"] != null))
            {
                //Get Info From Session
                jobChargeTo = (HttpContext.Current.Session["txtChargeTo"].ToString());
            }

            #endregion

            #region Get Incident Log

            var jobIncidentLog = -1;
            if ((HttpContext.Current.Session["ComboIncidentLog"] != null))
            {
                //Get Info From Session
                jobIncidentLog = Convert.ToInt32((HttpContext.Current.Session["ComboIncidentLog"].ToString()));
            }

            #endregion
            #endregion


            //Clear Errors
            _oJob.ClearErrors();

            try
            {

                {


                    //Update Job Step
                    var stepResults = _oJobStep.Update(jobStepId,
                        jobStepNumber,
                        jobStepConcurNumber,
                        jobStepFollowStepNumber,
                        workDesc,
                        jobStatus,
                        jobLaborClass,
                        postNotes,
                        notes,
                        -1,
                        jobGroup,
                        jobOutcome,
                        jobShift,
                        jobSupervisor,
                        jobActualDt,
                        jobActualLen,
                        jobEstimatedDt,
                        jobEstimatedLen,
                        jobRemainingDt,
                        jobRemainingLen,
                        startDate,
                        compDate,
                        jobReturnWithin,
                        fundSource,
                        -1,
                        requestPriority,
                        reasonCode,
                        _oLogon.UserID,
                        EditingTimeBachId,
                        EditingTiemBatchItemId);


                    {
                        //Throw Error
                        //throw new SystemException(
                        //    @"Error Updating Job Step -" + _oJobStep.LastError);
                    }
                    var objectResults = _oJobStep.ChangeJobAgainst(jobID, objectAgainstId, JobAgainstType.MaintenanceObjects, _oLogon.UserID);


                    var costResults = _oJob.UpdateJobCosting(
                            jobID,
                            costCodeId,
                            fundSource,
                            workOrder,
                            workOp,
                            orgCode,
                            fundingGroup,
                            equipNumber,
                            controlSection,
                            _oLogon.UserID);

                    var jobStepCostResults = _oJobStep.UpdateJobstepCosting(jobID, jobStepId,
                             costCodeId,
                             fundSource,
                             workOrder,
                             workOp,
                             orgCode,
                             fundingGroup,
                             equipNumber,
                             controlSection,
                             _oLogon.UserID);






                    //Save Route & Completion Information
                    if (!_oJobStep.UpdateRouteAndCompletionInfo(jobStepId, jobRouteTo, jobCompletedBy, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Route To And Completion Information -" + _oJobStep.LastError);
                    }

                    //Check Run Unit Table
                    if (_oRunUnit.Ds.Tables.Count > 0)
                    {
                        //Check Row Count
                        if (_oRunUnit.Ds.Tables[0].Rows.Count > 0)
                        {
                            //Loop Rows
                            for (var i = 0; i < _oRunUnit.Ds.Tables[0].Rows.Count; i++)
                            {
                                //Determine Unit
                                switch (i)
                                {
                                    case 0:
                                        {
                                            //Check If Not Read Only
                                            if (txtRunUnitOne.ReadOnly == false)
                                            {
                                                //Update Run Unit
                                                if (!_oRunUnit.Update((int)_oRunUnit.Ds.Tables[0].Rows[i][0],
                                                    Convert.ToDecimal(
                                                        txtRunUnitOne.Value.ToString()),
                                                    _oRunUnit.Ds.Tables[0].Rows[i][3].
                                                        ToString(),
                                                    (int)_oRunUnit.Ds.Tables[0].Rows[i][4],
                                                    _oLogon.UserID))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Updating Run Units -" + _oRunUnit.LastError);
                                                }
                                            }
                                            break;
                                        }
                                    case 1:
                                        {
                                            //Check If Not Read Only
                                            if (txtRunUnitTwo.ReadOnly == false)
                                            {
                                                //Update Run Unit
                                                if (!_oRunUnit.Update((int)_oRunUnit.Ds.Tables[0].Rows[i][0],
                                                    Convert.ToDecimal(
                                                        txtRunUnitTwo.Value.ToString()),
                                                    _oRunUnit.Ds.Tables[0].Rows[i][3].
                                                        ToString(),
                                                    (int)_oRunUnit.Ds.Tables[0].Rows[i][4],
                                                    _oLogon.UserID))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Updating Run Units -" + _oRunUnit.LastError);
                                                }
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            //Check If Not Read Only
                                            if (txtRunUnitThree.ReadOnly == false)
                                            {
                                                //Update Run Unit
                                                if (!_oRunUnit.Update((int)_oRunUnit.Ds.Tables[0].Rows[i][0],
                                                    Convert.ToDecimal(
                                                        txtRunUnitThree.Value.ToString()),
                                                    _oRunUnit.Ds.Tables[0].Rows[i][3].
                                                        ToString(),
                                                    (int)_oRunUnit.Ds.Tables[0].Rows[i][4],
                                                    _oLogon.UserID))
                                                {
                                                    //Throw Error
                                                    throw new SystemException(
                                                        @"Error Updating Run Units -" + _oRunUnit.LastError);
                                                }
                                            }
                                            break;
                                        }
                                }
                            }

                            //Update Completed Units
                            _oJob.ClearErrors();



                            //Create Temp Variables
                            decimal tmpUnit1 = 0;
                            decimal tmpUnit2 = 0;
                            decimal tmpUnit3 = 0;

                            //Check For Valid Input
                            if ((HttpContext.Current.Session["txtRunUnitOne"] != null) && (HttpContext.Current.Session["txtRunUnitOne"].ToString() != ""))
                            {
                                //Get Unit 1
                                tmpUnit1 = Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitOne"]);
                            }

                            //Check For Valid Input
                            if ((HttpContext.Current.Session["txtRunUnitTwo"] != null) && (HttpContext.Current.Session["txtRunUnitTwo"].ToString() != ""))
                            {
                                //Get Unit 2
                                tmpUnit2 = Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitTwo"]);
                            }

                            //Check For Valid Input
                            if ((HttpContext.Current.Session["txtRunUnitThree"] != null) && (HttpContext.Current.Session["txtRunUnitThree"].ToString() != ""))
                            {
                                //Get Unit 3
                                tmpUnit3 = Convert.ToDecimal(HttpContext.Current.Session["txtRunUnitThree"]);
                            }

                            //Update Completed Units
                            if (!_oJob.UpdateCompletedUnits(jobId, tmpUnit1, tmpUnit2, tmpUnit3))
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Updating Completed Units -" + _oJob.LastError);
                            }
                        }
                    }

                    //Update Charge To
                    if (!_oJobStep.UpdateChargeTo(jobId, jobStepId, jobChargeTo, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Charge To -" + _oJobStep.LastError);
                    }

                    //Update Incident Log Link
                    if (!_oJobStep.UpdateIncidentLogLink(jobId, jobStepId, jobIncidentLog, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Incident Log Link -" + _oJobStep.LastError);
                    }

                    //Update Production Units
                    if (!_oJob.UpdateProductionUnits(jobId, jobEstimatedUnits, jobActualUnits, _oLogon.UserID))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Production Units -" + _oJob.LastError);
                    }


                    //Check For Value
                    if (HttpContext.Current.Session["AssignedJobID"] != null)
                    {
                        //Get Additional Info From Session
                        lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                        //Check For Step
                        if (HttpContext.Current.Session["editingJobStepNum"] != null)
                        {
                            //Set Text
                            lblStep.Text = @"STEP #" +
                                           (HttpContext.Current.Session["editingJobStepID"]);
                        }

                        //Setup For Editing
                        SetupForEditing();

                        var jobStepKey = Convert.ToInt32(HttpContext.Current.Session["editingJobStepID"].ToString());
                        ;
                        Response.Redirect(
                                                    "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + jobStepId, true);
                    }

                    //Success Return True

                }

                ////Throw Error
                //throw new SystemException(
                //    @"Error Updating Job -" + _oJob.LastError);
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                //Return False To Prevent Navigation

            }
            GetStepTab();
        }

        private void GetStepTab()
        {
            if (StepTab.ActiveTabIndex.ToString() != null)

                switch (StepTab.ActiveTabIndex)
                {
                    case 0:
                        {
                            StepTab.ActiveTabIndex = 0;
                            //Steps
                            break;
                        }
                    case 1:
                        {
                            //Enable/Disable Member MultiSelect
                            StepTab.ActiveTabIndex = 1;
                            break;
                        }
                    case 2:
                        {
                            //Enable/Disable Crew MultiSelect
                            StepTab.ActiveTabIndex = 2;
                            break;
                        }
                    case 3:
                        {
                            //Enable/Disable Part MultiSelect
                            StepTab.ActiveTabIndex = 3;
                            break;
                        }
                    case 4:
                        {
                            //Enable/Disable Part MultiSelect
                            StepTab.ActiveTabIndex = 4;
                            break;
                        }
                    case 5:
                        {
                            //Enable/Disable Other MultiSelect
                            StepTab.ActiveTabIndex = 5;
                            break;
                        }
                    default:
                        {
                            //Do Nothing
                            break;
                        }
                }
            activeTab = StepTab.ActiveTabIndex;
            HttpContext.Current.Session.Add("activeTab", activeTab);
        }
        #endregion

        #region Post Job
        public void PostPlanJob(object sender, EventArgs e)
        {

            //Get Values
            var jobKey = Convert.ToInt32(HttpContext.Current.Session["n_Jobid"]);
            var jobStepKey = Convert.ToInt32(HttpContext.Current.Session["n_jobstepid"]);


            //Check Keys
            if ((jobKey > 0) && (jobStepKey > 0))
            {
                //Get Post Date
                if ((jobPostDate.Value != null) && (jobPostDate.Value.ToString() != "") && ComboOutcomeCode.Value != null)
                {
                    //Check Outcome Code

                    //Get Post Date
                    var postDate = Convert.ToDateTime(jobPostDate.Value.ToString());

                    //Get Outcome Code
                    var outcomeId = Convert.ToInt32(ComboOutcomeCode.Value.ToString());

                    //Clear Errors
                    _oJob.ClearErrors();

                    //Load Job Data
                    if (_oJob.LoadData(jobKey) && _oJob.Ds.Tables.Count > 0)
                    {
                        //Check Table Row Count
                        if (_oJob.Ds.Tables[0].Rows.Count == 1)
                        {
                            //Set History Flag
                            var isHist = (_oJob.Ds.Tables[0].Rows[0][30].ToString().ToUpper() == "Y");

                            //Check Flag
                            if (isHist == false)
                            {
                                //Update Job Step Information
                                if (_oJob.UpdateJobstepCompletionInfo(jobStepKey,
                                    outcomeId,
                                    postDate,
                                    Convert.ToInt32(chkPostDefaults.Checked),
                                    _oLogon.UserID))
                                {
                                    //Set Flag
                                    var hasPostingPblm = false;

                                    //Post To History
                                    if (!_oJob.PostWorkOrderToHistory(jobKey,
                                        true,
                                        _oLogon.UserID,
                                        ref hasPostingPblm))
                                    {
                                        //Check Flag
                                        if (hasPostingPblm)
                                        {
                                            //Throw Error
                                            throw new SystemException(
                                                @"Error Posting Job - " + _oJob.LastError);
                                        }
                                    }
                                }
                                else
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Updating Job For Batch Post");
                                }
                            }
                        }
                    
                    else
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Posting Job - " + _oJob.LastError);
                    }
                }
                else
                {
                    //Throw Error
                    throw new SystemException(
                        @"Valid Outcome Code Required For Batch Post");
                }
            }
            else
            {
                //Throw Error
                throw new SystemException(
                    @"Valid Completion Date Required For Batch Post");
            }
        }
            else
            {
                //Throw Error
                throw new SystemException(
                    @"Error Loading Job & Job Step Keys For Batch Post");
        }

    }
        #endregion

        #region Get GPS Methods
        protected decimal gpsX()
        {
            //Check Value
            if (GPSX.Value != null)
            {
                //Return Value
                return Convert.ToDecimal(GPSX.Value.ToString());
            }

            //Return Default
            return 0;
        }

        protected decimal gpsY()
        {
            //Check Value
            if (GPSY.Value != null)
            {
                //Return Value
                return Convert.ToDecimal(GPSY.Value.ToString());
            }

            //Return Default
            return 0;
        }

        protected decimal gpsZ()
        {
            //Check Value
            if (GPSZ.Value != null)
            {
                //Return Value
                return Convert.ToDecimal(GPSZ.Value.ToString());
            }

            //Return Default
            return 0;
        }
        #endregion

        /// <summary>
        /// Hides Default Edit Button
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void HideDefaultEditButtons(object sender, EventArgs e)
        {
            //Cast Sender As Hyperlink
            ASPxGridViewTemplateReplacement link = (ASPxGridViewTemplateReplacement)sender;

            //Set Visibility
            link.Visible = false;
        }

       

    }
}