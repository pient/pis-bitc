using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Dev
{
    public partial class TmplCfgEdit : BaseFormPage
    {
        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestActionString)
            {
                case "geteditortmpl":
                    this.GetTemplate();
                    break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("SNIncTypeEnum", Enumeration.GetEnumDict("SysTemplate.IncreaseType"));
            }
        }

        #endregion

        #region 支持方法

        /// <summary>
        /// 获取模版信息
        /// </summary>
        public void GetTemplate()
        {
            var code = RequestData.Get<string>("code");

            var ent = Model.Template.Get(code);

            this.PageState.Add("TmplData", ent);
        }

        #endregion
    }
}

