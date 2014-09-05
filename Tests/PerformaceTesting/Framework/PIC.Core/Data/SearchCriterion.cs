using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    /// <summary>
    /// 查询条件组合模式
    /// </summary>
    public enum JunctionMode
    {
        Or,
        And
    }

    /// <summary>
    /// 普通查询方式(由字段和查询运算符以及条件值构成的查询运算)
    /// </summary>
    public enum SearchModeEnum
    {
        Equal = 0,
        NotEqual = 1,
        In = 2,
        NotIn = 3,
        Like = 4,
        NotLike = 5,
        GreaterThan = 6,
        GreaterThanEqual = 7,
        LessThan = 8,
        LessThanEqual = 9,
        StartWith = 10,
        EndWith = 11,
        NotStartWith = 12,
        NotEndWith = 13,
        UnSettled = 14  // 未设定
    }

    /// <summary>
    /// 单值查询枚举(只有字段和查询运算符构成的查询条件)
    /// </summary>
    public enum SingleSearchModeEnum
    {
        IsEmpty = 0,    // 查询集合时使用
        IsNotEmpty = 1,    // 查询集合时使用
        IsNull = 2,
        IsNotNull = 3,
        UnSettled = 4   // 未设定
    }

    /// <summary>
    /// 查询规则类
    /// </summary>
    public class SearchCriterion
    {
        /// <summary>
        /// 自动排序字段一般为创建时间字段
        /// </summary>
        internal readonly ReadOnlyCollection<string> AUTO_ORDER_FIELDS = new ReadOnlyCollection<string>(
            new List<string>(SearchHelper.CREATED_TIME_FIELD_NAMES));

        #region 私有成员

        private static int _defaultPageSize = 20;

        // 自动添加自动排序字段
        protected bool _autoOrder = true;

        //是否消除重复行
        protected bool _distinct = false;

        //是否允许分页，默认true
        protected bool _allowPaging = false;

        //当前页
        protected int _currentPage = 1;

        //每页记录条数
        protected int _pageSize = _defaultPageSize;

        //总记录数量
        protected int _recordCount = -1;

        //总页数
        protected int _pageCount = -1;

        //是否获取总记录数量
        protected bool _getRecordCount = false;

        private List<string> queryFields = new List<string>();

        #endregion

        #region 属性成员

        /// <summary>
        /// 设置每页默认的记录数，默认值为20条,
        /// </summary>
        /// <param name="pageSize"></param>
        public virtual int DefaultPageSize
        {
            get { return _defaultPageSize; }
            set { _defaultPageSize = value; }
        }

        /// <summary>
        /// 自动添加自动排序字段,默认true
        /// </summary>
        public virtual bool AutoOrder
        {
            get { return _autoOrder; }
            set { _autoOrder = value; }
        }

        /// <summary>
        /// 是否允许分页,默认false
        /// </summary>
        public virtual bool AllowPaging
        {
            get { return _allowPaging; }
            set { _allowPaging = value; }
        }

        /// <summary>
        /// 当前第几页，默认为1
        /// </summary>
        public virtual int CurrentPageIndex
        {
            get { return this._currentPage; }
            set { this._currentPage = value; }
        }

        /// <summary>
        /// 是否获取总记录数，默认false
        /// </summary>
        public virtual bool GetRecordCount
        {
            get { return this._getRecordCount; }
            set { this._getRecordCount = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public virtual int PageCount
        {
            get
            {
                if (_pageCount != -1)
                {
                    return _pageCount;
                }

                if (this.PageSize >= 0)
                {
                    _pageCount = (((this.RecordCount - 1) / this.PageSize) + 1);
                    return _pageCount;
                }
                return 1;
            }
        }

        /// <summary>
        /// 每页记录条数，默认为10
        /// </summary>
        public virtual int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                this._pageSize = value;
                this._pageCount = -1;
            }
        }

        /// <summary>
        /// 记录数量
        /// </summary>
        public virtual int RecordCount
        {
            get
            {
                return this._recordCount;
            }
            set
            {
                this._recordCount = value;
                this._pageCount = -1;
            }
        }

        /// <summary>
        /// 是否Distinct查询
        /// </summary>
        public virtual bool IsDistinct
        {
            get;
            set;
        }

        /// <summary>
        /// JunctionSearch查询表达式
        /// </summary>
        public JunctionSearchCriterionItem Searches
        {
            get;
            set;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public List<OrderCriterionItem> Orders
        {
            get;
            set;
        }


        /// <summary>
        /// 查询字段列表
        /// </summary>
        public virtual List<string> QueryFields
        {
            get { return this.queryFields; }
        }

        #endregion

        #region 构造函数

        public SearchCriterion()
        {
            Orders = new List<OrderCriterionItem>();
            Searches = new JunctionSearchCriterionItem(JunctionMode.And);
        }

        #endregion

        #region 公共方法

        #region 添加全文检索

        public void AddFTSearch(string value)
        {
            Searches.AddFTSearch(value);
        }

        public void AddFTSearch(List<string> colList, string value)
        {
            Searches.AddFTSearch(colList, value);
        }

        #endregion

        #region 添加查询条件

        /// <summary>
        /// 添加多个查询条件（默认等于）
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void AddSearches(string[] propertyNames, SingleSearchModeEnum searchMode)
        {
            Searches.AddSearches(propertyNames, searchMode);
        }

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void AddSearch(string propertyName, SingleSearchModeEnum searchMode)
        {
            Searches.AddSearch(propertyName, searchMode);
        }

        /// <summary>
        /// 增加查询条件，查询方式默认为等于查询
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public virtual void AddSearch(string propertyName, object value)
        {
            Searches.AddSearch(propertyName, value, SearchModeEnum.Equal);
        }

        /// <summary>
        /// 增加查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="type">查询值类型</param>
        public virtual void AddSearch(string propertyName, object value, TypeCode type)
        {
            Searches.AddSearch(propertyName, value, SearchModeEnum.Equal, type);
        }

        /// <summary>
        /// 增加查询条件
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="searchMode">查询方式</param>
        /// <returns></returns>
        public virtual void AddSearch(string propertyName, object value, SearchModeEnum searchMode)
        {
            Searches.AddSearch(propertyName, value, searchMode);
        }

        /// <summary>
        /// 增加查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="searchMode">查询方法</param>
        /// <param name="type">查询值类型</param>
        public virtual void AddSearch(string propertyName, object value, SearchModeEnum searchMode, TypeCode type)
        {
            Searches.AddSearch(propertyName, value, searchMode, type);
        }

        /// <summary>
        /// 设置多个查询条件（默认等于）
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void SetSearches(string[] propertyNames, SingleSearchModeEnum searchMode)
        {
            Searches.SetSearches(propertyNames, searchMode);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void SetSearch(string propertyName, SingleSearchModeEnum searchMode)
        {
            Searches.SetSearch(propertyName, searchMode);
        }

        /// <summary>
        /// 设置查询条件，查询方式默认为等于查询
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public virtual void SetSearch(string propertyName, object value)
        {
            Searches.SetSearch(propertyName, value, SearchModeEnum.Equal);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="type">查询值类型</param>
        public virtual void SetSearch(string propertyName, object value, TypeCode type)
        {
            Searches.SetSearch(propertyName, value, SearchModeEnum.Equal, type);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="searchMode">查询方式</param>
        /// <returns></returns>
        public virtual void SetSearch(string propertyName, object value, SearchModeEnum searchMode)
        {
            Searches.SetSearch(propertyName, value, searchMode);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="searchMode">查询方法</param>
        /// <param name="type">查询值类型</param>
        public virtual void SetSearch(string propertyName, object value, SearchModeEnum searchMode, TypeCode type)
        {
            Searches.SetSearch(propertyName, value, searchMode, type);
        }

        /// <summary>
        /// 移除属性名为propertyName的查询
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void RemoveSearch(string propertyName)
        {
            Searches.RemoveSearch(propertyName);
        }

        /// <summary>
        /// 移除属性名为在propertyNames中的查询
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void RemoveSearch(string[] propertyNames)
        {
            Searches.RemoveSearch(propertyNames);
        }

        /// <summary>
        /// 获取查询
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual IList<CommonSearchCriterionItem> GetSearches(string propertyName)
        {
            return Searches.GetSearches(propertyName);
        }

        /// <summary>
        /// 获取查询
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual IList<CommonSearchCriterionItem> GetSearches(string propertyName, SearchModeEnum schMode)
        {
            return Searches.GetSearches(propertyName, schMode);
        }

        /// <summary>
        /// 获取第一个指定propertyName的查询
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual CommonSearchCriterionItem GetFirstSearch(string propertyName)
        {
            return Searches.GetFirstSearch(propertyName);
        }

        /// <summary>
        /// 获取第一个指定propertyName的查询
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual CommonSearchCriterionItem GetFirstSearch(string propertyName, SearchModeEnum schMode)
        {
            return Searches.GetFirstSearch(propertyName, schMode);
        }

        /// <summary>
        /// 获取第一个指定propertyName的查询
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual object GetSearchValue(string propertyName)
        {
            CommonSearchCriterionItem csci = Searches.GetFirstSearch(propertyName);

            if (csci == null)
            {
                return null;
            }
            else
            {
                return csci.Value;
            }
        }

        /// <summary>
        /// 获取第一个指定propertyName的查询
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="defVal">默认值</param>
        /// <returns></returns>
        public virtual object GetSearchValue(string propertyName, object defVal)
        {
            object val = GetSearchValue(propertyName);

            if (val == null)
            {
                return defVal;
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// 获取查询值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual T GetSearchValue<T>(string propertyName)
        {
            object val = GetSearchValue(propertyName);

            if (val == null)
            {
                return default(T);
            }
            else
            {
                return CLRHelper.ConvertValue<T>(val);
            }
        }

        /// <summary>
        /// 获取查询值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual T GetSearchValue<T>(string propertyName, T defVal)
        {
            object val = GetSearchValue(propertyName);

            if (val == null)
            {
                return defVal;
            }
            else
            {
                return CLRHelper.ConvertValue<T>(val);
            }
        }

        #endregion

        #region 排序字段

        /// <summary>
        /// 增加排序字段
        /// </summary>
        /// <param name="propertyName"></param>
        public void SetOrder(string propertyName)
        {
            this.SetOrder(propertyName, true);
        }

        /// <summary>
        /// 增加排序字段(默认升序)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="orderDir"></param>
        public void SetOrder(string propertyName, bool ascending)
        {
            if (Orders == null || Orders.Count <= 0 || !this.HasOrdered(propertyName))
            {
                Orders.Add(new OrderCriterionItem(propertyName, ascending));
            }
            else
            {
                OrderCriterionItem orderItem = Orders.First(ent => ent.PropertyName == propertyName);
                orderItem.Ascending = ascending;
            }
        }

        /// <summary>
        /// 是否已经针对某字段进行了排序
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool HasOrdered(string propertyName)
        {
            bool rtn = Orders.Exists(ent => ent.PropertyName == propertyName);
            return rtn;
        }

        /// <summary>
        /// 是否已进行排序
        /// </summary>
        /// <returns></returns>
        public bool HasOrdered()
        {
            if (Orders == null || Orders.Count <= 0)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Clear

        /// <summary>
        /// 清空查询条件和排序条件
        /// </summary>
        public virtual void Clear()
        {
            this.queryFields.Clear();

            this.Searches.Clear();
        }

        /// <summary>
        /// 标准化SearchCriterion，如将Int64类型的数据转换为Int32类型（一般数据库不存在Int64类型数据）
        /// </summary>
        public virtual void FormatSearch()
        {
            FormatSearch(Searches);
        }

        /// <summary>
        /// 标准化CommonSearchCriterion
        /// </summary>
        /// <param name="cschitem"></param>
        protected virtual void FormatSearch(CommonSearchCriterionItem cschitem)
        {
            cschitem.Value = SearchHelper.ConvertToSearchValue(cschitem.Value, cschitem.Type);
        }

        /// <summary>
        /// 标准化JunctionSearchCriterion
        /// </summary>
        /// <param name="jschitem"></param>
        protected virtual void FormatSearch(JunctionSearchCriterionItem jschitem)
        {
            foreach (SearchCriterionItem item in jschitem.Searches)
            {
                if (item is CommonSearchCriterionItem)
                {
                    FormatSearch(item as CommonSearchCriterionItem);
                }
                else if (item is JunctionSearchCriterionItem)
                {
                    FormatSearch(item as JunctionSearchCriterionItem);
                }
            }
        }

        #endregion      

        #region 查询指定类型数据

        /// <summary>
        /// 查询指定类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T[] FindAll<T>()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 导出SQL语句（WHERE后）
        /// </summary>
        /// <returns></returns>
        public string GetWhereString()
        {
            return this.Searches.ToSQLString();
        }

        /// <summary>
        /// 获取排序字段
        /// </summary>
        /// <returns></returns>
        public string GetOrderString()
        {
            StringBuilder ordsql = new StringBuilder();

            foreach (OrderCriterionItem torder in this.Orders)
            {
                ordsql.Append(torder.ToSQLString() + ",");
            }

            return ordsql.ToString().TrimEnd(',');
        }

        #endregion
    }

    /// <summary>
    /// 规则项
    /// </summary>
    public abstract class CriterionItem
    {
        public abstract string ToSQLString();
    }

    /// <summary>
    /// 查询规则项
    /// </summary>
    public abstract class SearchCriterionItem : CriterionItem
    {
    }

    /// <summary>
    /// 全文检索规则项
    /// </summary>
    public class FTSearchCriterionItem : SearchCriterionItem
    {
        #region 私有成员

        protected IList<string> columnList = new List<string>();

        #endregion

        #region 公共属性

        /// <summary>
        /// 全文检索列
        /// </summary>
        public IList<string> ColumnList
        {
            get { return columnList; }
            set { this.columnList = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }


        #endregion

        #region 构造函数

        public FTSearchCriterionItem()
        {
            Value = null;
        }

        public FTSearchCriterionItem(string val)
        {
            Value = val;
        }

        public FTSearchCriterionItem(IList<string> _columnList, string val)
            : this(val)
        {
            this.columnList = _columnList;
        }

        public FTSearchCriterionItem(FTSearchCriterionItem item)
        {
            this.columnList = item.columnList;
            this.Value = item.Value;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 导出SQL语句（WHERE后）
        /// </summary>
        /// <returns></returns>
        public override string ToSQLString()
        {
            string sql = String.Empty;

            if (!String.IsNullOrEmpty(this.Value))
            {
                string qryval = SQLFTQueryBuilder.ProcessQueryString(this.Value);

                sql = String.Format(" contains({0},'{1}') ", GetColumnListString(), qryval);
            }

            if (!String.IsNullOrEmpty(sql))
            {
                sql = " (" + sql + ") ";
            }

            return sql;
        }

        /// <summary>
        /// 获取全文检索列字段(需要对字段进行特殊字符处理)
        /// </summary>
        /// <param name="criterionItem"></param>
        /// <returns></returns>
        protected string GetColumnListString()
        {
            string rtncls = "*";

            if (this.ColumnList.Count > 0)
            {
                StringBuilder cls = new StringBuilder();

                foreach (string col in this.ColumnList)
                {
                    cls.AppendFormat("{0},", col);
                }

                rtncls = cls.ToString().TrimEnd(',');
            }

            return String.Format("({0})", rtncls);
        }

        #endregion
    }

    /// <summary>
    /// 普通查询规则项
    /// </summary>
    public class CommonSearchCriterionItem : SearchCriterionItem
    {
        #region 公共属性 

        /// <summary>
        /// 字段名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// 值数据类型
        /// </summary>
        public TypeCode Type { get; set; }

        /// <summary>
        /// 搜索模式
        /// </summary>
        public SearchModeEnum SearchMode { get; set; }

        /// <summary>
        /// 单值查询模式(优先)
        /// </summary>
        public SingleSearchModeEnum SingleSearchMode { get; set; }

        /// <summary>
        /// 是否单值查询
        /// </summary>
        public bool IsSingleSearch
        {
            get
            {
                return (SingleSearchMode != SingleSearchModeEnum.UnSettled);
            }
        }

        #endregion

        #region 构造函数

        public CommonSearchCriterionItem()
        {
            Init();
        }

        public CommonSearchCriterionItem(string propertyName, SingleSearchModeEnum searchMode)
        {
            Init();

            PropertyName = propertyName;
            SingleSearchMode = searchMode;
        }

        public CommonSearchCriterionItem(string propertyName, object value)
        {
            Init();

            PropertyName = propertyName;
            Value = value;
            SearchMode = SearchModeEnum.Equal;
        }

        public CommonSearchCriterionItem(string propertyName, object value, TypeCode type)
            : this(propertyName, value)
        {
            Type = type;
        }

        public CommonSearchCriterionItem(string propertyName, object value, SearchModeEnum searchMode)
            : this()
        {
            Init();

            PropertyName = propertyName;
            Value = value;
            SearchMode = searchMode;
        }

        public CommonSearchCriterionItem(string propertyName, object value, SearchModeEnum searchMode, TypeCode type)
            : this(propertyName, value, searchMode)
        {
            Type = type;
        }

        public CommonSearchCriterionItem(CommonSearchCriterionItem item)
            : this(item.PropertyName, item.Value, item.SearchMode)
        {
        }

        private void Init()
        {
            SearchMode = SearchModeEnum.Equal;
            SingleSearchMode = SingleSearchModeEnum.UnSettled;
        }

        #endregion

        #region 私有函数

        #endregion

        #region 公共方法

        /// <summary>
        /// 导出SQL语句（WHERE后）
        /// </summary>
        /// <returns></returns>
        public override string ToSQLString()
        {
            string sql = String.Empty;

            if (SingleSearchMode != SingleSearchModeEnum.UnSettled)
            {
                string exp = String.Empty;

                switch (SingleSearchMode)
                {
                    case SingleSearchModeEnum.IsNull:
                    case SingleSearchModeEnum.IsNotNull:
                        exp = this.SingleSearchMode.ToString();
                        break;
                    case SingleSearchModeEnum.IsEmpty:
                    case SingleSearchModeEnum.IsNotEmpty:
                        throw new NotImplementedException(" IsEmpty 和 IsNotEmpty方法未实现查询 ");
                }

                sql = String.Format(" {0} {1} ", this.PropertyName, exp);
            }
            else if(SearchMode != SearchModeEnum.UnSettled)
            {
                string exp = String.Empty;
                object val = null;
                TypeCode valtcode = CLRHelper.GetEnum<TypeCode>(this.Value.GetType().Name);

                switch (SearchMode)
                {
                    case SearchModeEnum.Equal:
                        exp = " = ";
                        break;
                    case SearchModeEnum.NotEqual:
                        exp = " <> ";
                        break;
                    case SearchModeEnum.GreaterThan:
                        exp = " > ";
                        break;
                    case SearchModeEnum.GreaterThanEqual:
                        exp = " >= ";
                        break;
                    case SearchModeEnum.LessThan:
                        exp = " < ";
                        break;
                    case SearchModeEnum.LessThanEqual:
                        exp = " <= ";
                        break;
                    case SearchModeEnum.In:
                        exp = " IN ";
                        break;
                    case SearchModeEnum.NotIn:
                        exp = " NOT IN ";
                        break;
                    case SearchModeEnum.StartWith:
                    case SearchModeEnum.EndWith:
                    case SearchModeEnum.Like:
                        exp = " LIKE ";
                        break;
                    case SearchModeEnum.NotStartWith:
                    case SearchModeEnum.NotEndWith:
                    case SearchModeEnum.NotLike:
                        exp = " NOT LIKE ";
                        val = String.Format("'%{0}%'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                }

                switch (SearchMode)
                {
                    case SearchModeEnum.Equal:
                    case SearchModeEnum.NotEqual:
                    case SearchModeEnum.GreaterThan:
                    case SearchModeEnum.GreaterThanEqual:
                    case SearchModeEnum.LessThan:
                    case SearchModeEnum.LessThanEqual:
                        switch (valtcode)
                        {
                            case TypeCode.Char:
                            case TypeCode.DateTime:
                            case TypeCode.String:
                            case TypeCode.Object:
                                val = String.Format("'{0}'", StringHelper.IsNullValue(this.Value, string.Empty));
                                break;
                        }
                        break;
                    case SearchModeEnum.StartWith:
                        val = String.Format("'%{0}'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                    case SearchModeEnum.EndWith:
                        val = String.Format("'{0}%'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                    case SearchModeEnum.Like:
                        val = String.Format("'%{0}%'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                    case SearchModeEnum.NotStartWith:
                        val = String.Format("'%{0}'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                    case SearchModeEnum.NotEndWith:
                        val = String.Format("'{0}%'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                    case SearchModeEnum.NotLike:
                        val = String.Format("'%{0}%'", StringHelper.IsNullValue(this.Value, string.Empty));
                        break;
                    case SearchModeEnum.In:
                    case SearchModeEnum.NotIn:
                        throw new NotImplementedException(" In 和 NotIn方法未实现查询 ");
                }

                sql = String.Format(" {0} {1} {2} ", this.PropertyName, exp, val.ToString());
            }

            if (!String.IsNullOrEmpty(sql))
            {
                sql = " (" + sql + ") ";
            }

            return sql;
        }

        #endregion
    }

    /// <summary>
    /// 组合查询规则项
    /// </summary>
    public class JunctionSearchCriterionItem : SearchCriterionItem
    {
        #region 公共属性

        /// <summary>
        /// NHibernate查询表达式
        /// </summary>
        public List<CommonSearchCriterionItem> Searches
        {
            get;
            set;
        }

        /// <summary>
        /// 全文检索查询表达式
        /// </summary>
        public List<FTSearchCriterionItem> FTSearches
        {
            get;
            set;
        }

        /// <summary>
        /// 组合查询
        /// </summary>
        public List<JunctionSearchCriterionItem> JuncSearches
        {
            get;
            set;
        }

        /// <summary>
        /// 查询条件组合模式
        /// </summary>
        public JunctionMode JunctionMode
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public JunctionSearchCriterionItem(JunctionMode junctionMode)
        {
            JunctionMode = junctionMode;

            Searches = new List<CommonSearchCriterionItem>();
            FTSearches = new List<FTSearchCriterionItem>();
            JuncSearches = new List<JunctionSearchCriterionItem>();
        }

        public JunctionSearchCriterionItem()
            : this(JunctionMode.Or)
        {
        }

        public JunctionSearchCriterionItem(JunctionSearchCriterionItem item)
            : this()
        {
            this.JunctionMode = item.JunctionMode;

            foreach (CommonSearchCriterionItem titem in item.Searches)
            {
                Searches.Add(titem);
            }

            foreach (FTSearchCriterionItem titem in item.FTSearches)
            {
                FTSearches.Add(titem);
            }

            foreach (JunctionSearchCriterionItem titem in item.JuncSearches)
            {
                JuncSearches.Add(titem);
            }
        }

        #endregion

        #region 公共方法

        #region 添加全文检索

        public void AddFTSearch(string value)
        {
            this.FTSearches.Add(new FTSearchCriterionItem(value));
        }

        public void AddFTSearch(List<string> colList, string value)
        {
            this.FTSearches.Add(new FTSearchCriterionItem(colList, value));
        }

        #endregion

        #region 添加查询条件

        /// <summary>
        /// 清空查询
        /// </summary>
        public virtual void Clear()
        {
            this.Searches.Clear();
            this.FTSearches.Clear();
            this.JuncSearches.Clear();
        }

        /// <summary>
        /// 添加多个查询条件（默认等于）
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void AddSearches(string[] propertyNames, SingleSearchModeEnum searchMode)
        {
            foreach (string propertyName in propertyNames)
            {
                this.AddSearch(propertyName, searchMode);
            }
        }

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void AddSearch(string propertyName, SingleSearchModeEnum searchMode)
        {
            this.Searches.Add(new CommonSearchCriterionItem(propertyName, searchMode));
        }

        /// <summary>
        /// 增加查询条件，查询方式默认为等于查询
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public virtual void AddSearch(string propertyName, object value)
        {
            this.AddSearch(propertyName, value, SearchModeEnum.Equal);
        }

        public virtual void AddSearch(string propertyName, object value, TypeCode type)
        {
            this.AddSearch(propertyName, value, SearchModeEnum.Equal, type);
        }

        /// <summary>
        /// 增加查询条件
        /// </summary>
        /// <param name="propertyName">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="searchMode">查询方式</param>
        /// <returns></returns>
        public virtual void AddSearch(string propertyName, object value, SearchModeEnum searchMode)
        {
            this.Searches.Add(new CommonSearchCriterionItem(propertyName, value, searchMode));
        }

        public virtual void AddSearch(string propertyName, object value, SearchModeEnum searchMode, TypeCode type)
        {
            this.Searches.Add(new CommonSearchCriterionItem(propertyName, SearchHelper.ConvertToSearchValue(value, type), searchMode, type));
        }

        /// <summary>
        /// 添加多个查询条件（默认等于）
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public virtual void SetSearches(string[] propertyNames, SingleSearchModeEnum searchMode)
        {
            foreach (string propertyName in propertyNames)
            {
                this.SetSearch(propertyName, searchMode);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void SetSearch(string propertyName, SingleSearchModeEnum searchMode)
        {
            CommonSearchCriterionItem csci = GetFirstSearch(propertyName);

            if (csci != null)
            {
                csci.SingleSearchMode = searchMode;
            }
            else
            {
                AddSearch(propertyName, searchMode);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void SetSearch(string propertyName, object value)
        {
            CommonSearchCriterionItem csci = GetFirstSearch(propertyName);

            if (csci != null)
            {
                csci.Value = value;
            }
            else
            {
                AddSearch(propertyName, value);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void SetSearch(string propertyName, object value, TypeCode type)
        {
            CommonSearchCriterionItem csci = GetFirstSearch(propertyName);

            if (csci != null)
            {
                csci.Value = SearchHelper.ConvertToSearchValue(value, type);
            }
            else
            {
                AddSearch(propertyName, value, type);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void SetSearch(string propertyName, object value, SearchModeEnum searchMode)
        {
            CommonSearchCriterionItem csci = GetFirstSearch(propertyName);

            if (csci != null)
            {
                csci.Value = value;
                csci.SearchMode = searchMode;
            }
            else
            {
                AddSearch(propertyName, value, searchMode);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void SetSearch(string propertyName, object value, SearchModeEnum searchMode, TypeCode type)
        {
            CommonSearchCriterionItem csci = GetFirstSearch(propertyName);

            if (csci != null)
            {
                csci.Value = SearchHelper.ConvertToSearchValue(value, type);
                csci.SearchMode = searchMode;
            }
            else
            {
                AddSearch(propertyName, value, searchMode, type);
            }
        }

        /// <summary>
        /// 删除查询限制
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void RemoveSearch(string propertyName)
        {
            if (this.Searches != null && this.Searches.Count > 0)
            {
                Searches.RemoveAll(tent => tent.PropertyName == propertyName);
            }
        }

        /// <summary>
        /// 删除查询限制
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void RemoveSearch(string[] propertyNames)
        {
            if (this.Searches != null && this.Searches.Count > 0 && propertyNames != null && propertyNames.Length > 0)
            {
                Searches.RemoveAll(tent => propertyNames.Contains(tent.PropertyName));
            }
        }

        /// <summary>
        /// 获取指定属性的CommonSearch
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IList<CommonSearchCriterionItem> GetSearches(string propertyName)
        {
            if (this.Searches != null && Searches.Count(tent => tent.PropertyName == propertyName) > 0)
            {
                IList<CommonSearchCriterionItem> rtn_schs = this.Searches.Where(tent => 
                    (tent.PropertyName == propertyName && tent.Value != null && !String.IsNullOrEmpty(tent.Value.ToString()))).ToList();

                return rtn_schs;
            }

            return null;
        }

        /// <summary>
        /// 获取指定属性的CommonSearch
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IList<CommonSearchCriterionItem> GetSearches(string propertyName, SearchModeEnum schMode)
        {
            if (this.Searches != null && Searches.Count(tent => tent.PropertyName == propertyName && tent.SearchMode == schMode) > 0)
            {
                return this.Searches.Where(tent => tent.PropertyName == propertyName && tent.SearchMode == schMode).ToList();
            }

            return null;
        }

        /// <summary>
        /// 检查指定属性的CommonSearch是否存在
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public CommonSearchCriterionItem GetFirstSearch(string propertyName)
        {
            if (this.Searches != null && Searches.Count(tent => tent.PropertyName == propertyName) > 0)
            {
                return this.Searches.First(tent => tent.PropertyName == propertyName);
            }

            return null;
        }

        /// <summary>
        /// 检查指定属性的CommonSearch是否存在
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public CommonSearchCriterionItem GetFirstSearch(string propertyName, SearchModeEnum schMode)
        {
            if (this.Searches != null && Searches.Count(tent => tent.PropertyName == propertyName && tent.SearchMode == schMode) > 0)
            {
                return this.Searches.First(tent => tent.PropertyName == propertyName && tent.SearchMode == schMode);
            }

            return null;
        }

        #endregion

        /// <summary>
        /// 导出SQL语句（WHERE后）
        /// </summary>
        /// <returns></returns>
        public override string ToSQLString()
        {
            StringBuilder sql = new StringBuilder();
            string exp = JunctionMode.ToString();

            foreach (CommonSearchCriterionItem titem in this.Searches)
            {
                if (titem.Value != null && !String.IsNullOrEmpty(StringHelper.IsNullValue(titem.Value)))
                {
                    sql.Append(titem.ToSQLString() + exp);
                }
            }

            foreach (FTSearchCriterionItem titem in this.FTSearches)
            {
                if (titem.Value != null && !String.IsNullOrEmpty(StringHelper.IsNullValue(titem.Value)))
                {
                    sql.Append(titem.ToSQLString() + exp);
                }
            }

            foreach (JunctionSearchCriterionItem titem in this.JuncSearches)
            {
                string t_sqlstr = titem.ToSQLString();

                if (!String.IsNullOrEmpty(t_sqlstr))
                {
                    sql.Append(t_sqlstr + exp);
                }
            }

            string sqlstr = sql.ToString().TrimEnd(exp);

            if (!String.IsNullOrEmpty(sqlstr))
            {
                sqlstr = " (" + sqlstr + ") ";
            }

            return sqlstr;
        }

        #endregion
    }

    /// <summary>
    /// 排序规则项
    /// </summary>
    public class OrderCriterionItem : CriterionItem
    {
        #region 私有字段

        private string propertyName = String.Empty;

        #endregion

        #region 公共属性

        /// <summary>
        /// 字段名
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public bool Ascending { get; set; }

        #endregion

        #region 构造函数

        public OrderCriterionItem(string propertyName, bool ascending)
        {
            this.propertyName = propertyName;
            Ascending = ascending;
        }

        public OrderCriterionItem()
            : this(String.Empty, true)
        {
        }

        public OrderCriterionItem(string propertyName)
            : this(propertyName, true)
        {
        }

        public OrderCriterionItem(OrderCriterionItem item)
            : this(item.PropertyName, item.Ascending)
        {
        }

        #endregion

        #region 公共方法
        

        /// <summary>
        /// 导出SQL语句（WHERE后）
        /// </summary>
        /// <returns></returns>
        public override string ToSQLString()
        {
            string exp = " ";

            if (!Ascending)
            {
                exp = " DESC ";
            }

            return String.Format(" {0} {1} ", this.PropertyName, exp);
        }

        #endregion
    }
}
