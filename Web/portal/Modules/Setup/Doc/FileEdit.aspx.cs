using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Doc.Model;


namespace PIC.Portal.Web.Modules.Setup.Doc
{
    public partial class FileEdit : BaseFormPage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        Guid? id = null;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.GetGuid("id");
            type = RequestData.Get<string>("type");

            DocFile ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<DocFile>();
                    ent.DoUpdate();
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<DocFile>();

                    ent.DoCreate();
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<DocFile>();
                    ent.DoDelete();
                    return;
            }

            if (op != "c")
            {
                if (!id.IsNullOrEmpty())
                {
                    ent = DocFile.Find(id);
                }
                
                this.SetFormData(ent);
            }
        }

        #endregion
    }
}

