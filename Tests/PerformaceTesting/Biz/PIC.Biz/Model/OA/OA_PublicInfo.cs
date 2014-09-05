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
	public partial class OA_PublicInfo
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

            this.Clicks = 0;
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
        /// 发布信息
        /// </summary>
        public void DoPublish()
        {
            this.PublishDate = DateTime.Now;
            this.IsExpired = "N";
            this.Status = "Published";

            this.DoSave();
        }

        /// <summary>
        /// 撤销发布
        /// </summary>
        public void DoRecall()
        {
            if (this.Status != "Published")
            {
                throw new MessageException("当前信息未发布，不能执行撤销。");
            }

            this.Status = "Recalled";

            this.DoSave();
        }

        #endregion
        
        #region 静态成员

        /// <summary>
        /// 查看信息，返回对象信息，并使点击计数增加 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static OA_PublicInfo Read(string id)
        {
            OA_PublicInfo ent = OA_PublicInfo.Find(id);

            if (ent.Clicks == null)
            {
                ent.Clicks = 0;
            }

            ent.Clicks++;

            ent.DoUpdate();

            return ent;
        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			OA_PublicInfo[] tents = OA_PublicInfo.FindAll(Expression.In("Id", args));

			foreach (OA_PublicInfo tent in tents)
			{
				tent.DoDelete();
			}
        }
        
        #endregion

    } // OA_PublicInfo
}


