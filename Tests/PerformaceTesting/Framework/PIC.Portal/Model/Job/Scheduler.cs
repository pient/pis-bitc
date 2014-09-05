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
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class Scheduler
    {
        #region 枚举

        /// <summary>
        /// 流程定义状态枚举
        /// </summary>
        public enum ExecStatusEnum
        {
            New,
            Running,    // 正在运行
            Started,    // 已启动，有效的
            Stopped,    // 已停止，失活的
            Unknown
        }

        /// <summary>
        /// 流程定义状态枚举
        /// </summary>
        public enum StatusEnum
        {
            Enabled,    // 正在运行
            Disabled
        }

        #endregion

        #region 成员变量

        [NonSerialized]
        protected SchedulerConfig schedulerConfig;

        #endregion

        #region 成员属性

        /// <summary>
        /// 任务计划状态
        /// </summary>
        [JsonIgnore]
        public StatusEnum SchedulerStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status); }
            set { this.Status = value.ToString(); }
        }

        /// <summary>
        /// 执行计划配置
        /// </summary>
        [JsonIgnore]
        public SchedulerConfig SchedulerConfig
        {
            get
            {
                if (schedulerConfig == null)
                {
                    if (String.IsNullOrEmpty(this.Config))
                    {
                        schedulerConfig = new SchedulerConfig();
                    }
                    else
                    {
                        schedulerConfig = JsonHelper.GetObject<SchedulerConfig>(this.Config);
                    }
                }

                return schedulerConfig;
            }

            internal set { this.schedulerConfig = value; }
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
            if (String.IsNullOrEmpty(SchedulerID))
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

            this.SchedulerStatus = StatusEnum.Enabled;

            this.CreatorID = UserInfo.UserID;
            this.CreatorName = UserInfo.Name;

            this.CreatedDate = DateTime.Now;

            this.Config = JsonHelper.GetJsonString(this.schedulerConfig);

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

            this.LastModifiedDate = DateTime.Now;

            if (this.SchedulerConfig != null)
            {
                this.Config = JsonHelper.GetJsonString(this.schedulerConfig);
            }

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
        /// 立即执行任务
        /// </summary>
        public TaskActionResult RunTask()
        {
            TaskActionResult result = null;

            if (this.SchedulerConfig != null 
                && !String.IsNullOrEmpty(this.SchedulerConfig.TaskID))
            {
                Task task = Task.Find(this.SchedulerConfig.TaskID);

                TaskActionArgs args = new TaskActionArgs
                {
                    Scheduler = this
                };

                result = task.Run(args);
            }

            this.LastExecutedTime = DateTime.Now;

            return result;
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            Scheduler[] tents = Scheduler.FindAll(Expression.In("SchedulerID", args));

			foreach (Scheduler tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Scheduler Get(string code)
        {
            Scheduler ent = Scheduler.FindFirstByProperties(Scheduler.Prop_Code, code);

            return ent;
        }
        
        #endregion

    } // SysScheduler
}


