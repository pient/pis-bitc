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
using PIC.Biz.Model;

namespace PIC.Biz.Web
{
    public partial class HR_EmployeeContractEdit : BizFlowFormPage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string eid = String.Empty;
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            eid = RequestData.Get<string>("eid");
            type = RequestData.Get<string>("type");

            HR_EmployeeContract ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<HR_EmployeeContract>();
                    ent.DoUpdate();
                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<HR_EmployeeContract>();

                    ent.DoCreate();
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<HR_EmployeeContract>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = HR_EmployeeContract.Find(id);
                }
                else if (!String.IsNullOrEmpty(eid))
                {
                    ent = HR_EmployeeContract.FindFirstByProperties(HR_EmployeeContract.Prop_EmployeeId, eid);
                }
                
                this.SetFormData(ent);
            }
        }

        #endregion
    }
}

