using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;
using NHibernate.Criterion;
using NHibernate.Transform;
using Castle.ActiveRecord;
using Newtonsoft.Json;

namespace PIC.Data
{
    /// <summary>
    /// Hql的查询条件
    /// </summary>
    public class HqlSearchCriterion : SearchCriterion
    {
        #region 私有成员

        FTSearchCriterionBuilderForHql _ftCritBuilder = null;

        #endregion

        #region 构造函数

        public HqlSearchCriterion()
        {
            _ftCritBuilder = new SQLFTSearchCriterionBuilderForHql();
        }

        public HqlSearchCriterion(FTSearchCriterionBuilderForHql ftCritBuilder)
        {
            _ftCritBuilder = ftCritBuilder;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置全文检索构建器
        /// </summary>
        /// <param name="ftCritBuilder"></param>
        public void SetFullTextCriterionBuilder(FTSearchCriterionBuilderForHql ftCritBuilder)
        {
            this._ftCritBuilder = ftCritBuilder;
        }

        /// <summary>
        /// 转换为Detached查询规则
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DetachedCriteria GetDetachedCriteria<T>()
        {
            bool isAdded = false;
            DetachedCriteria criterias = this.GetDetachedCriteriaWithoutOrder<T>();

            criterias = AddOrderCriteria(criterias, ref isAdded);

            if (this.AutoOrder)
            {
                ReadOnlyCollection<PropertyInfo> allProperties = new ReadOnlyCollection<PropertyInfo>(typeof(T).GetProperties());

                foreach (string tfield in AUTO_ORDER_FIELDS)
                {
                    PropertyInfo prop = allProperties.FirstOrDefault(v => String.Compare(v.Name, tfield) == 0);

                    if (prop != null)
                    {
                        object[] propattr = prop.GetCustomAttributes(typeof(Castle.ActiveRecord.PropertyAttribute), true);
                        if (propattr != null && propattr.Length > 0)
                        {
                            if (!this.HasOrdered(prop.Name))
                            {
                                criterias.AddOrder(new Order(prop.Name, false));
                            }
                        }
                        break;
                    }
                }

            }

            return criterias;
    }

        /// <summary>
        /// 获取没有排序的规则（一般用来获取记录数）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DetachedCriteria GetDetachedCriteriaWithoutOrder<T>()
        {
            DetachedCriteria criterias = DetachedCriteria.For(typeof(T));

            HqlJunctionSearchCriterionItem search = new HqlJunctionSearchCriterionItem(Searches) { FtCritBuilder = _ftCritBuilder };

            ICriterion crit = search.GetCriterion();

            if (crit != null)
            {
                criterias.Add(crit);
            }

            if (this.IsDistinct)
            {
                criterias.SetResultTransformer(new DistinctRootEntityResultTransformer());
                
            }

            return criterias;
        }

        /// <summary>
        /// 查询指定类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T[] FindAll<T>()
        {
            DetachedCriteria criterias = null;

            Array arr = null;

            if (this.AllowPaging)
            {
                criterias = GetDetachedCriteriaWithoutOrder<T>();

                if (this.GetRecordCount)
                {
                    this.RecordCount = ActiveRecordMediator.Count(typeof(T), criterias);
                }

                criterias = GetDetachedCriteria<T>();

                arr = ActiveRecordMediator.SlicedFindAll(typeof(T), (this.CurrentPageIndex - 1) * this.PageSize, this.PageSize, criterias);
            }
            else
            {
                criterias = this.GetDetachedCriteria<T>();

                arr = ActiveRecordMediator.FindAll(typeof(T), criterias);

                if (this.GetRecordCount)
                {
                    this.RecordCount = arr.Length;
                }
            }

            return (T[])arr;
        }

        /// <summary>
        /// 查询指定类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] FindAll<T>(params ICriterion[] crits)
        {
            DetachedCriteria criterias = null;
            
            Array arr = null;

            if (this.AllowPaging)
            {
                // SQL查询中，使用Count函数时不能有Order信息
                criterias = GetDetachedCriteriaWithoutOrder<T>();

                foreach (ICriterion crit in crits)
                {
                    if (crit != null)
                    {
                        criterias.Add(crit);
                    }
                }

                if (this.GetRecordCount)
                {
                    this.RecordCount = ActiveRecordMediator.Count(typeof(T), criterias);

                    if (this.PageCount < this.CurrentPageIndex)
                    {
                        this.CurrentPageIndex = this.PageCount;
                    }

                    if (this.CurrentPageIndex < 1)
                    {
                        this.CurrentPageIndex = 1;
                    }
                }

                criterias = GetDetachedCriteria<T>();

                foreach (ICriterion crit in crits)
                {
                    if (crit != null)
                    {
                        criterias.Add(crit);
                    }
                }

                arr = ActiveRecordMediator.SlicedFindAll(typeof(T), (this.CurrentPageIndex - 1) * this.PageSize, this.PageSize, criterias);
            }
            else
            {
                criterias = this.GetDetachedCriteria<T>();

                foreach (ICriterion crit in crits)
                {
                    if (crit != null)
                    {
                        criterias.Add(crit);
                    }
                }

                arr = ActiveRecordMediator.FindAll(typeof(T), criterias);

                if (this.GetRecordCount)
                {
                    this.RecordCount = arr.Length;
                }
            }

            return (T[])arr;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 添加排序规则(只限内部使用，且不能连续执行)
        /// </summary>
        private DetachedCriteria AddOrderCriteria(DetachedCriteria criterias, ref bool isAdded)
        {
            if (!isAdded)
            {
                foreach (OrderCriterionItem item in this.Orders)
                {
                    Order order = new HqlOrderCriterionItem(item).GetOrder();

                    if (order != null)
                    {
                        criterias.AddOrder(order);
                    }
                }
            }

            return criterias;
        }

        #endregion
    }

    /// <summary>
    /// Hql查询限制项
    /// </summary>
    public interface IHqlSearchCriterionItem
    {
        /// <summary>
        /// 获取查询限制项
        /// </summary>
        /// <returns></returns>
        ICriterion GetCriterion();
    }

    /// <summary>
    /// Hql排序项
    /// </summary>
    public interface IHqlOrderCriterionItem
    {
        /// <summary>
        /// 获取排序项
        /// </summary>
        /// <returns></returns>
        Order GetOrder();
    }

    /// <summary>
    /// 全文检索规则项
    /// </summary>
    public class HqlFTSearchCriterionItem : FTSearchCriterionItem, IHqlSearchCriterionItem
    {
        #region 属性

        [JsonIgnore]
        public FTSearchCriterionBuilderForHql FtCritBuilder
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public HqlFTSearchCriterionItem()
            :base()
        {
        }

        public HqlFTSearchCriterionItem(string val)
            : base(val)
        {
        }

        public HqlFTSearchCriterionItem(IList<string> _columnList, string val)
            : base(val)
        {
        }

        public HqlFTSearchCriterionItem(FTSearchCriterionItem item)
            : base(item)
        {
        }

        public HqlFTSearchCriterionItem(FTSearchCriterionBuilderForHql ftCritBuilder)
            : base()
        {
            this.FtCritBuilder = ftCritBuilder;
        }

        public HqlFTSearchCriterionItem(FTSearchCriterionBuilderForHql ftCritBuilder, string val)
            : base(val)
        {
            this.FtCritBuilder = ftCritBuilder;
        }

        public HqlFTSearchCriterionItem(FTSearchCriterionBuilderForHql ftCritBuilder, IList<string> _columnList, string val)
            : base(val)
        {
            this.FtCritBuilder = ftCritBuilder;
        }

        public HqlFTSearchCriterionItem(FTSearchCriterionBuilderForHql ftCritBuilder, FTSearchCriterionItem item)
            : base(item)
        {
            this.FtCritBuilder = ftCritBuilder;
        }

        #endregion

        #region IHqlSearchCriterionItem Members

        public ICriterion GetCriterion()
        {
            if (FtCritBuilder != null && !String.IsNullOrEmpty(this.Value))
            {
                return FtCritBuilder.BuildCriterion(this);
            }

            return null;
        }

        #endregion
    }

    /// <summary>
    /// 普通查询规则项
    /// </summary>
    public class HqlCommonSearchCriterionItem : CommonSearchCriterionItem, IHqlSearchCriterionItem
    {
        #region 构造函数

        public HqlCommonSearchCriterionItem()
            : base()
        {
        }

        public HqlCommonSearchCriterionItem(string propertyName, SingleSearchModeEnum searchMode)
            : base(propertyName, searchMode)
        {
        }

        public HqlCommonSearchCriterionItem(string propertyName, object value)
            : base(propertyName, value)
        {
        }

        public HqlCommonSearchCriterionItem(string propertyName, object value, TypeCode type)
            : base(propertyName, value)
        {
        }

        public HqlCommonSearchCriterionItem(string propertyName, object value, SearchModeEnum searchMode)
            : base(propertyName, value, searchMode)
        {
        }

        public HqlCommonSearchCriterionItem(string propertyName, object value, SearchModeEnum searchMode, TypeCode type)
            : base(propertyName, value, searchMode)
        {
        }

        public HqlCommonSearchCriterionItem(CommonSearchCriterionItem item)
            : base(item)
        {
        }

        #endregion

        #region IHqlSearchCriterionItem Members

        public ICriterion GetCriterion()
        {
            ICriterion hqlCriterion = null;

            string propertyName = this.PropertyName;

            object value = SearchHelper.ConvertToSearchValue(this.Value, this.Type);

            if (String.IsNullOrEmpty(propertyName) || value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }

            if (this.IsSingleSearch)
            {
                switch (this.SingleSearchMode)
                {
                    case SingleSearchModeEnum.IsEmpty:
                        hqlCriterion = Expression.IsEmpty(propertyName);
                        break;
                    case SingleSearchModeEnum.IsNotEmpty:
                        hqlCriterion = Expression.IsNotEmpty(propertyName);
                        break;
                    case SingleSearchModeEnum.IsNotNull:
                        hqlCriterion = Expression.IsNotNull(propertyName);
                        break;
                    case SingleSearchModeEnum.IsNull:
                        hqlCriterion = Expression.IsNull(propertyName);
                        break;
                }
            }
            else
            {
                switch (this.SearchMode)
                {
                    case SearchModeEnum.Equal:
                        hqlCriterion = Expression.Eq(propertyName, value);
                        break;
                    case SearchModeEnum.NotEqual:
                        hqlCriterion = Expression.Not(Expression.Eq(propertyName, value));
                        break;
                    case SearchModeEnum.LessThan:
                        hqlCriterion = Expression.Lt(propertyName, value);
                        break;
                    case SearchModeEnum.LessThanEqual:
                        hqlCriterion = Expression.Le(propertyName, value);
                        break;
                    case SearchModeEnum.Like:
                        hqlCriterion = Expression.Like(propertyName, "%" + value + "%");    // Like默认前后加%
                        break;
                    case SearchModeEnum.NotLike:
                        hqlCriterion = Expression.Not(Expression.Like(propertyName, "%" + value + "%"));
                        break;
                    case SearchModeEnum.GreaterThan:
                        hqlCriterion = Expression.Gt(propertyName, value);
                        break;
                    case SearchModeEnum.GreaterThanEqual:
                        hqlCriterion = Expression.Ge(propertyName, value);
                        break;
                    case SearchModeEnum.In:
                        if (value is ICollection)
                        {
                            hqlCriterion = Expression.In(propertyName, value as ICollection);
                        }
                        break;
                    case SearchModeEnum.NotIn:
                        if (value is ICollection)
                        {
                            hqlCriterion = Expression.Not(Expression.In(propertyName, value as ICollection));
                        }
                        break;
                    case SearchModeEnum.StartWith:
                        hqlCriterion = Expression.Like(propertyName, value.ToString(), MatchMode.Start);
                        break;
                    case SearchModeEnum.EndWith:
                        hqlCriterion = Expression.Like(propertyName, value.ToString(), MatchMode.End);
                        break;
                    case SearchModeEnum.NotEndWith:
                        hqlCriterion = Expression.Not(Expression.Like(propertyName, value.ToString(), MatchMode.Start));
                        break;
                    case SearchModeEnum.NotStartWith:
                        hqlCriterion = Expression.Not(Expression.Like(propertyName, value.ToString(), MatchMode.End));
                        break;
                }
            }

            return hqlCriterion;
        }

        #endregion
    }

    /// <summary>
    /// Hql组合查询项
    /// </summary>
    public class HqlJunctionSearchCriterionItem : JunctionSearchCriterionItem, IHqlSearchCriterionItem
    {
        #region 属性

        [JsonIgnore]
        public FTSearchCriterionBuilderForHql FtCritBuilder
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public HqlJunctionSearchCriterionItem(JunctionMode junctionMode)
            : base(junctionMode)
        {
        }

        public HqlJunctionSearchCriterionItem()
            : base()
        {
        }

        public HqlJunctionSearchCriterionItem(JunctionSearchCriterionItem item)
            : base(item)
        {
        }

        #endregion

        #region IHqlSearchCriterionItem Members

        public ICriterion GetCriterion()
        {
            ICriterion rtnCrit = null;

            IList<ICriterion> critList = new List<ICriterion>();

            foreach (CommonSearchCriterionItem item in this.Searches)
            {
                IHqlSearchCriterionItem titem = new HqlCommonSearchCriterionItem(item);
                ICriterion tcrit = titem.GetCriterion();

                if (tcrit != null)
                {
                    critList.Add(tcrit);
                }
            }

            foreach (FTSearchCriterionItem item in this.FTSearches)
            {
                IHqlSearchCriterionItem titem = new HqlFTSearchCriterionItem(FtCritBuilder, item);
                ICriterion tcrit = titem.GetCriterion();

                if (tcrit != null)
                {
                    critList.Add(tcrit);
                }
            }

            foreach (JunctionSearchCriterionItem item in this.JuncSearches)
            {
                IHqlSearchCriterionItem titem = new HqlJunctionSearchCriterionItem(item);
                ICriterion tcrit = titem.GetCriterion();

                if (tcrit != null)
                {
                    critList.Add(tcrit);
                }
            }

            if (critList.Count > 0)
            {
                switch (this.JunctionMode)
                {
                    case JunctionMode.And:
                        rtnCrit = SearchHelper.IntersectCriterions(critList.ToArray());
                        break;
                    default:
                        rtnCrit = SearchHelper.UnionCriterions(critList.ToArray());
                        break;
                }
            }

            return rtnCrit;
        }

        #endregion
    }

    /// <summary>
    /// Hql排序项
    /// </summary>
    public class HqlOrderCriterionItem : OrderCriterionItem, IHqlOrderCriterionItem
    {
        #region 构造函数

        public HqlOrderCriterionItem(string propertyName, bool ascending)
            : base(propertyName, ascending)
        {
        }

        public HqlOrderCriterionItem(OrderCriterionItem item)
            : base(item)
        {
        }

        #endregion

        #region IHqlOrderCriterionItem Members

        public Order GetOrder()
        {
            if (String.IsNullOrEmpty(this.PropertyName))
            {
                return null;
            }
            else
            {
                return new Order(this.PropertyName, this.Ascending);
            }
        }

        #endregion
    }
}
