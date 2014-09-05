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
	public class SysSystem
    {
        #region 私有成员

        public const string SYS_USERID = "SYSTEM";
        public const string SYS_USERNAME = "SYSTEM";
        public const string SYS_SESSIONID_CODE = "System.Inner.SysSessionID";

        #endregion

        #region 静态方法

        /// <summary>
        /// 检测系统是否失效
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsValid()
        {
            bool isValid = false;

            if (DateTime.Now < new DateTime(2013, 5, 1))
            {
                isValid = true;
            }

            //SysEvent evt = SysEvent.FindFirst(Order.Asc("DateTime"));

            //if (evt != null && DateTime.Now < evt.DateTime.Value.AddDays(90))
            //{
            //    isValid = true;
            //}

            return isValid;
        }

        #endregion

    } // PRJ_Charter
}


