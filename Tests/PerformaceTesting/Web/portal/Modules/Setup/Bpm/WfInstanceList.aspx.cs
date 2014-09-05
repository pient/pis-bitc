using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;
using NHibernate.Criterion;
using PIC.Portal.Workflow;


namespace PIC.Portal.Web.Modules.Setup.Bpm
{
    public partial class WfInstanceList : BaseListPage
    {
        #region 变量

        string iid = String.Empty;   // 流程实例id
        string did = String.Empty;   // 流程定义id
        string mode = String.Empty;

        private IList<WfInstance> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            iid = RequestData.Get<string>("iid");
            did = RequestData.Get<string>("did");
            mode = RequestData.Get<string>("mode");

            switch (this.RequestAction)
            {
                default:
                    switch (RequestActionString)
                    {
                        case "endflow":
                            DoEndFlow();
                            break;
                        default:
                            DoSelect();
                            break;
                    }
					break;
            }

            if (!IsAsyncRequest)
            {
                this.PageState.Add("WfInsStatusEnum", Enumeration.GetEnumDict("SysMag.Workflow.InsStatus"));
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!String.IsNullOrEmpty(did))
            {
                IList<ICriterion> crits = new List<ICriterion>();

                // 新建或草稿状态的流程不显示
                crits.Add(Expression.Not(Expression.Eq(WfInstance.Prop_Status, "New")));
                crits.Add(Expression.Not(Expression.Eq(WfInstance.Prop_Status, "Draft")));

                if ("mgmt" != mode)
                {
                    ICriterion crit = SearchHelper.UnionCriterions(
                        Expression.Eq(WfInstance.Prop_OwnerID, UserInfo.UserID),
                        Expression.Eq(WfInstance.Prop_CreatorID, UserInfo.UserID));

                    crits.Add(crit);
                }

                if (SearchCriterion.Orders.Count == 0)
                {
                    SearchCriterion.SetOrder(WfInstance.Prop_StartedTime, false);
                }

                SearchCriterion.SetSearch(WfInstance.Prop_DefineID, did);

                ents = WfInstance.FindAll(SearchCriterion, crits.ToArray());
                this.PageState.Add("EntList", ents);
            }
        }

        /// <summary>
        /// 终止流程
        /// </summary>
        private void DoEndFlow()
        {
            if (!String.IsNullOrEmpty(iid))
            {
                OperationResult result = WfService.Exec(WfServer.WfExecCommand.Abort, iid);

                if (!result.Success)
                {
                    throw new MessageException(result.Message);
                }
            }
        }

        #endregion
    }
}

