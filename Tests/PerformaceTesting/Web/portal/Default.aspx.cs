using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PIC.Common;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Workflow;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web
{
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = PortalService.SystemInfo.SystemName;

            switch (this.RequestActionString)
            {
                case "msgtask":
                    this.PageState.Add("NewMsgCount", Message.Get(UserInfo.UserID, Message.TypeEnum.Received, Message.StatusEnum.New).Count);
                    this.PageState.Add("NewTaskCount", WfAction.Get(UserInfo.UserID, WfAction.StatusEnum.New).Count);
                    break;
            }

            if (!IsAsyncRequest)
            {
                IEnumerable<Module> menuItems = new List<Module>();

                if (UserContext != null && UserContext.AccessibleApplications.Count > 0)
                {
                    Application defApp = UserContext.AccessibleApplications.FirstOrDefault();

                    if (defApp != null && UserContext.AccessibleModules.Count > 0)
                    {
                        Module rootMenuItem = UserContext.AccessibleModules.FirstOrDefault(tent => tent.ApplicationID == defApp.ApplicationID
                            && StringHelper.IsEqualsIgnoreCase(tent.Code, "M_SYS"));

                        if (rootMenuItem != null)
                        {
                            int rootLevel = rootMenuItem.PathLevel.GetValueOrDefault();

                            menuItems = UserContext.AccessibleModules.Where(tent =>
                                !String.IsNullOrEmpty(tent.Path)
                                && tent.Path.IndexOf(rootMenuItem.ID) >= 0
                                && tent.PathLevel.GetValueOrDefault() > rootLevel
                                && tent.ModuleID != rootMenuItem.ModuleID);
                        }

                        this.PageState.Add("RootMenuItem", rootMenuItem);

                        menuItems = menuItems.OrderBy(tent => tent.SortIndex);
                    }
                }

                this.PageState.Add("MdlList", menuItems);

                this.PageState.Add("NewMsgCount", Message.Get(UserInfo.UserID, Message.TypeEnum.Received, Message.StatusEnum.New).Count);

                var usr = OrgUser.Find(UserInfo.UserID);
                var basicCfg = usr.GetBasicConfig();
                this.PageState.Add("BasicCfg", basicCfg);
            }
        }

        #region 支持方法

        #endregion
    }
}
