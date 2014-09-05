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

namespace PIC.Portal.Web.Modules.Setup.Auth
{
    public partial class AuthEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id
        string pt = String.Empty; // 父模块类型

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");

            Model.Auth ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Update:
                        ent = this.GetMergedData<Model.Auth>();
                        ent.DoUpdate();
                        break;
                    default:
                        if (RequestActionString == "createsub")
                        {
                            ent = this.GetPostedData<Model.Auth>();

                            ent.CreateAsChild(id);
                        }
                        else if (RequestActionString == "createsib")
                        {
                            ent = this.GetPostedData<Model.Auth>();

                            ent.CreateAsSibling(id);
                        }
                        break;
                }
            }

            if (!String.IsNullOrEmpty(id) && RequestActionString != "createsib")
            {
                ent = Model.Auth.Find(id);
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    switch (op)
                    {
                        case "createsub":
                        case "createsib":
                            ent = new Model.Auth()
                            {
                                Code = ent.Code,
                                Type = ent.Type,
                                SortIndex = (ent.SortIndex ?? 0) + 100
                            };

                            if (op == "createsub")
                            {
                                ent.SortIndex = 100;
                            }
                            break;
                    }
                }

                DataEnum de = AuthType.GetAuthTypeEnum();
                this.PageState.Add("AuthTypeEnum", de);
            }

            this.SetFormData(ent);
        }
    }
}
