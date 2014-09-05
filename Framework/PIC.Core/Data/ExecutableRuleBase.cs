using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;

namespace PIC.Data
{
    public class ExecutableRuleBase<T> : RuleBase<T> where T : ExecutableEntityBase<T>
    {
        #region 静态成员

        /// <summary>
        /// 根据查询条件查询所有未启动的对象
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAllUnstarted()
        {
            return ExecutableEntityBase<T>.FindAll(SearchHelper.UnionCriterions(Expression.Eq("ExecuteState", ExecuteStateEnum.Unstarted.ToString()), Expression.Eq("ExecuteState", String.Empty), Expression.IsNull("ExecuteState")));
        }

        /// <summary>
        /// 根据查询条件查询查询所有未启动的对象
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAllUnstarted(SearchCriterion criterion)
        {
            return RuleBase<T>.FindAll(criterion, SearchHelper.UnionCriterions(Expression.Eq("ExecuteState", ExecuteStateEnum.Unstarted.ToString()), Expression.Eq("ExecuteState", String.Empty), Expression.IsNull("ExecuteState")));
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, ExecuteStateEnum execState)
        {
            return RuleBase<T>.FindAll(criterion, Expression.Eq("ExecuteState", execState.ToString()));
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, string execState)
        {
            return RuleBase<T>.FindAll(criterion, Expression.Eq("ExecuteState", execState));
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, ExecuteStateEnum execState, params ICriterion[] crits)
        {
            IList<ICriterion> critList = crits.ToList();
            critList.Add(Expression.Eq("ExecuteState", execState.ToString()));

            return RuleBase<T>.FindAll(criterion, critList.ToArray());
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, string execState, params ICriterion[] crits)
        {
            IList<ICriterion> critList = crits.ToList();
            critList.Add(Expression.Eq("ExecuteState", execState));

            return RuleBase<T>.FindAll(criterion, critList.ToArray());
        }

        #endregion
    }
}
