using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PIC.Component;
using PIC.Component.ThirdpartySupport.MsOffice;
using PIC.Portal.Model;

namespace PIC.Portal
{
    public abstract class TemplateParser
    {
        #region 成员属性

        public string _TemplateFilePath;

        /// <summary>
        /// 获取模版文件路径 
        /// </summary>
        public string TemplateFilePath
        {
            get
            {
                return _TemplateFilePath;
            }
        }

        ReadOnlyCollection<ExcelCell> ecList = null;

        /// <summary>
        /// ExcelCellList
        /// </summary>
        public ReadOnlyCollection<ExcelCell> ExcelCellList
        {
            get
            {
                if (ecList == null)
                {
                    using (ExcelProcessor processor = ExcelService.GetProcessor(_TemplateFilePath))
                    {
                        IList<ExcelCell> tecList = processor.GetCellsWithComment();

                        ecList = new ReadOnlyCollection<ExcelCell>(tecList);
                    }
                }

                return ecList;
            }
        }

        #endregion

        #region 构造函数

        public TemplateParser(string filePath)
        {
            _TemplateFilePath = filePath;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取模版结构
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public abstract TemplateStructure GetStructure();

        /// <summary>
        /// 获取下载文件
        /// </summary>
        public void GetDownloadFile()
        {

        }

        #endregion

        #region 私有函数



        #endregion
    }
}
