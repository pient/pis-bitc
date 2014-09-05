using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    /// <summary>
    /// 查询构建类
    /// </summary>
    public class QueryBuilder
    {
        /// <summary>
        /// 获取查询语句的列名数组
        /// </summary>
        /// <param name="qryString"></param>
        /// <returns></returns>
        public static string[] GetColumnNames(string qryString)
        {
            string upperQryString = qryString.ToUpper();
            int startLocation = 0;

            if (!upperQryString.Contains("SELECT"))
            {
                return null;
            }
            else
            {
                startLocation = upperQryString.IndexOf("SELECT") + "SELECT".Length;
            }

            int endLocation = 0;

            if (!upperQryString.Contains("FROM"))
            {
                return null;
            }
            else
            {
                endLocation = upperQryString.IndexOf("FROM");
            }

            if (startLocation >= endLocation)
            {
                return null;
            }

            string columnString = qryString.Substring(startLocation, endLocation - startLocation);

            string[] columnNames = columnString.Split(',');
            string[] rtnColumnNames = new string[columnNames.Length];

            for (int i = 0; i < rtnColumnNames.Length; i++)
            {
                string tmpname = columnNames[i].Trim();
                int asPosition = tmpname.ToUpper().IndexOf(" AS ");

                if (asPosition > 0)
                {
                    tmpname = tmpname.Substring(asPosition + 3).Trim();
                }

                int pointPosition = tmpname.ToUpper().IndexOf(".");

                if (pointPosition > 0)
                {
                    tmpname = tmpname.Substring(pointPosition + 1).Trim();
                }

                rtnColumnNames[i] = tmpname;
            }

            return rtnColumnNames;
        }

        /// <summary>
        /// 获取SQL排序字符串
        /// </summary>
        /// <param name="schCrit"></param>
        /// <returns></returns>
        public static string GetSQLOrderString(SearchCriterion schCrit)
        {
            string orderstr = String.Empty;

            if (schCrit.Orders != null && schCrit.Orders.Count > 0)
            {
                foreach (OrderCriterionItem item in schCrit.Orders)
                {
                    if (!String.IsNullOrEmpty(item.PropertyName))
                    {
                        orderstr += item.PropertyName;

                        if (item.Ascending)
                        {
                            orderstr += " ASC";
                        }
                        else
                        {
                            orderstr += " DESC";
                        }

                        orderstr += ",";
                    }
                }

                orderstr = orderstr.TrimEnd(',');
            }

            return orderstr;
        }
    }

    /// <summary>
    /// SQL Server全文检索查询构建器
    /// </summary>
    public class SQLFTQueryBuilder : QueryBuilder
    {
        #region 处理全文检索特殊字符 

        public static string ProcessQueryString(string qrystr)
        {
            // 空格换成%
            if (!String.IsNullOrEmpty(qrystr))
            {
                qrystr = "\"" + qrystr.Replace(" ", "\"\"").Replace("\'", "\'\'") + "\"";
            }

            return qrystr;
        }

        #endregion
    }
}
