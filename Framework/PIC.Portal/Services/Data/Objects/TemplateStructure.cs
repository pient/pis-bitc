using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using NHibernate.Id;
using PIC.Data;
using PIC.Portal.Model;

namespace PIC.Portal
{
    public class TemplateStructure
    {
        #region 成员属性

        private string _templateFileTypeString = String.Empty;
        private TemplateFileType _templateFileType = TemplateFileType.Other;

        /// <summary>
        /// 模版文件类型
        /// </summary>
        [JsonIgnore]
        public TemplateFileType TemplateFileType
        {
            get
            {
                return TemplateFileType;
            }

            set
            {
                _templateFileType = value;
                _templateFileTypeString = _templateFileType.ToString();
            }
        }

        /// <summary>
        /// 模版文件类型字符串
        /// </summary>
        public string TemplateFileTypeString
        {
            get
            {
                return _templateFileTypeString;
            }

            set
            {
                _templateFileTypeString = value;

                try
                {
                    _templateFileType = (TemplateFileType)Enum.Parse(typeof(TemplateFileType), _templateFileTypeString, true);
                }
                catch
                {
                    _templateFileType = TemplateFileType.Other;
                }
            }
        }

        #endregion

        #region 构造函数

        public TemplateStructure()
        {
            TemplateFileType = TemplateFileType.Other;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        public virtual string GetConfig()
        {
            string _config = JsonHelper.GetJsonString(this);

            return _config;
        }

        #endregion

        #region 静态方法

        ///// <summary>
        ///// 由配置文件获取模版结构
        ///// </summary>
        ///// <param name="template"></param>
        ///// <returns></returns>
        //public static ImportTemplateStructure GetFromConfig(string config)
        //{
        //    ImportTemplateStructure its = JsonHelper.GetObject<ImportTemplateStructure>(config);

        //    return its;
        //}

        #endregion
    }
}
