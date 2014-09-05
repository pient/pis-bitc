using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIC.Portal.Web.Masters.Ext
{
    public partial class formpage : System.Web.UI.MasterPage
    {
        public formpage()
        {
            MasterPageFile = WebPortalService.VirtualPathProvider.Params.SiteMasterPageLocation;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
