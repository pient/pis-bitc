using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PIC.Portal.Model
{
    [Serializable]
    public class TaskActionResult
    {
        #region 枚举

        public enum ResultEnum
        {
            Completed,
            Failed,
            None
        }

        #endregion

        #region 属性

        /// <summary>
        /// 结果
        /// </summary>
        public ResultEnum Result { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public object Return { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string ExtInfo { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        #endregion

        #region 构造函数

        public TaskActionResult()
        {
            this.Result = ResultEnum.None;
        }

        public TaskActionResult(Exception ex)
        {
            this.Result = ResultEnum.Failed;
            this.Message = ex.Message;
            this.ExtInfo = ex.StackTrace;
        }

        public TaskActionResult(ResultEnum result, string message)
        {
            this.Result = result;
            this.Message = message;
        }

        #endregion
    }

    public class TaskActionArgs
    {
        #region 属性

        /// <summary>
        /// 任务计划信息
        /// </summary>
        public Scheduler Scheduler { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public EasyDictionary Params { get; internal set; }

        #endregion

        #region 构造函数

        public TaskActionArgs()
        {
            Params = new EasyDictionary();
        }

        #endregion
    }

    public interface ITaskAction
    {
        TaskActionResult Execute(TaskActionArgs args);
    }
}
