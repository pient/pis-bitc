using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    /// <summary>
    /// 导入文件类型
    /// </summary>
    [Flags]
    public enum TemplateFileType
    {
        Pdf,
        Word,
        Excel,
        Other
    }
}
