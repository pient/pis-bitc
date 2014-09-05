using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace PIC.Data
{
    /// <summary>
    /// Hql全文检索表达式
    /// </summary>
    public abstract class FTSearchCriterionBuilderForHql
    {
        public abstract ICriterion BuildCriterion(FTSearchCriterionItem criterionItem);
    }

    /// <summary>
    /// SQL Server的Hql全文检索表达式
    /// </summary>
    public class SQLFTSearchCriterionBuilderForHql : FTSearchCriterionBuilderForHql
    {
        public override ICriterion BuildCriterion(FTSearchCriterionItem criterionItem)
        {
            ICriterion hqlCriterion = null;

            if (!String.IsNullOrEmpty(criterionItem.Value))
            {
                string qryval = SQLFTQueryBuilder.ProcessQueryString(criterionItem.Value);

                hqlCriterion = Expression.Sql(String.Format("contains({0},'{1}')", getColumnListString(criterionItem), qryval));
            }

            return hqlCriterion;
        }

        /// <summary>
        /// 获取全文检索列字段(需要对字段进行特殊字符处理)
        /// </summary>
        /// <param name="criterionItem"></param>
        /// <returns></returns>
        private string getColumnListString(FTSearchCriterionItem criterionItem)
        {
            string rtncls = "*";

            if (criterionItem.ColumnList.Count > 0)
            {
                StringBuilder cls = new StringBuilder();

                foreach (string col in criterionItem.ColumnList)
                {
                    cls.AppendFormat("{0},", col);
                }

                rtncls = cls.ToString().TrimEnd(',');
            }

            return String.Format("({0})", rtncls);
        }
    }
}
