using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;

namespace PIC.Data
{
    public class RuleBase<T> where T : EntityBase<T>
    {
        #region 静态成员

        /// <summary>
        /// 批量删除
        /// </summary>
        public static void BatchRemove(IList<EntityBase<T>> ents)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < ents.Count; i++)
                    {
                        ents[i].Delete();
                    }

                    trans.VoteCommit();
                }
                catch (Exception ex)
                {
                    trans.VoteRollBack();

                    throw ex;
                }
            }
        }

        /// <summary>
        /// 由主键列表批量删除
        /// </summary>
        /// <param name="ids">主键列表</param>
        public static void BatchRemoveByPrimaryKeys(IEnumerable<object> ids)
        {
            string pkName = EntityBase<T>.PrimaryKeyName;

            if (!String.IsNullOrEmpty(pkName))
            {
                object[] objIdsToDel = ids.Distinct().ToArray();

                EntityBase<T>[] ents = EntityBase<T>.FindAll(Expression.In(EntityBase<T>.PrimaryKeyName, objIdsToDel));
                
                BatchRemove(ents);
            }
            else
            {
                throw new Exception("没有发现实体主键，操作无法继续！");
            }
        }

        public static T[] FindAll(SearchCriterion criterion)
        {
            if (criterion is HqlSearchCriterion)
            {
                return EntityBase<T>.FindAllBySearchCriterion(criterion as HqlSearchCriterion);
            }
            else
            {
                throw new NotSupportedException("不支持除HqlSearchCriterion的查询。");
            }
        }

        public static T[] FindAll(SearchCriterion criterion, params ICriterion[] crits)
        {
            if (criterion is HqlSearchCriterion)
            {
                return EntityBase<T>.FindAllByCriterion(criterion as HqlSearchCriterion, crits);
            }
            else
            {
                throw new NotSupportedException("不支持除HqlSearchCriterion的查询。");
            }
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public static T[] FindAll()
        {
            return EntityBase<T>.FindAll();
        }

        #endregion
    }
}
