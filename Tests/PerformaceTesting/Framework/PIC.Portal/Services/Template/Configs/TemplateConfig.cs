using PIC.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PIC.Portal.Template
{
    public abstract class TemplateConfig
    {
        #region Enums & Classes

        /// <summary>
        /// 数据上下文的类型，目前只支持HqlQuery和String
        /// </summary>
        public enum DataType
        {
            HqlQuery,   // Hql查询
            String  // 字符串
        }

        public class HqlDataQuery : DataContextItem
        {
            #region 属性

            [JsonIgnore]
            public int Rows // 查询返回行数, 没写默认返回10行
            {
                get
                {
                    return this.Get<int>("Rows", 10);
                }
            }

            [JsonIgnore]
            public string String    // 查询字符串
            {
                get
                {
                    return this.Get<string>("String");
                }
            } 

            #endregion

            #region 公共方法

            /// <summary>
            /// 获取数据
            /// </summary>
            /// <returns></returns>
            public object GetData(EasyDictionary qryParams = null)
            {
                if (!String.IsNullOrEmpty(this.String))
                {
                    var session = DataHelper.OpenHqlSession();

                    var qry = DataHelper.GetHqlQuery(session, this.String);

                    if (qryParams != null && qry.NamedParameters.Count() > 0)
                    {
                        foreach (var p in qry.NamedParameters)
                        {
                            var pv = qryParams.Get(p);

                            qry.SetParameter(p, pv);
                        }
                    }

                    qry.SetMaxResults(this.Rows);

                    var list = qry.List();

                    DataHelper.ReleaseHqlSessin(session);

                    return list;
                }

                return null;
            }

            #endregion
        }

        /// <summary>
        /// 数据上下文项
        /// </summary>
        public class DataContextItem : EasyDictionary
        {
            #region 方法

            public DataType GetDataType()
            {
                    var dtType = DataType.String;

                    var typeStr = this.Get<string>("Type");

                    if (!String.IsNullOrEmpty(typeStr))
                    {
                        dtType = CLRHelper.GetEnum<DataType>(typeStr);
                    }

                    return dtType;
            }

            #endregion
        }

        /// <summary>
        /// 数据上下文配置
        /// </summary>
        public class DataContextConfig : EasyDictionary
        {
            #region 公共方法

            /// <summary>
            /// 获取数据上下文
            /// </summary>
            /// <param name="qryParams"></param>
            /// <returns></returns>
            public TmplContext GetDataContext(EasyDictionary qryParams = null, bool isSysContext = true)
            {
                TmplContext tmplCtx = new TmplContext();

                if (isSysContext == true)
                {
                    tmplCtx = TmplContext.GetSysContext();
                }

                foreach (var key in this.Keys)
                {
                    var ctxObj = this.Get(key);

                    var ctxData = ctxObj;

                    if (ctxObj is JObject)
                    {
                        var ctxJObj = ctxObj as JObject;
                        var ctxItem = ctxJObj.ToObject<DataContextItem>();

                        if (ctxItem != null)
                        {
                            switch (ctxItem.GetDataType())
                            {
                                case DataType.HqlQuery:
                                    var ctxQryItem = ctxJObj.ToObject<HqlDataQuery>();

                                    if (ctxQryItem != null)
                                    {
                                        ctxData = ctxQryItem.GetData(qryParams);
                                    }
                                    break;
                            }
                        }
                    }

                    tmplCtx.Set(key, ctxData);
                }

                return tmplCtx;
            }

            #endregion
        }

        #endregion

        #region 公共方法

        public abstract string RenderString(EasyDictionary ctxParams = null);

        #endregion
    }
}
