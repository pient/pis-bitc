using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal
{
    public class FileObject : IFileInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool? istemp { get; set; }

        public FileObject()
        {
            istemp = true;
        }

        #region IFileInfo 成员

        /// <summary>
        /// 获取最小文件信息
        /// </summary>
        /// <returns></returns>
        public MinFileInfo GetMinFileInfo()
        {
            return new MinFileInfo()
            {
                FileID = id,
                Name = name
            };
        }

        #endregion
    }
}
