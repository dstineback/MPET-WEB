using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class main : System.Web.UI.Page
{
    
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["LogonInfo"] != null)
        {
            {
                var userName = ((LogonObject)HttpContext.Current.Session["LogonInfo"]).FullName.ToUpper();
                divUser.Controls.Add(new LiteralControl (userName));
                var userID = ((LogonObject)HttpContext.Current.Session["LogonInfo"]).UserID.ToString();
                divUserID.Controls.Add(new LiteralControl(userID));
            }
        }
    }
}