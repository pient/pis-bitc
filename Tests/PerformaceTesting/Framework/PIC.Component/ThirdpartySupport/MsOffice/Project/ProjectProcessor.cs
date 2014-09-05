using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    /// <summary>
    /// Project 处理器
    /// </summary>
    public abstract class ProjectProcessor : IDisposable
    {
        #region 成员属性

        private Stream _XmlSchemaStream = null;

        #endregion

        #region 属性

        /// <summary>
        /// 获取Xml架构流
        /// </summary>
        public virtual Stream XmlSchemaStream
        {
            get
            {
                if (_XmlSchemaStream == null)
                {
                    _XmlSchemaStream = GetXmlSchemaStream();
                }

                return _XmlSchemaStream;
            }
        }

        #endregion

        #region 构造析构函数

        internal ProjectProcessor()
        {

        }

        ~ProjectProcessor()
        {
            this.Close();
        }

        #endregion

        #region 公共函数

        #region 获取DataSet数据

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(Stream stream)
        {
            //DataSet ds = GetSchemaDataSet();
            //ds.ReadXml(stream, XmlReadMode.IgnoreSchema); // 只读取符合Schema的数据

            DataSet ds = new DataSet();
            ds.ReadXml(stream);

            return ds;
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string fileName)
        {
            // DataSet ds = GetSchemaDataSet();
            // ds.ReadXml(fileName, XmlReadMode.IgnoreSchema); // 只读取符合Schema的数据

            DataSet ds = new DataSet();
            ds.ReadXml(fileName);

            return ds;
        }

        /// <summary>
        /// 获取空的已加载Schema的DataSet
        /// </summary>
        /// <returns></returns>
        public virtual DataSet GetSchemaDataSet()
        {
            DataSet ds = new DataSet();
            XmlSchemaStream.Position = 0;

            ds.ReadXmlSchema(XmlSchemaStream);

            XmlSchemaStream.Position = 0;

            return ds;
        }

        #endregion

        #endregion

        #region 私有函数

        protected abstract Stream GetXmlSchemaStream();

        /// <summary>
        /// 关闭处理器
        /// </summary>
        protected virtual void Close()
        {
            if (_XmlSchemaStream != null)
            {
                _XmlSchemaStream.Dispose();
            }
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
