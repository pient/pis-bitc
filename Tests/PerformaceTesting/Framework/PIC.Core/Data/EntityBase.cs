using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Internal;

using PIC.Aop;

namespace PIC.Data
{
    [Serializable]
    public abstract class EntityBase<T> : ActiveRecordBase<T>, IEntity, IPICNotifyPropertyChanged where T : EntityBase<T>
    {
        #region 成员属性

        public virtual event PICPropertyChangedEventHandler PICPropertyChanged;

        #endregion

        #region 构造函数

        public EntityBase()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            PICPropertyChanged += new PICPropertyChangedEventHandler(OnPropertyChanged);
        }

        /// <summary>
        /// 属性发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(object sender, PICPropertyChangedEventArgs e)
        {

        }

        public virtual void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (PICPropertyChanged != null)
            {
                PICPropertyChanged(this, new PICPropertyChangedEventArgs(propertyName, oldValue, newValue));
            }
        }

        #endregion

        #region ActiveRecordBase成员

        [Log]
        [Exception]
        public override void Save()
        {
            base.Save();
        }

        [Log]
        [Exception]
        public override void SaveAndFlush()
        {
            base.SaveAndFlush();
        }

        [Log]
        [Exception]
        public override void Create()
        {
            base.CreateAndFlush();
        }

        [Log]
        [Exception]
        public override void CreateAndFlush()
        {
            base.CreateAndFlush();
        }

        [Log]
        [Exception]
        public override void Update()
        {
            base.UpdateAndFlush();
        }

        [Log]
        [Exception]
        public override void UpdateAndFlush()
        {
            base.UpdateAndFlush();
        }

        [Log]
        [Exception]
        public override void Delete()
        {
            base.DeleteAndFlush();
        }

        [Log]
        [Exception]
        public override void DeleteAndFlush()
        {
            base.DeleteAndFlush();
        }

        /// <summary>
        /// 将对象从ActiveRecord缓存中释放
        /// </summary>
        public void Evict()
        {
            ActiveRecordMediator.Evict(this);
        }

        #endregion

        #region 方法成员

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        public virtual void SetValue(string propertyName, object val)
        {
            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            pi.SetValue(this, val, null);
        }

        /// <summary>
        /// 获取主键值
        /// </summary>
        public virtual object GetPrimaryValue()
        {
            return this.GetValue(EntityBase<T>.PrimaryKeyName);
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual object GetValue(string propertyName)
        {
            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            return pi.GetValue(this, null);
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual T1 GetValue<T1>(string propertyName)
        {
            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            return (T1)pi.GetValue(this, null);
        }

        /// <summary>
        /// 检查属性的唯一性情况(不能为空值)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="isCreate">创建情况，查找出记录即重复，其他存在非当前记录为重复</param>
        /// <returns></returns>
        public virtual bool IsPropertyUnique(string property)
        {
            object val = this.GetValue(property);
            object primaryVal = GetPrimaryValue();

            if (val == null)
            {
                return false;
            }

            bool isunique = false;

            if (primaryVal == null)
            {
                isunique = !ActiveRecordBase<T>.Exists(Expression.Eq(property, val));
            }
            else
            {
                isunique = !ActiveRecordBase<T>.Exists(
                    Expression.Eq(property, val),
                    Expression.Not(Expression.Eq(PrimaryKeyName, primaryVal)));
            }

            return isunique;
        }

        public override string ToString()
        {
            XmlSerializer xs = new XmlSerializer(GetType());
            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);
            xs.Serialize(tw, this);
            tw.Flush();
            string str = sb.ToString();
            tw.Close();
            return str;
        }

        #endregion

        #region 私有成员

        protected EntityBase<T> GetCopy()
        {
            EntityBase<T> ent = this.MemberwiseClone() as EntityBase<T>;

            return ent;
        }

        #endregion

        #region 静态方法


        /// <summary>
        /// 由主键获取实体(自动判断主键类型，并进行转换选择)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T AutoFind(object id)
        {
            object tid = null;

            if (id.GetType() != PrimaryKeyDataType)
            {
                if (typeof(Nullable<Guid>) == EntityBase<T>.PrimaryKeyDataType
                    || typeof(Guid) == EntityBase<T>.PrimaryKeyDataType)
                {
                    tid = id.ToGuid();
                }
                else
                {
                    tid = Convert.ChangeType(id, PrimaryKeyDataType);
                }
            }
            else
            {
                tid = id;
            }

            if (tid != null && tid.ToString().Trim().Length > 0) // tid 为空状态
            {
                return ActiveRecordMediator<T>.FindByPrimaryKey(tid);
            }
            else
            {
                return null;
            }
        }

        private static ReadOnlyCollection<PropertyInfo> allProperties = null;

        /// <summary>
        /// 获取所有属性
        /// </summary>
        public static ReadOnlyCollection<PropertyInfo> AllProperties
        {
            get
            {
                if (allProperties == null)
                {
                    allProperties = new ReadOnlyCollection<PropertyInfo>(typeof(T).GetProperties());
                }

                return allProperties;
            }
        }

        private static string className = null;

        /// <summary>
        /// 类名
        /// </summary>
        public static string ClassName
        {
            get
            {
                if (String.IsNullOrEmpty(className))
                {
                    className = typeof(T).Name;
                }

                return className;
            }
        }

        private static string tableName = null;

        /// <summary>
        /// 映射表名
        /// </summary>
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    if (tableName == null)
                    {
                        tableName = ActiveRecordModel.GetModel(typeof(T)).ActiveRecordAtt.Table;
                    }
                }

                return tableName;
            }
        }

        private static string primaryKeyName = null;

        /// <summary>
        /// 获取主键名
        /// </summary>
        public static String PrimaryKeyName
        {
            get
            {
                if (String.IsNullOrEmpty(primaryKeyName))
                {
                    primaryKeyName = ActiveRecordModel.GetModel(typeof(T)).PrimaryKey.PrimaryKeyAtt.Column;
                }

                return primaryKeyName;
            }
        }

        private static Type primaryKeyDataType = null;

        /// <summary>
        /// 主键类型
        /// </summary>
        public static Type PrimaryKeyDataType
        {
            get
            {
                if (!String.IsNullOrEmpty(PrimaryKeyName))
                {
                    primaryKeyDataType = ActiveRecordModel.GetModel(typeof(T)).PrimaryKey.Property.PropertyType;
                }

                return primaryKeyDataType;
            }
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion)
        {
            return FindAllByCriterion(criterion as HqlSearchCriterion);
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(SearchCriterion criterion, params ICriterion[] crits)
        {
            return FindAllByCriterion(criterion as HqlSearchCriterion, crits);
        }

        /// <summary>
        /// 过滤条件查询
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T[] FindAll(string querystr, params object[] args)
        {
            SimpleQuery<T> query = new SimpleQuery<T>(querystr, args);
            T[] targets = (T[])query.Execute();

            return targets;
        }

        public static T FindFirstByProperties(params object[] nameAndValues)
        {
            DetachedCriteria criteria = GetCriteriaByProperties(nameAndValues);

            return FindFirst(criteria);
        }

        public static T[] FindAllByProperties(params object[] nameAndValues)
        {
            DetachedCriteria criteria = GetCriteriaByProperties(nameAndValues);

            return FindAll(criteria, null);
        }

        public static T[] FindAllByProperties(int ascOrdesc, string orderbyprop, params object[] nameAndValues)
        {
            DetachedCriteria criteria = GetCriteriaByProperties(nameAndValues);

            Order order = new Order(orderbyprop, true);
            if (ascOrdesc == 1)
            {
                order = new Order(orderbyprop, false);
            }

            return FindAll(criteria, order);
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAllBySearchCriterion(HqlSearchCriterion criterion)
        {
            return criterion.FindAll<T>();
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAllByCriterion(HqlSearchCriterion criterion, params ICriterion[] crits)
        {
            return criterion.FindAll<T>(crits);
        }

        /// <summary>
        /// 根据主键列表查询实体
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T[] FindAllByPrimaryKeys(params object[] args)
        {
            T[] ents = ActiveRecordMediator<T>.FindAll(Expression.In(PrimaryKeyName, args));

            return ents;
        }

        /// <summary>
        /// 由属性值获取Criteria
        /// </summary>
        /// <returns></returns>
        public static DetachedCriteria GetCriteriaByProperties(params object[] nameAndValues)
        {
            if (nameAndValues.Length % 2 == 1)
            {
                throw new Exception("FindAllByPropertys参数数目不正确！");
            }

            SimpleExpression exp;
            int co = nameAndValues.Length / 2;
            DetachedCriteria criteria = DetachedCriteria.For(typeof(T));

            for (int i = 0; i < nameAndValues.Length / 2; i++)
            {
                exp = Expression.Eq(nameAndValues[i * 2].ToString(), nameAndValues[i * 2 + 1]);
                criteria.Add(exp);
            }

            return criteria;
        }

        #endregion
    }
}
