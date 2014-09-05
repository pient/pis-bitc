using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Statements;
using System.Threading;
using System.Collections;
using System.Activities.DurableInstancing;
using System.Runtime.DurableInstancing;
using PIC.Common;
using PIC.Portal.Model;
using System.IO;
using System.Activities.XamlIntegration;
using PIC.Data;
using Castle.ActiveRecord;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 项目流程
    /// </summary>
    public class WfServer
    {
        #region Classes

        /// <summary>
        /// 流程执行命令
        /// </summary>
        public class WfExecCommand
        {
            #region Consts

            public const string Abort = "Abort";

            #endregion
        }

        #endregion

        #region 成员变量

        private static object lockobj = new object();

        public readonly string STORE_DB_CONNSTR;

        #endregion

        #region 构造函数

        private static WfServer instance;

        /// <summary>
        /// 服务实例，单体模式
        /// </summary>
        internal static WfServer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockobj)
                    {
                        if (instance == null)
                        {
                            instance = new WfServer();
                        }
                    }
                }

                return instance;
            }
        }

        protected WfServer()
        {
            STORE_DB_CONNSTR = PICConfigurationManager.AppSettings[PortalService.PORTAL_TEMP_DB_CONNSTR_KEY];
        }

        #endregion

        #region 流程执行与任务分发

        /// <summary>
        /// 更换流程处理人
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        public static void ChangeActionUser(WfAction action, OrgUser user)
        {
            if (action.ActionStatus == WfAction.StatusEnum.New
                || action.ActionStatus == WfAction.StatusEnum.Started
                || action.ActionStatus == WfAction.StatusEnum.Pending
                || action.ActionStatus == WfAction.StatusEnum.Opened)
            {
                action.OwnerID = user.UserID;
                action.OwnerName = user.Name;

                action.DoUpdate();
            }
            else
            {
                throw new MessageException("只有未结束的活动可以更换处理人。");
            }
        }

        /// <summary>
        /// 根据Action请求，强制结束所有Action并结束任务
        /// </summary>
        public static void ForceEndTask(ActionRequest request)
        {
            var action = WfAction.Find(request.ActionID);
            var ins = action.GetWfInstance();

            request.Operation = FlowActionInfo.ActionOperation.ForceEnd;

            var ctx = new FlowContext(ins)
            {
                ActionRequest = request
            };
            
            // WfServer.CloseAction(ctx); // 关闭当前任务

            var actions = ctx.CurrentTask.GetActions().Where(a => 
                a.ActionStatus != WfAction.StatusEnum.Closed
                && a.ActionStatus != WfAction.StatusEnum.Completed);

            foreach (var act in actions)
            {
                act.ActionStatus = WfAction.StatusEnum.Aborted;
                act.DoUpdate();
            }

            ctx.CurrentTask.TaskStatus = WfTask.StatusEnum.ForceEnd;
            ctx.CurrentTask.EndedTime = DateTime.Now;

            WfServer.RunTask(ctx);

            ctx.Persist();
        }

        /// <summary>
        /// 分发自由任务
        /// </summary>
        /// <param name="request"></param>
        /// <param name="usrs"></param>
        public static void DispatchFreeActions(ActionRequest request, params OrgUser[] usrs)
        {
            WfActorCollection actors = new WfActorCollection();

            var userIds = usrs.Select(u => u.UserID).Join();

            actors.Add(new WfActor(WfActor.TypeEnum.User)
            {
                UserIds = userIds
            });

            DispatchFreeActions(request, actors);
        }

        /// <summary>
        /// 分发自由任务, 自由任务的完成不触发流程Execute事件
        /// </summary>
        public static void DispatchFreeActions(ActionRequest request, WfActorCollection actors)
        {
            string instanceID = null, taskID = null, grade = null, important = null, catalog = null, opString = null;

            switch (request.SourceInfo.Type)
            {
                case ActionSourceInfo.TypeEnum.Action:
                    WfAction _action = WfAction.Find(request.SourceInfo.Source);
                    if (_action != null)
                    {
                        instanceID = _action.InstanceID;
                    }
                    break;
                case ActionSourceInfo.TypeEnum.Task:
                    WfTask _task = WfTask.Find(request.SourceInfo.Source);
                    if (_task != null)
                    {
                        taskID = _task.TaskID;
                        instanceID = _task.InstanceID;
                    }
                    break;
                case ActionSourceInfo.TypeEnum.Instance:
                    WfInstance _instance = WfInstance.Find(request.SourceInfo.Source);
                    if (_instance != null)
                    {
                        instanceID = _instance.InstanceID;
                    }
                    break;
            }

            grade = StringHelper.IsNullValue(request.Tag["Grade"], "Normal");
            important = StringHelper.IsNullValue(request.Tag["Important"], "Normal");
            catalog = StringHelper.IsNullValue(request.Tag["Catalog"], "");
            opString = StringHelper.IsNullValue(request.Tag["OpString"], WfHelper.GetActionOpString("Pass", "提交"));

            foreach (WfActor actor in actors)
            {
                WfAction action = new WfAction();

                // action.OwnerID = actor.Id;
                // action.OwnerName = actor.Name;
                action.ActionState.OwnerTag = actor.Tag;

                action.InstanceID = instanceID;
                action.TaskID = taskID;
                action.Grade = grade;
                action.Important = important;
                action.Catalog = catalog;

                action.CreatorID = PortalService.CurrentUserInfo.UserID;
                action.CreatorName = PortalService.CurrentUserInfo.Name;
                action.ActionType = WfTask.TypeEnum.Free;

                action.ActionState.Request = request;

                action.ActionStatus = WfAction.StatusEnum.New;

                action.DoCreate();
            }
        }

        /// <summary>
        /// 保存流程实例
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="formData"></param>
        public static WfInstance SaveWfInstance(FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            WfInstance ins = GetWfInstance(basicInfo, actionInfo);

            SaveWfInstance(ref ins, basicInfo, formData, actionInfo);

            return ins;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 运行流程
        /// </summary>
        /// <param name="fReq"></param>
        public static void Run(FlowRequest fReq)
        {
            var ins = WfServer.SaveWfInstance(fReq.BasicInfo, fReq.FormData, fReq.ActionInfo);

            WfServer.Run(ins);
        }

        /// <summary>
        /// 运行流程
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="formData"></param>
        /// <param name="actionInfo"></param>
        public static void Run(FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            var ins = WfServer.SaveWfInstance(basicInfo, formData, actionInfo);

            WfServer.Run(ins);
        }

        /// <summary>
        /// 执行流程
        /// </summary>
        /// <param name="ins"></param>
        public static void Run(WfInstance ins)
        {
            switch (ins.InstanceStatus)
            {
                case WfInstance.StatusEnum.Canceled:
                case WfInstance.StatusEnum.Completed:
                case WfInstance.StatusEnum.Aborted:
                    throw new MessageException("此流程已结束，无法执行！");
                case WfInstance.StatusEnum.Draft:
                case WfInstance.StatusEnum.New:
                    Start(ins);
                    break;
                case WfInstance.StatusEnum.Started:
                case WfInstance.StatusEnum.Paused:
                    Process(ins);
                    break;
            }
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="ins"></param>
        public static void Start(WfInstance ins)
        {
            if (ins.InstanceStatus != WfInstance.StatusEnum.New 
                && ins.InstanceStatus != WfInstance.StatusEnum.Draft)
            {
                throw new MessageException("此流程已被启动，单一流程草稿只能被启动一次。");
            }

            var ctx = new FlowContext(ins);

            var waitHandler = new AutoResetEvent(false);
            var syncContext = SynchronizationContext.Current;   // AppFabric中执行, SynchronizationContext.Current为空

            // 开始时需要加inputs
            var wfApp = GetWfApplication(ctx, waitHandler, syncContext, new EasyDictionary<string, object>());

            // ----------执行开始方法 Step2 Begin---------

            if (ins.FlowState.Request.OnInstanceBegin != null)
            {
                ins.FlowState.Request.OnInstanceBegin(ctx);

                ins.FlowState.Request.OnInstanceBegin = null; // 确保只执行一次，并且不对此方法持久化
            }

            ins.InstanceState.WfInstanceID = wfApp.Id.ToString();

            OnInstanceStart(ctx);   // 设置实例相关值

            // ----------执行开始方法 Step2 End---------

            if (syncContext != null)
            {
                syncContext.OperationStarted();
            }

            wfApp.Run();

            waitHandler.WaitOne();

            if (!ctx.IsPersisted)
            {
                ctx.Persist();
            }
        }

        /// <summary>
        /// 处理流程（只要是处理Action请求）
        /// </summary>
        /// <param name="ins"></param>
        public static void Process(WfInstance ins)
        {
            var actionReq = new ActionRequest(ins.FlowState.ActionInfo, ins.FlowState.BasicInfo, ins.FlowState.FormData);
            var action = WfAction.Find(actionReq.ActionID);

            var ctx = new FlowContext(ins)
            {
                ActionRequest = actionReq,
                CurrentAction = action
            };

            ctx.ValidateActionRequest();    // 验证活动请求是否合法

            RunAction(ctx);

            if (!ctx.IsPersisted)
            {
                ctx.Persist();
            }
        }

        /// <summary>
        /// 执行流程命令
        /// </summary>
        /// <param name="cmdName"></param>
        /// <param name="args"></param>
        public static void Exec(string cmdName, string insId, EasyDictionary args = null)
        {
            WfInstance ins = null;

            if (!String.IsNullOrEmpty(insId))
            {
                ins = WfInstance.Find(insId);
            }

            switch (cmdName)
            {
                case WfExecCommand.Abort:
                    Abort(ins);
                    break;
            }
        }

        /// <summary>
        /// 终止流程
        /// </summary>
        /// <param name="ins"></param>
        internal static void Abort(WfInstance ins)
        {
            if (ins.InstanceStatus != WfInstance.StatusEnum.Started)
            {
                throw new MessageException("只有以启动的流程可以被终止！");
            }

            var ctx = new FlowContext(ins);

            var waitHandler = new AutoResetEvent(false);
            var syncContext = SynchronizationContext.Current;   // AppFabric中执行, SynchronizationContext.Current为空

            // 开始时需要加inputs
            var wfApp = GetWfApplication(ctx, waitHandler, syncContext, new EasyDictionary<string, object>());

            if (syncContext != null)
            {
                syncContext.OperationStarted();
            }

            // 终止所有Action
            var actions = ins.GetActions().Where(a =>
                a.ActionStatus != WfAction.StatusEnum.Closed
                && a.ActionStatus != WfAction.StatusEnum.Aborted
                && a.ActionStatus != WfAction.StatusEnum.Completed);

            foreach (var act in actions)
            {
                act.ActionStatus = WfAction.StatusEnum.Aborted;
                act.DoUpdate();
            }

            // 终止所有Task
            var tasks = ins.GetTasks().Where(a =>
                a.TaskStatus != WfTask.StatusEnum.Closed
                && a.TaskStatus != WfTask.StatusEnum.ForceEnd
                && a.TaskStatus != WfTask.StatusEnum.Completed);

            foreach (var t in tasks)
            {
                t.TaskStatus = WfTask.StatusEnum.ForceEnd;
                t.DoUpdate();
            }

            // 终止当前实例
            ins.InstanceStatus = WfInstance.StatusEnum.Aborted;
            ins.DoUpdate();

            // 终止流程
            wfApp.Abort("由" + PortalService.CurrentUserInfo.Name + "强制终止");

            // 发送流程终止消息给用户
            WfHelper.SendTaskAbortNotification(ins);
        }

        /// <summary>
        /// 保存流程实例
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="formData"></param>
        internal static void SaveWfInstance(ref WfInstance ins, 
            FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            if (String.IsNullOrEmpty(ins.InstanceID))
            {
                ins.DoCreate();

                basicInfo.Status = ins.InstanceStatus.ToString();

                basicInfo.InstanceID = ins.InstanceID;
                basicInfo.CreatedTime = ins.CreatedTime;
                var flowReq = new FlowRequest(basicInfo, formData, actionInfo);

                ins.InstanceState.WfFlowInfo.FlowState = new FlowState(flowReq);
            }
            else
            {
                if (ins.InstanceStatus == WfInstance.StatusEnum.New)
                {
                    ins.InstanceStatus = WfInstance.StatusEnum.Draft;
                }

                basicInfo.Status = ins.InstanceStatus.ToString();

                ins.InstanceState.WfFlowInfo.FlowState.BasicInfo = basicInfo;
                ins.InstanceState.WfFlowInfo.FlowState.FormData = formData;
                ins.InstanceState.WfFlowInfo.FlowState.ActionInfo = actionInfo;

                var def = WfDefine.Get(basicInfo.DefineCode);

                ins.Name = def.Name + " - " + basicInfo.Title;
            }

            ins.DoSave();
        }

        /// <summary>
        /// 由流程基本信息和活动信息，获取流程实例
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="actionInfo"></param>
        /// <returns></returns>
        internal static WfInstance GetWfInstance(FlowBasicInfo basicInfo, FlowActionInfo actionInfo = null)
        {
            WfInstance ins = null;
            WfDefine def = null;
            WfAction act = null;

            if (String.IsNullOrEmpty(basicInfo.InstanceID))
            {
                if (actionInfo != null && !String.IsNullOrEmpty(actionInfo.ActionID))
                {
                    act = WfAction.Find(actionInfo.ActionID);

                    basicInfo.InstanceID = act.InstanceID;
                }
            }

            if (String.IsNullOrEmpty(basicInfo.InstanceID))
            {
                def = WfDefine.Get(basicInfo.DefineCode);

                ins = new WfInstance()
                {
                    DefineID = def.DefineID,
                    ApplicationID = def.ApplicationID,
                    ModuleID = def.ModuleID,
                    Type = def.Type,
                    Catalog = def.Catalog
                };

                ins.InstanceStatus = WfInstance.StatusEnum.New;

                ins.Name = def.Name + " - " + basicInfo.Title;
            }
            else
            {
                ins = WfInstance.Find(basicInfo.InstanceID);
            }

            return ins;
        }

        #region Task 操作

        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="task">要执行的任务</param>
        /// <param name="action">触发任务执行的活动</param>
        /// <param name="request"></param>
        internal static void RunTask(FlowContext ctx)
        {
            ctx.ValidateTask();

            OnTaskRun(ctx);

            AutoResetEvent waitHandler = new AutoResetEvent(false);
            SynchronizationContext syncContext = null; //  SynchronizationContext.Current;

            var wfApp = GetWfApplication(ctx, waitHandler, syncContext);

            // 判断是否自动驳回操作(下一个节点为TaskActivity且当前操作位Reject)
            // var isRejReq = WfHelper.IsAutoRejectRequest(ctx);

            wfApp.Load(new Guid(ctx.Instance.InstanceState.WfInstanceID));

            wfApp.ResumeBookmark(ctx.Instance.FlowState.Current.TaskCode, ctx); // 传入的ctx非常重要，将在TaskActivity中的OnContinute中为Context赋值

            waitHandler.WaitOne();
        }

        /// <summary>
        /// 执行任务(一般在TaskActivity中调用)
        /// </summary>
        /// <param name="ins"></param>
        internal static void ExecuteTask(FlowContext ctx, IList<OrgUser> actionUserList = null)
        {
            // 构建任务上下文，并创建新的任务
            var taskCtx = ctx.NewTaskContext();

            taskCtx.Task.DefineID = ctx.Instance.DefineID;
            taskCtx.Task.InstanceID = ctx.Instance.InstanceID;

            taskCtx.Task.Code = ctx.Instance.FlowState.Current.TaskCode;
            taskCtx.Task.Title = ctx.Instance.FlowState.Current.Name;
            taskCtx.Task.TaskType = ctx.Instance.FlowState.Current.Type;
            taskCtx.Task.OwnerID = PortalService.CurrentUserInfo.UserID;
            taskCtx.Task.OwnerName = PortalService.CurrentUserInfo.Name;

            taskCtx.Task.TaskState = new WfTaskState(ctx.Instance.FlowState.Current);

            if (taskCtx.Task.TaskType == WfTask.TypeEnum.Immediate
                || ctx.Instance.FlowState.IsFirstEnter == true
                || (actionUserList != null && actionUserList.Count == 0))
            {
                // 立即任务或首次任务执行此段
                taskCtx.Task.TaskStatus = WfTask.StatusEnum.Completed;
                taskCtx.Task.StartedTime = taskCtx.Task.EndedTime = ctx.Instance.StartedTime = DateTime.Now;

                // 分发立即任务或首次任务
                if (ctx.Instance.FlowState.IsFirstEnter == true)
                {
                    WfServer.DispatchFirstAction(ctx);
                }
            }
            else
            {
                taskCtx.Task.TaskStatus = WfTask.StatusEnum.Started;
                taskCtx.Task.StartedTime = DateTime.Now;

                // 分发Action
                WfServer.DispatchActions(ctx, actionUserList);

                // 分发过程共更新了用户列表，这里需要再次更新
                ctx.CurrentTask.TaskState = new WfTaskState(ctx.Instance.FlowState.Current);
            }
        }

        /// <summary>
        /// 分发首次任务
        /// </summary>
        /// <param name="ctx"></param>
        internal static void DispatchFirstAction(FlowContext ctx)
        {
            if (ctx.Instance.FlowState.IsFirstEnter != true)
            {
                return;
            }

            WfAction action = new WfAction();

            var actTitle = WfHelper.GetActionTitle(ctx);

            action.InstanceID = ctx.CurrentTask.InstanceID;
            action.TaskID = ctx.CurrentTask.TaskID;
            action.Grade = ctx.CurrentTask.Grade;
            action.Important = ctx.CurrentTask.Important;
            action.Type = ctx.CurrentTask.Type;
            action.Catalog = ctx.CurrentTask.Catalog;

            action.OwnerID = action.CloserID = action.CreatorID = PortalService.CurrentUserInfo.UserID;
            action.OwnerName = action.CloserName = action.CreatorName = PortalService.CurrentUserInfo.Name;

            action.Title = actTitle;

            action.StartedTime = DateTime.Now;
            action.ClosedTime = DateTime.Now;   // 首次任务由于立即执行，开始时间等于结束时间
            action.ActionStatus = WfAction.StatusEnum.Completed;

            FlowActionInfo actionInfo = new FlowActionInfo()
            {
                Comments = "提交流程"
            };

            action.ActionState.Request = new ActionRequest(actionInfo);

            ctx.CurrentTaskContext.DispatchedActions.Add(action);
        }

        /// <summary>
        /// 分发活动
        /// </summary>
        internal static void DispatchActions(FlowContext ctx, IList<OrgUser> actionUserList = null)
        {
            var actTitle = WfHelper.GetActionTitle(ctx);

            if (actionUserList == null)
            {
                actionUserList = GetActionUserList(ctx);
            }

            ctx.Instance.FlowState.Current.Request.ResetUserList(actionUserList); // 保存分发人

            if (actionUserList.Count == 0)
            {
                throw new Exception("No action executor, flow cannot continue.");
            }

            foreach (var usr in actionUserList)
            {
                WfAction action = new WfAction();

                action.OwnerID = usr.UserID;
                action.OwnerName = usr.Name;

                // 设置活动代理人
                var agent = WfHelper.GetUserAgent(usr);
                if (agent != null)
                {
                    action.AgentID = agent.UserID;
                    action.AgentName = agent.Name;
                }

                action.InstanceID = ctx.CurrentTask.InstanceID;
                action.TaskID = ctx.CurrentTask.TaskID;
                action.Grade = ctx.CurrentTask.Grade;
                action.Important = ctx.CurrentTask.Important;
                action.CreatorID = PortalService.CurrentUserInfo.UserID;
                action.CreatorName = PortalService.CurrentUserInfo.Name;
                action.Type = ctx.CurrentTask.Type;
                action.Catalog = ctx.CurrentTask.Catalog;

                action.Title = actTitle;

                if (ctx.CurrentTask.TaskStatus == WfTask.StatusEnum.Immediate)
                {
                    action.ActionStatus = WfAction.StatusEnum.Immediate;
                }
                else
                {
                    action.ActionStatus = WfAction.StatusEnum.New;
                }

                ctx.CurrentTaskContext.DispatchedActions.Add(action);
            }
        }

        /// <summary>
        /// 获取所有将分配任务的用户
        /// </summary>
        /// <param name="ctx"></param>
        internal static IList<OrgUser> GetActionUserList(FlowContext ctx)
        {
            IList<OrgUser> userList = ctx.FlowState.Current.ActorList.GetUserList(ctx);

            // 如果当前步骤没有审批人,发到默认审批人
            if (userList.Count == 0)
            {
                userList = WfActorHelper.GetDefaultActorUserList();
            }

            return userList;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendNotification()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Action 操作

        /// <summary>
        /// 执行活动
        /// </summary>
        /// <param name="actionReq"></param>
        internal static void RunAction(FlowContext ctx)
        {
            ctx.ValidateAction();

            if (ctx.CurrentAction.ActionStatus == WfAction.StatusEnum.Completed
                || ctx.CurrentAction.ActionStatus == WfAction.StatusEnum.Closed)
            {
                throw new MessageException("此流程已完成或已关闭无法重复执行。");
            }

            if (ctx.CurrentAction.ActionStatus == WfAction.StatusEnum.Aborted)
            {
                throw new MessageException("此流程已取消，无法重复执行，请联系管理员。");
            }

            if (ctx.CurrentAction.ActionStatus != WfAction.StatusEnum.New
                && ctx.CurrentAction.ActionStatus != WfAction.StatusEnum.Opened
                && ctx.CurrentAction.ActionStatus != WfAction.StatusEnum.Pending
                && ctx.CurrentAction.ActionStatus != WfAction.StatusEnum.Started)
            {
                throw new MessageException("无法执行此状态流程活动。");
            }

            if (ctx.CurrentAction.OwnerID != PortalService.CurrentUserInfo.UserID
                && ctx.CurrentAction.AgentID != PortalService.CurrentUserInfo.UserID)
            {
                throw new MessageException("只有流程活动的所有人或代理人可以执行此流程活动。");
            }

            switch (ctx.ActionRequest.Operation)
            {
                case FlowActionInfo.ActionOperation.Open:
                    OpenAction(ctx);
                    break;
                case FlowActionInfo.ActionOperation.Execute:
                    ExecuteAction(ctx);
                    break;
                case FlowActionInfo.ActionOperation.Close:
                case FlowActionInfo.ActionOperation.Complete:
                    CloseAction(ctx);
                    break;
            }
        }

        /// <summary>
        /// 打开作业
        /// </summary>
        internal static void OpenAction(FlowContext ctx)
        {
            ctx.CurrentAction.OpenorID = PortalService.CurrentUserInfo.UserID;
            ctx.CurrentAction.OpenorName = PortalService.CurrentUserInfo.Name;
            ctx.CurrentAction.OpenedTime = DateTime.Now;
            ctx.CurrentAction.ActionStatus = WfAction.StatusEnum.Opened;
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        internal static void ExecuteAction(FlowContext ctx)
        {
            ctx.CurrentAction.ExecutorID = PortalService.CurrentUserInfo.UserID;
            ctx.CurrentAction.ExecutorName = PortalService.CurrentUserInfo.Name;
            ctx.CurrentAction.StartedTime = DateTime.Now;
            ctx.CurrentAction.ActionStatus = WfAction.StatusEnum.Started;

            if (ctx.ActionRequest.CompletionRate > 0)
            {
                ctx.CurrentAction.Rate = ctx.ActionRequest.CompletionRate;
            }
        }

        /// <summary>
        /// 关闭作业
        /// </summary>
        internal static WfTask CloseAction(FlowContext ctx)
        {
            OnActionClose(ctx);

            WfTask task = null;

            if (ctx.ActionRequest.Operation == FlowActionInfo.ActionOperation.ForceEnd
                || ctx.CurrentAction.ActionType == WfTask.TypeEnum.Free)
            {
                ctx.CurrentAction.ActionStatus = WfAction.StatusEnum.Closed;
            }
            else
            {
                // 关闭作业时将检查作业类型并处理相关Task的动作(主要是完成动作)
                task = ctx.CurrentAction.GetTask();
                var actions = task.GetActions();

                ctx.CurrentTask = task;
                ctx.TaskRequest = new TaskRequest(ctx.ActionRequest);

                // 根据任务类型，执行任务
                switch (task.TaskType)
                {
                    case WfTask.TypeEnum.Auto:
                    case WfTask.TypeEnum.Parallel:
                        if (actions.Where(a => a.ActionID != ctx.CurrentAction.ActionID)
                            .All(tent => tent.ActionStatus == WfAction.StatusEnum.Closed || tent.ActionStatus == WfAction.StatusEnum.Completed))
                        {
                            WfServer.RunTask(ctx);
                        }
                        break;
                    case WfTask.TypeEnum.Single:
                        WfServer.RunTask(ctx);
                        break;
                    case WfTask.TypeEnum.Serial:
                        break;
                    case WfTask.TypeEnum.Multicast:
                        break;
                    case WfTask.TypeEnum.Custom:
                        break;
                    case WfTask.TypeEnum.Other:
                        break;
                }
            }

            return task;
        }

        #endregion

        #endregion

        #region 支持方法

        /// <summary>
        /// 实例初始化时，设置实例相关值
        /// </summary>
        /// <param name="ins"></param>
        private static void OnInstanceStart(FlowContext ctx)
        {
            ctx.Instance.InstanceState.WfFlowObjectType = ctx.Instance.FlowState.Request.FlowObjectType;
            ctx.Instance.InstanceStatus = WfInstance.StatusEnum.Started;
            ctx.Instance.StartedTime = DateTime.Now;

            ctx.Instance.OwnerID = PortalService.CurrentUserInfo.UserID;
            ctx.Instance.OwnerName = PortalService.CurrentUserInfo.Name;

            ctx.Instance.FlowState.BasicInfo.StartedTime = ctx.Instance.StartedTime;
            ctx.Instance.FlowState.BasicInfo.ApplicantID = PortalService.CurrentUserInfo.UserID;
            ctx.Instance.FlowState.BasicInfo.ApplicantName = PortalService.CurrentUserInfo.Name;
        }

        /// <summary>
        /// 实例结束
        /// </summary>
        /// <param name="ins"></param>
        private static void OnInstanceEnd(FlowContext ctx, string status = null)
        {
            ctx.Instance.Status = status;

            if (String.IsNullOrEmpty(ctx.Instance.Status))
            {
                ctx.Instance.InstanceStatus = WfInstance.StatusEnum.Completed;
            }

            ctx.Instance.EndedTime = DateTime.Now;

            var tasks = ctx.Instance.GetTasks();

            // 关闭所有相关任务
            foreach (var task in tasks)
            {
                switch (task.TaskStatus)
                {
                    case WfTask.StatusEnum.New:
                        task.TaskStatus = WfTask.StatusEnum.Closed;
                        task.EndedTime = DateTime.Now;

                        task.DoUpdate();
                        break;
                }
            }
        }

        /// <summary>
        /// 任务运行前调用
        /// </summary>
        /// <param name="task"></param>
        /// <param name="triggerAction"></param>
        private static void OnTaskRun(FlowContext ctx)
        {
            // 设置任务状态
            ctx.CurrentTask.TaskState.TriggerActionID = ctx.CurrentAction.ActionID;
            ctx.CurrentTask.TaskStatus = WfTask.StatusEnum.Completed;
            ctx.CurrentTask.EndedTime = DateTime.Now;
        }

        /// <summary>
        /// 活动关闭前调用
        /// </summary>
        private static void OnActionClose(FlowContext ctx)
        {
            ctx.CurrentAction.CloserID = PortalService.CurrentUserInfo.UserID;
            ctx.CurrentAction.CloserName = PortalService.CurrentUserInfo.Name;
            ctx.CurrentAction.ClosedTime = DateTime.Now;

            ctx.CurrentAction.ActionState.Request = ctx.ActionRequest;

            if (ctx.ActionRequest.CompletionRate > 0)
            {
                ctx.CurrentAction.Rate = ctx.ActionRequest.CompletionRate;
            }

            switch (ctx.ActionRequest.Operation)
            {
                case FlowActionInfo.ActionOperation.Complete:
                    ctx.CurrentAction.ActionStatus = WfAction.StatusEnum.Completed;
                    break;
                case FlowActionInfo.ActionOperation.Close:
                    ctx.CurrentAction.ActionStatus = WfAction.StatusEnum.Closed;
                    break;
            }
        }

        /// <summary>
        /// 获取流程应用
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private static WorkflowApplication GetWfApplication(FlowContext ctx, AutoResetEvent waitHandler, SynchronizationContext syncContext, IDictionary<string, object> inputs = null)
        {
            var def = ctx.Instance.GetDefine();
            var wfConfig = def.GetConfig();

            var flowObject = wfConfig.NewFlowObject();
            ctx.Instance.FlowState.BasicInfo.FlowObjectType = flowObject.GetType().AssemblyQualifiedName;

            WorkflowApplication wfApp = null;
            
            if (inputs != null)
            {
                if (!inputs.Keys.Contains("Context"))
                {
                    // 设置流程上下文
                    inputs.Add("Context", ctx);
                }
            }

            if (inputs == null)
            {
                wfApp = new WorkflowApplication(flowObject);
            }
            else
            {
                wfApp = new WorkflowApplication(flowObject, inputs);
            }

            var insStore = new SqlWorkflowInstanceStore(Instance.STORE_DB_CONNSTR)
            {
                InstanceLockedExceptionAction = InstanceLockedExceptionAction.BasicRetry,
                InstanceCompletionAction = InstanceCompletionAction.DeleteAll,
                HostLockRenewalPeriod = TimeSpan.FromSeconds(20),
                RunnableInstancesDetectionPeriod = TimeSpan.FromSeconds(3)
            };

            var insHandle = insStore.CreateInstanceHandle();
            var view = insStore.Execute(insHandle, new CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(20));
            insHandle.Free();
            insStore.DefaultInstanceOwner = view.InstanceOwner;

            wfApp.InstanceStore = insStore;

            if (syncContext != null)
            {
                wfApp.SynchronizationContext = syncContext;
            }

            wfApp.PersistableIdle = (e) =>
            {
                ctx.Persist();

                return PersistableIdleAction.Unload;
            };

            wfApp.OnUnhandledException = (e) =>
            {
                LogService.Log(e.UnhandledException, LogService.WorkflowException);

                // 这里抛出异常，防止清理流程持久化信息
                throw e.UnhandledException;

                // return UnhandledExceptionAction.Terminate;
            };

            wfApp.Aborted = (args) =>
            {
                LogService.Log(args.Reason, LogService.WorkflowException);

                if (syncContext != null)
                {
                    syncContext.OperationCompleted();
                }
            };

            wfApp.Unloaded = (e) =>
            {
                if (waitHandler != null)
                {
                    waitHandler.Set();
                }
            };

            wfApp.Completed = (args) =>
            {
                // ----------执行完成方法 Begin---------

                ctx.Instance.FlowState.Current.TaskStatus = WfTask.StatusEnum.Completed;
                ctx.Instance.FlowState.BasicInfo.Status = WfTask.StatusEnum.Completed.ToString();

                if (ctx.Instance.FlowState.Request.OnInstanceEnd != null)
                {
                    ctx.Instance.FlowState.Request.OnInstanceEnd(ctx);

                    ctx.Instance.FlowState.Request.OnInstanceEnd = null; // 确保只执行一次，并且不对此方法持久化
                }

                if (args.CompletionState == ActivityInstanceState.Closed)
                {
                    OnInstanceEnd(ctx); // Close为正常完成
                }
                else
                {
                    OnInstanceEnd(ctx, args.CompletionState.ToString());
                }

                ctx.Persist();

                WfHelper.SendFlowFinishNotification(ctx.Instance);

                // ----------执行完成方法 End---------

                if (syncContext != null)
                {
                    syncContext.OperationCompleted();
                }
            };

            return wfApp;
        }

        #endregion
    }
}
