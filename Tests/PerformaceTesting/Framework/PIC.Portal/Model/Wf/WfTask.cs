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
using NVelocity;
using System.Activities.Statements;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class WfTask
    {
        #region 枚举

        /// <summary>
        /// 流程定义状态枚举
        /// </summary>
        public enum StatusEnum
        {
            New,
            Started,
            Paused,
            Canceled,
            Stopped,
            Immediate,
            Completed,
            Closed,
            ForceEnd,
            Unknown
        }

        /// <summary>
        /// 任务/活动类型
        /// </summary>
        public enum TypeEnum
        {
            Auto,       // 默认类型（看情形自动转化为其他类别）
            Immediate,  // 立即任务，当TaskActivity设为将不会再此暂停流程
            Single,     // 单任务（若生成多个Action，任意一个完成则任务结束）
            Serial,     // 串行（若发给多人，则按顺序依次生成下一个Action，所有人Action完成后任务结束）
            Parallel,   // 并行（同时生成多个Action，本Task下所有Action完成则任务结束）
            Multicast,  // 多播（不跟踪Action状态，生成Action后任务即结束）
            Custom,     // 自定义流程执行逻辑
            Free,       // 自由任务（不通过Task直接产生的Action，无法通过Task跟踪）
            Other
        }

        #endregion

        #region 成员变量

        [NonSerialized]
        protected WfTaskState taskState;

        #endregion

        #region 成员属性

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public StatusEnum TaskStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status, StatusEnum.Unknown); }
            set { this.Status = value.ToString(); }
        }

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public TypeEnum TaskType
        {
            get { return CLRHelper.GetEnum<TypeEnum>(this.Type, TypeEnum.Other); }
            set { this.Type = value.ToString(); }
        }

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public WfTaskState TaskState
        {
            get
            {
                if (taskState == null)
                {
                    if (String.IsNullOrEmpty(this.Tag))
                    {
                        taskState = new WfTaskState();
                    }
                    else
                    {
                        taskState = JsonHelper.GetObject<WfTaskState>(this.Tag);
                    }
                }

                return taskState;
            }

            internal set { this.taskState = value; }
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
            if (String.IsNullOrEmpty(TaskID))
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

            this.Tag = JsonHelper.GetJsonString(this.taskState);

            this.CreatorID = UserInfo.UserID;
            this.CreatorName = UserInfo.Name;
            this.CreatedTime = DateTime.Now;

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

            this.Tag = JsonHelper.GetJsonString(this.taskState);

            this.LastModifiedTime = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        /// <summary>
        /// 获取任务关联实例
        /// </summary>
        /// <returns></returns>
        public WfInstance GetWfInstance()
        {
            var ins = WfInstance.Find(this.InstanceID);

            return ins;
        }

        /// <summary>
        /// 获取任务相关活动
        /// </summary>
        /// <returns></returns>
        public IList<WfAction> GetActions()
        {
            var actions = WfAction.FindAllByProperty(WfAction.Prop_TaskID, this.TaskID);

            return actions;
        }

        /// <summary>
        /// 获取某个阶段最后一个活动, NVelocity模板不支持默认参数，这里需要新方法
        /// </summary>
        /// <returns></returns>
        public WfAction GetLastAction()
        {
            WfAction action = GetLastAction(null);

            return action;
        }

        /// <summary>
        /// 获取某个阶段、某个角色的最后一个活动
        /// </summary>
        /// <param name="taskCode"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public WfAction GetLastAction(string roleCode = null)
        {
            // 按关闭时间排序
            IList<Order> orders = new List<Order>();
            orders.Add(new Order(WfAction.Prop_ClosedTime, false));

            IList<ICriterion> crits = new List<ICriterion>();

            crits.Add(Expression.Eq(WfAction.Prop_InstanceID, this.InstanceID));
            crits.Add(Expression.Eq(WfAction.Prop_TaskID, this.TaskID));

            // 必须是已完成的活动
            crits.Add(Expression.IsNotNull(WfAction.Prop_ClosedTime));
            crits.Add(Expression.Eq(WfAction.Prop_Status, WfAction.StatusEnum.Completed.ToString()));

            if (!String.IsNullOrEmpty(roleCode))
            {
                crits.Add(Expression.Eq(WfAction.Prop_RoleCode, roleCode));
            }

            WfAction action = WfAction.FindFirst(orders.ToArray(), crits.ToArray());

            return action;
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			WfTask[] tents = WfTask.FindAll(Expression.In("TaskID", args));

			foreach (WfTask tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        
        #endregion

    } // SysWfTask
}


