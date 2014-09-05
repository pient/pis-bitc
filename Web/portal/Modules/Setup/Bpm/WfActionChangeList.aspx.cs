﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;
using PIC.Portal.Workflow;


namespace PIC.Portal.Web.Modules.Setup.Bpm
{
    public partial class WfActionChangeList : BaseListPage
    {
        #region 变量

        string tid = String.Empty;   // 任务id
        string iid = String.Empty;   // 任务id

        private IList<WfAction> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            tid = RequestData.Get<string>("tid");
            iid = RequestData.Get<string>("iid");

            switch (RequestActionString)
            {
                case "chgop":
                    DoChangeOperator();
                    break;
                default:
                    DoSelect();
                    break;
            }

            if (!IsAsyncRequest)
            {
                this.PageState.Add("WfActStatusEnum", Enumeration.GetEnumDict("SysMag.Workflow.ActionStatus"));
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            SearchCriterion.SetOrder(WfAction.Prop_CreatedTime, false);

            if (!String.IsNullOrEmpty(tid) || !String.IsNullOrEmpty(iid))
            {
                if (!String.IsNullOrEmpty(tid))
                {
                    SearchCriterion.SetSearch(WfAction.Prop_TaskID, tid);
                }
                else if (!String.IsNullOrEmpty(iid))
                {
                    SearchCriterion.SetSearch(WfAction.Prop_InstanceID, iid);
                }

                ents = WfAction.FindAll(SearchCriterion);
                this.PageState.Add("EntList", ents);
            }
        }

        /// <summary>
        /// 修改处理人
        /// </summary>
        private void DoChangeOperator()
        {
            var aid = RequestData.Get<string>("aid");
            var uid = RequestData.Get<string>("uid");

            if (!String.IsNullOrEmpty(aid))
            {
                var act = WfAction.Find(aid);
                var usr = OrgUser.Find(uid);

                WfServer.ChangeActionUser(act, usr);    // 更换流程处理人
            }
        }

        #endregion
    }
}
