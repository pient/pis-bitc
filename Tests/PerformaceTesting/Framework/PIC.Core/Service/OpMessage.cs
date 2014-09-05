using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PIC.Data;

namespace PIC.Common.Service
{
    public class OpParameter : DataParameter
    {
        #region 构造函数

        public OpParameter()
            : base()
        {
        }

        public OpParameter(string xmlsrc)
            : base(xmlsrc)
        {
        }

        public OpParameter(XmlElement ele)
            : base(ele)
        {
        }

        public OpParameter(DataParameter dataParam)
            : base(dataParam.ToString())
        {
        }

        public OpParameter(OpParameter opParam)
            : base(opParam.ToString())
        {
        }

        public OpParameter(string name, TypeCode type, object value)
            : base()
        {
            this.SetName(name);

            this.SetAttr("Type", type.ToString());

            if (value != null)
            {
                this.SetValue(value.ToString());
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 参数类型
        /// </summary>
        new public TypeCode Type
        {
            get
            {
                string type = this.GetAttr("Type");

                if (!String.IsNullOrEmpty(type))
                {
                    return CLRHelper.GetEnum<TypeCode>(type, TypeCode.Empty);
                }

                return TypeCode.Empty;
            }
            set
            {
                this.SetAttr("Type", value.ToString());
            }
        }

        /// <summary>
        /// 参数值(一般为简单类型)
        /// </summary>
        public object Value
        {
            get
            {
                string value = this.GetValue();
                return Convert.ChangeType(value, this.Type);
            }

            set
            {
                this.SetValue(value.ToString());
            }
        }

        /// <summary>
        /// 参数名(元素Id)
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetName();
            }

            set
            {
                this.SetName(value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 参数集合
    /// </summary>
    public class OpParameterCollection : DataCollection
    {
        #region 构造函数

        public OpParameterCollection()
            : base()
        {
        }

        public OpParameterCollection(string xmlsrc)
            : base(xmlsrc)
        {
        }

        public OpParameterCollection(XmlElement ele)
            : base(ele)
        {
        }

        public OpParameterCollection(DataCollection dtColl)
            : base(dtColl.ToString())
        {
        }

        public OpParameterCollection(OpParameterCollection opcoll)
            : base(opcoll.ToString())
        {
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public OpParameter GetParameter(string name)
        {
            return this.GetElementById(name) as OpParameter;
        }

        #endregion
    }

    public class OpMessage : DataMessage
    {
        #region 私有成员

        #endregion

        #region 成员属性

        /// <summary>
        /// 用户Session
        /// </summary>
        public string SessionID
        {
            get { return this.Content["SessionID"]; }
            set
            {
                this.Content["SessionID"] = value;
            }
        }

        /// <summary>
        /// 操作
        /// </summary>
        public string Operation
        {
            get { return this.Content["Operation"]; }
            set
            {
                this.Content["Operation"] = value;
            }
        }

        #endregion

        #region 构造函数

        public OpMessage()
            : base()
        {
        }

        public OpMessage(string dataString)
            : base(dataString)
        {
        }

        public OpMessage(string sessionID, string op)
        {
            Content["SessionID"] = sessionID;
            Content["Operation"] = op;
        }

        #endregion

        #region 公共方法

        public OpParameter this[string name]
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

        public OpParameter this[string name, OpParameter defValue]
        {
            get
            {
                OpParameter dp = Content.Get(DataNodeType.Parameter, name) as OpParameter;

                if (dp == null)
                {
                    return defValue;
                }

                return dp;
            }

            set
            {
                OpParameter op = Content.Get(DataNodeType.Parameter, name) as OpParameter;

                if (op == null)
                {
                    op = new OpParameter(value);

                    Content.Add(op);
                }
                else
                {
                    op.Value = value;
                }
            }
        }

        #endregion
    }
}
