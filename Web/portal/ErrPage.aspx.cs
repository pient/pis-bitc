using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PIC.Portal.Web
{
    public partial class ErrPage : System.Web.UI.Page
    {
        #region 变量

        protected string sc = String.Empty;   // status code

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            sc = Request["sc"];
        }

        #endregion
    }
}
