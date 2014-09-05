using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    // 编辑状态枚举
    [Flags]
    public enum EditStatusEnum
    {
        Create,
        Read,
        Update,
        Delete,
        Other
    }

    /// <summary>
    /// 编辑状态敏感对象
    /// </summary>
    public interface IEditSensitiveEntity
    {
        /// <summary>
        /// 编辑状态标识
        /// </summary>
        string EditStatus { get; set; }
    }
}
