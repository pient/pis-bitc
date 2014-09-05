using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Portal.Workflow;
using PIC.Biz.Model;
using PIC.Biz.Model.Reimbursement;

namespace PIC.Biz.Web
{
    public partial class OA_ReimbursementFlow : BizFlowFormPage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");

            OA_Reimbursement ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                case RequestActionEnum.Create:
                    DoSave();
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<OA_Reimbursement>();
                    ent.DoDelete();
                    return;
                default:
                    switch (RequestActionString)
                    {
                        case "submit":
                            DoSubmit();
                            break;
                        case "action":
                            DoAction();
                            break;
                    }
                    break;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OA_Reimbursement.Find(id);
                }
            }

            this.SetFormData(ent);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 保存数据
        /// </summary>
        private OA_Reimbursement DoSave()
        {
            OA_Reimbursement mergedData = GetMergedData<OA_Reimbursement>();

            mergedData.DoSave();

            return mergedData;
        }

        /// <summary>
        /// 提交流程
        /// </summary>
        private void DoSubmit()
        {
            OA_Reimbursement reimb = DoSave();

            FlowRequest freq = new FlowRequest("OA_AdminFeeByMonthFlow");
            freq.FormPath = "/biz/reimb/OA_ReimbursementFlow.aspx";
            freq.Title = reimb.Name;
            freq.ActionTitleFormat = "${FlowTitle} - ${ActionTitle}";
            freq.ModelID = reimb.Id;
            freq.TaskRequest.RouteCode = "AdminSpecialist";
            freq.TaskRequest.ActorList.AddUsersByID(reimb.CreatorId);

            reimb.DoStartFlow(freq);
        }

        /// <summary>
        /// 接收
        /// </summary>
        private void DoAction()
        {
            OA_Reimbursement ent = OA_Reimbursement.Find(id);
            string actionCode = RequestData.Get<string>("ActionCode");
            string actionId = RequestData.Get<string>("ActionId");
            string comments = RequestData.Get<string>("Comments", "");

            ActionRequest areq = new ActionRequest(ActionRequest.ActionOperation.Complete);
            areq.ActionCode = actionCode;
            areq.Comments = comments;
            areq.SourceInfo = ActionSourceInfo.ActionSource(actionId);

            ent.DoAction(actionId, areq);
        }

        #endregion
    }
}

