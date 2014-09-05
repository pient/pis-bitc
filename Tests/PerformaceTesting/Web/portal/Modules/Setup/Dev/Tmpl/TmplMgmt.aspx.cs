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

namespace PIC.Portal.Web.Modules.Setup.Dev.Tmpl
{
    public partial class TmplMgmt : BaseListPage
    {
        #region 属性

        private IList<TemplateCatalog> ents = null;

        #endregion

        #region 变量

        string id = String.Empty;   // 对象id
        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");

            TemplateCatalog ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<TemplateCatalog>();
                    ent.ParentID = String.IsNullOrEmpty(ent.ParentID) ? null : ent.ParentID;
                    ent.DoUpdate();
                    this.SetMessage("更新成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
                        var idList = RequestData.GetIdList();
                        if (idList != null && idList.Count > 0)
                        {
                            TemplateCatalog.DoBatchDelete(idList.ToArray());
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

            sc.SetOrder("SortIndex");

            ICriterion crit = null;

            if (RequestActionString == "querychildren")
            {
                if (ids != null && ids.Count > 0 || pids != null && pids.Count > 0)
                {
                    if (ids != null && ids.Count > 0)
                    {
                        IEnumerable<string> distids = ids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));
                        crit = Expression.In(TemplateCatalog.Prop_CatalogID, distids.ToArray());
                    }

                    if (pids != null && pids.Count > 0)
                    {
                        IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                        if (crit != null)
                        {
                            crit = SearchHelper.UnionCriterions(crit, Expression.In(TemplateCatalog.Prop_ParentID, dispids.ToArray()));
                        }
                        else
                        {
                            crit = Expression.In(TemplateCatalog.Prop_ParentID, dispids.ToArray());
                        }
                    }
                }
            }
            else
            {
                crit = SearchHelper.UnionCriterions(Expression.IsNull(TemplateCatalog.Prop_ParentID),
                    Expression.Eq(TemplateCatalog.Prop_ParentID, String.Empty));
            }

            ents = TemplateCatalog.FindAll(sc, crit);
            //ents = SysTemplateCatalog.FindAll(sc, crit,
            //    Expression.Not(Expression.Eq(SysTemplateCatalog.Prop_Code, "System")));

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}
