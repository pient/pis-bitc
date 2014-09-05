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
    public partial class GroupSelect : BasePage
    {
        #region 变量

        IList<OrgGroup> ents = null;

        int type = 0;
        string id = String.Empty;
        string mode = String.Empty; // 查看模式

        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表
        IList<string> rootcodes = null;   // 根节点编码列表

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
            type = RequestData.Get<int>("type");
            mode = RequestData.Get<string>("mode");

            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");

            // rootcodes用于提供根节点编码，若已提供pids，则rootcodes将无效
            if (pids == null || pids.Count == 0)
            {
                rootcodes = RequestData.GetList<string>("rootcodes");

                if (rootcodes == null)
                {
                    String codes_str = RequestData.Get<String>("rootcodes");

                    if (!String.IsNullOrEmpty(codes_str))
                    {
                        rootcodes = codes_str.Split(',');
                    }
                }
            }

            ICriterion crit = null;

            if (!String.IsNullOrEmpty(query))
            {
                crit = SearchHelper.UnionCriterions(
                    Expression.Like(OrgGroup.Prop_Name, "%" + query + "%"),
                    Expression.Like(OrgGroup.Prop_Code, "%" + query + "%"));
            }

            // 构建查询表达式
            SearchCriterion sc = new HqlSearchCriterion();

            if (type > 0)
            {
                sc.AddSearch(OrgGroup.Prop_Type, type);
            }

            sc.SetOrder("SortIndex");

            if (ids != null && ids.Count > 0 
                || pids != null && pids.Count > 0
                || rootcodes != null && rootcodes.Count > 0)
            {
                if (ids != null && ids.Count > 0)
                {
                    IEnumerable<string> distids = ids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));
                    crit = Expression.In(OrgGroup.Prop_GroupID, distids.ToArray());
                }

                if (rootcodes != null && rootcodes.Count > 0)
                {
                    IEnumerable<string> distcodes = rootcodes.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                    if (crit != null)
                    {
                        crit = SearchHelper.UnionCriterions(crit, Expression.In(OrgGroup.Prop_Code, distcodes.ToArray()));
                    }
                    else
                    {
                        crit = Expression.In(OrgGroup.Prop_Code, distcodes.ToArray());
                    }
                }

                if (pids != null && pids.Count > 0)
                {
                    IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                    if (crit != null)
                    {
                        crit = SearchHelper.UnionCriterions(crit, Expression.In(OrgGroup.Prop_ParentID, dispids.ToArray()));
                    }
                    else
                    {
                        crit = Expression.In(OrgGroup.Prop_ParentID, dispids.ToArray());
                    }
                }
            }
            else
            {
                crit = SearchHelper.UnionCriterions(Expression.IsNull(OrgGroup.Prop_ParentID),
                    Expression.Eq(OrgGroup.Prop_ParentID, String.Empty));
            }

            if (crit != null)
            {
                ents = OrgGroup.FindAll(sc, crit);
            }
            else
            {
                ents = OrgGroup.FindAll(sc);
            }

            PageState.Add("EntList", ents);
        }

        #endregion
    }
}
