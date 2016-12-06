using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using MPETDSFactory;

namespace Pages.Tasks
{
    public partial class TasksList : System.Web.UI.Page
    {
        private LogonObject _oLogon;
        private bool _userCanDelete;
        private bool _userCanAdd;
        private bool _userCanEdit;
        private bool _userCanView;
        private const int AssignedFormId = 9;
        private MPETDSFactory.Tasks _oTasks;
        private string _connectionString = "";
        private bool _useWeb;

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
                    Master.ShowPrintButton = true;
                    Master.ShowPdfButton = false;
                    Master.ShowXlsButton = true;
                    Master.ShowMultiSelectButton = _userCanDelete;
                }
            }

            //Check For Post Back
            if (!IsPostBack)
            {
                //Bind Grid
                PMTaskGrid.DataBind();
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
                        case "MultiSelect":
                        {
                            //Enable/Disable MultiSelect
                            EnableMultiSelect(!(PMTaskGrid.Columns[0].Visible));
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
            Master.ShowNewButton = !(PMTaskGrid.Columns[0].Visible);
            Master.ShowEditButton = !(PMTaskGrid.Columns[0].Visible);
            Master.ShowViewButton = !(PMTaskGrid.Columns[0].Visible);
            Master.ShowPrintButton = !(PMTaskGrid.Columns[0].Visible);

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(PMTaskGrid.Columns[0].Visible))
            {
                //Uncheck All
                PMTaskGrid.Selection.UnselectAll();
            }
        }

        /// <summary>
        /// Sets Up Form Permissions
        /// </summary>
        /// <param name="userId">User Id To Load Permissions For</param>
        /// <returns>True/False For Success</returns>
        public bool FormSetup(int userId)
        {
            //Create Flag
            bool rightsLoaded ;

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

        /// <summary>
        /// Enable/Disable Multiselect For Grid
        /// </summary>
        /// <param name="showMultiSelect">Bool To Enable/Disable</param>
        protected void EnableMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            PMTaskGrid.Columns[0].Visible = showMultiSelect;
        }

        /// <summary>
        /// Adds New Task
        /// </summary>
        private void AddNewRow()
        {
            //Redirect To Task Page
            Response.Redirect("~/Pages/Tasks/Tasks.aspx", true);
        }

        /// <summary>
        /// Edits Selected Row
        /// </summary>
        private void EditSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("taskid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/Tasks/Tasks.aspx?taskid=" + Selection.Get("taskid"), true);
            }
        }

        /// <summary>
        /// Views Selected Row
        /// </summary>
        private void ViewSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("taskid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/Tasks/Tasks.aspx?taskid=" + Selection.Get("taskid"), true);
            }
        }

        /// <summary>
        /// Deletes Selected Row
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
                if ((PMTaskGrid.Columns[0].Visible))
                {
                    //Get Selections
                    var recordIdSelection = PMTaskGrid.GetSelectedFieldValues("n_taskid");

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
                            _oTasks.ClearErrors();

                            //Delete Jobstep
                            if (_oTasks.Delete(recordToDelete))
                            {
                                //Set Deletion Done
                                deletionDone = true;
                            }
                        }

                        //Check Deletion Done
                        if (deletionDone)
                        {
                            //Perform Refresh
                            PMTaskGrid.DataBind();
                        }
                    }
                }
                else
                {
                    //Check For Job ID
                    if (Selection.Contains("n_taskid"))
                    {
                        //Get ID
                        recordToDelete = Convert.ToInt32(Selection.Get("n_taskid"));
                    }

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oTasks.ClearErrors();

                        //Delete Jobstep
                        if (_oTasks.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        PMTaskGrid.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// Prints Selected Row
        /// </summary>
        private void PrintSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("n_taskid"))
            {
                //Check For Previous Session Report Parm ID
                if (HttpContext.Current.Session["ReportParm"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportParm");
                }

                //Add Session Report Parm ID
                HttpContext.Current.Session.Add("ReportParm", Selection.Get("n_taskid"));

                //Check For Previous Report Name
                if (HttpContext.Current.Session["ReportToDisplay"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportToDisplay");
                }

                //Add Report To Display
                HttpContext.Current.Session.Add("ReportToDisplay", "TaskDetail.rpt");

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
        /// <param name="fileName">Filename</param>
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

        /// <summary>
        /// Initialize Classes On Page Init
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //Set Connection Info
            _connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
            _useWeb = (ConfigurationManager.AppSettings["UsingWebService"] == "Y");

            //Initialize Classes
            _oTasks = new MPETDSFactory.Tasks(_connectionString, _useWeb);
        }

        protected void PMTaskGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //Redirect To Edit Page With Job ID
            ASPxWebControl.RedirectOnCallback("~/Pages/Tasks/Tasks.aspx?taskid=" + e.EditingKeyValue);
        }

        /// <summary>
        /// Unloads & Registers Update Panels
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            //Register Panel
            RegisterUpdatePanel((UpdatePanel)sender);
        }

        /// <summary>
        /// Registers Update Panel
        /// </summary>
        /// <param name="panel">Panel To Register</param>
        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }
    }
}