using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    /// <summary>
    /// 模版节点
    /// </summary>
    public class TemplateNode
    {
        #region 成员属性

        public virtual string Name
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public TemplateNode()
        {
            Name = String.Empty;
        }

        #endregion
    }

    /// <summary>
    /// 导入模版数据列节点
    /// </summary>
    public class TemplateColumnNode : TemplateNode
    {
        #region 成员属性

        /// <summary>
        /// 数据库列名
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// 行值位置
        /// </summary>
        public int? ValueRowIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 列值位置
        /// </summary>
        public int? ValueColumnIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 单元格默认值
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public TemplateColumnNode()
        {
        }

        #endregion
    }
}
