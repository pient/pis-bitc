using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Common.Bpm
{
    public partial class FlowSelect : BasePage
    {
        #region 属性

        string mode = String.Empty; // 用户编辑操作
        string id = String.Empty;
        string refId = String.Empty;
        string query = String.Empty;
        int start = 0;
        int limit = 0;

        #endregion

        #region ASP.NET 事件

        ICriterion crit = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            query = RequestData.Get<string>("query");
            start = RequestData.Get<int>("start", 0);
            limit = RequestData.Get<int>("limit", 25);
            mode = RequestData.Get<string>("mode");
            id = RequestData.Get<string>("id");
            refId = RequestData.Get<string>("refid");

            SearchCriterion.AllowPaging = true;
            SearchCriterion.GetRecordCount = true;
            SearchCriterion.CurrentPageIndex = (start / limit) + 1;
            SearchCriterion.PageSize = limit;

            switch (mode)
            {
                default:
                    DoSelect();
                    break;
            }

            if (!this.PageState.ContainsKey("SearchCriterion"))
            {
                this.PageState.Add("SearchCriterion", SearchCriterion);
            }

            if (!IsAsyncRequest)
            {
                this.PageState.Add("WfCatalogEnum", Enumeration.GetEnumDict("SysMag.Workflow.Catalog"));
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            var usr = OrgUser.Find(UserInfo.UserID);

            var didList = AuthService.GetAccessibleFlows(usr).Select(d=>d.DefineID).ToArray();

            var ents = WfDefine.FindAll(SearchCriterion, crit, 
                Expression.In(WfDefine.Prop_DefineID, didList));

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}
