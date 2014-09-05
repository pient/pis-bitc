using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Configuration;

using PIC.Common.Configuration;

namespace PIC.Common
{
    /// <summary>
    /// 配置文件管理器宿主，方便以后在扩展时使用不同的Manager（如将配置文件保存在数据库中时）
    /// </summary>
    public static class ConfigurationHosting
    {
        public const string TopSectionName = @"PIC";

        #region 属性

        /// <summary>
        /// 默认配置管理器
        /// </summary>
        public static DefaultConfigurationManager ConfigManager
        {
            get
            {
                DefaultConfigurationManager configManager = ConfigurationManager.GetSection(TopSectionName) as DefaultConfigurationManager;
                return configManager;
            }
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SystemConfiguration SystemConfiguration
        {
            get { return ConfigManager.SystemConfiguration; }
        }

        public static Dictionary<Type, IConfiguration> DefaultConfigurations
        {
            get { return ConfigManager.Configurations; }
        }

        /// <summary>
        /// 系统服务配置
        /// </summary>
        public static ServicesConfiguration ServicesConfiguration
        {
            get { return ConfigManager.ServicesConfiguration; }
        }

        /// <summary>
        /// 第三方配置
        /// </summary>
        public static ThirdPartiesConfiguration ThirdPartiesConfiguration
        {
            get { return ConfigManager.ThirdPartiesConfiguration; }
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public static CacheConfiguration CacheConfiguration
        {
            get { return ConfigManager.CacheConfiguration; }
        }

        /// <summary>
        /// 异常处理配置
        /// </summary>
        public static ExceptionConfiguration ExceptionConfiguration
        {
            get { return ConfigManager.ExceptionConfiguration; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 单体模式
        /// </summary>
        static ConfigurationHosting()
        {
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取指定类型配置
        /// </summary>
        /// <param name="configurationManagerType">配置管理器类型</param>
        /// <returns></returns>
        public static IConfigurationManager RetrieveConfigurationManager(string configurationManager)
        {
            return SystemConfiguration.ConfigurationManagers[configurationManager];
        }

        #endregion
    }
}
