using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Data
{
    /// <summary>
    /// DataElement 的摘要说明。
    /// </summary>
    public class DataElement
    {
        public XmlDocument XmlDoc;

        public XmlElement XmlEle;

        public DataNodeType Type = DataNodeType.Element;

        /// <summary>
        /// 构造空的DataElement
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        public DataElement()
        {
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.LoadXml("<element />");
            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据一个XmlDocument和一个XmlElement构造DataElement
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        public DataElement(XmlDocument doc, XmlElement ele)
        {
            if (doc != null) this.XmlDoc = doc;
            if (ele != null) this.XmlEle = ele;
        }

        #region 共有方法

        /// <summary>
        /// 根据index获得节点值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetAttr(int index)
        {
            string value = null;
            XmlNode node = this.XmlEle.Attributes.Item(index);
            if (node != null) value = StringHelper.IsNullValue(node.Value);
            return value;
        }

        /// <summary>
        /// 根据属性name获得节点值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAttr(string name)
        {
            string value = null;
            value = this.XmlEle.GetAttribute(name);

            if (value.Trim() != "")
            {
                value = StringHelper.IsNullValue(value);
            }

            return value;
        }

        /// <summary>
        /// 根据index设置节点值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetAttr(int index, string value)
        {
            XmlNode node = this.XmlEle.Attributes.Item(index);
            node.Value = value;
        }

        /// <summary>
        /// 根据属性name设置节点值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttr(string name, string value)
        {
            this.XmlEle.SetAttribute(name, value);
        }

        /// <summary>
        /// 根据index删除节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAttr(int index)
        {
            XmlNode node = this.XmlEle.Attributes.Item(index);
            this.XmlEle.Attributes.RemoveNamedItem(node.Name);
        }

        /// <summary>
        /// 根据属性name删除节点
        /// </summary>
        /// <param name="name"></param>
        public void RemoveAttr(string name)
        {
            this.XmlEle.Attributes.RemoveNamedItem(name);
        }

        /// <summary>
        /// 根据index获得节点名称
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetAttrName(int index)
        {
            return this.XmlEle.Attributes.Item(index).Name;
        }

        /// <summary>
        /// 获得属性节点数量
        /// </summary>
        /// <returns></returns>
        public int GetAttrCount()
        {
            return this.XmlEle.Attributes.Count;
        }

        /// <summary>
        /// 获得根节点名称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.XmlEle.Name;
        }

        /// <summary>
        /// 获得包含本对象的容器
        /// </summary>
        /// <returns></returns>
        public DataContainer GetDataContainer()
        {
            XmlElement ele = this.XmlEle;

            for (int i = 0; i < 10; i++)
            {
                if (ele == null || ele.Name.ToLower() == "container")
                {
                    break;
                }

                ele = (XmlElement)ele.ParentNode;
            }

            if (ele == null) return null;
            return new DataContainer(this.XmlEle);
        }

        /// <summary>
        /// 获得本节点的值
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return StringHelper.IsNullValue(this.XmlEle.InnerText);
        }

        /// <summary>
        /// 设置本节点的值
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(string value)
        {
            this.XmlEle.InnerText = value;
        }

        /// <summary>
        /// 获得本节点的XML
        /// </summary>
        /// <returns></returns>
        public string GetXmlValue()
        {
            return this.XmlEle.InnerXml;
        }

        /// <summary>
        /// 设置本节点的XML
        /// </summary>
        /// <param name="xmlsrc"></param>
        public void SetXmlValue(string xmlsrc)
        {
            this.XmlEle.InnerXml = xmlsrc;
        }

        /// <summary>
        /// 将本对象转换成为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.XmlEle.OuterXml;
        }

        public DataElement Clone(bool includeChild)
        {
            XmlElement node = (XmlElement)this.XmlEle.CloneNode(includeChild);
            return new DataElement(this.XmlDoc, this.XmlEle);
        }

        #endregion
    }
}
