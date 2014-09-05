using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Script.Serialization;

using NHibernate.Criterion;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;
using PIC.Portal.Workflow;


namespace PIC.Portal.Web.Modules.Common.Bpm
{
    public partial class FlowPreview : BaseFlowPage
    {
        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            var did = RequestData.Get<string>("did");
            var dcode = RequestData.Get<string>("dcode");

            if (!IsAsyncRequest)
            {
                WfDefine define = null;

                if (!String.IsNullOrEmpty(did))
                {
                    define = WfDefine.Find(did);
                }
                else if (!String.IsNullOrEmpty(dcode))
                {
                    define = WfDefine.Get(dcode);
                }

                if (define != null)
                {
                    var wfConfig = define.GetConfig();
                    var formDefine = define.FormDefine;
                    var basicInfo = define.GetInitBasicInfo();

                    PageState.Add("FlowData", new
                    {
                        Define = define,
                        Basic = basicInfo,
                        Config = wfConfig,
                        FormDefine = formDefine
                    });
                }
            }
        }

        #endregion
    }
}
