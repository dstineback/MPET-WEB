using System;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using DevExpress.Web;
using MPETDSFactory;

namespace Pages.ServiceRequests
{
    /// <summary>
    /// Service Request Page
    /// </summary>
    public partial class ServiceRequests : Page
    {
        #region Page Variables

        #region Bools

        /// <summary>
        /// Use Web Flag
        /// </summary>
        private bool _useWeb;

        #endregion

        #region Classes

        /// <summary>
        /// Work Order Job Class
        /// </summary>
        private WorkOrder _oJob;

        /// <summary>
        /// Logon Object Class
        /// </summary>
        private LogonObject _oLogon;

        /// <summary>
        /// Job ID Generator Class
        /// </summary>
        private JobIdGenerator _oJobIdGenerator;

        /// <summary>
        /// Job Request Info Class
        /// </summary>
        private JobRequestInfo _oJobRequestInfo;

        #endregion

        #region Dates

        /// <summary>
        /// Global Null Date
        /// </summary>
        private readonly DateTime _nullDate = Convert.ToDateTime("1/1/1960 23:59:59");

        #endregion

        #region Email Settings

        /// <summary>
        /// Returns Service Request Email To From Web Config
        /// </summary>
        private static string EmailTo
        {
            get
            {
                //Get Service Request Email To
                return WebConfigurationManager.AppSettings["ServiceRequestEmailTo"];
            }
        }

        /// <summary>
        /// Returns Service Request EmailFrom From Web Config
        /// </summary>
        private static string EmailFrom
        {
            get
            {
                //Get Service Request Email From
                return WebConfigurationManager.AppSettings["EmailFrom"];
            }
        }

        /// <summary>
        /// Returns Email Host From Web Config
        /// </summary>
        private static string EmailHost
        {
            get
            {
                //Get Service Request Email Host
                return WebConfigurationManager.AppSettings["EmailHost"];
            }
        }

        /// <summary>
        /// Returns Email Port From Web Config
        /// </summary>
        private static string EmailPort
        {
            get
            {
                //Get Service Request Email Port
                return WebConfigurationManager.AppSettings["EmailPort"];
            }
        }

        /// <summary>
        /// Returns Email User To Logon With From Web Config
        /// </summary>
        private static string EmailUser
        {
            get
            {
                //Get Service Request Email Logon
                return WebConfigurationManager.AppSettings["EmailUsername"];
            }
        }

        /// <summary>
        /// Returns Email Password From Web Config To Logon With
        /// </summary>
        private static string EmailPassword
        {
            get
            {
                //Get Service Request Email Password
                return WebConfigurationManager.AppSettings["EmailPassword"];
            }
        }

        /// <summary>
        /// Returns SSL Flag To Use To Logon With From Web Config
        /// </summary>
        private static string EmailSsl
        {
            get
            {
                //Get Service Request Email SSL Flag
                return WebConfigurationManager.AppSettings["EmailSSL"];
            }
        }

        #endregion

        #region Strings

        /// <summary>
        /// Assigned Job GUID
        /// </summary>
        public string AssignedGuid = "";

        /// <summary>
        /// Assigned Job ID
        /// </summary>
        public string AssignedJobId = "";

        /// <summary>
        /// Connection String
        /// </summary>
        private string _connectionString = "";

        #endregion

        #endregion

        #region Page Events

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Load Page
            try
            {
                //Check For Logon Class
                if (HttpContext.Current.Session["LogonInfo"] != null)
                {
                    //Get Logon Info From Session
                    _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
                }

                //Set Flag
                Master.ShowMenu = true;



                //Check For Post To Setup Form
                if (!IsPostBack)
                {
                    //Setup For Adding
                    SetupForAdding();
                }
                else
                {
                    var manager = ScriptManager.GetCurrent(Page);
                    if (manager != null && manager.IsInAsyncPostBack)
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
                            case "SaveButton":
                                {
                                    //Check For Job ID
                                    if (HttpContext.Current.Session["editingJobID"] != null)
                                    {
                                        //Save Session Data
                                        SaveSessionData();

                                        //Update Job
                                        UpdateRequest();
                                    }
                                    else
                                    {
                                        //Save Session Data
                                        SaveSessionData();

                                        //Update Job
                                        AddRequest();
                                    }

                                    //Break
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

                //Check For Query String
                if (!String.IsNullOrEmpty(Request.QueryString["jobid"]))
                {
                    //Check For Editing Job ID
                    if (HttpContext.Current.Session["editingJobID"] == null)
                    {
                        //Setup For Editing
                        SetupForEditing();

                        //Creat GUID VARIABLE
                        var jobGuid = "";

                        _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                        //Load Job Info & Populate Session Variables
                        if (_oJob.GetJobGuidFromJobID(-1, Request.QueryString["jobid"], ref jobGuid))
                        {
                            var userId = -1;

                            if (_oLogon != null)
                            {
                                userId = _oLogon.UserID;
                            }

                            //Load Job Info From GUID
                            if (_oJob.LoadDataByGuid(jobGuid, userId))
                            {
                                //Exepcted Schema
                                //tbl_Jobs.n_Jobid AS 'n_Jobid',
                                //tbl_Jobs.Jobid AS 'Jobid',
                                //tbl_Jobs.Title AS 'Title',
                                //tbl_Jobs.TypeOfJob AS 'TypeOfJob',
                                //tbl_Jobs.AssignedGUID AS 'AssignedGUID',
                                //tbl_Jobs.JobAgainstID AS 'JobAgainstID',
                                //tbl_Jobs.n_MaintObjectID AS 'n_MaintObjectID',
                                //tbl_Jobs.n_AreaObjectID AS 'n_AreaObjectID',
                                //tbl_Jobs.n_LocationObjectID AS 'n_LocationObjectID',
                                //tbl_Jobs.n_GPSObjectID AS 'n_GPSObjectID',
                                //tbl_Jobs.n_ActionPriority AS 'n_ActionPriority',
                                //tbl_Jobs.n_jobreasonid AS 'nJobReasonID' ,
                                //tbl_Jobs.Notes AS 'Notes',
                                //tbl_Jobs.EstimatedJobHours AS 'EstimatedJobHours',
                                //tbl_Jobs.ActualJobHours AS 'ActualJobHours',
                                //tbl_Jobs.IsRequestOnly AS 'IsRequestOnly',
                                //tbl_Jobs.n_RouteTo AS 'n_RouteTo',
                                //tbl_Jobs.RequestDate AS 'RequestDate',
                                //tbl_Jobs.n_requestPriority AS 'n_priorityid' ,
                                //tbl_Jobs.n_requestor AS 'UserID' ,
                                //tbl_Jobs.pmcalc_startdate AS 'pmcalc_startdate',
                                //tbl_Jobs.n_MobileOwner AS 'n_MobileOwner',
                                //tbl_Jobs.MobileDate AS 'MobileDate',
                                //tbl_Jobs.IssuedDate AS 'IssuedDate',
                                //tbl_Jobs.GPS_X AS 'GPS_X',
                                //tbl_Jobs.GPS_Y AS 'GPS_Y',
                                //tbl_Jobs.GPS_Z AS 'GPS_Z',
                                //tbl_Jobs.n_group AS 'n_group',
                                //tbl_Jobs.SentToPDA AS 'SentToPDA',
                                //tbl_Jobs.JobOpen AS 'JobOpen',
                                //tbl_Jobs.IsHistory AS 'IsHistory',
                                //tbl_Jobs.ServicingEquipment AS 'ServicingEquipment',
                                //tbl_Jobs.completed_units1 AS 'completed_units1',
                                //tbl_Jobs.completed_units2 AS 'completed_units2',
                                //tbl_Jobs.completed_units3 AS 'completed_units3',
                                //tbl_Jobs.n_OutcomeID AS 'n_OutcomeID',
                                //tbl_Jobs.n_OwnerID AS 'n_OwnerID',
                                //tbl_Jobs.n_TaskID AS 'n_TaskID',
                                //tbl_Jobs.n_workeventid AS 'n_workeventid',
                                //tbl_Jobs.n_worktypeid AS 'n_WorkOpID' ,
                                //tbl_Jobs.PostedDate AS 'PostedDate',
                                //tbl_Jobs.SubAssemblyID AS 'SubAssemblyID',
                                //tbl_Jobs.n_StateRouteID AS 'n_StateRouteID' ,
                                //tbl_Jobs.Milepost AS 'Milepost' ,
                                //tbl_Jobs.IncreasingMP AS 'n_MilePostDirectionID' ,
                                //tbl_Jobs.b_AdditionalDamage AS 'b_AdditionalDamage',
                                //tbl_Jobs.PercentOverage AS 'PercentOverage',
                                //ISNULL(tbl_IsFlaggedRecord.RecordID, -1) AS 'FlaggedRecordID' ,
                                //tbl_Requestor.Username AS 'Username' ,
                                //tbl_Priorities.priorityid AS 'priorityid' ,
                                //tbl_JobReasons.JobReasonID AS 'JobReasonID' ,
                                //tbl_Owner.Username AS 'OwnerID' ,
                                //tbl_StateRoutes.StateRouteID AS 'StateRouteID' ,
                                //tbl_MPDirections.MilePostDirectionID AS 'MilePostDirectionID' ,
                                //tbl_WorkOP.WorkOpID AS 'WorkOpID' ,
                                //tbl_Jobs.n_CostCodeID AS 'n_CostCodeID' ,
                                //tbl_CostCodes.costcodeid AS 'costcodeid' ,
                                //tbl_CostCodes.SupplementalCode as 'SupplementalCode' ,
                                //tbl_Jobs.n_FundSrcCodeID AS 'n_FundSrcCodeID' ,
                                //tbl_FSC.FundSrcCodeID AS 'FundSrcCodeID' ,
                                //tbl_Jobs.n_WorkOrderCodeID AS 'n_WorkOrderCodeID' ,
                                //tbl_WOCodes.WorkOrderCodeID AS 'WorkOrderCodeID' ,
                                //tbl_Jobs.n_OrganizationCodeID AS 'n_OrganizationCodeID' ,
                                //tbl_OrgCodes.OrganizationCodeID AS 'OrganizationCodeID' ,
                                //tbl_Jobs.n_FundingGroupCodeID AS 'n_FundingGroupCodeID' ,
                                //tbl_FundGroup.FundingGroupCodeID AS 'FundingGroupID' ,
                                //tbl_Jobs.n_ControlSectionID AS 'n_ControlSectionID' ,
                                //tbl_CtlSections.ControlSectionID AS 'ControlSectionID' ,
                                //tbl_Jobs.n_EquipmentNumberID AS 'n_EquipmentNumberID' ,
                                //tbl_EquipNum.EquipmentNumberID AS 'EquipmentNumberID'
                                //tbl_MO.objectid AS 'ObjectID',
                                //tbl_MO.description AS 'ObjectDesc',
                                //tbl_MO.assetnumber AS 'ObjectAsset',
                                //tbl_MO.locationid AS 'ObjectLoc',
                                //tbl_MO.areaid AS 'ObjectArea'

                                #region Setup Job Data

                                //Add Job ID Class
                                HttpContext.Current.Session.Add("oJob", _oJob);

                                //Add Editing Job ID
                                HttpContext.Current.Session.Add("editingJobID",
                                    ((int)_oJob.Ds.Tables[0].Rows[0]["n_Jobid"]));

                                //Add Job String ID
                                HttpContext.Current.Session.Add("AssignedJobID", _oJob.Ds.Tables[0].Rows[0]["Jobid"]);

                                //Add Description
                                HttpContext.Current.Session.Add("txtWorkDescription",
                                    _oJob.Ds.Tables[0].Rows[0]["Title"]);

                                //Add Request Date
                                HttpContext.Current.Session.Add("TxtWorkRequestDate",
                                    _oJob.Ds.Tables[0].Rows[0]["RequestDate"]);

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

                                #region Seetup Requestor

                                HttpContext.Current.Session.Add("ComboRequestor", _oJob.Ds.Tables[0].Rows[0]["UserID"]);
                                HttpContext.Current.Session.Add("ComboRequestorText",
                                    _oJob.Ds.Tables[0].Rows[0]["Username"]);

                                #endregion

                                #region Setup Priority

                                HttpContext.Current.Session.Add("ComboPriority",
                                    _oJob.Ds.Tables[0].Rows[0]["n_priorityid"]);
                                HttpContext.Current.Session.Add("ComboPriorityText",
                                    _oJob.Ds.Tables[0].Rows[0]["priorityid"]);

                                #endregion

                                #region Setup Reason

                                HttpContext.Current.Session.Add("comboReason",
                                    _oJob.Ds.Tables[0].Rows[0]["nJobReasonID"]);
                                HttpContext.Current.Session.Add("comboReasonText",
                                    _oJob.Ds.Tables[0].Rows[0]["JobReasonID"]);

                                #endregion

                                #region Setup Route To

                                HttpContext.Current.Session.Add("comboRouteTo", _oJob.Ds.Tables[0].Rows[0]["n_RouteTo"]);
                                HttpContext.Current.Session.Add("comboRouteToText",
                                    _oJob.Ds.Tables[0].Rows[0]["OwnerID"]);

                                #endregion

                                #region Setup Hwy Route

                                HttpContext.Current.Session.Add("comboHwyRoute",
                                    _oJob.Ds.Tables[0].Rows[0]["n_StateRouteID"]);
                                HttpContext.Current.Session.Add("comboHwyRouteText",
                                    _oJob.Ds.Tables[0].Rows[0]["StateRouteID"]);

                                #endregion

                                #region Setup Milepost

                                HttpContext.Current.Session.Add("txtMilepost", _oJob.Ds.Tables[0].Rows[0]["Milepost"]);
                                HttpContext.Current.Session.Add("txtMilepostTo",
                                    _oJob.Ds.Tables[0].Rows[0]["MilepostTo"]);
                                HttpContext.Current.Session.Add("comboMilePostDir",
                                    _oJob.Ds.Tables[0].Rows[0]["n_MilePostDirectionID"]);
                                HttpContext.Current.Session.Add("comboMilePostDirText",
                                    _oJob.Ds.Tables[0].Rows[0]["MilePostDirectionID"]);

                                #endregion

                                #region Setup Cost Code

                                HttpContext.Current.Session.Add("ComboCostCode",
                                    _oJob.Ds.Tables[0].Rows[0]["n_CostCodeID"]);
                                HttpContext.Current.Session.Add("ComboCostCodeText",
                                    _oJob.Ds.Tables[0].Rows[0]["costcodeid"]);

                                #endregion

                                #region Setup Fund Source

                                HttpContext.Current.Session.Add("ComboFundSource",
                                    _oJob.Ds.Tables[0].Rows[0]["n_FundSrcCodeID"]);
                                HttpContext.Current.Session.Add("ComboFundSourceText",
                                    _oJob.Ds.Tables[0].Rows[0]["FundSrcCodeID"]);

                                #endregion

                                #region Setup Work Order

                                HttpContext.Current.Session.Add("ComboWorkOrder",
                                    _oJob.Ds.Tables[0].Rows[0]["n_WorkOrderCodeID"]);
                                HttpContext.Current.Session.Add("ComboWorkOrderText",
                                    _oJob.Ds.Tables[0].Rows[0]["WorkOrderCodeID"]);

                                #endregion

                                #region Setup Work Op

                                HttpContext.Current.Session.Add("ComboWorkOp", _oJob.Ds.Tables[0].Rows[0]["n_WorkOpID"]);
                                HttpContext.Current.Session.Add("ComboWorkOpText",
                                    _oJob.Ds.Tables[0].Rows[0]["WorkOpID"]);

                                #endregion

                                #region Setup Org Code

                                HttpContext.Current.Session.Add("ComboOrgCode",
                                    _oJob.Ds.Tables[0].Rows[0]["n_OrganizationCodeID"]);
                                HttpContext.Current.Session.Add("ComboOrgCodeText",
                                    _oJob.Ds.Tables[0].Rows[0]["OrganizationCodeID"]);

                                #endregion

                                #region Setup Fund Group

                                HttpContext.Current.Session.Add("ComboFundGroup",
                                    _oJob.Ds.Tables[0].Rows[0]["n_FundingGroupCodeID"]);
                                HttpContext.Current.Session.Add("ComboFundGroupText",
                                    _oJob.Ds.Tables[0].Rows[0]["FundingGroupCodeID"]);

                                #endregion

                                #region Setup Control Section

                                HttpContext.Current.Session.Add("ComboCtlSection",
                                    _oJob.Ds.Tables[0].Rows[0]["n_ControlSectionID"]);
                                HttpContext.Current.Session.Add("ComboCtlSectionText",
                                    _oJob.Ds.Tables[0].Rows[0]["ControlSectionID"]);

                                #endregion

                                #region Setup Equip Num

                                HttpContext.Current.Session.Add("ComboEquipNum",
                                    _oJob.Ds.Tables[0].Rows[0]["n_EquipmentNumberID"]);
                                HttpContext.Current.Session.Add("ComboEquipNumText",
                                    _oJob.Ds.Tables[0].Rows[0]["EquipmentNumberID"]);

                                #endregion

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtFN"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtFN");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtLN"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtLN");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtEmail"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtEmail");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtPhone"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtPhone");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtExt"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtExt");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtMail"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtMail");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtBuilding"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtBuilding");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["txtRoomNum"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("txtRoomNum");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["ComboServiceOffice"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("ComboServiceOffice");
                                //}

                                ////Check For Prior Value
                                //if (HttpContext.Current.Session["ComboServiceOfficeText"] != null)
                                //{
                                //    //Remove Old One
                                //    HttpContext.Current.Session.Remove("ComboServiceOfficeText");
                                //}

                                #region Setup Location

                                //Check X
                                if (Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["GPS_X"]) != 0)
                                {
                                    HttpContext.Current.Session.Add("GPSX", _oJob.Ds.Tables[0].Rows[0]["GPS_X"]);
                                }

                                //Check Y
                                if (Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["GPS_Y"]) != 0)
                                {
                                    HttpContext.Current.Session.Add("GPSY", _oJob.Ds.Tables[0].Rows[0]["GPS_Y"]);
                                }

                                //Check Z
                                if (Convert.ToInt32(_oJob.Ds.Tables[0].Rows[0]["GPS_Z"]) != 0)
                                {
                                    HttpContext.Current.Session.Add("GPSZ", _oJob.Ds.Tables[0].Rows[0]["GPS_Z"]);
                                }


                                #endregion

                                #region Setup Additional Info

                                HttpContext.Current.Session.Add("txtAddDetail", _oJob.Ds.Tables[0].Rows[0]["Notes"]);

                                #endregion
                            }
                        }
                    }
                }

                if (!IsPostBack)
                {
                    //Check For Previous Session Variables
                    if (HttpContext.Current.Session["txtWorkDescription"] != null)
                    {
                        //Get Additional Info From Session
                        txtWorkDescription.Text = (HttpContext.Current.Session["txtWorkDescription"].ToString());
                    }

                    //Job ID
                    if (HttpContext.Current.Session["AssignedJobID"] != null)
                    {
                        //Get Additional Info From Session
                        lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());
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
                    TxtWorkRequestDate.Value = HttpContext.Current.Session["TxtWorkRequestDate"] != null
                        ? Convert.ToDateTime((HttpContext.Current.Session["TxtWorkRequestDate"].ToString()))
                        : DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Initializes Classes & Page Variables
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //Initialize Classes & Page Variables
            try
            {
                //Set Connection Info
                _connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
                _useWeb = (ConfigurationManager.AppSettings["UsingWebService"] == "Y");

                //Initialize Classes
                _oJob = new WorkOrder(_connectionString, _useWeb);
                _oJobRequestInfo = new JobRequestInfo(_connectionString, _useWeb);

                //Set Datasources
                HwyRouteSqlDatasource.ConnectionString = _connectionString;
                MilePostDirSqlDatasource.ConnectionString = _connectionString;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Register And Updates Panels
        /// </summary>
        /// <param name="panel">Panel To Use</param>
        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            //Get Script Manager
            var sType = typeof(ScriptManager);

            //Get Method
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);

            //Check For Null
            if (mInfo != null)
            {
                //Invoke Current Page/Panel
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
            }
        }

        #endregion

        #region Form Permission Events

        /// <summary>
        /// Enables Form Options For Adding
        /// </summary>
        private void SetupForAdding()
        {
            //Enable Form Options For Adding
            try
            {
                //Setup Buttons
                Master.ShowSaveButton = true;
                Master.ShowNewButton = false;
                Master.ShowSubmitTExt = true;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Enables Form Options For Editing
        /// </summary>
        private void SetupForEditing()
        {
            //Enable Form Options For Editing
            try
            {
                //Setup Buttons
                Master.ShowSaveButton = true;
                Master.ShowNewButton = true;
                //Master.ShowEmailButton = true;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        #endregion

        #region Combo Loading Events

        /// <summary>
        /// Get Highway Routes By Filter
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void comboHwyRoute_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //Get Highway Routes By Filter
            try
            {
                //Get Combo
                var comboBox = (ASPxComboBox)source;

                //Set Command
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

                //Clear Parameters
                HwyRouteSqlDatasource.SelectParameters.Clear();

                //Add Filter Parameter
                HwyRouteSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));

                //Add Start Index Parameter
                HwyRouteSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));

                //Add End Index Parameter
                HwyRouteSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));

                //Get Data
                comboBox.DataSource = HwyRouteSqlDatasource;

                //Bind Data
                comboBox.DataBind();
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Get Highway Route By SQL ID
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void comboHwyRoute_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            //Get Highway Route By SQL ID
            try
            {
                //Create Variable
                long value;

                //Check Current Value
                if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                {
                    //Return
                    return;
                }

                //Get Combo
                var comboBox = (ASPxComboBox)source;

                //Set Select
                HwyRouteSqlDatasource.SelectCommand = @"SELECT  tblStateRoutes.n_StateRouteID ,
                                                            tblStateRoutes.StateRouteID ,
                                                            tblStateRoutes.[Description],
                                                            ROW_NUMBER() OVER ( ORDER BY tblStateRoutes.[n_StateRouteID] ) AS [rn]
                                                    FROM    dbo.StateRoutes AS tblStateRoutes
                                                    WHERE   ( n_StateRouteID = @ID )
                                                    ORDER BY StateRouteID";

                //Clear Parameters
                HwyRouteSqlDatasource.SelectParameters.Clear();

                //Set Parrameters
                HwyRouteSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());

                //Get Data
                comboBox.DataSource = HwyRouteSqlDatasource;

                //Bind Data
                comboBox.DataBind();
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Get Mile Post Directions Based On Filter
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void comboMilePostDir_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //Get Mile Post Directions Based On Filter
            try
            {
                //Get Combo
                var comboBox = (ASPxComboBox)source;

                //Set Select Command
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

                //Clear Parameters
                MilePostDirSqlDatasource.SelectParameters.Clear();

                //Add Filter
                MilePostDirSqlDatasource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));

                //Add Start Index
                MilePostDirSqlDatasource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));

                //Add End Index
                MilePostDirSqlDatasource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));

                //Get Data
                comboBox.DataSource = MilePostDirSqlDatasource;

                //Bind Data
                comboBox.DataBind();
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Get Mile Post Direction By Sql Value
        /// </summary>
        /// <param name="source">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void comboMilePostDir_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            //Get Mile Post Direction By SQL Value
            try
            {
                //Create Variable
                long value;

                //Check Value
                if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                {
                    //Return 
                    return;
                }

                //Get Combo Box
                var comboBox = (ASPxComboBox)source;

                //Set Select Command
                MilePostDirSqlDatasource.SelectCommand = @"SELECT  tblMilePostDir.n_MilePostDirectionID ,
                                                            tblMilePostDir.MilePostDirectionID ,
                                                            tblMilePostDir.[Description] ,
                                                            ROW_NUMBER() OVER ( ORDER BY tblMilePostDir.[n_MilePostDirectionID] ) AS [rn]
                                                    FROM    dbo.MilePostDirections AS tblMilePostDir
                                                    WHERE   ( n_MilePostDirectionID = @ID )
                                                    ORDER BY MilePostDirectionID";

                //Clear Parameters
                MilePostDirSqlDatasource.SelectParameters.Clear();

                //Add Parameters
                MilePostDirSqlDatasource.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());

                //Get Data
                comboBox.DataSource = MilePostDirSqlDatasource;

                //Bind Data
                comboBox.DataBind();
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        #endregion

        #region Add/Update Events

        /// <summary>
        /// Updates Work Reqeust
        /// </summary>
        /// <returns>True/False For Success</returns>
        protected bool UpdateRequest()
        {
            const int actionPriority = -1;
            const int mobileEquip = -1;
            const int subAssemblyId = -1;
            const bool additionalDamage = false;
            const decimal percentOverage = 0;

            //Get Logon Info
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Logon Info From Session
                _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);
            }

            var objectAgainstId = -1;
            if (HttpContext.Current.Session["ObjectIDCombo"] != null)
            {
                //Get Info From Session
                objectAgainstId = Convert.ToInt32((HttpContext.Current.Session["ObjectIDCombo"].ToString()));
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
                gpsY = Convert.ToDecimal((HttpContext.Current.Session["GPSY"].ToString()));
            }

            //Get GPS Z
            decimal gpsZ = 0;
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Get Info From Session
                gpsZ = Convert.ToDecimal((HttpContext.Current.Session["GPSZ"].ToString()));
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

            //Clear Errors
            _oJob.ClearErrors();

            try
            {

                if (_oJob.Update(Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()),
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
                    0,
                    0,
                    0,
                    -1,
                    mobileEquip,
                    _oJob.NullDate,
                    routeTo,
                    -1,
                    -1,
                    workOp,
                    -1,
                    subAssemblyId,
                    stateRouteId,
                    milepost,
                    milepostTo,
                    mpIncreasing,
                    additionalDamage,
                    percentOverage,
                    requestor,
                    ref AssignedJobId))
                {
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
                            requestor))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Updating Service Request - " +
                            _oJob.LastError);
                    }

                    //Check For Value
                    if (HttpContext.Current.Session["editingJobID"] != null)
                    {
                        //Get Additional Info From Session
                        lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                        //Setup For Editing
                        SetupForEditing();
                    }

                    //Success Return True
                    return true;
                }

                //Throw Error
                throw new SystemException(
                    @"Error Updating Service Request - " +
                    _oJob.LastError);
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
            const int actionPriority = -1;
            const int mobileEquip = -1;
            const bool additionalDamage = false;
            const decimal percentOverage = 0;
            const int subAssemblyId = -1;
            var errorFromJobIdGeneration = "";
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
                gpsY = Convert.ToDecimal((HttpContext.Current.Session["GPSY"].ToString()));
            }

            //Get GPS Z
            decimal gpsZ = 0;
            if (HttpContext.Current.Session["GPSZ"] != null)
            {
                //Get Info From Session
                gpsZ = Convert.ToDecimal((HttpContext.Current.Session["GPSZ"].ToString()));
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
                    _oJobIdGenerator =
                        new JobIdGenerator(_connectionString,
                            _useWeb, _oLogon.UserID, _oLogon.AreaID);
                }
                else
                {
                    //Setup For Non Logged In User
                    _oJobIdGenerator =
                        new JobIdGenerator(_connectionString,
                            _useWeb, -1, -1);
                }

                //Clear Errors
                _oJobIdGenerator.ClearErrors();

                //Get Pool
                if (!_oJobIdGenerator.GetJobPoolInUse(ref poolTypeForJob))
                {
                    //Throw Error
                    throw new SystemException(
                        @"Error Adding Service Request - " +
                        _oJobIdGenerator.LastError);
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
                    subAssemblyId,
                    stateRouteId,
                    milepost,
                    milepostTo,
                    mpIncreasing,
                    additionalDamage,
                    percentOverage,
                    ref AssignedJobId,
                    ref errorFromJobIdGeneration,
                    ref AssignedGuid,
                    requestor))
                {
                    //Add Job To Session
                    HttpContext.Current.Session.Add("oJob", _oJob);
                    HttpContext.Current.Session.Add("editingJobID", _oJob.RecordID);
                    HttpContext.Current.Session.Add("AssignedJobID", AssignedJobId);

                    //Set Text
                    lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                    //Add Job Requestor Info
                    if (!_oJobRequestInfo.Update(AssignedGuid,
                        txtFN.Text.Trim(),
                        txtLN.Text.Trim(),
                        "",
                        txtPhone.Text.Trim(),
                        "",
                        "",
                        txtExt.Text.Trim(),
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        _nullDate,
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        requestor))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Adding Service Request - " +
                            _oJob.LastError);
                    }

                    //Update Origin
                    if (!_oJob.UpdateOriginType(_oJob.RecordID,
                        "W",
                        requestor))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Adding Service Request - " +
                            _oJob.LastError);
                    }

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
                        requestor))
                    {
                        //Throw Error
                        throw new SystemException(
                            @"Error Adding Service Request - " +
                            _oJob.LastError);
                    }

                    //Setup For Editing
                    SetupForEditing();

                    //Email Receipt
                    EmailReceipt();

                    //Return True
                    return true;
                }

                //Throw Error
                throw new SystemException(
                    @"Error Adding Service Request - " +
                    _oJob.LastError);
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                //Prevent Further Navigation
                return false;
            }
        }

        /// <summary>
        /// Clears Session & Reloads Page For New Request
        /// </summary>
        private void AddNewRow()
        {
            //Clear Session & Reload Page For New Request
            try
            {
                //Clear Session Variables
                ResetSession();

                //Redirect To Edit Page With Job ID
                Response.Redirect("~/Pages/ServiceRequests/ServiceRequests.aspx", true);
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        #endregion

        #region Email Events

        /// <summary>
        /// Generates & Returns Subject For Email Receipts
        /// </summary>
        /// <returns>Generated Email Subject String</returns>
        protected string EmailSubject()
        {
            //Generate Subject For Email Receipt
            try
            {
                //Create Subject Variable

                //Set Subject
                var emailSubject = @"Receipt For Service Request " + (HttpContext.Current.Session["AssignedJobID"]);

                return emailSubject;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                //Return Blank
                return "";
            }
        }

        /// <summary>
        /// Generates Email Body For Email Receipts
        /// </summary>
        /// <returns>Email Body For Email Receipt</returns>
        protected string EmailBody()
        {
            //Generate Body For Email Receipt
            try
            {
                var emailBody = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">"
                                + @"<html xmlns=""http://www.w3.org/1999/xhtml"">"
                                + @"<head>"
                                + @"<title>New Service Request Entered</title>"
                                + @"</head>"
                                + @"<style type=""text/css"">"
                                + @"body {background-color:ffffff;background-image:url(http://);background-repeat:no-repeat;background-position:top left;background-attachment:fixed;}
						            h2{text-align:left;font-family:Georgia;color:000000;}
						            p {font-family:Georgia;font-size:14px;font-style:normal;font-weight:normal;color:000000;}
						            .style1 {width: 200px;}
						            .style2 {color: #808080;}
					            </style>
					            <body>"
                                + @"<strong>A New Service Request Has Been Entered.</strong>"
                                + @"<br />"
                                + @"<br />"
                                +
                                @"<table style=""border-style: solid; border-color: #FFFFFF; width: 1000px;"" bgcolor=#F4F7FC border-bottom:1px solid white;>"
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                +
                                @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Request ID:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["AssignedJobID"]
                                + @"</td></tr>"
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Request Date:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["TxtWorkRequestDate"]
                                + @"</td></tr> "
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Road Name:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["comboHwyRouteText"]
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>First Name:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["txtFN"]
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Last Name:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["txtLN"]
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Email:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["txtEmail"]
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Phone Number:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["txtPhone"]
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"" class=""style1"">"
                                + @"<strong>Ext:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none solid none;"">"
                                + HttpContext.Current.Session["txtExt"]
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none none none;"" class=""style1"">"
                                + @"<strong>Requested Action:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none none none;"">"
                                + HttpContext.Current.Session["txtWorkDescription"]
                                + @"</td></tr>"
                                + @"<tr style=""border-bottom:1px solid white;"">"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none none none;"" class=""style1"">"
                                + @"<strong>Additional Info:</strong></td>"
                                + @"<td style=""border-color: #FFFFFF; border-style: none none none none;"">"
                                + HttpContext.Current.Session["txtAddDetailForEmail"]
                                + @"</td></tr>"
                                + @"</table><br/>"
                                +
                                @"<strong>***Do Not Reply To This Email. This Is An Automatically Generated Email.***</strong>"
                                + @"</body></html>";

                //Return Email Body
                return emailBody;
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);

                //Return Blank
                return "";
            }
        }

        /// <summary>
        /// Emails Service Request Receipt
        /// Receipients Found In Web Config
        /// Credentials Found In Web Config
        /// </summary>
        protected void EmailReceipt()
        {
            //Email Service Reqeust
            try
            {
                //Check For Null/Empty Email To
                if (!string.IsNullOrEmpty(EmailTo))
                {
                    //Check For Null Email From
                    if (!string.IsNullOrEmpty(EmailFrom))
                    {
                        //Check For Null/Empty Host
                        if (!string.IsNullOrEmpty(EmailHost))
                        {
                            //Check For Null/Empty Port
                            if (!string.IsNullOrEmpty(EmailPort))
                            {
                                //Check For Null/Empty Email User
                                if (!string.IsNullOrEmpty(EmailUser))
                                {
                                    //Check For Null/Empty Password
                                    if (!string.IsNullOrEmpty(EmailPassword))
                                    {
                                        //Check For Null/Empty SSL
                                        if (!string.IsNullOrEmpty(EmailPassword))
                                        {
                                            //Create New Mail Message
                                            var mail = new MailMessage
                                            {
                                                //Add From
                                                From = new MailAddress(EmailFrom, EmailFrom)
                                            };

                                            //Check To See If Requester Email Exists
                                            if (HttpContext.Current.Session["txtEmail"] != null)
                                            {
                                                //Add Them To BCC List
                                                mail.Bcc.Add(EmailTo + "," + HttpContext.Current.Session["txtEmail"]);
                                            }
                                            else
                                            {
                                                //Set Normal BCC Receiptients 
                                                mail.Bcc.Add(EmailTo);
                                            }

                                            //Set Mail Subject
                                            mail.Subject = EmailSubject();

                                            //Set Mail Body
                                            mail.Body = EmailBody();

                                            //Set HTML Mail Type
                                            mail.IsBodyHtml = true;

                                            //Set Priority
                                            mail.Priority = MailPriority.Normal;

                                            //Create New SMTP Client
                                            var smtp = new SmtpClient
                                            {
                                                //Setup SMTP CLient
                                                Host = EmailHost,
                                                Port = Convert.ToInt32(EmailPort),
                                                Credentials = new System.Net.NetworkCredential(EmailUser, EmailPassword),
                                                EnableSsl = (EmailSsl.ToUpper() == "TRUE")
                                            };

                                            //Send Mail
                                            smtp.Send(mail);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SmtpException ex)
            {
                //Throw Error
                throw new SystemException(
                    @"SMTP Exception Error - " + ex);
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }

        }

        #endregion

        #region Session Events

        /// <summary>
        /// Resets Session Variables
        /// </summary>
        protected void ResetSession()
        {
            //Reset Session Variables
            try
            {
                //Clear Session & Fields
                if (HttpContext.Current.Session["navObject"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("navObject");
                }

                //Check For Prior Value
                if (HttpContext.Current.Session["editingJobID"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("editingJobID");
                }

                //Check For Prior Value
                if (HttpContext.Current.Session["txtAddDetailForEmail"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("txtAddDetailForEmail");
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
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Saves Session Data
        /// </summary>
        protected void SaveSessionData()
        {
            //Save Session Data
            try
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
                    if (HttpContext.Current.Session["txtAddDetailForEmail"] != null)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("txtAddDetailForEmail");
                    }

                    //Add New Value
                    HttpContext.Current.Session.Add("txtAddDetailForEmail", txtAdditionalInfo.Text);
                }

                //Check For Input
                if (txtAdditionalInfo.Text.Length > 0)
                {
                    //Check For Prior Value
                    if (HttpContext.Current.Session["txtAddDetail"] != null)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("txtAddDetail");
                    }

                    //Build Text 
                    //Get Required Date For Notes
                    var strRequiredDate = "";
                    if ((TxtWorkRequestDate.Value != null) && (TxtWorkRequestDate.Text != ""))
                    {
                        //Assign Date
                        strRequiredDate = Convert.ToDateTime(TxtWorkRequestDate.Value).ToShortDateString();
                    }

                    //Get Values
                    var notes = "Name: "
                                + txtFN.Text.Trim()
                                + " "
                                + txtLN.Text.Trim()
                                + Environment.NewLine
                                + "Email: "
                                + txtEmail.Text.Trim()
                                + Environment.NewLine
                                + "Phone #: "
                                + txtPhone.Text.Trim()
                                + Environment.NewLine
                                + "Ext: "
                                + txtExt.Text.Trim()
                                + Environment.NewLine
                                + Environment.NewLine
                                + "Requested Action: "
                                + txtWorkDescription.Text.Trim()
                                + Environment.NewLine
                                + Environment.NewLine
                                + "Request Date: "
                                + strRequiredDate
                                + Environment.NewLine
                                + Environment.NewLine
                                + "Additional Details: "
                                + Environment.NewLine
                                + txtAdditionalInfo.Text.Trim();

                    //Add New Value
                    HttpContext.Current.Session.Add("txtAddDetail", notes);

                    //Set Additional Info
                    txtAdditionalInfo.Text = notes.Trim();
                }
                else
                {
                    //Check For Prior Value
                    if (HttpContext.Current.Session["txtAddDetail"] != null)
                    {
                        //Remove Old One
                        HttpContext.Current.Session.Remove("txtAddDetail");
                    }

                    //Build Text 
                    //Get Required Date For Notes
                    var strRequiredDate = "";
                    if ((TxtWorkRequestDate.Value != null) && (TxtWorkRequestDate.Text != ""))
                    {
                        //Assign Date
                        strRequiredDate = Convert.ToDateTime(TxtWorkRequestDate.Value).ToShortDateString();
                    }

                    //Temp Phone
                    var tmpPhone = "";
                    if (txtPhone.Text != @"1111111111")
                    {
                        tmpPhone = txtPhone.Text.Trim();
                    }

                    //Get Values
                    var notes = "Name: "
                                + txtFN.Text.Trim()
                                + " "
                                + txtLN.Text.Trim()
                                + Environment.NewLine
                                + "Email: "
                                + txtEmail.Text.Trim()
                                + Environment.NewLine
                                + "Phone #: "
                                + tmpPhone
                                + Environment.NewLine
                                + "Ext: "
                                + txtExt.Text.Trim()
                                + Environment.NewLine
                                + Environment.NewLine
                                + "Requested Action: "
                                + txtWorkDescription.Text.Trim()
                                + Environment.NewLine
                                + Environment.NewLine
                                + "Request Date: "
                                + strRequiredDate;

                    //Add New Value
                    HttpContext.Current.Session.Add("txtAddDetail", notes);

                    //Set Additional Info
                    txtAdditionalInfo.Text = notes.Trim();
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
                    //Check Input
                    if (txtPhone.Text != @"1111111111")
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
                }

                //Check Value
                if (txtMilepost.Value != null)
                {
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
            }
            catch (Exception ex)
            {
                //Show Error
                Master.ShowError(ex.Message);
            }
        }

        #endregion
    }
}