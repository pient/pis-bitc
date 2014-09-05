using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;
using NHibernate.Criterion;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class GroupRoleView : BasePage
    {
        string id = String.Empty;   // 对象id, 默认用户id
        string mode = String.Empty; // 查询类型，默认用户

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            mode = RequestData.Get<string>("mode", "group");

            switch (this.RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
        }

        #region 支持方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!String.IsNullOrEmpty(id))
            {
                SearchCriterion.SetSearch("GroupID", id);

                string qryString = SearchHelper.GetPagingSQLString("SELECT * FROM vw_OrgGroupRole", SearchCriterion, "ORDER BY SortIndex");
                var dicts = DataHelper.QueryDictList(qryString);
                this.PageState.Add("EntList", dicts);

                string countString = SearchHelper.GetCountSQLString("SELECT * FROM vw_OrgGroupRole", SearchCriterion);
                SearchCriterion.RecordCount = DataHelper.QueryValue<int>(countString);
                this.PageState.Add(SearchCriterionStateKey, SearchCriterion);
            }
        }

        #endregion
    }
}
