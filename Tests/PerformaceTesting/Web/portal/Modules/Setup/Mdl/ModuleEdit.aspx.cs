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

namespace PIC.Portal.Web.Modules.Setup.Mdl
{
    public partial class ModuleEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 父模块类型

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");

            Module ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Module>();
                    ent.Update();
                    break;
                default:
                    if (RequestActionString == "createsub")
                    {
                        ent = this.GetPostedData<Module>();

                        ent.CreateAsChild(id);
                    }
                    else if (RequestActionString == "createsib")
                    {
                        ent = this.GetPostedData<Module>();

                        ent.CreateAsSibling(id);
                    }
                    break;
            }

            if (!String.IsNullOrEmpty(id) && RequestActionString != "createsib")
            {
                ent = Module.Find(id);
            }

            if (!IsAsyncRequest)
            {
                if (ent != null)
                {
                    switch (op)
                    {
                        case "createsub":
                        case "createsib":
                            ent = new Module()
                            {
                                Code = ent.Code,
                                Type = ent.Type,
                                Status = ent.Status,
                                SortIndex = (ent.SortIndex ?? 0) + 100,
                                Icon = ent.Icon
                            };

                            if (op == "createsub")
                            {
                                ent.SortIndex = 100;
                            }
                            break;
                    }
                }

                DataEnum de = ModuleType.GetModuleTypeEnum();
                this.PageState.Add("MdlTypeEnum", de);
            }

            this.SetFormData(ent);
        }
    }
}
