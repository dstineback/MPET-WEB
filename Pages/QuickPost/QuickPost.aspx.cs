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

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.RegisterBaseScript(Page);

            #region Attempt to Load Logon Info
            //Check for Logon Class
            if(Session["LogonInfo"] != null)
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
                SetupForAdding();
                txtWorkDescription.Focus();

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
                        case "SaveButton":
                            {                                  
                                        SaveSessionData();
                                        AddRequest();
                                        PlanJobRoutine();
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
            #region Setup Fields on Page Load
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

            #endregion
            #region Is not a Post-Back
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

            #endregion
            #region Button Status
            Master.ShowNewButton = true;
            Master.ShowPostButton = true;
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
            TxtWorkStartDate.Value = DateTime.Now;
           

        }

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
            Master.ShowSaveButton = (_userCanAdd || _userCanEdit);
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
        #endregion

        #region Add/Post Job
        #region Save Session
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

          

            if (HttpContext.Current.Session["ComboOutcome"] != null)
            {
                HttpContext.Current.Session.Add("ComboOutcome", ComboOutcomeCode.Value);
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

            
        }
        #endregion
        #region Add Work request
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

                    ////Set Text
                    //lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                    ////Set Text
                    //lblStep.Text = @"STEP #1";

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
        #endregion
        #region Add Planned Job
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

    }

}