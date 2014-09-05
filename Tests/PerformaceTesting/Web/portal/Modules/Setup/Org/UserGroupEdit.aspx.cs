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
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class UserGroupEdit : BaseFormPage
    {
        string op = String.Empty;
        string mode = String.Empty;
        string id = String.Empty;
        string uid = String.Empty;   // 用户id
        string gid = String.Empty;   // 组id

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            mode = RequestData.Get<string>("mode"); // 用户编辑操作
            id = RequestData.Get<string>("id");
            uid = RequestData.Get<string>("uid");
            gid = RequestData.Get<string>("gid");

            OrgGroup ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                    case RequestActionEnum.Update:
                        DoSave();
                        break;
                    case RequestActionEnum.Delete:
                        ent = this.GetTargetData<OrgGroup>();
                        ent.DoDelete();
                        break;
                }
            }

            this.SetFormData();

            if (!IsAsyncRequest)
            {
                // 根据组id获取所有可选角色
                if (!String.IsNullOrEmpty(gid))
                {
                    var grp = OrgGroup.Find(gid);

                    var roleList = grp.GetRoleList();
                    PageState.Add("RoleList", roleList);
                }
            }
        }

        #region 支持方法

        /// <summary>
        /// 包含批量创建
        /// </summary>
        [ActiveRecordTransaction]
        private void DoSave()
        {
            var roleIDs = RequestData.GetList<string>("rids");

            using (new SessionScope())
            {
                if (mode == "groupuser")
                {
                    if (!String.IsNullOrEmpty(gid))
                    {
                        var grp = OrgGroup.Find(gid);
                        var ent = this.GetPostedData<OrgUserGroup>();

                        if (!String.IsNullOrEmpty(ent.UserID))
                        {
                            var userIDs = ent.UserID.Split(',').Distinct().ToArray();

                            var usrs = OrgUser.FindAll(Expression.In(OrgUser.Prop_UserID, userIDs));

                            foreach (var usr in usrs)
                            {
                                SaveSingleUserGroup(usr, grp, roleIDs);
                            }
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(gid))
                    {
                        var usr = OrgUser.Find(uid);
                        var grp = OrgGroup.Find(gid);

                        SaveSingleUserGroup(usr, grp, roleIDs);
                    }
                }
            }
        }

        private void SaveSingleUserGroup(OrgUser user, OrgGroup group, IList<string> roleIDs)
        {
            if (roleIDs.Count == 0)
            {
                // 若没有角色，则添加默认角色
                roleIDs.Add(OrgRole.DefaulRoleID);
            }

            OrgUserGroup.GrantAndRovkeRoleByID(user, group, null, roleIDs.ToArray());
        }

        private void SetFormData()
        {
            if (!String.IsNullOrEmpty(gid))
            {
                if (mode == "groupuser")
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        SetFormDataByUserIDAndGroupID(id, gid);
                    }
                    else
                    {
                        SetFormDataByGroupID(gid);
                    }
                }
                else if (!String.IsNullOrEmpty(uid))
                {
                    SetFormDataByUserIDAndGroupID(uid, gid);
                }
                else
                {
                    SetFormDataByGroupID(gid);
                }
            }
            else if (!String.IsNullOrEmpty(id))
            {
                var dicts = DataHelper.QueryDictList("SELECT TOP 1 * FROM vw_OrgRoleAuth WHERE RoleAuthID = '" + id + "'");

                this.SetFormData(dicts.FirstOrDefault());
            }
        }

        private void SetFormDataByUserIDAndGroupID(string uid, string gid)
        {
            string sqlString = String.Format("SELECT TOP 1 * FROM vw_OrgUserGroup WHERE UserID = '{0}' AND GroupID = '{1}'", uid, gid);
            var dicts = DataHelper.QueryDictList(sqlString);

            if (dicts.Count > 0)
            {
                this.SetFormData(dicts.FirstOrDefault());
            }
            else
            {
                OrgUser usr = OrgUser.Find(uid);
                OrgGroup grp = OrgGroup.Find(gid);

                this.SetFormData(new
                {
                    UserID = usr.UserID,
                    UserName = usr.Name,
                    GroupID = grp.GroupID,
                    GroupName = grp.Name,
                    RoleID = OrgRole.DefaulRoleID
                });
            }
        }

        private void SetFormDataByGroupID(string gid)
        {
            OrgGroup grp = OrgGroup.Find(gid);

            this.SetFormData(new
            {
                GroupID = grp.GroupID,
                GroupName = grp.Name,
                RoleID = OrgRole.DefaulRoleID
            });
        }

        #endregion
    }
}
