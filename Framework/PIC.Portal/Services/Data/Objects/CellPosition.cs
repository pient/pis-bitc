using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    public class CellPosition
    {
        #region 成员属性

        /// <summary>
        /// 列值位置
        /// </summary>
        public int ValueColumnIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 行值位置
        /// </summary>
        public int ValueRowIndex
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public CellPosition()
        {
        }

        public CellPosition(int columnIndex, int rowIndex)
        {
            ValueColumnIndex = columnIndex;
            ValueRowIndex = rowIndex;
        }

        #endregion
    }
}
