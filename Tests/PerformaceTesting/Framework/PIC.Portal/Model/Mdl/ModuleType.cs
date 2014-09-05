// Business class ModuleType generated from ModuleType
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
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class ModuleType
    {
        #region 静态方法

        /// <summary>
        /// 获取模块类型枚举
        /// </summary>
        public static DataEnum GetModuleTypeEnum()
        {
            DataEnum de = new DataEnum();

            ModuleType[] sml = ModuleType.FindAll();

            foreach (ModuleType sm in sml)
            {
                de.Add(sm.ModuleTypeID.ToString(), sm.Name);
            }

            return de;
        }

        #endregion

    } // SysModuleType
}

