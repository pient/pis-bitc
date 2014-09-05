using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    /// <summary>
    /// Project 处理器
    /// </summary>
    public class Project2003Processor : ProjectProcessor
    {
        #region 私有函数

        /// <summary>
        /// 获取Xml结构流
        /// </summary>
        /// <returns></returns>
        protected override Stream GetXmlSchemaStream()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
