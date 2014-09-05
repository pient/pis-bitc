using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Data;
using PIC.Portal.Model;
using System.Threading;

namespace PIC.Portal.Web.UI
{
    // 客户端操作枚举
    [Flags]
    public enum RequestActionEnum
    {
        Create,
        Read,
        Delete,
        Update,
        Query,
        Default,
        Execute,    // 执行
        Custom
    }

    // 页面类型
    public enum PageTypeEnum
    {
        Site,   // 一般页面
        Form,   // 表单页面
        Bpm,   // 业务流程页面
        Simple, // 简单页面
        Custom, // 自定义页面
        Other   // 其他
    }

    /// <summary>
    /// PIC系统页面基类
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        #region 变量

        public const string PageStateKey = "PageState";
        public const string UserInfoKey = "UserInfo";
        public const string SystemInfoKey = "SystemInfo";
        public const string SearchCriterionStateKey = "SearchCriterion";
        public const string PICFormCtrlTag = "form";
        public const string FormDataKey = "frmdata";
        public const string RequestDataKey = "reqdata";
        public const string RequestActionKey = "reqaction";
        public const string PortalDirectory = "portal";

        private SearchCriterion _searchCriterion = new HqlSearchCriterion();

        private Hashtable pageState = new Hashtable();

        private PageStateContainer containter;

        private bool isAsyncPage = false;

        private string requestActionString = String.Empty;

        private RequestActionEnum requestAction = RequestActionEnum.Default;

        private DataRequest formData = null;
        private string formDataString = null;

        private DataRequest requestData = null;

        private bool isCheckAuth = false;

        private bool isCheckLogon = true;

        private bool isPortalContentPage = true;

        private string pageKey = String.Empty;
        private string pageType = String.Empty;

        #endregion

        #region 属性

        /// <summary>
        /// 页面状态，用于前后台传输数据
        /// </summary>
        public virtual Hashtable PageState
        {
            get { return pageState; }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        public bool IsCheckAuth
        {
            get { return isCheckAuth; }
            set { isCheckAuth = value; }
        }

        /// <summary>
        /// 检查用户登录状态
        /// </summary>
        public bool IsCheckLogon
        {
            get { return isCheckLogon; }
            set { isCheckLogon = value; }
        }

        /// <summary>
        /// 是否嵌套在MasterPage中的Portal页面
        /// </summary>
        protected bool IsPortalContentPage
        {
            get { return isPortalContentPage; }
            set { isPortalContentPage = value; }
        }

        /// <summary>
        /// 是否是Portal表单页面
        /// </summary>
        protected bool IsPortalFormPage
        {
            get
            {
                return PageType == PageTypeEnum.Form;
            }
        }

        public PageTypeEnum PageType
        {
            get { return CLRHelper.GetEnum<PageTypeEnum>(this.pageType, PageTypeEnum.Custom); }
            protected set { this.pageType = value.ToString(); }
        }

        /// <summary>
        /// 页面Key
        /// </summary>
        public string PageKey
        {
            get { return pageKey; }
            set { pageKey = value; }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public SearchCriterion SearchCriterion
        {
            get { return _searchCriterion; }
            set { _searchCriterion = value; }
        }

        /// <summary>
        /// 是否异步加载（异步加载在第一次加载页面时不进行任何服务端输出，而由客户端异步获取数据）
        /// </summary>
        public bool IsAsyncPage
        {
            get { return isAsyncPage; }
            set
            {
                isAsyncPage = value;
            }
        }

        /// <summary>
        /// 当前是否异步访问
        /// </summary>
        public bool IsAsyncRequest
        {
            get;
            set;
        }

        /// <summary>
        /// 操作字符
        /// </summary>
        public string RequestActionString
        {
            get { return requestActionString; }
        }

        /// <summary>
        /// 操作枚举(同操作字符结合，这里给出常见操作，以易于使用)
        /// </summary>
        public RequestActionEnum RequestAction
        {
            get { return requestAction; }
        }

        /// <summary>当前应用程序路径，如： /PoliceOnline/ </summary> 
        public string AppPath
        {
            get
            {
                if (Context.Request.ApplicationPath == "/")
                {
                    return "";
                }
                else
                {
                    return Context.Request.ApplicationPath;
                }
            }
        }

        /// <summary>
        /// 表单数据
        /// </summary>
        public DataRequest FormData
        {
            get {
                if (formData == null && !String.IsNullOrEmpty(formDataString))
                {
                    try
                    {
                        Dictionary<string, Object> frmDict = JsonHelper.GetObject<Dictionary<string, Object>>(formDataString);

                        formData = new DataRequest(frmDict);
                    }
#if debug
				catch (Exception ex)
#else
                    catch
#endif
                    {
                    }
                }

                return formData; 
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        public DataRequest RequestData
        {
            get { return requestData; }
        }

        /// <summary>
        /// 当前SessionID
        /// </summary>
        public string UserSID
        {
            get
            {
                return WebPortalService.CurrentUserSID;
            }
        }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get
            {
                return WebPortalService.CurrentUserInfo;
            }
        }

        /// <summary>
        /// 当前系统最基本信息(日期等, 非保密信息)
        /// </summary>
        public SystemInfo SystemInfo
        {
            get
            {
                return WebPortalService.SystemInfo;
            }
        }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserContext UserContext
        {
            get
            {
                return AuthService.CurrentUserContext;
            }
        }

        #endregion

        #region 构造函数

        public BasePage()
        {
            IsAsyncPage = false; // 默认同步页面
            IsCheckAuth = false; // 默认不检查页面权限
            isCheckLogon = true;    // 默认检查登录状态
            PageType = PageTypeEnum.Site;   // 默认Site页面
        }

        #endregion

        #region ASP.NET 事件

        /// <summary>
        /// 初始化之前触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            if (IsPortalContentPage)
            {
                // Set MasterPageFile
                if (String.IsNullOrEmpty(MasterPageFile))
                {
                    switch (PageType)
                    {
                        case PageTypeEnum.Form:
                            MasterPageFile = WebPortalService.VirtualPathProvider.Params.FormMasterPageLocation;
                            break;
                        case PageTypeEnum.Simple:
                            MasterPageFile = WebPortalService.VirtualPathProvider.Params.SimpleMasterPageLocation;
                            break;
                        case PageTypeEnum.Bpm:
                            MasterPageFile = WebPortalService.VirtualPathProvider.Params.BpmMasterPageLocation;
                            break;
                        case PageTypeEnum.Site:
                        default:
                            MasterPageFile = WebPortalService.VirtualPathProvider.Params.SiteMasterPageLocation;
                            break;
                    }
                }
            }

            base.OnPreInit(e);
        }

        /// <summary>
        /// 初始化方法（先于Page_Load和OnLoad执行）
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EnableViewState = false;   // 禁用ViewState

            // 判断是否异步请求
            if (!String.IsNullOrEmpty(Request["AsyncReq"]) && Request["AsyncReq"].Trim().ToUpper() == "TRUE")
            {
                this.IsAsyncRequest = true;

                this.Controls.Clear();  // 如果是异步请求，这里清空所有页面控件(防止输出时，同时输出页面元素)
            }
            else
            {
                if (IsAsyncPage)
                {
                    PageState.Add("AsyncState", true);  // 异步状态
                    Form.Attributes["async"] = "true";
                }
            }

            try
            {
                // for testing - by ray 2014-3-13
                if (IsCheckLogon)
                {
                    AuthService.CheckLogon();
                }
                else if (!String.IsNullOrEmpty(PageKey) && IsCheckAuth)
                {
                    AuthService.CheckAuth(PageKey);
                }
            }
            catch (SecurityException sex)
            {
                // 非异步请求转向
                if (!IsAsyncRequest)
                {
                    // 登出并导航到登录页面
                    WebPortalService.LogoutAndRedirect();
                }
                else
                {
                    throw sex;
                }
            }

            // 解包请求
            this.unPackRequest();

            if (!this.IsAsyncRequest)
            {
                if (this.containter == null && Form != null)
                {
                    Form.Attributes.Add("picctrl", PICFormCtrlTag);
                    Form.Attributes.Add("dsname", FormDataKey);

                    this.containter = new PageStateContainer();
                    this.Form.Controls.Add(containter);
                }
            }
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            
            /*--写事件日志开始--*/

            if (ex is MessageException)
            {
                // MessageException mex = ex as MessageException;

                PageState.Add(WebExceptionMessage.DefaultMessageLabel, new WebExceptionMessage(ex));
            }
            else
            {
                LogService.Log(String.Format("Message:{0}\r\n\r\nStackTrace:{1}", ex.Message, ex.StackTrace), LogTypeEnum.Error);

                /*--写事件日志结束--*/

                if (ex is SecurityException)
                {
                    pageState.Add(WebExceptionMessage.SecurityMessageLabel, new WebExceptionMessage(ex));
                }
                else
                {
                    PageState.Add(WebExceptionMessage.DefaultMessageLabel, new WebExceptionMessage(ex));
                }
            }

            // 这里只对异步的异常做处理，同步的可以自定义处理方式
            if (IsAsyncRequest)
            {
                Server.ClearError();

                Response.Write(this.PackPageState());
                Response.Flush();

                // Response.End可能会导致ThreadAbortException异常
                // Response.End();

                if (HttpContext.Current.ApplicationInstance != null)
                {
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }

        /// <summary>
        /// 渲染前触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_PreRender(object sender, EventArgs e)
        {
            // 异步请求时，直接输出数据
            if (IsAsyncRequest)
            {
                Response.Write(this.PackPageState());
                // Response.Flush();

                try
                {

                    // Response.End可能会导致ThreadAbortException异常
                    Response.End();
                }
                catch (ThreadAbortException)
                {
                    if (HttpContext.Current.ApplicationInstance != null)
                    {
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            else if (containter != null)
            {
                containter.Value = this.PackPageState();
            }
        }

        #endregion 事件

        #region 保护方法

        /// <summary>
        /// 添加表单数据
        /// </summary>
        /// <param name="data"></param>
        protected void SetFormData(object data)
        {
            PageState[FormDataKey] = data;
        }

        /// <summary>
        /// 获取目标数据表单数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetTargetData<T>() where T : EntityBase<T>
        {
            T data = default(T);

            if (FormData != null && FormData.ContainsKey(EntityBase<T>.PrimaryKeyName))
            {
                object primaryKeyValue = FormData[EntityBase<T>.PrimaryKeyName];

                if (primaryKeyValue != null)
                {
                    data = EntityBase<T>.AutoFind(primaryKeyValue);
                }
            }

            return data;
        }

        /// <summary>
        /// 融合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected T GetMergedData<T>() where T : EntityBase<T>, new()
        {
            T merged = default(T);

            if (this.FormData != null)
            {
                T target = this.GetTargetData<T>();

                T postedData = GetPostedData<T>();

                if (postedData != null)
                {
                    if (target != null)
                    {
                        merged = DataHelper.MergeData(target, postedData, this.FormData.Keys);
                    }
                    else
                    {
                        merged = postedData;
                    }
                }
                else if(target != null)
                {
                    merged = target;
                }
            }

            return merged;
        }


        /// <summary>
        /// 获取提交的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetPostedData<T>() where T : EntityBase<T>, new()
        {
            T postedData = default(T);

            if (this.FormData != null)
            {
                postedData = JsonHelper.GetObject<T>(formDataString);
            }

            return postedData;
        }

        /// <summary>
        /// 设置Web消息
        /// </summary>
        /// <param name="msg"></param>
        protected void SetMessage(string msg)
        {
            PageState[WebMessage.DefaultMessageLabel] = new WebMessage(msg);
        }

        /// <summary>
        /// 设置Web消息
        /// </summary>
        /// <param name="msg"></param>
        protected void SetMessage(string title, string msg)
        {
            PageState[WebMessage.DefaultMessageLabel] = new WebMessage(title, msg);
        }

        /// <summary>
        /// 打包页面信息，并将其绑定到隐藏的服务器端控件上
        /// </summary>
        protected string PackPageState()
        {
            if (!IsAsyncRequest)
            {
                if (!this.pageState.Contains(UserInfoKey))
                {
                    this.pageState.Add(UserInfoKey, UserInfo);
                }

                if (!this.pageState.Contains(SystemInfoKey))
                {
                    this.pageState.Add(SystemInfoKey, SystemInfo);
                }
            }

            string json = JsonHelper.GetJsonString(this.pageState);

            return json;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 拆包请求的数据，初始化相关变量
        /// </summary>
        private void unPackRequest()
        {
            this.SetRequestActionString(Context.Request[RequestActionKey]);

            if (!String.IsNullOrEmpty(Request[PageStateKey]))
            {
                try
                {
                    this.pageState = JsonHelper.GetObject<Hashtable>(Request[PageStateKey]);
                }
                catch { }
            }

            if (!String.IsNullOrEmpty(Request[SearchCriterionStateKey]))
            {
                try
                {
                    this.SearchCriterion = JsonHelper.GetObject<HqlSearchCriterion>(Request[SearchCriterionStateKey]);
                    this.SearchCriterion.FormatSearch();
                }
#if debug
				catch (Exception ex)
#else
                catch
#endif
                { }
            }

            // 打包请求数据
            Dictionary<string, Object> reqDict = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(Request[RequestDataKey]))
            {
                try
                {
                    reqDict = JsonHelper.GetObject<Dictionary<string, Object>>(Request[RequestDataKey]);
                }
                catch
                {
                }
            }

            NameValueCollection nvc = Request.QueryString;
            if (nvc != null && nvc.Count > 0)
            {
                string keyUpper = String.Empty;
                foreach (string key in nvc.Keys)
                {
                    if (key != FormDataKey && key != PageStateKey && key != SearchCriterionStateKey && key != RequestActionKey && key != RequestDataKey)
                    {
                        if (!String.IsNullOrEmpty(key) && !reqDict.ContainsKey(key))
                        {
                            reqDict.Add(key, nvc[key]);
                        }
                    }
                }
            }

            requestData = new DataRequest(reqDict);

            if (!String.IsNullOrEmpty(Request[FormDataKey]))
            {
                formDataString = HttpUtility.UrlDecode(Request[FormDataKey]);
            }
        }

        /// <summary>
        /// 设置请求操作字符
        /// </summary>
        private void SetRequestActionString(string actionstr)
        {
            if (String.IsNullOrEmpty(Context.Request[RequestActionKey]))
            {
                this.requestAction = RequestActionEnum.Default;
                requestActionString = this.requestAction.ToString();
            }
            else
            {
                try
                {
                    this.requestAction = (RequestActionEnum)Enum.Parse(typeof(RequestActionEnum), actionstr, true);
                }
                catch
                {
                    this.requestAction = RequestActionEnum.Custom;
                }

                requestActionString = actionstr;
            }
        }

        #endregion
    }
}
