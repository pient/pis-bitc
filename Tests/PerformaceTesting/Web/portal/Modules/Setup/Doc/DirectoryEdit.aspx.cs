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
    public partial class DirectoryEdit : BaseFormPage
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
            id = RequestData.Get("id").ToGuid();
            type = RequestData.Get<string>("type");

            DocDirectory ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<DocDirectory>();
                    ent.DoUpdate();
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<DocDirectory>();

                    if (id.IsNullOrEmpty())
                    {
                        ent.CreateAsRoot();
                    }
                    else
                    {
                        ent.CreateAsSibling(DocDirectory.Find(id));
                    }
                    break;
                default:
                    if (RequestActionString == "createsub")
                    {
                        ent = this.GetPostedData<DocDirectory>();

                        ent.CreateAsChild(id);
                    }
                    else if (RequestActionString == "createsib")
                    {
                        ent = this.GetPostedData<DocDirectory>();

                        ent.CreateAsSibling(id);
                    }
                    break;
            }

            if (RequestActionString != "createsib" && !id.IsNullOrEmpty())
            {
                ent = DocDirectory.Find(id);
            }

            if (!IsAsyncRequest)
            {
                if (!id.IsNullOrEmpty())
                {
                    switch (op)
                    {
                        case "createsub":
                        case "createsib":
                            ent = new DocDirectory()
                            {
                                Code = ent.Code,
                                SortIndex = (ent.SortIndex ?? 0) + 100
                            };

                            if (op == "createsub")
                            {
                                ent.SortIndex = 100;
                            }
                            break;
                    }
                }
            }

            this.SetFormData(ent);
        }

        #endregion
    }
}

