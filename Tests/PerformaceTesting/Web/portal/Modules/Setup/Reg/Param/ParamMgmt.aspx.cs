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

namespace PIC.Portal.Web.Modules.Setup.Reg
{
    public partial class ParamMgmt : BaseListPage
    {
        #region 属性

        private IList<ParameterCatalog> ents = null;

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

            ParameterCatalog ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<ParameterCatalog>();
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
                            ParameterCatalog.DoBatchDelete(idList.ToArray());
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
                        crit = Expression.In(ParameterCatalog.Prop_ParameterCatalogID, distids.ToArray());
                    }

                    if (pids != null && pids.Count > 0)
                    {
                        IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                        if (crit != null)
                        {
                            crit = SearchHelper.UnionCriterions(crit, Expression.In(ParameterCatalog.Prop_ParentID, dispids.ToArray()));
                        }
                        else
                        {
                            crit = Expression.In(ParameterCatalog.Prop_ParentID, dispids.ToArray());
                        }
                    }
                }
            }
            else
            {
                crit = SearchHelper.UnionCriterions(Expression.IsNull(ParameterCatalog.Prop_ParentID),
                    Expression.Eq(ParameterCatalog.Prop_ParentID, String.Empty));
            }

            ents = ParameterCatalog.FindAll(sc, crit);
            //ents = SysParameterCatalog.FindAll(sc, crit,
            //    Expression.Not(Expression.Eq(SysParameterCatalog.Prop_Code, "System")));

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}
