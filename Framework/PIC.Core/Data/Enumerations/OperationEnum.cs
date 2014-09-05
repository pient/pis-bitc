using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    /// <summary>
    /// 粘贴数据来源类型
    /// </summary>
    public enum PasteDataSourceEnum
    {
        Copy,   // 由复制而来
        Cut, // 由剪切而来（粘贴后将删除原数据）
        Unknown
    }

    /// <summary>
    /// 粘贴为类型
    /// </summary>
    public enum PasteAsEnum
    {
        Sibling,    // 粘贴为目标节点兄弟节点
        Child,   // 粘贴为目标节点子节点
        Other
    }
}
