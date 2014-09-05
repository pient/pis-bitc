using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Setup.Dev
{
    public partial class TmplList : BaseListPage
    {
        #region 变量

        string cid = String.Empty;   // 对象分类id

        private IList<Model.Template> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            cid = RequestData.Get<string>("cid");

            Model.Template ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Model.Template>();
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

            if (!String.IsNullOrEmpty(cid))
            {
                var tmplCatalog = TemplateCatalog.Find(cid);
                PageState.Add("Catalog", tmplCatalog); // 增加参数分类信息
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

                ents = Model.Template.FindAll(SearchCriterion);
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
                Model.Template.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}

