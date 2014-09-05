using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace PIC.Data
{
    /// <summary>
    /// DataList 的摘要说明。
    /// </summary>
    public class DataCollection : DataNode
    {
        #region 构造函数

        /// <summary>
        /// 空的DataList构造函数
        /// </summary>
        public DataCollection()
            : base()
        {
            this.Type = DataNodeType.Collection;
            this.Init();
        }

        /// <summary>
        /// 根据XML字符串构造DataList
        /// </summary>
        /// <param name="xmlsrc"></param>
        public DataCollection(string xmlsrc)
            : base(xmlsrc)
        {
            this.Type = DataNodeType.Collection;
        }

        /// <summary>
        /// 根据XmlElement构造DataList
        /// </summary>
        /// <param name="ele"></param>
        public DataCollection(XmlElement ele)
            : base(ele)
        {
            this.Type = DataNodeType.Collection;
        }

        /// <summary>
        /// 根据DataList构造新的DataList
        /// </summary>
        /// <param name="ele"></param>
        public DataCollection(DataCollection dataList)
            : base(dataList.ToString())
        {
            this.Type = DataNodeType.Collection;
        }

        /// <summary>
        /// 根据XmlDocument和XmlElement构造DataForm
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        internal DataCollection(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
            this.Type = DataNodeType.Collection;
        }
        #endregion

        #region 共有方法

        public DataElement GetElementById(string id)
        {
            XmlElement ele = (XmlElement)this.XmlEle.SelectSingleNode("*[@Id='" + id + "']");
            if (ele == null) return null;
            return new DataElement(this.XmlDoc, ele);
        }

        /// <summary>
        /// 新建子节点
        /// </summary>
        /// <returns></returns>
        public new DataElement NewElement()
        {
            XmlElement node = this.XmlDoc.CreateElement("element");
            this.XmlEle.AppendChild(node);
            return new DataElement(this.XmlDoc, node);
        }

        /// <summary>
        /// 根据index取得子节点DataElement的名称为name的属性值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(int index, string name)
        {
            XmlElement node = (XmlElement)this.XmlEle.ChildNodes[index];
            if (node == null) return null;
            string value = node.GetAttribute(name);
            if (value == null) return null;
            return StringHelper.IsNullValue(value);
        }

        /// <summary>
        /// 根据index设置子节点DataElement的名称为name的属性值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue(int index, string name, string value)
        {
            XmlElement node = (XmlElement)this.XmlEle.ChildNodes[index];
            if (node == null) return;
            node.SetAttribute(name, value);
        }

        /// <summary>
        /// 根据name克隆DataList对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new DataCollection Clone(bool includeChild)
        {
            XmlElement node = (XmlElement)this.CloneNode(includeChild);
            return new DataCollection(this.XmlDoc, node);
        }

        /// <summary>
        /// 根据name克隆DataList对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataCollection Clone(string name)
        {
            XmlElement node = (XmlElement)this.CloneNode(name);
            return new DataCollection(this.XmlDoc, node);
        }

        /// <summary>
        /// 对当前对象的XmlElement进行排序
        /// </summary>
        /// <param name="columnName"></param>
        public void Sort(string columnName, bool isAsc)
        {
            XmlElement ele = this.XmlEle;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < ele.ChildNodes.Count; i++)
            {
                arrayList.Add(ele.ChildNodes[i]);
            }

            arrayList.Sort(new Comparer(columnName, isAsc));

            for (int i = 0; i < arrayList.Count; i++)
            {
                ele.AppendChild((XmlNode)arrayList[i]);
            }
        }

        #endregion
    }


    public class Comparer : IComparer
    {
        private string columnName;
        private bool isAsc;

        #region IComparer 构造函数

        public Comparer(string columnName, bool isAsc)
        {
            this.columnName = columnName;
            this.isAsc = isAsc;
        }
        #endregion

        #region IComparer 成员

        public int Compare(object x, object y)
        {
            // TODO:  添加 Comparer.Compare 实现
            int iResult = 0;
            XmlElement diA = (XmlElement)x;
            XmlElement diB = (XmlElement)y;

            if (this.isAsc)
            {
                iResult = diA.GetAttribute(this.columnName).CompareTo(diB.GetAttribute(this.columnName));
            }
            else
            {
                iResult = diB.GetAttribute(this.columnName).CompareTo(diA.GetAttribute(this.columnName));
            }

            return iResult;
        }
        #endregion
    }
}
