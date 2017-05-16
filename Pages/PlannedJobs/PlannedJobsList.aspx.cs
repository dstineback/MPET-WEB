using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Blob;
using MPETDSFactory;
using Page = System.Web.UI.Page;

namespace Pages.PlannedJobs
{
    public partial class PlannedJobsList : Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check For Logon Class
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
                this.Session["UserID"] = _oLogon.UserID;

                //Load Form Permissions
                if (FormSetup(_oLogon.UserID))
                {
                    //Setup Buttons
                    Master.ShowSaveButton = false;
                    Master.ShowPostButton = _userCanEdit;
                    Master.ShowNewButton = _userCanAdd;
                    Master.ShowEditButton = _userCanEdit;
                    Master.ShowDeleteButton = _userCanDelete;
                    Master.ShowViewButton = _userCanView;
                    Master.ShowCopyJobButton = _userCanAdd;
                    Master.ShowIssueButton = _userCanEdit;
                    Master.ShowRoutineJobButton = (_userCanEdit && _userCanAdd);
                    Master.ShowForcePmButton = (_userCanEdit && _userCanAdd);
                    Master.ShowMultiSelectButton = _userCanDelete;
                    Master.ShowPrintButton = _userCanView;
                    Master.ShowMapDisplayButton = _userCanEdit;
                    Master.ShowPdfButton = false;
                    Master.ShowXlsButton = false;
                    
                }
            }

            //Check For Post Back
            if (!IsPostBack)
            {
                //Bind Grid
                PlannedGrid.DataBind();
            }
            else
            {
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
                                //Call Map Display
                                MapItem();
                                //break
                                break;
                        }
                        case "ExportXLS":
                        {
                            //Call Export XLS Option
                            ExportXls();
                            break;
                        }
                        case "ExportPDF":
                        {
                            //Call Export PDF Option
                            ExportPdf();
                            break;
                        }
                        case "MultiSelect":
                        {
                            //Enable/Disable MultiSelect
                            EnableMultiSelect(!(PlannedGrid.Columns[0].Visible));
                            break;
                        }
                        case"CopyJob":
                        {
                            //Call Copy Routine
                            CopyJobRoutine();

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
                        
                        case"BatchCrewAdd":
                        {
                            //Bind Grid
                            CrewLookupGrid.DataBind();

                            //Show Popup
                            BatchCrewPopup.ShowOnPageLoad = true;

                            //break
                            break;
                        }
                        case "BatchSupervisorAdd":
                        {
                            //Bind Grid
                            SupervisorGrid.DataBind();

                            //Show Popup
                            BatchSuperPopup.ShowOnPageLoad = true;
                             
                            //break
                            break;
                        }
                        case "BatchEquipmentAdd":
                        {
                            //Bind Grid
                            EquipmentGrid.DataBind();

                            //Show Popup
                            BatchEquipPopup.ShowOnPageLoad = true;

                            //break 
                            break;
                        }
                        case "BatchPartAdd":
                        {
                            //Bind Grid
                            PartGrid.DataBind();

                            //Show Popup
                            BatchPartPopup.ShowOnPageLoad = true;

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
                        case "PostJob":
                        {
                            //Clear Fields
                            txtPostDate.Value = null;
                            ComboOutcomeCode.Value = null;
                            chkPostDefaults.Checked = false;

                            //Show Popup
                            BatchPostPopup.ShowOnPageLoad = true;

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
                    BatchCrewPopup.ShowOnPageLoad = false;
                    BatchSuperPopup.ShowOnPageLoad = false;
                    BatchPostPopup.ShowOnPageLoad = false;
                    RoutineJobPopup.ShowOnPageLoad = false;
                    ForcePMPopup.ShowOnPageLoad = false;
                    BatchEquipPopup.ShowOnPageLoad = false;
                    BatchPartPopup.ShowOnPageLoad = false;
                }
            }

            //Enable/Disable Buttons
            Master.ShowNewButton = !(PlannedGrid.Columns[0].Visible);
            Master.ShowEditButton = !(PlannedGrid.Columns[0].Visible);
            Master.ShowViewButton = !(PlannedGrid.Columns[0].Visible);
            Master.ShowPrintButton = !(PlannedGrid.Columns[0].Visible);
            Master.ShowCopyJobButton = !(PlannedGrid.Columns[0].Visible);
            Master.ShowBatchCrewAddButton = ((PlannedGrid.Columns[0].Visible) && _userCanEdit);
            Master.ShowBatchSupervisorAddButton = ((PlannedGrid.Columns[0].Visible) && _userCanEdit);
            Master.ShowBatchEquipmentButton = ((PlannedGrid.Columns[0].Visible) && _userCanEdit);
            Master.ShowBatchPartButton = ((PlannedGrid.Columns[0].Visible) && _userCanEdit);
            Master.ShowPostButton = (_userCanEdit);
            Master.ShowMapDisplayButton = false;
            Master.ShowForcePmButton = !((PlannedGrid.Columns[0].Visible) && _userCanEdit);
            Master.ShowRoutineJobButton = !((PlannedGrid.Columns[0].Visible) && _userCanEdit);

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(PlannedGrid.Columns[0].Visible))
            {
                //Uncheck All
                PlannedGrid.Selection.UnselectAll();
            }
        }

        protected void EnableMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            PlannedGrid.Columns[0].Visible = showMultiSelect;
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

        //protected void PlannedGrid_DataBinding(object sender, EventArgs e)
        //{
        //    // Assign the data source in grid_DataBinding
        //    PlannedGrid.DataSource = GetData(PlannedGrid.FilterExpression);
        //}

        protected void Page_Init(object sender, EventArgs e)
        {
            //Set Connection Info
            _connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
            _useWeb = (ConfigurationManager.AppSettings["UsingWebService"] == "Y");

            //Initialize Classes
            _oJob = new WorkOrder(_connectionString, _useWeb);
            _oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

            //Set Data Sources
            OutcomeCodeSqlDatasource.ConnectionString = _connectionString;

        }

        protected void PlannedGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            //Redirect To Edit Page With Job ID
            ASPxWebControl.RedirectOnCallback("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + e.EditingKeyValue);
        }

        protected string GetUrl(GridViewDataItemTemplateContainer container)
        {
            var values = (int)container.Grid.GetRowValues(container.VisibleIndex, new[] { "n_jobstepid" });
            return "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + values;
        }

        /// <summary>
        /// Export Grid To XLS
        /// </summary>
        private void ExportXls()
        {
            //Call Export Routine
            gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        /// <summary>
        /// Export Grid To PDF
        /// </summary>
        private void ExportPdf()
        {
            //Call Export Routine
            gridExport.WritePdfToResponse();
        }

        /// <summary>
        /// Views Selected Row
        /// </summary>
        private void ViewSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("n_jobstepid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + Selection.Get("n_jobstepid"), true);
            }
        }

        /// <summary>
        /// Edits Selected Job Step
        /// </summary>
        private void EditSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("n_jobstepid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + Selection.Get("n_jobstepid"), true);
            }
        }

        /// <summary>
        /// Deletes Selected Job Step
        /// </summary>
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
                if ((PlannedGrid.Columns[0].Visible))
                {
                    //Get Selections
                    var recordIdSelection = PlannedGrid.GetSelectedFieldValues("n_jobstepid");

                    //Process Multi Selection
                    foreach (var selected in recordIdSelection)
                    {
                        //Get ID
                        recordToDelete = Convert.ToInt32(selected.ToString());

                        //Set Continue Bool
                        continueDeletion = (recordToDelete > 0);

                        //Check Continue Bool
                        if (continueDeletion)
                        {
                            //Clear Errors
                            _oJobStep.ClearErrors();

                            //Delete Jobstep
                            if (_oJobStep.Delete(recordToDelete, _oLogon.UserID))
                            {
                                //Set Deletion Done
                                deletionDone = true;
                            }
                        }

                        //Check Deletion Done
                        if (deletionDone)
                        {
                            //Perform Refresh
                            PlannedGrid.DataBind();
                        }
                    }
                }
                else
                {
                    //Check For Job ID
                    if (Selection.Contains("n_jobstepid"))
                    {
                        //Get ID
                        recordToDelete = Convert.ToInt32(Selection.Get("n_jobstepid"));
                    }

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oJobStep.ClearErrors();

                        //Delete Jobstep
                        if (_oJobStep.Delete(recordToDelete, _oLogon.UserID))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        PlannedGrid.DataBind();
                    }
                }
            }
        }

        private void PrintSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("n_jobstepid"))
            {
                //Check For Previous Session Report Parm ID
                if (HttpContext.Current.Session["ReportParm"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportParm");
                }

                //Add Session Report Parm ID
                HttpContext.Current.Session.Add("ReportParm", Selection.Get("n_jobstepid"));

                //Check For Previous Report Name
                if (HttpContext.Current.Session["ReportToDisplay"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportToDisplay");
                }

                //Add Report To Display
                HttpContext.Current.Session.Add("ReportToDisplay","mltstpwo.rpt");

                

                //Redirect To Report Page
                Response.Redirect("~/Reports/ReportViewer.aspx", true);

                
            }
        }

        private void AddNewRow()
        {
            ResetSession();
            //Redirect To Edit Page With Job ID
            Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx", true);
        }

        protected void ASPxGridView1_HtmlRowPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //Look For Job ID Column
            if (e.DataColumn.FieldName == "Jobid")
            {
                //Set Background Color
                e.Cell.Font.Bold = true;
            }
        }

        /// <summary>
        /// Unloads & Registers Update Panel
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            //Register Panel
            RegisterUpdatePanel((UpdatePanel)sender);
        }

        /// <summary>
        /// Registers Panel
        /// </summary>
        /// <param name="panel">Panel To Register</param>
        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }

        /// <summary>
        /// Copies Selected Job
        /// </summary>
        protected void CopyJobRoutine()
        {
            //Copy Selected Job
            try
            {
                //Make Sure Multi Select Is Disabled
                if (!(PlannedGrid.Columns[0].Visible))
                {
                    //Check For Job ID
                    if (Selection.Contains("n_Jobid"))
                    {
                        //Check For Job Step ID
                        if (Selection.Contains("n_jobstepid"))
                        {
                            //Check For Step Number
                            if (Selection.Contains("step"))
                            {
                                //Get Values
                                var jobKey = Convert.ToInt32(Selection.Get("n_Jobid"));
                                var jobStepNum = Convert.ToInt32(Selection.Get("step"));
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

                                                //Forward User To Copied Work Order
                                                Response.Redirect("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + newJobStep, true);
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
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Maps selected items on a Map display
        /// </summary>
        protected void MapItem()
        {
            var sel = Selection.Count;
            var MapSelected = PlannedGrid.GetSelectedFieldValues("Jobid","n_jobid","n_jobstepid","Step Title","step","Object ID", "Latitude", "Longitude");

        if(sel > 0 || MapSelected.Count > 0) { 

                if (MapSelected.Count > 0)
                {
                    if (HttpContext.Current.Session["MapSelected"] != null)
                    {
                        HttpContext.Current.Session.Remove("MapSelected");
                    }
                    HttpContext.Current.Session.Add("MapSelected", MapSelected);
                }
                //Check For Row Value In Hidden Field (Set Via JS)
                if (Selection.Contains("n_jobstepid") )
                {
                    //Check For Previous Session Report Parm ID
                    if (HttpContext.Current.Session["jobstepid"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("jobstepid");
                        //Add Session Report Parm ID
                    }
                    HttpContext.Current.Session.Add("jobstepid", Selection.Get("n_jobstepid"));


                    //Check For Previous Session Report Parm ID
                    if (HttpContext.Current.Session["Latitude"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("Latitude");
                    }
                    //Add Session Report Parm ID
                    HttpContext.Current.Session.Add("Latitude", Selection.Get("Latitude"));


                    //Check For Previous Session Report Parm ID
                    if (HttpContext.Current.Session["Longitude"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("Longitude");
                    }
                    //Add Session Report Parm ID
                    HttpContext.Current.Session.Add("Longitude", Selection.Get("Longitude"));

                    if (HttpContext.Current.Session["description"] != null)
                    {
                        HttpContext.Current.Session.Remove("description");
                    }

                    HttpContext.Current.Session.Add("description", Selection.Get("Step Title"));

                    if(HttpContext.Current.Session["jobid"] != null)
                    {
                        HttpContext.Current.Session.Remove("jobid");
                    }
                    HttpContext.Current.Session.Add("jobid", Selection.Get("Jobid"));

                    if(HttpContext.Current.Session["n_jobid"] != null)
                    {
                        HttpContext.Current.Session.Remove("n_jobid");
                    }
                    HttpContext.Current.Session.Add("njobid", Selection.Get("n_jobid"));

                    if (HttpContext.Current.Session["step"] != null)
                    {
                        HttpContext.Current.Session.Remove("step");
                    }
                    HttpContext.Current.Session.Add("step", Selection.Get("step"));

                    if (HttpContext.Current.Session["objectid"] != null)
                    {
                        HttpContext.Current.Session.Remove("objectid");
                    }
                    HttpContext.Current.Session.Add("objectid", Selection.Get("Object ID"));

                }
                //Redirect To Report Page
                Response.Redirect("~/Pages/Map/MapForm.aspx", true);
            }
            else { HttpContext.Current.Response.Write("<script language='javascript'>alert('Error trying to Map Items, No rows were selected.');</script>"); };
        }

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
                    //Check For Multi Selection
                    if ((PlannedGrid.Columns[0].Visible))
                    {
                        //Get Selections
                        var recordIdSelection = PlannedGrid.GetSelectedFieldValues("n_jobid");

                        //Process Multi Selection
                        foreach (var selected in recordIdSelection)
                        {
                            //Get ID
                            var jobKey = Convert.ToInt32(selected.ToString());

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
                    else
                    {

                        //Check For Job ID
                        if (Selection.Contains("n_Jobid"))
                        {
                            //Get Record ID
                            var jobKey = Convert.ToInt32(Selection.Get("n_Jobid"));

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

        /// <summary>
        /// Adds Selected Crew For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddCrew_Click(object sender, EventArgs e)
        {
            //Process Batch Crew Add Selections
            try
            {
                //Check For Multi Selection
                if ((PlannedGrid.Columns[0].Visible))
                {
                    //Create Temp Variables
                    var crewLaborId = "";
                    var crewLaborDesc = "";
                    var crewLaborClass = 0;
                    const int defaultShift = -1;
                    decimal straightTimeRate = 0;
                    decimal overtimeRate = 0;
                    decimal otherRate = 0;

                    //Get Job Selections
                    var jobSelections =
                        PlannedGrid.GetSelectedFieldValues(new[] {"n_jobid", "n_jobstepid", "Completion Date", "I"});

                    //Process Multi Selection
                    foreach (object[] selected in jobSelections)
                    {
                        //Get Keys & Dates
                        var jobKey = Convert.ToInt32(selected[0].ToString());
                        var jobStepKey = Convert.ToInt32(selected[1].ToString());
                        var workDate = _nullDate;

                        //Check Date
                        if ((selected[2].ToString() != ""))
                        {
                            //Set Date
                            workDate = Convert.ToDateTime(selected[2].ToString());
                        }

                        //Get Crew Selection
                        var recordIdSelection = CrewLookupGrid.GetSelectedFieldValues("nUserID");

                        //Create Job Crew Class
                        using (
                            var oJobCrew =
                                new JobstepJobCrew(_connectionString, _useWeb))
                        {

                            //Loop Through Job Selections To Add Crew To
                            foreach (
                                var crewMemberId in
                                    recordIdSelection.Select(selectedCrew => Convert.ToInt32(selectedCrew.ToString())))
                            {
                                //Create User Class
                                using (
                                    var userInfo =
                                        new MpetUserDbClass(_connectionString, _useWeb))
                                {
                                    //Get LC Info
                                    if (
                                        !userInfo.GetUsersLaborSettingText(crewMemberId, ref crewLaborClass,
                                            ref crewLaborId, ref crewLaborDesc))
                                    {
                                        //Throw Error
                                        throw new SystemException(
                                            @"Error Getting Labor Class - " + userInfo.LastError);
                                    }
                                }

                                //Clear Errors
                                oJobCrew.ClearErrors();

                                //Get Adjusted Rate
                                if (!oJobCrew.GetAdjustedUserRate(crewMemberId,
                                    defaultShift,
                                    crewLaborClass,
                                    ref straightTimeRate,
                                    ref overtimeRate,
                                    ref otherRate))
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Determining Crew Rate - " + oJobCrew.LastError);
                                }

                                //Clear Errors
                                oJobCrew.ClearErrors();

                                //Add Job Crew Member
                                if (!oJobCrew.Add(jobKey,
                                    jobStepKey,
                                    crewMemberId,
                                    crewLaborClass,
                                    -1,
                                    defaultShift,
                                    0,
                                    straightTimeRate,
                                    0,
                                    0,
                                    0,
                                    workDate,
                                    _oLogon.UserID,
                                    -1,
                                    -1))
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Adding Job Crew Member - " + oJobCrew.LastError);
                                }
                            }
                        }
                    }

                    //Issue Jobs
                    //IssueRoutine();
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Adds Selected Equipment For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddEquipment_Click(object sender, EventArgs e)
        {
            //Process Batch Equipment Add Selections
            try
            {
                //Check For Multi Selection
                if ((PlannedGrid.Columns[0].Visible))
                {
                    //Get Job Selections
                    var jobSelections =
                        PlannedGrid.GetSelectedFieldValues(new[] { "n_jobid", "n_jobstepid", "Completion Date", "I" });

                    //Process Multi Selection
                    foreach (object[] selected in jobSelections)
                    {
                        //Get Keys & Dates
                        var jobKey = Convert.ToInt32(selected[0].ToString());
                        var jobStepKey = Convert.ToInt32(selected[1].ToString());
                        var workDate = _nullDate;

                        //Check Date
                        if ((selected[2].ToString() != ""))
                        {
                            //Set Date
                            workDate = Convert.ToDateTime(selected[2].ToString());
                        }

                        //Get Equipment Selection
                        var recordIdSelection = EquipmentGrid.GetSelectedFieldValues(new[] { "n_objectid", "description" });

                        //Create Job Equipment Class
                        using (
                            var oJobEquipment =
                                new JobEquipment(_connectionString, _useWeb))
                        {
                            //Process Multi Selection
                            foreach (object[] equipmentSelections in recordIdSelection)
                            {
                                //Get Record ID
                                var equipRecordId = Convert.ToInt32(equipmentSelections[0].ToString());

                                //Get Description
                                var equipDesc = equipmentSelections[1].ToString();

                                //Clear Errors
                                oJobEquipment.ClearErrors();

                                //Create New Table
                                var dt = new DataTable();

                                //Reset Cost
                                decimal equipmentCost = 0;

                                //Get Cost
                                if (oJobEquipment.GetJobEquipmentInfo(equipRecordId, ref dt))
                                {
                                    //Check Row Count
                                    if (dt.Rows.Count > 0)
                                    {
                                        //Set Cost
                                        equipmentCost = Convert.ToDecimal(dt.Rows[0][18]);
                                    }
                                }

                                //Add Equipment
                                if (!oJobEquipment.Add(jobKey,
                                    jobStepKey,
                                    equipRecordId,
                                    equipmentCost,
                                    equipDesc,
                                    "",
                                    0,
                                    0,
                                    workDate,
                                    -1,
                                    _oLogon.UserID,
                                    -1,
                                    -1))
                                {
                                    //Throw Error
                                    throw new SystemException(
                                        @"Error Adding Job Equipment  - " + oJobEquipment.LastError);
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
        /// Adds Selected Parts For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddPart_Click(object sender, EventArgs e)
        {
            //Process Batch Part Add Selections
            try
            {
                //Check For Multi Selection
                if ((PlannedGrid.Columns[0].Visible))
                {
                    //Get Job Selections
                    var jobSelections =
                        PlannedGrid.GetSelectedFieldValues(new[] { "n_jobid", "n_jobstepid", "Completion Date", "I" });

                    //Process Multi Selection
                    foreach (object[] selected in jobSelections)
                    {
                        //Get Keys & Dates
                        var jobKey = Convert.ToInt32(selected[0].ToString());
                        var jobStepKey = Convert.ToInt32(selected[1].ToString());

                        //Get Equipment Selection
                        var recordIdSelection = PartGrid.GetSelectedFieldValues(new[] { "n_masterpartid", "masterpartid", "Description", "listcost", "cPrefMfg", "n_PrefMFGPartID" });

                        //Create Job Part Class
                        using (
                            var oJobPart =
                                new JobstepJobParts(_connectionString, _useWeb))
                        {
                            //Set Job ID/Step
                            oJobPart.SetJobstepInfo(jobKey, jobStepKey);

                            //Process Multi Selection
                            if ((from object[] partSelections in recordIdSelection
                                let partId = Convert.ToInt32(partSelections[0].ToString())
                                let editingJpnsPartId = partSelections[1].ToString()
                                let editingJpnsPartDesc = partSelections[2].ToString()
                                let editingJpnsPartCost = Convert.ToDecimal(partSelections[3].ToString())
                                let cPreferedMfgPartId = partSelections[4].ToString()
                                let editingJpMfgPartId = Convert.ToInt32(partSelections[5].ToString())
                                where !oJobPart.Add(partId,
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
                                throw new SystemException(
                                    @"Error Adding Stock Master Part  - " +
                                    oJobPart.LastError);
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
        /// Adds Selected Supervisor For Job Step
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void btnAddSupervisor_Click(object sender, EventArgs e)
        {
            //Process Batch Supervisor Add Selections
            try
            {
                //Check For Multi Selection
                if ((PlannedGrid.Columns[0].Visible))
                {
                    //Get Job Selections
                    var jobSelections =
                        PlannedGrid.GetSelectedFieldValues(new[] { "n_jobid", "n_jobstepid" });

                    //Process Multi Selection
                    foreach (object[] selected in jobSelections)
                    {
                        //Get Keys & Dates
                        var jobKey = Convert.ToInt32(selected[0].ToString());
                        var jobStepKey = Convert.ToInt32(selected[1].ToString());

                        //Get Sueprvisor Selection
                        var recordIdSelection = SupervisorGrid.GetSelectedFieldValues("nUserID");

                        foreach (var supervisorId in recordIdSelection.Select(selectedSupervisor => Convert.ToInt32(selectedSupervisor.ToString())))
                        {
                            //Clear Errors
                            _oJobStep.ClearErrors();

                            //Add Job Supervisor
                            if (!_oJobStep.UpdateSupervisor(jobKey,
                                jobStepKey,
                                supervisorId,
                                _oLogon.UserID))
                            {
                                //Throw Error
                                throw new SystemException(
                                    @"Error Adding Job Supervisor  - " + _oJobStep.LastError);
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
        /// Loads Outcome Code Data By Filter
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ComboOutcomeCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;
            OutcomeCodeSqlDatasource.SelectCommand =
                @"SELECT  n_outcomecodeid ,
                            outcomecodeid ,
                            [description]
                    FROM    ( SELECT    tblOutcome.n_outcomecodeid ,
                                        tblOutcome.outcomecodeid ,
                                        tblOutcome.[description] ,
                                        ROW_NUMBER() OVER ( ORDER BY tblOutcome.n_outcomecodeid ) AS [rn]
                              FROM      dbo.OutcomeCodes AS tblOutcome
                              WHERE     ( ( outcomecodeid + ' ' + [description] ) LIKE @filter )
                                        AND tblOutcome.b_IsActive = 'Y'
                                        AND tblOutcome.n_outcomecodeid > 0
                            ) AS st
                    WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            OutcomeCodeSqlDatasource.SelectParameters.Clear();
            OutcomeCodeSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            OutcomeCodeSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));
            OutcomeCodeSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));
            comboBox.DataSource = OutcomeCodeSqlDatasource;
            comboBox.DataBind();
        }

        /// <summary>
        /// Loads Outcome Code Data By ID
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ComboOutcomeCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            var comboBox = (ASPxComboBox)source;
            OutcomeCodeSqlDatasource.SelectCommand = @"SELECT    
                                                        tblOutcome.n_outcomecodeid ,
                                                        tblOutcome.outcomecodeid ,
                                                        tblOutcome.[description] ,
                                                        ROW_NUMBER() OVER ( ORDER BY tblOutcome.n_outcomecodeid ) AS [rn]
                                                FROM      dbo.OutcomeCodes AS tblOutcome
                                                WHERE   ( n_outcomecodeid = @ID )
                                                ORDER BY outcomecodeid";

            OutcomeCodeSqlDatasource.SelectParameters.Clear();
            OutcomeCodeSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = OutcomeCodeSqlDatasource;
            comboBox.DataBind();
        }

        /// <summary>
        /// Batch Posts Selected Jobs
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void BatchPostSelection(object sender, EventArgs e)
        {
            //Batch Post Selected Jobs
            try
            {
                //Check For Multi Selection
                if ((PlannedGrid.Columns[0].Visible))
                {
                    //Get Job Selections
                    var jobSelections =
                        PlannedGrid.GetSelectedFieldValues(new[] { "n_jobid", "n_jobstepid" });

                    //Process Multi Selection
                    foreach (object[] selected in jobSelections)
                    {
                        //Get Keys & Dates
                        var jobKey = Convert.ToInt32(selected[0].ToString());
                        var jobStepKey = Convert.ToInt32(selected[1].ToString());

                        //Check Keys
                        if ((jobKey > 0) && (jobStepKey > 0))
                        {
                            //Get Post Date
                            if ((txtPostDate.Value != null) && (txtPostDate.Value.ToString() != ""))
                            {
                                //Check Outcome Code
                                if (ComboOutcomeCode.Value != null)
                                {
                                    //Get Post Date
                                    var postDate = Convert.ToDateTime(txtPostDate.Value.ToString());

                                    //Get Outcome Code
                                    var outcomeId = Convert.ToInt32(ComboOutcomeCode.Value.ToString());

                                    //Clear Errors
                                    _oJob.ClearErrors();

                                    //Load Job Data
                                    if (_oJob.LoadData(jobKey))
                                    {
                                        //Check Table COunt
                                        if (_oJob.Ds.Tables.Count > 0)
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
                }
                else
                {
                    if (!(PlannedGrid.Columns[0].Visible)  && Selection.Contains("n_Jobid")  && Selection.Contains("n_jobstepid") && Selection.Contains("step"))
                    {                       
                                    //Get Values
                                    var jobKey = Convert.ToInt32(Selection.Get("n_Jobid"));
                                    var jobStepKey = Convert.ToInt32(Selection.Get("n_jobstepid"));
                             
                                    //Check Keys
                                    if ((jobKey > 0) && (jobStepKey > 0))
                                    {
                                        //Get Post Date
                                        if ((txtPostDate.Value != null) && (txtPostDate.Value.ToString() != "") && ComboOutcomeCode.Value != null)
                                        {
                                            //Check Outcome Code
                                            
                                                //Get Post Date
                                                var postDate = Convert.ToDateTime(txtPostDate.Value.ToString());

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
                if (!(PlannedGrid.Columns[0].Visible))
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
                    _oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

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
                                        if (_oJobStep.Add(jobId,
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
                                            jobstepId = _oJobStep.RecordID;

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
                                                        (int) dsTaskEquip.Tables[0].Rows[je][5];
                                                    var equipmentCost =
                                                        (decimal) dsTaskEquip.Tables[0].Rows[je][7];
                                                    var desc =
                                                        (string) dsTaskEquip.Tables[0].Rows[je][2];
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
                                            errorInfo = "Error Adding Routine Job Step - " + _oJobStep.LastError;
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
                if (!(PlannedGrid.Columns[0].Visible))
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
                    _oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

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
                                            if (_oJobStep.Add(jobId,
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
                                                jobstepId = _oJobStep.RecordID;

                                                //Update Jobstep Costing
                                                if (_oJobStep.UpdateJobstepCosting(jobId,
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
                                                        _oJobStep.LastError;
                                                }
                                            }
                                            else
                                            {
                                                //Error Hit
                                                //Set Flag
                                                success = false;

                                                //Set Error Information
                                                errorInfo = "Error Adding PM Job Step - " + _oJobStep.LastError;
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
                                                                            catch (Exception)
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