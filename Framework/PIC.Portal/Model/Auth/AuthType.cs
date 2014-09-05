// Business class AuthType generated from AuthType
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
using NHibernate.Transform;
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class AuthType
    {
        #region 静态方法

        /// <summary>
        /// 获取类型枚举
        /// </summary>
        public static DataEnum GetAuthTypeEnum()
        {
            DataEnum de = new DataEnum();

            AuthType[] types = AuthType.FindAll();

            foreach (AuthType type in types)
            {
                de.Add(type.AuthTypeID.ToString(), type.Name);
            }

            return de;
        }

        #endregion
    } // OrgAuthType
}

