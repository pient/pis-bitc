
using System;
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
	public partial class Parameter
    {
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

        #endregion

        #region 静态成员

        /// <summary>
        /// 由编码获取Parameter
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Parameter Get(string code)
        {
            Parameter[] tents = Parameter.FindAllByProperty(Parameter.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }

        public static string GetValue(string code, string def)
        {
            string rtn = def;

            Parameter p = Parameter.Get(code);

            if (p != null)
            {
                rtn = StringHelper.IsEmptyValue(p.Value, def);
            }

            return rtn;
        }

        /// <summary>
        /// 由编码获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T GetValue<T>(string code, T def)
        {
            T rtn = def;

            Parameter p = Parameter.Get(code);

            if (p != null)
            {
                rtn = CLRHelper.ConvertValue<T>(p.Value, def);
            }

            return rtn;
        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        [ActiveRecordTransaction]
        public static void DoBatchDelete(params object[] args)
        {
			Parameter[] tents = Parameter.FindAllByPrimaryKeys(args);

			foreach (Parameter tent in tents)
			{
				tent.DoDelete();
			}
        }
        
        #endregion

    } // SysParameter
}


