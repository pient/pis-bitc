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
    public partial class GroupUserView : BasePage
    {
        string id = String.Empty;   // 对象id, 默认用户id
        string mode = String.Empty; // 查询类型，默认用户

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            mode = RequestData.Get<string>("mode", "group");

            switch (this.RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
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
            if (!String.IsNullOrEmpty(id))
            {
                SearchCriterion.SetOrder("WorkNo", true);
                SearchCriterion.SetSearch("GroupID", id);

                string qryString = SearchHelper.GetPagingSQLString("SELECT * FROM vw_OrgUserGroup", SearchCriterion, "ORDER BY WorkNo");
                var dicts = DataHelper.QueryDictList(qryString);
                this.PageState.Add("EntList", dicts);

                string countString = SearchHelper.GetCountSQLString("SELECT * FROM vw_OrgUserGroup", SearchCriterion);
                SearchCriterion.RecordCount = DataHelper.QueryValue<int>(countString);
                this.PageState.Add(SearchCriterionStateKey, SearchCriterion);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            var gid = RequestData.Get<string>("gid", String.Empty);
            var idList = RequestData.GetIdList();

            if (idList != null && idList.Count > 0 && !String.IsNullOrEmpty(gid))
            {
                var userGroups = OrgUserGroup.FindAll(
                    Expression.Eq(OrgUserGroup.Prop_GroupID, gid),
                    Expression.In(OrgUserGroup.Prop_UserID, idList.ToArray()));

                foreach (var tent in userGroups)
                {
                    tent.DoDelete();
                }
            }
        }

        #endregion
    }
}
