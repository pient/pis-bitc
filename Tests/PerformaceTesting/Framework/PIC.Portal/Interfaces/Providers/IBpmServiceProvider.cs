using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using NServiceBus;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Portal.Workflow;

namespace PIC.Portal
{
    /// <summary>
    /// 业务流程服务支持类
    /// </summary>
    public interface IBpmServiceProvider
    {
        /// <summary>
        /// 执行服务
        /// </summary>
        /// <returns></returns>
        OperationResult RunService(FlowRequest reqObject);

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmdName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationResult ExecCommand(string cmdName, string insId, EasyDictionary args = null);
    }
}
