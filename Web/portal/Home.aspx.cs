using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PIC.Common;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web
{
    public partial class Home : BasePage
    {
        #region ASP.NET 事件

        public Home()
        {
            if (!String.IsNullOrEmpty(this.UserSID))
            {
                this.IsCheckLogon = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAsyncRequest)
            {
                QueryPortalInfo();

                PageState.Add("SysDataModule", Enumeration.Get("SysData.Module").ChildNodes);
            }
        }

        #endregion

        #region 支持方法

        /// <summary>
        /// 获取门户信息（包含门户Layout和弹出信息）
        /// </summary>
        private void QueryPortalInfo()
        {
            var layout = Model.Template.Get("Sys.Portal.Layout.Default");

            this.PageState.Add("Layout", layout);
        }

        #endregion
    }
}
