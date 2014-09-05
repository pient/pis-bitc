using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
using PIC.Portal.Workflow;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class WfInstance
    {
        #region 枚举

        /// <summary>
        /// 流程定义状态枚举
        /// </summary>
        public enum StatusEnum
        {
            Draft,
            New,
            Started,
            Paused,
            Canceled,
            Completed,
            Aborted,
            Unknown
        }

        #endregion

        #region 成员变量

        [NonSerialized]
        protected WfInstanceState instanceState;

        #endregion

        #region 成员属性

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public StatusEnum InstanceStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status, StatusEnum.Unknown); }
            set { this.Status = value.ToString(); }
        }

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public WfInstanceState InstanceState
        {
            get
            {
                if (instanceState == null)
                {
                    if (String.IsNullOrEmpty(this.Tag))
                    {
                        instanceState = new WfInstanceState();
                    }
                    else
                    {
                        instanceState = JsonHelper.GetObject<WfInstanceState>(this.Tag);
                    }
                }

                return instanceState;
            }

            internal set { this.instanceState = value; }
        }

        /// <summary>
        /// 流程状态信息
        /// </summary>
        [JsonIgnore]
        public FlowState FlowState
        {
            get
            {
                return this.InstanceState.WfFlowInfo.FlowState;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            /*if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }*/
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(InstanceID))
            {
                this.DoCreate();
            }
            else
            {
                this.DoUpdate();
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            if (String.IsNullOrEmpty(this.Code))
            {
                this.Code = Model.Template.RenderString("Sys.Code.Str.WfInstance");
            }

            if (String.IsNullOrEmpty(this.Status))
            {
                this.InstanceStatus = StatusEnum.New;
            }

            if (String.IsNullOrEmpty(this.CreatorName))
            {
                this.CreatorID = UserInfo.UserID;
                this.CreatorName = UserInfo.Name;
            }

            this.CreatedTime = DateTime.Now;

            this.Tag = JsonHelper.GetJsonString(this.instanceState);

            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedTime = DateTime.Now;

            this.Tag = JsonHelper.GetJsonString(this.instanceState);

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            if (this.InstanceStatus != StatusEnum.New
                && this.InstanceStatus != StatusEnum.Draft)
            {
                throw new MessageException("只有新建和草稿状态的实例可以删除。");
            }

            this.Delete();
        }

        /// <summary>
        /// 作废当前流程
        /// </summary>
        public void DoDiscard()
        {
            ValidationResult vResult = this.CheckIsAllowDiscardBy(PortalService.CurrentUserInfo);

            WfService.Exec(WfServer.WfExecCommand.Abort, this.InstanceID);
        }

        /// <summary>
        /// 检查是否允许某人作废当前流程
        /// </summary>
        /// <param name="user"></param>
        public ValidationResult CheckIsAllowDiscard()
        {
            return CheckIsAllowDiscardBy(PortalService.CurrentUserInfo);
        }

        /// <summary>
        /// 检查是否允许某人作废当前流程
        /// </summary>
        /// <param name="user"></param>
        public ValidationResult CheckIsAllowDiscardBy(IUserInfo user)
        {
            ValidationResult vResult = new ValidationResult();

            if (this.InstanceStatus != StatusEnum.Started)
            {
                vResult.AddErrorMessage("只有已启动的流程可以作废。");

                return vResult;
            }

            // 当前任务编码为开始，且当前用户为申请人时或者当前处理过的用户只有申请人时才可以作废
            if (!StringHelper.IsEqualsIgnoreCase(this.FlowState.Current.TaskCode, "START")
                || !StringHelper.IsEqualsIgnoreCase(this.OwnerID, user.GetMinUserInfo().UserID))
            {
                IList<WfAction> acts = this.GetActions();

                if(acts.Any(
                    a=>a.ActionStatus != WfAction.StatusEnum.New
                    && !StringHelper.IsEqualsIgnoreCase(a.OwnerID, user.GetMinUserInfo().UserID)))
                {
                    vResult.AddErrorMessage("只有流程没有任何人审批，申请人在开始任务时或者才可以作废流程。");
                }
            }

            return vResult;
        }

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <returns></returns>
        public WfDefine GetDefine()
        {
            var define = WfDefine.Find(this.DefineID);

            return define;
        }

        /// <summary>
        /// 获取实例关联任务
        /// </summary>
        /// <returns></returns>
        public IList<WfTask> GetTasks()
        {
            IList<Order> orders = new List<Order>();
            orders.Add(new Order(WfTask.Prop_CreatedTime, false));
            orders.Add(new Order(WfTask.Prop_EndedTime, false));

            var tasks = WfTask.FindAll(orders.ToArray(), Expression.Eq(WfTask.Prop_InstanceID, this.InstanceID));

            return tasks;
        }

        /// <summary>
        /// 获取实例关联活动
        /// </summary>
        /// <returns></returns>
        public IList<WfAction> GetActions()
        {
            IList<Order> orders = new List<Order>();
            orders.Add(new Order(WfAction.Prop_CreatedTime, false));
            orders.Add(new Order(WfAction.Prop_ClosedTime, false));

            var actions = WfAction.FindAll(orders.ToArray(), Expression.Eq(WfAction.Prop_InstanceID, this.InstanceID));

            return actions;
        }

        /// <summary>
        /// 获取某个编码最后一个任务
        /// </summary>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public WfTask GetLastTask(string taskCode)
        {
            // 按关闭时间排序
            IList<Order> orders = new List<Order>();
            orders.Add(new Order(WfTask.Prop_EndedTime, false));

            IList<ICriterion> crits = new List<ICriterion>();

            crits.Add(Expression.Eq(WfTask.Prop_InstanceID, this.InstanceID));
            crits.Add(Expression.Eq(WfTask.Prop_Code, taskCode));

            // 必须是已完成的活动
            crits.Add(Expression.IsNotNull(WfTask.Prop_EndedTime));
            crits.Add(Expression.Eq(WfTask.Prop_Status, WfTask.StatusEnum.Completed.ToString()));
            crits.Add(Expression.Not(Expression.Eq(WfTask.Prop_Type, WfTask.TypeEnum.Immediate.ToString())));

            WfTask task = WfTask.FindFirst(orders.ToArray(), crits.ToArray());

            return task;
        }

        /// <summary>
        /// 获取某个阶段最后一个活动, NVelocity模板不支持默认参数，这里需要新方法
        /// </summary>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public WfAction GetLastAction(string taskCode)
        {
            WfAction action = GetLastAction(taskCode, null);

            return action;
        }

        /// <summary>
        /// 获取某个阶段、某个角色的最后一个活动
        /// </summary>
        /// <param name="taskCode"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public WfAction GetLastAction(string taskCode, string roleCode)
        {
            WfAction action = null;

            WfTask task = this.GetLastTask(taskCode);

            if (task != null)
            {
                action = task.GetLastAction(roleCode);
            }

            return action;
        }

        #endregion

        #region 静态方法
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			WfInstance[] tents = WfInstance.FindAll(Expression.In("InstanceID", args));

			foreach (WfInstance tent in tents)
			{
				tent.DoDelete();
			}
        }
        
        #endregion

        #region 流程方法

        #endregion

    } // WfInstance
}


