using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Doc.Model;
using PIC.Doc;
using NHibernate.Criterion;


namespace PIC.Portal.Web.Modules.Setup.Doc
{
    public partial class FileList : BaseListPage
    {
        #region 变量

        Guid? did = null;   // 对象分类id

        private IList<DocFile> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            did = RequestData.GetGuid("did");

            DocFile ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<DocFile>();
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
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!did.IsNullOrEmpty())
            {
                SearchCriterion.SetOrder(DocFile.Prop_Name);
                SearchCriterion.SetSearch(DocFile.Prop_DirectoryID, did);

                ents = DocFile.FindAll(SearchCriterion);
                this.PageState.Add("EntList", ents);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            var idList = RequestData.GetGuidIdList();

            if (idList != null && idList.Count > 0)
            {
                var tents = DocFile.FindAll(Expression.In(DocFile.Prop_FileID, idList.ToArray()));

                DocService.Delete(tents);
            }
        }

        #endregion
    }
}

