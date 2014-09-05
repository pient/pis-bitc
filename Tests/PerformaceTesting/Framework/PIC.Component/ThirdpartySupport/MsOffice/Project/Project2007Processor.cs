using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    /// <summary>
    /// Project 处理器
    /// </summary>
    public class Project2007Processor : ProjectProcessor
    {
        #region 成员变量

        public const string PRJ2007_XML_SCHEMA_PATH = "ThirdpartySupport.MsOffice.Project.mspdi_pj12.xsd";

        #endregion

        #region 构造析构函数

        internal Project2007Processor()
        {

        }

        #endregion


        #region 公共方法

        #endregion

        #region 私有函数

        /// <summary>
        /// 获取Xml结构流
        /// </summary>
        /// <returns></returns>
        protected override Stream GetXmlSchemaStream()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            string schemaPath = String.Format("{0}.{1}", asm.GetName().Name, PRJ2007_XML_SCHEMA_PATH);
            Stream st = asm.GetManifestResourceStream(schemaPath);

            return st;
        }

        #endregion
    }
}
