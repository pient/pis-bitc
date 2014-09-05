using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Criterion;

namespace PIC.Data
{
    /// <summary>
    /// 可执行实体
    /// </summary>
    public class EditSensitiveRuleBase<T> : RuleBase<T> where T : EditSensitiveEntityBase<T>
    {
        #region 方法

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, EditStatusEnum editStatus)
        {
            return RuleBase<T>.FindAll(criterion, Expression.Eq("EditStatus", editStatus.ToString()));
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, string editStatus)
        {
            return RuleBase<T>.FindAll(criterion, Expression.Eq("EditStatus", editStatus));
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, EditStatusEnum editStatus, params ICriterion[] crits)
        {
            IList<ICriterion> critList = crits.ToList();
            critList.Add(Expression.Eq("EditStatus", editStatus.ToString()));

            return RuleBase<T>.FindAll(criterion, critList.ToArray());
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, string editStatus, params ICriterion[] crits)
        {
            IList<ICriterion> critList = crits.ToList();
            critList.Add(Expression.Eq("EditStatus", editStatus));

            return RuleBase<T>.FindAll(criterion, critList.ToArray());
        }

        /// <summary>
        /// 通过编辑状态找数据
        /// </summary>
        /// <param name="editStatus"></param>
        public T[] FindAll(EditStatusEnum editStatus)
        {
            return FindAll(editStatus.ToString());
        }

        /// <summary>
        /// 通过编辑状态找数据
        /// </summary>
        /// <param name="editStatus"></param>
        public T[] FindAll(string editStatus)
        {
            return EntityBase<T>.FindAllByProperty("EditStatus", editStatus);
        }

        #endregion
    }
}
