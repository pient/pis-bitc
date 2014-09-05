using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using Microsoft.Synchronization.MetadataStorage;
using Microsoft.Windows.Design.Metadata;
using NHibernate.Criterion;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web.Services;
using System.Threading;

namespace PIC.Portal.Workflow
{
    [Designer(typeof(TaskActivityDesigner))]
    public class TaskActivity : PortalActivity
    {
        #region 成员

        private TaskState taskState;

        private InArgument<FlowContext> ctx;

        private string defaultNextRouteCode = "Submit";

        #endregion

        #region 构造函数

        static TaskActivity()
        {
        }

        #endregion

        #region 属性

        /// <summary>
        /// 任务状态信息
        /// </summary>
        [Browsable(false)]
        public TaskState TaskState
        {
            get
            {
                if (taskState == null)
                {
                    taskState = new TaskState(null);
                }

                return taskState;
            }
        }

        /// <summary>
        /// 流程状态信息
        /// </summary>
        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public InArgument<FlowContext> Context
        {
            get { return this.ctx; }
            set { this.ctx = value; }
        }

        /// <summary>
        /// 默认下一节点编码
        /// </summary>
        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public string DefaultNextRouteCode
        {
            get { return this.defaultNextRouteCode; }
            set { this.defaultNextRouteCode = value; }
        }

        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public string TaskCode
        {
            get { return TaskState.TaskCode; }
            set { TaskState.TaskCode = value; }
        }

        [Browsable(true), DefaultValue(WfTask.TypeEnum.Auto), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public WfTask.TypeEnum Type
        {
            get { return TaskState.Type; }
            set { TaskState.Type = value; }
        }

        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public string ActionTitle
        {
            get { return TaskState.ActionTitle; }
            set { TaskState.ActionTitle = value; }
        }

        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public string ActorsString
        {
            get { return TaskState.ActorsString; }
            set { TaskState.ActorsString = value; }
        }

        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public string RouteExpression
        {
            get { return TaskState.RouteExpression; }
            set { TaskState.RouteExpression = value; }
        }

        /// <summary>
        /// 为true为默认打回节点
        /// </summary>
        [Browsable(true), Category(PIC.ContantValue.PIC_CATEGORY_WORKFLOW)]
        public bool IsDefaultReject
        {
            get { return TaskState.IsDefaultReject; }
            set { TaskState.IsDefaultReject = value; }
        }

        #endregion

        #region 重载方法

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                // 设置Context
                var ctx = GetFlowContext(context);

                // 一般当TaskActivity为EndActivity时ctx可能会为空
                if (ctx == null)
                {
                    return;
                }

                if (!String.IsNullOrEmpty(ctx.UserSessionID))
                {
                    AuthService.PSProvider.SetCurrentPrincipal(ctx.UserSessionID);
                }

                if (String.IsNullOrEmpty(this.ActionTitle))
                {
                    if (!String.IsNullOrEmpty(this.DisplayName))
                    {
                        this.ActionTitle = this.DisplayName;
                    }
                }

                var srcTaskState = ctx.Instance.FlowState.Current;   // 上一个任务状态

                if (srcTaskState != null)
                {
                    TaskState.Request.SourceTaskCode = srcTaskState.TaskCode;  // 源流程编号
                }

                // 检查并创建Task记录或修改任务状态
                TaskState.Name = this.DisplayName;
                TaskState.ActorList = WfActorHelper.GetActorsByString(this.ActorsString, ctx);

                ctx.Instance.FlowState.Current = TaskState;

                if (ctx.Instance.FlowState.Request.OnTaskExecute != null)
                {
                    ctx.Instance.FlowState.Request.OnTaskExecute(ctx);
                }

                // 设置活动用户
                var actionUserList = GetActionUserList(ctx);

                // 判断是否立即执行节点
                if (this.CanInduceIdle && !this.IsImmediate(ctx))
                {
                    if (actionUserList.Count == 0 
                        && this.DefaultNextRouteCode != null 
                        && !String.IsNullOrWhiteSpace(this.DefaultNextRouteCode))
                    {
                        if (ctx.TaskRequest == null)
                        {
                            ctx.TaskRequest = new TaskRequest(this.DefaultNextRouteCode);
                        }
                        else
                        {
                            ctx.TaskRequest.NextRouteCode = this.DefaultNextRouteCode;
                        }
                    }
                    else
                    {
                        context.CreateBookmark(TaskState.TaskCode, new BookmarkCallback(this.OnContinue));
                    }
                }

                WfServer.ExecuteTask(ctx, actionUserList);

                if (ctx.Instance.FlowState.Request.OnTaskEnd != null)
                {
                    ctx.Instance.FlowState.Request.OnTaskEnd(ctx);
                }

                if (ctx.Instance.FlowState.IsFirstEnter)
                {
                    ctx.Instance.FlowState.IsFirstEnter = false;    // 只能执行一次
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 支持方法

        protected virtual void OnContinue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var _ctx = obj as FlowContext;

            // 将传过来的值，全部复制到当前Context中，非常重要! 否则Context大部分值将为空
            if (_ctx != null)
            {
                var fctx = Context.Get(context);

                fctx.Apply(_ctx);
            }

            if (_ctx.Instance.FlowState.Request.OnTaskContinue != null)
            {
                _ctx.Instance.FlowState.Request.OnTaskContinue(_ctx);
            }
        }

        /// <summary>
        /// 当前环节是否满足立即条件（即可以立即跳过）
        /// </summary>
        protected virtual bool IsImmediate(FlowContext flowContext)
        {
            bool isImmdiate = this.Type == WfTask.TypeEnum.Immediate    // 立即任务
                || flowContext.Instance.FlowState.IsFirstEnter;  // 首次任务

            return isImmdiate;
        }

        /// <summary>
        /// 获取活动执行人
        /// </summary>
        /// <returns></returns>
        protected virtual IList<OrgUser> GetActionUserList(FlowContext flowContext)
        {
            // 设置活动用户
            IList<OrgUser> actionUserList = WfServer.GetActionUserList(flowContext);

            // 移除当前用户
            actionUserList = actionUserList.Where(ent =>
                !String.Equals(ent.UserID, PortalService.CurrentUserInfo.UserID, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return actionUserList;
        }

        public FlowContext GetFlowContext(NativeActivityContext context)
        {
            return Context.Get(context);
        }

        #endregion
    }
}
