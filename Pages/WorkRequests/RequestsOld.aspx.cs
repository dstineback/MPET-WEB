using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using MPETDSFactory;

public partial class Pages_WorkRequests_RequestsOld : System.Web.UI.Page
{
    private const int _navigationIndex = 0;
    private WorkOrder _oJob;
    private LogonObject _oLogon;
    private JobIdGenerator _oJobIDGenerator;
    private AttachmentObject _oAttachments;
    private MaintAttachmentObject _oObjAttachments;
    public string AssignedGuid = "";
    public string AssignedJobID = "";
    private bool _userCanDelete;
    private bool _userCanAdd;
    private bool _userCanEdit;
    private bool _userCanView;
    private const int AssignedFormID = 3;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check For Logon Class
        if (HttpContext.Current.Session["LogonInfo"] != null)
        {
            //Get Logon Info From Session
            _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

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
        }

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

        //Check For Post To Setup Form
        if (!IsPostBack)
        {
            //Check For Session Variable To Distinguish Previous Edit
            if (HttpContext.Current.Session["editingJobID"] != null)
            {
                //Reset Session
                ResetSession();

                //Setup For Editing -> Checks Later For Viewing Only 
                SetupForEditing();
            }
            else
            {
                //Setup For Adding
                SetupForAdding();

                //Check Tab
                if (requestTab.ActiveTabIndex == 0)
                {
                    //Set Focus
                    txtWorkDescription.Focus();
                }
            }
        }
        else
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
            if ((HttpContext.Current.Session["ComboServiceOffice"] != null) &&
                (HttpContext.Current.Session["ComboServiceOfficeText"] != null))
            {
                //Get Info From Session
                ComboServiceOffice.Value = Convert.ToInt32((HttpContext.Current.Session["ComboServiceOffice"].ToString()));
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
            TxtWorkRequestDate.Value = HttpContext.Current.Session["TxtWorkRequestDate"] != null ? Convert.ToDateTime((HttpContext.Current.Session["TxtWorkRequestDate"].ToString())) : DateTime.Now;

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
            if ((HttpContext.Current.Session["comboMilePostDir"] != null) &&
                (HttpContext.Current.Session["comboMilePostDirText"] != null))
            {
                //Get Info From Session
                comboMilePostDir.Value = Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
                comboMilePostDir.Text = (HttpContext.Current.Session["comboMilePostDirText"].ToString());
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ObjectPhoto"] != null)
            {
                //Set Image
                objectImg.ImageUrl = HttpContext.Current.Session["ObjectPhoto"].ToString();
            }

            //Get Control That Caused Post Back
            var controlName = this.Request.Params.Get("__EVENTTARGET");

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
                    case "DeleteButton":
                        {
                            //Call View Routine
                            DeleteSelectedRow();
                            break;
                        }
                    case "PrintButton":
                        {
                            //Call Print Routine
                            PrintSelectedRow();
                            break;
                        }
                    case "ShowLocation":
                    {
                        //Set Active Tab
                        requestTab.ActiveTabIndex = 2;

                        //Call JS Location Function
                        ScriptManager.RegisterStartupScript(this, GetType(), "getLocation", "getLocation();", true);

                        //Save Session Data
                        //SaveSessionData();

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
            //Setup For Editing -> Checks Later For Viewing Only 
            SetupForEditing();

            //Check For Editing Job ID
            if (HttpContext.Current.Session["editingJobID"] == null)
            {
                //Creat GUID VARIABLE
                var jobGuid = "";

                _oLogon = ((LogonObject) HttpContext.Current.Session["LogonInfo"]);

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
                            ((int) _oJob.Ds.Tables[0].Rows[0]["n_Jobid"]));

                        //Add Job String ID
                        HttpContext.Current.Session.Add("AssignedJobID", _oJob.Ds.Tables[0].Rows[0]["Jobid"]);

                        //Add Description
                        HttpContext.Current.Session.Add("txtWorkDescription", _oJob.Ds.Tables[0].Rows[0]["Title"]);

                        //Add Request Date
                        HttpContext.Current.Session.Add("TxtWorkRequestDate", _oJob.Ds.Tables[0].Rows[0]["RequestDate"]);

                        #endregion

                        #region Setup Object Info

                        HttpContext.Current.Session.Add("ObjectIDCombo", _oJob.Ds.Tables[0].Rows[0]["n_MaintObjectID"]);
                        HttpContext.Current.Session.Add("ObjectIDComboText", _oJob.Ds.Tables[0].Rows[0]["ObjectID"]);
                        HttpContext.Current.Session.Add("txtObjectDescription", _oJob.Ds.Tables[0].Rows[0]["ObjectDesc"]);
                        HttpContext.Current.Session.Add("txtObjectArea", _oJob.Ds.Tables[0].Rows[0]["ObjectArea"]);
                        HttpContext.Current.Session.Add("txtObjectLocation", _oJob.Ds.Tables[0].Rows[0]["ObjectLoc"]);
                        HttpContext.Current.Session.Add("txtObjectAssetNumber",
                            _oJob.Ds.Tables[0].Rows[0]["ObjectAsset"]);

                        #endregion

                        #region Seetup Requestor

                        HttpContext.Current.Session.Add("ComboRequestor", _oJob.Ds.Tables[0].Rows[0]["UserID"]);
                        HttpContext.Current.Session.Add("ComboRequestorText", _oJob.Ds.Tables[0].Rows[0]["Username"]);

                        #endregion

                        #region Setup Priority

                        HttpContext.Current.Session.Add("ComboPriority", _oJob.Ds.Tables[0].Rows[0]["n_priorityid"]);
                        HttpContext.Current.Session.Add("ComboPriorityText", _oJob.Ds.Tables[0].Rows[0]["priorityid"]);

                        #endregion

                        #region Setup Reason

                        HttpContext.Current.Session.Add("comboReason", _oJob.Ds.Tables[0].Rows[0]["nJobReasonID"]);
                        HttpContext.Current.Session.Add("comboReasonText", _oJob.Ds.Tables[0].Rows[0]["JobReasonID"]);

                        #endregion

                        #region Setup Route To

                        HttpContext.Current.Session.Add("comboRouteTo", _oJob.Ds.Tables[0].Rows[0]["n_RouteTo"]);
                        HttpContext.Current.Session.Add("comboRouteToText", _oJob.Ds.Tables[0].Rows[0]["OwnerID"]);

                        #endregion

                        #region Setup Hwy Route

                        HttpContext.Current.Session.Add("comboHwyRoute", _oJob.Ds.Tables[0].Rows[0]["n_StateRouteID"]);
                        HttpContext.Current.Session.Add("comboHwyRouteText", _oJob.Ds.Tables[0].Rows[0]["StateRouteID"]);

                        #endregion

                        #region Setup Milepost

                        HttpContext.Current.Session.Add("txtMilepost", _oJob.Ds.Tables[0].Rows[0]["Milepost"]);
                        HttpContext.Current.Session.Add("comboMilePostDir",
                            _oJob.Ds.Tables[0].Rows[0]["n_MilePostDirectionID"]);
                        HttpContext.Current.Session.Add("comboMilePostDirText",
                            _oJob.Ds.Tables[0].Rows[0]["MilePostDirectionID"]);

                        #endregion

                        #region Setup Cost Code

                        HttpContext.Current.Session.Add("ComboCostCode", _oJob.Ds.Tables[0].Rows[0]["n_CostCodeID"]);
                        HttpContext.Current.Session.Add("ComboCostCodeText", _oJob.Ds.Tables[0].Rows[0]["costcodeid"]);

                        #endregion

                        #region Setup Fund Source

                        HttpContext.Current.Session.Add("ComboFundSource", _oJob.Ds.Tables[0].Rows[0]["n_FundSrcCodeID"]);
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
                        HttpContext.Current.Session.Add("ComboWorkOpText", _oJob.Ds.Tables[0].Rows[0]["WorkOpID"]);

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

                        //Load Object Attachments To Get First Photo
                        if (_oObjAttachments.GetAttachments(((int) _oJob.Ds.Tables[0].Rows[0]["n_MaintObjectID"])))
                        {
                            //Check For Table
                            if (_oObjAttachments.Ds.Tables.Count > 0)
                            {
                                //Create Control Flag
                                var firstPicFound = false;

                                //Loop Attachments
                                for (var rowIndex = 0; rowIndex < _oObjAttachments.Ds.Tables[0].Rows.Count; rowIndex++)
                                {
                                    //Determine Attachment Type
                                    switch (_oObjAttachments.Ds.Tables[0].Rows[rowIndex][1].ToString().ToUpper())
                                    {
                                        case "GIF":
                                        case "BMP":
                                        case "JPG":
                                        {
                                            //Check For Prior Value
                                            if (HttpContext.Current.Session["ObjectPhoto"] != null)
                                            {
                                                //Remove Old One
                                                HttpContext.Current.Session.Remove("ObjectPhoto");
                                            }

                                            //Add New Value
                                            HttpContext.Current.Session.Add("ObjectPhoto",
                                                _oObjAttachments.Ds.Tables[0].Rows[rowIndex]["LocationOrURL"].ToString());

                                            firstPicFound = true;

                                            //Break
                                            break;
                                        }
                                        default:
                                        {
                                            //Do Nothing
                                            break;
                                        }
                                    }

                                    //Check Control
                                    if (firstPicFound)
                                    {
                                        //Break Loop
                                        break;
                                    }
                                }
                            }
                        }

                        //Enable Tab
                        requestTab.Enabled = true;
                    }
                }
            }
            else
            {
                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtWorkDescription"] != null)
                //{
                //    //Get Additional Info From Session
                //    txtWorkDescription.Text = (HttpContext.Current.Session["txtWorkDescription"].ToString());
                //}

                ////Job ID
                //if (HttpContext.Current.Session["AssignedJobID"] != null)
                //{
                //    //Get Additional Info From Session
                //    lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboCostCode"] != null) &&
                //    (HttpContext.Current.Session["ComboCostCodeText"] != null))
                //{
                //    //Get Info From Session
                //    ComboCostCode.Value = Convert.ToInt32((HttpContext.Current.Session["ComboCostCode"].ToString()));
                //    ComboCostCode.Text = (HttpContext.Current.Session["ComboCostCodeText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboFundSource"] != null) &&
                //    (HttpContext.Current.Session["ComboFundSourceText"] != null))
                //{
                //    //Get Info From Session
                //    ComboFundSource.Value = Convert.ToInt32((HttpContext.Current.Session["ComboFundSource"].ToString()));
                //    ComboFundSource.Text = (HttpContext.Current.Session["ComboFundSourceText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboWorkOrder"] != null) &&
                //    (HttpContext.Current.Session["ComboWorkOrderText"] != null))
                //{
                //    //Get Info From Session
                //    ComboWorkOrder.Value = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOrder"].ToString()));
                //    ComboWorkOrder.Text = (HttpContext.Current.Session["ComboWorkOrderText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboWorkOp"] != null) &&
                //    (HttpContext.Current.Session["ComboWorkOpText"] != null))
                //{
                //    //Get Info From Session
                //    ComboWorkOp.Value = Convert.ToInt32((HttpContext.Current.Session["ComboWorkOp"].ToString()));
                //    ComboWorkOp.Text = (HttpContext.Current.Session["ComboWorkOpText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboOrgCode"] != null) &&
                //    (HttpContext.Current.Session["ComboOrgCodeText"] != null))
                //{
                //    //Get Info From Session
                //    ComboOrgCode.Value = Convert.ToInt32((HttpContext.Current.Session["ComboOrgCode"].ToString()));
                //    ComboOrgCode.Text = (HttpContext.Current.Session["ComboOrgCodeText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboFundGroup"] != null) &&
                //    (HttpContext.Current.Session["ComboFundGroupText"] != null))
                //{
                //    //Get Info From Session
                //    ComboFundGroup.Value = Convert.ToInt32((HttpContext.Current.Session["ComboFundGroup"].ToString()));
                //    ComboFundGroup.Text = (HttpContext.Current.Session["ComboFundGroupText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboCtlSection"] != null) &&
                //    (HttpContext.Current.Session["ComboCtlSectionText"] != null))
                //{
                //    //Get Info From Session
                //    ComboCtlSection.Value = Convert.ToInt32((HttpContext.Current.Session["ComboCtlSection"].ToString()));
                //    ComboCtlSection.Text = (HttpContext.Current.Session["ComboCtlSectionText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboEquipNum"] != null) &&
                //    (HttpContext.Current.Session["ComboEquipNumText"] != null))
                //{
                //    //Get Info From Session
                //    ComboEquipNum.Value = Convert.ToInt32((HttpContext.Current.Session["ComboEquipNum"].ToString()));
                //    ComboEquipNum.Text = (HttpContext.Current.Session["ComboEquipNumText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtAddDetail"] != null)
                //{
                //    //Get Additional Info From Session
                //    txtAdditionalInfo.Text = (HttpContext.Current.Session["txtAddDetail"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboServiceOffice"] != null) &&
                //    (HttpContext.Current.Session["ComboServiceOfficeText"] != null))
                //{
                //    //Get Info From Session
                //    ComboServiceOffice.Value = Convert.ToInt32((HttpContext.Current.Session["ComboServiceOffice"].ToString()));
                //    ComboServiceOffice.Text = (HttpContext.Current.Session["ComboServiceOfficeText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtFN"] != null)
                //{
                //    //Get Info From Session
                //    txtFN.Value = (HttpContext.Current.Session["txtFN"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtLN"] != null)
                //{
                //    //Get Info From Session
                //    txtLN.Value = (HttpContext.Current.Session["txtLN"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtEmail"] != null)
                //{
                //    //Get Info From Session
                //    txtEmail.Value = (HttpContext.Current.Session["txtEmail"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtPhone"] != null)
                //{
                //    //Get Info From Session
                //    txtPhone.Value = (HttpContext.Current.Session["txtPhone"].ToString());
                //}
                //else
                //{
                //    //Set Default
                //    txtPhone.Value = 1111111111;
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtExt"] != null)
                //{
                //    //Get Info From Session
                //    txtExt.Value = (HttpContext.Current.Session["txtExt"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtMail"] != null)
                //{
                //    //Get Info From Session
                //    txtMail.Value = (HttpContext.Current.Session["txtMail"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtBuilding"] != null)
                //{
                //    //Get Info From Session
                //    txtBuilding.Value = (HttpContext.Current.Session["txtBuilding"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtRoomNum"] != null)
                //{
                //    //Get Info From Session
                //    txtRoomNum.Value = (HttpContext.Current.Session["txtRoomNum"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["GPSX"] != null)
                //{
                //    //Get Info From Session
                //    GPSX.Value = (HttpContext.Current.Session["GPSX"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["GPSY"] != null)
                //{
                //    //Get Info From Session
                //    GPSY.Value = (HttpContext.Current.Session["GPSY"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["GPSZ"] != null)
                //{
                //    //Get Info From Session
                //    GPSZ.Value = (HttpContext.Current.Session["GPSZ"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["ObjectIDCombo"] != null)
                //{
                //    //Get Info From Session
                //    ObjectIDCombo.Value = Convert.ToInt32((HttpContext.Current.Session["ObjectIDCombo"].ToString()));
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["ObjectIDComboText"] != null)
                //{
                //    //Get Info From Session
                //    ObjectIDCombo.Text = (HttpContext.Current.Session["ObjectIDComboText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtObjectDescription"] != null)
                //{
                //    //Get Info From Session
                //    txtObjectDescription.Text = (HttpContext.Current.Session["txtObjectDescription"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtObjectArea"] != null)
                //{
                //    //Get Info From Session
                //    txtObjectArea.Text = (HttpContext.Current.Session["txtObjectArea"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtObjectLocation"] != null)
                //{
                //    //Get Info From Session
                //    txtObjectLocation.Text = (HttpContext.Current.Session["txtObjectLocation"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtObjectAssetNumber"] != null)
                //{
                //    //Get Info From Session
                //    txtObjectAssetNumber.Text = (HttpContext.Current.Session["txtObjectAssetNumber"].ToString());
                //}

                ////Check For Previous Session Variables
                //TxtWorkRequestDate.Value = HttpContext.Current.Session["TxtWorkRequestDate"] != null ? Convert.ToDateTime((HttpContext.Current.Session["TxtWorkRequestDate"].ToString())) : DateTime.Now;

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboRequestor"] != null) &&
                //    (HttpContext.Current.Session["ComboRequestorText"] != null))
                //{
                //    //Get Info From Session
                //    ComboRequestor.Value = Convert.ToInt32((HttpContext.Current.Session["ComboRequestor"].ToString()));
                //    ComboRequestor.Text = (HttpContext.Current.Session["ComboRequestorText"].ToString());
                //}
                //else if (HttpContext.Current.Session["LogonInfo"] != null)
                //{
                //    //Get Logon Info
                //    _oLogon = ((LogonObject)HttpContext.Current.Session["LogonInfo"]);

                //    //Set Requestor
                //    ComboRequestor.Value = _oLogon.UserID;
                //    ComboRequestor.Text = _oLogon.Username;

                //    //Add Session Variables
                //    HttpContext.Current.Session.Add("ComboRequestor", _oLogon.UserID);
                //    HttpContext.Current.Session.Add("ComboRequestorText", _oLogon.Username);
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["ComboPriority"] != null) &&
                //    (HttpContext.Current.Session["ComboPriorityText"] != null))
                //{
                //    //Get Info From Session
                //    ComboPriority.Value = Convert.ToInt32((HttpContext.Current.Session["ComboPriority"].ToString()));
                //    ComboPriority.Text = (HttpContext.Current.Session["ComboPriorityText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["comboReason"] != null) &&
                //    (HttpContext.Current.Session["comboReasonText"] != null))
                //{
                //    //Get Info From Session
                //    comboReason.Value = Convert.ToInt32((HttpContext.Current.Session["comboReason"].ToString()));
                //    comboReason.Text = (HttpContext.Current.Session["comboReasonText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["comboRouteTo"] != null) &&
                //    (HttpContext.Current.Session["comboRouteToText"] != null))
                //{
                //    //Get Info From Session
                //    comboRouteTo.Value = Convert.ToInt32((HttpContext.Current.Session["comboRouteTo"].ToString()));
                //    comboRouteTo.Text = (HttpContext.Current.Session["comboRouteToText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["comboHwyRoute"] != null) &&
                //    (HttpContext.Current.Session["comboHwyRouteText"] != null))
                //{
                //    //Get Info From Session
                //    comboHwyRoute.Value = Convert.ToInt32((HttpContext.Current.Session["comboHwyRoute"].ToString()));
                //    comboHwyRoute.Text = (HttpContext.Current.Session["comboHwyRouteText"].ToString());
                //}

                ////Check For Previous Session Variables
                //if (HttpContext.Current.Session["txtMilepost"] != null)
                //{
                //    //Get Info From Session
                //    txtMilepost.Value = (HttpContext.Current.Session["txtMilepost"].ToString());
                //}

                ////Check For Previous Session Variables
                //if ((HttpContext.Current.Session["comboMilePostDir"] != null) &&
                //    (HttpContext.Current.Session["comboMilePostDirText"] != null))
                //{
                //    //Get Info From Session
                //    comboMilePostDir.Value = Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
                //    comboMilePostDir.Text = (HttpContext.Current.Session["comboMilePostDirText"].ToString());
                //}

                ////Check For Prior Value
                //if (HttpContext.Current.Session["ObjectPhoto"] != null)
                //{
                //    //Set Image
                //    objectImg.ImageUrl = HttpContext.Current.Session["ObjectPhoto"].ToString();
                //}

                //Enable Tab
                requestTab.Enabled = true;
            }
        }

        //Setup User Defined Fields
        SetupUserDefinedFields();

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
            if ((HttpContext.Current.Session["ComboServiceOffice"] != null) &&
                (HttpContext.Current.Session["ComboServiceOfficeText"] != null))
            {
                //Get Info From Session
                ComboServiceOffice.Value = Convert.ToInt32((HttpContext.Current.Session["ComboServiceOffice"].ToString()));
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
            TxtWorkRequestDate.Value = HttpContext.Current.Session["TxtWorkRequestDate"] != null ? Convert.ToDateTime((HttpContext.Current.Session["TxtWorkRequestDate"].ToString())) : DateTime.Now;

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
            if ((HttpContext.Current.Session["comboMilePostDir"] != null) &&
                (HttpContext.Current.Session["comboMilePostDirText"] != null))
            {
                //Get Info From Session
                comboMilePostDir.Value = Convert.ToInt32((HttpContext.Current.Session["comboMilePostDir"].ToString()));
                comboMilePostDir.Text = (HttpContext.Current.Session["comboMilePostDirText"].ToString());
            }

            //Check For Prior Value
            if (HttpContext.Current.Session["ObjectPhoto"] != null)
            {
                //Set Image
                objectImg.ImageUrl = HttpContext.Current.Session["ObjectPhoto"].ToString();
            }
        }
    }

    private void AddNewRow()
    {
        //Clear Session Variables
        ResetSession();

        //Redirect To Edit Page With Job ID
        Response.Redirect("~/Pages/WorkRequests/Requests.aspx", true);
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

            //Create Deletion Count
            var totalDeleted = 0;

            //Create Deletion Key
            var recordToDelete = -1;


            //Check For Job ID
            if (HttpContext.Current.Session["editingJobID"] != null)
            {
                //Get ID
                recordToDelete = Convert.ToInt32(HttpContext.Current.Session["editingJobID"]);
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
                    //Updte Deletion Count
                    totalDeleted++;

                    //Set Deletion Done
                    deletionDone = true;
                }
            }

            //Check Deletion Done
            if (deletionDone)
            {
                //Clear Session Variables
                ResetSession();

                //Direct Back To List
                Response.Redirect("~/Pages/WorkRequests/RequestsList.aspx", true);
            }
        }
    }

    private void PrintSelectedRow()
    {
        //Check For Job ID
        if (HttpContext.Current.Session["editingJobID"] != null)
        {
            //Check For Previous Session Report Parm ID
            if (HttpContext.Current.Session["ReportParm"] != null)
            {
                //Remove Value
                HttpContext.Current.Session.Remove("ReportParm");
            }

            //Add Session Report Parm ID
            HttpContext.Current.Session.Add("ReportParm", HttpContext.Current.Session["ReportParm"]);

            //Check For Previous Report Name
            if (HttpContext.Current.Session["ReportToDisplay"] != null)
            {
                //Remove Value
                HttpContext.Current.Session.Remove("ReportToDisplay");
            }

            //Add Report To Display
            HttpContext.Current.Session.Add("ReportToDisplay", "simplewo.rpt");

            //Redirect To Report Page
            Response.Redirect("~/Reports/ViewReport.aspx", true);
        }
    }

    public bool FormSetup(int userId)
    {
        //Create Flag
        var rightsLoaded = false;

        //Get Security Settings
        using (
            var oSecurity = new UserSecurityTemplate(ConfigurationManager.ConnectionStrings["connection"].ToString(),
                ConfigurationManager.AppSettings["UsingWebService"] == "Y"))
        {
            //Check Class
            if (oSecurity != null)
            {
                //Get Rights
                rightsLoaded = oSecurity.GetUserFormRights(userId, AssignedFormID,
                    ref _userCanEdit, ref _userCanAdd,
                    ref _userCanDelete, ref _userCanView);
            }
        }

        //Return Flag
        return rightsLoaded;
    }

    /// <summary>
    /// Enables Form Buttons For Viewing
    /// </summary>
    private void SetupForViewing()
    {
        //Setup Buttons
        Master.ShowSaveButton = false;
        Master.ShowNewButton = _userCanAdd;
        Master.ShowDeleteButton = false;
        Master.ShowPrintButton = true;

    }

    /// <summary>
    /// Enables Form Optiosn For Editing
    /// </summary>
    private void SetupForEditing()
    {
        //Setup Buttons
        Master.ShowSaveButton = (_userCanAdd || _userCanEdit);
        Master.ShowNewButton = _userCanAdd;
        Master.ShowDeleteButton = _userCanDelete;
        Master.ShowPrintButton = true;
    }

    /// <summary>
    /// Enables Form Options For Adding
    /// </summary>
    private void SetupForAdding()
    {
        //Setup Buttons
        Master.ShowSaveButton = (_userCanAdd || _userCanEdit);
        Master.ShowNewButton = _userCanAdd;
    }

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

    protected void Page_Init(object sender, EventArgs e)
    {
        //Initialize Classes
        _oJob = new WorkOrder(ConfigurationManager.ConnectionStrings["connection"].ToString(), (ConfigurationManager.AppSettings["UsingWebService"] == "Y"));
        _oAttachments = new AttachmentObject(ConfigurationManager.ConnectionStrings["connection"].ToString(), (ConfigurationManager.AppSettings["UsingWebService"] == "Y"));
        _oObjAttachments = new MaintAttachmentObject(ConfigurationManager.ConnectionStrings["connection"].ToString(), (ConfigurationManager.AppSettings["UsingWebService"] == "Y"));

        //Set Datasources
        AreaSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        ObjectDataSource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        CostCodeSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        FundSourceSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        WorkOrderSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        WorkOpSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        OrgCodeSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        FundGroupSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        CtlSectionSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        EquipNumSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        RequestorSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        PrioritySqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        ReasonSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        RouteToSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        HwyRouteSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        MilePostDirSqlDatasource.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();

        //Setup Fields
        TxtWorkRequestDate.Value = DateTime.Now;
        txtPhone.Value = 1111111111;

    }

    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        // RemoveFileWithDelay(e.UploadedFile.FileNameInStorage, 5);

        string name = e.UploadedFile.FileName;
        string url = GetImageUrl(e.UploadedFile.FileNameInStorage);
        long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
        string sizeText = sizeInKilobytes.ToString() + " KB";
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

                if (_oAttachments.Add(Convert.ToInt32(HttpContext.Current.Session["editingJobID"].ToString()),
                    -1,
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
                }
            }
        }
    }

    string GetImageUrl(string fileName)
    {
        AzureFileSystemProvider provider = new AzureFileSystemProvider("");
        provider.StorageAccountName = UploadControl.AzureSettings.StorageAccountName;
        provider.AccessKey = UploadControl.AzureSettings.AccessKey;
        provider.ContainerName = UploadControl.AzureSettings.ContainerName;
        FileManagerFile file = new FileManagerFile(provider, fileName);
        FileManagerFile[] files = new FileManagerFile[] { file };
        return provider.GetDownloadUrl(files);
    }

    // <summary>
    // Checks For User Defined Fields And Labels Accordingly
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

    #endregion 

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
        const decimal mileageTo = 0;

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
                subAssemblyID,
                stateRouteId,
                milepost,
                mileageTo,
                mpIncreasing,
                additionalDamage,
                percentOverage,
                _oLogon.UserID,
                ref AssignedJobID))
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
                        _oLogon.UserID))
                {
                    //Return False To Prevent Navigation
                    return false;
                }
                else
                {
                    //Check For Value
                    if (HttpContext.Current.Session["AssignedJobID"] != null)
                    {
                        //Get Additional Info From Session
                        lblHeader.Text = (HttpContext.Current.Session["AssignedJobID"].ToString());

                        //Setup For Editing
                        SetupForEditing();
                    }

                    //Success Return True
                    return true;
                }
            }
            else
            {
                //Return False To Prevent Navigation
                return false;
            }
        }
        catch (System.Exception ex)
        {
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
        const decimal mileageTo = 0;
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
                _oJobIDGenerator =
                    new JobIdGenerator(ConfigurationManager.ConnectionStrings["connection"].ToString(),
                        (ConfigurationManager.AppSettings["UsingWebService"] == "Y"), _oLogon.UserID, _oLogon.AreaID);
            }
            else
            {
                //Setup For Non Logged In User
                _oJobIDGenerator =
                    new JobIdGenerator(ConfigurationManager.ConnectionStrings["connection"].ToString(),
                        (ConfigurationManager.AppSettings["UsingWebService"] == "Y"), -1, -1);
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
                mileageTo,
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
                else
                {
                    //Setup For Editing
                    SetupForEditing();

                    //Enable Tab
                    requestTab.Enabled = true;

                    //Return True
                    return true;
                }
            }
            else
            {
                //Return False To Prevent Navigation
                return false;
            }
        }
        catch (Exception ex)
        {
            //Return False To Prevent Navigation
            return false;
        }
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
        if (HttpContext.Current.Session["ObjectPhoto"] != null)
        {
            //Remove Old One
            HttpContext.Current.Session.Remove("ObjectPhoto");
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

            #region Photo

            //Check For Prior Value
            if (HttpContext.Current.Session["ObjectPhoto"] != null)
            {
                //Remove Old One
                HttpContext.Current.Session.Remove("ObjectPhoto");
            }

            //Add New Value
            HttpContext.Current.Session.Add("ObjectPhoto", objectImg.ImageUrl);

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
        if (HttpContext.Current.Session["txtMilepost"] != null)
        {
            //Remove Old One
            HttpContext.Current.Session.Remove("txtMilepost");
        }

        if (txtMilepost.Value != null)
        {

            //Add New Value
            HttpContext.Current.Session.Add("txtMilepost", txtMilepost.Value.ToString());
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
}