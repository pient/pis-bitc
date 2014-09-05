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
    public partial class ParamEdit : BaseFormPage
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

            Model.Parameter ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Model.Parameter>();
                    ent.DoUpdate();
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<Model.Parameter>();

                    ent.CreaterID = UserInfo.UserID;
                    ent.CreaterName = UserInfo.Name;
                    ent.DoCreate();
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Model.Parameter>();
                    ent.DoDelete();
                    return;
            }

            if (op != "c")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Model.Parameter.Find(id);
                }
                
                this.SetFormData(ent);
            }
        }

        #endregion
    }
}

