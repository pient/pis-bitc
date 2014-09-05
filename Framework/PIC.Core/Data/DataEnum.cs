using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    /// <summary>
    /// 数据枚举
    /// </summary>
    public class DataEnum : Dictionary<string, string>
    {

    }

    /// <summary>
    /// 数据枚举增强
    /// </summary>
    public class DataEnumEx<T> : IList<DataEnumItem<T>>
    {
        #region 私有成员

        private List<DataEnumItem<T>> m_items = new List<DataEnumItem<T>>();

        #endregion

        #region 构造函数

        public DataEnumEx() { }

        public DataEnumEx(IList<DataEnumItem<T>> items)
        {
            foreach (DataEnumItem<T> t_item in items)
            {
                this.Add(t_item);
            }
        }

        public DataEnumEx(Dictionary<T, string> dict)
        {
            foreach (T key in dict.Keys)
            {
                this.Add(key, dict[key]);
            }
        }

        #endregion

        #region IList<DataEnumItem> Members

        public int IndexOf(DataEnumItem<T> item)
        {
            return m_items.IndexOf(item);
        }

        public void Insert(int index, DataEnumItem<T> item)
        {
            if(!this.Exists(item.Value))
            {
                this.m_items.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            m_items.RemoveAt(index);
        }

        public DataEnumItem<T> this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DataEnumItem<T> this[T value]
        {
            get
            {
                return this.GetItem(value);
            }
        }

        #endregion

        #region ICollection<DataEnumItem<T>> Members

        public void Add(DataEnumItem<T> item)
        {
            if (!Exists(item.Value))
            {
                this.Add(item);
            }
        }

        public void Clear()
        {
            this.m_items.Clear();
        }

        public bool Contains(DataEnumItem<T> item)
        {
            return m_items.Contains(item);
        }

        public void CopyTo(DataEnumItem<T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return m_items.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(DataEnumItem<T> item)
        {
            return this.Remove(item);
        }

        #endregion

        #region IEnumerable<DataEnumItem<T>> Members

        public IEnumerator<DataEnumItem<T>> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 公有函数

        public bool Add(T value, string text)
        {
            if (Exists(value) || value == null)
            {
                return false;
            }
            else
            {
                m_items.Add(new DataEnumItem<T>() { Value = value, DisplayText = text });
                return true;
            }
        }

        public bool Add(T value, object tag)
        {
            if (Exists(value) || value == null)
            {
                return false;
            }
            else
            {
                m_items.Add(new DataEnumItem<T>() { Value = value, Tag = tag });
                return true;
            }
        }

        public bool Add(T value, string text, object tag)
        {
            if (Exists(value) || value == null)
            {
                return false;
            }
            else
            {
                m_items.Add(new DataEnumItem<T>() { Value = value, DisplayText = text, Tag = tag });
                return true;
            }
        }

        public bool Remove(T value)
        {
            DataEnumItem<T> t_item = GetItem(value);
            if (t_item == null)
            {
                return false;
            }
            else
            {
                return m_items.Remove(t_item);
            }
        }

        public string GetTag(T value)
        {
            var t_items = from t in m_items where t.Value.Equals(value) select t.DisplayText;
            if (t_items == null || t_items.Count() <= 0)
            {
                return null;
            }
            else
            {
                return t_items.First();
            }
        }

        public string GetText(T value)
        {
            var t_items = from t in m_items where t.Value.Equals(value) select t.DisplayText;
            if (t_items == null || t_items.Count() <= 0)
            {
                return null;
            }
            else
            {
                return t_items.First();
            }
        }

        public DataEnumItem<T> GetItem(T value)
        {
            var t_items = from t in m_items where t.Value.Equals(value) select t;
            if (t_items == null || t_items.Count() <= 0)
            {
                return null;
            }
            else
            {
                return t_items.First<DataEnumItem<T>>();
            }
        }

        public Object GetValueByText(string text)
        {
            var t_items = from t in m_items where t.DisplayText == text select t.Value;
            if (t_items == null || t_items.Count() <= 0)
            {
                return null;
            }
            else
            {
                return t_items.First();
            }
        }

        public DataEnumItem<T> GetItemByText(string text)
        {
            var t_items = from t in m_items where t.DisplayText == text select t;
            if (t_items == null || t_items.Count() <= 0)
            {
                return null;
            }
            else
            {
                return t_items.First<DataEnumItem<T>>();
            }
        }

        /// <summary>
        /// 检查指定键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(T value)
        {
            return (this.GetItem(value) != null);
        }

        #endregion
    }

    public class DataEnumItem<T>
    {
        /// <summary>
        /// 显示值
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// 枚举值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 附带值
        /// </summary>
        public object Tag { get; set; }
    }

    public class DataEnumEx : DataEnumEx<string>
    {
        #region 构造函数

        public DataEnumEx() { }

        public DataEnumEx(IList<DataEnumItem> items)
        {
            foreach (DataEnumItem t_item in items)
            {
                this.Add(t_item);
            }
        }

        public DataEnumEx(IList<DataEnumItem<string>> items)
        {
            foreach (DataEnumItem<string> t_item in items)
            {
                this.Add(t_item);
            }
        }

        public DataEnumEx(Dictionary<string, string> dict)
        {
            foreach (string value in dict.Keys)
            {
                this.Add(value, dict[value]);
            }
        }

        public DataEnumEx(DataEnum denum)
        {
            foreach (string value in denum.Keys)
            {
                this.Add(value, denum[value]);
            }
        }

        #endregion
    }

    public class DataEnumItem : DataEnumItem<string>
    {
    }
}
