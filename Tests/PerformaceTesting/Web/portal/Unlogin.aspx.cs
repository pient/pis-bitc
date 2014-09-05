using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIC.Portal.Web
{
    public partial class Unlogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = CLRHelper.ConvertValue<string>(Request["action"], "relogin");

            switch (action)
            {
                case "exit":
                    AuthService.Logout();
                    break;
                default:
                    WebPortalService.LogoutAndRedirect(System.Web.Security.FormsAuthentication.LoginUrl);
                    break;
            }
        }
    }
}
