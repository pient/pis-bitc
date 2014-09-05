using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NVelocity;
using Newtonsoft.Json;

namespace PIC.Portal.Template
{
    /// <summary>
    /// 模版上下文，借用NVelocity概念
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class TmplContext : EasyDictionary<string, object>
    {
        #region Enums & Classes

        /// <summary>
        /// 模版上下文实用类，由于NVelocity无法调用静态方法
        /// 先将模版上下文实用的一些方法集合放到TmplContextUtil
        /// </summary>
        public class TmplContextUtil
        {
            #region 公共方法 

            public object GetJsonObj(string str)
            {
                if (String.IsNullOrEmpty(str))
                {
                    return null;
                }

                var obj = JsonConvert.DeserializeObject(str);

                return obj;
            }

            /// <summary>
            /// 返回
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public string GetJsonStr(Object obj)
            {
                var rtnStr = JsonHelper.GetJsonString(obj);

                return rtnStr;
            }

            #endregion
        }

        #endregion

        #region 构造函数

        public TmplContext()
        {
        }

        public TmplContext(IDictionary<string, object> innerDirectionary)
            : base(innerDirectionary)
        {
        }

        /// <summary>
        /// 获取NVelocity上下对象
        /// </summary>
        /// <returns></returns>
        public VelocityContext ToVelocityContext()
        {
            var context = new VelocityContext();

            foreach (var key in this.Keys)
            {
                context.Put(key, this[key]);
            }

            return context;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 获取系统上下文
        /// </summary>
        /// <returns></returns>
        public static TmplContext GetSysContext()
        {
            var ctx = new TmplContext();

            ctx.Set("Util", new TmplContextUtil());
            ctx.Set("Date", DateTime.Now);
            ctx.Set("UserInfo", PortalService.CurrentUserInfo);
            ctx.Set("SystemInfo", PortalService.SystemInfo);

            return ctx;
        }

        #endregion
    }
}
