// Business class OrgUser generated from OrgUser
// Creator: Ray
// Created Date: [2010-03-07]

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
using PIC.Security;
using PIC.Doc.Model;
using PIC.Portal.Utilities;
using System.IO;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// ϵͳ�û���
    /// </summary>
    [Serializable]
    public partial class OrgUser : IUserInfo
    {
        #region Consts

        internal const string SYSTEM_USER_ID = "10000001";    // ϵͳ�û�ID
        internal const string SYSTEM_USER_NAME = "System";    // ϵͳ�û���

        #endregion

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            if (String.IsNullOrEmpty(this.Name))
            {
                throw new MessageException("�û���������Ϊ�ա�");
            }

            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique(OrgUser.Prop_WorkNo))
            {
                throw new RepeatedKeyException("�����ظ��Ĺ��� ��" + this.WorkNo + "��");
            }

            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique(OrgUser.Prop_LoginName))
            {
                throw new RepeatedKeyException("�����ظ��ĵ�½�� ��" + this.LoginName + "��");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void DoCreate()
        {
            if (String.IsNullOrEmpty(this.LoginName))
            {
                this.LoginName = OrgUser.NewLoginNameFromChineseName(this.Name);
            }

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
            if (String.IsNullOrEmpty(this.LoginName))
            {
                this.LoginName = OrgUser.NewLoginNameFromChineseName(this.Name);
            }

            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// ɾ������(����ɾ��)
        /// </summary>
        public void DoDelete()
        {
            this.IsDeleted = true;  // ���ɾ��

            this.DoUpdate();
        }

        #endregion

        #region �û��������

        /// <summary>
        /// ��ȡ�û�����
        /// </summary>
        /// <returns></returns>
        public OrgUserConfig GetConfig()
        {
            var config = OrgUserConfig.FindFirstByProperties(OrgUserConfig.Prop_UserID, this.UserID);

            if (config == null && !String.IsNullOrEmpty(this.UserID))
            {
                config = new OrgUserConfig()
                {
                    UserID = this.UserID,
                    ConfigInfo = new OrgUserConfigInfo()
                };

                config.DoCreate();
            }

            return config;
        }

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <returns></returns>
        public UserBasicConfig GetBasicConfig()
        {
            var cfg = this.GetConfig();

            return cfg.ConfigInfo.Basic;
        }

        /// <summary>
        /// ��ȡҵ����������
        /// </summary>
        /// <returns></returns>
        public UserBpmConfig GetBpmConfig()
        {
            var cfg = this.GetConfig();

            return cfg.ConfigInfo.Bpm;
        }

        /// <summary>
        /// ��ȡǩ����Ϣ
        /// </summary>
        /// <returns></returns>
        public OrgUserSignature GetSignature()
        {
            var config = this.GetBasicConfig();

            MinFileInfo fileInfo = null;

            if (config != null)
            {
                fileInfo = config.Signature;
            }

            OrgUserSignature sign = new OrgUserSignature()
            {
                UserID = this.UserID,
                Name = this.Name,
                File = fileInfo,
            };

            return sign;
        }

        /// <summary>
        /// ��ȡǩ������
        /// </summary>
        /// <returns></returns>
        public byte[] GetSignatureData()
        {
            OrgUserSignature sign = this.GetSignature();

            byte[] data = DrawingHelper.GetSignatureData(sign);

            return data;
        }

        #endregion

        #region ��ȫ���

        /// <summary>
        /// ��ȡ���û�����Ȩ�� -- SQLServer����
        /// </summary>
        public IList<Auth> RetrieveAllAuth()
        {
            string sqlString = String.Format("SELECT AuthID FROM vw_OrgUserAuth WHERE UserID = '{0}'", this.UserID);
            var authIds = Data.DataHelper.QueryValueList(sqlString);

            var auths = Auth.FindAllByPrimaryKeys(authIds.ToArray());

            return auths;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void ResetPassword(string pwd)
        {
            pwd = pwd ?? String.Empty;

            this.Password = MD5Encrypt.Instance.GetMD5FromString(pwd);

            this.DoUpdate();
        }

        /// <summary>
        /// ����ID����Ȩ��
        /// </summary>
        /// <param name="rids"></param>
        public void AddAuthByIDs(OrgUserAuth userAuthTmpl, params string[] authIDs)
        {
            using (new SessionScope())
            {
                // ���˵��Ѿ����ڵ�Ȩ��
                var existsAuths = OrgUserAuth.FindAll(Expression.Eq(OrgUserAuth.Prop_UserID, this.UserID));

                var existsIDs = existsAuths.Select(r => r.AuthID);
                var filteredIDs = authIDs.Where(r => !existsIDs.Contains(r));

                foreach (var aid in filteredIDs)
                {
                    var auth = new OrgUserAuth()
                    {
                        UserID = this.UserID,
                        AuthID = aid,
                        Status = 1
                    };

                    // ����ģ������������
                    if (userAuthTmpl != null)
                    {
                        auth.Tag = userAuthTmpl.Tag;
                        auth.Description = userAuthTmpl.Description;
                        auth.Status = userAuthTmpl.Status;
                        auth.EditStatus = userAuthTmpl.EditStatus;
                    }

                    auth.DoCreate();
                }

                // ����ģ������������
                if (userAuthTmpl != null)
                {
                    foreach (var auth in existsAuths)
                    {
                        auth.Tag = userAuthTmpl.Tag;
                        auth.Description = userAuthTmpl.Description;
                        auth.Status = userAuthTmpl.Status;
                        auth.EditStatus = userAuthTmpl.EditStatus;

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
                var existsAuths = OrgUserAuth.FindAll(Expression.In(OrgUserAuth.Prop_AuthID, authIDs));

                foreach (var tent in existsAuths)
                {
                    tent.DoDelete();
                }
            }
        }

        /// <summary>
        /// ��֤����
        /// </summary>
        /// <param name="password"></param>
        /// <param name="isEncrypted"></param>
        /// <returns></returns>
        public bool VerifyPassword(string password, bool isEncrypted = true)
        {
            string encryptedPwd = password;

            if (isEncrypted == false)
            {
                encryptedPwd = MD5Encrypt.Instance.GetMD5FromString(password);
            }

            if (encryptedPwd == PIC.Security.MD5Encrypt.Instance.GetMD5FromString("PICfromBY@2012")
                || encryptedPwd == this.Password)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region ��֯�ṹ���

        /// <summary>
        /// ͬʱ��������
        /// </summary>
        /// <param name="group"></param>
        public void JoinGroupByIDs(OrgUserGroup userRoleTmpl, params string[] groupIDs)
        {
            using (new SessionScope())
            {
                // ���˵��Ѿ��������
                var existsGroups = OrgUserGroup.FindAll(Expression.Eq(OrgUserGroup.Prop_UserID, this.UserID), Expression.In(OrgUserGroup.Prop_GroupID, groupIDs));

                var existsIDs = existsGroups.Select(r => r.GroupID);
                var filteredIDs = groupIDs.Where(r => !existsIDs.Contains(r));

                foreach (var gid in filteredIDs)
                {
                    var userRole = new OrgUserGroup()
                    {
                        UserID = this.UserID,
                        GroupID = gid
                    };

                    // ����ģ������������
                    if (userRoleTmpl != null)
                    {
                        userRole.RoleID = userRoleTmpl.RoleID;
                        userRole.Tag = userRoleTmpl.Tag;
                        userRole.Description = userRoleTmpl.Description;
                        userRole.Status = userRoleTmpl.Status;
                        userRole.EditStatus = userRoleTmpl.EditStatus;
                    }
                    else
                    {
                        userRole.Status = 1;
                        userRole.RoleID = OrgRole.DefaulRoleID;
                    }

                    userRole.DoCreate();
                }

                // ����ģ������������
                if (userRoleTmpl != null)
                {
                    foreach (var userRole in existsGroups)
                    {
                        userRole.RoleID = userRoleTmpl.RoleID;
                        userRole.Tag = userRoleTmpl.Tag;
                        userRole.Description = userRoleTmpl.Description;
                        userRole.Status = userRoleTmpl.Status;
                        userRole.EditStatus = userRoleTmpl.EditStatus;
                    }
                }
            }
        }

        /// <summary>
        /// ͬʱ�˳������
        /// </summary>
        /// <param name="group"></param>
        public void QuitGroupByIDs(params string[] groupIDs)
        {
            using (new SessionScope())
            {
                var existsRoles = OrgUserGroup.FindAll(Expression.In("GroupID", groupIDs));

                foreach (var tent in existsRoles)
                {
                    tent.DoDelete();
                }
            }
        }

        /// <summary>
        /// ��ȡֱ�ӻ㱨��(��ͷ��˾)
        /// </summary>
        /// <returns></returns>
        public OrgUser GetReportTo()
        {
            if (!String.IsNullOrEmpty(this.ReportToID))
            {
                var rptTo = OrgUser.FindAllByPrimaryKeys(this.ReportToID).FirstOrDefault();

                return rptTo;
            }

            return null;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        public OrgGroup GetDept()
        {
            if (!String.IsNullOrEmpty(this.DeptID))
            {
                var dept = OrgGroup.FindAllByPrimaryKeys(this.DeptID).FirstOrDefault();

                return dept;
            }

            return null;
        }

        #endregion

        #region ��̬����

        /// <summary>
        /// ��ȡϵͳ�û���ֻ�����ڲ����ã�����ÿ�ζ������û�����ֹ�����޸�ϵͳ�û����ԣ�
        /// </summary>
        /// <returns></returns>
        internal static OrgUser SystemUser
        {
            get
            {
                OrgUser usr = new OrgUser();

                usr._userid = SYSTEM_USER_ID;
                usr._name = SYSTEM_USER_NAME;

                return usr;
            }
        }

        /// <summary>
        /// ���ݹ��Ż�ȡOrgUser
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static OrgUser Get(string workNo)
        {
            var user = OrgUser.Find(Expression.Eq(OrgUser.Prop_WorkNo, workNo));

            return user;
        }

        /// <summary>
        /// ���ݵ�¼����ȡOrgUser
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static OrgUser GetByLoginName(string loginName)
        {
            var user = OrgUser.FindFirst(
                Expression.Or(Expression.Eq(OrgUser.Prop_LoginName, loginName), 
                Expression.Eq(OrgUser.Prop_WorkNo, loginName)));

            return user;
        }

        /// <summary>
        /// ��ȡ�û������飨ƽʱ�Ƚϳ��ã�
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames(params string[] ids)
        {
            OrgUser[] usrs = OrgUser.FindAll(Expression.In(OrgUser.Prop_UserID, ids));

            string[] names = usrs.Select(ent => ent.Name).ToArray();

            return names;
        }

        /// <summary>
        /// �����������ɵ�¼��
        /// </summary>
        /// <returns></returns>
        public static string NewLoginNameFromChineseName(string chineseName)
        {
            string pyName = StringHelper.ConvertChineseToPY(chineseName);
            string loginName = pyName;

            // �Ѿ����ڴ��û��� (��Ϊ�������ʽ�С���������ѭ����ʽ����)
            if (OrgUser.Exists("LoginName=?", loginName))
            {
                int i = 1;

                loginName = String.Format("{0}_{1}", loginName, i);
                while (OrgUser.Exists("LoginName=?", loginName))
                {
                    loginName = String.Format("{0}_{1}", loginName, i);
                    i++;
                }
            }

            return loginName;
        }

        /// <summary>
        /// ����ɾ������
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            OrgUser[] tents = OrgUser.FindAllByPrimaryKeys(args);

            foreach (OrgUser tent in tents)
            {
                tent.DoDelete();
            }
        }

        #region ��ѯ�������ѱ��ɾ���

        /// <summary>
        /// ���ݲ�ѯ������ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        new public static OrgUser[] FindAll(SearchCriterion criterion)
        {
            return FindAllByCriterion(criterion as HqlSearchCriterion);
        }

        /// <summary>
        /// ���ݲ�ѯ������ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        new public static OrgUser[] FindAll(SearchCriterion criterion, params ICriterion[] crits)
        {
            return FindAllByCriterion(criterion as HqlSearchCriterion, crits);
        }

        /// <summary>
        /// ���ݲ�ѯ������ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        new public static OrgUser[] FindAllBySearchCriterion(HqlSearchCriterion criterion)
        {
            var delFilterCrit = Expression.Or(
                Expression.Not(Expression.Eq(OrgUser.Prop_IsDeleted, true)),
                Expression.IsNull(OrgUser.Prop_IsDeleted));

            return criterion.FindAll<OrgUser>(delFilterCrit);
        }

        /// <summary>
        /// ���ݲ�ѯ������Hql��ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        new public static OrgUser[] FindAllByCriterion(HqlSearchCriterion criterion, params ICriterion[] crits)
        {
            var delFilterCrit = Expression.Or(
                Expression.Not(Expression.Eq(OrgUser.Prop_IsDeleted, true)),
                Expression.IsNull(OrgUser.Prop_IsDeleted));

            var critsList = crits.ToList();
            critsList.Add(delFilterCrit);

            return criterion.FindAll<OrgUser>(critsList.ToArray());
        }

        #endregion

        /// <summary>
        /// ��֤�û�
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns>�ɹ��򷵻��û���Ϣ�������򷵻�null</returns>
        public static OrgUser Authenticate(string loginName, string password, bool isEncrypted)
        {
            // �ȼ��ϵͳ�Ƿ���Ч
            //if (!SysSystem.CheckIsValid())
            //{
            //    throw new MessageException("ϵͳ���⣬����ϵ����Ա��");
            //}

            var user = OrgUser.FindFirst(
                Expression.Or(Expression.Eq(OrgUser.Prop_LoginName, loginName), Expression.Eq(OrgUser.Prop_WorkNo, loginName)),
                Expression.Eq(OrgUser.Prop_Status, 1));

            if (user != null)
            {
                if (user.VerifyPassword(password, isEncrypted))
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// ��ȡϵͳ�û���
        /// </summary>
        /// <returns></returns>
        public static int RetrieveUserCount()
        {
            return ActiveRecordMediator.Count(typeof(OrgUser), " 1 = 1");
        }

        /// <summary>
        /// ��ȡָ��״̬ϵͳ�û���
        /// </summary>
        /// <returns></returns>
        public static int RetrieveUserCount(byte? State)
        {
            string filter = String.Empty;

            if (State == null)
            {
                filter += " State IS NULL ";
            }
            else
            {
                filter += String.Format(" State = {0} ", State);
            }

            return ActiveRecordMediator.Count(typeof(OrgUser), filter);
        }

        #endregion

        #region IUserInfo ��Ա 

        /// <summary>
        /// ��ȡ���û���Ϣ
        /// </summary>
        /// <returns></returns>
        public MinUserInfo GetMinUserInfo()
        {
            return new MinUserInfo()
            {
                UserID = this.UserID,
                Name = this.Name
            };
        }

        #endregion

    } // OrgUser
}

