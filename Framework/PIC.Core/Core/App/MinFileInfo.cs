using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 文件信息相关，都应该实现此接口，并可获取最小文件信息
    /// </summary>
    public interface IFileInfo
    {
        MinFileInfo GetMinFileInfo();
    }

    /// <summary>
    /// 简化版用户信息，一般用于流程审批等过程的人员记录
    /// </summary>
    public class MinFileInfo : IFileInfo
    {
        public string FileID { get; set; }
        public string Name { get; set; }

        public MinFileInfo GetMinFileInfo()
        {
            return this;
        }
    }
}
