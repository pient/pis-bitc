using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程操作信息（一般用于将流程信息传给Bus页面）
    /// </summary>
    [Serializable]
    public class FlowActionInfo
    {
        #region 枚举

        /// <summary>
        /// 活动操作类型
        /// </summary>
        public enum ActionOperation
        {
            Complete,    // 完成活动
            Close,       // 关闭活动
            Open,       // 打开活动
            Execute,    // 执行活动
            Pending,    // 闲置活动
            Abort,      // 中断活动
            ForceEnd   // 强制结束本任务，中断未执行的Action(Abort)
        }

        #endregion

        #region 成员属性

        /// <summary>
        /// 活动标识
        /// </summary>
        public string ActionID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public ActionOperation Operation { get; set; }

        /// <summary>
        /// 路径编码
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 提交路径目标编码
        /// </summary>
        public string TargetCode { get; set; }

        /// <summary>
        /// 提交路径目标名称
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 完成率
        /// </summary>
        public decimal CompletionRate { get; set; }

        /// <summary>
        /// 意见评论
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachments { get; set; }

        public EasyDictionary Tag { get; set; }

        #endregion

        #region 构造函数 

        public FlowActionInfo()
        {
            this.Operation = ActionOperation.Complete;  // 默认请求为完成
        }

        #endregion

    }
}
