using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate;
using NHibernate.Criterion;

namespace PIC.Data
{
    public class SearchHelper
    {
        /// <summary>
        /// 创建时间可能的字段名
        /// </summary>
        public static readonly string[] CREATED_TIME_FIELD_NAMES = new string[] { "CreateDate", "CreateTime", "CreatedDate", "CreatedTime" };

        /// <summary>
        /// 创建人Id可能的字段名
        /// </summary>
        public static readonly string[] CREATER_ID_FIELD_NAMES = new string[] { "CreaterID", "CreateId" };

        /// <summary>
        /// 创建人名可能的字段名
        /// </summary>
        public static readonly string[] CREATER_NAME_FIELD_NAMES = new string[] { "CreaterName", "CreateName" };

        /// <summary>
        /// 从DataTable中获取特定类型的字段
        /// </summary>
        /// <returns></returns>
        public static string GetSpecialField(DataTable dt, string[] fnames)
        {
            if (dt.Columns.Count > 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    for (int i = 0; i < fnames.Length; i++)
                    {
                        if (String.Compare(col.ColumnName, fnames[i], true) == 0)
                        {
                            return col.ColumnName;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取数量语句
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="schcrit"></param>
        /// <returns></returns>
        public static string GetCountSQLString(string sqlString, SearchCriterion schcrit = null)
        {
            string wherestr = String.Empty; 

            if (schcrit != null)
            {
                wherestr = schcrit.GetWhereString();
            }

            if (!String.IsNullOrEmpty(wherestr))
            {
                sqlString = String.Format("SELECT * FROM ({0}) as __perq WHERE 1=1 AND ({1})", sqlString, wherestr);
            }

            string countsql = String.Format("SELECT COUNT(*) FROM ({0}) AS iq", sqlString);

            return countsql;
        }

        /// <summary>
        /// 获取分页的SQL语句(目前只支持SQLServer)
        /// </summary>
        /// <param name="sqlString">原来sql语句（必须是完整的，可执行的sql语句）</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderstr">排序字符串(必须单独提供)</param>
        /// <returns></returns>
        public static string GetPagingSQLString(string sqlString, int pageIndex, int pageSize, string orderstr)
        {
            string pgsql = String.Format("SELECT TOP {0} * FROM  ( SELECT *, ROW_NUMBER() OVER ({1}) AS sort_row_number FROM ({2}) AS iq ) as q WHERE q.sort_row_number > {3} ORDER BY q.sort_row_number ",
                pageSize, orderstr, sqlString, (pageIndex - 1) * pageSize);

            return pgsql;
        }

        /// <summary>
        /// 获取分页的SQL语句(目前只支持SQLServer)
        /// </summary>
        /// <param name="sqlString">原来sql语句（必须是完整的，可执行的sql语句）</param>
        /// <param name="schcrit"></param>
        /// <param name="defOrderStr">默认排序字段（当无排序时，默认按此排序）</param>
        /// <returns></returns>
        public static string GetPagingSQLString(string sqlString, SearchCriterion schcrit, string defOrderStr)
        {
            string pgsql = String.Empty;

            string ordstr = schcrit.GetOrderString();

            if (String.IsNullOrEmpty(ordstr))
            {
                ordstr = defOrderStr;
            }
            else
            {
                ordstr = " ORDER BY " + ordstr;
            }

            string wherestr = schcrit.GetWhereString();

            if (!String.IsNullOrEmpty(wherestr))
            {
                sqlString = String.Format("SELECT * FROM ({0}) as __perq WHERE 1=1 AND ({1})", sqlString, wherestr);
            }

            pgsql = GetPagingSQLString(sqlString, schcrit.CurrentPageIndex, schcrit.PageSize, ordstr);

            return pgsql;
        }

        /// <summary>
        ///将值转换为符合数据库标准的值（如Int64转换为Int32）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertToSearchValue(object value, TypeCode type)
        {
            if (value == null || (value is string && value.ToString() == ""))
            {
                return value;
            }

            Type srcType = value.GetType();
            object rtnValue = value;

            if (type == TypeCode.Object || type == TypeCode.Empty)
            {
                if (srcType == typeof(Int64))
                {
                    type = TypeCode.Int32;
                }
            }

            if (type != TypeCode.Object && type != TypeCode.Empty)
            {
                rtnValue = Convert.ChangeType(value, type);
            }

            return rtnValue;
        }

        /// <summary>
        /// 构建限制条件的并集
        /// </summary>
        /// <returns></returns>
        public static ICriterion UnionCriterions(params ICriterion[] crits)
        {
            IList<ICriterion> tcrits = crits.ToList();

            Disjunction ucrit = new Disjunction();

            foreach (ICriterion tc in tcrits)
            {
                if (tc == null)
                {
                    continue;
                }

                ucrit.Add(tc);
            }

            return ucrit;
        }

        /// <summary>
        /// 构建限制条件的交集
        /// </summary>
        /// <returns></returns>
        public static ICriterion IntersectCriterions(params ICriterion[] crits)
        {
            IList<ICriterion> tcrits = crits.ToList();

            Conjunction ucrit = new Conjunction();

            foreach (ICriterion tc in tcrits)
            {
                if (tc == null)
                {
                    continue;
                }

                ucrit.Add(tc);
            }

            return ucrit;
        }
    }
}
