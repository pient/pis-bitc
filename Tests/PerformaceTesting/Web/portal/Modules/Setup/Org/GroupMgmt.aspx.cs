using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Script.Serialization;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class GroupMgmt : BaseListPage
    {
        #region 变量

        string id = String.Empty;
        string type = String.Empty;
        string mode = String.Empty; // 查看模式

        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            mode = RequestData.Get<string>("mode");

            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");

            switch (RequestActionString)
            {
                case "refreshsys":
                    RefreshSys();
                    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }

        #endregion

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
            else
            {
                ents = OrgGroup.FindAll(sc);
            }

            PageState.Add("EntList", ents);
        }

        private void RefreshSys()
        {
            PortalService.RefreshSystemContext();
        }

        private void DoBatchDelete()
        {
            var idList = RequestData.GetIdList();

            if (idList != null && idList.Count > 0)
            {
                OrgGroup.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}
