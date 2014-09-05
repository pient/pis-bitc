// Business class Auth generated from Auth
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class Auth
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
                if (_IsLeaf == null && !String.IsNullOrEmpty(this.AuthID))
                {
                    bool isLeaf = !Auth.Exists("ParentID = ?", this.AuthID);
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
            if (!this.IsPropertyUnique(Auth.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的编号 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        protected override void DoCreate()
        {
            this.DoValidate();

            this.CreateDate = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改模块(同时修改类型为2，对应权限的名称)
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        public void DoDelete()
        {
            base.DeleteAndFlush();
        }

        /// <summary>
        /// 由系统应用创建权限
        /// </summary>
        public void CreateByApplication(string appid)
        {
            Application app = Application.Find(appid);

            this.CreateByApplication(app);
        }

        /// <summary>
        /// 由系统应用创建权限
        /// </summary>
        public void CreateByApplication(Application app)
        {
            this.SetDataByApplication(app);  // 根据对应的SysApplication设置数据

            this.DoCreate();
        }

        /// <summary>
        /// 由系统应用更新权限
        /// </summary>
        public void UpdateByApplication(string appid)
        {
            Application app = Application.Find(appid);

            this.UpdateByApplication(app);
        }

        /// <summary>
        /// 由系统应用更新权限
        /// </summary>
        public void UpdateByApplication(Application app)
        {
            this.SetDataByApplication(app);  // 根据对应的SysApplication设置数据

            this.DoUpdate();
        }

        /// <summary>
        /// 由系统模块创建权限
        /// </summary>
        public void CreateByModule(string mdlid)
        {
            Module mdl = Module.Find(mdlid);

            this.CreateByModule(mdl);
        }

        /// <summary>
        /// 由系统模块创建权限
        /// </summary>
        public void CreateByModule(Module mdl)
        {
            this.SetDataByModule(mdl);  // 根据对应的Module设置数据

            this.DoCreate();
        }

        /// <summary>
        /// 由系统模块更新权限
        /// </summary>
        public void UpdateByModule(string mdlid)
        {
            Module mdl = Module.Find(mdlid);
            this.UpdateByModule(mdl);
        }

        /// <summary>
        /// 由系统模块更新权限
        /// </summary>
        public void UpdateByModule(Module mdl)
        {
            this.SetDataByModule(mdl);  // 根据对应的Module设置数据

            this.DoUpdate();
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 由编码获取Auth
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Auth Get(string code)
        {
            Auth[] tents = Auth.FindAllByProperty(Auth.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }

        /// <summary>
        /// 获取某个类型权限的根权限
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Auth GetRootAuth(int type)
        {
            Auth rootAuth = Auth.FindFirst(
                Expression.IsNull(Auth.Prop_ParentID),
                Expression.Eq(Auth.Prop_Type, type));

            return rootAuth;
        }

        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            Auth[] tents = Auth.FindAllByPrimaryKeys(args);

            foreach (Auth tent in tents)
            {
                tent.DoDelete();
            }
        }

        /// <summary>
        /// 由ID集合获取权限集合
        /// </summary>
        /// <param name="authIDs"></param>
        /// <returns></returns>
        public static IList GetAuthList(ICollection authIDs)
        {
            IList myAuthIDs = null;

            if (authIDs is JArray)
            {
                JArray arrAuths = authIDs as JArray;
                myAuthIDs = new List<string>(arrAuths.Values<string>());
            }
            else
            {
                myAuthIDs = authIDs as IList;
            }

            return myAuthIDs;
        }

        /// <summary>
        /// 由ID集合获取权限集合
        /// </summary>
        /// <param name="authIDs"></param>
        /// <returns></returns>
        public static ICollection<Auth> GetAuthByIDs(ICollection authIDs)
        {
            ICollection myAuthIDs = null;

            if (authIDs is JArray)
            {
                JArray arrAuths = authIDs as JArray;
                myAuthIDs = new List<string>(arrAuths.Values<string>());
            }
            else
            {
                myAuthIDs = authIDs;
            }

            Auth[] tAuths = Auth.FindAll(Expression.In("AuthID", myAuthIDs));

            return tAuths;
        }

        #endregion

        #region 支持方法

        private void SetDataByApplication(Application app)
        {
            this.Name = app.Name;
            this.Code = String.Format("AUTH_APP_{0}", app.Code);
            this.Data = app.ApplicationID;
            this.Path = ".";
            this.PathLevel = 0;
            this.ParentID = null;
            this.Type = 1;  // 1为系统入口权限
            this.Description = String.Format("应用 {0} 访问权限", app.Name);
        }

        private void SetDataByModule(Module mdl)
        {
            Auth[] pAuths;
            Auth pAuth = null;

            // 获取父权限（与系统模块对应）
            if (!String.IsNullOrEmpty(mdl.ParentID))
            {
                pAuths = Auth.FindAllByProperties("ModuleID", mdl.ParentID);
            }
            else
            {
                pAuths = Auth.FindAllByProperties("Data", mdl.ApplicationID);
            }

            if (pAuths.Length > 0)
            {
                pAuth = pAuths[0];
                this.ParentID = pAuth.AuthID;
                this.Path = String.Format("{0}.{1}", pAuth.Path, pAuth.AuthID);
                this.PathLevel = (pAuth.PathLevel == null ? 0 : (pAuth.PathLevel + 1));
            }

            this.Name = mdl.Name;
            this.Code = String.Format("AUTH_MDL_{0}", mdl.Code);
            this.Type = 1;  // 1为系统入口权限
            this.SortIndex = mdl.SortIndex;
            this.ModuleID = mdl.ModuleID;
            this.Description = String.Format("模块 {0} 访问权限", mdl.Name);
        }

        #endregion
    } // OrgAuth
}

