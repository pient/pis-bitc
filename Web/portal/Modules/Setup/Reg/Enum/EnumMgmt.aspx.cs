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
    public partial class EnumMgmt : BaseListPage
    {
        #region 属性

        private IList<Enumeration> ents = null;

        #endregion

        #region 变量

        string id = String.Empty;   // 对象id
        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表
        string code = String.Empty; // 对象编码

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id", String.Empty);
            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");
            code = RequestData.Get<string>("code", String.Empty);

            Enumeration ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Enumeration>();
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
                            Enumeration.DoBatchDelete(idList.ToArray());
                        }
                    }
                    if (RequestActionString == "paste")
                    {
                        DoPaste();
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
                        crit = Expression.In(Enumeration.Prop_EnumerationID, distids.ToArray());
                    }

                    if (pids != null && pids.Count > 0)
                    {
                        IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                        if (crit != null)
                        {
                            crit = SearchHelper.UnionCriterions(crit, Expression.In(Enumeration.Prop_ParentID, dispids.ToArray()));
                        }
                        else
                        {
                            crit = Expression.In(Enumeration.Prop_ParentID, dispids.ToArray());
                        }
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(code))
                {
                    Enumeration tent = Enumeration.FindFirstByProperties(Enumeration.Prop_Code, code);
                    crit = Expression.Eq(Enumeration.Prop_ParentID, tent.EnumerationID);
                }
                else
                {
                    crit = SearchHelper.UnionCriterions(Expression.IsNull(Enumeration.Prop_ParentID),
                        Expression.Eq(Enumeration.Prop_ParentID, String.Empty));
                }
            }

            if (crit != null)
            {
                ents = Enumeration.FindAll(sc, crit);
            }
            else
            {
                ents = Enumeration.FindAll(sc);
            }

            if (ents != null && ents.Count != 0)
            {
                this.PageState.Add("EntList", ents);
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        [ActiveRecordTransaction]
        private void DoPaste()
        {
            var idList = RequestData.GetIdList();
            string type = RequestData.Get<string>("type", String.Empty);
            string tid = RequestData.Get<string>("tid", String.Empty);  // 目标节点id
            string pdstype = RequestData.Get<string>("pdstype", String.Empty);  // 粘贴数据来源类型

            if (!String.IsNullOrEmpty(tid))
            {
                Enumeration target = Enumeration.Find(tid);

                PasteDataSourceEnum pdsenum = PasteDataSourceEnum.Unknown;
                PasteAsEnum paenum = PasteAsEnum.Other;
                
                if (pdstype == "cut")
                {
                    pdsenum = PasteDataSourceEnum.Cut;
                }
                else if (pdstype == "copy")
                {
                    pdsenum = PasteDataSourceEnum.Copy;
                }

                if (type == "sib")
                {
                    paenum = PasteAsEnum.Sibling;
                }
                else if (type == "sub")
                {
                    paenum = PasteAsEnum.Child;
                }

                if (pdsenum != PasteDataSourceEnum.Unknown && paenum != PasteAsEnum.Other)
                {
                    // 粘贴操作
                    Enumeration.DoPaste(pdsenum, paenum, tid, idList.ToArray());

                    ents = new List<Enumeration>();

                    // 返回粘贴后节点
                    if (type == "sib")
                    {
                        if (target.Parent == null)
                        {
                            ents = target.GetRoots();
                        }
                        else
                        {
                            ents = target.Parent.ChildNodes;
                        }
                    }
                    else if (type == "sub")
                    {
                        ents.Add(target);
                    }

                    if (ents != null)
                    {
                        this.PageState.Add("EntList", ents.OrderBy(v => v.SortIndex));
                    }
                }
            }
        }

        #endregion
    }
}
