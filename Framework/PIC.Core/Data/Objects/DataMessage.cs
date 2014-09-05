using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Data
{
    public class DataMessage : SvcMessage<DataContainer>
    {
        #region 成员属性

        public override string Lable
        {
            get
            {
                return Content["Lable"];
            }
            set
            {
                Content["Lable"] = value;
            }
        }

        #endregion

        #region 构造函数

        public DataMessage()
        {
            Content = new DataContainer();
        }

        public DataMessage(string dataString)
        {
            Content = new DataContainer(dataString);
        }

        public DataMessage(DataContainer dataContainer)
            : this(dataContainer.ToString())
        {
        }

        #endregion

        #region 重载

        public override string ToString()
        {
            return Content.ToString();
        }

        #endregion
    }
}
