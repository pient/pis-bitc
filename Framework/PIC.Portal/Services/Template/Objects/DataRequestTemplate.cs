using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using PIC.Data;

namespace PIC.Portal.Template
{
    [Serializable]
    public class DataRequestTemplate : TemplateBase<string>
    {
        #region Properties

        /// <summary>
        /// 用于查询数据
        /// </summary>
        public string Hql
        {
            get;
            set;
        }

        /// <summary>
        /// 模块
        /// </summary>
        public string DataModule
        {
            get;
            set;
        }

        /// <summary>
        /// 数据行数
        /// </summary>
        public int Rows
        {
            get;
            set;
        }

        /// <summary>
        /// 查询数据模板类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 开始模板
        /// </summary>
        public string Begin
        {
            get;
            set;
        }

        /// <summary>
        /// 数据项模板
        /// </summary>
        public string Item
        {
            get;
            set;
        }

        /// <summary>
        /// 结束模板
        /// </summary>
        public string End
        {
            get;
            set;
        }

        #endregion

        #region TemplateBase Methods

        /// <summary>
        /// 生成数据
        /// </summary>
        /// <returns></returns>
        public override string Generate()
        {
            StringBuilder sb = new StringBuilder();

            string typestr = String.Empty;



            return sb.ToString();
        }

        #endregion
    }
}
