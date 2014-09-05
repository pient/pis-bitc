using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 活动信息
    /// </summary>
    public class ActionSourceInfo
    {
        #region 枚举

        /// <summary>
        /// 活动源类型
        /// </summary>
        public enum TypeEnum
        {
            Task,   // 由任务产生
            Action, // 由活动直接产生，一般为自由活动
            Instance,   // 由主流程直接产生，不经过任务
            Free,   // 完全孤立自由活动，无法跟踪
            Other
        }

        public EasyDictionary Tag { get; set; }

        #endregion

        #region 属性

        public TypeEnum Type { get; set; }

        public string Source { get; set; }

        #endregion

        #region 构造函数

        public ActionSourceInfo()
        {
        }

        protected ActionSourceInfo(TypeEnum type)
        {
            this.Type = type;
            this.Tag = new EasyDictionary();
        }

        #endregion

        #region 静态函数

        /// <summary>
        /// 获取活动源信息
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ActionSourceInfo Get(FlowContext ctx)
        {
            if (ctx.CurrentTask != null && !String.IsNullOrEmpty(ctx.CurrentTask.TaskID))
            {
                return TaskSource(ctx.CurrentTask.TaskID);
            }
            else if (ctx.CurrentAction != null && !String.IsNullOrEmpty(ctx.CurrentAction.ActionID))
            {
                return TaskSource(ctx.CurrentAction.ActionID);
            }
            else
            {
                return FreeSource();
            }
        }

        public static ActionSourceInfo TaskSource(string taskID)
        {
            ActionSourceInfo sinfo = new ActionSourceInfo(TypeEnum.Task);

            sinfo.Source = taskID;

            return sinfo;
        }

        public static ActionSourceInfo ActionSource(string actionID)
        {
            ActionSourceInfo sinfo = new ActionSourceInfo(TypeEnum.Action);

            sinfo.Source = actionID;

            return sinfo;
        }

        public static ActionSourceInfo FreeSource()
        {
            ActionSourceInfo sinfo = new ActionSourceInfo(TypeEnum.Free);

            return sinfo;
        }

        #endregion
    }
}
