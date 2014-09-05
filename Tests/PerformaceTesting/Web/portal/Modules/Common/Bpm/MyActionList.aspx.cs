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


namespace PIC.Portal.Web.Modules.Common.Bpm
{
    public partial class MyActionList : BaseListPage
    {
        #region 变量

        string status = String.Empty;   // 状态
        string did = String.Empty;  // 定义

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            status = RequestData.Get<string>("status", "").ToLower();
            did = RequestData.Get<string>("did", "").ToLower();

            switch (this.RequestAction)
            {
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

            if (!this.IsAsyncRequest)
            {
                this.PageState.Add("WfInsStatusEnum", Enumeration.GetEnumDict("SysMag.Workflow.InsStatus"));
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
            if (SearchCriterion.Orders.Count == 0)
            {
                SearchCriterion.SetOrder(WfInstance.Prop_CreatedTime, false);
            }

            string sql = "";
            string extCond = " 1=1 ";

            if (status == "draft")
            {
                sql = "SELECT * FROM WfInstance WHERE Status IN ('New', 'Draft') AND CreatorID ='" + UserInfo.UserID + "'";
            }
            else if (status == "mine")
            {
                sql = "SELECT * FROM WfInstance WHERE Status IN ('Opened', 'Started', 'Pending') AND OwnerID ='" + UserInfo.UserID + "'";
            }
            else
            {
                sql = "SELECT i.*, a.ActionID, a.[Status] AS MyStatus, a.[CreatedTime] AS MyCreatedTime FROM WfInstance i ";

                if (status == "agent")
                {
                    sql += " INNER JOIN ("
                    + " SELECT * FROM WfAction a WHERE AgentID = '" + UserInfo.UserID + "' AND NOT EXISTS"
                    + " (SELECT 1 from WfAction WHERE AgentID = '" + UserInfo.UserID + "' AND InstanceID=a.InstanceID AND CreatedTime > a.CreatedTime) "
                    + ") a ON i.InstanceID = a.InstanceID";
                }
                else
                {
                    sql += " INNER JOIN ("
                    + " SELECT * FROM WfAction a WHERE OwnerID = '" + UserInfo.UserID + "' AND NOT EXISTS"
                    + " (SELECT 1 from WfAction WHERE OwnerID = '" + UserInfo.UserID + "' AND InstanceID=a.InstanceID AND CreatedTime > a.CreatedTime) "
                    + ") a ON i.InstanceID = a.InstanceID";

                    switch (status)
                    {
                        case "new":
                            extCond += " AND MyStatus IN ('New', 'Opened', 'Started', 'Pending') ";
                            break;
                        case "closed":
                            extCond += " AND MyStatus IN ('Completed', 'Closed', 'Aborted') ";
                            break;
                    }
                }
            }

            if (!String.IsNullOrEmpty(did))
            {
                extCond += " AND DefineID = '" + did + "' ";
            }

            if (!String.IsNullOrEmpty(extCond))
            {
                sql = "SELECT * FROM (" + sql + ") myi WHERE " + extCond;
            }

            var countsql = SearchHelper.GetCountSQLString(sql, SearchCriterion);
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>(countsql);

            sql = SearchHelper.GetPagingSQLString(sql, SearchCriterion, " ORDER BY MyCreatedTime DESC ");
            var results = DataHelper.QueryDictList(sql);

            this.PageState.Add("EntList", results);
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
                WfInstance.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}

