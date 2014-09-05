using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class UserGroupView : BasePage
    {
        string id = String.Empty;   // 对象id
        string mode = String.Empty; // 操作模式

        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            mode = RequestData.Get<string>("mode");

            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");

            switch (RequestActionString)
            {
                case "qryrolelist":
                    DoQryRoleList();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }

        #region 私有方法

        private void DoSelect()
        {
            // 构建查询表达式
            SearchCriterion sc = new HqlSearchCriterion();

            sc.SetOrder("SortIndex");

            ICriterion crit = null;

            IList<OrgGroup> ents = null;

            if (RequestActionString == "querychildren")
            {
                if (ids != null && ids.Count > 0 || pids != null && pids.Count > 0)
                {
                    if (ids != null && ids.Count > 0)
                    {
                        IEnumerable<string> distids = ids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));
                        crit = Expression.In(OrgGroup.Prop_GroupID, distids.ToArray());
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

            PageState.Add("EntList", ents);
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        private void DoQryRoleList()
        {
            if (!String.IsNullOrEmpty(id))
            {
                string sqlString = String.Format("SELECT * FROM vw_OrgUserGroup WHERE UserID = '{0}'", id);
                var grpRoles = DataHelper.QueryDictList(sqlString);

                this.PageState.Add("GroupRoleList", grpRoles);
            }
        }

        #endregion
    }
}
