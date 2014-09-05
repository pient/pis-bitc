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
    public partial class GroupFuncView : BasePage
    {
        string id = String.Empty;   // 对象id
        string mode = String.Empty; // 查询类型

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            mode = RequestData.Get<string>("mode", "group");

            switch (this.RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
                    break;
                case "batchaddfuncs":
                    DoBatchAddFuncs();
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
                SearchCriterion.SetSearch("GroupID", id);

                string qryString = SearchHelper.GetPagingSQLString("SELECT * FROM vw_OrgGroupFunction", SearchCriterion, "ORDER BY CreatedDate Desc");
                var dicts = DataHelper.QueryDictList(qryString);
                this.PageState.Add("EntList", dicts);

                string countString = SearchHelper.GetCountSQLString("SELECT * FROM vw_OrgGroupFunction", SearchCriterion);
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
            var idList = RequestData.GetIdList();

            if (idList != null && idList.Count > 0)
            {
                OrgGroupFunction.DoBatchDelete(idList.ToArray());
            }
        }

        /// <summary>
        /// 批量保存角色
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchAddFuncs()
        {
            var gid = RequestData.Get<string>("gid");
            var fids = RequestData.GetList<string>("fids");

            if (!String.IsNullOrEmpty(gid) && fids != null && fids.Count > 0)
            {
                var grp = OrgGroup.Find(gid);

                if (grp != null)
                {
                    grp.AddFuncByIDs(null, fids.ToArray());
                }
            }
        }

        #endregion
    }
}
