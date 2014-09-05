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
    public partial class FuncSelect : BasePage
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
                case "group":
                    DoSelectByGroup();
                    break;
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
            var ents = OrgFunction.FindAll(SearchCriterion, crit);

            this.PageState.Add("EntList", ents);
        }

        /// <summary>
        /// 根据职能查询
        /// </summary>
        private void DoSelectByGroup()
        {
            var gid = RequestData.Get<string>("gid", refId);

            ICriterion r_crit = null;   // 角色过滤
            ICriterion t_crit = null;   // 类型过滤

            if (!String.IsNullOrEmpty(gid))
            {
                // 过滤已选中的角色
                string qrysql = "SELECT FunctionID FROM OrgGroupFunction WHERE GroupID ='" + gid + "'";

                // 包含已提供id
                if (!String.IsNullOrEmpty(id))
                {
                    qrysql += " AND GroupFunctionID <> '" + id + "'";
                }

                var funcIds = DataHelper.QueryValueList(qrysql);

                r_crit = Expression.Not(Expression.In(OrgFunction.Prop_FunctionID, funcIds.ToArray()));

                // 过滤类型
                var grp = OrgGroup.FindAllByPrimaryKeys(gid).FirstOrDefault();

                if (grp != null)
                {
                    t_crit = Expression.Eq(OrgFunction.Prop_Type, grp.Type);
                }
            }

            var ents = OrgFunction.FindAll(SearchCriterion, crit, r_crit, t_crit);

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}
