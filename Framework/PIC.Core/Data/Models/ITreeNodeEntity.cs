using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    /// <summary>
    /// 树节点实体接口
    /// </summary>
    public interface ITreeNodeEntity<T>
    {
        /// <summary>
        /// 父节点标识
        /// </summary>
        string ParentID { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>
        string Path { get; set; }
    }
}
