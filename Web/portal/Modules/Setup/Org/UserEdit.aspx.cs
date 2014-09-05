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
    public partial class UserEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");

            OrgUser ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                        ent = this.GetPostedData<OrgUser>();
                        ent.DoCreate();
                        break;
                    case RequestActionEnum.Update:
                        ent = this.GetMergedData<OrgUser>();
                        ent.DoUpdate();
                        break;
                    case RequestActionEnum.Delete:
                        ent = this.GetTargetData<OrgUser>();
                        ent.DoDelete();
                        break;
                }
            }

            if (op != "c")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OrgUser.Find(id);
                }
            }

            this.SetFormData(ent);
        }
    }
}
