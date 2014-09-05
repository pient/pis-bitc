using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Dev.TS
{
    public partial class TaskEdit : BaseFormPage
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

            Task ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Task>();
                    ent.DoUpdate();
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<Task>();

                    ent.DoCreate();
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Task>();
                    ent.DoDelete();
                    return;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Task.Find(id);
                }
                
                this.SetFormData(ent);
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("SysTaskScheduler.TaskType"));
                PageState.Add("CatalogEnum", Enumeration.GetEnumDict("SysTaskScheduler.TaskCatalog"));
                PageState.Add("StatusEnum", Enumeration.GetEnumDict("SysTaskScheduler.TaskStatus"));
            }
        }

        #endregion
    }
}

