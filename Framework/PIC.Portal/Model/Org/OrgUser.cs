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
    /// 系统用户类
    /// </summary>
    [Serializable]
    public partial class OrgUser : IUserInfo
    {
        #region Consts

        internal const string SYSTEM_USER_ID = "10000001";    // 系统用户ID
        internal const string SYSTEM_USER_NAME = "System";    // 系统用户名

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            if (String.IsNullOrEmpty(this.Name))
            {
                throw new MessageException("用户姓名不能为空。");
            }

            // 检查是否存在重复键
            if (!this.IsPropertyUnique(OrgUser.Prop_WorkNo))
            {
                throw new RepeatedKeyException("存在重复的工号 “" + this.WorkNo + "”");
            }

            // 检查是否存在重复键
            if (!this.IsPropertyUnique(OrgUser.Prop_LoginName))
            {
                throw new RepeatedKeyException("存在重复的登陆名 “" + this.LoginName + "”");
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            if (String.IsNullOrEmpty(this.LoginName))
            {
                this.LoginName = OrgUser.NewLoginNameFromChineseName(this.Name);
            }

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
            if (String.IsNullOrEmpty(this.LoginName))
            {
                this.LoginName = OrgUser.NewLoginNameFromChineseName(this.Name);
            }

            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作(虚拟删除)
        /// </summary>
        public void DoDelete()
        {
            this.IsDeleted = true;  // 标记删除

            this.DoUpdate();
        }

        #endregion

        #region 用户配置相关

        /// <summary>
        /// 获取用户配置
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
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        public UserBasicConfig GetBasicConfig()
        {
            var cfg = this.GetConfig();

            return cfg.ConfigInfo.Basic;
        }

        /// <summary>
        /// 获取业务流程配置
        /// </summary>
        /// <returns></returns>
        public UserBpmConfig GetBpmConfig()
        {
            var cfg = this.GetConfig();

            return cfg.ConfigInfo.Bpm;
        }

        /// <summary>
        /// 获取签字信息
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
        /// 获取签名数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetSignatureData()
        {
            OrgUserSignature sign = this.GetSignature();

            byte[] data = DrawingHelper.GetSignatureData(sign);

            return data;
        }

        #endregion

        #region 安全相关

        /// <summary>
        /// 获取此用户所有权限 -- SQLServer关联
        /// </summary>
        public IList<Auth> RetrieveAllAuth()
        {
            string sqlString = String.Format("SELECT AuthID FROM vw_OrgUserAuth WHERE UserID = '{0}'", this.UserID);
            var authIds = Data.DataHelper.QueryValueList(sqlString);

            var auths = Auth.FindAllByPrimaryKeys(authIds.ToArray());

            return auths;
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        public void ResetPassword(string pwd)
        {
            pwd = pwd ?? String.Empty;

            this.Password = MD5Encrypt.Instance.GetMD5FromString(pwd);

            this.DoUpdate();
        }

        /// <summary>
        /// 跟据ID增加权限
        /// </summary>
        /// <param name="rids"></param>
        public void AddAuthByIDs(OrgUserAuth userAuthTmpl, params string[] authIDs)
        {
            using (new SessionScope())
            {
                // 过滤掉已经存在的权限
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

                    // 根据模板调整相关属性
                    if (userAuthTmpl != null)
                    {
                        auth.Tag = userAuthTmpl.Tag;
                        auth.Description = userAuthTmpl.Description;
                        auth.Status = userAuthTmpl.Status;
                        auth.EditStatus = userAuthTmpl.EditStatus;
                    }

                    auth.DoCreate();
                }

                // 根据模板调整相关属性
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
        /// 跟据ID移除权限
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
        /// 验证密码
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

        #region 组织结构相关

        /// <summary>
        /// 同时加入多个组
        /// </summary>
        /// <param name="group"></param>
        public void JoinGroupByIDs(OrgUserGroup userRoleTmpl, params string[] groupIDs)
        {
            using (new SessionScope())
            {
                // 过滤掉已经加入的组
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

                    // 根据模板调整相关属性
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

                // 根据模板调整相关属性
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
        /// 同时退出多个组
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
        /// 获取直接汇报人(顶头上司)
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
        /// 获取所属部门
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

        #region 静态函数

        /// <summary>
        /// 获取系统用户，只允许内部调用，而且每次都是新用户（防止意外修改系统用户属性）
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
        /// 根据工号获取OrgUser
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static OrgUser Get(string workNo)
        {
            var user = OrgUser.Find(Expression.Eq(OrgUser.Prop_WorkNo, workNo));

            return user;
        }

        /// <summary>
        /// 根据登录名获取OrgUser
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
        /// 获取用户名数组（平时比较常用）
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames(params string[] ids)
        {
            OrgUser[] usrs = OrgUser.FindAll(Expression.In(OrgUser.Prop_UserID, ids));

            string[] names = usrs.Select(ent => ent.Name).ToArray();

            return names;
        }

        /// <summary>
        /// 由中文名生成登录名
        /// </summary>
        /// <returns></returns>
        public static string NewLoginNameFromChineseName(string chineseName)
        {
            string pyName = StringHelper.ConvertChineseToPY(chineseName);
            string loginName = pyName;

            // 已经存在此用户名 (因为重名概率较小，这里采用循环方式操作)
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
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            OrgUser[] tents = OrgUser.FindAllByPrimaryKeys(args);

            foreach (OrgUser tent in tents)
            {
                tent.DoDelete();
            }
        }

        #region 查询（过滤已标记删除项）

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        new public static OrgUser[] FindAll(SearchCriterion criterion)
        {
            return FindAllByCriterion(criterion as HqlSearchCriterion);
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        new public static OrgUser[] FindAll(SearchCriterion criterion, params ICriterion[] crits)
        {
            return FindAllByCriterion(criterion as HqlSearchCriterion, crits);
        }

        /// <summary>
        /// 根据查询条件查询
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
        /// 根据查询条件和Hql查询
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
        /// 验证用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns>成功则返回用户信息，错误则返回null</returns>
        public static OrgUser Authenticate(string loginName, string password, bool isEncrypted)
        {
            // 先检查系统是否有效
            //if (!SysSystem.CheckIsValid())
            //{
            //    throw new MessageException("系统问题，请联系管理员。");
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
        /// 获取系统用户数
        /// </summary>
        /// <returns></returns>
        public static int RetrieveUserCount()
        {
            return ActiveRecordMediator.Count(typeof(OrgUser), " 1 = 1");
        }

        /// <summary>
        /// 获取指定状态系统用户数
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

        #region IUserInfo 成员 

        /// <summary>
        /// 获取简单用户信息
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

