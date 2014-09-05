using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIC.Portal.Web.Masters.Ext
{
    public partial class form : System.Web.UI.MasterPage
    {
        public form()
        {
            MasterPageFile = WebPortalService.VirtualPathProvider.Params.SiteMasterPageLocation;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
