using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Portal.Model;
using System.Runtime.Serialization;
using PIC.Data;
using Castle.ActiveRecord;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Threading;
using PIC.Common.Authentication;
using PIC.Portal.Template;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 当前的工作流环境
    /// </summary>
    [DataContract]
    public sealed class FlowContext
    {
        #region 成员

        private string _userSessionID = null;
        private bool _isPersisted = false;

        private EasyCollection<TaskContext> _taskContexts = null;

        #endregion

        #region 属性

        /// <summary>
        /// 是否已经持久化
        /// </summary>
        public bool IsPersisted
        {
            get { return _isPersisted; }
        }

        public WfInstance Instance { get; private set; }

        public WfTask CurrentTask { get; set; }

        public WfAction CurrentAction { get; set; }

        public TaskRequest TaskRequest { get; set; }

        public ActionRequest ActionRequest { get; set; }

        public TaskContext CurrentTaskContext { get; private set; }

        public FlowState FlowState
        {
            get
            {
                if (Instance != null)
                {
                    return Instance.FlowState;
                }

                return null;
            }
        }

        public string NextRouteCode
        {
            get
            {
                string routeCode = null;

                if (FlowState != null)
                {
                    routeCode = FlowState.NextRouteCode;
                }

                if (String.IsNullOrEmpty(routeCode) && TaskRequest != null)
                {
                    routeCode = TaskRequest.NextRouteCode;
                }

                if (String.IsNullOrEmpty(routeCode) && ActionRequest != null)
                {
                    routeCode = ActionRequest.RouteCode;
                }

                return routeCode;
            }
        }

        /// <summary>
        /// 当前令牌
        /// </summary>
        public string UserSessionID
        {
            get { return _userSessionID; }
            internal set { _userSessionID = value; }
        }

        #endregion

        #region 构造函数

        private FlowContext()
        {
            _isPersisted = false;
            _taskContexts = new EasyCollection<TaskContext>();

            var _principal=Thread.CurrentPrincipal as SysPrincipal;

            if (_principal != null)
            {
                var _identity = _principal.Identity as SysIdentity;

                if (_identity != null)
                {
                    _userSessionID = _identity.UserSID;
                }
            }
        }

        public FlowContext(WfInstance ins)
            : this()
        {
            this.Instance = ins;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证任务有效性，针对Action
        /// </summary>
        public bool ValidateTask()
        {
            if (this.CurrentTask == null)
            {
                throw new MessageException("必须提供任务对象");
            }

            if (this.Instance.InstanceID != this.CurrentTask.InstanceID
                || this.Instance.InstanceID != this.CurrentAction.InstanceID)
            {
                throw new MessageException("实例与任务或活动不对应!");
            }

            if (this.CurrentTask.TaskID != this.CurrentAction.TaskID)
            {
                throw new MessageException("任务与活动不对应!");
            }

            return true;
        }

        /// <summary>
        /// 验证活动有效性
        /// </summary>
        public bool ValidateAction()
        {
            if (CurrentAction == null)
            {
                throw new MessageException("必须提供活动对象");
            }

            if (Instance.InstanceID != CurrentAction.InstanceID)
            {
                throw new MessageException("实例与活动不对应");
            }

            if (ActionRequest != null)
            {
                if (ActionRequest.ActionID != CurrentAction.ActionID)
                {
                    throw new MessageException("活动请求不对应");
                }
            }

            return true;
        }

        /// <summary>
        /// 检查活动请求是否合法
        /// </summary>
        /// <param name="actionReq"></param>
        /// <returns></returns>
        public bool ValidateActionRequest()
        {
            if (String.IsNullOrEmpty(this.ActionRequest.ActionID))
            {
                throw new MessageException("任务编号不能为空！");
            }

            // 验证RouteCode是否存在
            var fdefine = this.Instance.GetDefine();

            var taskDict = WfHelper.GetNextTaskStates(fdefine, this.Instance.FlowState.Current.TaskCode);

            if (taskDict.Count > 0)
            {
                if (!taskDict.Keys.Contains(this.ActionRequest.RouteCode))
                {
                    throw new MessageException("请求的路径编码不正确！");
                }
            }

            return true;
        }

        /// <summary>
        /// 新的任务上下文
        /// </summary>
        /// <returns></returns>
        public TaskContext NewTaskContext(WfTask task = null)
        {
            if (task == null)
            {
                task = new WfTask();
            }

            this.CurrentTask = task;

            var taskCtx = new TaskContext(this.CurrentTask);

            _taskContexts.Add(taskCtx);

            this.CurrentTaskContext = taskCtx;

            return taskCtx;
        }

        public void Apply(FlowContext fCtx)
        {
            _isPersisted = fCtx.IsPersisted;
            _taskContexts = fCtx._taskContexts;
            _userSessionID = fCtx._userSessionID;

            Instance = fCtx.Instance;

            ActionRequest = fCtx.ActionRequest;
            CurrentAction = fCtx.CurrentAction;

            TaskRequest = fCtx.TaskRequest;
            CurrentTask = fCtx.CurrentTask;
        }

        /// <summary>
        /// 重设
        /// </summary>
        public void Reset(WfInstance ins)
        {
            _isPersisted = false;
            _taskContexts = new EasyCollection<TaskContext>();

            this.Instance = ins;

            ActionRequest = null;
            CurrentAction = null;

            TaskRequest = null;
            CurrentTask = null;
        }

		/// <summary>
		/// 持久化
		/// </summary>
        [ActiveRecordTransaction]
        public void Persist()
        {
            if (_isPersisted == false)
            {
                using (new SessionScope())
                {
                    if (CurrentAction != null)
                    {
                        CurrentAction.DoSave();
                    }

                    if (CurrentTask != null)
                    {
                        CurrentTask.DoSave();
                    }

                    if (Instance != null)
                    {
                        Instance.DoSave();
                    }

                    foreach (var taskCtx in _taskContexts)
                    {
                        if (taskCtx.Task != CurrentTask)
                        {
                            taskCtx.Task.DoSave();
                        }

                        if (taskCtx.DispatchedActions != null && taskCtx.DispatchedActions.Count > 0)
                        {
                            foreach (var act in taskCtx.DispatchedActions)
                            {
                                if (String.IsNullOrEmpty(act.TaskID))
                                {
                                    act.TaskID = taskCtx.Task.TaskID;
                                }

                                act.DoSave();
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("不可以重复持久化，请重设Context后再执行持久化。");
            }

            _isPersisted = true;
        }

        #endregion
    }

    [DataContract]
    public sealed class TaskContext
    {
        #region 属性

        public WfTask Task { get; set; }

        public EasyCollection<WfAction> DispatchedActions { get; set; }

        #endregion

        #region 构造函数

        private TaskContext()
        {
        }

        public TaskContext(WfTask task)
        {
            this.Task = task;

            this.DispatchedActions = new EasyCollection<WfAction>();
        }

        #endregion
    }
}
