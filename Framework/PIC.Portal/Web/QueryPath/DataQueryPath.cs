using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Web
{
    /// <summary>
    /// 数据查询路径(以.分割，第一段为数据类型，第二段对数据模块, 第三段为子模块)
    /// 如：data.portlet.publicinfo(表示,数据类型为data,模块为portlet下的子模块publicinfo数据)
    /// 如：list.portlet.message(表示,数据类型为list,模块为portlet下的子模块message数据)
    /// </summary>
    public class DataQueryPath : QueryPath
    {
        public const string DEFAULT_DATATYPE = "list";

        #region Constructor Method

        public DataQueryPath(string pathstr)
            : base(pathstr)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 查询数据类型
        /// </summary>
        public string DataType
        {
            get
            {
                return this.Get(0, DEFAULT_DATATYPE);
            }
        }

        /// <summary>
        /// 路径模块
        /// </summary>
        public string Module
        {
            get
            {
                return this.Get(1, String.Empty);
            }
        }

        /// <summary>
        /// 子路径模块
        /// </summary>
        public string SubModule
        {
            get
            {
                return this.Get(2, String.Empty);
            }
        }

        #endregion
    }
}
