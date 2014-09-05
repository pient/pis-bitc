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
using PIC.Portal;
using PIC.Portal.Model;
	
namespace PIC.Biz.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
    public partial class HR_Employee
    {
        #region 枚举

        /// <summary>
        /// 状态枚举
        /// </summary>
        public enum StatusEnum
        {
            OnWork,
            Available,
            Disabled,
            ContractWarned,
            Unknown
        }

        #endregion

        #region 成员变量

        [JsonIgnore]
        public StatusEnum EmployeeStatus
        {

            get { return CLRHelper.GetEnum<StatusEnum>(this.Status, StatusEnum.Unknown); }
            set { this.Status = value.ToString();
            }

        }

        #endregion

        #region 成员属性

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 工号不能为空
            if (String.IsNullOrEmpty(this.Code))
            {
                throw new MessageException("员工编号不能为空");
            }

            // 检查是否存在重复键
            if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(Id))
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

            this.CreatorId = UserInfo.Name;
            this.CreatorName = UserInfo.Name;
            this.CreatedDate = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();

            // 创建合同信息
            // HR_EmployeeContract contract = HR_EmployeeContract.CreateFromEmployee(this);
        }

        /// <summary>
        /// 创建并同步数据到系统
        /// </summary>
        [ActiveRecordTransaction]
        public void DoCreateAndSync()
        {
            DoCreate();

            SyncToSystemByCode(this);
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
        /// 创建并同步数据到系统
        /// </summary>
        [ActiveRecordTransaction]
        public void DoUpdateAndSync()
        {
            using (new SessionScope())
            {
                DoUpdate();

                SyncToSystemByCode(this);
            }
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        /// <summary>
        /// 获取对应系统用户
        /// </summary>
        /// <returns></returns>
        public OrgUser GetSysUser()
        {
            if (!String.IsNullOrEmpty(this.UserId))
            {
                return OrgUser.Find(this.UserId);
            }

            return null;
        }

        /// <summary>
        /// 获取对应部门
        /// </summary>
        /// <returns></returns>
        public OrgGroup GetDepartment()
        {
            if (!String.IsNullOrEmpty(this.DepartmentId))
            {
                return OrgGroup.Find(this.DepartmentId);
            }

            return null;
        }

        /// <summary>
        /// 转换部门
        /// </summary>
        public void ChangeDepartment(string grpid)
        {
            if (this.DepartmentId != grpid)
            {
                string origDeptId = this.DepartmentId;

                this.DepartmentId = grpid;
                OrgUser usr = SyncToSystemByCode(this).FirstOrDefault();

                // 从原来部门移出
                if (!String.IsNullOrEmpty(origDeptId))
                {
                    OrgGroup origDept = OrgGroup.Find(origDeptId);

                    // origDept.RemoveUser(usr);
                }
            }
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			HR_Employee[] tents = HR_Employee.FindAll(Expression.In("Id", args));

			foreach (HR_Employee tent in tents)
			{
				tent.DoDelete();
			}
        }

        #region 与系统信息同步

        /// <summary>
        /// 批量同步操作
        /// </summary>
        public static void DoBatchSync(params object[] args)
        {
            HR_Employee[] tents = HR_Employee.FindAll(Expression.In("Id", args));

            SyncToSystemByCode(tents);
        }

        /// <summary>
        /// 根据工号同步员工信息到系统（若用户没有工号则无法同步）
        /// </summary>
        /// <param name="employee"></param>
        public static IList<OrgUser> SyncToSystemByCode(params HR_Employee[] employees)
        {
            IList<OrgUser> rtn_usrs = new List<OrgUser>();

            IEnumerable<string> codes = employees.Select(ent => ent.Code);
            IEnumerable<string> grpids = employees.Where(ent => !String.IsNullOrEmpty(ent.DepartmentId))
                .Select(ent => ent.DepartmentId);

            OrgUser[] existsUsrs = OrgUser.FindAll(Expression.In(OrgUser.Prop_WorkNo, codes.ToArray()));
            OrgGroup[] grps = OrgGroup.FindAll(Expression.In(OrgGroup.Prop_GroupID, grpids.ToArray()));

            foreach (HR_Employee t in employees)
            {
                OrgUser tusr = existsUsrs.FirstOrDefault(tent =>
                {
                    if (StringHelper.IsEqualsIgnoreCase(tent.WorkNo, t.Code))
                        return true;
                    else
                        return false;
                });

                OrgGroup grp = null;

                if (!String.IsNullOrEmpty(t.DepartmentId))
                {
                    grp = grps.First(tent => StringHelper.IsEqualsIgnoreCase(tent.GroupID, t.DepartmentId));
                }

                OrgUser rtn_usr;

                if (tusr != null)
                {
                    rtn_usr = SyncToSystem(t, tusr, grp);
                }
                else
                {
                    rtn_usr = SyncToSystem(t, grp);
                }

                rtn_usrs.Add(rtn_usr);
            }

            return rtn_usrs;
        }

        /// <summary>
        /// 根据员工ID同步员工信息到系统, 用于为没有员工编号的情形（若用户没有工号则无法同步, 原来部门是否保留？）
        /// </summary>
        /// <param name="employee"></param>
        public static IList<OrgUser> SyncToSystemByID(params HR_Employee[] employees)
        {
            IList<OrgUser> rtn_usrs = new List<OrgUser>();

            IEnumerable<string> usrids = employees.Where(ent => !String.IsNullOrEmpty(ent.UserId))
                .Select(ent => ent.UserId);

            IEnumerable<string> grpids = employees.Where(ent => !String.IsNullOrEmpty(ent.DepartmentId))
                .Select(ent => ent.DepartmentId);

            OrgUser[] usrs = OrgUser.FindAll(Expression.In(OrgUser.Prop_UserID, usrids.ToArray()));
            OrgGroup[] grps = OrgGroup.FindAll(Expression.In(OrgGroup.Prop_GroupID, grpids.ToArray()));

            foreach (HR_Employee t in employees)
            {
                OrgUser tusr = usrs.FirstOrDefault(tent =>
                {
                    if (StringHelper.IsEqualsIgnoreCase(tent.UserID, t.UserId))
                        return true;
                    else
                        return false;
                });

                OrgGroup grp = null;

                if (!String.IsNullOrEmpty(t.DepartmentId))
                {
                    grp = grps.First(tent => StringHelper.IsEqualsIgnoreCase(tent.GroupID, t.DepartmentId));
                }

                OrgUser rtn_usr;

                if (tusr != null)
                {
                    rtn_usr = SyncToSystem(t, tusr, grp);
                }
                else
                {
                    rtn_usr = SyncToSystem(t, grp);
                }

                rtn_usrs.Add(rtn_usr);
            }

            return rtn_usrs;
        }

        /// <summary>
        /// 同步信息到系统（用户已存在）
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="usr"></param>
        private static OrgUser SyncToSystem(HR_Employee employee, OrgUser usr, OrgGroup grp)
        {
            usr.WorkNo = employee.Code;
            usr.Name = employee.Name;
            usr.Email = employee.Email;
            usr.Password = PIC.Security.MD5Encrypt.Instance.GetMD5FromString(usr.WorkNo);   // 密码默认为工号

            usr.DoUpdate();

            if (grp != null)
            {
                // grp.AddUser(usr);
            }

            employee.UserId = usr.UserID;
            employee.Update();

            return usr;
        }

        /// <summary>
        /// 同步信息到系统（用户不存在）
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="usr"></param>
        private static OrgUser SyncToSystem(HR_Employee employee, OrgGroup grp)
        {
            OrgUser usr = new OrgUser();

            usr.WorkNo = employee.Code;
            usr.Name = employee.Name;
            usr.Email = employee.Email;
            usr.Status = 1; // 同步的同时激活

            usr.DoCreate();

            if (grp != null)
            {
                // grp.AddUser(usr.UserID);
            }

            employee.UserId = usr.UserID;
            employee.UpdateAndFlush();

            return usr;
        }

        /// <summary>
        /// 根据工号同步系统员工信息到HR库
        /// </summary>
        /// <param name="employee"></param>
        public static void SyncFromSystemByCode(params OrgUser[] usrs)
        {
            IEnumerable<string> codes = usrs.Select(ent => ent.WorkNo);

            HR_Employee[] existsEmployees = HR_Employee.FindAll(Expression.In(HR_Employee.Prop_Code, codes.ToArray()));

            foreach (OrgUser t in usrs)
            {
                HR_Employee t_employee = existsEmployees.FirstOrDefault(tent =>
                {
                    if (StringHelper.IsEqualsIgnoreCase(tent.Code, t.WorkNo))
                        return true;
                    else
                        return false;
                });

                if (t_employee != null)
                {
                    SyncFromSystem(t_employee, t);
                }
                else
                {
                    SyncFromSystem(t);
                }
            }
        }

        /// <summary>
        /// 根据系统ID同步系统员工信息到HR库(组信息无法同步)
        /// </summary>
        /// <param name="employee"></param>
        public static void SyncFromSystemByID(params OrgUser[] usrs)
        {
            IEnumerable<string> usrids = usrs.Select(ent => ent.UserID);

            HR_Employee[] existsEmployees = HR_Employee.FindAll(Expression.In(HR_Employee.Prop_UserId, usrids.ToArray()));

            foreach (OrgUser t in usrs)
            {
                HR_Employee t_employee = existsEmployees.FirstOrDefault(tent =>
                {
                    if (StringHelper.IsEqualsIgnoreCase(tent.UserId, t.UserID))
                        return true;
                    else
                        return false;
                });

                if (t_employee != null)
                {
                    SyncFromSystem(t_employee, t);
                }
                else
                {
                    SyncFromSystem(t);
                }
            }
        }

        /// <summary>
        /// 同步信息到系统（用户已存在）
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="usr"></param>
        private static void SyncFromSystem(HR_Employee employee, OrgUser usr)
        {
            employee.UserId = usr.UserID;
            employee.Code = usr.WorkNo;
            employee.Name = usr.Name;
            employee.Email = usr.Email;

            employee.DoUpdate();
        }

        /// <summary>
        /// 同步信息到系统（用户不存在）
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="usr"></param>
        private static void SyncFromSystem(OrgUser usr)
        {
            HR_Employee employee = new HR_Employee();

            employee.UserId = usr.UserID;
            employee.Code = usr.WorkNo;
            employee.Name = usr.Name;
            employee.Email = usr.Email;

            usr.DoCreate();
        }

        #endregion

        #endregion

    } // HR_Employee
}


