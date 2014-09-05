using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    /// <summary>
    /// 查询路径(以.分割，第一段为数据类型，第二段对数据模块, 第三段为子模块)
    /// 如：data.portlet.publicinfo(表示,数据类型为data,模块为portlet下的子模块publicinfo数据)
    /// 如：list.portlet.publicinfo(表示,数据类型为data,模块为portlet下的子模块publicinfo数据)
    /// </summary>
    public class QueryPath
    {
        public const char SPLIT_CHAR = '.';
        public const string DEFAULT_DATATYPE = "list";

        string pathstr = String.Empty;
        string[] pathArr;

        public QueryPath(string pathstr)
        {
            this.pathstr = pathstr;
            this.pathArr = pathstr.Split(SPLIT_CHAR);
        }

        /// <summary>
        /// 查询数据类型
        /// </summary>
        public string DataType
        {
            get
            {
                if (pathArr.Length > 0)
                {
                    return pathArr[0];
                }

                return DEFAULT_DATATYPE;
            }
        }
    }
}
