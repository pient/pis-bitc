using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Common.Configuration
{
    public class ServiceConfiguration : BaseConfiguration
    {
        #region 属性

        // 普通服务地址
        public string CommonServicePath
        {
            get
            {
                XmlNode commonServicePathNode = configurationData.SelectSingleNode(@"./CommonService/ServicesPath");
                if (commonServicePathNode == null)
                {
                    return String.Empty;
                }
                else
                {
                    return commonServicePathNode.InnerText;
                }
            }
        }

        public EasyDictionary UserSession
        {
            get
            {
                return this.RetrieveAttributeSettings(@"./UserSession");
            }
        }

        #endregion

        #region 构造函数

        public ServiceConfiguration(XmlNode sections)
            : base(sections)
        {
        }

        #endregion
    }
}
