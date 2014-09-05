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
    public partial class RoleAuthEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;
        string rid = String.Empty;
        string aid = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");
            rid = RequestData.Get<string>("rid");
            aid = RequestData.Get<string>("aid");

            OrgRoleAuth ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                    case RequestActionEnum.Update:
                        ent = this.GetPostedData<OrgRoleAuth>();
                        ent.DoSave();
                        break;
                    case RequestActionEnum.Delete:
                        ent = this.GetTargetData<OrgRoleAuth>();
                        ent.DoDelete();
                        break;
                }
            }

            if (!String.IsNullOrEmpty(id))
            {
                var dicts = DataHelper.QueryDictList("SELECT TOP 1 * FROM vw_OrgRoleAuth WHERE RoleAuthID = '" + id + "'");

                if (dicts.Count > 0)
                {
                    this.SetFormData(dicts.FirstOrDefault());
                }
                else
                {
                    this.SetFormData(ent);
                }
            }
            else if (!String.IsNullOrEmpty(rid) && !String.IsNullOrEmpty(aid))
            {
                string sqlString = String.Format("SELECT TOP 1 * FROM vw_OrgRoleAuth WHERE RoleID = '{0}' AND AuthID = '{1}'", rid, aid);
                var dicts = DataHelper.QueryDictList(sqlString);

                if (dicts.Count > 0)
                {
                    this.SetFormData(dicts.FirstOrDefault());
                }
                else
                {
                    var role = OrgRole.Find(rid);
                    var auth = Model.Auth.Find(aid);

                    this.SetFormData(new
                    {
                        RoleID = role.RoleID,
                        RoleName = role.Name,
                        AuthID = auth.AuthID,
                        AuthName = auth.Name
                    });
                }
            }
            else if (ent != null)
            {
                this.SetFormData(ent);
            }
        }
    }
}
