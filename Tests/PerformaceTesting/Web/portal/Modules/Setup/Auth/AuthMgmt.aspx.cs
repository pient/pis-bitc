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

namespace PIC.Portal.Web.Modules.Setup.Auth
{
    public partial class AuthMgmt : BaseListPage
    {
        #region 变量

        IList<Model.Auth> ents = null;

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

            if (RequestActionString == "querychildren")
            {
                if (ids != null && ids.Count > 0 || pids != null && pids.Count > 0)
                {
                    if (ids != null && ids.Count > 0)
                    {
                        IEnumerable<string> distids = ids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));
                        crit = Expression.In(Model.Auth.Prop_AuthID, distids.ToArray());
                    }

                    if (pids != null && pids.Count > 0)
                    {
                        IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                        if (crit != null)
                        {
                            crit = SearchHelper.UnionCriterions(crit, Expression.In(Model.Auth.Prop_ParentID, dispids.ToArray()));
                        }
                        else
                        {
                            crit = Expression.In(Model.Auth.Prop_ParentID, dispids.ToArray());
                        }
                    }
                }
            }
            else
            {
                crit = SearchHelper.UnionCriterions(Expression.IsNull(Model.Auth.Prop_ParentID),
                    Expression.Eq(Model.Auth.Prop_ParentID, String.Empty));
            }

            if (crit != null)
            {
                ents = Model.Auth.FindAll(sc, crit);
            }
            else
            {
                ents = Model.Auth.FindAll(sc);
            }

            PageState.Add("EntList", ents);
        }

        #endregion
    }
}
