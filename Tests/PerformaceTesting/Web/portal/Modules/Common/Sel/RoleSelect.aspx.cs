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
    public partial class RoleSelect : BasePage
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
                case "func":
                    DoSelectByFunc();
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
            var type = RequestData.Get<int>("type");

            if (type != 0)
            {
                SearchCriterion.SetSearch(OrgRole.Prop_Type, type);
            }

            var ents = OrgRole.FindAll(SearchCriterion, crit);

            this.PageState.Add("EntList", ents);
        }

        /// <summary>
        /// 根据职能查询
        /// </summary>
        private void DoSelectByFunc()
        {
            var fid = RequestData.Get<string>("fid", refId);

            ICriterion r_crit = null;   // 角色过滤
            ICriterion t_crit = null;   // 类型过滤

            if (!String.IsNullOrEmpty(fid))
            {
                // 过滤已选中的角色
                string qrysql = "SELECT RoleID FROM OrgFunctionRole WHERE FunctionID ='" + fid + "'";

                // 包含已提供id
                if (!String.IsNullOrEmpty(id))
                {
                    qrysql += " AND FunctionRoleID <> '" + id + "'";
                }

                var roleIds = DataHelper.QueryValueList(qrysql);

                r_crit = Expression.Not(Expression.In(OrgRole.Prop_RoleID, roleIds.ToArray()));

                // 过滤类型
                var func = OrgFunction.FindAllByPrimaryKeys(fid).FirstOrDefault();

                if (func != null)
                {
                    t_crit = Expression.Eq(OrgRole.Prop_Type, func.Type);
                }
            }

            var ents = OrgRole.FindAll(SearchCriterion, crit, r_crit, t_crit);

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}
