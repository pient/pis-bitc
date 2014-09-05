using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Common.Msg
{
    public partial class PubMsgView : BasePage
    {
        #region 成员变量

        private string id;

        Model.Message msg = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");

            if (!String.IsNullOrEmpty(id))
            {
                msg = Model.Message.Find(id);

                PageState.Add("Message", msg);
            }
        }

        #endregion
    }
}

