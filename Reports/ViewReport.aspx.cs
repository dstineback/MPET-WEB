using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_ViewReport : System.Web.UI.Page
{
    ArrayList ParameterArrayList = new ArrayList(); //Report parameter list
    private ReportDocument ObjReportClientDocument = new ReportDocument(); //Report document
    private string _reportName = "";
    private string _reportParameter = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get Report To Load From Session
        if (HttpContext.Current.Session["ReportToDisplay"] != null)
        {
            //Get Info From Session
            _reportName = (HttpContext.Current.Session["ReportToDisplay"].ToString());
        }

        //Get Report Parameter To Use From Session
        if (HttpContext.Current.Session["ReportParm"] != null)
        {
            //Get Info From Session
            _reportParameter = (HttpContext.Current.Session["ReportParm"].ToString());
            //_reportParameter = Convert.ToInt32(HttpContext.Current.Session["ReportParm"]);

            //Add To Array
            ParameterArrayList.Add(0);
            ParameterArrayList.Add(_reportParameter); 
        }

        //Get Document
        GetReportDocument();

        //View Report
        ViewCystalReport();

        CrystalReportViewer1.RefreshReport();
    }

  
    private void GetReportDocument()
    {
        ReportBase objReportBase = new ReportBase();
        string sRptFolder = string.Empty;
        string sRptName = string.Empty;

        sRptName = _reportName;
        sRptFolder = Server.MapPath("~/Reports"); //Report folder name

        ObjReportClientDocument = objReportBase.PFSubReportConnectionMainParameter(sRptName, ParameterArrayList,
            sRptFolder);

        //This section is for manipulating font and font size. This an optional section
        foreach (Section oSection in ObjReportClientDocument.ReportDefinition.Sections)
        {
            foreach (ReportObject obj in oSection.ReportObjects)
            {
                FieldObject field;
                field = ObjReportClientDocument.ReportDefinition.ReportObjects[obj.Name] as FieldObject;

                if (field != null)
                {
                    Font oFon1 = new Font("Arial Narrow", field.Font.Size - 1F);
                    Font oFon2 = new Font("Arial", field.Font.Size - 1F);

                    if (oFon1 != null)
                    {
                        field.ApplyFont(oFon1);
                    }
                    else if (oFon2 != null)
                    {
                        field.ApplyFont(oFon2);
                    }
                }
            }
        }
    }

    protected void CrystalReportViewer1_ReportRefresh(object source, ViewerEventArgs e)
    {
        //OnInit(e);
        //ViewCystalReport();
    }

    ///
    /// To view crystal report
    ///
    private void ViewCystalReport()
    {

        //Set generated report document as Crystal Report viewer report source
        CrystalReportViewer1.ReportSource = ObjReportClientDocument;
    }
}