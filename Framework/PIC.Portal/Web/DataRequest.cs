using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using PIC.Data;

namespace PIC.Portal.Web
{
    /// <summary>
    /// 获取的数据
    /// </summary>
    [CollectionDataContract]
    public class DataRequest : EasyDictionary
    {
        #region Consts Key

        public const string IdListKey = "IdList";

        #endregion

        #region 构造函数

        public DataRequest()
        {
        }

        public DataRequest(IDictionary<string, Object> innerDictionary)
            : base(innerDictionary)
        {
        }

        #endregion

        #region 属性索引

        #endregion

        #region 公共方法

        public override T Get<T>(string key, T defValue)
        {
            if (this[key] != null && this[key] is JObject)
            {
                if (typeof(T) == typeof(string))
                {
                    return CLRHelper.ConvertValue<T>(this[key].ToString());
                }
                else
                {
                    return JsonHelper.GetObject<T>(this[key].ToString());
                }
            }
            else
            {
                return base.Get<T>(key, defValue);
            }
        }

        public virtual Guid? GetGuid(string key, Guid? defValue = null)
        {
            var rtnVal = defValue;

            var origVal = this.Get(key, null);

            if (origVal != null)
            {
                rtnVal = origVal.ToGuid(defValue);
            }

            return rtnVal;
        }

        public virtual IList<Guid> GetGuidList(string key)
        {
            var rtnList = new List<Guid>();

            if (this.ContainsKey(key))
            {
                var list = this.GetList<string>(key);

                if (list != null)
                {
                    Guid? tId = null;

                    foreach (var id in list)
                    {
                        tId = id.ToGuid();

                        if (tId != null)
                        {
                            rtnList.Add(tId.Value);
                        }
                    }
                }
            }

            return rtnList;
        }

        public virtual IList<Guid> GetGuidIdList()
        {
            return this.GetGuidList(IdListKey);
        }

        public virtual IList<object> GetIdList()
        {
            return this.GetList<object>(IdListKey);
        }

        public virtual IList<T> GetIdList<T>()
        {
            return this.GetList<T>(IdListKey);
        }

        public virtual IList<T> GetList<T>(string key)
        {
            IList<T> rtn = null;

            JArray vals = null;
            if (this[key] != null)
            {
                if (this[key] is JArray)
                {
                    vals = this[key] as JArray;
                }
            }

            if (vals != null)
            {
                IEnumerable<T> ids = vals.Values<T>();
                rtn = ids.ToList();
            }

            return rtn;
        }

        #endregion
    }
}
