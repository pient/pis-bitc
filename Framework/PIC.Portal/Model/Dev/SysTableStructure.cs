// Business class SysTableStructure generated from SysTableStructure
// Creator: Ray
// Created Date: [2010-06-23]
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
	public partial class SysTableStructure
    {
        #region 静态方法

        /// <summary>
        /// 获取表主键
        /// </summary>
        /// <returns></returns>
        public static string[] GetTablePrimaryKey(string tableName)
        {
            SysTableStructure[] sts = SysTableStructure.FindAllByProperties("TableName", tableName, "IsPrimary", 1);

            return sts.Select(tent => tent.FieldName).ToArray();
        }

        #endregion

    } // SysTableStructure
}

