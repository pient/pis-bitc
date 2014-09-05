using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Data
{
    public class DataContainer : DataNode
    {
        #region 构造函数

        /// <summary>
        /// 空的DataContainer构造函数
        /// </summary>
        public DataContainer()
            : base(null, null)
        {
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.LoadXml("<container></container>");
            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据XML字符串构造DataContainer
        /// </summary>
        /// <param name="xmlsrc"></param>
        /// <param name="type">字符串格式</param>
        public DataContainer(string xmlsrc, string strFormat)
            : base(null, null)
        {
            this.XmlDoc = new XmlDocument();
            string xmlstr = "";

            switch (strFormat)
            {
                case "": xmlstr = xmlsrc; break;
            }

            this.XmlDoc.LoadXml(xmlsrc);
            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据XML字符串构造DataContainer
        /// </summary>
        /// <param name="xmlsrc"></param>
        /// <param name="type">字符串格式</param>
        public DataContainer(string xmlsrc)
            : base(null, null)
        {
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.LoadXml(xmlsrc);
            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据XML字符串构造DataContainer
        /// </summary>
        /// <param name="dataContainer"></param>
        public DataContainer(DataContainer dataContainer)
            : base(null, null)
        {
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.LoadXml(dataContainer.ToString());
            this.XmlEle = this.XmlDoc.DocumentElement;
        }

        /// <summary>
        /// 根据XmlElement构造DataContainer
        /// </summary>
        /// <param name="ele"></param>
        public DataContainer(XmlElement ele)
            : base(null, null)
        {
            this.XmlDoc = ele.OwnerDocument;
            this.XmlEle = ele;
        }

        #endregion

        public string this[string name]
        {
            get
            {
                return this[name, null];
            }
            set
            {
                this[name, null] = value;
            }
        }

        public string this[string name, string defValue]
        {
            get
            {
                DataParameter dp = (DataParameter)this.Get(DataNodeType.Parameter, name);

                if (dp == null)
                {
                    if (defValue != null)
                    {
                        return defValue;
                    }
                    else
                    {
                        return null;
                    }
                }

                return dp.GetValue();
            }

            set
            {
                DataParameter dp = (DataParameter)this.Get(DataNodeType.Parameter, name);

                if (dp == null)
                {
                    dp = new DataParameter();
                    dp.SetName(name);
                    dp.SetValue(value);

                    this.Add(dp);
                }
                else
                {
                    dp.SetValue(value);
                }
            }
        }

        #region 公有方法

        /// <summary>
        /// 根据index获取DataContainer中的数据元素
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataNode Get(DataNodeType type, int index)
        {
            switch (type)
            {
                case  DataNodeType.Collection : 
                    return this.Collections(index);
                case DataNodeType.Parameter : 
                    return this.Parameters(index);
            }

            return null;
        }

        /// <summary>
        /// 根据name获取DataContainer中的数据元素
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataNode Get(DataNodeType type, string name)
        {
            switch (type)
            {
                case DataNodeType.Collection :
                    return this.Collections(name);
                case DataNodeType.Parameter :
                    return this.Parameters(name);
            }

            return null;
        }

        /// <summary>
        /// 根据index获取XML节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public XmlNode GetXmlNode(DataNodeType type, int index)
        {
            XmlNode node = null;
            string ptag = this.GetTag(type, true);

            node = this.XmlEle.SelectSingleNode(ptag).ChildNodes[index];
            return node;
        }

        /// <summary>
        /// 根据name获取XML节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlNode GetXmlNode(DataNodeType type, string name)
        {
            XmlNode node = null;
            string tag = this.GetTag(type);
            string ptag = this.GetTag(type, true);
            node = this.XmlEle.SelectSingleNode(ptag + "/" + tag + "[@Name='" + name + "']");
            return node;
        }

        /// <summary>
        /// 向本DataContainer中添加数据节点
        /// </summary>
        /// <param name="dnode"></param>
        public void Add(DataNode dnode)
        {
            this.Add(dnode, null);
        }

        public void Add(DataNode dnode, string name)
        {
            string ptag = this.GetTag(dnode.Type, true);
            XmlNode pnode = this.XmlEle.SelectSingleNode(ptag);

            if (pnode == null)
            {
                pnode = this.XmlDoc.CreateElement(ptag);
                this.XmlEle.AppendChild(pnode);
            }

            XmlElement newElement = this.XmlDoc.CreateElement(this.GetTag(dnode.Type));
            XmlNode node = pnode.AppendChild(newElement);
            dnode.XmlEle.CopyAttributesTo(newElement);
            newElement.InnerXml = dnode.XmlEle.InnerXml;

            if (name != null)
            {
                newElement.SetAttribute("Name", name);
            }
        }

        /// <summary>
        /// 在本DataContainer中新建数据节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataNode New(DataNodeType type)
        {
            string ptag = this.GetTag(type, true);

            XmlNode pnode = this.XmlEle.SelectSingleNode(ptag);
            XmlNode node = null;
            DataNode nodeobj = null;

            if (pnode == null)
            {
                pnode = this.XmlDoc.CreateElement(ptag);
                this.XmlEle.AppendChild(pnode);
            }

            switch (type)
            {
                case DataNodeType.Collection:
                    node = (XmlElement)this.XmlDoc.CreateElement("collection");
                    pnode.AppendChild(node);
                    nodeobj = new DataCollection(this.XmlDoc, (XmlElement)node);
                    break;
                case DataNodeType.Parameter:
                    node = (XmlElement)this.XmlDoc.CreateElement("parameter");
                    pnode.AppendChild(node);
                    nodeobj = new DataParameter(this.XmlDoc, (XmlElement)node);
                    break;
            }

            return nodeobj;
        }

        /// <summary>
        /// 根据index获取一个DataCollection
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataCollection Collections(int index)
        {
            XmlNode node = this.GetXmlNode(DataNodeType.Collection, index);
            if (node == null) return null;
            return new DataCollection(this.XmlDoc, (XmlElement)node);
        }

        /// <summary>
        /// 根据name获取一个DataCollection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataCollection Collections(string name)
        {
            XmlNode node = this.GetXmlNode(DataNodeType.Collection, name);
            if (node == null) return null;
            return new DataCollection(this.XmlDoc, (XmlElement)node);
        }

        /// <summary>
        /// 根据index获取一个DataParameter
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataParameter Parameters(int index)
        {
            XmlNode node = this.GetXmlNode(DataNodeType.Parameter, index);
            if (node == null) return null;
            return new DataParameter(this.XmlDoc, (XmlElement)node);
        }

        /// <summary>
        /// 根据name获取一个DataParameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataParameter Parameters(string name)
        {
            XmlNode node = this.GetXmlNode(DataNodeType.Parameter, name);
            if (node == null) return null;
            return new DataParameter(this.XmlDoc, (XmlElement)node);
        }

        /// <summary>
        /// 删除传入的DataNode
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(DataNode objNode)
        {
            if (this.XmlDoc == objNode.XmlDoc)
            {
                objNode.XmlEle.ParentNode.RemoveChild(objNode.XmlEle);
            }
        }

        /// <summary>
        /// 根据index删除DataNode
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void Remove(DataNodeType type, string name)
        {
            XmlNode node = null;
            node = this.GetXmlNode(type, name);
            if (node != null) node.ParentNode.RemoveChild(node);
        }

        /// <summary>
        /// 根据index删除DataNode
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void Remove(DataNodeType type, int index)
        {
            XmlNode node = null;
            node = this.GetXmlNode(type, index);
            if (node != null) node.ParentNode.RemoveChild(node);
        }

        /// <summary>
        /// 清空一个DataContainer
        /// </summary>
        public override void Clear()
        {
            XmlNodeList nodes = this.XmlEle.ChildNodes;
            XmlNode pnode = this.XmlEle;

            int count = nodes.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                pnode.RemoveChild(nodes[i]);
            }
        }

        /// <summary>
        /// 清空一个DataContainer中的一种type的节点
        /// </summary>
        /// <param name="type"></param>
        public void Clear(DataNodeType type)
        {
            string ptag = this.GetTag(type);
            XmlNode pnode = this.XmlEle.SelectSingleNode(ptag);
            XmlNodeList nodes = pnode.ChildNodes;
            int count = nodes.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                pnode.RemoveChild(nodes[i]);
            }
        }

        /// <summary>
        /// 继承的克隆节点方法
        /// </summary>
        /// <param name="deep"></param>
        /// <returns></returns>
        public new DataContainer Clone(bool deep)
        {
            return new DataContainer(this.XmlEle.CloneNode(deep).OuterXml, "");
        }

        #endregion

        public int GetCount()
        {
            return this.GetNodeCount("collection") + this.GetNodeCount("parameter");
        }

        public int GetNodeCount(string nodeType)
        {
            string type = nodeType.ToLower();
            DataNodeType ntype = 0;
            switch (type)
            {
                case "collection":
                    ntype = DataNodeType.Collection;
                    break;
                case "parameter":
                    ntype = DataNodeType.Parameter;
                    break;
            }

            return this.GetNodeCount(ntype);
        }

        public int GetNodeCount(DataNodeType nodeType)
        {
            string xpath = "";
            switch (nodeType)
            {
                case DataNodeType.Collection:
                    xpath = "collections";
                    break;
                case DataNodeType.Parameter:
                    xpath = "parameters";
                    break;
            }

            XmlNode node = this.XmlEle.SelectSingleNode(xpath);
            if (node == null)
            {
                return 0;
            }
            else
            {
                return node.ChildNodes.Count;
            }
        }

        #region 私有方法

        /// <summary>
        /// 根据type取得节点标签
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetTag(DataNodeType type)
        {
            return GetTag(type, false);
        }

        /// <summary>
        /// 根据type取得节点标签(默认非集合)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isCollection">是否集合</param>
        /// <returns></returns>
        private string GetTag(DataNodeType type, bool isCollection)
        {
            string tag = type.ToString();

            if (isCollection)
            {
                switch (type)
                {
                    case DataNodeType.Collection:
                        tag = "collections";
                        break;
                    case DataNodeType.Parameter:
                        tag = "parameters";
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case DataNodeType.Collection:
                        tag = "collection";
                        break;
                    case DataNodeType.Parameter:
                        tag = "parameter";
                        break;
                }
            }

            return tag;
        }

        #endregion
    }
}
