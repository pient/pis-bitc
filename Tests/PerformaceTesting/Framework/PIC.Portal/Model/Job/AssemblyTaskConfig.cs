using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Model
{
    public class AssemblyTaskConfig : SysTaskConfig
    {
        #region 属性

        /// <summary>
        /// 对象
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 程序集文件
        /// </summary>
        public string AssemblyFileName { get; set; }

        public string MethodName { get; set; }

        public IList<object> Params { get; set; }

        #endregion

        #region 构造函数

        public AssemblyTaskConfig()
            : base()
        {
        }

        #endregion
    }
}
