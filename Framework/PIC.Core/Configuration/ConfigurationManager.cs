using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Configuration;

namespace PIC.Common.Configuration
{
    /// <summary>
    /// 配置管理器接口
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// 配置信息集合
        /// </summary>
        Dictionary<Type, IConfiguration> Configurations { get; }
    }


    /// <summary>
    /// 基配置管理类
    /// </summary>
    public class BaseConfigurationManager : IConfigurationManager
    {
        protected XmlNode configurationData;

        #region 构造函数

        /// <summary>
        /// 配置数据
        /// </summary>
        /// <param name="configurationData"></param>
        public BaseConfigurationManager(XmlNode configurationData)
        {
            this.configurationData = configurationData;

            configurations = new Dictionary<Type, IConfiguration>();
        }

        #endregion

        #region IConfigurationManager实现

        protected Dictionary<Type, IConfiguration> configurations;

        public Dictionary<Type, IConfiguration> Configurations
        {
            get { return configurations; }
        }

        #endregion
    }
}
