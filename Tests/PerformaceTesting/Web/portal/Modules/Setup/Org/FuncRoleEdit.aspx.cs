using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Common;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class FuncRoleEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id
        string fid = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");
            fid = RequestData.Get<string>("fid");

            OrgFunctionRole ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                        this.DoCreate();    // 可能需要批量创建
                        break;
                    case RequestActionEnum.Update:
                        ent = this.GetMergedData<OrgFunctionRole>();
                        ent.DoUpdate();
                        break;
                    case RequestActionEnum.Delete:
                        ent = this.GetTargetData<OrgFunctionRole>();
                        ent.DoDelete();
                        break;
                }
            }

            if (op != "c")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    var dicts = DataHelper.QueryDictList("SELECT TOP 1 * FROM vw_OrgFunctionRole WHERE FunctionRoleID = '" + id + "'");

                    this.SetFormData(dicts.FirstOrDefault());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(fid))
                {
                    var func = OrgFunction.Find(fid);

                    if (func != null)
                    {
                        this.SetFormData(new { FunctionID = func.FunctionID, FuncName = func.Name });
                    }
                }
            }
        }

        #region 支持方法

        /// <summary>
        /// 包含批量创建
        /// </summary>
        private void DoCreate()
        {
            var ent = this.GetPostedData<OrgFunctionRole>();

            if (!String.IsNullOrEmpty(ent.RoleID) && !String.IsNullOrEmpty(ent.FunctionID))
            {
                var roleIDs = ent.RoleID.Split(',');    // 批量创建

                var func = OrgFunction.Find(ent.FunctionID);

                func.AddRoleByIDs(ent, roleIDs);
            }
        }

        #endregion
    }
}
