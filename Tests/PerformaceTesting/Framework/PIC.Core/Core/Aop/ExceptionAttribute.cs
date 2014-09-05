using System;
using System.Collections.Generic;
using System.Text;

using PostSharp.Aspects;

namespace PIC.Aop
{
    /// <summary>
    /// 异常管理
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false), Serializable]
    public class ExceptionAttribute : OnExceptionAspect
    {
        public static MessageDelegate del;

        public override void OnException(MethodExecutionArgs eventArgs)
        {
            eventArgs.FlowBehavior = FlowBehavior.Return;
            string log = string.Format("出现异常：{0}", eventArgs.Exception.Message);

            if (del != null)
            {
                del(log);
            }
        }
    }
}
