using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Setup.Reg
{
    public partial class ParamList : BaseListPage
    {
        #region 变量

        string cid = String.Empty;   // 对象分类id

        private IList<Model.Parameter> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            cid = RequestData.Get<string>("cid");

            Model.Parameter ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Model.Parameter>();
                    ent.DoDelete();
                    break;
                default:
                    switch (RequestActionString)
                    {
                        case "batchdelete":
                            DoBatchDelete();
                            break;
                        default:
                            DoSelect();
                            break;
                    }
					break;
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(cid))
                {
                    var paramCatalog = ParameterCatalog.Find(cid);
                    PageState.Add("Catalog", paramCatalog); // 增加参数分类信息
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!String.IsNullOrEmpty(cid))
            {
                SearchCriterion.SetOrder("SortIndex");
                SearchCriterion.SetSearch("CatalogID", cid);

                ents = Model.Parameter.FindAll(SearchCriterion);
                this.PageState.Add("EntList", ents);
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
                Model.Parameter.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}

