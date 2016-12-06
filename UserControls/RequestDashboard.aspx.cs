using System;

namespace UserControls
{
    public partial class RequestDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Configures Dashboard Data Connection
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        protected void ASPxDashboardViewer1_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
        {
        
            ////Cast As Connection Parm
            //MsSqlConnectionParameters parm = e.ConnectionParameters as MsSqlConnectionParameters;

            ////Create Connection Builder
            //SqlConnectionStringBuilder builder =
            //    new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            ////Check For Null
            //if (parm != null)
            //{
            //    //Set SQL Server Authentication
            //    parm.AuthorizationType = MsSqlAuthorizationType.SqlServer;

            //    //Set Username
            //    parm.UserName = builder.UserID;

            //    //Set Password
            //    parm.Password = builder.Password;

            //    //Set Server
            //    parm.ServerName = builder.DataSource;

            //    //Set Catalog
            //    parm.DatabaseName = builder.InitialCatalog;

            
            //}
        }
    }
}