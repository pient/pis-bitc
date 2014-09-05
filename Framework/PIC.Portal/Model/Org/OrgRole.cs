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
    /// ��ɫ���󣬵�TypeֵС��10Ϊϵͳ��ɫ��һ�㲻����༭
    /// </summary>
    [Serializable]
	public partial class OrgRole
    {
        #region Consts

        public const string DefaulRoleID = "__default";  // Ĭ�Ͻ�ɫ���
        public const string DefaulRoleName = "Ĭ��";  // Ĭ�Ͻ�ɫ��

        #endregion

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("�����ظ��� ��� ��" + this.Code + "��");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();
            this.CreateDate = DateTime.Now;

            // ����ʼ
            this.CreateAndFlush();
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
        /// ɾ������
        /// </summary>
        public void DoDelete()
        {
            base.Delete();
        }

        /// <summary>
        /// ��ȡ��ǰ��ɫ�������û�
        /// </summary>
        /// <returns></returns>
        public IList<OrgUser> GetAllUserList()
        {
            return GetUserListByGroupId("All");
        }

        /// <summary>
        /// �ɸ�����Id��ȡ�û�
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
        /// ��ȡ���ж�Ӧ��ɫ��Ա
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
        /// ����ID����Ȩ��
        /// </summary>
        /// <param name="rids"></param>
        public void AddAuthByIDs(OrgRoleAuth roleAuthTmpl, params string[] authIDs)
        {
            using (new SessionScope())
            {
                // ���˵��Ѿ����ڵ�Ȩ��
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

                    // ����ģ������������
                    if (roleAuthTmpl != null)
                    {
                        roleAuth.Tag = roleAuthTmpl.Tag;
                        roleAuth.Description = roleAuthTmpl.Description;
                        roleAuth.Status = roleAuthTmpl.Status;
                        roleAuth.EditStatus = roleAuthTmpl.EditStatus;
                    }

                    roleAuth.DoCreate();
                }

                // ����ģ������������
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
        /// ����ID�Ƴ�Ȩ��
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
        /// ��ȡ��ɫ����Ȩ��
        /// </summary>
        public IList<Auth> RetrieveAllAuth()
        {
            var roleAuths = OrgRoleAuth.FindAll(Expression.Eq(OrgRoleAuth.Prop_RoleID, this.RoleID));
            var authIds = roleAuths.Select(a => a.AuthID);

            var auths = Auth.FindAllByPrimaryKeys(authIds.ToArray());

            return auths;
        }

        #endregion

        #region ��̬��Ա

        /// <summary>
        /// �ɱ����ȡEnumeration
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
        /// ����ɾ������
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

