using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PIC.Security;
using PIC.Common;

namespace PIC.Tools.Config
{
    public class Config
    {
        #region 成员

        XmlDocument xmlDoc;
        string macCode;

        #endregion

        #region 构造函数

        public Config(string config, string macCode)
        {
            LoadConfig(config);
            this.macCode = macCode;
        }

        #endregion

        #region 属性

        public bool IsProtected
        {
            get
            {
                bool isProtected = false;

                if (xmlDoc.DocumentElement.Attributes["IsProtected"] != null &&
                    xmlDoc.DocumentElement.Attributes["IsProtected"].Value.ToLower() == "true")
                {
                    isProtected = true;
                }

                return isProtected;
            }
        }

        public string Content
        {
            get
            {
                string content;

                if (IsProtected)
                {
                    content = PICConfigurationManager.DecryptConfig(xmlDoc.DocumentElement, macCode);

                    content = XmlHelper.GetFormatXml(content);
                }
                else
                {
                    content = xmlDoc.DocumentElement.InnerXml;
                }

                return content;
            }
        }

        public string EncryptedContent
        {
            get
            {
                string content;

                if (!IsProtected)
                {
                    content = PICConfigurationManager.EncryptConfig(xmlDoc.DocumentElement, macCode);
                }
                else
                {
                    content = xmlDoc.DocumentElement.OuterXml;
                }

                return content;
            }
        }

        #endregion

        #region 方法

        private void LoadConfig(string config)
        {
            try
            {
                config = XmlHelper.GetFormatXml(config);

                xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(config);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
