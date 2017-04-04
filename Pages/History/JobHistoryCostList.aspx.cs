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
    public partial class JobHistoryCostList : Page
    {
        private LogonObject _oLogon;
        private WorkOrder _oJob;
        private bool _userCanDelete;
        private bool _userCanAdd;
        private bool _userCanEdit;
        private bool _userCanView;
        private const int AssignedFormId = 55;

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
                //ReqGrid.DataSource = GetData(ReqGrid.FilterExpression);
                HistoryCostGrid.DataBind();
            }
            else
            {
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
                    switch (controlName.Replace("ctl00$Footer$", ""))
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
            Master.ShowViewButton = !(HistoryCostGrid.Columns[0].Visible);
            Master.ShowPrintButton = false;/*!(HistoryCostGrid.Columns[0].Visible);*/
            Master.ShowCopyJobButton = !(HistoryCostGrid.Columns[0].Visible);

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(HistoryCostGrid.Columns[0].Visible))
            {
                //Uncheck All
                HistoryCostGrid.Selection.UnselectAll();
            }

            //Bind Grid
            //HistoryCostGrid.DataBind();
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

        /// <summary>
        /// Prints Selected Row
        /// </summary>
        private void PrintSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("n_jobid"))
            {
                //Check For Previous Session Report Parm ID
                if (HttpContext.Current.Session["ReportParm"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportParm");
                }

                //Add Session Report Parm ID
                HttpContext.Current.Session.Add("ReportParm", Selection.Get("n_jobid"));

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

        /// <summary>
        /// Export Grid To PDF
        /// </summary>
        private void ExportPdf()
        {
            //Call Export Routine
            gridExport.WritePdfToResponse();
        }

        /// <summary>
        /// Export Curren Grid TO XLS Document
        /// </summary>
        private void ExportXls()
        {
            //Call Export Routine
            gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        /// <summary>
        /// Writes Export Response To File
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="saveAsFile">Save As Flag</param>
        /// <param name="fileFormat">File Format</param>
        /// <param name="stream">Memory Stream To Use</param>
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

        protected void HistoryCostGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            //Redirect To Edit Page With Job ID
            ASPxWebControl.RedirectOnCallback("~/Pages/PlannedJobs/PlannedJobs.aspx?n_jobid=" + e.EditingKeyValue);
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
                if (!(HistoryCostGrid.Columns[0].Visible))
                {
                    //Check For Job ID
                    if (Selection.Contains("n_jobid"))
                    {
                        //Check For Job Step ID
                        if (Selection.Contains("n_jobstepid"))
                        {
                            //Get Values
                            var jobKey = Convert.ToInt32(Selection.Get("n_jobid"));
                            const int jobStepNum = 1;
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
    }
}