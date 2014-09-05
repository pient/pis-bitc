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
using PIC.Doc.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Doc
{
    public partial class DocMgmt : BaseListPage
    {
        #region 属性

        private IList<DocDirectory> ents = null;

        #endregion

        #region 变量

        Guid? id = null;   // 对象id
        IList<Guid> ids = null;   // 节点列表
        IList<Guid> pids = null;   // 父节点列表

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.GetGuid("id");
            ids = RequestData.GetGuidList("ids");
            pids = RequestData.GetGuidList("pids");

            DocDirectory ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<DocDirectory>();
                    ent.ParentID = ent.ParentID.IsNullOrEmpty() ? null : ent.ParentID;
                    ent.DoUpdate();
                    this.SetMessage("更新成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
                        var idList = RequestData.GetIdList();
                        if (idList != null && idList.Count > 0)
                        {
                            DocDirectory.DoBatchDelete(idList.ToArray());
                        }
                    }
                    else
                    {
                        DoSelect();
                    }

                    break;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 选择数据
        /// </summary>
        private void DoSelect()
        {
            // 构建查询表达式
            SearchCriterion sc = new HqlSearchCriterion();

            sc.SetOrder(DocDirectory.Prop_SortIndex);
            sc.SetSearch(DocDirectory.Prop_Status, "Enabled");

            ICriterion crit = null;

            if (RequestActionString == "querychildren")
            {
                if (ids != null && ids.Count > 0 || pids != null && pids.Count > 0)
                {
                    if (ids != null && ids.Count > 0)
                    {
                        var distids = ids.Distinct().Where(tent => !CLRHelper.IsNullOrEmpty(tent));
                        crit = Expression.In(DocDirectory.Prop_DirectoryID, distids.ToArray());
                    }

                    if (pids != null && pids.Count > 0)
                    {
                        var dispids = pids.Distinct().Where(tent => !CLRHelper.IsNullOrEmpty(tent));
                        
                        if (crit != null)
                        {
                            crit = SearchHelper.UnionCriterions(crit, Expression.In(DocDirectory.Prop_ParentID, dispids.ToArray()));
                        }
                        else
                        {
                            crit = Expression.In(DocDirectory.Prop_ParentID, dispids.ToArray());
                        }
                    }
                }
            }
            else
            {
                crit = SearchHelper.UnionCriterions(Expression.IsNull(DocDirectory.Prop_ParentID));
            }

            ents = DocDirectory.FindAll(sc, crit);

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}
