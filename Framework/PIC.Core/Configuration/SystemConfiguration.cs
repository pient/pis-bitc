using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using PIC.Security;

namespace PIC.Common.Configuration
{
    /// <summary>
    /// 配置文件接口
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        void Reload();

        /// <summary>
        /// 配置名
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// 基配置类
    /// </summary>
    public abstract class BaseConfiguration : IConfiguration
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        protected XmlNode configurationData;

        private string m_name = String.Empty;

        /// <summary>
        /// 配置名
        /// </summary>
        public virtual string Name
        {
            get { return configurationData.Name; }
        }

        internal BaseConfiguration(XmlNode configurationData)
        {
            this.configurationData = configurationData;
        }

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public virtual void Reload() { }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public virtual XmlNode GetConfig(string configName = ".")
        {
            XmlNode xmlNode = configurationData.SelectSingleNode(@"./" + configName);

            return xmlNode;
        }

        #region 保护方法

        /// <summary>
        /// 获取指定节下属性配置信息配置信息
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public virtual string RetrieveAttributeSetting(string sectionPath, string attributeName)
        {
            XmlNode xmlData = configurationData.SelectSingleNode(sectionPath);

            string setting = String.Empty;

            if (xmlData != null)
            {
                if (xmlData != null && xmlData.Attributes[attributeName] != null)
                {
                    setting = xmlData.Attributes[attributeName].Value;
                }
            }

            return setting;
        }

        /// <summary>
        /// 获取指定节下属性信息并转换为字典信息
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <returns></returns>
        public virtual EasyDictionary RetrieveAttributeSettings(string sectionPath)
        {
            XmlNode xmlData = configurationData.SelectSingleNode(sectionPath);

            EasyDictionary settings = new EasyDictionary();

            if (xmlData != null)
            {
                foreach (XmlAttribute attr in xmlData.Attributes)
                {
                    settings.Add(attr.Name, attr.Value);
                }
            }

            return settings;
        }

        /// <summary>
        /// 获取指定节下配置信息并转换为字典信息
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <returns></returns>
        public virtual EasyDictionary RetrieveNodeSettings(string sectionPath)
        {
            XmlNode xmlData = configurationData.SelectSingleNode(sectionPath);

            EasyDictionary settings = new EasyDictionary();

            if (xmlData != null)
            {
                foreach (XmlNode typeNode in xmlData.ChildNodes)
                {
                    settings.Add(typeNode.Name, typeNode.InnerXml);
                }
            }

            return settings;
        }

        /// <summary>
        /// 由类型属性创建对象
        /// </summary>
        /// <param name="typeAttribute"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object CreateObjectByAttribute(string typeAttribute, params object[] args)
        {
            if (configurationData != null && configurationData.Attributes[typeAttribute] != null
                && !String.IsNullOrEmpty(configurationData.Attributes[typeAttribute].Value))
            {
                return Activator.CreateInstance(Type.GetType(configurationData.Attributes[typeAttribute].Value), args);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }

    /// <summary>
    /// 系统配置，必须存在
    /// </summary>
    public class SystemConfiguration : BaseConfiguration
    {
        #region 属性

        private Dictionary<string, IConfigurationManager> m_configurationManagers = new Dictionary<string, IConfigurationManager>();

        /// <summary>
        /// 配置管理器列表
        /// </summary>
        public Dictionary<string, IConfigurationManager> ConfigurationManagers
        {
            get { return m_configurationManagers; }
        }

        private Dictionary<string, IConfiguration> m_configurations = new Dictionary<string, IConfiguration>();

        /// <summary>
        /// 配置列表
        /// </summary>
        public Dictionary<string, IConfiguration> Configurations
        {
            get { return m_configurations; }
        }

        private Dictionary<string, string> m_connectionStrings = new Dictionary<string, string>();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public Dictionary<string, string> ConnectionStrings
        {
            get { return m_connectionStrings; }
        }

        private Dictionary<string, string> m_appSettings = new Dictionary<string, string>();

        /// <summary>
        /// 程序定义
        /// </summary>
        public Dictionary<string, string> AppSettings
        {
            get { return m_appSettings; }
        }

        #endregion

        #region 构造函数

        internal SystemConfiguration(XmlNode configurationData)
            : base(configurationData)
        {
            Initialize();
        }

        #endregion

        #region BaseConfiguration实现

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public override void Reload()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public void Initialize()
        {
            m_connectionStrings = new Dictionary<string, string>();
            m_appSettings = new Dictionary<string, string>();

            EasyDictionary tmp_dict = RetrieveNodeSettings(@"//ConnectionStrings"); // 获取数据库连接字符串列表
            foreach (string key in tmp_dict.Keys)
            {
                m_connectionStrings.Add(key, tmp_dict.Get<string>(key));
            }

            tmp_dict = RetrieveNodeSettings(@"//AppSettings"); // 获取自定义配置信息
            foreach (string key in tmp_dict.Keys)
            {
                m_appSettings.Add(key, tmp_dict.Get<string>(key));
            }

            InitializeConfigurationsInfo(); // 初始化配置文件信息
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 获取配置信息列表(配置管理器，和配置)
        /// </summary>
        private void InitializeConfigurationsInfo()
        {
            XmlNode xmlData = configurationData.SelectSingleNode(@"//Configurations");
            XmlNode sections = xmlData.SelectSingleNode(@"//ConfigurationSection");
            XmlNode datas = xmlData.SelectSingleNode(@"//ConfigurationData");

            if (sections != null)
            {
                foreach (XmlNode typeNode in sections.SelectNodes(@"./Section"))
                {
                    string typestr = ((typeNode.Attributes["Type"] == null) ? String.Empty : typeNode.Attributes["Type"].Value);
                    string secname = ((typeNode.Attributes["Name"] == null) ? String.Empty : typeNode.Attributes["Name"].Value);
                    string typecategory = ((typeNode.Attributes["Category"] == null) ? "configuration" : typeNode.Attributes["Category"].Value);

                    if (!String.IsNullOrEmpty(typestr) || !String.IsNullOrEmpty(secname))
                    {
                        object[] parameters = { datas.SelectSingleNode(@"//" + secname) }; // 类型参宿信息

                        try
                        {
                            if (String.IsNullOrEmpty(typecategory) || typecategory.ToLower() == "configuration")
                            {
                                if (m_configurations.ContainsKey(secname))
                                {
                                    m_configurations.Remove(secname);
                                }

                                m_configurations.Add(secname, (IConfiguration)Activator.CreateInstance(Type.GetType(typestr), parameters));
                            }
                            else if (typecategory.ToLower() == "configurationmanager")
                            {
                                if (m_configurationManagers.ContainsKey(secname))
                                {
                                    m_configurationManagers.Remove(secname);
                                }

                                m_configurationManagers.Add(secname, (IConfigurationManager)Activator.CreateInstance(Type.GetType(typestr), parameters));
                            }
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
