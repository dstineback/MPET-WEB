using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;
using DevExpress.XtraReports.Parameters;
using System.Data.SqlClient;

/// <summary>
/// Summary description for workRequestReport
/// </summary>
public class workRequestReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRLabel cAreaIDLable;
    private XRLabel cgroupLable;
    private XRLabel cjobreasonidLable;
    private XRLabel cMaintObjectIDLable;
    private XRLabel cOutcomeCodeLable;
    private XRLabel RequestDateLable;
    private XRLabel crequestPriorityLable;
    private XRLabel cRouteToLable;
    private XRLabel fundSrcCodeIDLable;
    private XRLabel GPS_XLable;
    private XRLabel GPS_YLable;
    private XRLabel JobidLable;
    private XRLabel nMaintObjectIDLable;
    private XRLabel NotesLable;
    private XRLabel TitleLable;
    private XRLabel cAreaID;
    private XRLabel cgroup;
    private XRLabel xrLabel19;
    private XRLabel cMaintObjectID;
    private XRLabel cOutComecode;
    private XRLabel xrLabel22;
    private XRLabel xrLabel23;
    private XRLabel xrLabel24;
    private XRLabel cTypeOfJob;
    private XRLabel FundSrcCodeID;
    private XRLabel GPS_X;
    private XRLabel GPS_Y;
    private XRLabel jobidsrc;
    private XRLabel n_MaintObjectCodeID;
    private XRLabel xrLabel31;
    private XRLabel xrLabel32;
    private XRLine xrLine1;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private PageFooterBand pageFooterBand1;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand reportHeaderBand1;
    private XRLabel xrLabel33;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private XRLabel xrLabel37;
    private XRLabel xrLabel36;
    private XRLabel xrLabel35;
    private XRLabel xrLabel34;
    private int reportParm = 0;
    private XRLabel WorkOrderCodeID;
   
    private XRLabel xrLabel39;
    private Parameter param;
    private Parameter jobid;
    private XRLabel CellNumberLable;
    private XRLabel WorkNumberLable;
    private XRLabel LastNameLable;
    private XRLabel firstNameLable;
    private XRLine xrLine2;
    private XRLabel ObjectDescLable;
    private XRLabel xrLabel1;
    private XRLine xrLine3;


    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    
    
    public workRequestReport()
    {
        GetParam();
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
            string resourceFileName = "workRequestReport.resx";
            System.Resources.ResourceManager resources = global::Resources.workRequestReport.ResourceManager;
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery2 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.param = new DevExpress.XtraReports.Parameters.Parameter();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.ObjectDescLable = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.CellNumberLable = new DevExpress.XtraReports.UI.XRLabel();
            this.WorkNumberLable = new DevExpress.XtraReports.UI.XRLabel();
            this.LastNameLable = new DevExpress.XtraReports.UI.XRLabel();
            this.firstNameLable = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.WorkOrderCodeID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.cAreaIDLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cgroupLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cjobreasonidLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cMaintObjectIDLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cOutcomeCodeLable = new DevExpress.XtraReports.UI.XRLabel();
            this.RequestDateLable = new DevExpress.XtraReports.UI.XRLabel();
            this.crequestPriorityLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cRouteToLable = new DevExpress.XtraReports.UI.XRLabel();
            this.fundSrcCodeIDLable = new DevExpress.XtraReports.UI.XRLabel();
            this.GPS_XLable = new DevExpress.XtraReports.UI.XRLabel();
            this.GPS_YLable = new DevExpress.XtraReports.UI.XRLabel();
            this.nMaintObjectIDLable = new DevExpress.XtraReports.UI.XRLabel();
            this.NotesLable = new DevExpress.XtraReports.UI.XRLabel();
            this.TitleLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cAreaID = new DevExpress.XtraReports.UI.XRLabel();
            this.cgroup = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.cMaintObjectID = new DevExpress.XtraReports.UI.XRLabel();
            this.cOutComecode = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.FundSrcCodeID = new DevExpress.XtraReports.UI.XRLabel();
            this.GPS_X = new DevExpress.XtraReports.UI.XRLabel();
            this.GPS_Y = new DevExpress.XtraReports.UI.XRLabel();
            this.n_MaintObjectCodeID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.JobidLable = new DevExpress.XtraReports.UI.XRLabel();
            this.cTypeOfJob = new DevExpress.XtraReports.UI.XRLabel();
            this.jobidsrc = new DevExpress.XtraReports.UI.XRLabel();
            this.pageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.jobid = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ClientConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "GetSimpleJob";
            queryParameter1.Name = "@RequestJobID";
            queryParameter1.Type = typeof(int);
            queryParameter1.ValueInfo = "0";
        queryParameter1.Value = reportParm;
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.StoredProcName = "GetSimpleJob";
            customSqlQuery1.Name = "Requestor";
            queryParameter2.Name = "@jobID";
            queryParameter2.Type = typeof(int);
            queryParameter2.ValueInfo = "0";
        queryParameter2.Value = reportParm;
            customSqlQuery1.Parameters.Add(queryParameter2);
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            customSqlQuery2.Name = "Object";
            queryParameter3.Name = "@jobID";
            queryParameter3.Type = typeof(int);
            queryParameter3.ValueInfo = "0";
        queryParameter3.Value = reportParm;
            customSqlQuery2.Parameters.Add(queryParameter3);
            customSqlQuery2.Sql = "SELECT \r\n\t[description]\r\n     \r\n     from .dbo.MaintenanceObjects as M inner join" +
    " dbo.Jobs AS J \r\n  \r\n  on M.n_objectid = J.n_MaintObjectID\r\n  where J.n_Jobid = " +
    "@jobID;";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1,
            customSqlQuery1,
            customSqlQuery2});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // param
            // 
            this.param.Description = "param";
            this.param.Name = "param";
            this.param.Type = typeof(int);
            this.param.ValueInfo = "499796";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine3,
            this.ObjectDescLable,
            this.xrLabel1,
            this.xrLine2,
            this.CellNumberLable,
            this.WorkNumberLable,
            this.LastNameLable,
            this.firstNameLable,
            this.xrLabel39,
            this.WorkOrderCodeID,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel35,
            this.xrLabel34,
            this.cAreaIDLable,
            this.cgroupLable,
            this.cjobreasonidLable,
            this.cMaintObjectIDLable,
            this.cOutcomeCodeLable,
            this.RequestDateLable,
            this.crequestPriorityLable,
            this.cRouteToLable,
            this.fundSrcCodeIDLable,
            this.GPS_XLable,
            this.GPS_YLable,
            this.nMaintObjectIDLable,
            this.NotesLable,
            this.TitleLable,
            this.cAreaID,
            this.cgroup,
            this.xrLabel19,
            this.cMaintObjectID,
            this.cOutComecode,
            this.xrLabel22,
            this.xrLabel23,
            this.xrLabel24,
            this.FundSrcCodeID,
            this.GPS_X,
            this.GPS_Y,
            this.n_MaintObjectCodeID,
            this.xrLabel31,
            this.xrLabel32,
            this.xrLine1});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 540.625F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine3
            // 
            this.xrLine3.Dpi = 100F;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 523.9583F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(745.75F, 4.166565F);
            // 
            // ObjectDescLable
            // 
            this.ObjectDescLable.Dpi = 100F;
            this.ObjectDescLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 30.00002F);
            this.ObjectDescLable.Name = "ObjectDescLable";
            this.ObjectDescLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ObjectDescLable.SizeF = new System.Drawing.SizeF(122.4167F, 23F);
            this.ObjectDescLable.StyleName = "FieldCaption";
            this.ObjectDescLable.Text = "Description";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Object.description")});
            this.xrLabel1.Dpi = 100F;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 30.00002F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(238.75F, 23F);
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 100F;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(1.999982F, 373.9583F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(746.875F, 2.083313F);
            // 
            // CellNumberLable
            // 
            this.CellNumberLable.Dpi = 100F;
            this.CellNumberLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0835F, 121F);
            this.CellNumberLable.Name = "CellNumberLable";
            this.CellNumberLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.CellNumberLable.SizeF = new System.Drawing.SizeF(122.4167F, 22.99999F);
            this.CellNumberLable.StyleName = "FieldCaption";
            this.CellNumberLable.Text = "Cell Number #:";
            // 
            // WorkNumberLable
            // 
            this.WorkNumberLable.Dpi = 100F;
            this.WorkNumberLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0835F, 98.00002F);
            this.WorkNumberLable.Name = "WorkNumberLable";
            this.WorkNumberLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.WorkNumberLable.SizeF = new System.Drawing.SizeF(122.4167F, 23F);
            this.WorkNumberLable.StyleName = "FieldCaption";
            this.WorkNumberLable.Text = "Work Phone #";
            // 
            // LastNameLable
            // 
            this.LastNameLable.Dpi = 100F;
            this.LastNameLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0835F, 75F);
            this.LastNameLable.Name = "LastNameLable";
            this.LastNameLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LastNameLable.SizeF = new System.Drawing.SizeF(122.4167F, 23F);
            this.LastNameLable.StyleName = "FieldCaption";
            this.LastNameLable.Text = "Requestor Last";
            // 
            // firstNameLable
            // 
            this.firstNameLable.Dpi = 100F;
            this.firstNameLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0835F, 51.99998F);
            this.firstNameLable.Name = "firstNameLable";
            this.firstNameLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.firstNameLable.SizeF = new System.Drawing.SizeF(122.4167F, 23F);
            this.firstNameLable.StyleName = "FieldCaption";
            this.firstNameLable.Text = "Requestor First";
            // 
            // xrLabel39
            // 
            this.xrLabel39.Dpi = 100F;
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(0F, 199.9584F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(122.4167F, 23F);
            this.xrLabel39.StyleName = "FieldCaption";
            this.xrLabel39.Text = "Work Order Code";
            // 
            // WorkOrderCodeID
            // 
            this.WorkOrderCodeID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.WorkOrderCodeID")});
            this.WorkOrderCodeID.Dpi = 100F;
            this.WorkOrderCodeID.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 199.9584F);
            this.WorkOrderCodeID.Name = "WorkOrderCodeID";
            this.WorkOrderCodeID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.WorkOrderCodeID.SizeF = new System.Drawing.SizeF(232.75F, 23F);
            this.WorkOrderCodeID.StyleName = "DataField";
            this.WorkOrderCodeID.Text = "WorkOrderCodeID";
            // 
            // xrLabel37
            // 
            this.xrLabel37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Requestor.CellPhone")});
            this.xrLabel37.Dpi = 100F;
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 121F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(201.4998F, 22.99999F);
            this.xrLabel37.StyleName = "DataField";
            // 
            // xrLabel36
            // 
            this.xrLabel36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Requestor.WorkPhone")});
            this.xrLabel36.Dpi = 100F;
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 97.99998F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(201.5003F, 23F);
            this.xrLabel36.StyleName = "DataField";
            // 
            // xrLabel35
            // 
            this.xrLabel35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Requestor.LastName")});
            this.xrLabel35.Dpi = 100F;
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 75F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(201.4996F, 23F);
            this.xrLabel35.StyleName = "DataField";
            // 
            // xrLabel34
            // 
            this.xrLabel34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Requestor.FirstName")});
            this.xrLabel34.Dpi = 100F;
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 51.99998F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(201.5003F, 23F);
            this.xrLabel34.StyleName = "DataField";
            // 
            // cAreaIDLable
            // 
            this.cAreaIDLable.Dpi = 100F;
            this.cAreaIDLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 101F);
            this.cAreaIDLable.Name = "cAreaIDLable";
            this.cAreaIDLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cAreaIDLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.cAreaIDLable.StyleName = "FieldCaption";
            this.cAreaIDLable.Text = "Area";
            // 
            // cgroupLable
            // 
            this.cgroupLable.Dpi = 100F;
            this.cgroupLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 80.99999F);
            this.cgroupLable.Name = "cgroupLable";
            this.cgroupLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cgroupLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.cgroupLable.StyleName = "FieldCaption";
            this.cgroupLable.Text = "System Group";
            // 
            // cjobreasonidLable
            // 
            this.cjobreasonidLable.Dpi = 100F;
            this.cjobreasonidLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0833F, 289F);
            this.cjobreasonidLable.Name = "cjobreasonidLable";
            this.cjobreasonidLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cjobreasonidLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.cjobreasonidLable.StyleName = "FieldCaption";
            this.cjobreasonidLable.Text = "Reason Code";
            // 
            // cMaintObjectIDLable
            // 
            this.cMaintObjectIDLable.Dpi = 100F;
            this.cMaintObjectIDLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999998F);
            this.cMaintObjectIDLable.Name = "cMaintObjectIDLable";
            this.cMaintObjectIDLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cMaintObjectIDLable.SizeF = new System.Drawing.SizeF(122.4167F, 20.00001F);
            this.cMaintObjectIDLable.StyleName = "FieldCaption";
            this.cMaintObjectIDLable.Text = "Maint Object ID";
            // 
            // cOutcomeCodeLable
            // 
            this.cOutcomeCodeLable.Dpi = 100F;
            this.cOutcomeCodeLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 327.875F);
            this.cOutcomeCodeLable.Name = "cOutcomeCodeLable";
            this.cOutcomeCodeLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cOutcomeCodeLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.cOutcomeCodeLable.StyleName = "FieldCaption";
            this.cOutcomeCodeLable.Text = "Outcome ID";
            // 
            // RequestDateLable
            // 
            this.RequestDateLable.Dpi = 100F;
            this.RequestDateLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0833F, 9.000015F);
            this.RequestDateLable.Name = "RequestDateLable";
            this.RequestDateLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.RequestDateLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.RequestDateLable.StyleName = "FieldCaption";
            this.RequestDateLable.Text = "Request Date";
            // 
            // crequestPriorityLable
            // 
            this.crequestPriorityLable.Dpi = 100F;
            this.crequestPriorityLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0833F, 269F);
            this.crequestPriorityLable.Name = "crequestPriorityLable";
            this.crequestPriorityLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.crequestPriorityLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.crequestPriorityLable.StyleName = "FieldCaption";
            this.crequestPriorityLable.Text = "Request Priority";
            // 
            // cRouteToLable
            // 
            this.cRouteToLable.Dpi = 100F;
            this.cRouteToLable.LocationFloat = new DevExpress.Utils.PointFloat(422.0833F, 308.9999F);
            this.cRouteToLable.Name = "cRouteToLable";
            this.cRouteToLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cRouteToLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.cRouteToLable.StyleName = "FieldCaption";
            this.cRouteToLable.Text = "Route To";
            // 
            // fundSrcCodeIDLable
            // 
            this.fundSrcCodeIDLable.Dpi = 100F;
            this.fundSrcCodeIDLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 237F);
            this.fundSrcCodeIDLable.Name = "fundSrcCodeIDLable";
            this.fundSrcCodeIDLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.fundSrcCodeIDLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.fundSrcCodeIDLable.StyleName = "FieldCaption";
            this.fundSrcCodeIDLable.Text = "Cost Code";
            // 
            // GPS_XLable
            // 
            this.GPS_XLable.Dpi = 100F;
            this.GPS_XLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 269F);
            this.GPS_XLable.Name = "GPS_XLable";
            this.GPS_XLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.GPS_XLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.GPS_XLable.StyleName = "FieldCaption";
            this.GPS_XLable.Text = "GPS X";
            // 
            // GPS_YLable
            // 
            this.GPS_YLable.Dpi = 100F;
            this.GPS_YLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 295F);
            this.GPS_YLable.Name = "GPS_YLable";
            this.GPS_YLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.GPS_YLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.GPS_YLable.StyleName = "FieldCaption";
            this.GPS_YLable.Text = "GPS Y";
            // 
            // nMaintObjectIDLable
            // 
            this.nMaintObjectIDLable.Dpi = 100F;
            this.nMaintObjectIDLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 121F);
            this.nMaintObjectIDLable.Name = "nMaintObjectIDLable";
            this.nMaintObjectIDLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.nMaintObjectIDLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.nMaintObjectIDLable.StyleName = "FieldCaption";
            this.nMaintObjectIDLable.Text = "Asset #";
            // 
            // NotesLable
            // 
            this.NotesLable.Dpi = 100F;
            this.NotesLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 440.9583F);
            this.NotesLable.Name = "NotesLable";
            this.NotesLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.NotesLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.NotesLable.StyleName = "FieldCaption";
            this.NotesLable.Text = "Notes ";
            // 
            // TitleLable
            // 
            this.TitleLable.Dpi = 100F;
            this.TitleLable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 399F);
            this.TitleLable.Name = "TitleLable";
            this.TitleLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TitleLable.SizeF = new System.Drawing.SizeF(122.4167F, 20F);
            this.TitleLable.StyleName = "FieldCaption";
            this.TitleLable.Text = "Job Description";
            // 
            // cAreaID
            // 
            this.cAreaID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cAreaID")});
            this.cAreaID.Dpi = 100F;
            this.cAreaID.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 101F);
            this.cAreaID.Name = "cAreaID";
            this.cAreaID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cAreaID.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.cAreaID.StyleName = "DataField";
            this.cAreaID.Text = "cAreaID";
            // 
            // cgroup
            // 
            this.cgroup.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cgroup")});
            this.cgroup.Dpi = 100F;
            this.cgroup.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 80.99998F);
            this.cgroup.Name = "cgroup";
            this.cgroup.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cgroup.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.cgroup.StyleName = "DataField";
            this.cgroup.Text = "cgroup";
            // 
            // xrLabel19
            // 
            this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cjobreasonid")});
            this.xrLabel19.Dpi = 100F;
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 289F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(201.4999F, 20F);
            this.xrLabel19.StyleName = "DataField";
            this.xrLabel19.Text = "xrLabel19";
            // 
            // cMaintObjectID
            // 
            this.cMaintObjectID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cMaintObjectID")});
            this.cMaintObjectID.Dpi = 100F;
            this.cMaintObjectID.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 9.999998F);
            this.cMaintObjectID.Name = "cMaintObjectID";
            this.cMaintObjectID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cMaintObjectID.SizeF = new System.Drawing.SizeF(238.75F, 20.00001F);
            this.cMaintObjectID.StyleName = "DataField";
            this.cMaintObjectID.Text = "cMaintObjectID";
            // 
            // cOutComecode
            // 
            this.cOutComecode.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cOutcomeID")});
            this.cOutComecode.Dpi = 100F;
            this.cOutComecode.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 327.875F);
            this.cOutComecode.Name = "cOutComecode";
            this.cOutComecode.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cOutComecode.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.cOutComecode.StyleName = "DataField";
            this.cOutComecode.Text = "cOutComecode";
            // 
            // xrLabel22
            // 
            this.xrLabel22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cRequestDate")});
            this.xrLabel22.Dpi = 100F;
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 9.000015F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(201.5001F, 20F);
            this.xrLabel22.StyleName = "DataField";
            this.xrLabel22.Text = "xrLabel22";
            // 
            // xrLabel23
            // 
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.crequestPriority")});
            this.xrLabel23.Dpi = 100F;
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 269F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(201.4999F, 20F);
            this.xrLabel23.StyleName = "DataField";
            this.xrLabel23.Text = "xrLabel23";
            // 
            // xrLabel24
            // 
            this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cRouteTo")});
            this.xrLabel24.Dpi = 100F;
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(544.5F, 308.9999F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(201.4996F, 20F);
            this.xrLabel24.StyleName = "DataField";
            this.xrLabel24.Text = "xrLabel24";
            // 
            // FundSrcCodeID
            // 
            this.FundSrcCodeID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.FundSrcCodeID")});
            this.FundSrcCodeID.Dpi = 100F;
            this.FundSrcCodeID.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 237F);
            this.FundSrcCodeID.Name = "FundSrcCodeID";
            this.FundSrcCodeID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.FundSrcCodeID.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.FundSrcCodeID.StyleName = "DataField";
            this.FundSrcCodeID.Text = "FundSrcCodeID";
            // 
            // GPS_X
            // 
            this.GPS_X.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.GPS_X", "{0:#.00}")});
            this.GPS_X.Dpi = 100F;
            this.GPS_X.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 269F);
            this.GPS_X.Name = "GPS_X";
            this.GPS_X.NullValueText = "  0.00";
            this.GPS_X.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.GPS_X.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.GPS_X.StyleName = "DataField";
            this.GPS_X.Text = "GPS_X";
            // 
            // GPS_Y
            // 
            this.GPS_Y.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.GPS_Y", "{0:#.00}")});
            this.GPS_Y.Dpi = 100F;
            this.GPS_Y.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 295F);
            this.GPS_Y.Name = "GPS_Y";
            this.GPS_Y.NullValueText = "No data";
            this.GPS_Y.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.GPS_Y.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.GPS_Y.StyleName = "DataField";
            this.GPS_Y.Text = "GPS_Y";
            // 
            // n_MaintObjectCodeID
            // 
            this.n_MaintObjectCodeID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.n_MaintObjectID")});
            this.n_MaintObjectCodeID.Dpi = 100F;
            this.n_MaintObjectCodeID.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 121F);
            this.n_MaintObjectCodeID.Name = "n_MaintObjectCodeID";
            this.n_MaintObjectCodeID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.n_MaintObjectCodeID.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.n_MaintObjectCodeID.StyleName = "DataField";
            this.n_MaintObjectCodeID.Text = "n_MaintObjectCodeID";
            // 
            // xrLabel31
            // 
            this.xrLabel31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.Notes")});
            this.xrLabel31.Dpi = 100F;
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 440.9583F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.NullValueText = "No Notes Posted";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(238.75F, 20F);
            this.xrLabel31.StyleName = "DataField";
            this.xrLabel31.Text = "xrLabel31";
            // 
            // xrLabel32
            // 
            this.xrLabel32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.Title")});
            this.xrLabel32.Dpi = 100F;
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(122.4167F, 399F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(515.5833F, 20F);
            this.xrLabel32.StyleName = "DataField";
            this.xrLabel32.Text = "xrLabel32";
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 100F;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.000005F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(746.0003F, 5.999994F);
            this.xrLine1.StyleName = "DataField";
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 23F;
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
            // JobidLable
            // 
            this.JobidLable.Dpi = 100F;
            this.JobidLable.LocationFloat = new DevExpress.Utils.PointFloat(241.1251F, 70.79169F);
            this.JobidLable.Name = "JobidLable";
            this.JobidLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.JobidLable.SizeF = new System.Drawing.SizeF(159.875F, 20F);
            this.JobidLable.StyleName = "FieldCaption";
            this.JobidLable.Text = "Work Order Number:";
            // 
            // cTypeOfJob
            // 
            this.cTypeOfJob.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.cTypeOfJob")});
            this.cTypeOfJob.Dpi = 100F;
            this.cTypeOfJob.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cTypeOfJob.LocationFloat = new DevExpress.Utils.PointFloat(299.9584F, 39F);
            this.cTypeOfJob.Name = "cTypeOfJob";
            this.cTypeOfJob.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cTypeOfJob.SizeF = new System.Drawing.SizeF(202.8749F, 19.99998F);
            this.cTypeOfJob.StyleName = "DataField";
            this.cTypeOfJob.StylePriority.UseFont = false;
            this.cTypeOfJob.Text = "cTypeOfJob";
            // 
            // jobidsrc
            // 
            this.jobidsrc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GetSimpleJob.Jobid")});
            this.jobidsrc.Dpi = 100F;
            this.jobidsrc.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobidsrc.LocationFloat = new DevExpress.Utils.PointFloat(401.0001F, 70.79169F);
            this.jobidsrc.Name = "jobidsrc";
            this.jobidsrc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.jobidsrc.SizeF = new System.Drawing.SizeF(193.4999F, 20F);
            this.jobidsrc.StyleName = "DataField";
            this.jobidsrc.StylePriority.UseFont = false;
            this.jobidsrc.Text = "jobidsrc";
            // 
            // pageFooterBand1
            // 
            this.pageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.pageFooterBand1.Dpi = 100F;
            this.pageFooterBand1.HeightF = 43.58336F;
            this.pageFooterBand1.Name = "pageFooterBand1";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 100F;
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.58337F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo1.StyleName = "PageInfo";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Dpi = 100F;
            this.xrPageInfo2.Format = "Page {0} of {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(433.0002F, 10.58337F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel33,
            this.jobidsrc,
            this.JobidLable,
            this.cTypeOfJob});
            this.reportHeaderBand1.Dpi = 100F;
            this.reportHeaderBand1.HeightF = 121.8333F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrLabel33
            // 
            this.xrLabel33.Dpi = 100F;
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(1.999982F, 6.00001F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(748F, 33F);
            this.xrLabel33.StyleName = "Title";
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "Maintenance Work Request";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            this.FieldCaption.Font = new System.Drawing.Font("Times New Roman", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
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
            // jobid
            // 
            this.jobid.Name = "jobid";
            this.jobid.Type = typeof(int);
            this.jobid.ValueInfo = "0";
            this.jobid.Visible = false;
            // 
            // workRequestReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageFooterBand1,
            this.reportHeaderBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "Requestor";
            this.DataSource = this.sqlDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(48, 52, 23, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.jobid});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

   

    private void GetParam()
    {
        if (HttpContext.Current.Session["ReportParm"] != null)
        {
            
            reportParm = Convert.ToInt32(HttpContext.Current.Session["ReportParm"].ToString());
            
        }

        Parameter param = new Parameter();
        param.Name = "param";
       
        param.Type = typeof(System.Int32);
        param.Value = reportParm;
        param.Description = "param";
        
        
        
        
    }
}
