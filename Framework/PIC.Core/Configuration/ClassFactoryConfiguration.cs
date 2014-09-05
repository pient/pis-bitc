using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Common.Configuration
{
    public class ClassFactoryConfiguration : BaseConfiguration
    {
        #region 属性

        /// <summary>
        /// 工厂节点
        /// </summary>
        public XmlNodeList FactoryNodes
        {
            get { return configurationData.ChildNodes; }
        }

        #endregion

        #region 构造函数

        public ClassFactoryConfiguration(XmlNode sections)
            : base(sections)
        {
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 由获取类配置信息
        /// </summary>
        /// <param name="name">指定类工厂名称</param>
        /// <returns></returns>
        public XmlNode GetFactoryData(string name)
        {
            return configurationData.SelectSingleNode("Class[@name='" + name + "']");
        }

        #endregion

        #region 私有函数

        #endregion
    }
}
