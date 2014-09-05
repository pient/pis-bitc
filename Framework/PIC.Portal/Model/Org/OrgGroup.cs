// Business class OrgGroup generated from OrgGroup
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Criterion;
using NHibernate.Transform;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class OrgGroup : TreeNodeEntityBase<OrgGroup>
    {
        #region 私有变量

        private bool? _IsLeaf = null;

        #endregion

        #region 成员属性

        /// <summary>
        /// 是否根模块（没有子节点）
        /// </summary>
        public override bool? IsLeaf
        {
            get
            {
                if (_IsLeaf == null && !String.IsNullOrEmpty(this.GroupID))
                {
                    bool isLeaf = !OrgGroup.Exists("ParentID = ?", this.GroupID);
                    _IsLeaf = isLeaf;
                }

                return _IsLeaf.GetValueOrDefault();
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique(OrgGroup.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的 编号 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        protected override void DoCreate()
        {
            this.DoValidate();

            if (this.Parent != null)
            {
                this.Type = this.Parent.Type;
            }

            this.CreateDate = DateTime.Now;

            // 事务开始
            base.DoCreate();
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
        /// 删除模块
        /// </summary>
        public void DoDelete()
        {
            DataHelper.ExecSp("usp_Org_DeleteGroup", "@_GroupID", this.GroupID);
        }

        /// <summary>
        /// 获取当前组所拥有的职能
        /// </summary>
        public IList<OrgGroupFunction> GetFuncList()
        {
            var grpFuncList = OrgGroupFunction.FindAll(Expression.Eq(OrgGroupFunction.Prop_GroupID, this.GroupID));

            return grpFuncList;
        }

        /// <summary>
        /// 获取当前组可以拥有的角色
        /// </summary>
        public IList<OrgRole> GetRoleList()
        {
            using (new SessionScope())
            {
                var grpFuncs = this.GetFuncList();

                if (grpFuncs.Count > 0)
                {
                    var funcIDs = grpFuncs.Select(gf => gf.FunctionID).Distinct().ToList();
                    var funcRoles = OrgFunctionRole.FindAll(Expression.In(OrgFunctionRole.Prop_FunctionID, funcIDs));
                    var roleIDs = funcRoles.Select(fr =>fr.RoleID).Distinct().ToList();

                    var roleList = OrgRole.FindAll(Expression.In(OrgRole.Prop_RoleID, roleIDs));

                    return roleList;
                }

                return null;
            }
        }

        /// <summary>
        /// 获取当前角色所有组用户
        /// </summary>
        /// <returns></returns>
        public IList<OrgUser> GetAllUserList()
        {
            return GetUserListByRoleId("All");
        }

        /// <summary>
        /// 由跟据组Id获取用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IList<OrgUser> GetUserListByRoleId(string roleId)
        {
            using (new SessionScope())
            {
                var crits = new List<ICriterion>();
                crits.Add(Expression.Eq(OrgUserGroup.Prop_GroupID, this.GroupID));

                if (!roleId.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                {
                    crits.Add(Expression.Eq(OrgUserGroup.Prop_GroupID, roleId));
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
        public IList<OrgUser> GetUserListByRoleCode(string roleCode)
        {
            using (new SessionScope())
            {
                IList<OrgUser> userList = new List<OrgUser>();
                IList<OrgUserGroup> ugList = new List<OrgUserGroup>();

                if (roleCode.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                {
                    userList = GetAllUserList();
                }
                else
                {
                    var role = OrgRole.Get(roleCode);
                    userList = GetUserListByRoleId(role.RoleID);
                }

                return userList;
            }
        }

        /// <summary>
        /// 跟据ID增加角色
        /// </summary>
        /// <param name="fids"></param>
        public void AddFuncByIDs(OrgGroupFunction groupFuncTmpl, params string[] fids)
        {
            using (new SessionScope())
            {
                // 过滤掉已经存在的角色
                var existsFuncs = OrgGroupFunction.FindAll(
                    Expression.Eq(OrgGroupFunction.Prop_GroupID, this.GroupID),
                    Expression.In(OrgGroupFunction.Prop_FunctionID, fids));

                var existsIDs = existsFuncs.Select(r => r.FunctionID);
                var filteredIDs = fids.Where(id => !existsIDs.Contains(id));

                foreach (var id in filteredIDs)
                {
                    var func = new OrgGroupFunction()
                    {
                        GroupID = this.GroupID,
                        FunctionID = id,
                        Status = 1
                    };

                    // 根据模板调整相关属性
                    if (groupFuncTmpl != null)
                    {
                        func.Tag = groupFuncTmpl.Tag;
                        func.Description = groupFuncTmpl.Description;
                        func.Status = groupFuncTmpl.Status;
                        func.EditStatus = groupFuncTmpl.EditStatus;
                    }

                    func.DoCreate();
                }

                // 根据模板调整相关属性
                if (groupFuncTmpl != null)
                {
                    foreach (var func in existsFuncs)
                    {
                        func.Tag = groupFuncTmpl.Tag;
                        func.Description = groupFuncTmpl.Description;
                        func.Status = groupFuncTmpl.Status;
                        func.EditStatus = groupFuncTmpl.EditStatus;

                        func.DoUpdate();
                    }
                }
            }
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 由编码获取Enumeration
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OrgGroup Get(string code)
        {
            OrgGroup[] tents = OrgGroup.FindAllByProperty(OrgGroup.Prop_Code, code);
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
            OrgGroup[] tents = OrgGroup.FindAllByPrimaryKeys(args);

            foreach (OrgGroup tent in tents)
            {
                tent.DoDelete();
            }
        }

        public static IList<OrgUser> GetUserList(string groupCode, string roleCode)
        {
            var grp = OrgGroup.Get(groupCode);

            var userList = grp.GetUserListByRoleCode(roleCode);

            return userList;
        }

        #endregion

    } // OrgGroup
}

