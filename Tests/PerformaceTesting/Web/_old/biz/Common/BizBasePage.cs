using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Biz.Web
{
    public class BizBasePage : BasePage
    {
        #region 私有变量

        public static object uclslocker = new object(); // 添加一个对象作为UserContext的锁

        // EPC应用编码
        public const string BIZ_APP_CODE = "BIZ";

        public const string BizIDKey = "BizID";
        public const string BizContextKey = "BizContext";

        public BizContext bizContext;

        #endregion

        #region 构造函数

        public BizBasePage()
        {
        }

        #endregion 构造函数

        #region 属性

        /// <summary>
        /// 项目上下文
        /// </summary>
        protected BizContext BizContext
        {
            get
            {
                return bizContext;
            }
        }

        #endregion

        #region ASP.NET 事件

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (UserContext != null && !UserContext.ExtData.ContainsKey(BizContextKey))
            {
                if (UserInfo.ExtData.ContainsKey(BizIDKey))
                {
                    string bizID = UserInfo.ExtData[BizIDKey];
                    bizContext = new BizContext(bizID);
                }
                else
                {
                    bizContext = new BizContext();
                }

                try
                {
                    lock (uclslocker)
                    {
                        UserContext.ExtData.Add(BizContextKey, bizContext);
                    }
                }
                catch(Exception) { }
            }
        }

        protected override void Page_PreRender(object sender, EventArgs e)
        {
            if (BizContext != null)
            {
            }

            base.Page_PreRender(sender, e);
        }

        #endregion ASP.NET 事件

        #region 方法

        /// <summary>
        /// 重新加载当前项目上下文
        /// </summary>
        protected void ReloadBiz()
        {
            if (bizContext != null)
            {

            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 重新加载考核上下文
        /// </summary>
        /// <param name="bizID"></param>
        public static void ReloadBiz(string bizID)
        {
            // 切换考核上下文
        }

        #endregion
    }

    /// <summary>
    /// 项目上下文
    /// </summary>
    public class BizContext
    {
        #region 成员

        private Hashtable _tag;

        #endregion

        #region 属性

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

        public BizContext()
        {
        }

        public BizContext(string prjid)
        {

        }

        #endregion
    }
}