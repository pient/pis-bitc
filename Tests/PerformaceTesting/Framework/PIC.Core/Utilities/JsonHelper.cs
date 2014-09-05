using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace PIC
{
    /// <summary>
    /// 对Newtonsoft.Json的扩展
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 反序列化Json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static T GetObject<T>(string jsonstr)
        {
            return JsonConvert.DeserializeObject<T>(jsonstr);
        }

        /// <summary>
        /// 格式化Json
        /// </summary>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static string FormatJsonString(string jsonstr)
        {
            JObject json = JObject.Parse(jsonstr);
            string formatted = json.ToString();

            return formatted;
        }

        /// <summary>
        /// 反序列号Json List对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static IList<T> GetObjectList<T>(object jsonobj)
        {
            IList<T> rtn = null;

            JArray vals = jsonobj as JArray;

            if (vals != null)
            {
                IEnumerable<T> ids = vals.Values<T>();
                rtn = ids.ToList();
            }

            return rtn;
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJsonString(object obj)
        {
            if (obj == null)
            {
                return String.Empty;
            }

            return JsonConvert.SerializeObject(obj, Formatting.None);
        }

        /// <summary>
        /// 从DataTable获取Json字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetJsonStringFromDataTable(DataTable dt)
        {
            string jstr = JsonConvert.SerializeObject(dt, new DataTableConverter());

            return jstr;
        }
    }
}
