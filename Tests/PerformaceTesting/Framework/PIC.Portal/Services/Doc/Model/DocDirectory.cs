﻿using System;
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
	
namespace PIC.Doc.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class DocDirectory
    {
        #region 成员变量

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
                if (_IsLeaf == null && !this.DirectoryID.IsNullOrEmpty())
                {
                    bool isLeaf = !DocDirectory.Exists(Expression.Eq(DocDirectory.Prop_ParentID, this.DirectoryID));
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
            if (!this.IsPropertyUnique(DocDirectory.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (DirectoryID.IsNullOrEmpty())
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
            if (this.Parent != null)
            {
                this.ModuleID = this.Parent.ModuleID;

                this.Status = (this.Status ?? this.Parent.Status);
            }

            this.OwnerID = UserInfo.UserID;
            this.OwnerName = UserInfo.Name;

            this.DoValidate();

            this.CreatedDate = DateTime.Now;

            // 事务开始
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

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        public DocModule GetDocModule()
        {
            var mdl = DocModule.Find(this.ModuleID);

            return mdl;
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			DocDirectory[] tents = DocDirectory.FindAll(Expression.In("DirectoryID", args));

			foreach (DocDirectory tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DocDirectory Get(string code)
        {
            DocDirectory ent = DocDirectory.FindFirstByProperties(DocDirectory.Prop_Code, code);

            return ent;
        }
        
        #endregion

    } // DocDirectory
}

