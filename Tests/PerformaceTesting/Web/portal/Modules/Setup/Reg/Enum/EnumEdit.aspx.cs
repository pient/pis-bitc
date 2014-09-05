using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Setup.Reg
{
    public partial class EnumEdit : BaseFormPage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");

            Enumeration ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Enumeration>();
                    ent.DoUpdate();
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<Enumeration>();

                    ent.CreaterID = UserInfo.UserID;
                    ent.CreaterName = UserInfo.Name;

                    if (String.IsNullOrEmpty(id))
                    {
                        ent.CreateAsRoot();
                    }
                    else
                    {
                        ent.CreateAsSibling(id);
                    }
                    break;
                default:
                    if (RequestActionString == "createsub")
                    {
                        ent = this.GetPostedData<Enumeration>();
                        ent.CreaterID = UserInfo.UserID;
                        ent.CreaterName = UserInfo.Name;

                        ent.CreateAsChild(id);
                    }
                    else if (RequestActionString == "createsib")
                    {
                        ent = this.GetPostedData<Enumeration>();
                        ent.CreaterID = UserInfo.UserID;
                        ent.CreaterName = UserInfo.Name;

                        ent.CreateAsSibling(id);
                    }
                    break;
            }

            if (RequestActionString != "createsib" && !String.IsNullOrEmpty(id))
            {
                ent = Enumeration.Find(id);
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    switch (op)
                    {
                        case "createsub":
                        case "createsib":
                            ent = new Enumeration()
                            {
                                Code = ent.Code
                            };
                            break;
                    }
                }
            }

            this.SetFormData(ent);
        }

        #endregion
    }
}

