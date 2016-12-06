using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Internal;

public partial class _Default : BasePage {
    protected override bool IsPopulateDatabasePage { get { return true; } }

    protected void Page_Load(object sender, EventArgs e) 
    {

        if (HttpContext.Current.Session["LogonInfo"] != null)
        {
            //Direct To Main Page When MPET logo is clicked
            Response.Redirect("~/main.aspx");
        }
    }

    protected void Callback_Callback(object source, CallbackEventArgs e) {
        e.Result = HtmlConvertor.ToJSON(GetCallbackResult(e.Parameter));
    }
    object GetCallbackResult(string parameter) {
        if(parameter == "create") {
            if(!IsDatabasePopulating) {
                IsDatabasePopulating = true;
                try {
                    DataContext.SalesDataContext.PopulateDatabaseIfNecessary();
                    IsDatabasePopulated = true;
                } catch(Exception e) {
                    return e.Message;
                } finally {
                    IsDatabasePopulating = false;
                }
                return true;
            }
            else
                return false;
        } else if(parameter == "progress") {
            return !IsDatabasePopulated ? DataContext.SalesDataContext.DatabasePopulatingProgressPercentValue : -1;
        }
        throw new ArgumentException("Wrong parameter");
    }
}
