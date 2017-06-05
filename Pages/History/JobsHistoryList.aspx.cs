using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using MPETDSFactory;
using Page = System.Web.UI.Page;

namespace Pages.History
{
    public partial class JobsHistoryList : Page
    {
        private LogonObject _oLogon;
        private WorkOrder _oJob;
        private bool _userCanDelete;
        private bool _userCanAdd;
        private bool _userCanEdit;
        private bool _userCanView;
        private const int AssignedFormId = 55;

        /// <summary>
        /// Processes Page Load 
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
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
                    Master.ShowNewButton = false;
                    Master.ShowEditButton = false;
                    Master.ShowDeleteButton = false;
                    Master.ShowViewButton = _userCanView;
                    Master.ShowCopyJobButton = _userCanAdd;
                    Master.ShowPrintButton = false;
                    Master.ShowPdfButton = false;
                    Master.ShowXlsButton = false;
                    Master.ShowMultiSelectButton = false;
                }
            }

            //Check For Post Back
            if (!IsPostBack)
            {
                //Bind Grid
                HistoryGrid.DataBind();
            }
            else
            {
                //Check Async
                var scriptManager = ScriptManager.GetCurrent(Page);
                if (scriptManager != null && scriptManager.IsInAsyncPostBack)
                {

                }

                //Get Control That Caused Post Back
                var controlName = Request.Params.Get("__EVENTTARGET");

                //Check For Null
                if (!string.IsNullOrEmpty(controlName))
                {
                    //Determing What To Do
                    switch (controlName.Replace("ctl00$Footer$",""))
                    {
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
                        case "CopyJob":
                        {
                            //Call Copy Routine
                            CopyJobRoutine();

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
            }

            //Enable/Disable Buttons
            Master.ShowViewButton = !(HistoryGrid.Columns[0].Visible);
            Master.ShowPrintButton = false;/*!(HistoryGrid.Columns[0].Visible);*/
            Master.ShowCopyJobButton = !(HistoryGrid.Columns[0].Visible);

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(HistoryGrid.Columns[0].Visible))
            {
                //Uncheck All
                HistoryGrid.Selection.UnselectAll();
            }
        }

        /// <summary>
        /// Sets Up Form Permissions For Logged In User
        /// </summary>
        /// <param name="userId">User Logged In</param>
        /// <returns>True/False For Success</returns>
        public bool FormSetup(int userId)
        {
            //Create Flag
            bool rightsLoaded;

            //Get Security Settings
            using (
                var oSecurity = new UserSecurityTemplate(
                    ConfigurationManager.ConnectionStrings["connection"].ToString(),
                    ConfigurationManager.AppSettings["UsingWebService"] == "Y"))
            {
                //Get Rights
                rightsLoaded = oSecurity.GetUserFormRights(userId, AssignedFormId,
                    ref _userCanEdit, ref _userCanAdd,
                    ref _userCanDelete, ref _userCanView);
            }

            //Return Flag
            return rightsLoaded;
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

        private void PrintSelectedRow()
        {
        
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
        /// Export Grid To XLS
        /// </summary>
        private void ExportXls()
        {
            //Call Export Routine
            gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        /// <summary>
        /// Writes File Response
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="saveAsFile">Save As Flag</param>
        /// <param name="fileFormat">File Format</param>
        /// <param name="stream">Memory Stream</param>
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
            //Initialize Work Order Class
            _oJob = new WorkOrder(ConfigurationManager.ConnectionStrings["connection"].ToString(),
                ConfigurationManager.AppSettings["UsingWebService"] == "Y");
        }

        protected void HistoryGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            //Redirect To Edit Page With Job ID
            ASPxWebControl.RedirectOnCallback("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + e.EditingKeyValue);
        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            RegisterUpdatePanel((UpdatePanel)sender);
        }

        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }

        protected string GetUrl(GridViewDataItemTemplateContainer container)
        {
            var values = (int)container.Grid.GetRowValues(container.VisibleIndex, new[] { "n_jobstepid" });
            return "~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobstepid=" + values;
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
                if (!(HistoryGrid.Columns[0].Visible))
                {
                    //Check For Job ID
                    if (Selection.Contains("n_jobid"))
                    {
                        //Check For Job Step ID
                        if (Selection.Contains("n_jobstepid"))
                        {
                            //Check For Step Number
                            if (Selection.Contains("step"))
                            {
                                //Get Values
                                var jobKey = Convert.ToInt32(Selection.Get("n_jobid"));
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
    }
}