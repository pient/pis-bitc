using System;
using System.Collections.Generic;
using System.Text;
using PostSharp.Aspects;

namespace PIC.Aop
{
    //信息代理
    public delegate void MessageDelegate(string message);

    /// <summary>
    /// 操作日志管理
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false), Serializable]
    public class LogAttribute : OnMethodBoundaryAspect
    {
        public static MessageDelegate del;
        public override void OnEntry(MethodExecutionArgs eventArgs)
        {
            string log = "实体:{0},操作:{1}!";

            log = String.Format(log, eventArgs.Instance.GetType().ToString(), eventArgs.Method.Name);

            if (del != null)
                del(log);
        }

        public override void OnExit(MethodExecutionArgs eventArgs)
        {
            
        }
    }
}
