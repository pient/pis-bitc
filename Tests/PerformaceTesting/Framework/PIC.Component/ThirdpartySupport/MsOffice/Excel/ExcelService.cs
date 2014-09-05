using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    // Ms Office Excel数据处理
    public class ExcelService : IDisposable
    {
        #region 私有成员

        private static ExcelService service;

        #endregion

        #region 构造析构函数

        /// <summary>
        /// 单体模式
        /// </summary>
        private ExcelService()
        {
        }

        ~ExcelService()
        {
            this.Close();
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 服务实例
        /// </summary>
        internal static ExcelService Instance
        {
            get
            {
                if (service == null)
                {
                    service = new ExcelService();
                }

                return service;
            }
        }

        /// <summary>
        /// 获取Excel处理器
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static ExcelProcessor GetProcessor(string filepath)
        {
            return new ExcelProcessor(filepath);
        }

        /// <summary>
        /// 获取Excel处理器
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static ExcelProcessor GetProcessor(string filepath, string extProp)
        {
            return new ExcelProcessor(filepath, extProp);
        }

        /// <summary>
        /// 从DataTable读取数据到Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sw"></param>
        public static void WriteExcel(DataTable dt, StreamWriter w)
        {
            try
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    w.Write(dt.Columns[i]);
                    w.Write(' ');
                }

                w.Write(" ");

                object[] values = new object[dt.Columns.Count];
                foreach (DataRow dr in dt.Rows)
                {
                    values = dr.ItemArray;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        w.Write(values[i]);
                        w.Write(' ');
                    }
                    w.Write(" ");
                }
                w.Flush();
                w.Close();
            }
            finally
            {
                w.Close();
            }
        }

        #endregion

        #region 私有方法

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
