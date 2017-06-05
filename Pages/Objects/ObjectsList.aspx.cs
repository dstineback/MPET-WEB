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

namespace Pages.Objects
{
    public partial class ObjectsList : Page
    {
        private LogonObject _oLogon;
        private MaintenanceObject _oMaintenanceObject;
        private bool _userCanDelete;
        private bool _userCanAdd;
        private bool _userCanEdit;
        private bool _userCanView;
        private const int AssignedFormId = 40;
        private string _connectionString = "";
        private bool _useWeb;
        private object _MapSelected;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check For Logon Class
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //Add Use ID TO Session
                HttpContext.Current.Session.Add("UserID", _oLogon.UserID);

                RestSession();
                //Load Form Permissions
                if (FormSetup(_oLogon.UserID))
                {
                    
                    //Setup Buttons
                    Master.ShowSaveButton = false;
                    Master.ShowNewWRButton = _userCanAdd;
                    Master.ShowQuickPostButton = _userCanAdd;
                    
                    Master.ShowEditButton = false;/*_userCanEdit;*/
                    Master.ShowDeleteButton = false; /*_userCanDelete;*/
                    Master.ShowViewButton = false;  /*_userCanView;*/
                    Master.ShowPrintButton = false;
                    Master.ShowPdfButton = false;
                    Master.ShowXlsButton = false;
                    Master.ShowMultiSelectButton = _userCanView; /*_userCanDelete;*/
                    Master.ShowMapDisplayButton = _userCanEdit;
                }
            }

            //Check For Post Back
            if (!IsPostBack)
            {
                //Bind Grid
                ObjectGrid.DataBind();
                ObjectGrid.Focus();
                MaintainScrollPositionOnPostBack = true;

                
                
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
                        case "NewWRButton":
                        {                            
                            //Call View Routine
                            AddNewRow();
                            break;
                        }
                        case "QuickPostButton":
                        {
                                QuickPost();
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
                            EnableMultiSelect(!(ObjectGrid.Columns[0].Visible));
                            break;
                        }
                        case "MapDisplay":
                        {
                                MapItem();
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
            Master.ShowNewWRButton = !(ObjectGrid.Columns[0].Visible);
            Master.ShowQuickPostButton = !(ObjectGrid.Columns[0].Visible);
            //Master.ShowEditButton = !(ObjectGrid.Columns[0].Visible);
            //Master.ShowViewButton = !(ObjectGrid.Columns[0].Visible);
            //Master.ShowPrintButton = !(ObjectGrid.Columns[0].Visible);

            //Clear Prior Selection If Edit Check Is No Longer Visible
            if (!(ObjectGrid.Columns[0].Visible))
            {
                //Uncheck All
                ObjectGrid.Selection.UnselectAll();
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

        /// <summary>
        /// Initializes Object Classes On Page Init
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //Set Connection Info
            _connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
            _useWeb = (ConfigurationManager.AppSettings["UsingWebService"] == "Y");

            //Initialize Classes
            _oMaintenanceObject = new MaintenanceObject(_connectionString, _useWeb);
        }

        /// <summary>
        /// Enable/Disable Multiselect For Grid
        /// </summary>
        /// <param name="showMultiSelect">Bool To Enable/Disable</param>
        protected void EnableMultiSelect(bool showMultiSelect)
        {
            //Enable/Disable Grid Select
            ObjectGrid.Columns[0].Visible = showMultiSelect;
        }

        /// <summary>
        /// Adds New Object
        /// </summary>
        private void AddNewRow()
        {

            if (Selection.Contains("objectid"))
            {
                Session.Remove("objectid");
                Session.Add("objectid", Selection.Get("objectid"));

                if (Selection.Contains("description"))
                {
                    Session.Remove("description");
                    Session.Add("description", Selection.Get("description"));
                }

                if (Selection.Contains("n_object"))
                {
                    Session.Remove("n_object");
                    Session.Add("n_object", Selection.Get("n_object"));
                }

            //Redirect To Task Page
            Response.Redirect("~/Pages/WorkRequests/WorkRequestForm.aspx", true);
            } else
            {
                Response.Write("<script language='javascript'>alert('No Item was selected from Grid. Please choose one item to make a new work request.');</script>");
            }
        }

        private void QuickPost()
        {
            
            if (Selection.Contains("objectid"))
            {
                Session.Remove("objectid");
                Session.Add("objectid", Selection.Get("objectid"));

                if (Selection.Contains("description"))
                {
                    Session.Remove("description");
                    Session.Add("description", Selection.Get("description"));
                }

                if (Selection.Contains("n_objectid"))
                {
                    Session.Remove("n_objectid");
                    Session.Add("nobjectid", Selection.Get("n_objectid"));
                }

                if (Selection.Contains("Area"))
                {
                    Session.Remove("Area");
                    Session.Add("Area", Selection.Get("Area"));
                }

                if (Selection.Contains("Location"))
                {
                    Session.Remove("LocationID");
                    Session.Add("LocationID", Selection.Get("LocationID"));
                }
                
                if (Selection.Contains("AssetNumber"))
                {
                    Session.Remove("AssetNumber");
                    Session.Add("AssetNumber", Selection.Get("AssetNumber"));
                }

                //Redirect To Task Page
                Response.Redirect("~/Pages/QuickPost/QuickPost.aspx", true);
            }
            else
            {
                Response.Write("<script language='javascript'>alert('No Item was selected from Grid. Please choose one item to make a new work request.');</script>");
            }
        }

        /// <summary>
        /// Edits Selected Row
        /// </summary>
        private void EditSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("objectid"))
            {
                //Redirect To Edit Page With ID
                Response.Redirect("~/Pages/Objects/Objects.aspx?objectid=" + Selection.Get("objectid"), true);
            }
        }

        /// <summary>
        /// Views Selected Row
        /// </summary>
        private void ViewSelectedRow()
        {
            //Check For Row Value In Hidden Field (Set Via JS)
            if (Selection.Contains("objectid"))
            {
                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/Objects/Objects.aspx?objectid=" + Selection.Get("objectid"), true);
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
                if ((ObjectGrid.Columns[0].Visible))
                {
                    //Get Selections
                    var recordIdSelection = ObjectGrid.GetSelectedFieldValues("n_taskid");

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
                            _oMaintenanceObject.ClearErrors();

                            //Delete Jobstep
                            if (_oMaintenanceObject.Delete(recordToDelete))
                            {
                                //Set Deletion Done
                                deletionDone = true;
                            }
                        }

                        //Check Deletion Done
                        if (deletionDone)
                        {
                            //Perform Refresh
                            ObjectGrid.DataBind();
                        }
                    }
                }
                else
                {
                    //Check For Job ID
                    if (Selection.Contains("n_objectid"))
                    {
                        //Get ID
                        recordToDelete = Convert.ToInt32(Selection.Get("n_objectid"));
                    }

                    //Set Continue Bool
                    continueDeletion = (recordToDelete > 0);

                    //Check Continue Bool
                    if (continueDeletion)
                    {
                        //Clear Errors
                        _oMaintenanceObject.ClearErrors();

                        //Delete Jobstep
                        if (_oMaintenanceObject.Delete(recordToDelete))
                        {
                            //Set Deletion Done
                            deletionDone = true;
                        }
                    }

                    //Check Deletion Done
                    if (deletionDone)
                    {
                        //Perform Refresh
                        ObjectGrid.DataBind();
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
            if (Selection.Contains("n_objectid"))
            {
                //Check For Previous Session Report Parm ID
                if (HttpContext.Current.Session["ReportParm"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportParm");
                }

                //Add Session Report Parm ID
                HttpContext.Current.Session.Add("ReportParm", Selection.Get("n_objectid"));

                //Check For Previous Report Name
                if (HttpContext.Current.Session["ReportToDisplay"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("ReportToDisplay");
                }

                //Add Report To Display
                HttpContext.Current.Session.Add("ReportToDisplay", "ObjectDetail.rpt");

                //Redirect To Report Page
                Response.Redirect("~/Reports/ViewReport.aspx", true);
            }
        }

        private void RestSession()
        {
            if (HttpContext.Current.Session["MapSelected"] != null)
            {
                //Remove Value
                HttpContext.Current.Session.Remove("MapSelected");
            }

            //Check For Previous Session n_objectid
            if (HttpContext.Current.Session["n_objectid"] != null)
            {
                //Remove Value
                HttpContext.Current.Session.Remove("n_objectid");
            }

            //Check for previous session object ID
            if (HttpContext.Current.Session["objectid"] != null)
            {
                //Remove Value
                HttpContext.Current.Session.Remove("objectid");
            }

        }

        private void MapItem()
        {
            var sel = Selection.Count;
            var MapSelected = ObjectGrid.GetSelectedFieldValues("objectid","n_objectid", "Latitude", "Longitude", "description", "Area", "AssetNumber", "LocationID");

            if (sel > 0 || MapSelected.Count > 0) { 
                if(HttpContext.Current.Session["MapSelected"] != null)
                {
                    //Remove Value
                    HttpContext.Current.Session.Remove("MapSelected");
                }

                if(MapSelected.Count > 0)
                {

                    HttpContext.Current.Session.Add("MapSelected", MapSelected);
                }
            
                //Check For Row Value In Hidden Field (Set Via JS)
                if (Selection.Contains("n_objectid") && MapSelected.Count < 1)
                {
                    //Check For Previous Session n_objectid
                    if (HttpContext.Current.Session["n_objectid"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("n_objectid");
                    }
                    //Add Session N_objectid
                    HttpContext.Current.Session.Add("n_objectid", Selection.Get("n_objectid"));

                    //Check for previous session object ID
                    if(HttpContext.Current.Session["objectid"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("objectid");
                    }
                    //Add session object ID
                    HttpContext.Current.Session.Add("objectid", Selection.Get("objectid"));


                    //Check For Previous Session Latitude
                    if (HttpContext.Current.Session["Latitude"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("Latitude");
                    }
                    //Add Session Latitude
                    HttpContext.Current.Session.Add("Latitude", Selection.Get("Latitude"));


                    //Check For Previous Session Longitude
                    if (HttpContext.Current.Session["Longitude"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("Longitude");
                    }
                    //Add Session Longitude
                    HttpContext.Current.Session.Add("Longitude", Selection.Get("Longitude"));

                    //Check for previous session object description
                    if(HttpContext.Current.Session["description"] != null)
                    {
                        //Remove Value
                        HttpContext.Current.Session.Remove("description");
                    }
                    //Add session object description
                    HttpContext.Current.Session.Add("objectDescription", Selection.Get("description")); 
                    
                    //Check for Previous Session Object Area
                    if(Session["Area"] != null)
                    {
                        Session.Remove("Area");
                    }
                    Session.Add("Area", Selection.Get("Area"));

                    //Check for previous Session Object Asset number
                    if(Session["AssetNumber"] != null)
                    {
                        Session.Remove("AssetNumber");
                    }
                    Session.Add("AssetNumber", Selection.Get("AssetNumber"));

                    //Check for previous Session Object Location
                    if(Session["LocationID"] != null)
                    {
                        Session.Remove("LocationID");
                    }
                    Session.Add("LocationID", Selection.Get("LocationID"));
                }
                    //Redirect To Report Page
                    Response.Redirect("~/Pages/Map/MapForm.aspx", true);
            }
            else { HttpContext.Current.Response.Write("<script language='javascript'>alert('Error trying to Map Items, No rows were selected.');</script>"); };
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

        protected void ObjectGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            //Redirect To Edit Page With Job ID
            ASPxWebControl.RedirectOnCallback("~/Pages/Objects/Objects.aspx?objectid=" + e.EditingKeyValue);
        }
    }
}