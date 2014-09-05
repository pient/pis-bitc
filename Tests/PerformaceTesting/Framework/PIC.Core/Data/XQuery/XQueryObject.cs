using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Common;

namespace PIC.Data.XQuery
{
    public abstract class XQueryObject
    {
        public const string XQUERY_PROVIDER_KEY = "XQueryProvider";

        #region 属性 

        /// <summary>
        /// XQuery查询
        /// </summary>
        private static IXQueryProvider xQueryProvider = null;
        public IXQueryProvider XQueryProvider
        {
            get
            {
                if (xQueryProvider == null)
                {
                    xQueryProvider = RetrieveXQueryProvider();
                }

                return xQueryProvider;
            }
        }

        #endregion

        #region 重载方法

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public abstract string RetrieveData();

        /// <summary>
        /// 获取XQueryProvider
        /// </summary>
        /// <returns></returns>
        protected virtual IXQueryProvider RetrieveXQueryProvider()
        {
            IXQueryProvider provider = null;

            if (PICConfigurationManager.AppSettings.ContainsKey(XQUERY_PROVIDER_KEY))
            {
                string typeName = PICConfigurationManager.AppSettings[XQUERY_PROVIDER_KEY];

                provider = (IXQueryProvider)Activator.CreateInstance(Type.GetType(typeName));
            }
            else
            {
                // 默认SqlXQueryProvider
                provider = new SqlXQueryProvider();
            }

            return provider;
        }

        #endregion
    }
}
