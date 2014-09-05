using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;

namespace PIC.Portal.Model
{
    [Serializable]
    public partial class Application
    {
        #region  Consts & Enums

        public const string PortalAppCode = "PORTAL";

        // 应用类型枚举
        public enum ApplicationTypeEnum
        {
            System,
            Common,
            Other
        }

        #endregion

        #region 成员属性

        [JsonIgnore]
        public ApplicationTypeEnum ApplicationType
        {
            get { return CLRHelper.GetEnum<ApplicationTypeEnum>(this.Type, ApplicationTypeEnum.Common); }
            set { this.Type = value.ToString(); }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique(Application.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的编号 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 创建应用(将同时生成一条Type为1的权限信息)
        /// </summary>
        [ActiveRecordTransaction]
        public void DoCreate()
        {
            this.Status = 1;

            if (String.IsNullOrEmpty(this.Type))
            {
                this.ApplicationType = ApplicationTypeEnum.Common;
            }

            this.DoValidate();

            this.CreateDate = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();

            Auth auth = new Auth();
            auth.CreateByApplication(this);
        }

        /// <summary>
        /// 修改应用(同时修改类型为1，对应权限的名称)
        /// </summary>
        [ActiveRecordTransaction]
        public void DoUpdate()
        {
            this.DoValidate();

            Auth[] auths = this.GetRelatedAuth();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();

            if (auths.Length > 0)
            {
                foreach (Auth auth in auths)
                {
                    auth.UpdateByApplication(this);
                }
            }
            else
            {
                Auth auth = new Auth();
                auth.CreateByApplication(this);
            }
        }

        /// <summary>
        /// 删除应用（同时删除类型为1, 对应的权限）
        /// </summary>
        [ActiveRecordTransaction]
        public void DoDelete()
        {
            Auth[] auths = this.GetRelatedAuth();

            foreach (Auth auth in auths)
            {
                auth.DoDelete();
            }

            this.Delete();
        }

        /// <summary>
        /// 获取指定应用区间级别的模块
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<Module> GetModulesByLevel(int from, int to)
        {
            var mdls = new List<Module>();

            if (from >= 0)
            {
                if (from == to)
                {
                    return Module.FindAllByProperty(Module.Prop_PathLevel, from);
                }
                else if (from < to)
                {
                    DetachedCriteria crits = DetachedCriteria.For<Module>();

                    crits.Add(Expression.Eq(Module.Prop_ApplicationID, this.ApplicationID));
                    crits.Add(Expression.Ge(Module.Prop_PathLevel, from));
                    crits.Add(Expression.Le(Module.Prop_PathLevel, to));

                    return Module.FindAll(crits);
                }
            }

            return mdls;
        }

        /// <summary>
        /// 获取相关权限
        /// </summary>
        /// <returns></returns>
        public Auth[] GetRelatedAuth()
        {
            // Type为1 代表应用权限
            Auth[] auths = Auth.FindAllByProperties("Type", 1, "Data", this.ApplicationID);
            return auths;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 由编码获取对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Application Get(string code)
        {
            Application ent = Application.FindFirstByProperties(Application.Prop_Code, code);

            return ent;
        }

        public static Application GetPortalApp()
        {
            var portalApp = Application.Get(PortalAppCode);

            return portalApp;
        }

        /// <summary>
        /// 获取活动状态的应用
        /// </summary>
        /// <returns></returns>
        public static IList<Application> GetActiveApplications()
        {
            IList<Application> apps = Application.FindAll(Expression.Eq(Prop_Status, 1));

            return apps;
        }

        #endregion

    }
}
