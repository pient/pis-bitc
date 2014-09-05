// Business class Module generated from Module
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class Module
    {
        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique(Auth.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的编号 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        [ActiveRecordTransaction]
        protected override void DoCreate()
        {
            this.DoValidate();

            if (String.IsNullOrEmpty(this.ApplicationID))
            {
                Application app = Application.Get("PORTAL");

                this.ApplicationID = app.ApplicationID;
            }

            this.CreateDate = DateTime.Now;

            // 事务开始
            base.DoCreate();

            Auth auth = new Auth();
            auth.CreateByModule(this);
        }

        /// <summary>
        /// 修改模块(同时修改类型为1，对应权限的名称)
        /// </summary>
        /// <returns></returns>
        [ActiveRecordTransaction]
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            Auth[] auths = this.GetRelatedAuth();

            this.UpdateAndFlush();

            if (auths.Length > 0)
            {
                foreach (Auth auth in auths)
                {
                    auth.UpdateByModule(this);
                }
            }
            else
            {
                Auth auth = new Auth();
                auth.CreateByModule(this);
            }
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        public void DoDelete()
        {
            DataHelper.ExecSp("usp_Mdl_DeleteModule", "@_ModuleID", this.ModuleID);
        }

        /// <summary>
        /// 获取模块所属的应用
        /// </summary>
        /// <returns></returns>
        public Application OwnerApplication()
        {
            if (!String.IsNullOrEmpty(ApplicationID))
            {
                return Application.Find(this.ApplicationID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取关联权限
        /// </summary>
        /// <returns></returns>
        public Auth[] GetRelatedAuth()
        {
            // Type为1 代表应用权限
            Auth[] auths = Auth.FindAllByProperties("Type", 1, "ModuleID", this.ModuleID);
            return auths;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 有模块键获取模块
        /// </summary>
        /// <param name="code"></param>
        public static Module FindByCode(string code)
        {
            Module mdl = Module.FindFirstByProperties("Code", code);
            return mdl;
        }

        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            Module[] tents = Module.FindAll(Expression.In(Prop_ModuleID, args));

            foreach (Module tent in tents)
            {
                tent.DoDelete();
            }
        }

        #endregion

    } // Module
}

