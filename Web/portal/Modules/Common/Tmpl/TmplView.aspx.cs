using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Common.Tmpl
{
    public partial class TmplView : BasePage
    {
        #region 成员变量

        private string tid, tcode;
        private EasyDictionary ctxParams;

        Model.Template tmpl = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            tid = RequestData.Get<string>("tid");
            tcode = RequestData.Get<string>("tcode");
            ctxParams = RequestData.Get<EasyDictionary>("ctxparams");

            if (!String.IsNullOrEmpty(tid))
            {
                tmpl = Model.Template.Find(tid);
            }
            else if (!String.IsNullOrEmpty(tcode))
            {
                tmpl = Model.Template.Get(tcode);
            }

            var renderStr = tmpl.RenderString(ctxParams);

            PageState.Add("Template", tmpl);
            PageState.Add("RenderStr", renderStr);
        }

        #endregion
    }
}

