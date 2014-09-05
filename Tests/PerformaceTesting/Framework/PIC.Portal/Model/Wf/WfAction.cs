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
	public partial class WfAction
    {
        #region 枚举

        /// <summary>
        /// 流程定义状态枚举
        /// </summary>
        public enum StatusEnum
        {
            New,
            Opened,
            Started,
            Pending,
            Immediate,
            Completed,
            Closed,
            Aborted,      // 中断
            Unknown
        }

        #endregion

        #region 成员变量

        [NonSerialized]
        protected WfActionState actionState;

        #endregion

        #region 成员属性

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public StatusEnum ActionStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status, StatusEnum.Unknown); }
            set { this.Status = value.ToString(); }
        }

        /// <summary>
        /// 活动类型
        /// </summary>
        [JsonIgnore]
        public WfTask.TypeEnum ActionType
        {
            get { return CLRHelper.GetEnum<WfTask.TypeEnum>(this.Type, WfTask.TypeEnum.Other); }
            set { this.Type = value.ToString(); }
        }

        /// <summary>
        /// 流程活动状态
        /// </summary>
        [JsonIgnore]
        public WfActionState ActionState
        {
            get
            {
                if (actionState == null)
                {
                    if (String.IsNullOrEmpty(this.Tag))
                    {
                        actionState = new WfActionState();
                    }
                    else
                    {
                        actionState = JsonHelper.GetObject<WfActionState>(this.Tag);
                    }
                }

                return actionState;
            }

            internal set { this.actionState = value; }
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
            if (String.IsNullOrEmpty(ActionID))
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

            this.Tag = JsonHelper.GetJsonString(this.actionState);

            this.CreatorID = UserInfo.Name;
            this.CreatorName = UserInfo.Name;
            this.CreatedTime = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();

            // 创建事务的同时会发送邮件
            WfHelper.SendTaskNotification(this);
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            if (actionState != null)
            {
                this.Tag = JsonHelper.GetJsonString(this.actionState);
            }

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
        /// 获取对应的流程任务
        /// </summary>
        /// <returns></returns>
        public WfTask GetTask()
        {
            var task = WfTask.Find(this.TaskID);

            return task;
        }

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <returns></returns>
        public WfInstance GetWfInstance()
        {
            var ins = WfInstance.FindByPrimaryKey(this.InstanceID);

            return ins;
        }

        /// <summary>
        /// 获取意见信息
        /// </summary>
        /// <returns></returns>
        public string GetComments()
        {
            String comments = null;

            if (this.ActionState != null && this.ActionState.Request != null)
            {
                comments = this.ActionState.Request.Comments;
            }

            return comments;
        }

        /// <summary>
        /// 获取签字
        /// </summary>
        /// <returns></returns>
        public OrgUserSignature GetSignature()
        {
            if (!String.IsNullOrEmpty(this.CloserID))
            {
                OrgUser closer = OrgUser.Find(this.CloserID);

                return closer.GetSignature();
            }

            return null;
        }

        #endregion

        #region 静态成员

        public static IList<WfAction> Get(string ownerId, StatusEnum status)
        {
            IList<WfAction> actions = WfAction.FindAll(
                Expression.Eq(Prop_OwnerID, ownerId),
                Expression.Eq(Prop_Status, status.ToString()));

            return actions;
        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			WfAction[] tents = WfAction.FindAll(Expression.In("ActionID", args));

			foreach (WfAction tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        
        #endregion

    } // SysWfAction
}


