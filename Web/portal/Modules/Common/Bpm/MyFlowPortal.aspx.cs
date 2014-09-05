using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Common.Bpm
{
    public partial class MyFlowPortal : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "query":
                default:
                    DoSelect();
                    break;
            }

            if (!IsAsyncRequest)
            {
                // 流程按启动频率列表
                var freqsql = "SELECT d.*, i.FreqCount "
                    + " FROM WfDefine d INNER JOIN ("
                    + " SELECT DefineID, COUNT(InstanceID) AS FreqCount FROM WfInstance WHERE OwnerID = '" + UserInfo.UserID + "' GROUP BY DefineID"
                    + " ) i ON d.DefineID = i.DefineID ORDER BY i.FreqCount DESC";

                var freqList = PIC.Data.DataHelper.QueryDictList(freqsql);
                this.PageState.Add("FlowFreqList", freqList);

                // 查询统计数据（当前待办数）
                //var statsql = "SELECT i.DefineID, D.Name as DefineName, i.[Catalog], "
                //    + " COUNT(i.InstanceID) AS NewCount, "
                //    + " SUM(CASE WHEN ISNULL(a.AgentID, '') <> '' THEN 0 ELSE 1 END) AS NewMyCount, "
                //    + " SUM(CASE WHEN ISNULL(a.AgentID, '') = '' THEN 0 ELSE 1 END) AS NewAgentCount "
                //    + " FROM WfInstance i INNER JOIN ( "
                //    + " SELECT * FROM WfAction a WHERE (OwnerID = '" + UserInfo.UserID + "' OR AgentID = '" + UserInfo.UserID + "') AND NOT EXISTS "
                //    + " (SELECT 1 from WfAction WHERE (OwnerID = '" + UserInfo.UserID + "' OR AgentID = '" + UserInfo.UserID + "') AND InstanceID=a.InstanceID AND CreatedTime > a.CreatedTime)  "
                //    + " ) a ON i.InstanceID = a.InstanceID AND a.[Status] IN ('New', 'Opened', 'Started', 'Pending') "
                //    + " INNER JOIN WfDefine d ON i.DefineID = d.DefineID "
                //    + " GROUP BY i.[Catalog], i.DefineID, d.Name ";

                //var statList = PIC.Data.DataHelper.QueryDictList(statsql);
                //this.PageState.Add("FlowStatList", statList);

                this.PageState.Add("WfCatalogEnum", Enumeration.GetEnumDict("SysMag.Workflow.Catalog"));
            }
        }

        #region 支持方法 

        private void DoSelect()
        {
            var qrytext = RequestData.Get<string>("qrytext");

            // 获取可访问流程
            var flows = UserContext.GetAccessibleFlows();

            if (flows.Count > 0 && !String.IsNullOrEmpty(qrytext))
            {
                flows = flows.Where(f => f.Name.IndexOf(qrytext) >= 0).ToList();
            }

            this.PageState.Add("FlowList", flows);
        }

        #endregion
    }
}
