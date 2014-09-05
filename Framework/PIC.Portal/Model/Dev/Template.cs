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
using PIC.Portal.Template;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class Template
    {
        #region Enums & Classes

        /// <summary>
        /// 实际应用中，模版的类型会经常变化，此处的枚举只是作为参考
        /// </summary>
        public enum TemplateTypeEnum
        {
            Standard,  // 标准模版（包括数据模版和展示模版）
            Data,      // 数据模版（只包含数据模版）
            Code,      // 编码模版（一般用于生成表单或记录编号等）
            SN,         // 序列号模版（一般用于自增长的序列号等）
            Custom     // 自定义模版
        }

        /// <summary>
        /// 配置信息编辑器类型
        /// </summary>
        public enum ConfigEditorTypeEnum
        {
            Standard,   // 标准模版编辑器，如果模版类型为标准模版，则采用此编辑器
            Data,       // 数据模版编辑器，若模版类型为数据模版，则采用此编辑器
            Code,       // 编码模版编辑器，若模版类型为编码模版，则采用此编辑器
            SN,         // 序列号模版编辑器，若模版类型为序列号模版，则采用此编辑器
            Template,   // 由模版定义的编辑器, 若模版类型为自定义，则根据类别配置信息采用此编辑器或自定义编辑器
            Custom      // 自定义模版编辑器
        }

        #endregion

        #region 成员变量

        #endregion

        #region 成员属性

        /// <summary>
        /// 模版类型
        /// </summary>
        public TemplateTypeEnum TemplateType
        {
            get { return CLRHelper.GetEnum<TemplateTypeEnum>(this.Type, TemplateTypeEnum.Custom); }
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
            if (!this.IsPropertyUnique(Parameter.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的 编码 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(TemplateID))
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

            this.CreatedDate = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();
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
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        /// <summary>
        /// 获取模版配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public TemplateConfig GetConfig()
        {
            TemplateConfig tmplCfg = null;

            switch (this.TemplateType)
            {
                case TemplateTypeEnum.Standard:
                    tmplCfg = JsonHelper.GetObject<StandardTemplateConfig>(this.Config);
                    break;
                case TemplateTypeEnum.Data:
                    tmplCfg = JsonHelper.GetObject<DataTemplateConfig>(this.Config);
                    break;
                case TemplateTypeEnum.Code:
                    tmplCfg = JsonHelper.GetObject<CodeTemplateConfig>(this.Config);
                    break;
                case TemplateTypeEnum.SN:
                    tmplCfg = JsonHelper.GetObject<SNTemplateConfig>(this.Config);
                    break;
            }

            return tmplCfg;
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="ctxParams"></param>
        /// <returns></returns>
        public string RenderString(EasyDictionary ctxParams = null)
        {
            var tmplStr = this.Config;

            var tmplCfg = this.GetConfig();

            if (tmplCfg != null)
            {
                tmplStr = tmplCfg.RenderString(ctxParams);

                // 若模版类型为序列号，则需要对配置进行保存，以保存自增长信息
                if (tmplCfg is SNTemplateConfig)
                {
                    this.Config = JsonHelper.GetJsonString(tmplCfg);

                    this.DoUpdate();
                }
            }

            return tmplStr;
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			Template[] tents = Template.FindAll(Expression.In("TemplateID", args));

			foreach (Template tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Template Get(string code)
        {
            Template ent = Template.FindFirstByProperties(Template.Prop_Code, code);

            return ent;
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="ctxParams"></param>
        /// <returns></returns>
        public static string RenderString(string code, EasyDictionary ctxParams = null)
        {
            var tmpl = Model.Template.Get(code);

            return tmpl.RenderString(ctxParams);
        }
        
        #endregion

    } // Template
}


