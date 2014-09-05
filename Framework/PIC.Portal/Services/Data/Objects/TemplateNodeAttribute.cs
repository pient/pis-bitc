using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    /// <summary>
    /// 模版节点特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class TemplateNodeAttribute : BaseAttribute
    {
        #region 构造函数

        public TemplateNodeAttribute()
        {
        }

        #endregion

        #region 成员属性

        /// <summary>
        /// 预处理器
        /// </summary>
        public Type Processor
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 模版节点属性特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TemplateNodePropertyAttribute : BaseAttribute
    {
        #region 构造函数

        public TemplateNodePropertyAttribute()
        {
        }

        #endregion

        #region 成员属性

        /// <summary>
        /// 隐藏默认值(没有显示赋值时的默认值)
        /// </summary>
        public object ImplicitDefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 显示默认值(显示赋值但没有给出值时的默认值)
        /// </summary>
        public object ExplicitDefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 预处理器
        /// </summary>
        public Type Processor
        {
            get;
            set;
        }

        #endregion
    }
}
