// Business class OrgRole generated from OrgRole
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using NHibernate.Criterion;
using Newtonsoft.Json.Linq;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 角色对象，当Type值小于10为系统角色，一般不允许编辑
    /// </summary>
    [Serializable]
	public partial class OrgRole
    {
        #region Consts

        public const string DefaulRoleID = "__default";  // 默认角色编号
        public const string DefaulRoleName = "默认";  // 默认角色名

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的 编号 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();
            this.CreateDate = DateTime.Now;

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
            base.Delete();
        }

        /// <summary>
        /// 获取当前角色所有组用户
        /// </summary>
        /// <returns></returns>
        public IList<OrgUser> GetAllUserList()
        {
            return GetUserListByGroupId("All");
        }

        /// <summary>
        /// 由跟据组Id获取用户
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public IList<OrgUser> GetUserListByGroupId(params string[] groupIds)
        {
            string[] _groupIds = groupIds;

            if (groupIds.Length == 1)
            {
                _groupIds = _groupIds[0].Split(',');
            }

            using (new SessionScope())
            {
                var crits = new List<ICriterion>();
                crits.Add(Expression.Eq(OrgUserGroup.Prop_RoleID, this.RoleID));

                if (!_groupIds.Contains("All", true))
                {
                    crits.Add(Expression.In(OrgUserGroup.Prop_GroupID, _groupIds));
                }

                var ugList = OrgUserGroup.FindAll(crits.ToArray());

                var userIDs = ugList.Select(ug => ug.UserID).Distinct().ToList();

                var userList = OrgUser.FindAll(Expression.In(OrgUser.Prop_UserID, userIDs));

                return userList;
            }
        }

        /// <summary>
        /// 获取组中对应角色人员
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public IList<OrgUser> GetUserListByGroupCode(params string[] groupCodes)
        {
            string[] _groupCodes = groupCodes;

            if (groupCodes.Length == 1)
            {
                _groupCodes = groupCodes[0].Split(',');
            }

            IList<OrgUser> userList = new List<OrgUser>();

            using (new SessionScope())
            {
                if (_groupCodes.Contains("All", true))
                {
                    userList = GetAllUserList();
                }
                else
                {
                    OrgGroup[] groups = OrgGroup.FindAll(Expression.In(OrgGroup.Prop_Code, _groupCodes));

                    string[] groupIds = groups.Select(g => g.GroupID).ToArray();
                    userList = GetUserListByGroupId(groupIds);
                }
            }

            userList = userList.Distinct().ToList();

            return userList;
        }

        /// <summary>
        /// 跟据ID增加权限
        /// </summary>
        /// <param name="rids"></param>
        public void AddAuthByIDs(OrgRoleAuth roleAuthTmpl, params string[] authIDs)
        {
            using (new SessionScope())
            {
                // 过滤掉已经存在的权限
                var existsAuths = OrgRoleAuth.FindAll(Expression.Eq("RoleID", this.RoleID));

                var existsIDs = existsAuths.Select(r => r.AuthID);
                var filteredIDs = authIDs.Where(r => !existsIDs.Contains(r));

                foreach (var aid in filteredIDs)
                {
                    var roleAuth = new OrgRoleAuth()
                    {
                        RoleID = this.RoleID,
                        AuthID = aid,
                        Status = 1
                    };

                    // 根据模板调整相关属性
                    if (roleAuthTmpl != null)
                    {
                        roleAuth.Tag = roleAuthTmpl.Tag;
                        roleAuth.Description = roleAuthTmpl.Description;
                        roleAuth.Status = roleAuthTmpl.Status;
                        roleAuth.EditStatus = roleAuthTmpl.EditStatus;
                    }

                    roleAuth.DoCreate();
                }

                // 根据模板调整相关属性
                if (roleAuthTmpl != null)
                {
                    foreach (var auth in existsAuths)
                    {
                        auth.Tag = roleAuthTmpl.Tag;
                        auth.Description = roleAuthTmpl.Description;
                        auth.Status = roleAuthTmpl.Status;
                        auth.EditStatus = roleAuthTmpl.EditStatus;

                        auth.DoUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// 跟据ID移除权限
        /// </summary>
        /// <param name="authIDs"></param>
        public void RemoveAuthByIDs(params string[] authIDs)
        {
            using (new SessionScope())
            {
                var existsAuths = OrgRoleAuth.FindAll(Expression.In("AuthID", authIDs));

                foreach (var tent in existsAuths)
                {
                    tent.DoDelete();
                }
            }
        }

        /// <summary>
        /// 获取角色所有权限
        /// </summary>
        public IList<Auth> RetrieveAllAuth()
        {
            var roleAuths = OrgRoleAuth.FindAll(Expression.Eq(OrgRoleAuth.Prop_RoleID, this.RoleID));
            var authIds = roleAuths.Select(a => a.AuthID);

            var auths = Auth.FindAllByPrimaryKeys(authIds.ToArray());

            return auths;
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 由编码获取Enumeration
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OrgRole Get(string code)
        {
            OrgRole[] tents = OrgRole.FindAllByProperty(OrgRole.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }

        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            OrgRole[] tents = OrgRole.FindAllByPrimaryKeys(args);

            foreach (OrgRole tent in tents)
            {
                tent.DoDelete();
            }
        }

        #endregion

    } // OrgRole
}

