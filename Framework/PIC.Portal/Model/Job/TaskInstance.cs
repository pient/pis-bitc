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
	public partial class TaskInstance
    {
        #region 成员变量

        [NonSerialized]
        protected TaskActionResult insResult;

        #endregion

        #region 成员属性

        /// <summary>
        /// 任务实例运行结果
        /// </summary>
        [JsonIgnore]
        public TaskActionResult InstanceResult
        {
            get
            {
                if (insResult == null)
                {
                    if (String.IsNullOrEmpty(this.Result))
                    {
                        insResult = new TaskActionResult();
                    }
                    else
                    {
                        insResult = JsonHelper.GetObject<TaskActionResult>(this.Result);
                    }
                }

                return insResult;
            }

            internal set { this.insResult = value; }
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

            if (UserInfo != null)
            {
                this.CreatorID = UserInfo.UserID;
                this.CreatorName = UserInfo.Name;
            }

            this.CreatedTime = DateTime.Now;

            this.Result = JsonHelper.GetJsonString(this.InstanceResult);

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

            this.Result = JsonHelper.GetJsonString(this.InstanceResult);

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			TaskInstance[] tents = TaskInstance.FindAll(Expression.In("InstanceID", args));

			foreach (TaskInstance tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TaskInstance Get(string code)
        {
            TaskInstance ent = TaskInstance.FindFirstByProperties(TaskInstance.Prop_Code, code);

            return ent;
        }

        /// <summary>
        /// 开始任务实例
        /// </summary>
        /// <returns></returns>
        public static TaskInstance Start(Task task)
        {
            TaskInstance taskIns = new TaskInstance()
            {
                TaskID = task.TaskID,
                Name = task.Name
            };

            taskIns.Code = Template.RenderString("Sys.Code.Str.TaskInstance");
            taskIns.StartedTime = DateTime.Now;
            taskIns.Status = "Started";

            taskIns.DoCreate();

            return taskIns;
        }

        /// <summary>
        /// 任务实例结束
        /// </summary>
        /// <returns></returns>
        public static TaskInstance End(TaskInstance taskIns, TaskActionResult result)
        {
            taskIns.Status = "Ended";
            taskIns.EndedTime = DateTime.Now;
            taskIns.InstanceResult = result;

            taskIns.DoUpdate();

            return taskIns;
        }
        
        #endregion

    } // SysTaskInstance
}


