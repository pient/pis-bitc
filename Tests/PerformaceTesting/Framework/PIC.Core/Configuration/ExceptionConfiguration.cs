using System;
using System.Xml;

namespace PIC.Common.Configuration
{
    public class ExceptionConfiguration : BaseConfiguration
    {
        #region 属性

        private string m_exceptionPolicy = string.Empty;

        /// <summary>
        /// 默认异常处理策略
        /// </summary>
        public string ExceptionPolicy
        {
            get { return m_exceptionPolicy; }
        }

        #endregion

        #region 构造函数

        public ExceptionConfiguration(XmlNode sections)
            : base(sections)
        {
            if (configurationData.Attributes["ExceptionPolicy"] != null)
            {
                m_exceptionPolicy = StringHelper.IsEmptyValue(configurationData.Attributes["ExceptionPolicy"].Value);
            }
        }

        #endregion


        #region 私有函数

        #endregion
    }
}
