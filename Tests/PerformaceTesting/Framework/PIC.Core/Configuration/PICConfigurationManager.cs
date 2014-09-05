using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PIC.Security;
using PIC.Common.Configuration;

namespace PIC.Common
{
    public class PICConfigurationManager : BaseConfigurationManager
    {
        #region 成员

        public const string SourceSectionName = @"Source";
        public const string TopSectionName = @"PIC";
        public const string SysSectionPath = @"//System";    // 系统配置节路径

        public const string LogSectionName = @"Log";    // 服务配置节名
        public const string ServicesSectionName = @"Services";    // 服务配置节名
        public const string ThirdPartiesSectionName = @"ThirdParties";    // 第三方配置节名
        public const string CacheSectionName = @"Cache";    // 缓存配置节名
        public const string ExceptionSectionName = @"Exception";    // 异常配置节名

        private SystemConfiguration m_systemConfiguration;
        private LogConfiguration m_logConfiguration;
        private ServiceConfiguration m_servicesConfiguration;
        private ThirdPartyConfiguration m_thirdPartiesConfiguration;
        private CacheConfiguration m_cacheConfiguration;
        private ExceptionConfiguration m_exceptionConfiguration;

        #endregion

        #region 属性

        private static PICConfigurationManager _instance;

        public static PICConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = System.Configuration.ConfigurationManager.GetSection(TopSectionName) as PICConfigurationManager;
                }

                return _instance;
            }
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SystemConfiguration SystemConfiguration
        {
            get
            {
                return Instance.m_systemConfiguration;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static Dictionary<string, string> ConnectionStrings
        {
            get
            {
                return SystemConfiguration.ConnectionStrings;
            }
        }

        /// <summary>
        /// 程序定义
        /// </summary>
        public static Dictionary<string, string> AppSettings
        {
            get { return SystemConfiguration.AppSettings; }
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        public static LogConfiguration LogConfiguration
        {
            get
            {
                if (Instance.m_logConfiguration == null)
                {
                    if (SystemConfiguration.Configurations.ContainsKey(LogSectionName))
                    {
                        Instance.m_logConfiguration = SystemConfiguration.Configurations[LogSectionName] as LogConfiguration;
                    }
                }

                return Instance.m_logConfiguration;
            }
        }

        /// <summary>
        /// 服务配置
        /// </summary>
        public static ServiceConfiguration ServicesConfiguration
        {
            get
            {
                if (Instance.m_servicesConfiguration == null)
                {
                    if (SystemConfiguration.Configurations.ContainsKey(ServicesSectionName))
                    {
                        Instance.m_servicesConfiguration = SystemConfiguration.Configurations[ServicesSectionName] as ServiceConfiguration;
                    }
                }

                return Instance.m_servicesConfiguration;
            }
        }

        /// <summary>
        /// 第三方配置
        /// </summary>
        public static ThirdPartyConfiguration ThirdPartiesConfiguration
        {
            get
            {
                if (Instance.m_thirdPartiesConfiguration == null)
                {
                    if (SystemConfiguration.Configurations.ContainsKey(ThirdPartiesSectionName))
                    {
                        Instance.m_thirdPartiesConfiguration = SystemConfiguration.Configurations[ThirdPartiesSectionName] as ThirdPartyConfiguration;
                    }
                }

                return Instance.m_thirdPartiesConfiguration;
            }
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public static CacheConfiguration CacheConfiguration
        {
            get
            {
                if (Instance.m_cacheConfiguration == null)
                {
                    if (SystemConfiguration.Configurations.ContainsKey(CacheSectionName))
                    {
                        Instance.m_cacheConfiguration = SystemConfiguration.Configurations[CacheSectionName] as CacheConfiguration;
                    }
                }

                return Instance.m_cacheConfiguration;
            }
        }

        /// <summary>
        /// 异常配置
        /// </summary>
        public static ExceptionConfiguration ExceptionConfiguration
        {
            get
            {
                if (Instance.m_exceptionConfiguration == null)
                {
                    if (SystemConfiguration.Configurations.ContainsKey(ExceptionSectionName))
                    {
                        Instance.m_exceptionConfiguration = SystemConfiguration.Configurations[ExceptionSectionName] as ExceptionConfiguration;
                    }
                }

                return Instance.m_exceptionConfiguration;
            }
        }

        #endregion

        #region 构造函数

        internal PICConfigurationManager(XmlNode sections)
            : base(sections)
        {
            Initialize();
        }

        #endregion

        #region IConfiguration实现

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            if (configurationData != null)
            {
                XmlNode sysConfigurationData = configurationData.SelectSingleNode(SysSectionPath);

                if (sysConfigurationData.Attributes[SourceSectionName] != null)
                {
                    string source = sysConfigurationData.Attributes[SourceSectionName].Value;

                    if (!String.IsNullOrEmpty(source))
                    {
                        source = SystemHelper.GetPath(source);

                        if (File.Exists(source))
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(source);

                            sysConfigurationData = xmlDoc.DocumentElement;
                        }
                    }
                }

                if (IsProtectedConfig(sysConfigurationData))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    string configstr = DecryptConfig(sysConfigurationData);
                    xmlDoc.LoadXml(configstr);

                    sysConfigurationData = xmlDoc.DocumentElement;
                }

                m_systemConfiguration = new SystemConfiguration(sysConfigurationData);
            }
        }

        /// <summary>
        /// 重新从本地加载文件
        /// </summary>
        public void Reload()
        {
            try
            {
                ReloadConfiguration();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 加密配置数据
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static string EncryptConfig(XmlNode configData, string code)
        {
            string configstr = configData.OuterXml;

            configstr = DESEncrypt.DoEncryptStringByMAC(configstr, code);

            configstr = "<System IsProtected=\"true\">" + configstr + "</System>";

            return configstr;
        }

        /// <summary>
        /// 由本地信息揭秘数据
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static string DecryptConfig(XmlNode configData)
        {
            string maccode = SystemHelper.GetMACAddress();

            return DecryptConfig(configData, maccode);
        }

        /// <summary>
        /// 由本地信息揭秘数据(此函数不应暴露，这里临时使用)
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static string DecryptConfig(XmlNode configData, string maccode)
        {
            string configstr = configData.InnerXml;

            configstr = DESEncrypt.DoDecryptStringByMAC(configstr, maccode);

            return configstr;
        }

        /// <summary>
        /// 判断是否加密数据
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static bool IsProtectedConfig(XmlNode configData)
        {
            bool isPortected = false;

            // 配置数据被加密, 先进行解密
            if (configData.Attributes["IsProtected"] != null
                && StringHelper.IsEqualsIgnoreCase(configData.Attributes["IsProtected"].Value, "true"))
            {
                isPortected = true;
            }

            return isPortected;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 刷新配置文件
        /// </summary>
        private void ReloadConfiguration()
        {
            SystemConfiguration.Reload();
        }

        #endregion
    }
}
