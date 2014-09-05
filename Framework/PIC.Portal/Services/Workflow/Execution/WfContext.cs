using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 当前的工作流环境
    /// </summary>
    public sealed class WfContext
    {
        #region 静态变量

        private static object lockerobj = new object(); // 添加一个对象作为WfContextDict的锁

        private static EasyDictionary<string, WfContext> WfContextDict = new EasyDictionary<string, WfContext>();

        #endregion

        #region 属性

        public WfInstance WfInstance { get; private set; }

        internal WfUnitOfWork UnitOfWork { get; private set; }

        #endregion

        #region 构造函数

        private WfContext(WfInstance ins)
        {
            this.WfInstance = ins;

            UnitOfWork = new WfUnitOfWork();
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建新的WfContext, 原来的将被丢弃
        /// </summary>
        /// <param name="wfIns"></param>
        /// <returns></returns>
        public static WfContext NewWfContext(WfInstance wfIns)
        {
            var ctx = new WfContext(wfIns);

            SetContext(ctx);

            return ctx;
        }

        /// <summary>
        /// 获取WfContext
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public static WfContext GetWfContext(string instanceID)
        {
            return WfContextDict.Get(instanceID);
        }

        #endregion

        #region 支持方法

        /// <summary>
        /// 添加上下文
        /// </summary>
        /// <param name="ctx"></param>
        private static void SetContext(WfContext ctx)
        {
            lock (lockerobj)
            {
                WfContextDict.Set(ctx.WfInstance.InstanceID, ctx);
            }
        }

        /// <summary>
        /// 移除上下文
        /// </summary>
        /// <param name="ctx"></param>
        private static void RemoveContext(string instanceID)
        {
            lock (lockerobj)
            {
                WfContextDict.Remove(instanceID);
            }
        }

        /// <summary>
        /// 清理上下文
        /// </summary>
        /// <param name="ctx"></param>
        private static void ClearContext(string instanceID)
        {
            lock (lockerobj)
            {
                WfContextDict.Clear();
            }
        }

        #endregion
    }
}
