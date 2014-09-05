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
using PIC.Portal.Workflow;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class WfDefine
    {
        #region 枚举

        /// <summary>
        /// 流程定义状态枚举
        /// </summary>
        public enum StatusEnum
        {
            New,
            Published,
            Disabled,
            Unknown
        }

        #endregion

        #region 成员变量

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public StatusEnum DefineStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status, StatusEnum.Unknown); }
            set { this.Status = value.ToString(); }
        }

        [NonSerialized]
        protected WfDefineConfig defineConfig;

        #endregion

        #region 属性

        /// <summary>
        /// 定义配置
        /// </summary>
        [JsonIgnore]
        public WfDefineConfig DefineConfig
        {
            get
            {
                if (defineConfig == null)
                {
                    if (String.IsNullOrEmpty(this.Config))
                    {
                        defineConfig = new WfDefineConfig();
                    }
                    else
                    {
                        defineConfig = JsonHelper.GetObject<WfDefineConfig>(this.Config);
                    }
                }

                return defineConfig;
            }

            internal set { this.defineConfig = value; }
        }

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
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(DefineID))
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
        [ActiveRecordTransaction]
        public void DoCreate()
        {
            if (String.IsNullOrEmpty(this.Code))
            {
                this.Code = Model.Template.RenderString("Sys.Code.Str.WfDefine");
            }

            this.DoValidate();

            this.CreatorID = UserInfo.Name;
            this.CreatorName = UserInfo.Name;
            this.CreatedDate = DateTime.Now;

            this.Config = JsonHelper.GetJsonString(this.DefineConfig);

            // 事务开始
            this.CreateAndFlush();

            this.SaveAuth();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        [ActiveRecordTransaction]
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.Config = JsonHelper.GetJsonString(this.DefineConfig);

            this.UpdateAndFlush();

            this.SaveAuth();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            // 有关两实例只能虚拟删除
            if (this.HasInstance())
            {
                this.Status = "Disabled";

                this.UpdateAndFlush();
            }
            else
            {
                // 先删除关联权限
                var auth = this.GetRelatedAuth();

                if (auth != null)
                {
                    auth.DoDelete();
                }

                var cfg = this.GetConfig();

                var flowPath = WfHelper.GetFlowFilePath(cfg.FlowPath);
                var diagramPath = WfHelper.GetFlowFilePath(cfg.FlowDiagramPath);

                if (!String.IsNullOrEmpty(flowPath) && System.IO.File.Exists(flowPath))
                {
                    System.IO.File.Delete(flowPath);
                }

                if (!String.IsNullOrEmpty(diagramPath) && System.IO.File.Exists(diagramPath))
                {
                    System.IO.File.Delete(diagramPath);
                }

                this.Delete();
            }
        }

        /// <summary>
        /// 发布流程定义
        /// </summary>
        public void DoPublish()
        {
            this.DefineStatus = StatusEnum.Published;

            this.Update();
        }

        /// <summary>
        /// 返回定义配置
        /// </summary>
        /// <returns></returns>
        public WfDefineConfig GetConfig()
        {
            var cfgstr = WfHelper.GetWfDataString(this.Config);

            WfDefineConfig cfg = null;

            try
            {
                cfg = JsonHelper.GetObject<WfDefineConfig>(cfgstr);
            }
            catch { }

            if (cfg == null)
            {
                cfg = new WfDefineConfig();
            }

            if (String.IsNullOrEmpty(cfg.FlowDiagramPath))
            {
                if (String.IsNullOrEmpty(cfg.FlowPath) || cfg.FlowPath == ".")
                {
                    cfg.FlowDiagramPath = this.Code;
                }
                else
                {
                    cfg.FlowDiagramPath = cfg.FlowPath.TrimEnd(".xaml");
                }

                cfg.FlowDiagramPath = cfg.FlowDiagramPath + ".jpg";
            }

            return cfg;
        }

        /// <summary>
        /// 获取初始基本信息
        /// </summary>
        /// <returns></returns>
        public FlowBasicInfo GetInitBasicInfo()
        {
            var basicInfo = new FlowBasicInfo();

            var userInfo = PortalService.CurrentUserInfo;

            if (userInfo != null)
            {
                var orgUser = OrgUser.Find(userInfo.UserID);

                basicInfo.ApplicantID = orgUser.UserID;
                basicInfo.ApplicantName = orgUser.Name;

                var dept = orgUser.GetDept();
                if (dept != null)
                {
                    basicInfo.DeptID = dept.GroupID;
                    basicInfo.DeptName = dept.Name;
                }
            }

            basicInfo.DefineCode = this.Code;
            basicInfo.DefineName = this.Name;

            var config = this.GetConfig();
            basicInfo.FormPath = config.FormPath;

            return basicInfo;
        }

        /// <summary>
        /// 获取初始实例
        /// </summary>
        /// <returns></returns>
        public WfInstance GetInitInstance()
        {
            WfInstance instance = new WfInstance();

            instance.DefineID = this.DefineID;
            instance.ApplicationID = this.ApplicationID;
            instance.ModuleID = this.ModuleID;
            instance.Code = this.Code;
            instance.Name = this.Name;
            instance.CreatorID = UserInfo.UserID;
            instance.CreatorName = UserInfo.Name;

            instance.InstanceStatus = WfInstance.StatusEnum.New;
            instance.CreatedTime = DateTime.Now;

            return instance;
        }

        /// <summary>
        /// 由项目定义创建项目实例(默认需要发布后才可以创建实例)
        /// </summary>
        public WfInstance NewInstance()
        {
            WfInstance instance = GetInitInstance();

            instance.DoCreate();

            return instance;
        }

        /// <summary>
        /// 以否已拥有实例
        /// </summary>
        /// <returns></returns>
        public bool HasInstance()
        {
            var has = WfInstance.Exists(Expression.Eq(WfInstance.Prop_DefineID, this.DefineID));

            return has;
        }

        #endregion

        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            var tents = WfDefine.FindAll(Expression.In(WfDefine.Prop_DefineID, args));

			foreach (var tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WfDefine Get(string code)
        {
            var ent = WfDefine.FindFirstByProperties(WfDefine.Prop_Code, code);

            return ent;
        }
        
        #endregion

        #region 支持方法

        /// <summary>
        /// 保存或创建关联权限
        /// </summary>
        private void SaveAuth()
        {
            if (String.IsNullOrEmpty(this.DefineID))
            {
                throw new MessageException("请先保存流程定义信息。");
            }

            var auth = this.GetRelatedAuth();

            if (auth == null)
            {
                Auth pAuth = Auth.GetRootAuth(2);   // 2为流程权限

                auth = this.SetAuthData(new Auth());
                auth.CreateAsChild(pAuth);
            }
            else
            {
                this.SetAuthData(auth);
                auth.DoUpdate();
            }
        }

        /// <summary>
        /// 获取关联权限
        /// </summary>
        /// <returns></returns>
        private Auth GetRelatedAuth()
        {
            var auth = Auth.FindFirst(
                Expression.Eq(Auth.Prop_ModuleID, this.DefineID),
                Expression.Eq(Auth.Prop_Type, 2));

            return auth;
        }

        /// <summary>
        /// 由WfDefine生成新权限
        /// </summary>
        /// <returns></returns>
        private Auth SetAuthData(Auth auth)
        {
            auth.Type = 2;  // 2为流程权限
            auth.ModuleID = this.DefineID;
            auth.Name = this.Name;
            auth.Code = String.Format("AUTH_WF_{0}", this.Code);
            auth.SortIndex = this.SortIndex;
            auth.Description = String.Format("流程 {0} 访问权限", this.Name);

            return auth;
        }

        #endregion

    } // SysWfDefine
}


