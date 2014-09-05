using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Hosting;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using NServiceBus;
using PIC.Data;
using PIC.Common;
using PIC.Common.Service;
using PIC.Common.Authentication;
using PIC.Portal.Model;
using PIC.Portal.Web;
using System.IO;

namespace PIC.Portal
{
    /// <summary>
    /// 处理Portal服务相关信息(设计有待优化，目前只能用于IIS的Web Portal)
    /// </summary>
    public class PortalService
    {
        public static object uclslocker = new object(); // 添加一个对象作为UserContextList的锁

        #region Consts

        public const string PORTAL_SERVICE_KEY = "PIC_PORTAL_SERVICE";
        public const string PORTAL_RES_FOLDER_KEY = "ResourceFolder";
        public const string PORTAL_TEMP_DB_CONNSTR_KEY = "TempDbConnectionString";
        public const string PORTAL_SERVICE_PROVIDER_KEY = "PortalServiceProvider";
        public const string PORTAL_VIRTUAL_PATH_PROVIDER_KEY = "PortalVirtualPathProvider";

        #endregion

        #region 成员

        public event EventHandler ServiceInitialize;

        protected bool isInitialized = false;

        /// <summary>
        /// 标识PortalService实例
        /// </summary>
        public string InstanceID { get; private set; }

        protected IPortalServiceProvider _provider = null;

        protected SystemContext systemContext;

        protected IServiceBus serviceBus;

        protected string portalDirectory = null;

        #endregion

        #region 属性

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        /// <summary>
        /// Service 提供者，供其它服务使用，分担PortalService职则
        /// </summary>
        internal static IPortalServiceProvider Provider
        {
            get
            {
                if (!ps.isInitialized)
                {
                    return null;
                }
                else
                {
                    return ps._provider;
                }
            }
        }

        /// <summary>
        /// 系统上下文
        /// </summary>
        internal static SystemContext SystemContext
        {
            get
            {
                if (ps.systemContext == null)
                {
                    ps.systemContext = new SystemContext();
                }

                return ps.systemContext;
            }
        }

        public static IServiceBus ServiceBus
        {
            get
            {
                if (ps.serviceBus == null)
                {
                    ps.serviceBus = ps._provider.RetrieveServiceBus();
                }
                return ps.serviceBus;
            }
        }

        /// <summary>
        /// 系统信息
        /// </summary>
        public static SystemInfo SystemInfo
        {
            get
            {
                return SystemContext.SystemInfo;
            }
        }

        /// <summary>
        /// 当前SessionID
        /// </summary>
        public static string CurrentUserSID
        {
            get
            {
                return AuthService.CurrentUserSID;
            }
        }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static UserInfo CurrentUserInfo
        {
            get
            {
                return AuthService.CurrentUserInfo;
            }
        }

        /// <summary>
        /// 获取当前用户上下文
        /// </summary>
        public static UserContext CurrentUserContext
        {
            get
            {
                return AuthService.CurrentUserContext;
            }
        }

        /// <summary>
        /// 刷新系统模块，组，人员等
        /// </summary>
        public static void RefreshSystemContext()
        {
            if (SystemContext != null)
            {
                SystemContext.RefreshModules();

                SystemContext.RefreshOrg();
            }
        }

        /// <summary>
        /// 系统临时文件夹
        /// </summary>
        public static string TempFolder
        {
            get { return Path.Combine(ResourceFolder, @".\Temp"); }
        }

        /// <summary>
        /// 资源文件夹
        /// </summary>
        public static string ResourceFolder
        {
            get { return PICConfigurationManager.AppSettings[PORTAL_RES_FOLDER_KEY]; }
        }

        #endregion

        #region 构造函数

        protected readonly static PortalService ps = new PortalService();

        protected PortalService()
        {
            this.InstanceID = Guid.NewGuid().ToString();

            // 初始化门户虚拟目录提供者
            PortalVirtualPathProvider pvpp = GetPortalVirtualPathProvider();

            if (pvpp != null && HostingEnvironment.IsHosted)
            {
                RegisterVirtualPathProvider(pvpp);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取默认PortalService提供者
        /// </summary>
        /// <returns></returns>
        public static IPortalServiceProvider GetDefaultProvider()
        {
            string typeName = PICConfigurationManager.AppSettings[PORTAL_SERVICE_PROVIDER_KEY];

            IPortalServiceProvider psp = (IPortalServiceProvider)Activator.CreateInstance(Type.GetType(typeName));

            return psp;
        }

        #region 初始化PortalService

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool Initialize()
        {
            IPortalServiceProvider provider = GetDefaultProvider();

            return Initialize(provider, null);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="exAssemblyNames"></param>
        /// <param name="exceptTypes"></param>
        /// <returns></returns>
        public static bool Initialize(string[] exAssemblyNames, params Type[] exceptTypes)
        {
            IPortalServiceProvider provider = GetDefaultProvider();

            return Initialize(provider, exAssemblyNames, exceptTypes);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="exAssemblyNames"></param>
        /// <param name="exceptTypes"></param>
        /// <returns></returns>
        public static bool Initialize(string assemblyNames, string[] exAssemblyNames, params Type[] exceptTypes)
        {
            IPortalServiceProvider provider = GetDefaultProvider();

            return Initialize(provider, assemblyNames, exAssemblyNames, exceptTypes);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        public static bool Initialize(Assembly[] assemblies, Assembly[] exAssemblies, params Type[] exceptTypes)
        {
            IPortalServiceProvider provider = GetDefaultProvider();

            return Initialize(provider, assemblies, exAssemblies, exceptTypes);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool Initialize(IPortalServiceProvider provider)
        {
            return Initialize(provider, null);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="exAssemblyNames"></param>
        /// <param name="exceptTypes"></param>
        /// <returns></returns>
        public static bool Initialize(IPortalServiceProvider provider, string[] exAssemblyNames, params Type[] exceptTypes)
        {
            Assembly[] assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
            Assembly[] exAssemblies = CLRHelper.GetAssemblysByNames(exAssemblyNames);

            return Initialize(provider, assemblies, exAssemblies, exceptTypes);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="exAssemblyNames"></param>
        /// <param name="exceptTypes"></param>
        /// <returns></returns>
        public static bool Initialize(IPortalServiceProvider provider, string assemblyNames, string[] exAssemblyNames, params Type[] exceptTypes)
        {
            Assembly[] assemblies = CLRHelper.GetAssemblysByNames(assemblyNames);
            Assembly[] exAssemblies = CLRHelper.GetAssemblysByNames(exAssemblyNames);

            return Initialize(provider, assemblies, exAssemblies, exceptTypes);
        }

        /// <summary>
        /// 初始化PortalService
        /// </summary>
        public static bool Initialize(IPortalServiceProvider provider, Assembly[] assemblies, Assembly[] exAssemblies, params Type[] exceptTypes)
        {
            if (ps != null && ps.isInitialized)
            {
                throw new Exception("PortalService已初始化");
            }

            // 初始化Entity
            EntityManager.InitializeEntity(assemblies, exAssemblies, exceptTypes);

            // 设置Portal入口服务提供者
            ps._provider = provider;

            // 初始化服务上下文
            ps.systemContext = new SystemContext();

            // 获取NServiceBus
            ps.serviceBus = provider.RetrieveServiceBus();

            // 刷新模块
            RefreshSystemContext();

            if (ps.ServiceInitialize != null)
            {
                ps.ServiceInitialize(ps, new EventArgs());
            }

            ps.isInitialized = true;

            return ps.isInitialized;
        }

        #endregion

        /// <summary>
        /// 获取蓄力目录提供者
        /// </summary>
        public static PortalVirtualPathProvider VirtualPathProvider
        {
            get
            {
                return HostingEnvironment.VirtualPathProvider as PortalVirtualPathProvider;
            }
        }

        public static PortalVirtualPathProvider GetPortalVirtualPathProvider()
        {
            PortalVirtualPathProvider pvpp = null;

            if (PICConfigurationManager.AppSettings.ContainsKey(PORTAL_VIRTUAL_PATH_PROVIDER_KEY))
            {
                string typeName = PICConfigurationManager.AppSettings[PORTAL_VIRTUAL_PATH_PROVIDER_KEY];
                pvpp = CLRHelper.CreateInstance<PortalVirtualPathProvider>(typeName);
            }

            return pvpp;
        }

        /// <summary>
        /// 注册虚拟目录提供者
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pvpp"></param>
        public static void RegisterVirtualPathProvider(PortalVirtualPathProvider pvpp)
        {
            HostingEnvironment.RegisterVirtualPathProvider(pvpp);
        }

        /// <summary>
        /// 新建操作请求
        /// </summary>
        /// <returns></returns>
        public static OperationRequest NewOperationRequest()
        {
            var op = new OperationRequest(CurrentUserSID);

            return op;
        }

        #endregion
    }

    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemContext
    {
        #region 私有变量

        private Hashtable _tag;

        private SystemInfo _systemInfo;

        private ReadOnlyCollection<Model.Application> applications;
        private ReadOnlyCollection<Model.Module> modules;
        private ReadOnlyCollection<Auth> auths;
        private ReadOnlyCollection<OrgGroup> groups;
        private ReadOnlyCollection<OrgRole> roles;

        #endregion

        #region 属性

        /// <summary>
        /// 系统信息
        /// </summary>
        public SystemInfo SystemInfo
        {
            get
            {
                return this._systemInfo;
            }
        }

        /// <summary>
        /// 获取系统所有应用
        /// </summary>
        public IList<Application> Applications
        {
            get
            {
                if (applications == null)
                {
                    RefreshModules();
                }

                return applications;
            }
        }

        /// <summary>
        /// 获取系统所有模块
        /// </summary>
        public IList<Model.Module> Modules
        {
            get
            {
                if (modules == null)
                {
                    RefreshModules();
                }

                return modules;
            }
        }

        /// <summary>
        /// 获取系统所有权限
        /// </summary>
        public IList<Auth> Auths
        {
            get
            {
                if (auths == null)
                {
                    RefreshModules();
                }

                return auths;
            }
        }

        /// <summary>
        /// 获取系统所有角色
        /// </summary>
        public IList<OrgRole> Roles
        {
            get
            {
                if (roles == null)
                {
                    RefreshModules();
                }

                return roles;
            }
        }

        /// <summary>
        /// 获取系统所有组
        /// </summary>
        public IList<OrgGroup> Groups
        {
            get
            {
                if (groups == null)
                {
                    RefreshModules();
                }

                return groups;
            }
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public Hashtable ExtData
        {
            get
            {
                if (_tag == null)
                {
                    _tag = new Hashtable();
                }

                return _tag;
            }
        }

        #endregion

        #region 构造函数

        public SystemContext()
        {
            _systemInfo = new SystemInfo();
        }

        #endregion

        #region 公共函数

        /// <summary>
        /// 刷新应用模块
        /// </summary>
        public void RefreshModules()
        {
            applications = new ReadOnlyCollection<Application>(Application.FindAll(Expression.Eq(Model.Application.Prop_Status, 1)));
            modules = new ReadOnlyCollection<Model.Module>(Model.Module.FindAll(Expression.Eq(Model.Module.Prop_Status, 1)));
            auths = new ReadOnlyCollection<Auth>(Auth.FindAll());
        }

        /// <summary>
        /// 刷新组织结构
        /// </summary>
        public void RefreshOrg()
        {
            roles = new ReadOnlyCollection<OrgRole>(OrgRole.FindAll());
            groups = new ReadOnlyCollection<OrgGroup>(OrgGroup.FindAll());
        }

        #endregion
    }
}
