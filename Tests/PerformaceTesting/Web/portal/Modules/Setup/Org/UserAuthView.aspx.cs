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
    public partial class UserAuthView : BasePage
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

            switch (this.RequestActionString)
            {
                case "qryauthlist":
                    DoQryAuthList();
                    break;
                case "saveauth":
                    DoSaveAuth();
                    break;
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
            IList<Model.Auth> ents = null;

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

            PageState.Add("EntList", ents);
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        private void DoQryAuthList()
        {
            if (!String.IsNullOrEmpty(id))
            {
                string sqlString = String.Format("SELECT * FROM vw_OrgUserAuth WHERE UserID = '{0}'", id);
                var roleAuths = DataHelper.QueryDictList(sqlString);

                this.PageState.Add("UserAuthList", roleAuths);
            }
        }

        /// <summary>
        /// 保存权限设置
        /// </summary>
        private void DoSaveAuth()
        {
            if (!String.IsNullOrEmpty(id))
            {
                var addIDs = RequestData.GetList<string>("addIDs");
                var removeIDs = RequestData.GetList<string>("removeIDs");

                var user = OrgUser.Find(id);

                if (user != null)
                {
                    user.AddAuthByIDs(null, addIDs.ToArray());
                    user.RemoveAuthByIDs(removeIDs.ToArray());
                }
            }
        }

        #endregion
    }
}
