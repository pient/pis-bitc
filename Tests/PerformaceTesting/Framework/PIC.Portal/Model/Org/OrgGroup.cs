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
        #region ˽�б���

        private bool? _IsLeaf = null;

        #endregion

        #region ��Ա����

        /// <summary>
        /// �Ƿ��ģ�飨û���ӽڵ㣩
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

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique(OrgGroup.Prop_Code))
            {
                throw new RepeatedKeyException("�����ظ��� ��� ��" + this.Code + "��");
            }
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        protected override void DoCreate()
        {
            this.DoValidate();

            if (this.Parent != null)
            {
                this.Type = this.Parent.Type;
            }

            this.CreateDate = DateTime.Now;

            // ����ʼ
            base.DoCreate();
        }

        /// <summary>
        /// �޸Ĳ���
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// ɾ��ģ��
        /// </summary>
        public void DoDelete()
        {
            DataHelper.ExecSp("usp_Org_DeleteGroup", "@_GroupID", this.GroupID);
        }

        /// <summary>
        /// ��ȡ��ǰ����ӵ�е�ְ��
        /// </summary>
        public IList<OrgGroupFunction> GetFuncList()
        {
            var grpFuncList = OrgGroupFunction.FindAll(Expression.Eq(OrgGroupFunction.Prop_GroupID, this.GroupID));

            return grpFuncList;
        }

        /// <summary>
        /// ��ȡ��ǰ�����ӵ�еĽ�ɫ
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
        /// ��ȡ��ǰ��ɫ�������û�
        /// </summary>
        /// <returns></returns>
        public IList<OrgUser> GetAllUserList()
        {
            return GetUserListByRoleId("All");
        }

        /// <summary>
        /// �ɸ�����Id��ȡ�û�
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
        /// ��ȡ���ж�Ӧ��ɫ��Ա
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
        /// ����ID���ӽ�ɫ
        /// </summary>
        /// <param name="fids"></param>
        public void AddFuncByIDs(OrgGroupFunction groupFuncTmpl, params string[] fids)
        {
            using (new SessionScope())
            {
                // ���˵��Ѿ����ڵĽ�ɫ
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

                    // ����ģ������������
                    if (groupFuncTmpl != null)
                    {
                        func.Tag = groupFuncTmpl.Tag;
                        func.Description = groupFuncTmpl.Description;
                        func.Status = groupFuncTmpl.Status;
                        func.EditStatus = groupFuncTmpl.EditStatus;
                    }

                    func.DoCreate();
                }

                // ����ģ������������
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

        #region ��̬��Ա

        /// <summary>
        /// �ɱ����ȡEnumeration
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
        /// ����ɾ������
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

