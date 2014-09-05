using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Pattern;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 当前的工作流环境
    /// </summary>
    public class WfUnitOfWork : UnitOfWork<WfDataContext>
    {
        #region 属性

        #endregion

        #region 构造函数

        internal WfUnitOfWork()
        {
        }

        #endregion
    }
}
