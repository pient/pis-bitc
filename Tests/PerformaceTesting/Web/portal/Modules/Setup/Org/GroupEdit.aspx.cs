using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Model;

using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class GroupEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id
        string pt = String.Empty; // 父模块类型

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");
            pt = RequestData.Get<string>("pt");

            OrgGroup ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Update:
                        ent = this.GetMergedData<OrgGroup>();
                        ent.DoUpdate();
                        break;
                    default:
                        if (RequestActionString == "createsub")
                        {
                            ent = this.GetPostedData<OrgGroup>();

                            ent.CreateAsChild(id);
                        }
                        else if (RequestActionString == "createsib")
                        {
                            ent = this.GetPostedData<OrgGroup>();

                            ent.CreateAsSibling(id);
                        }
                        break;
                }
            }

            if (!String.IsNullOrEmpty(id) && RequestActionString != "createsib")
            {
                ent = OrgGroup.Find(id);
            }

            if (!IsAsyncRequest)
            {
                if (ent != null)
                {
                    switch (op)
                    {
                        case "createsub":
                        case "createsib":
                            ent = new OrgGroup()
                            {
                                Code = ent.Code,
                                Type = ent.Type,
                                Status = ent.Status,
                                SortIndex = (ent.SortIndex ?? 0) + 100
                            };

                            if (op == "createsub")
                            {
                                ent.SortIndex = 100;
                            }
                            break;
                    }
                }

                DataEnum de = OrgType.GetOrgTypeEnum();
                this.PageState.Add("OrgTypeEnum", de);
            }

            this.SetFormData(ent);
        }
    }
}
