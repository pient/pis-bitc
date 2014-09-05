using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace PIC
{
    [Serializable]
    [CollectionDataContract]
    public class EasyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        #region 构造函数

        public EasyDictionary()
        {
        }

        public EasyDictionary(IDictionary<TKey, TValue> innerDirectionary)
            : base(innerDirectionary)
        {
        }

        #endregion

        #region 属性索引

        new public virtual TValue this[TKey key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public virtual TValue this[TKey key, TValue defValue]
        {
            get
            {
                return Get(key, defValue);
            }
        }

        #endregion

        #region 公共方法

        public virtual TValue Get(TKey key)
        {
            return this.Get(key, default(TValue));
        }

        public virtual TValue Get(TKey key, TValue defValue)
        {
            TValue rtn = defValue;

            if (this.ContainsKey(key))
            {
                try
                {
                    rtn = base[key];
                }
                catch
                {
                }
            }

            return rtn;
        }

        public virtual void Set(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                base.Add(key, value);
            }
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class EasyDictionary: EasyDictionary<string, object>
    {
        #region 构造函数

        public EasyDictionary()
        {
        }

        public EasyDictionary(IDictionary<string, Object> innerDictionary)
            : base(innerDictionary)
        {
        }

        public EasyDictionary(IList<EasyDictionary> dicts)
        {
            string[] keyFields = null;

            string keystr = String.Empty;

            foreach (EasyDictionary tdict in dicts)
            {
                if (tdict.Keys.Count >= 2)
                {
                    keyFields = tdict.Keys.ToArray();

                    keystr = CLRHelper.ConvertValue<string>(tdict.Get(keyFields[0]));

                    this.Set(keystr, tdict.Get(keyFields[1]));
                }
            }
        }

        public EasyDictionary(IList<EasyDictionary> dicts, string keyField, string textField)
        {
            string keystr = String.Empty;

            foreach (EasyDictionary tdict in dicts)
            {
                keystr = CLRHelper.ConvertValue<string>(tdict.Get(keyField));

                this.Set(keystr, tdict.Get(textField));
            }
        }

        public EasyDictionary(DataTable dt, string keyColumnName, string textColumnName)
        {
            string keystr = String.Empty;

            foreach (DataRow trow in dt.Rows)
            {
                keystr = CLRHelper.ConvertValue<string>(trow[keyColumnName]);

                this.Set(keystr, trow[textColumnName]);
            }
        }

        public EasyDictionary(DataTable dt)
        {
            string keystr = String.Empty;

            if (dt.Columns.Count >= 2)
            {
                string keyColumnName = dt.Columns[0].ColumnName;
                string textColumnName = dt.Columns[1].ColumnName;

                foreach (DataRow trow in dt.Rows)
                {
                    keystr = CLRHelper.ConvertValue<string>(trow[keyColumnName]);

                    this.Set(keystr, trow[textColumnName]);
                }
            }
        }

        #endregion

        #region 属性索引

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataType"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public virtual object Get(string key, Type dataType, object defValue)
        {
            object rtnval = defValue;

            if (this.ContainsKey(key))
            {
                object val = this[key];

                if (val != null)
                {
                    try
                    {
                        val = Convert.ChangeType(val, dataType);
                    }
                    catch { }

                    rtnval = val;
                }
            }

            return rtnval;
        }

        public virtual T Get<T>(string key)
        {
            return this.Get<T>(key, default(T));
        }

        public virtual T Get<T>(string key, T defValue)
        {
            T rtn = defValue;

            var targetTypeCode = Type.GetTypeCode(typeof(T));

            if (this[key] != null)
            {
                if (this[key] is JObject)
                {
                    if (targetTypeCode == TypeCode.String)
                    {
                        rtn = CLRHelper.ConvertValue<T>(this[key].ToString());
                    }
                    else
                    {
                        rtn = JsonHelper.GetObject<T>(this[key].ToString());
                    }
                }
                else if (this[key] is string && targetTypeCode == TypeCode.Object)
                {
                    rtn = JsonHelper.GetObject<T>(this[key].ToString());
                }
                else
                {
                    rtn = CLRHelper.ConvertValue<T>(this[key]);
                }
            }

            return rtn;
        }

        #endregion
    }
}
