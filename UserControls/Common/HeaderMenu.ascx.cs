using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;

namespace UserControls.Common
{
    public partial class HeaderMenu : UserControl {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Bind Menu
            mainMenu.DataBind();
            if (mainMenu.SelectedItem != null && mainMenu.SelectedItem.Parent != mainMenu.RootItem)
                mainMenu.SelectedItem.Parent.Text = string.Format("{0}: {1}", mainMenu.SelectedItem.Parent,
                    mainMenu.SelectedItem.Text);

            //Check For Previous Session Variable
            if (HttpContext.Current.Session["LogonInfo"] != null)
            {
                //Get Username
                var textToShow = ((LogonObject) HttpContext.Current.Session["LogonInfo"]).Username.ToUpper();

                //Create Menu Item
                MenuItem helpMenuItem = mainMenu.Items.Add(textToShow, "helpMenuItem");
                helpMenuItem.ItemStyle.CssClass = "helpMenuItem";
                helpMenuItem.Image.Url = "~/Content/Images/UserWhite.png";

                if (((LogonObject) HttpContext.Current.Session["LogonInfo"]).UserID == 1)
                {
                    //Show Admin Logo
                    helpMenuItem.Image.Url = "~/Content/Images/AdminIcon.png";
                }

                ////Get Logon Form
                //var HelpMenuPopup = ((ASPxPopupControl)(Master.FindControl("HelpMenuPopup")));

                ////Hide Logon Details
                //HelpMenuPopup.ShowOnPageLoad = false;
            }
            else
            {
                //Create Default Logon Menu Option
                MenuItem helpMenuItem = mainMenu.Items.Add("Logon", "helpMenuItem");
                helpMenuItem.ItemStyle.CssClass = "helpMenuItem";
                helpMenuItem.Image.Url = "~/Content/Images/UserWhite.png";
               
            }
        }
    }
}
