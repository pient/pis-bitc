using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Web
{
    /// <summary>
    /// 查询路径
    /// </summary>
    public class QueryPath
    {
        public const char SPLIT_CHAR = '.';

        protected string pathstr = String.Empty;
        protected char splitchar = SPLIT_CHAR;
        protected string[] pathArr;

        #region Constructor Method

        public QueryPath(string pathstr)
            : this(pathstr, SPLIT_CHAR)
        {
        }

        public QueryPath(string pathstr, char splitchar)
        {
            this.pathstr = pathstr;
            this.pathArr = pathstr.Split(splitchar);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取指定位置值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string Get(int index)
        {
            if (pathArr.Length > index )
            {
                return pathArr[index];
            }

            return null;
        }

        /// <summary>
        /// 获取指定位置值，如果不存在，则返回默认值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defstr"></param>
        /// <returns></returns>
        public string Get(int index, string defstr)
        {
            if (pathArr.Length > index)
            {
                return pathArr[index];
            }

            return defstr;
        }

        #endregion

        #region Static Method

        public static string Get(string str, int index, char splitchar)
        {
            QueryPath qp = new QueryPath(str, splitchar);

            return qp.Get(index);
        }

        public static string Get(string str, int index)
        {
            QueryPath qp = new QueryPath(str);

            return qp.Get(index);
        }

        #endregion
    }
}
