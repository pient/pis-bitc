using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;

namespace PIC.Common.Configuration
{
    public class ConfigurationHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// 创建配置管理器
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(Object parent, object configContext, XmlNode section)
        {
            // 调用配置文件构造函数
            object configObject = null;
            object[] parameters = { section };

            try
            {
                // configObject = new DefaultConfigurationManager(section);
                configObject = new PICConfigurationManager(section);
            }
            catch (System.Exception ex)
            {
                string x = ex.Message;
                return null;
            }

            return configObject;
        }
    }
}
