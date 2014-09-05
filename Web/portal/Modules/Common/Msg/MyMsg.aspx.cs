using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Common.Msg
{
    public partial class MyMsg : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAsyncRequest)
            {
                PageState.Add("MsgStatusEnum", Enumeration.GetEnumDict("SysUtil.Message.Status"));
            }
        }
    }
}
