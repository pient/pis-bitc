using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Criterion;

namespace PIC.Data
{
    /// <summary>
    /// 可执行实体
    /// </summary>
    public abstract class ExecutableEntityBase<T> : EntityBase<T>, IExecutableEntity where T : EntityBase<T>
    {
        #region 成员

        #endregion

        #region 属性

        /// <summary>
        /// 执行状态标志
        /// </summary>
        public ExecuteStateEnum ExecuteStateFlag
        {
            get
            {
                return ExecutableEntityHelper.GetExecuteStateFlag(this.ExecuteState);
            }

            set
            {
                this.ExecuteState = value.ToString();
            }
        }

        #endregion

        #region 虚拟方法

        public abstract string ExecuteState { get; set; }

        #endregion
    }
}
