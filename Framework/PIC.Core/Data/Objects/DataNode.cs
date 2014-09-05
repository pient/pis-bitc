using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Data
{
    /// <summary>
    /// 数据节点类型
    /// </summary>
    [Flags]
    public enum DataNodeType
    {
        Collection, // 数据集合
        Parameter,   // 数据参数
        Node, // 节点元素
        Element // 元素（默认）
    }

    /// <summary>
    /// DataNode 的摘要说明。
    /// </summary>
    public class DataNode : DataElement
    {
        #region 构造函数

        /// <summary>
        /// 空的DataNode构造函数
        /// </summary>
        protected DataNode()
            : base(null, null)
        {
        }

        protected void Init()
        {
            this.XmlDoc = new XmlDocument();

            switch (this.Type)
            {
                case DataNodeType.Collection : 
                    this.XmlDoc.LoadXml("<collection Name=''></collection>"); 
                    break;
                case DataNodeType.Parameter : 
                    this.XmlDoc.LoadXml("<parameter Name=''></parameter>"); 
                    break;
            }

            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据XML字符串构造DataNode
        /// </summary>
        /// <param name="xmlsrc"></param>
        protected DataNode(string xmlsrc)
            : base(null, null)
        {
            this.Type = DataNodeType.Node;

            this.XmlDoc = new XmlDocument();
            this.XmlDoc.LoadXml(xmlsrc);
            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据XmlElement构造DataNode
        /// </summary>
        /// <param name="ele"></param>
        protected DataNode(XmlElement ele)
            : base(null, null)
        {
            this.Type = DataNodeType.Node;

            XmlNode node = ele.CloneNode(true);
            this.XmlDoc = node.OwnerDocument;
            this.XmlEle = (XmlElement)node;
        }

        /// <summary>
        /// 根据XmlDocument和XmlElement构造DataNode
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        protected DataNode(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
            this.Type = DataNodeType.Node;

            if (doc != null) this.XmlDoc = doc;
            if (ele != null) this.XmlEle = ele;
        }

        #endregion

        //----------------------------------------------------------------------

        #region 公有方法

        /// <summary>
        /// 添加子节点
        /// </summary>
        public DataElement AddElement(DataElement ele)
        {
            XmlElement newElement = this.XmlDoc.CreateElement("element");
            XmlNode node = this.XmlEle.AppendChild(newElement);
            ele.XmlEle.CopyAttributesTo(newElement);
            newElement.InnerXml = ele.XmlEle.InnerXml;
            ele.XmlDoc = this.XmlDoc;
            ele.XmlEle = newElement;
            return ele;
        }

        /// <summary>
        /// 新增子节点
        /// </summary>
        public void NewElement() { }

        /// <summary>
        /// 根据index删除节点
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            XmlNode node = this.XmlEle.ChildNodes[index];
            if (node != null) node.ParentNode.RemoveChild(node);
        }

        /// <summary>
        /// 根据传入的DataElement删除节点
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(DataElement obj)
        {
            if (this.XmlEle == obj.XmlEle.ParentNode)
                obj.XmlEle.ParentNode.RemoveChild(obj.XmlEle);
        }

        /// <summary>
        /// 清除此节点下的所有子节点
        /// </summary>
        public virtual void Clear()
        {
            XmlNodeList nodes = this.XmlEle.ChildNodes;
            int count = nodes.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                this.XmlEle.RemoveChild(nodes[i]);
            }
        }

        /// <summary>
        /// 获得子节点的数量
        /// </summary>
        /// <returns></returns>
        public int GetElementCount()
        {
            return this.XmlEle.ChildNodes.Count;
        }

        /// <summary>
        /// 根据index获取DataElement
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataElement GetElement(int index)
        {
            XmlElement node = null;
            node = (XmlElement)this.XmlEle.ChildNodes.Item(index);
            if (node == null) return null;
            return new DataElement(this.XmlDoc, node);
        }

        /// <summary>
        /// 根据XMLxpath获取DataElement
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public DataElement GetElement(string xpath)
        {
            XmlElement node = null;
            node = (XmlElement)this.XmlEle.SelectSingleNode(xpath);
            if (node == null) return null;
            return new DataElement(this.XmlDoc, node);
        }

        /// <summary>
        /// 根据index获取DataElement
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataElement GetElement(string attrName, string attrValue)
        {
            XmlElement node = null;
            node = (XmlElement)this.XmlEle.SelectSingleNode("*[@" + attrName + "='" + attrValue + "']");
            if (node == null) return null;
            return new DataElement(this.XmlDoc, node);
        }

        /// <summary>
        /// 根据XMLxpath获取多个DataElement
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public ArrayList GetElements(string xpath)
        {
            XmlNodeList nodes = null;
            if (xpath == "")
            {
                nodes = this.XmlEle.ChildNodes;
            }
            else
            {
                nodes = this.XmlEle.SelectNodes(xpath);
            }

            ArrayList diary = new ArrayList(nodes.Count);

            for (int i = 0; i < nodes.Count; i++)
            {
                diary.Add(new DataElement(this.XmlDoc, (XmlElement)nodes[i]));
            }

            return diary;
        }

        /// <summary>
        /// 获得节点名称
        /// </summary>
        /// <returns></returns>
        public new string GetName()
        {
            return this.XmlEle.GetAttribute("Name");
        }

        /// <summary>
        /// 设置节电名称
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            this.XmlEle.SetAttribute("Name", name);
        }

        /// <summary>
        /// 根据节点名称克隆新节点
        /// </summary>
        /// <param name="name"></param>
        public XmlNode CloneNode(bool includeChild)
        {
            XmlElement node = (XmlElement)this.XmlEle.CloneNode(includeChild);
            return node;
        }
        /// <summary>
        /// 根据节点名称克隆新节点
        /// </summary>
        /// <param name="name"></param>
        public XmlNode CloneNode(string name)
        {
            XmlElement node = (XmlElement)this.XmlEle.CloneNode(true);
            node.SetAttribute("Name", name);
            return node;
        }

        /// <summary>
        /// 按照节点标签排序
        /// </summary>
        /// <returns></returns>
        public string Sort()
        {
            return null;
        }

        /// <summary>
        /// 按照节点名称排序
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string Sort(string name, string dataType)
        {
            return null;
        }

        #endregion
    }
}
