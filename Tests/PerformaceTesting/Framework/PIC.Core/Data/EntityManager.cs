using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using PIC.Common;
using PIC.Common.Configuration;

namespace PIC.Data
{
    public class EntityManager
    {
        /// <summary>
        /// 初始化实体
        /// </summary>
        public static void InitializeEntity(string assemblyName)
        {
            Assembly asmModel = Assembly.Load(assemblyName);

            IConfigurationSource configSource = RetrieveActiveRecordConfigSource();

            ActiveRecordStarter.Initialize(asmModel, configSource);
        }

        public static void InitializeEntity(string[] assemblyNames, params Type[] types)
        {
            Assembly[] assms = CLRHelper.GetAssemblysByNames(assemblyNames);

            IConfigurationSource configSource = RetrieveActiveRecordConfigSource();

            ActiveRecordStarter.Initialize(assms, configSource, types);
        }

        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="assemblyNames">主程序集（继承于ActiveRecordBase或ActiveRecordBase<>）</param>
        /// <param name="exAssemblyNames">扩展程序集（用于多数据库连接）</param>
        /// <param name="exceptTypes">排除类型（不进行初始化的类型）</param>
        public static void InitializeEntity(string[] assemblyNames, string[] exAssemblyNames, params Type[] exceptTypes)
        {
            Assembly[] assms = CLRHelper.GetAssemblysByNames(assemblyNames);
            Assembly[] exAssms = CLRHelper.GetAssemblysByNames(exAssemblyNames);

            InitializeEntity(assms, exAssms, exceptTypes);
        }

        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="assemblies">主程序集（继承于ActiveRecordBase或ActiveRecordBase<>）</param>
        /// <param name="exAssembliesm">扩展程序集（用于多数据库连接）</param>
        /// <param name="exceptTypes">排除类型（不进行初始化的类型）</param>
        public static void InitializeEntity(Assembly[] assemblies, Assembly[] exAssemblies, params Type[] exceptTypes)
        {
            IList<Assembly> assms = new List<Assembly>(assemblies);
            IList<Type> exTypes = new List<Type>();

            if (exAssemblies != null && exAssemblies.Length > 0)
            {
                foreach (Assembly tassms in exAssemblies)
                {
                    assms.Add(tassms);

                    /*Type[] ttypes = tassms.GetTypes();

                    foreach (Type ttype in ttypes)
                    {
                        if (!exceptTypes.Contains(ttype) && ttype.IsDefined(typeof(ActiveRecordAttribute), false))
                        {
                            exTypes.Add(ttype);
                        }
                    }*/
                }
            }

            IConfigurationSource configSource = RetrieveActiveRecordConfigSource();

            ActiveRecordStarter.Initialize(assms.ToArray(), configSource, exTypes.ToArray());
        }

        private static IConfigurationSource RetrieveActiveRecordConfigSource()
        {
            // IConfigurationSource configSource = ActiveRecordSectionHandler.Instance;

            XmlNode xmlNode = PICConfigurationManager.ThirdPartiesConfiguration.GetConfig("activerecord");
            TextReader txtReader = new StringReader(xmlNode.OuterXml);

            IConfigurationSource configSource = new XmlConfigurationSource(txtReader);

            return configSource;
        }
    }
}
