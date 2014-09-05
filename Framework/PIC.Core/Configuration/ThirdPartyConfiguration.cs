using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Common.Configuration
{
    public class ThirdPartyConfiguration : BaseConfiguration
    {
        #region 属性

        /// <summary>
        /// 服务节点
        /// </summary>
        public XmlNodeList ThirdPartyNodes
        {
            get { return configurationData.ChildNodes; }
        }

        #endregion

        #region 构造函数

        public ThirdPartyConfiguration(XmlNode sections)
            : base(sections)
        {
        }

        #endregion

        #region 公共方法

        public override XmlNode GetConfig(string configName)
        {
            XmlNode xmlNode = configurationData.SelectSingleNode(@"//" + configName);

            return xmlNode;
        }

        #endregion

        #region 私有函数

        #endregion
    }
}
