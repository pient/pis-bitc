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
	
namespace PIC.Doc.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class DocFile : IFileInfo
    {
        #region 成员变量

        public static string Prop_Directory = "Directory";

        private DocDirectory _directory;

        #endregion

        #region 成员属性

        [JsonIgnore]
        [BelongsTo("DirectoryID", Type = typeof(DocDirectory), Access = PropertyAccess.NosetterCamelcaseUnderscore, Insert = false, Update = false, Lazy = FetchWhen.OnInvoke)]
        public DocDirectory Directory
        {
            get { return _directory; }
            set
            {
                if ((_directory == null) || (value == null) || (value.DirectoryID != _directory.DirectoryID))
                {
                    object oldValue = _directory;
                    if (value == null)
                        _directory = null;
                    else
                        _directory = (!value.DirectoryID.IsNullOrEmpty()) ? value : null;

                    RaisePropertyChanged(DocFile.Prop_Directory, oldValue, value);
                }
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
            if (FileID.IsNullOrEmpty())
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
            this.OwnerID = UserInfo.UserID;
            this.OwnerName = UserInfo.Name;

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
			DocFile[] tents = DocFile.FindAll(Expression.In("FileID", args));

			foreach (DocFile tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        
        #endregion

        #region IFileInfo成员

        public MinFileInfo GetMinFileInfo()
        {
            return new MinFileInfo()
            {
                FileID = this.FileID.ToString(),
                Name = this.Name
            };
        }

        #endregion
    } // DocFile
}


