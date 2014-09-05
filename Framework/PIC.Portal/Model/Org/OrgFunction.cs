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
	public partial class OrgFunction
    {
        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique(OrgFunction.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(FunctionID))
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

            this.CreateDate = DateTime.Now;

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
            this.DeleteAndFlush();
        }

        /// <summary>
        /// 跟据ID增加角色
        /// </summary>
        /// <param name="rids"></param>
        public void AddRoleByIDs(OrgFunctionRole funcRoleTmpl, params string[] rids)
        {
            using (new SessionScope())
            {
                // 过滤掉已经存在的角色
                var existsRoles = OrgFunctionRole.FindAll(
                    Expression.Eq(OrgFunctionRole.Prop_FunctionID, this.FunctionID),
                    Expression.In(OrgFunctionRole.Prop_RoleID, rids));

                var existsIDs = existsRoles.Select(r => r.RoleID);
                var filteredIDs = rids.Where(r => !existsIDs.Contains(r));

                foreach (var rid in filteredIDs)
                {
                    var funcRole = new OrgFunctionRole()
                    {
                        FunctionID = this.FunctionID,
                        RoleID = rid,
                        Status = 1
                    };

                    // 根据模板调整相关属性
                    if (funcRoleTmpl != null)
                    {
                        funcRole.Tag = funcRoleTmpl.Tag;
                        funcRole.Description = funcRoleTmpl.Description;
                        funcRole.Status = funcRoleTmpl.Status;
                        funcRole.EditStatus = funcRoleTmpl.EditStatus;
                    }

                    funcRole.DoCreate();
                }

                // 根据模板调整相关属性
                if (funcRoleTmpl != null)
                {
                    foreach (var role in existsRoles)
                    {
                        role.Tag = funcRoleTmpl.Tag;
                        role.Description = funcRoleTmpl.Description;
                        role.Status = funcRoleTmpl.Status;
                        role.EditStatus = funcRoleTmpl.EditStatus;

                        role.DoUpdate();
                    }
                }
            }
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            OrgFunction[] tents = OrgFunction.FindAll(Expression.In(OrgFunction.Prop_FunctionID, args));

			foreach (OrgFunction tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OrgFunction Get(string code)
        {
            OrgFunction ent = OrgFunction.FindFirstByProperties(OrgFunction.Prop_Code, code);

            return ent;
        }
        
        #endregion

    } // OrgFunction
}


