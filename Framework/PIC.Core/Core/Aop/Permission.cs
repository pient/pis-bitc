using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;

namespace PIC.Aop
{
    public delegate void FuncDelegate(MethodInterceptionArgs args);

    /// <summary>
    /// 权限许可
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false), Serializable]
    public class PermissionAttribute : MethodInterceptionAspect
    {
        public static FuncDelegate del;
        public override void OnInvoke(MethodInterceptionArgs eventArgs)
        {
            //eventArgs.Proceed();
            //string log = "实体:{0},操作:{1}!";

            if (del != null)
            {
                del(eventArgs);
            }
        }
    }
}
