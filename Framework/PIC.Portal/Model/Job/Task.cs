using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;
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
	public partial class Task
    {
        #region 枚举

        /// <summary>
        /// 任务类型枚举
        /// </summary>
        public enum TypeEnum
        {
            Assembly, // 加载程序集调用方法
            System,   // 系统任务（调用指定系统方法）
            Inner,    // 系统内部任务
            Unknown
        }

        /// <summary>
        /// 任务状态枚举
        /// </summary>
        public enum StatusEnum
        {
            Enabled,    // 有效
            Disabled,   // 无效
            Unknown
        }

        #endregion

        #region 成员变量

        #endregion

        #region 成员属性

        /// <summary>
        /// 任务类型
        /// </summary>
        [JsonIgnore]
        public TypeEnum TaskType
        {
            get { return CLRHelper.GetEnum<TypeEnum>(this.Type); }
            set { this.Type = value.ToString(); }
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        [JsonIgnore]
        public StatusEnum TaskStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status); }
            set { this.Status = value.ToString(); }
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

            this.TaskStatus = StatusEnum.Enabled;
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
        /// 运行任务
        /// </summary>
        public TaskActionResult Run(TaskActionArgs args = null)
        {
            TaskInstance taskIns = null;
            TaskActionResult result = null;

            if (args == null)
            {
                args = new TaskActionArgs();
            }

            args.Task = this;

            try
            {
                taskIns = TaskInstance.Start(this);

                switch (this.TaskType)
                {
                    case TypeEnum.Assembly:
                        result = this.ExecuteAssemblyTask(args);
                        break;
                    case TypeEnum.System:
                    case TypeEnum.Inner:
                        result = this.ExecuteInnerTask(args);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);

                result = new TaskActionResult(ex);
            }
            finally
            {
                TaskInstance.End(taskIns, result);
            }

            return result;
        }

        #endregion
        
        #region 静态成员

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="code"></param>
        public static void RunTask(string taskid)
        {
            Task task = Task.Find(taskid);
            task.Run();
        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			Task[] tents = Task.FindAll(Expression.In("TaskID", args));

			foreach (Task tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Task Get(string code)
        {
            Task ent = Task.FindFirstByProperties(Task.Prop_Code, code);

            return ent;
        }
        
        #endregion

        #region 静态方法

        /// <summary>
        /// 执行程序集任务
        /// </summary>
        /// <returns></returns>
        private TaskActionResult ExecuteAssemblyTask(TaskActionArgs args)
        {
            TaskActionResult result = null;

            AssemblyTaskConfig config = JsonHelper.GetObject<AssemblyTaskConfig>(this.Config);

            if (config != null)
            {
                ITaskAction action = null;

                if (!String.IsNullOrEmpty(config.AssemblyFileName))
                {
                    Assembly assem = Assembly.LoadFile(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, config.AssemblyFileName));
                    action = assem.CreateInstance(config.AssemblyName) as ITaskAction;
                }
                else
                {
                    action = Activator.CreateInstance(System.Type.GetType(config.AssemblyName)) as ITaskAction;
                }

                result = action.Execute(args);
            }

            return result;
        }

        /// <summary>
        /// 执行内部任务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private TaskActionResult ExecuteInnerTask(TaskActionArgs args)
        {
            var taskAction = new Services.SysInnerTaskAction();

            var result = taskAction.Execute(args);

            return result;
        }

        #endregion

    } // SysTask
}


