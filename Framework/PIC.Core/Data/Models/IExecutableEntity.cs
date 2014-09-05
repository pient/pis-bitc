using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    /// <summary>
    /// 执行状态枚举
    /// </summary>
    [Flags]
    public enum ExecuteStateEnum
    {
        Unstarted,  // 未执行
        Executing,  // 执行中
        Waiting,    // 等待执行（一般等待其他任务执行完成）
        Consigning, // 代理执行
        Suspended,  // 暂停
        Aborted,    // 取消
        Stopped,    // 停止
        Finish,     // 结束
        Other       // 其他状态
    }

    /// <summary>
    /// 可执行实体（用于流程实体）
    /// </summary>
    public interface IExecutableEntity
    {
        /// <summary>
        /// 执行状态标识
        /// </summary>
        string ExecuteState { get; set; }
    }
}
