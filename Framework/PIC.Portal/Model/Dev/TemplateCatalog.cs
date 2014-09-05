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
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class TemplateCatalog
    {
        #region 成员变量

        #endregion

        #region 成员属性
        
        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            /*if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }*/
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(CatalogID))
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
        protected override void DoCreate()
        {
            this.DoValidate();

            this.CreatedDate = DateTime.Now;

            base.DoCreate();
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

        [ActiveRecordTransaction]
        public override void Delete()
        {
            if (ExistsTempalteItem())
            {
                throw new MessageException("不能执行删除操作，此类型下存在模版，请清空此类型下所有模版，再进行删除操作.");
            }
            
            // 删除本节点及子节点以及相关子节点类型
            string sql = String.Format("DELETE FROM Template WHERE CatalogID IN (SELECT CatalogID FROM TemplateCatalog WHERE Path LIKE '%{0}%' OR CatalogID = '{0}')"
                + "DELETE FROM TemplateCatalog WHERE Path LIKE '%{0}%' OR CatalogID = '{0}'; ", this.CatalogID);

            if (!String.IsNullOrEmpty(this.ParentID))
            {
                sql += String.Format("UPDATE TemplateCatalog SET IsLeaf = 1 WHERE CatalogID = '{0}' AND NOT EXISTS (SELECT CatalogID FROM TemplateCatalog WHERE ParentID = '{0}')", this.ParentID);
            }

            DataHelper.QueryValue(sql);
        }

        /// <summary>
        /// 此分类下是否包含模版节点
        /// </summary>
        /// <returns></returns>
        public bool ExistsTempalteItem()
        {
            var exists = Template.Exists(Expression.Eq(Prop_CatalogID, this.CatalogID));

            return exists;
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			TemplateCatalog[] tents = TemplateCatalog.FindAll(Expression.In("CatalogID", args));

			foreach (TemplateCatalog tent in tents)
			{
				tent.Delete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TemplateCatalog Get(string code)
        {
            TemplateCatalog ent = TemplateCatalog.FindFirstByProperties(TemplateCatalog.Prop_Code, code);

            return ent;
        }
        
        #endregion

    } // TemplateCatalog
}


