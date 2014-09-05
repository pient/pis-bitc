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

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class FuncRoleView : BasePage
    {
        string id = String.Empty;   // 对象id
        string mode = String.Empty; // 查询类型

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            mode = RequestData.Get<string>("mode", "func");

            switch (this.RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
                    break;
                case "batchaddroles":
                    DoBatchAddRoles();
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
                SearchCriterion.SetSearch("FunctionID", id);

                string qryString = SearchHelper.GetPagingSQLString("SELECT * FROM vw_OrgFunctionRole", SearchCriterion, "ORDER BY CreatedDate Desc");
                var dicts = DataHelper.QueryDictList(qryString);
                this.PageState.Add("EntList", dicts);

                string countString = SearchHelper.GetCountSQLString("SELECT * FROM vw_OrgFunctionRole", SearchCriterion);
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
                OrgFunctionRole.DoBatchDelete(idList.ToArray());
            }
        }

        /// <summary>
        /// 批量保存角色
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchAddRoles()
        {
            var fid = RequestData.Get<string>("fid");
            var rids = RequestData.GetList<string>("rids");

            if (!String.IsNullOrEmpty(fid) && rids != null && rids.Count > 0)
            {
                var func = OrgFunction.Find(fid);

                if (func != null)
                {
                    func.AddRoleByIDs(null, rids.ToArray());
                }
            }
        }

        #endregion
    }
}
