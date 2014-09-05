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
    public partial class AuthSelect : BasePage
    {
        #region 变量

        IList<Auth> ents = null;

        string id = String.Empty;
        string type = String.Empty;
        string mode = String.Empty; // 查看模式

        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string query = RequestData.Get<string>("query");

            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            mode = RequestData.Get<string>("mode");

            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");

            ICriterion crit = null;

            if (!String.IsNullOrEmpty(query))
            {
                crit = SearchHelper.UnionCriterions(
                    Expression.Like(OrgGroup.Prop_Name, "%" + query + "%"),
                    Expression.Like(OrgGroup.Prop_Code, "%" + query + "%"));
            }

            // 构建查询表达式
            SearchCriterion sc = new HqlSearchCriterion();

            sc.SetOrder("SortIndex");

            if (ids != null && ids.Count > 0 || pids != null && pids.Count > 0)
            {
                if (ids != null && ids.Count > 0)
                {
                    IEnumerable<string> distids = ids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));
                    crit = Expression.In(Auth.Prop_AuthID, distids.ToArray());
                }

                if (pids != null && pids.Count > 0)
                {
                    IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                    if (crit != null)
                    {
                        crit = SearchHelper.UnionCriterions(crit, Expression.In(Auth.Prop_ParentID, dispids.ToArray()));
                    }
                    else
                    {
                        crit = Expression.In(Auth.Prop_ParentID, dispids.ToArray());
                    }
                }
            }
            else
            {
                crit = SearchHelper.UnionCriterions(Expression.IsNull(Auth.Prop_ParentID),
                    Expression.Eq(Auth.Prop_ParentID, String.Empty));
            }

            if (crit != null)
            {
                ents = Auth.FindAll(sc, crit);
            }
            else
            {
                ents = Auth.FindAll(sc);
            }

            PageState.Add("EntList", ents);
        }

        #endregion
    }
}
