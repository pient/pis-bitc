using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using Excel = Aspose.Cells;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    /// <summary>
    /// Excel处理器
    /// </summary>
    public class WordProcessor : IDisposable
    {
        #region 成员属性

        private string _filePath;
        private string _extProp = "Word 8.0";

        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        #endregion

        #region 构造析构函数

        internal WordProcessor(string filePath)
        {
            this._filePath = filePath;
        }

        internal WordProcessor(string filePath, string extProp)
            : this(filePath)
        {
            this._extProp = extProp;
        }

        ~WordProcessor()
        {
            this.Close();
        }

        #endregion

        #region 公共函数

        #endregion

        #region 私有函数

        /// <summary>
        /// 关闭应用
        /// </summary>
        private void Close()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
