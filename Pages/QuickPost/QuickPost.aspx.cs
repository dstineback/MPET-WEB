using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Web.Configuration;
using DevExpress.Web;
using MPETDSFactory;
using Page = System.Web.UI.Page;

namespace Pages.QuickPost
{
    public partial class QuickPost : Page
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
        private JobType jobType = JobType.Corrective;
        public string AssignedGuid = "";
        public string AssignedJobID = "";
        private bool requestOnlyJob = true;
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
        private int JobID;
        private int JobStepID;
        private object oJob;
        private string crewMemberSelected;



        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.RegisterBaseScript(Page);

            #region Attempt to Load Logon Info
            //Check for Logon Class
            if (Session["LogonInfo"] != null)
            {
                //Get Logon from Session
                _oLogon = ((LogonObject)Session["LogonInfo"]);

                //Addd UserID to Sesion
                Session.Add("UserID", _oLogon.UserID);

                //Load Permissions
                if (FormSetup(_oLogon.UserID))
                {
                    //Setup Buttons
                }

            }
            #endregion
            #region Attempt to Load Azure Details
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

            #endregion
            #region Check for post to Setup Form
            if (!IsPostBack)
            {

                #region Make Job ID and Step ID
                if (Session["LogonInfo"] != null)
                {
                    ResetSession();   
                    GetJobID();
                    oJob = HttpContext.Current.Session["oJob"];
                    JobID = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
                    AssignedJobID = HttpContext.Current.Session["AssignedJobID"].ToString();
                    comboElementID.Enabled = false;

                    chkPostDefaults.Enabled = false;
                    ComboCostCode.Enabled = false;
                    ComboFundGroup.Enabled = false;
                    ComboOrgCode.Enabled = false;
                    ComboWorkOrder.Enabled = false;
                    ComboWorkOp.Enabled = false;
                    AttachmentGrid.Enabled = false;
                    AttachmentGrid.Visible = false;
                    PhotoContainer.Visible = false;
                    UpdatePanel1.Visible = false;
                    TabPageControl.Visible = false;
                    TabPageContainer.Visible = false;
                    CrewGrid.Enabled = false;
                    CrewGrid.Visible = false;
                    MemberGrid.Enabled = false;
                    MemberGrid.Visible = false;
                    PartGrid.Enabled = false;
                    PartGrid.Visible = false;
                    EquipGrid.Enabled = false;
                    EquipGrid.Visible = false;
                    Master.ShowPostButton = false;
                    chkUpdateObjects.Enabled = false;

                }
                #endregion
                SetupForAdding();
                txtWorkDescription.Focus();
                #region Check for Object field population from another source
                
                if (Selection.Contains("nobjectid"))
                {
                    Session.Remove("nobjectid");
                    Session.Add("nobjectid", Selection.Get("nobjectid"));
                }
                {
                    
                }
                if (Session["nobjectid"] != null)
                {
                    ObjectIDCombo.Value = Convert.ToInt32(HttpContext.Current.Session["nobjectId"]).ToString();
                    
                    
                    if (Session["description"] != null)
                    {
                        txtObjectDescription.Value = Session["description"];
                    }

                    if (Session["objectDescription"] != null)
                    {
                        
                        txtObjectDescription.Value = Session["objectDescription"];
                    }

                    if (Session["Area"] != null)
                    {
                        txtObjectArea.Value = Session["Area"].ToString();
                    }

                    if (Session["AssestNumber"] != null)
                    {
                        txtObjectAssetNumber.Value = Session["AssetNumber"].ToString();
                    }

                    if (Session["LocationID"] != null)
                    {
                        txtObjectLocation.Value = Session["LocationID"].ToString();
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

            } else
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
                                AddNew();
                                break;
                            }
                        case "Save":
                            {
                                SaveSessionData();

                                break;
                            }
                        case "PostJob":
                            {
                                SaveSessionData();
                                if(Session["editingJobID"] != null && Session["JobStepID"] != null)
                                {
                                    PostPlanJob();
                                } else
                                {
                                    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('Not all fields or IDs are obtained. Please make sure to save and popluate required fields to proceed.');</script>");
                                }

                            }
                            break;
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


                }
                #endregion

            }
            #endregion
            #region Is not a Post-Back
            if (IsPostBack)
            {
                if(Session["JobStepID"] != null && Session["editingJobID"] != null)
                {
                    Master.ShowPostButton = true;
                } else
                {
                    Master.ShowPostButton = false;
                }
                #region Setting Headers with Job ID
                if (Session["AssignedJobID"] != null)
                {
                    lblHeader.Text = "Job ID: " + Session["AssignedJobID"].ToString();
                }
                else
                {
                    lblHeader.Text = "Job ID: ";
                }

                if (Session["JobStepID"] != null)
                {
                    lblStep.Text = "Job Step ID: " + Session["JobStepID"].ToString();
                }
                else
                {
                    lblStep.Text = "Job Step ID: ";
                }
                #endregion

                #region Work Description
                if (Session["txtWorkDescription"] != null)
                {
                    txtWorkDescription.Text = Session["txtWorkDescription"].ToString();
                }
                #endregion

                #region Start Date
                if (Session["TxtWorkStartDate"] != null)
                {
                    TxtWorkStartDate.Value = Convert.ToDateTime(Session["txtWorkStartDate"].ToString());
                }
                #endregion

                #region Object Info
                //Check For Previous Session Variables
                if (HttpContext.Current.Session["ObjectIDCombo"] != null)
                {
                    //Get Info From Session
                    ObjectIDCombo.Value = HttpContext.Current.Session["ObjectIDCombo"];
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
                #endregion

                #region Complettion Date
                if (Session["txtWorkCompDate"] != null)
                {
                    TxtWorkCompDate.Value = Convert.ToDateTime(Session["txtWorkCompDate"].ToString());
                }
                #endregion

                #region Completed By
                if (Session["comboCompletedBy"] != null)
                {
                    ComboCompletedBy.Value = Session["comboCompletedBy"].ToString();
                }
                #endregion

                #region Job Length
                if (Session["txtJobLength"] != null)
                {
                    txtJobLength.Value = Session["txtJobLength"].ToString();
                }
                #endregion

                #region Reason
                if (Session["comboReason"] != null)
                {
                    comboReason.Value = Session["comboReason"].ToString();
                }
                #endregion

                #region OutCome
                if (Session["ComboOutcome"] != null)
                {
                    ComboOutcomeCode.Value = Session["outcomeCode"].ToString();
                }
                #endregion

                #region Priority
                if (Session["ComboPriority"] != null)
                {
                    ComboPriority.Value = Session["ComboPriority"].ToString();
                }
                #endregion

                #region Element
                if (Session["elementID"] != null)
                {
                    comboElementID.Value = Session["elementID"].ToString();

                }
                #endregion

                #region Sub Assembly
                if (Session["subAssembly"] != null)
                {
                    comboSubAssembly.Value = Session["subAssembly"].ToString();
                }
                #endregion

                #region Highway Route
                if (Session["comboHwyRoute"] != null)
                {
                    comboHwyRoute.Value = Session["comboHwyRoute"].ToString();
                }
                #endregion

                #region Milepost From
                if (Session["txtMilepost"] != null)
                {
                    txtMilepost.Value = Session["txtMilepost"].ToString();
                }
                #endregion

                #region Direction
                if (Session["comboMilePostDir"] != null)
                {
                    comboMilePostDir.Value = Session["comboMilePostDir"].ToString();
                }
                #endregion

                #region MilePost To
                if (Session["txtMilepostTo"] != null)
                {
                    txtMilepostTo.Value = Session["txtMilepostTo"].ToString();
                }
                #endregion

                #region Breakdown
                if (Session["breakdownBox"] != null)
                {
                    breakdownBox.Checked = true;
                }
                #endregion

                #region Update Object
                
                #endregion

                #region Post Defaults
                if (Session["chkPostDefaults"] != null)
                {
                    chkPostDefaults.Value = Session["chkPostDefaults"].ToString();
                }
                #endregion

                #region CostCode
                if (Session["ComboCostCode"] != null)
                {
                    ComboCostCode.Value = Session["ComboCostCode"].ToString();
                }
                #endregion

                #region Fund Source
                if (Session["ComboFundSource"] != null)
                {
                    ComboFundSource.Value = Session["ComboFundSource"].ToString();
                }
                #endregion

                #region Work Order Code
                if (Session["ComboWorkOrder"] != null)
                {
                    ComboWorkOrder.Value = Session["ComboWorkOrder"].ToString();
                }
                #endregion

                #region Work Operation
                if (Session["ComboWorkOp"] != null)
                {
                    ComboWorkOp.Value = Session["ComboWorkOp"].ToString();
                }
                #endregion

                #region Organization Code
                if (Session["ComboOrgCode"] != null)
                {
                    ComboOrgCode.Value = Session["ComboOrgCode"].ToString();
                }
                #endregion

                #region Funding Group
                if (Session["ComboFundGroup"] != null)
                {
                    ComboFundGroup.Value = Session["ComboFundGroup"].ToString();
                }
                #endregion

                #region ControlSection
                if (Session["ComboCtlSelection"] != null)
                {
                    ComboCtlSection.Value = Session["ComboCtlSelection"].ToString();
                }
                #endregion

                #region Equipment
                if (Session["ComboEquipNum"] != null)
                {
                    comboElementID.Value = Session["ComboEquipNum"].ToString();
                }
                #endregion

                #region Post Notes
                if (Session["txtPostNotes"] != null)
                {
                    txtPostNotes.Text = Session["txtPostNotes"].ToString();
                }
                #endregion
            }
            #endregion
            #region Button Status
            Master.ShowNewButton = true;
            if (Session["editingJobID"] != null && Session["JobStepID"] != null)
            {
                Master.ShowPostButton = true;
                
            }
            else
            {
                Master.ShowPostButton = false;
            }


            //Clear Prior Selection If Edit Check Is No Longer Visible
            //if (!(CrewGrid.Columns[0].Visible))
            //{
            //    //Uncheck All
            //    CrewGrid.Selection.UnselectAll();
            //}
            //else
            //{
            //    //Make Sure Settings Are Right
            //    CrewGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            //}

            ////Clear Prior Selection If Edit Check Is No Longer Visible
            //if (!(MemberGrid.Columns[0].Visible))
            //{
            //    //Uncheck All
            //    MemberGrid.Selection.UnselectAll();
            //}
            //else
            //{
            //    //Make Sure Settings Are Right
            //    MemberGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            //}

            ////Clear Prior Selection If Edit Check Is No Longer Visible
            //if (!(PartGrid.Columns[0].Visible))
            //{
            //    //Uncheck All
            //    PartGrid.Selection.UnselectAll();
            //}
            //else
            //{
            //    //Make Sure Settings Are Right
            //    PartGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            //}

            ////Clear Prior Selection If Edit Check Is No Longer Visible
            //if (!(EquipGrid.Columns[0].Visible))
            //{
            //    //Uncheck All
            //    EquipGrid.Selection.UnselectAll();
            //}
            //else
            //{
            //    //Make Sure Settings Are Right
            //    EquipGrid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            //}



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

                            TabPageControl.TabPages[0].ClientEnabled = false;  //Crew
                            TabPageControl.TabPages[2].ClientEnabled = false;  //Parts
                            TabPageControl.TabPages[3].ClientEnabled = false;  //Equip

                            break;
                        }
                    case "CrewGrid":
                        {
                            //Disable Other Tabs

                            TabPageControl.TabPages[1].ClientEnabled = false;  //Members
                            TabPageControl.TabPages[2].ClientEnabled = false;  //Parts
                            TabPageControl.TabPages[3].ClientEnabled = false;  //Equip

                            break;
                        }
                    case "PartGrid":
                        {
                            //Disable Other Tabs

                            TabPageControl.TabPages[0].ClientEnabled = false;  //Members
                            TabPageControl.TabPages[1].ClientEnabled = false;  //Crew
                            TabPageControl.TabPages[3].ClientEnabled = false;  //Equip

                            break;
                        }
                    case "EquipGrid":
                        {
                            //Disable Other Tabs

                            TabPageControl.TabPages[0].ClientEnabled = false;  //Members
                            TabPageControl.TabPages[1].ClientEnabled = false;  //Crew
                            TabPageControl.TabPages[2].ClientEnabled = false;  //Parts

                            break;
                        }

                    default:
                        {
                            //Make Sure All Tabs Are Client Enabled

                            TabPageControl.TabPages[0].ClientEnabled = true;  //Crews
                            TabPageControl.TabPages[1].ClientEnabled = true;  //Members
                            TabPageControl.TabPages[2].ClientEnabled = true;  //Parts
                            TabPageControl.TabPages[3].ClientEnabled = true;  //Equip

                            break;
                        }
                }
            }
            else
            {
                //Make Sure All Tabs Are Client Enabled

                TabPageControl.TabPages[0].ClientEnabled = true;  //Crew
                TabPageControl.TabPages[1].ClientEnabled = true;  //Members
                TabPageControl.TabPages[2].ClientEnabled = true;  //Parts
                TabPageControl.TabPages[3].ClientEnabled = true;  //Equip

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
            ElementsDataSource.ConnectionString = _connectionString;
            SubAssemblyDataSource.ConnectionString = _connectionString;

            //Setup Fields
            TxtWorkStartDate.Value = DateTime.Now;


        }

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
            }
            else
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

        /// <summary>
        /// Form Set up
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Set up buttons for Adding
        /// </summary>
        private void SetupForAdding()
        {
            //Setup Buttons
            Master.ShowPostButton = (_userCanAdd || _userCanEdit && Session["JobStepID"] != null);
            Master.ShowNewButton = _userCanAdd;

            //Disable Tabs
            //requestTab.Enabled = false;
        }

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

        protected void ComboSubAssembly_onItemRequestedByFiltercondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            try
            {

                ASPxComboBox comboBox = (ASPxComboBox)source;
                SubAssemblyDataSource.SelectCommand = @"Select [subAssemblyName],
	                                                            [n_subAssemblyID],
	                                                            [subAssemblyDesc]
	                                                            FROM ( Select	tblSubAssembly.nSubAssemblyID AS 'n_subAssemblyID'
					                                                            , tblSubAssembly.SubAssemblyName AS 'subAssemblyName'
					                                                            ,tblSubAssembly.SubAssemblyDesc AS 'subAssemblyDesc'
					                                                            , ROW_NUMBER() OVER (Order by tblSubAssembly.[subAssemblyName]) AS [rn]
					                                            FROM dbo.SubAssemblyNames AS tblSubAssembly
					                                            WHERE (([subAssemblyName] + ' ' + [subAssemblyDesc] ) LIKE @filter)
					                                                            AND tblSubAssembly.b_IsActive = 'Y'
					                                                            AND tblSubAssembly.nSubAssemblyID > 0 
					                                                            ) AS st
					                                                            WHERE st.[rn] BETWEEN @startIndex AND @endIndex";

                SubAssemblyDataSource.SelectParameters.Clear();
                SubAssemblyDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                SubAssemblyDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                SubAssemblyDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = SubAssemblyDataSource;
                comboBox.DataBind();


            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }


        protected void ComboSubAssembly_OnitemsRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            try
            {

                ASPxComboBox comboBox = (ASPxComboBox)source;
                SubAssemblyDataSource.SelectCommand = @"SELECT [nSubAssemblyID] AS 'n_subAssemblyID'
                                                          ,[SubAssemblyName]
                                                          ,[SubAssemblyDesc]
      
                                                      FROM [dbo].[SubAssemblyNames]
                                                      where b_IsActive = 'Y' and nSubAssemblyID > 0
                                                       ORDER By SubAssemblyName";

                SubAssemblyDataSource.SelectParameters.Clear();
                SubAssemblyDataSource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
                comboBox.DataSource = SubAssemblyDataSource;
                comboBox.DataBind();

            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        protected void ComboElements_OnItemsRequestedByFiltercondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {

        }

        protected void ComboElements_OnitemsRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {

        }
        #endregion

        #region Add/Post Job
        #region Save Session
        /// <summary>
        /// Saves Session Data
        /// </summary>
        protected void SaveSessionData()
        {
            if(Session["AssignedJobID"] != null)
            {
                lblHeader.Text = "Job ID: " + Session["AssignedJobID"].ToString();
            } else
            {
                lblHeader.Text = "Job ID: ";
            }

            if (Session["JobStepID"] != null)
            {
                lblStep.Text = "Job Step ID: " + Session["JobStepID"].ToString();
            } else
            {
                lblStep.Text = "Job Step ID: ";
            }
            
            #region Job Description 

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

            #region Object Info

            //Check For Input
            if (ObjectIDCombo.Value != null)
            {
                //See If Selection Changed

                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["ObjectIDCombo"] != null)
                {
                    if (ObjectIDCombo.Value.ToString() != null)
                    {
                        var n_objectid = ObjectIDCombo.Value.ToString();
                        //Remove Old One
                        HttpContext.Current.Session.Remove("ObjectIDCombo");
                        HttpContext.Current.Session.Add("ObjectIDCombo", n_objectid);

                    } else
                    {
                        var results = Session["ObjectIDCombo"].ToString();
                    }


                } else
                {
                    if (ObjectIDCombo.Value.ToString() != null)
                    {
                        var n_objectid = ObjectIDCombo.Value.ToString();
                        HttpContext.Current.Session.Add("ObjectIDCombo", n_objectid);
                        var results = Session["ObjectIDCombo"].ToString();
                    }
                    else
                    {
                        var results = Session["ObjectIDCombo"].ToString();
                    }
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

            if(Session["ObjectIDComb"] != null)
            {
                ObjectIDCombo.Value = Session["ObjectIDCombo"].ToString();
            }

            #endregion

            #region Completed By
            if (ComboCompletedBy.Value != null)
            {
                Session.Remove("ComboCompletedBy");
                HttpContext.Current.Session.Add("ComboCompletedBy", ComboCompletedBy.Value);
            }

            if(Session["ComboCompletedBy"] != null)
            {
                ComboCompletedBy.Value = Session["ComboCompletedBy"].ToString();
            }
            #endregion

            #region Job Length
            if (txtJobLength.Value != null)
            {
                var JobLength = txtJobLength.Value.ToString();
                Session.Add("txtJobLength", JobLength);
            }
            #endregion

            #region Start and Completion Dates

            if (TxtWorkStartDate.Value != null)
            {
                //Set Value
                HttpContext.Current.Session.Add("TxtWorkStartDate", TxtWorkStartDate.Value.ToString());

            }

            //Add Comp Date
            if (TxtWorkCompDate.Value != null)
            {

                //Set Value
                HttpContext.Current.Session.Add("TxtWorkCompDate", TxtWorkCompDate.Value.ToString());

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
            #region OutCome
            if (ComboOutcomeCode.Value != null)
            {
                if (HttpContext.Current.Session["ComboOutcome"] != null)
                {
                    Session.Remove("ComboOutCome");
                    HttpContext.Current.Session.Add("ComboOutcome", ComboOutcomeCode.Value);
                }

            } else
            {
                HttpContext.Current.Session.Add("ComboOutcome", ComboOutcomeCode.Value);
            }
            #endregion
            
            #region Elements
            if(comboElementID.Value != null)
            {
                Session.Add("elementID", comboElementID.Value);
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
            if (HttpContext.Current.Session["txtMilepost"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("txtMilepost");
                //Add New Value
                HttpContext.Current.Session.Add("txtMilepost", txtMilepost.Value.ToString());
            }


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

            if (comboHwyRoute.Value != null)
            {
                #region Combo Value

                //Check For Prior Value
                if (HttpContext.Current.Session["comboMilePostDir"] != null)
                {
                    if (HttpContext.Current.Session["comboMilePostDirText"].ToString() != comboHwyRoute.Text)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("comboMilePostDir");

                        //Add New Value
                        HttpContext.Current.Session.Add("comboMilePostDir", comboHwyRoute.Value.ToString());
                    }
                }
                else
                {
                    //Add New Value
                    HttpContext.Current.Session.Add("comboMilePostDir", comboHwyRoute.Value.ToString());
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
                HttpContext.Current.Session.Add("comboMilePostDirText", comboHwyRoute.Text.Trim());

                #endregion
            }

            #endregion

            ///TODO Add Sub ASSEMBLY Combo
            #region Sub Assembly
            if(comboSubAssembly.Value != null)
            {
                if(Session["subAssembly"] != null)
                {
                    Session.Remove("subAssembly");
                }
                Session.Add("subAssembly", comboSubAssembly.Value);
            }
            #endregion

            ///TODO add UPDATE OBJECT MAYBE
            #region Update Object

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

            #region BreakdownBox
            if (breakdownBox.Checked == true)
            {
                Session.Add("breakdownBox", breakdownBox.Checked);
            }
            #endregion

            #region PostDefaults
            if (chkPostDefaults.Checked == true)
            {
                Session.Add("chkPostDefaults", chkPostDefaults.Value);
            }

            #endregion


        }
        #endregion

        #region Post Job
        public void PostPlanJob()
        {
            #region Setting Vars to use in Stored Procedures from Session



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
            #region Get Description

            var workDesc = "";
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Get Additional Info From Session
                workDesc = (HttpContext.Current.Session["txtWorkDescription"].ToString());
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

            #region Completed By
            int completedBy = _oLogon.UserID;
            if (HttpContext.Current.Session["ComboCompletedBy"] != null)
            {
                completedBy = Convert.ToInt32(HttpContext.Current.Session["ComboCompletedBy"]);
            }
            #endregion

            #region Get Actual Job Length
            decimal jobActualLen = 0;
            if (Session["txtJobLength"] != null)
            {
                jobActualLen = Convert.ToDecimal(Session["txtJobLength"].ToString());
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

            #region Get Job Reason

            var reasonCode = -1;
            if ((HttpContext.Current.Session["comboReason"] != null))
            {
                //Get Info From Session
                reasonCode = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
            }

            #endregion

            #region Get Outcome

            var jobOutcome = -1;
            if ((HttpContext.Current.Session["ComboOutcome"] != null))
            {
                jobOutcome = Convert.ToInt32(Session["ComboOutcome"].ToString());
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

            #region Get Element
            var elementID = -1;
            if (Session["comboElementID"] != null)
            {
                elementID = Convert.ToInt32(Session["comboElementID"].ToString());
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

            #region Sub Assembly
            var subAssemblyID = -1;
            if (Session["comboSubAssembly"] != null)
            {
                subAssemblyID = Convert.ToInt32(Session["comboSubAssembly"].ToString());
            }
            #endregion

            ///TODO Update Object
            #region Update Object

            #endregion

            #region BreakDown
            var breakdown = false;
            if (breakdownBox.Checked == true)
            {
                breakdown = true;
            }
            #endregion

            #region Post Deafaults
            var postDefaults = false;
            if (chkPostDefaults.Checked == true)
            {
                postDefaults = true;
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

            #region Get Work Op

            var workOp = -1;
            if ((HttpContext.Current.Session["ComboWorkOp"] != null))
            {
                //Get Info From Session
                workOp = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
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

            var requestor = _oLogon.UserID;

            var jobStepConcurNumber = -1;
            var jobStepFollowStepNumber = -1;
            var jobStatus = -1;
            var jobLaborClass = -1;
            var jobGroup = -1;

            var jobShift = -1;
            var jobSupervisor = -1;
            var jobActualDt = 0;
            var jobEstimatedDt = 0;
            var jobEstimatedLen = 0;
            var jobRemainingDt = 0;
            var jobRemainingLen = 0;
            var jobReturnWithin = 0;
            var jobRouteTo = -1;
            var jobCompletedBy = requestor;
            //Create Class
            var oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);





            #endregion

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
                equipNumber,
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
            if (!_oJobStep.UpdateRouteAndCompletionInfo(jobStepId, jobRouteTo, completedBy, _oLogon.UserID))
            {
                //Throw Error
                throw new SystemException(
                    @"Error Updating Route To And Completion Information -" + _oJobStep.LastError);
            }
            if (!_oJobStep.UpdateJobstepCosting(jobID, jobStepId,
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
        
            //Get Values
            var jobKey = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
            var jobStepKey = Convert.ToInt32(HttpContext.Current.Session["JobStepID"]);
            #region Post functions
            //Check Keys
            if ((jobKey > 0) && (jobStepKey > -2))
            {
                //Get Post Date
                if ((TxtWorkCompDate.Value != null) && (TxtWorkCompDate.Value.ToString() != "") && ComboOutcomeCode.Value != null)
                {
                    //Check Outcome Code

                    //Get Post Date
                    var postDate = Convert.ToDateTime(TxtWorkCompDate.Value.ToString());

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


            Response.Redirect("~/main.aspx");
            Response.Write("<script language='javascript'>alert('You have sussefully created a Quick Post.');</script>");

        }
        #endregion
        #endregion
        #endregion
        #endregion
        private void AddNew()
        {
            ResetSession();
            Response.Redirect("~/Pages/QuickPost/QuickPost.aspx");
        }

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
            
            
        }

        protected void DeleteAttachmentButton_Click(object sender, EventArgs e)
        {
            DeleteGridViewAttachment();
        }
        #endregion

        #region Delete Setup for tab Grids
        /// <summary>
        /// Determines What Grid To Delete Items From
        /// </summary>
        private void DeleteItems()
        {
            //Determine Grid
            switch (TabPageControl.ActiveTabIndex)
            {
                
                case 0:
                    {
                        //Crew
                        DeleteSelectedCrew();
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
                        //Parts
                        DeleteSelectedParts();
                        break;
                    }
                case 3:
                    {
                        //Equip
                        DeleteSelectedEquip();
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
            switch (TabPageControl.ActiveTabIndex)
            {
                
                case 0:
                    {

                        //Crew Lookup

                        //Bind Grid
                        CrewLookupGrid.DataBind();

                        //Show Popup
                        AddCrewPopup.ShowOnPageLoad = true;
                       
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
                        //Part Lookup

                        //Bind Grid
                        PartLookupGrid.DataBind();

                        //Show Popup
                        AddPartPopup.ShowOnPageLoad = true;
                        break;
                    }
                case 3:
                    {
                        //Equipment Lookup

                        //Bind Grid
                        EquipLookupGrid.DataBind();

                        //Show Popup
                        AddEquipPopup.ShowOnPageLoad = true;
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
            crewMemberSelected = recordIdSelection.ToString();
            Session.Add("crewMemberSlected", recordIdSelection);
            
            //Check Count
            if (recordIdSelection.Count > 0)
            {
                //Check Permissions
                if (_userCanEdit)
                {

                    //Get Job Step ID
                    var jobStepId = -1;
                    if ((HttpContext.Current.Session["JobStepID"] != null))
                    {
                        //Get Info From Session
                        jobStepId = Convert.ToInt32(HttpContext.Current.Session["JobStepID"]);
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
            SaveSessionData();

            NextStepButton.Enabled = false;
            txtWorkDescription.Enabled = false;
            //ObjectIDCombo.Enabled = false;
            //txtObjectDescription.Enabled = false;
            //txtObjectArea.Enabled = false;
            //txtObjectLocation.Enabled = false;
            //txtObjectAssetNumber.Enabled = false;
            //ComboCompletedBy.Enabled = true;
            txtJobLength.Enabled = false;
            comboReason.Enabled = true;
            ComboOutcomeCode.Enabled = true;
            TxtWorkCompDate.Enabled = true;
            ComboPriority.Enabled = false;
            comboSubAssembly.Enabled = true;
            comboHwyRoute.Enabled = false;
            comboMilePostDir.Enabled = false;
            txtMilepost.Enabled = false;
            txtMilepostTo.Enabled = false;
            breakdownBox.Enabled = false;
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
            switch (TabPageControl.ActiveTabIndex)
            {
                
                case 0:
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
                case 3:
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
                default:
                    {
                        //Do Nothing
                        break;
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
            TabPageControl.TabPages[1].Text = @"MEMBERS (" + MemberGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void CrewGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            TabPageControl.TabPages[0].Text = @"CREW (" + CrewGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void PartGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            TabPageControl.TabPages[2].Text = @"PARTS (" + PartGrid.VisibleRowCount + @")";
        }

        /// <summary>
        /// Sets Tab Row Count
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void EquipGridBound(object sender, EventArgs e)
        {
            //Show Row Count On Tab 
            TabPageControl.TabPages[3].Text = @"EQUIPMENT (" + EquipGrid.VisibleRowCount + @")";
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


        protected void AddNewMemBnt_Click(object sender, EventArgs e)
        {
            AddItems();
        }

        protected void GetJobID()
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
            var requestor = _oLogon.UserID;
           
            //Get Object ID
            var objectAgainstId = -1;
            

            //Get Description
            var workDesc = "";
   
            //Get Work Date
            var startDate = DateTime.Now;
           

            //Get Priority
            var requestPriority = -1;
           

            //Get State Route
            var stateRouteId = -1;
            

            //Get Milepost
            decimal milepost = 0;
           

            //Get Milepost To
            decimal milepostTo = 0;
            

            //Get Milepost Direction
            var mpIncreasing = -1;
            

            //Get Work Op
            var workOp = -1;
           

            //Get Job Reason
            var reasonCode = -1;
           

            //Get Route To
            var routeTo = -1;
           

            //Get Cost Code
            var costCodeId = -1;
           

            //Get Fund Source
            var fundSource = -1;
           

            //Get Work Order
            var workOrder = -1;
           

            //Get Org Code
            var orgCode = -1;
           

            //Get Fund Group
            var fundingGroup = -1;
           

            //Get Equip Num
            var equipNumber = -1;
           

            //Get Ctl Section
            var controlSection = -1;
           

            //Get Notes
            var notes = "";
           

            //Get GPS X
            decimal gpsX = 0;
           

            //Get GPS Y
            decimal gpsY = 0;
            

            //Get GPS Z
            decimal gpsZ = 0;

            var success = false;
            

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
                startDate,
                requestPriority,
                requestor,
                _oJob.NullDate,
                gpsX, gpsY, gpsZ,
                -1,
                equipNumber,
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
                lblHeader.Text = "Job ID: " + (HttpContext.Current.Session["AssignedJobID"].ToString());

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
                        success = true;
                }
                    success = true;
            }
                success = true;
        }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
}

        protected void GetJobIDStepID()
        {


            #region Setting Vars to use in Stored Procedures from Session
            var jobAgainstArea = 0;
            const bool requestOnly = true;
            const int actionPriority = -1;
            const int mobileEquip = -1;
            const bool additionalDamage = false;
            const decimal percentOverage = 0;
            var routeTo = -1;
            var requestor = _oLogon.UserID;
            var gpsX = 0;
            var gpsY = 0;
            var gpsZ = 0;
            var jobStepConcurNumber = -1;
            var jobStepFollowStepNumber = -1;
            var jobStatus = -1;
            var jobLaborClass = -1;
            var jobGroup = -1;

            var jobShift = -1;
            var jobSupervisor = -1;
            var jobActualDt = 0;
            var jobEstimatedDt = 0;
            var jobEstimatedLen = 0;
            var jobRemainingDt = 0;
            var jobRemainingLen = 0;
            var jobReturnWithin = 0;
            var jobRouteTo = -1;
            var jobCompletedBy = requestor;
            JobType jobType = JobType.Corrective;
            //Title
            var newJobID = "";
            var errorFromJobIDGeneration = "";
            var poolTypeForJob = JobPoolType.Global;
            //Create ID
            var plannerdJobStepId = -1;
            //Create Class
            var oJobStep = new WorkOrderJobStep(_connectionString, _useWeb);

            if (breakdownBox.Checked == true)
            {
                jobType = JobType.Breakdown;
            }

            var success = false;

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
            #region Job info from Session and populate vars
            #region Get Description

            var workDesc = "";
            if (HttpContext.Current.Session["txtWorkDescription"] != null)
            {
                //Get Additional Info From Session
                workDesc = (HttpContext.Current.Session["txtWorkDescription"].ToString());
            }

            #endregion

            #region Get Object

            var objectAgainstId = -1;
            if (HttpContext.Current.Session["ObjectIDCombo"] != null)
            {
                //Get Info From Session
                objectAgainstId = Convert.ToInt32(HttpContext.Current.Session["ObjectIDCombo"].ToString());
            }

            #endregion

            #region Completed By
            var completedBy = "";
            if (HttpContext.Current.Session["ComboCompletedBy"] != null)
            {
                completedBy = HttpContext.Current.Session["ComboCompletedBy"].ToString();
            }
            #endregion

            #region Get Actual Job Length
            decimal jobActualLen = 0;
            if (Session["txtJobLength"] != null)
            {
                jobActualLen = Convert.ToDecimal(Session["txtJobLength"].ToString());
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

            #region Get Job Reason

            var reasonCode = -1;
            if ((HttpContext.Current.Session["comboReason"] != null))
            {
                //Get Info From Session
                reasonCode = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
            }

            #endregion

            #region Get Outcome

            var jobOutcome = -1;
            if ((HttpContext.Current.Session["ComboOutcome"] != null))
            {
                jobOutcome = Convert.ToInt32(HttpContext.Current.Session["ComboOutCome"].ToString());
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

            #region Get Element
            var elementID = -1;
            if (Session["comboElementID"] != null)
            {
                elementID = Convert.ToInt32(Session["comboElementID"].ToString());
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

            #region Sub Assembly
            var subAssemblyID = -1;
            if (Session["comboSubAssembly"] != null)
            {
                subAssemblyID = Convert.ToInt32(Session["comboSubAssembly"].ToString());
            }
            #endregion

            ///TODO Update Object
            #region Update Object

            #endregion




            #region Post Deafaults
            var postDefaults = false;
            if (chkPostDefaults.Checked == true)
            {
                postDefaults = true;
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

            #region Get Work Op

            var workOp = -1;
            if ((HttpContext.Current.Session["ComboWorkOp"] != null))
            {
                //Get Info From Session
                workOp = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
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

            #region Get Step #
            var jobStepNumber = 1;
            if (HttpContext.Current.Session["stepnumber"] != null)
            {
                jobStepNumber = Convert.ToInt32(HttpContext.Current.Session["stepnumber"].ToString());

            }


            #endregion

            #endregion
            var jobTitle = workDesc;
            #endregion
            //Clear Errors
            _oJob.ClearErrors();

            #region Update Job and Job cost codes
                    //Get Job From Session
                    
                   var jobId = Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString());

            try
            {
               

                //Add Job
                if (_oJob.Update(jobId,
                   workDesc,
                   jobType,
                   JobAgainstType.MaintenanceObjects,
                   objectAgainstId,
                   actionPriority,
                   reasonCode,
                   notes,
                   routeTo,
                   true,
                   startDate,
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
                    HttpContext.Current.Session.Add("AssignedJobID", AssignedJobID);

                    //Set Text
                    lblHeader.Text = "Job ID: " + (HttpContext.Current.Session["AssignedJobID"].ToString());



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
                        ////Return False To Prevent Navigation
                        //return false;
                    }



                    ////Return True
                    //return true;
                }

                ////Return False To Prevent Navigation
                //return false;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                ////Return False To Prevent Navigation
                //return false;
            }

            #endregion
            #region Generate Step ID

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




                //Add Default Step
                if (oJobStep.InsertDefaultJobStep(recordToPlan,
                jobType,
                jobTitle,
                notes,
                equipNumber,
                subAssemblyID,
                requestPriority,
                reasonCode,
                _oLogon.UserID,
                ref plannerdJobStepId))
                {
                    Session.Add("plannedJobStepId", plannerdJobStepId);
                    Session.Add("JobStepID", plannerdJobStepId);
                    Session.Add("editingJobStepID", plannerdJobStepId); 

                    ////Set Text
                    lblStep.Text = "Job Step ID: " + Session["JobStepID"].ToString();
                    #region Set Default Group, Supervisor, Labor & Shift

                    //Get User's Default Group And Group's Supervisor
                    try
                    {
                        var loaded = true;
                        var groupId = -1;
                        var supervisorId = -1;

                        //Check Requestor Field
                        var userId = _oLogon.UserID;

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

                }
                else
                {
                    //Throw Error
                    throw new SystemException(
                        @"Error Planning Job - " + oJobStep.LastError);
                }
                #endregion
                
            }
        }

        protected void NextStep_Click(object sender, EventArgs e)
        {
            SaveSessionData();
            GetJobIDStepID();

            Master.ShowPostButton = true;

            NextStepButton.Enabled = false;
            txtWorkDescription.Enabled = false;
            //ObjectIDCombo.Enabled = false;
            //txtObjectDescription.Enabled = false;
            //txtObjectArea.Enabled = false;
            //txtObjectLocation.Enabled = false;
            //txtObjectAssetNumber.Enabled = false;          
            txtJobLength.Enabled = false;
            comboHwyRoute.Enabled = false;
            comboMilePostDir.Enabled = false;
            txtMilepost.Enabled = false;
            txtMilepostTo.Enabled = false;
            breakdownBox.Enabled = false;
            TxtWorkCompDate.Enabled = true;
            ComboOutcomeCode.Enabled = true;
            

            comboElementID.Enabled = true;
            ComboPriority.Enabled = true;
            chkPostDefaults.Enabled = true;
            ComboCostCode.Enabled = true;
            ComboFundGroup.Enabled = true;
            ComboOrgCode.Enabled = true;
            ComboWorkOrder.Enabled = true;
            ComboWorkOp.Enabled = true;
            AttachmentGrid.Enabled = true;
            AttachmentGrid.Visible = true;
            PhotoContainer.Visible = true;
            UpdatePanel1.Visible = true;
            TabPageControl.Visible = true;
            TabPageContainer.Visible = true;
            CrewGrid.Enabled = true;
            CrewGrid.Visible = true;
            MemberGrid.Enabled = true;
            MemberGrid.Visible = true;
            PartGrid.Enabled = true;
            PartGrid.Visible = true;
            EquipGrid.Enabled = true;
            EquipGrid.Visible = true;

        }
    }
}

