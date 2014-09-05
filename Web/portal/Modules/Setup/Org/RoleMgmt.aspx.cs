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
    public partial class RoleMgmt : BaseListPage
    {
        #region 成员属性

        private IList<OrgRole> ents = null;

        private string id = String.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");

            switch (RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }

            if (!IsAsyncRequest)
            {
                this.PageState.Add("OrgTypeEnum", OrgType.GetOrgTypeEnum());
            }
        }

        #endregion

        #region 私有方法

        private void DoSelect()
        {
            SearchCriterion.SetOrder("SortIndex");

            ents = OrgRole.FindAll(SearchCriterion, Expression.Not(Expression.Eq(OrgRole.Prop_Type, 0)));

            this.PageState.Add("EntList", ents);
        }

        private void DoBatchDelete()
        {
            var idList = RequestData.GetIdList();

            if (idList != null && idList.Count > 0)
            {
                OrgRole.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}
