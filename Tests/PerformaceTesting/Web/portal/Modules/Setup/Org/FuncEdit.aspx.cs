using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Common;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class FuncEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");

            OrgFunction ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                        ent = this.GetPostedData<OrgFunction>();
                        ent.DoCreate();
                        break;
                    case RequestActionEnum.Update:
                        ent = this.GetMergedData<OrgFunction>();
                        ent.DoUpdate();
                        break;
                    case RequestActionEnum.Delete:
                        ent = this.GetTargetData<OrgFunction>();
                        ent.DoDelete();
                        break;
                }
            }

            if (op != "c")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OrgFunction.Find(id);
                }
            }

            this.SetFormData(ent);

            if (!IsAsyncRequest)
            {
                this.PageState.Add("OrgTypeEnum", OrgType.GetOrgTypeEnum());
            }
        }
    }
}
