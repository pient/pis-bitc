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
	public partial class OrgRoleAuth
    {
        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            var existsEnt = OrgRoleAuth.FindFirst(
                Expression.Eq(OrgRoleAuth.Prop_AuthID, this.AuthID),
                Expression.Eq(OrgRoleAuth.Prop_RoleID, this.RoleID));

            if (existsEnt != null)
            {
                if (existsEnt.RoleAuthID != this.RoleAuthID)
                {
                    throw new MessageException("此角色已拥有指定权限，不能重复添加。");
                }

                existsEnt.Evict();  // 从ActiveRecord中释放缓存
            }

            if (String.IsNullOrEmpty(this.AuthID))
            {
                throw new MessageException("请提供权限。");
            }

            if (String.IsNullOrEmpty(this.RoleID))
            {
                throw new MessageException("请提供角色。");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(RoleAuthID))
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

            this.CreatorID = UserInfo.UserID;
            this.CreatorName = UserInfo.Name;
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

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			OrgRoleAuth[] tents = OrgRoleAuth.FindAll(Expression.In("RoleAuthID", args));

			foreach (OrgRoleAuth tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        
        #endregion

    } // OrgRoleAuth
}


