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

namespace PIC.Portal.Web.Modules.Common.Sel
{
    public partial class UserSelect : BasePage
    {
        #region 属性

        string mode = String.Empty; // 用户编辑操作
        string id = String.Empty;
        string refId = String.Empty;
        string query = String.Empty;
        int start = 0;
        int limit = 0;

        // IList<string> rolecodes = null; // 指定编号角色的员工
        // IList<string> groupcodes = null; // 指定编号组的员工

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

            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }

            if (!this.PageState.ContainsKey("SearchCriterion"))
            {
                this.PageState.Add("SearchCriterion", SearchCriterion);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (SearchCriterion.Orders.Count == 0)
            {
                SearchCriterion.SetOrder(OrgUser.Prop_WorkNo, true);
            }

            string whereStr = SearchCriterion.GetWhereString();

            string sql = "select u.*, g.Code as DeptCode "
                + " from OrgUser u left join OrgGroup g on u.DeptID = g.GroupID";

            var countsql = SearchHelper.GetCountSQLString(sql, SearchCriterion);
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>(countsql);

            sql = SearchHelper.GetPagingSQLString(sql, SearchCriterion, " ORDER BY CreateDate DESC ");
            var results = DataHelper.QueryDictList(sql);

            this.PageState.Add("EntList", results);
        }

        #endregion
    }
}
