using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
    public partial class OrgUserGroup
    {
        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            /*if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }*/
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(UserGroupID))
            {
                this.DoCreate();
            }
            else
            {
                this.DoUpdate();
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            this.CreatedDate = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            OrgUserGroup[] tents = OrgUserGroup.FindAll(Expression.In(OrgUserGroup.Prop_UserGroupID, args));

			foreach (OrgUserGroup tent in tents)
			{
				tent.DoDelete();
			}
        }

        public static void GrantAndRovkeRoleByID(OrgUser user, OrgGroup group, OrgUserGroup userRoleTmpl, params string[] roleIDs)
        {
            using (new SessionScope())
            {
                var currentRoles = OrgUserGroup.FindAllByProperties(
                    OrgUserGroup.Prop_UserID, user.UserID, OrgUserGroup.Prop_GroupID, group.GroupID);

                var revokedRoles = currentRoles.Where(r => !roleIDs.Contains(r.RoleID));
                var existsRoles = currentRoles.Where(r => roleIDs.Contains(r.RoleID));

                var currentIDs = currentRoles.Select(r => r.RoleID);
                var newIDs = roleIDs.Where(r => !currentIDs.Contains(r));

                foreach (var rid in newIDs)
                {
                    var useRole = new OrgUserGroup()
                    {
                        UserID = user.UserID,
                        GroupID = group.GroupID,
                        RoleID = rid,
                        Status = 1
                    };

                    // 根据模板调整相关属性
                    if (userRoleTmpl != null)
                    {
                        useRole.Tag = userRoleTmpl.Tag;
                        useRole.Description = userRoleTmpl.Description;
                        useRole.Status = userRoleTmpl.Status;
                        useRole.EditStatus = userRoleTmpl.EditStatus;
                    }

                    useRole.DoCreate();
                }

                // 根据模板调整相关属性
                if (userRoleTmpl != null)
                {
                    foreach (var useRole in existsRoles)
                    {
                        useRole.Tag = userRoleTmpl.Tag;
                        useRole.Description = userRoleTmpl.Description;
                        useRole.Status = userRoleTmpl.Status;
                        useRole.EditStatus = userRoleTmpl.EditStatus;

                        useRole.DoUpdate();
                    }
                }

                foreach (var useRole in revokedRoles)
                {
                    useRole.DoDelete();
                }
            }
        }

        /// <summary>
        /// 由组和角色信息获取人员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IList<OrgUser> GetUserList(string groupId = null, string roleId = null)
        {
            IList<ICriterion> crits = new List<ICriterion>();

            if (!String.IsNullOrEmpty(groupId))
            {
                crits.Add(Expression.Eq(OrgUserGroup.Prop_GroupID, groupId));
            }

            if (!String.IsNullOrEmpty(groupId))
            {
                crits.Add(Expression.Eq(OrgUserGroup.Prop_RoleID, roleId));
            }

            var userIds = OrgUserGroup.FindAll(crits.ToArray()).Select(g => g.UserID);

            IList<OrgUser> userList = new List<OrgUser>();
            if (userIds.Count() > 0)
            {
                userList = OrgUser.FindAllByPrimaryKeys(userIds.ToArray());
            }

            return userList;
        }
        
        #endregion

    } // OrgUserRole
}


