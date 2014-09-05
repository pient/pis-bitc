using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    // Ms Office Word等数据处理
    public class WordService : IDisposable
    {
        #region 私有成员

        private static WordService service;

        #endregion

        #region 构造析构函数

        /// <summary>
        /// 单体模式
        /// </summary>
        private WordService()
        {
        }

        ~WordService()
        {
            this.Close();
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 服务实例
        /// </summary>
        internal static WordService Instance
        {
            get
            {
                if (service == null)
                {
                    service = new WordService();
                }

                return service;
            }
        }

        /// <summary>
        /// 获取Word处理器
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static WordProcessor GetProcessor(string filepath)
        {
            return new WordProcessor(filepath);
        }

        /// <summary>
        /// 获取Word处理器
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static WordProcessor GetProcessor(string filepath, string extProp)
        {
            return new WordProcessor(filepath, extProp);
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
