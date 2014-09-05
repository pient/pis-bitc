using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;

namespace PIC.Data
{
    public class DataParameter : DataNode
    {
        #region 构造函数

        /// <summary>
        /// 空的DataParameter构造函数
        /// </summary>
        public DataParameter()
            : base()
        {
            this.Type = DataNodeType.Parameter;
            this.Init();
        }

        /// <summary>
        /// 根据XML字符串构造DataParameter
        /// </summary>
        /// <param name="xmlsrc"></param>
        public DataParameter(string xmlsrc)
            : base(xmlsrc)
        {
            this.Type = DataNodeType.Parameter;
        }

        /// <summary>
        /// 根据XmlElement构造DataParameter
        /// </summary>
        /// <param name="ele"></param>
        public DataParameter(XmlElement ele)
            : base(ele)
        {
            this.Type = DataNodeType.Parameter;
        }

        /// <summary>
        /// 根据DataParameter构造新的DataParameter
        /// </summary>
        /// <param name="ele"></param>
        public DataParameter(DataParameter dataParam)
            : base(dataParam.ToString())
        {
            this.Type = DataNodeType.Parameter;
        }

        /// <summary>
        /// 根据XmlElement构造DataParameter
        /// </summary>
        /// <param name="ele"></param>
        public DataParameter(string name, string value)
            : base()
        {
            this.Type = DataNodeType.Parameter;
            this.Init();
            this.SetName(name);
            this.SetValue(value);
        }

        /// <summary>
        /// 根据XmlDocument和XmlElement构造DataParameter
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        internal DataParameter(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
            this.Type = DataNodeType.Parameter;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="dElement"></param>
        public new DataElement AddElement(DataElement ele) { return null; }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new void Remove(int index) { }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new void Remove(DataElement obj) { }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new void Clear() { }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new int GetElementCount() { return 0; }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new DataElement GetElement(int index) { return null; }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new DataElement GetElement(string xpath) { return null; }

        /// <summary>
        /// 屏蔽父类的方法
        /// </summary>
        /// <param name="index"></param>
        public new ArrayList GetElements(string xpath) { return null; }

        /// <summary>
        /// 根据name克隆DataList对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new DataParameter Clone(bool includeChild)
        {
            XmlElement node = (XmlElement)this.CloneNode(includeChild);
            return new DataParameter(this.XmlDoc, node);
        }

        /// <summary>
        /// 根据name克隆DataList对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataParameter Clone(string name)
        {
            XmlElement node = (XmlElement)this.CloneNode(name);
            return new DataParameter(this.XmlDoc, node);
        }

        #endregion
    }
}
