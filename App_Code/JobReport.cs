using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;

/// <summary>
/// Summary description for JobReport
/// </summary>
public class JobReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRLabel ObjectIDLable;
    private XRLabel ObjectDescLable;
    private XRLabel ObjectID;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private PageFooterBand pageFooterBand1;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand reportHeaderBand1;
    private XRLabel xrLabel5;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private LogonObject _oLogon;
    private int jobID = -1;
    
    
    private DevExpress.XtraReports.Parameters.Parameter parameterJobID;


    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private XRLabel ObjectDesc;
    private XRLabel PriorityDesc;
    private XRLabel RequestDate;
    private XRLabel Requestor;
    private XRLabel FundSrcCodeDesc;
    private XRLabel WorkOrderCodeID;
    private XRLabel TypeOfJob;
    private XRLabel cStatusID;
    private XRLabel cSupervisorName;
    private XRLabel cShiftID;
    private XRLabel GroupDesc;
    private XRLabel cReasoncodeID;
    private XRLabel actualdowntime;
    private XRLabel estimateddowntime;
    private XRLabel return_within;
    private XRLabel TaskID;
    private XRLabel JobstepNotes;
    private XRLabel LocationDesc;
    private XRLabel LacationSrc;
    private XRLabel AssignedAreaDesc;
    private XRLabel AssignedArea;
    private XRLabel AreaLable;
    private XRLabel PriorityLable;
    private XRLabel GroupLable;
    private XRLabel RequestorLable;
    private XRLabel StartDate;
    private XRLabel ActTimeLable;
    private XRLabel TypeLable;
    private XRLabel WorkOpLable;
    private XRLabel FundSourceLable;
    private XRLabel StatusLable;
    private XRLabel SupervisorLable;
    private XRLabel ShiftLable;
    private XRLabel JobSummaryLable;
    private XRLabel LocationDescLable;
    private XRLabel LocattionLable;
    private XRLabel ReasonLable;
    private XRLabel EstTimeLable;
    private XRLabel RtnWithinLable;
    private XRLabel StartDateLable;
    private XRLabel RequestDateLable;
    private XRLabel AreaDescLable;
    private XRLabel cJobID;
    private XRLabel xrLabel50;
    private XRLine xrLine2;
    private XRLabel cServiceingEquipment;
    private XRLabel HistoryAuditTrail;
    private XRLine xrLine3;
    private XRLabel xrLabel1;
    private XRLabel JobstepTitle;
    private XRLabel DetailedDescriptionLable;
    private TopMarginBand topMarginBand1;
    private DetailBand detailBand1;
    private BottomMarginBand bottomMarginBand1;
    private XRLabel TaskIDLable;
    
    public JobReport()
    {
        if(HttpContext.Current.Session["ReportParm"] != null)
        {
            jobID = Convert.ToInt32(HttpContext.Current.Session["ReportParm"].ToString());
        }

            InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            string resourceFileName = "JobReport.resx";
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.JobstepTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailedDescriptionLable = new DevExpress.XtraReports.UI.XRLabel();
            this.HistoryAuditTrail = new DevExpress.XtraReports.UI.XRLabel();
            this.cServiceingEquipment = new DevExpress.XtraReports.UI.XRLabel();
            this.StartDate = new DevExpress.XtraReports.UI.XRLabel();
            this.ActTimeLable = new DevExpress.XtraReports.UI.XRLabel();
            this.TypeLable = new DevExpress.XtraReports.UI.XRLabel();
            this.WorkOpLable = new DevExpress.XtraReports.UI.XRLabel();
            this.FundSourceLable = new DevExpress.XtraReports.UI.XRLabel();
            this.StatusLable = new DevExpress.XtraReports.UI.XRLabel();
            this.SupervisorLable = new DevExpress.XtraReports.UI.XRLabel();
            this.ShiftLable = new DevExpress.XtraReports.UI.XRLabel();
            this.JobSummaryLable = new DevExpress.XtraReports.UI.XRLabel();
            this.LocationDescLable = new DevExpress.XtraReports.UI.XRLabel();
            this.LocattionLable = new DevExpress.XtraReports.UI.XRLabel();
            this.ReasonLable = new DevExpress.XtraReports.UI.XRLabel();
            this.EstTimeLable = new DevExpress.XtraReports.UI.XRLabel();
            this.RtnWithinLable = new DevExpress.XtraReports.UI.XRLabel();
            this.StartDateLable = new DevExpress.XtraReports.UI.XRLabel();
            this.RequestDateLable = new DevExpress.XtraReports.UI.XRLabel();
            this.AreaDescLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cStatusID = new DevExpress.XtraReports.UI.XRLabel();
            this.cSupervisorName = new DevExpress.XtraReports.UI.XRLabel();
            this.cShiftID = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.cReasoncodeID = new DevExpress.XtraReports.UI.XRLabel();
            this.actualdowntime = new DevExpress.XtraReports.UI.XRLabel();
            this.estimateddowntime = new DevExpress.XtraReports.UI.XRLabel();
            this.return_within = new DevExpress.XtraReports.UI.XRLabel();
            this.TaskID = new DevExpress.XtraReports.UI.XRLabel();
            this.JobstepNotes = new DevExpress.XtraReports.UI.XRLabel();
            this.LocationDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.LacationSrc = new DevExpress.XtraReports.UI.XRLabel();
            this.AssignedAreaDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.AssignedArea = new DevExpress.XtraReports.UI.XRLabel();
            this.AreaLable = new DevExpress.XtraReports.UI.XRLabel();
            this.PriorityLable = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupLable = new DevExpress.XtraReports.UI.XRLabel();
            this.RequestorLable = new DevExpress.XtraReports.UI.XRLabel();
            this.TaskIDLable = new DevExpress.XtraReports.UI.XRLabel();
            this.ObjectDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.PriorityDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.RequestDate = new DevExpress.XtraReports.UI.XRLabel();
            this.Requestor = new DevExpress.XtraReports.UI.XRLabel();
            this.FundSrcCodeDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.WorkOrderCodeID = new DevExpress.XtraReports.UI.XRLabel();
            this.TypeOfJob = new DevExpress.XtraReports.UI.XRLabel();
            this.ObjectIDLable = new DevExpress.XtraReports.UI.XRLabel();
            this.ObjectDescLable = new DevExpress.XtraReports.UI.XRLabel();
            this.ObjectID = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.pageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.cJobID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.parameterJobID = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.JobstepTitle,
            this.DetailedDescriptionLable,
            this.HistoryAuditTrail,
            this.cServiceingEquipment,
            this.StartDate,
            this.ActTimeLable,
            this.TypeLable,
            this.WorkOpLable,
            this.FundSourceLable,
            this.StatusLable,
            this.SupervisorLable,
            this.ShiftLable,
            this.JobSummaryLable,
            this.LocationDescLable,
            this.LocattionLable,
            this.ReasonLable,
            this.EstTimeLable,
            this.RtnWithinLable,
            this.StartDateLable,
            this.RequestDateLable,
            this.AreaDescLable,
            this.cStatusID,
            this.cSupervisorName,
            this.cShiftID,
            this.GroupDesc,
            this.cReasoncodeID,
            this.actualdowntime,
            this.estimateddowntime,
            this.return_within,
            this.TaskID,
            this.JobstepNotes,
            this.LocationDesc,
            this.LacationSrc,
            this.AssignedAreaDesc,
            this.AssignedArea,
            this.AreaLable,
            this.PriorityLable,
            this.GroupLable,
            this.RequestorLable,
            this.TaskIDLable,
            this.ObjectDesc,
            this.PriorityDesc,
            this.RequestDate,
            this.Requestor,
            this.FundSrcCodeDesc,
            this.WorkOrderCodeID,
            this.TypeOfJob,
            this.ObjectIDLable,
            this.ObjectDescLable,
            this.ObjectID});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 549.6667F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 10, 0, 100F);
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 100F;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 333.875F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(150F, 22.99997F);
            this.xrLabel1.StyleName = "FieldCaption";
            this.xrLabel1.Text = "Audit History";
            // 
            // JobstepTitle
            // 
            this.JobstepTitle.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.JobstepTitle")});
            this.JobstepTitle.Dpi = 100F;
            this.JobstepTitle.LocationFloat = new DevExpress.Utils.PointFloat(93.99999F, 204.2084F);
            this.JobstepTitle.Name = "JobstepTitle";
            this.JobstepTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.JobstepTitle.SizeF = new System.Drawing.SizeF(420.2915F, 23F);
            this.JobstepTitle.StyleName = "DataField";
            this.JobstepTitle.Text = "JobStepTitle";
            // 
            // DetailedDescriptionLable
            // 
            this.DetailedDescriptionLable.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            this.DetailedDescriptionLable.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.DetailedDescriptionLable.BorderWidth = 2F;
            this.DetailedDescriptionLable.Dpi = 100F;
            this.DetailedDescriptionLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 402F);
            this.DetailedDescriptionLable.Name = "DetailedDescriptionLable";
            this.DetailedDescriptionLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.DetailedDescriptionLable.SizeF = new System.Drawing.SizeF(250F, 22.99997F);
            this.DetailedDescriptionLable.StyleName = "FieldCaption";
            this.DetailedDescriptionLable.StylePriority.UseBorderDashStyle = false;
            this.DetailedDescriptionLable.StylePriority.UseBorders = false;
            this.DetailedDescriptionLable.StylePriority.UseBorderWidth = false;
            this.DetailedDescriptionLable.Text = "Detailed Description";
            // 
            // HistoryAuditTrail
            // 
            this.HistoryAuditTrail.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.HistoryAuditTrail", "{0:MMMM d, yyyy h:mm tt}")});
            this.HistoryAuditTrail.Dpi = 100F;
            this.HistoryAuditTrail.LocationFloat = new DevExpress.Utils.PointFloat(0F, 356.875F);
            this.HistoryAuditTrail.Multiline = true;
            this.HistoryAuditTrail.Name = "HistoryAuditTrail";
            this.HistoryAuditTrail.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.HistoryAuditTrail.SizeF = new System.Drawing.SizeF(412.75F, 19.875F);
            this.HistoryAuditTrail.StyleName = "DataField";
            // 
            // cServiceingEquipment
            // 
            this.cServiceingEquipment.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.cServiceingEquipment")});
            this.cServiceingEquipment.Dpi = 100F;
            this.cServiceingEquipment.LocationFloat = new DevExpress.Utils.PointFloat(93.99999F, 260.4166F);
            this.cServiceingEquipment.Name = "cServiceingEquipment";
            this.cServiceingEquipment.NullValueText = "No Equipment to Display";
            this.cServiceingEquipment.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cServiceingEquipment.SizeF = new System.Drawing.SizeF(445.2916F, 22.99997F);
            this.cServiceingEquipment.StyleName = "DataField";
            this.cServiceingEquipment.Text = "Job Equipment";
            // 
            // StartDate
            // 
            this.StartDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.StartingDate")});
            this.StartDate.Dpi = 100F;
            this.StartDate.LocationFloat = new DevExpress.Utils.PointFloat(388.458F, 74.99991F);
            this.StartDate.Name = "StartDate";
            this.StartDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.StartDate.SizeF = new System.Drawing.SizeF(121.6666F, 23.00002F);
            this.StartDate.StyleName = "DataField";
            this.StartDate.Text = "StartDate";
            // 
            // ActTimeLable
            // 
            this.ActTimeLable.Dpi = 100F;
            this.ActTimeLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 143.9999F);
            this.ActTimeLable.Name = "ActTimeLable";
            this.ActTimeLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ActTimeLable.SizeF = new System.Drawing.SizeF(85.41666F, 23F);
            this.ActTimeLable.StyleName = "FieldCaption";
            this.ActTimeLable.StylePriority.UseTextAlignment = false;
            this.ActTimeLable.Text = "Act. Time:";
            this.ActTimeLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TypeLable
            // 
            this.TypeLable.Dpi = 100F;
            this.TypeLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 167F);
            this.TypeLable.Name = "TypeLable";
            this.TypeLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TypeLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.TypeLable.StyleName = "FieldCaption";
            this.TypeLable.StylePriority.UseTextAlignment = false;
            this.TypeLable.Text = "Type:";
            this.TypeLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // WorkOpLable
            // 
            this.WorkOpLable.Dpi = 100F;
            this.WorkOpLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 143.9999F);
            this.WorkOpLable.Name = "WorkOpLable";
            this.WorkOpLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.WorkOpLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.WorkOpLable.StyleName = "FieldCaption";
            this.WorkOpLable.StylePriority.UseTextAlignment = false;
            this.WorkOpLable.Text = "Work Op:";
            this.WorkOpLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // FundSourceLable
            // 
            this.FundSourceLable.Dpi = 100F;
            this.FundSourceLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 120.9999F);
            this.FundSourceLable.Name = "FundSourceLable";
            this.FundSourceLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.FundSourceLable.SizeF = new System.Drawing.SizeF(83.33331F, 22.99998F);
            this.FundSourceLable.StyleName = "FieldCaption";
            this.FundSourceLable.StylePriority.UseTextAlignment = false;
            this.FundSourceLable.Text = "Fund Source:";
            this.FundSourceLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // StatusLable
            // 
            this.StatusLable.Dpi = 100F;
            this.StatusLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 97.99992F);
            this.StatusLable.Name = "StatusLable";
            this.StatusLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.StatusLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.StatusLable.StyleName = "FieldCaption";
            this.StatusLable.StylePriority.UseTextAlignment = false;
            this.StatusLable.Text = "Status:";
            this.StatusLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // SupervisorLable
            // 
            this.SupervisorLable.Dpi = 100F;
            this.SupervisorLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 74.99991F);
            this.SupervisorLable.Name = "SupervisorLable";
            this.SupervisorLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.SupervisorLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.SupervisorLable.StyleName = "FieldCaption";
            this.SupervisorLable.StylePriority.UseTextAlignment = false;
            this.SupervisorLable.Text = "Supervisor:";
            this.SupervisorLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ShiftLable
            // 
            this.ShiftLable.Dpi = 100F;
            this.ShiftLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 51.9999F);
            this.ShiftLable.Name = "ShiftLable";
            this.ShiftLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ShiftLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.ShiftLable.StyleName = "FieldCaption";
            this.ShiftLable.StylePriority.UseTextAlignment = false;
            this.ShiftLable.Text = "Shift:";
            this.ShiftLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // JobSummaryLable
            // 
            this.JobSummaryLable.Dpi = 100F;
            this.JobSummaryLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 204.2084F);
            this.JobSummaryLable.Name = "JobSummaryLable";
            this.JobSummaryLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.JobSummaryLable.SizeF = new System.Drawing.SizeF(93.99996F, 23F);
            this.JobSummaryLable.StyleName = "FieldCaption";
            this.JobSummaryLable.StylePriority.UseTextAlignment = false;
            this.JobSummaryLable.Text = "Job Summary:";
            this.JobSummaryLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // LocationDescLable
            // 
            this.LocationDescLable.Dpi = 100F;
            this.LocationDescLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 166.9998F);
            this.LocationDescLable.Name = "LocationDescLable";
            this.LocationDescLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LocationDescLable.SizeF = new System.Drawing.SizeF(93.99996F, 23F);
            this.LocationDescLable.StyleName = "FieldCaption";
            this.LocationDescLable.StylePriority.UseTextAlignment = false;
            this.LocationDescLable.Text = "Location Desc:";
            this.LocationDescLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // LocattionLable
            // 
            this.LocattionLable.Dpi = 100F;
            this.LocattionLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 143.9999F);
            this.LocattionLable.Name = "LocattionLable";
            this.LocattionLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LocattionLable.SizeF = new System.Drawing.SizeF(93.99996F, 23F);
            this.LocattionLable.StyleName = "FieldCaption";
            this.LocattionLable.StylePriority.UseTextAlignment = false;
            this.LocattionLable.Text = "Location:";
            this.LocattionLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReasonLable
            // 
            this.ReasonLable.Dpi = 100F;
            this.ReasonLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 166.9999F);
            this.ReasonLable.Name = "ReasonLable";
            this.ReasonLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ReasonLable.SizeF = new System.Drawing.SizeF(84.16672F, 23.00003F);
            this.ReasonLable.StyleName = "FieldCaption";
            this.ReasonLable.StylePriority.UseTextAlignment = false;
            this.ReasonLable.Text = "Reason:";
            this.ReasonLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // EstTimeLable
            // 
            this.EstTimeLable.Dpi = 100F;
            this.EstTimeLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 120.9999F);
            this.EstTimeLable.Name = "EstTimeLable";
            this.EstTimeLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstTimeLable.SizeF = new System.Drawing.SizeF(84.16672F, 23.00002F);
            this.EstTimeLable.StyleName = "FieldCaption";
            this.EstTimeLable.StylePriority.UseTextAlignment = false;
            this.EstTimeLable.Text = "Est. Time:";
            this.EstTimeLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // RtnWithinLable
            // 
            this.RtnWithinLable.Dpi = 100F;
            this.RtnWithinLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 97.99995F);
            this.RtnWithinLable.Name = "RtnWithinLable";
            this.RtnWithinLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.RtnWithinLable.SizeF = new System.Drawing.SizeF(84.16672F, 23F);
            this.RtnWithinLable.StyleName = "FieldCaption";
            this.RtnWithinLable.StylePriority.UseTextAlignment = false;
            this.RtnWithinLable.Text = "Rtn Within:";
            this.RtnWithinLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // StartDateLable
            // 
            this.StartDateLable.Dpi = 100F;
            this.StartDateLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 74.99995F);
            this.StartDateLable.Name = "StartDateLable";
            this.StartDateLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.StartDateLable.SizeF = new System.Drawing.SizeF(86.45831F, 23.00001F);
            this.StartDateLable.StyleName = "FieldCaption";
            this.StartDateLable.StylePriority.UseTextAlignment = false;
            this.StartDateLable.Text = "Start Date:";
            this.StartDateLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // RequestDateLable
            // 
            this.RequestDateLable.Dpi = 100F;
            this.RequestDateLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 51.99994F);
            this.RequestDateLable.Name = "RequestDateLable";
            this.RequestDateLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.RequestDateLable.SizeF = new System.Drawing.SizeF(86.45834F, 23F);
            this.RequestDateLable.StyleName = "FieldCaption";
            this.RequestDateLable.StylePriority.UseTextAlignment = false;
            this.RequestDateLable.Text = "Request Date:";
            this.RequestDateLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // AreaDescLable
            // 
            this.AreaDescLable.Dpi = 100F;
            this.AreaDescLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 97.99995F);
            this.AreaDescLable.Name = "AreaDescLable";
            this.AreaDescLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AreaDescLable.SizeF = new System.Drawing.SizeF(93.99996F, 23F);
            this.AreaDescLable.StyleName = "FieldCaption";
            this.AreaDescLable.StylePriority.UseTextAlignment = false;
            this.AreaDescLable.Text = "Area Desc:";
            this.AreaDescLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // cStatusID
            // 
            this.cStatusID.CanGrow = false;
            this.cStatusID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.cStatusID")});
            this.cStatusID.Dpi = 100F;
            this.cStatusID.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 97.99992F);
            this.cStatusID.Multiline = true;
            this.cStatusID.Name = "cStatusID";
            this.cStatusID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cStatusID.SizeF = new System.Drawing.SizeF(201.4584F, 23F);
            this.cStatusID.StyleName = "DataField";
            // 
            // cSupervisorName
            // 
            this.cSupervisorName.CanGrow = false;
            this.cSupervisorName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.cSupervisorName")});
            this.cSupervisorName.Dpi = 100F;
            this.cSupervisorName.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 74.99991F);
            this.cSupervisorName.Multiline = true;
            this.cSupervisorName.Name = "cSupervisorName";
            this.cSupervisorName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cSupervisorName.SizeF = new System.Drawing.SizeF(201.4584F, 23.00001F);
            this.cSupervisorName.StyleName = "DataField";
            // 
            // cShiftID
            // 
            this.cShiftID.CanGrow = false;
            this.cShiftID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.cShiftID")});
            this.cShiftID.Dpi = 100F;
            this.cShiftID.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 48.99991F);
            this.cShiftID.Multiline = true;
            this.cShiftID.Name = "cShiftID";
            this.cShiftID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cShiftID.SizeF = new System.Drawing.SizeF(201.4584F, 23F);
            this.cShiftID.StyleName = "DataField";
            // 
            // GroupDesc
            // 
            this.GroupDesc.CanGrow = false;
            this.GroupDesc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.GroupDesc")});
            this.GroupDesc.Dpi = 100F;
            this.GroupDesc.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 2.999973F);
            this.GroupDesc.Multiline = true;
            this.GroupDesc.Name = "GroupDesc";
            this.GroupDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.GroupDesc.SizeF = new System.Drawing.SizeF(201.4584F, 23F);
            this.GroupDesc.StyleName = "DataField";
            this.GroupDesc.Text = "GroupDesc";
            // 
            // cReasoncodeID
            // 
            this.cReasoncodeID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.cReasoncodeID")});
            this.cReasoncodeID.Dpi = 100F;
            this.cReasoncodeID.LocationFloat = new DevExpress.Utils.PointFloat(386.1664F, 166.9999F);
            this.cReasoncodeID.Name = "cReasoncodeID";
            this.cReasoncodeID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cReasoncodeID.SizeF = new System.Drawing.SizeF(123.9583F, 23F);
            this.cReasoncodeID.StyleName = "DataField";
            // 
            // actualdowntime
            // 
            this.actualdowntime.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.actualdowntime")});
            this.actualdowntime.Dpi = 100F;
            this.actualdowntime.LocationFloat = new DevExpress.Utils.PointFloat(388.458F, 143.9999F);
            this.actualdowntime.Name = "actualdowntime";
            this.actualdowntime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.actualdowntime.SizeF = new System.Drawing.SizeF(121.6666F, 23F);
            this.actualdowntime.StyleName = "DataField";
            this.actualdowntime.Text = "actualdowntime";
            // 
            // estimateddowntime
            // 
            this.estimateddowntime.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.estimateddowntime")});
            this.estimateddowntime.Dpi = 100F;
            this.estimateddowntime.LocationFloat = new DevExpress.Utils.PointFloat(387.4163F, 120.9999F);
            this.estimateddowntime.Name = "estimateddowntime";
            this.estimateddowntime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.estimateddowntime.SizeF = new System.Drawing.SizeF(122.7083F, 23.00001F);
            this.estimateddowntime.StyleName = "DataField";
            this.estimateddowntime.Text = "estimateddowntime";
            // 
            // return_within
            // 
            this.return_within.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.return_within")});
            this.return_within.Dpi = 100F;
            this.return_within.LocationFloat = new DevExpress.Utils.PointFloat(387.4163F, 97.99989F);
            this.return_within.Name = "return_within";
            this.return_within.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.return_within.SizeF = new System.Drawing.SizeF(122.7083F, 23.00003F);
            this.return_within.StyleName = "DataField";
            // 
            // TaskID
            // 
            this.TaskID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.TaskID")});
            this.TaskID.Dpi = 100F;
            this.TaskID.LocationFloat = new DevExpress.Utils.PointFloat(388.458F, 2.999957F);
            this.TaskID.Name = "TaskID";
            this.TaskID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TaskID.SizeF = new System.Drawing.SizeF(121.6666F, 23F);
            this.TaskID.StyleName = "DataField";
            this.TaskID.Text = "TaskID";
            // 
            // JobstepNotes
            // 
            this.JobstepNotes.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.JobstepNotes")});
            this.JobstepNotes.Dpi = 100F;
            this.JobstepNotes.LocationFloat = new DevExpress.Utils.PointFloat(0F, 433.3333F);
            this.JobstepNotes.Name = "JobstepNotes";
            this.JobstepNotes.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.JobstepNotes.SizeF = new System.Drawing.SizeF(438F, 23F);
            this.JobstepNotes.StyleName = "DataField";
            this.JobstepNotes.Text = "JobstepNotes";
            // 
            // LocationDesc
            // 
            this.LocationDesc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.LocationDesc")});
            this.LocationDesc.Dpi = 100F;
            this.LocationDesc.LocationFloat = new DevExpress.Utils.PointFloat(95.24994F, 166.9998F);
            this.LocationDesc.Name = "LocationDesc";
            this.LocationDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LocationDesc.SizeF = new System.Drawing.SizeF(206.75F, 23F);
            this.LocationDesc.StyleName = "DataField";
            this.LocationDesc.Text = "LocationDesc";
            // 
            // LacationSrc
            // 
            this.LacationSrc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.Location")});
            this.LacationSrc.Dpi = 100F;
            this.LacationSrc.LocationFloat = new DevExpress.Utils.PointFloat(95.24994F, 143.9999F);
            this.LacationSrc.Name = "LacationSrc";
            this.LacationSrc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LacationSrc.SizeF = new System.Drawing.SizeF(206.75F, 23F);
            this.LacationSrc.StyleName = "DataField";
            this.LacationSrc.Text = "LacationSrc";
            // 
            // AssignedAreaDesc
            // 
            this.AssignedAreaDesc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.AssignedAreaDesc")});
            this.AssignedAreaDesc.Dpi = 100F;
            this.AssignedAreaDesc.LocationFloat = new DevExpress.Utils.PointFloat(93.99999F, 97.99989F);
            this.AssignedAreaDesc.Name = "AssignedAreaDesc";
            this.AssignedAreaDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AssignedAreaDesc.SizeF = new System.Drawing.SizeF(207.9997F, 22.99999F);
            this.AssignedAreaDesc.StyleName = "DataField";
            this.AssignedAreaDesc.Text = "AssignedAreaDesc";
            // 
            // AssignedArea
            // 
            this.AssignedArea.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.AssignedArea")});
            this.AssignedArea.Dpi = 100F;
            this.AssignedArea.LocationFloat = new DevExpress.Utils.PointFloat(93.99999F, 74.99994F);
            this.AssignedArea.Name = "AssignedArea";
            this.AssignedArea.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AssignedArea.SizeF = new System.Drawing.SizeF(207.9999F, 23F);
            this.AssignedArea.StyleName = "DataField";
            this.AssignedArea.Text = "AssignedArea";
            // 
            // AreaLable
            // 
            this.AreaLable.Dpi = 100F;
            this.AreaLable.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreaLable.ForeColor = System.Drawing.Color.Navy;
            this.AreaLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 74.99994F);
            this.AreaLable.Name = "AreaLable";
            this.AreaLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AreaLable.SizeF = new System.Drawing.SizeF(93.99996F, 23F);
            this.AreaLable.StyleName = "FieldCaption";
            this.AreaLable.StylePriority.UseFont = false;
            this.AreaLable.StylePriority.UseForeColor = false;
            this.AreaLable.StylePriority.UseTextAlignment = false;
            this.AreaLable.Text = "Area:";
            this.AreaLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PriorityLable
            // 
            this.PriorityLable.Dpi = 100F;
            this.PriorityLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 28.99992F);
            this.PriorityLable.Name = "PriorityLable";
            this.PriorityLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PriorityLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.PriorityLable.StyleName = "FieldCaption";
            this.PriorityLable.StylePriority.UseTextAlignment = false;
            this.PriorityLable.Text = "Priority:";
            this.PriorityLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupLable
            // 
            this.GroupLable.Dpi = 100F;
            this.GroupLable.LocationFloat = new DevExpress.Utils.PointFloat(512.2083F, 2.999973F);
            this.GroupLable.Name = "GroupLable";
            this.GroupLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.GroupLable.SizeF = new System.Drawing.SizeF(83.33331F, 23F);
            this.GroupLable.StyleName = "FieldCaption";
            this.GroupLable.StylePriority.UseTextAlignment = false;
            this.GroupLable.Text = "Group:";
            this.GroupLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // RequestorLable
            // 
            this.RequestorLable.Dpi = 100F;
            this.RequestorLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 28.99992F);
            this.RequestorLable.Name = "RequestorLable";
            this.RequestorLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.RequestorLable.SizeF = new System.Drawing.SizeF(86.45834F, 23F);
            this.RequestorLable.StyleName = "FieldCaption";
            this.RequestorLable.StylePriority.UseTextAlignment = false;
            this.RequestorLable.Text = "Requestor:";
            this.RequestorLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TaskIDLable
            // 
            this.TaskIDLable.Dpi = 100F;
            this.TaskIDLable.LocationFloat = new DevExpress.Utils.PointFloat(301.9997F, 2.999973F);
            this.TaskIDLable.Name = "TaskIDLable";
            this.TaskIDLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TaskIDLable.SizeF = new System.Drawing.SizeF(86.45834F, 23F);
            this.TaskIDLable.StyleName = "FieldCaption";
            this.TaskIDLable.StylePriority.UseTextAlignment = false;
            this.TaskIDLable.Text = "Task ID:";
            this.TaskIDLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ObjectDesc
            // 
            this.ObjectDesc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.ObjectDesc")});
            this.ObjectDesc.Dpi = 100F;
            this.ObjectDesc.LocationFloat = new DevExpress.Utils.PointFloat(93.99986F, 28.99992F);
            this.ObjectDesc.Name = "ObjectDesc";
            this.ObjectDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ObjectDesc.SizeF = new System.Drawing.SizeF(207.9999F, 23F);
            this.ObjectDesc.StyleName = "DataField";
            this.ObjectDesc.Text = "ObjectDesc";
            // 
            // PriorityDesc
            // 
            this.PriorityDesc.CanGrow = false;
            this.PriorityDesc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.PriorityDesc")});
            this.PriorityDesc.Dpi = 100F;
            this.PriorityDesc.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 25.99991F);
            this.PriorityDesc.Multiline = true;
            this.PriorityDesc.Name = "PriorityDesc";
            this.PriorityDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PriorityDesc.SizeF = new System.Drawing.SizeF(201.4584F, 23F);
            this.PriorityDesc.StyleName = "DataField";
            // 
            // RequestDate
            // 
            this.RequestDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.RequestDate")});
            this.RequestDate.Dpi = 100F;
            this.RequestDate.LocationFloat = new DevExpress.Utils.PointFloat(388.458F, 51.99992F);
            this.RequestDate.Name = "RequestDate";
            this.RequestDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.RequestDate.SizeF = new System.Drawing.SizeF(121.6666F, 23F);
            this.RequestDate.StyleName = "DataField";
            this.RequestDate.Text = "RequestDate";
            // 
            // Requestor
            // 
            this.Requestor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.Requestor")});
            this.Requestor.Dpi = 100F;
            this.Requestor.LocationFloat = new DevExpress.Utils.PointFloat(388.458F, 28.99992F);
            this.Requestor.Name = "Requestor";
            this.Requestor.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Requestor.SizeF = new System.Drawing.SizeF(121.6666F, 23F);
            this.Requestor.StyleName = "DataField";
            this.Requestor.Text = "Requestor";
            // 
            // FundSrcCodeDesc
            // 
            this.FundSrcCodeDesc.CanGrow = false;
            this.FundSrcCodeDesc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.FundSrcCodeDesc")});
            this.FundSrcCodeDesc.Dpi = 100F;
            this.FundSrcCodeDesc.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 120.9999F);
            this.FundSrcCodeDesc.Multiline = true;
            this.FundSrcCodeDesc.Name = "FundSrcCodeDesc";
            this.FundSrcCodeDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.FundSrcCodeDesc.SizeF = new System.Drawing.SizeF(201.4584F, 22.99998F);
            this.FundSrcCodeDesc.StyleName = "DataField";
            // 
            // WorkOrderCodeID
            // 
            this.WorkOrderCodeID.CanGrow = false;
            this.WorkOrderCodeID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.WorkOrderCodeID")});
            this.WorkOrderCodeID.Dpi = 100F;
            this.WorkOrderCodeID.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 143.9999F);
            this.WorkOrderCodeID.Multiline = true;
            this.WorkOrderCodeID.Name = "WorkOrderCodeID";
            this.WorkOrderCodeID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.WorkOrderCodeID.SizeF = new System.Drawing.SizeF(201.4584F, 22.99998F);
            this.WorkOrderCodeID.StyleName = "DataField";
            // 
            // TypeOfJob
            // 
            this.TypeOfJob.CanGrow = false;
            this.TypeOfJob.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.TypeOfJob")});
            this.TypeOfJob.Dpi = 100F;
            this.TypeOfJob.LocationFloat = new DevExpress.Utils.PointFloat(595.5416F, 167F);
            this.TypeOfJob.Multiline = true;
            this.TypeOfJob.Name = "TypeOfJob";
            this.TypeOfJob.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TypeOfJob.SizeF = new System.Drawing.SizeF(201.4584F, 23F);
            this.TypeOfJob.StyleName = "DataField";
            this.TypeOfJob.Text = "TypeOfJob";
            // 
            // ObjectIDLable
            // 
            this.ObjectIDLable.Dpi = 100F;
            this.ObjectIDLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.999973F);
            this.ObjectIDLable.Name = "ObjectIDLable";
            this.ObjectIDLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ObjectIDLable.SizeF = new System.Drawing.SizeF(93.99994F, 23F);
            this.ObjectIDLable.StyleName = "FieldCaption";
            this.ObjectIDLable.StylePriority.UseTextAlignment = false;
            this.ObjectIDLable.Text = "Object ID:";
            this.ObjectIDLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ObjectDescLable
            // 
            this.ObjectDescLable.Dpi = 100F;
            this.ObjectDescLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28.99993F);
            this.ObjectDescLable.Name = "ObjectDescLable";
            this.ObjectDescLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ObjectDescLable.SizeF = new System.Drawing.SizeF(93.99994F, 22.99998F);
            this.ObjectDescLable.StyleName = "FieldCaption";
            this.ObjectDescLable.StylePriority.UseTextAlignment = false;
            this.ObjectDescLable.Text = "Object Desc:";
            this.ObjectDescLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ObjectID
            // 
            this.ObjectID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.ObjectID")});
            this.ObjectID.Dpi = 100F;
            this.ObjectID.LocationFloat = new DevExpress.Utils.PointFloat(93.99999F, 2.999957F);
            this.ObjectID.Name = "ObjectID";
            this.ObjectID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ObjectID.SizeF = new System.Drawing.SizeF(207.9999F, 23.00002F);
            this.ObjectID.StyleName = "DataField";
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 46F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ClientConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "GetSimpleJobStep";
            queryParameter1.Name = "@JobID";
            queryParameter1.Type = typeof(int);
            queryParameter1.ValueInfo = "0";
        queryParameter1.Value = jobID;
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.StoredProcName = "GetSimpleJobStep";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            // 
            // pageFooterBand1
            // 
            this.pageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine3,
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.pageFooterBand1.Dpi = 100F;
            this.pageFooterBand1.HeightF = 57.12503F;
            this.pageFooterBand1.Name = "pageFooterBand1";
            this.pageFooterBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 5, 0, 100F);
            this.pageFooterBand1.StylePriority.UsePadding = false;
            // 
            // xrLine3
            // 
            this.xrLine3.Dpi = 100F;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.791641F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(796.9999F, 5.208336F);
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 100F;
            this.xrPageInfo1.ForeColor = System.Drawing.Color.Black;
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 16.83337F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(99.99995F, 23F);
            this.xrPageInfo1.StyleName = "PageInfo";
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Dpi = 100F;
            this.xrPageInfo2.ForeColor = System.Drawing.Color.Black;
            this.xrPageInfo2.Format = "Page {0} of {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(703.125F, 16.83337F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(93.87488F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.cJobID,
            this.xrLabel50,
            this.xrLine2,
            this.xrLabel5});
            this.reportHeaderBand1.Dpi = 100F;
            this.reportHeaderBand1.HeightF = 103.0833F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // cJobID
            // 
            this.cJobID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJobStep.cJobID")});
            this.cJobID.Dpi = 100F;
            this.cJobID.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cJobID.ForeColor = System.Drawing.Color.Black;
            this.cJobID.LocationFloat = new DevExpress.Utils.PointFloat(449.2083F, 54.08332F);
            this.cJobID.Name = "cJobID";
            this.cJobID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cJobID.SizeF = new System.Drawing.SizeF(146.3333F, 23F);
            this.cJobID.StylePriority.UseFont = false;
            this.cJobID.StylePriority.UseForeColor = false;
            this.cJobID.Text = "cJobID";
            // 
            // xrLabel50
            // 
            this.xrLabel50.Dpi = 100F;
            this.xrLabel50.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel50.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(251.0417F, 54.08332F);
            this.xrLabel50.Name = "xrLabel50";
            this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel50.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel50.StyleName = "FieldCaption";
            this.xrLabel50.StylePriority.UseFont = false;
            this.xrLabel50.StylePriority.UseForeColor = false;
            this.xrLabel50.Text = "Job ID:";
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 100F;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.87498F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(796.9999F, 5.208336F);
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 100F;
            this.xrLabel5.Font = new System.Drawing.Font("Verdana", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.000002F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(797F, 33F);
            this.xrLabel5.StyleName = "Title";
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "M-PET Work Order";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Times New Roman", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Title.ForeColor = System.Drawing.Color.Navy;
            this.Title.Name = "Title";
            // 
            // FieldCaption
            // 
            this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
            this.FieldCaption.BorderColor = System.Drawing.Color.Black;
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.FieldCaption.BorderWidth = 1F;
            this.FieldCaption.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FieldCaption.ForeColor = System.Drawing.Color.Navy;
            this.FieldCaption.Name = "FieldCaption";
            // 
            // PageInfo
            // 
            this.PageInfo.BackColor = System.Drawing.Color.Transparent;
            this.PageInfo.BorderColor = System.Drawing.Color.Black;
            this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PageInfo.BorderWidth = 1F;
            this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.PageInfo.ForeColor = System.Drawing.Color.Navy;
            this.PageInfo.Name = "PageInfo";
            // 
            // DataField
            // 
            this.DataField.BackColor = System.Drawing.Color.Transparent;
            this.DataField.BorderColor = System.Drawing.Color.Black;
            this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DataField.BorderWidth = 1F;
            this.DataField.Font = new System.Drawing.Font("Arial", 8F);
            this.DataField.ForeColor = System.Drawing.Color.Black;
            this.DataField.Name = "DataField";
            this.DataField.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // parameterJobID
            // 
            this.parameterJobID.Name = "parameterJobID";
            this.parameterJobID.Type = typeof(int);
            this.parameterJobID.ValueInfo = "0";
            this.parameterJobID.Visible = false;
            // 
            // JobReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageFooterBand1,
            this.reportHeaderBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "GetSimpleJobStep";
            this.DataSource = this.sqlDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(25, 28, 46, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.parameterJobID});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
