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
using PIC.Portal.Workflow;


namespace PIC.Portal.Web.Modules.Common.Bpm
{
    public partial class FlowBus : BaseFlowPage
    {
        #region 成员属性

        string did = null;  // define id, 获取当前流程定义号
        string iid = null;  // instance id, 获取当前流程实例号
        string tid = null;  // task id, 获取当前流程任务号
        string aid = null;  // action id, 获取当前流程活动号

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            did = RequestData.Get<string>("did");
            iid = RequestData.Get<string>("iid", RequestData.Get<string>("id"));
            tid = RequestData.Get<string>("tid");
            aid = RequestData.Get<string>("aid");

            switch (RequestActionString)
            {
                case "save":
                    DoSave();
                    break;
                case "delete":
                    DoDelete();
                    break;
                case "submit":
                    DoSubmit();
                    break;
                case "qryactorusers":
                    DoQueryActorUsers();
                    break;
                case "qrytask":
                    DoQueryTaskList();
                    break;
                case "qryacts":
                    DoQueryActionList();
                    break;
                case "export":
                    DoExport();
                    break;
                case "discard":
                    DoDiscard();
                    break;
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(did))
                {
                    var define = WfDefine.Find(did);
                    var wfConfig = define.GetConfig();
                    var basicInfo = define.GetInitBasicInfo();

                    PageState.Add("FlowData", new { 
                        type = "define", 
                        Define = define,
                        FormDefine = define.FormDefine,
                        Config = wfConfig, 
                        Basic = basicInfo 
                    });
                }
                else if (!String.IsNullOrEmpty(iid))
                {
                    var instance = WfInstance.Find(iid);
                    var define = instance.GetDefine();
                    var wfConfig = define.GetConfig();

                    ValidationResult vResult = instance.CheckIsAllowDiscard();

                    PageState.Add("FlowData", new { 
                        type = "instance",
                        allowDiscard = vResult.Success,
                        Define = define,
                        FormDefine = define.FormDefine,
                        Config = wfConfig, 
                        Instance = instance, 
                        FlowState = instance.FlowState 
                    });
                }
                else if (!String.IsNullOrEmpty(aid))
                {
                    var action = WfAction.Find(aid);
                    var instance = action.GetWfInstance();
                    var define = instance.GetDefine();
                    var wfConfig = define.GetConfig();

                    ValidationResult vResult = instance.CheckIsAllowDiscard();

                    var task = action.GetTask();
                    var nextTasks = WfHelper.GetNextTaskStates(define, task.Code);

                    PageState.Add("FlowData", new {
                        type = "action",
                        allowDiscard = vResult.Success,
                        Define = define,
                        FormDefine = define.FormDefine,
                        Config = wfConfig, 
                        Instance = instance, 
                        FlowState = instance.FlowState, 
                        Action = action,
                        NextTasks = nextTasks
                    });
                }

                this.PageState.Add("WfOpinionsEnum", Enumeration.GetEnumDict("SysMag.Workflow.Opinions"));
                this.PageState.Add("WfInsStatusEnum", Enumeration.GetEnumDict("SysMag.Workflow.InsStatus"));
                this.PageState.Add("WfActStatusEnum", Enumeration.GetEnumDict("SysMag.Workflow.ActionStatus"));
            }
        }

        #endregion

        #region 支持方法

        private void DoQueryTaskList()
        {
            if (!String.IsNullOrEmpty(iid))
            {
                var taskList = WfTask.FindAllByProperty(WfTask.Prop_InstanceID, iid)
                    .OrderByDescending(t => t.CreatedTime ?? DateTime.MaxValue)
                    .OrderByDescending(t => t.StartedTime ?? DateTime.MaxValue)
                    .OrderByDescending(t => t.EndedTime ?? DateTime.MaxValue);

                PageState.Add("TaskList", taskList);
            }
        }

        /// <summary>
        /// 查询活动用户
        /// </summary>
        private void DoQueryActorUsers()
        {
            var iid = RequestData.Get<string>("iid");
            var taskCode = RequestData.Get<string>("taskCode");

            if (!String.IsNullOrEmpty(iid))
            {
                var ins = WfInstance.Find(iid);
                var def = ins.GetDefine();

                var taskActivity = WfHelper.GetTaskActivity(def, taskCode);

                if (taskActivity != null && taskActivity.TaskState != null)
                {
                    var ctx = new FlowContext(ins);

                    var actors = WfActorHelper.GetActorsByString(taskActivity.TaskState.ActorsString, ctx);

                    var usrList = actors.GetUserList(ctx);

                    if (usrList == null || usrList.Count ==0)
                    {
                        // usrList = WfActorHelper.GetDefaultActorUserList();
                    }

                    PageState.Add("ActorUserList", usrList);
                }
            }
        }

        /// <summary>
        /// 获取任务操作信息
        /// </summary>
        private void DoQueryActionList()
        {
            IList<WfAction> actionList = new List<WfAction>();

            if (!String.IsNullOrEmpty(tid))
            {
                var task = WfTask.Find(tid);
                actionList = task.GetActions();
            }
            else if (!String.IsNullOrEmpty(iid))
            {
                var ins = WfInstance.Find(iid);
                actionList = ins.GetActions()
                    .OrderByDescending(a => a.CreatedTime ?? DateTime.MaxValue)
                    .OrderByDescending(a => a.ClosedTime ?? DateTime.MaxValue)
                    .ToList();
            }

            this.PageState.Add("ActionList", actionList);
        }

        /// <summary>
        /// 暂存流程数据
        /// </summary>
        private void DoSave()
        {
            var basicData = RequestData.Get<FlowBasicInfo>("Basic");
            var formData = RequestData.Get<FlowFormData>("Form");

            WfService.SaveWfInstance(basicData, formData);
        }

        /// <summary>
        /// 提交请求
        /// </summary>
        private void DoSubmit()
        {
            var basicInfo = RequestData.Get<FlowBasicInfo>("Basic");
            var formData = RequestData.Get<FlowFormData>("Form");
            var actionInfo = RequestData.Get<FlowActionInfo>("Approve");

            // 运行时使用WfService，以保证由WCF服务运行WfService
            var opResult = WfService.Run(basicInfo, formData, actionInfo);

            if (opResult.Success != true)
            {
                throw new MessageException(opResult.Message);
            }
        }

        private void DoDelete()
        {
            if (!String.IsNullOrEmpty(iid))
            {
                var ins = WfInstance.Find(iid);

                ins.DoDelete();
            }
        }

        /// <summary>
        /// 作废流程
        /// </summary>
        private void DoDiscard()
        {
            string iid = RequestData.Get<String>("iid");

            WfInstance ins = WfInstance.Find(iid);

            ins.DoDiscard();
        }

        /// <summary>
        /// 导出文档
        /// </summary>
        private void DoExport()
        {
            string iid = RequestData.Get<String>("iid");

            WfInstance ins = WfInstance.Find(iid);

            string tmpl_name = RequestData.Get<string>("Name");
            string tmpl_fileName = RequestData.Get<string>("FileName", tmpl_name);
            string tmpl_path = RequestData.Get<string>("Path");
            bool tmpl_isCheckAuth = RequestData.Get<bool>("IsCheckAuth");

            WfDocTempalte docTmpl = new WfDocTempalte()
            {
                Name = tmpl_name,
                Path = tmpl_path,
                IsCheckAuth = tmpl_isCheckAuth
            };

            WfHelper.ResponseTmplDoc(ins, docTmpl, tmpl_fileName);

            Response.Flush();
            Response.End();
        }

        #endregion
    }
}
