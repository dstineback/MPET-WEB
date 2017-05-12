using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using DevExpress.Web;

public partial class SiteMasterBase : MasterPage {

    private LogonObject _oLogon;
    public string CssVersion { get; set; }

    #region Show Menu Routine

    //Create Flags
    private bool _showMenu = true;

    public bool ShowMenu { get { return _showMenu; } set { _showMenu = value; } }

    #endregion

    #region Show Button Routines

    //Create Show Button Flags
    private bool _showSaveButton;
    private bool _showNewButton;
    private bool _showNewWRButton;
    private bool _showQuickPostButton;
    private bool _showEditButton;
    private bool _showDeleteButton;
    private bool _showPdfButton;
    private bool _showXlsButton;
    private bool _showViewButton;
    private bool _showPrintButton;
    private bool _showPostButton;
    private bool _showUnPostButton;
    private bool _showSetDefaultsButton;
    private bool _showSetEndDateButton;
    private bool _showSetStartDateButton;
    private bool _showLocationButton;
    private bool _showIssueButton;
    private bool _showAddCrewLaborButton;
    private bool _showAddCrewGroupButton;
    private bool _showMultiSelectButton;
    private bool _showEmailButton;
    private bool _showPlanButton;
    private bool _showCopyJobButton;
    private bool _showRoutineJobButton;
    private bool _showBatchCrewAddButton;
    private bool _showBatchSupervisorAddButton;
    private bool _showForcePmButton;
    private bool _showSubmitText;
    private bool _showBatchEquipmentButton;
    private bool _showBatchPartButton;
    private bool _showNonStockAddButton;
    private bool _showPrevStepButton;
    private bool _showNextStepButton;
    private bool _showMapDisplayButton;

    //Create Show Button Events
    public bool ShowSaveButton { get { return _showSaveButton; } set { _showSaveButton = value; } }
    public bool ShowNewButton { get { return _showNewButton; } set { _showNewButton = value; } }
    public bool ShowNewWRButton { get { return _showNewWRButton; } set { _showNewWRButton = value; } }
    public bool ShowQuickPostButton { get { return _showQuickPostButton; } set { _showQuickPostButton = value; } }
    public bool ShowEditButton { get { return _showEditButton; } set { _showEditButton = value; } }
    public bool ShowDeleteButton { get { return _showDeleteButton; } set { _showDeleteButton = value; } }
    public bool ShowPdfButton { get { return _showPdfButton; } set { _showPdfButton = value; } }
    public bool ShowXlsButton { get { return _showXlsButton; } set { _showXlsButton = value; } }
    public bool ShowViewButton { get { return _showViewButton; } set { _showViewButton = value; } }
    public bool ShowPrintButton { get { return _showPrintButton; } set { _showPrintButton = value; } }
    public bool ShowPostButton { get { return _showPostButton; } set { _showPostButton = value; } }
    public bool ShowUnPostButton { get { return _showUnPostButton; } set { _showUnPostButton = value; } }
    public bool ShowSetDefaultsButton { get { return _showSetDefaultsButton; } set { _showSetDefaultsButton = value; } }
    public bool ShowSetEndDateButton { get { return _showSetEndDateButton; } set { _showSetEndDateButton = value; } }
    public bool ShowSetStartDateButton { get { return _showSetStartDateButton; } set { _showSetStartDateButton = value; } }
    public bool ShowLocationButton { get { return _showLocationButton; } set { _showLocationButton = value; } }
    public bool ShowIssueButton { get { return _showIssueButton; } set { _showIssueButton = value; } }
    public bool ShowAddCrewLaborButton { get { return _showAddCrewLaborButton; } set { _showAddCrewLaborButton = value; } }
    public bool ShowAddCrewGroupButton { get { return _showAddCrewGroupButton; } set { _showAddCrewGroupButton = value; } }
    public bool ShowMultiSelectButton { get { return _showMultiSelectButton; } set { _showMultiSelectButton = value; } }
    public bool ShowMapDisplayButton { get { return _showMapDisplayButton; }  set { _showMapDisplayButton = value; } }
    public bool ShowEmailButton { get { return _showEmailButton; } set { _showEmailButton = value; } }
    public bool ShowPlanButton { get { return _showPlanButton; } set { _showPlanButton = value; } }
    public bool ShowCopyJobButton { get { return _showCopyJobButton; } set { _showCopyJobButton = value; } }
    public bool ShowRoutineJobButton { get { return _showRoutineJobButton; } set { _showRoutineJobButton = value; } }
    public bool ShowBatchCrewAddButton { get { return _showBatchCrewAddButton; } set { _showBatchCrewAddButton = value; } }
    public bool ShowBatchSupervisorAddButton { get { return _showBatchSupervisorAddButton; } set { _showBatchSupervisorAddButton = value; } }
    public bool ShowForcePmButton { get { return _showForcePmButton; } set { _showForcePmButton = value; } }
    public bool ShowSubmitTExt { get { return _showSubmitText; } set { _showSubmitText = value; } }
    public bool ShowBatchEquipmentButton { get { return _showBatchEquipmentButton; } set { _showBatchEquipmentButton = value; } }
    public bool ShowBatchPartButton { get { return _showBatchPartButton; } set { _showBatchPartButton = value; } }
    public bool ShowNonStockAddButton { get { return _showNonStockAddButton; } set { _showNonStockAddButton = value; } }
    public bool ShowPrevStepButton { get { return _showPrevStepButton; } set { _showPrevStepButton = value; } }
    public bool ShowNextStepButton { get { return _showNextStepButton; } set { _showNextStepButton = value; } }

    #endregion

    #region Enable Button Routines

    //Create Show Button Flags
    private bool _enableSaveButton = true;
    private bool _enableNewButton = true;
    private bool _enableNewWRButton = true;
    private bool _enableQuickPostButton = true;
    private bool _enableEditButton = true;
    private bool _enableDeleteButton = true;
    private bool _enablePdfButton = true;
    private bool _enableXlsButton = true;
    private bool _enableViewButton = true;
    private bool _enablePrintButton = true;
    private bool _enablePostButton = true;
    private bool _enableUnPostButton = true;
    private bool _enableSetDefaultsButton = true;
    private bool _enableSetEndDateButton = true;
    private bool _enableSetStartDateButton = true;
    private bool _enableLocationButton = true;
    private bool _enableIssueButton = true;
    private bool _enableAddCrewLaborButton = true;
    private bool _enableAddCrewGroupButton = true;
    private bool _enableMultiSelectButton = true;
    private bool _enableMapDisplayButton = true;
    private bool _enablePlanButton = true;
    private bool _enableCopyJobButton = true;
    private bool _enableRoutineJobButton = true;
    private bool _enableBatchCrewAddButton = true;
    private bool _enableBatchSupervisorAddButton = true;
    private bool _enableForcePmButton = true;
    private bool _enableBatchEquipmentButton = true;
    private bool _enableBatchPartButton = true;
    private bool _enableNonStockAddButton = true;
    private bool _enablePrevStepButton = true;
    private bool _enableNextStepButton = true;

    //Create Show Button Events
    public bool EnableSaveButton { get { return _enableSaveButton; } set { _enableSaveButton = value; } }
    public bool EnableNewButton { get { return _enableNewButton; } set { _enableNewButton = value; } }
    public bool EnableNewWRButton { get { return _enableNewWRButton; } set { _enableNewWRButton = value; } }
    public bool EnableQuickPostButton { get { return _enableQuickPostButton; } set { _enableQuickPostButton = value; } }
    public bool EnableEditButton { get { return _enableEditButton; } set { _enableEditButton = value; } }
    public bool EnableDeleteButton { get { return _enableDeleteButton; } set { _enableDeleteButton = value; } }
    public bool EnablePdfButton { get { return _enablePdfButton; } set { _enablePdfButton = value; } }
    public bool EnableXlsButton { get { return _enableXlsButton; } set { _enableXlsButton = value; } }
    public bool EnableViewButton { get { return _enableViewButton; } set { _enableViewButton = value; } }
    public bool EnablePrintButton { get { return _enablePrintButton; } set { _enablePrintButton = value; } }
    public bool EnablePostButton { get { return _enablePostButton; } set { _enablePostButton = value; } }
    public bool EnableUnPostButton { get { return _enableUnPostButton; } set { _enableUnPostButton = value; } }
    public bool EnableSetDefaultsButton { get { return _enableSetDefaultsButton; } set { _enableSetDefaultsButton = value; } }
    public bool EnableSetEndDateButton { get { return _enableSetEndDateButton; } set { _enableSetEndDateButton = value; } }
    public bool EnableSetStartDateButton { get { return _enableSetStartDateButton; } set { _enableSetStartDateButton = value; } }
    public bool EnableLocationButton { get { return _enableLocationButton; } set { _enableLocationButton = value; } }
    public bool EnableIssueButton { get { return _enableIssueButton; } set { _enableIssueButton = value; } }
    public bool EnableAddCrewLaborButton { get { return _enableAddCrewLaborButton; } set { _enableAddCrewLaborButton = value; } }
    public bool EnableAddCrewGroupButton { get { return _enableAddCrewGroupButton; } set { _enableAddCrewGroupButton = value; } }
    public bool EnableMultiSelectButton { get { return _enableMultiSelectButton; } set { _enableMultiSelectButton = value; } }
    public bool EnableMapDisplayButton { get { return _enableMapDisplayButton; } set { _enableMapDisplayButton = value; } }
    public bool EnablePlanButton { get { return _enablePlanButton; } set { _enablePlanButton = value; } }
    public bool EnableCopyJobButton { get { return _enableCopyJobButton; } set { _enableCopyJobButton = value; } }
    public bool EnableRoutineJobButton { get { return _enableRoutineJobButton; } set { _enableRoutineJobButton = value; } }
    public bool EnableBatchCrewAddButton { get { return _enableBatchCrewAddButton; } set { _enableBatchCrewAddButton = value; } }
    public bool EnableBatchSupervisorAddButton { get { return _enableBatchSupervisorAddButton; } set { _enableBatchSupervisorAddButton = value; } }
    public bool EnableForcePmButton { get { return _enableForcePmButton; } set { _enableForcePmButton = value; } }
    public bool EnableBatchEquipmentButton { get { return _enableBatchEquipmentButton; } set { _enableBatchEquipmentButton = value; } }
    public bool EnableBatchPartButton { get { return _enableBatchPartButton; } set { _enableBatchPartButton = value; } }
    public bool EnableNonStockAddButton { get { return _enableNonStockAddButton; } set { _enableNonStockAddButton = value; } }
    public bool EnablePrevStepButton { get { return _enablePrevStepButton; } set { _enablePrevStepButton = value; } }
    public bool EnableNextStepButton { get { return _enableNextStepButton; } set { _enableNextStepButton = value; } }

    #endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        //Set CSS Version <--Controls Downloading File From Server
        CssVersion = ConfigurationManager.AppSettings["CssVersionNumber"];

        //Check If We Are Showing Menu
        if (!ShowMenu)
        {
            //Hide Logon Prompt
            HelpMenuPopup.ShowOnPageLoad = false;

            //Hide Menu
            Header.Visible = false;
            
        }
        else
        {
            //Show Menu
            Header.Visible = true;

            //Show Logon Prompt If User Isn't Logged In Via Session Varaible
            HelpMenuPopup.ShowOnPageLoad = (HttpContext.Current.Session["LogonInfo"] == null);
        }

        //Setup Save Button
        var saveButton = ((ASPxButton) Footer.FindControl("SaveButton"));
        saveButton.Visible = _showSaveButton;
        saveButton.Enabled = _enableSaveButton;
        saveButton.Text = _showSubmitText ? @"Submit" : @"";

        //Setup New Button
        var newButton = ((ASPxButton)Footer.FindControl("NewButton"));
        newButton.Visible = _showNewButton;
        newButton.Enabled = _enableNewButton;

        var newWRButton = ((ASPxButton)Footer.FindControl("NewWRButton"));
        newWRButton.Visible = _showNewWRButton;
        newWRButton.Enabled = _enableNewWRButton;

        var quickPostButton = ((ASPxButton)Footer.FindControl("QuickPostButton"));
        quickPostButton.Visible = _showQuickPostButton;
        quickPostButton.Enabled = _enableQuickPostButton;

        //Setup New Non-Stock Button
        var newNonStockPart = ((ASPxButton)Footer.FindControl("NewNonStockPart"));
        newNonStockPart.Visible = _showNonStockAddButton;
        newNonStockPart.Enabled = _enableNonStockAddButton;

        //Setup Edit Button
        var editButton = ((ASPxButton)Footer.FindControl("EditButton"));
        editButton.Visible = _showEditButton;
        editButton.Enabled = _enableEditButton;

        //Setup Delete Button
        var deleteButton = ((ASPxButton)Footer.FindControl("DeleteButton"));
        deleteButton.Visible = _showDeleteButton;
        deleteButton.Enabled = _enableDeleteButton;

        //Setup View Button
        var viewButton = ((ASPxButton)Footer.FindControl("ViewButton"));
        viewButton.Visible = _showViewButton;
        viewButton.Enabled = _enableViewButton;

        //Setup Print Button
        var printButton = ((ASPxButton)Footer.FindControl("PrintButton"));
        printButton.Visible = _showPrintButton;
        printButton.Enabled = _enablePrintButton;

        //Setup PDFButton
        var pdfButton = ((ASPxButton)Footer.FindControl("ExportPDF"));
        pdfButton.Visible = _showPdfButton;
        pdfButton.Enabled = _enablePdfButton;

        //Setup XLSButton
        var xlsButton = ((ASPxButton)Footer.FindControl("ExportXLS"));
        xlsButton.Visible = _showXlsButton;
        xlsButton.Enabled = _enableXlsButton;

        //Setup Plan
        var planButton = ((ASPxButton)Footer.FindControl("PlanJob"));
        planButton.Visible = _showPlanButton;
        planButton.Enabled = _enablePlanButton;

        //Setup Copy Job Button
        var copyJobButton = ((ASPxButton)Footer.FindControl("CopyJob"));
        copyJobButton.Visible = _showCopyJobButton;
        copyJobButton.Enabled = _enableCopyJobButton;

        //Setup Routine Job Button
        var routineJobButton = ((ASPxButton)Footer.FindControl("RoutineJob"));
        routineJobButton.Visible = _showRoutineJobButton;
        routineJobButton.Enabled = _enableRoutineJobButton;

        //Setup Force PM Button
        var forcePmButton = ((ASPxButton)Footer.FindControl("ForcePM"));
        forcePmButton.Visible = _showForcePmButton;
        forcePmButton.Enabled = _enableForcePmButton;

        //Setup Post
        var postButton = ((ASPxButton)Footer.FindControl("PostJob"));
        postButton.Visible = _showPostButton;
        postButton.Enabled = _enablePostButton;

        //Setup Unpost
        var unPostButton = ((ASPxButton)Footer.FindControl("UnpostJob"));
        unPostButton.Visible = _showUnPostButton;
        unPostButton.Enabled = _enableUnPostButton;

        //Setup SetDefaults
        var setDefaultsButton = ((ASPxButton)Footer.FindControl("SetDefaults"));
        setDefaultsButton.Visible = _showSetDefaultsButton;
        setDefaultsButton.Enabled = _enableSetDefaultsButton;

        //Setup SetEndDate
        var setEndButton = ((ASPxButton)Footer.FindControl("SetToEndDate"));
        setEndButton.Visible = _showSetEndDateButton;
        setEndButton.Enabled = _enableSetEndDateButton;

        //Setup SetStartDate
        var setStartButton = ((ASPxButton)Footer.FindControl("SetToStartDate"));
        setStartButton.Visible = _showSetStartDateButton;
        setStartButton.Enabled = _enableSetStartDateButton;

        ////Setup SetLocation
        //var locationButton = ((ASPxButton)Footer.FindControl("ShowLocation"));
        //locationButton.ClientVisible = _showLocationButton;
        //locationButton.Enabled = _enableLocationButton;

        //Setup Issue
        var issueButton = ((ASPxButton)Footer.FindControl("IssueJob"));
        issueButton.Visible = _showIssueButton;
        issueButton.Enabled = _enableIssueButton;

        //Setup Prev
        var prevStepButton = ((ASPxButton)Footer.FindControl("PreviousStep"));
        prevStepButton.Visible = _showPrevStepButton;
        prevStepButton.Enabled = _enablePrevStepButton;

        //Setup Next
        var nextStepButton = ((ASPxButton)Footer.FindControl("NextStep"));
        nextStepButton.Visible = _showNextStepButton;
        nextStepButton.Enabled = _enableNewButton;

        //Setup Batch Crew Add Button
        var batchCrewAddButton = ((ASPxButton)Footer.FindControl("BatchCrewAdd"));
        batchCrewAddButton.Visible = _showBatchCrewAddButton;
        batchCrewAddButton.Enabled = _enableBatchCrewAddButton;

        //Setup Batch Supervisor Add Button
        var batchSupervisorAddButton = ((ASPxButton)Footer.FindControl("BatchSupervisorAdd"));
        batchSupervisorAddButton.Visible = _showBatchSupervisorAddButton;
        batchSupervisorAddButton.Enabled = _enableBatchSupervisorAddButton;

        //Setup Batch Equipment Add Button
        var batchEquipAddButton = ((ASPxButton)Footer.FindControl("BatchEquipmentAdd"));
        batchEquipAddButton.Visible = _showBatchEquipmentButton;
        batchEquipAddButton.Enabled = _enableBatchEquipmentButton;

        //Setup Batch Part Add Button
        var batchPartButton = ((ASPxButton)Footer.FindControl("BatchPartAdd"));
        batchPartButton.Visible = _showBatchPartButton;
        batchPartButton.Enabled = _enableBatchPartButton;

        //Setup CrewLabor
        var crewLaborButton = ((ASPxButton)Footer.FindControl("AddCrewByLabor"));
        crewLaborButton.Visible = _showAddCrewLaborButton;
        crewLaborButton.Enabled = _enableAddCrewLaborButton;

        //Setup CrewGroup
        var crewGroupButton = ((ASPxButton)Footer.FindControl("AdCrewByGroup"));
        crewGroupButton.Visible = _showAddCrewGroupButton;
        crewGroupButton.Enabled = _enableAddCrewGroupButton;

        var mutiSelectButton = ((ASPxButton)Footer.FindControl("MultiSelect"));
        mutiSelectButton.Visible = _showMultiSelectButton;
        mutiSelectButton.Enabled = _enableMultiSelectButton;

        var emailButton = ((ASPxButton)Footer.FindControl("EmailButton"));
        emailButton.Visible = _showEmailButton;
        emailButton.Enabled = _showEmailButton;

        var mapDisplayButton = ((ASPxButton)Footer.FindControl("MapDisplay"));
        mapDisplayButton.Visible = _showMapDisplayButton;
        mapDisplayButton.Enabled = _showMapDisplayButton;
    }

    /// <summary>
    /// Displays Error To User
    /// </summary>
    /// <param name="errorMessage">Error Message To Display To User</param>
    public void ShowError(string errorMessage)
    {
        //Set Text
        txtErrorInfo.Text = errorMessage.Trim();

        //Show Popup
        MsgBoxPopup.ShowOnPageLoad = true;
    }


    /// <summary>
    /// Attempt To Logon User
    /// </summary>
    /// <param name="sender">Sending Object</param>
    /// <param name="e">Event Args</param>
    protected void btnSubmitLoginCredentials_Click(object sender, EventArgs e)
    {
        //Check For Input
        if ((txtUsername.Text != "") && (txtPassword.Text != ""))
        {
            //Instanciate Class & Values
            _oLogon = new LogonObject {Username = txtUsername.Text, Password = txtPassword.Text};

            //Run Login Routine
            if (_oLogon.PerformLogin())
            {
                //Check For Previous Session Variable
                if (HttpContext.Current.Session["LogonInfo"] != null)
                {
                    //Remove Old One
                    HttpContext.Current.Session.Remove("LogonInfo");
                }

                //Add New Session State For Logon
                HttpContext.Current.Session.Add("LogonInfo", _oLogon);

                //Get Username
                //var textToShow = ((LogonObject)HttpContext.Current.Session["LogonInfo"]).Username.ToUpper();

                //Create Menu Item
                //UserControl userControl = form1.FindControl("ucTextBox") as UserControl;

                //var mainMenu = (userControl.FindControl("mainMenu") as ASPxMenu);
                //    //((this.Master.FindControl("HeaderMenu") as UserControl).FindControl("mainMenu") as ASPxMenu);

                //MenuItem helpMenuItem = mainMenu.Items.Add(textToShow, "helpMenuItem");
                //helpMenuItem.ItemStyle.CssClass = "helpMenuItem";

                //Hide Logon Details
                HelpMenuPopup.ShowOnPageLoad = false;
                Response.Redirect("~/main.aspx");
            }
            else
            {
                //Show Error In Footer
                HelpMenuPopup.FooterText = _oLogon.LastError;

                //Set Focus
                txtPassword.Focus();
            }
        }
    }

   
}
