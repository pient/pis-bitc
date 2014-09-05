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
    public class BizSimplePage : BaseSimplePage
    {
        #region 私有变量

        #endregion

        #region 构造函数

        public BizSimplePage()
        {
        }

        #endregion 构造函数

        #region 属性

        #endregion

        #region ASP.NET 事件

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Page_PreRender(object sender, EventArgs e)
        {
            base.Page_PreRender(sender, e);
        }

        #endregion ASP.NET 事件
    }
}