using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    public static class CLRExtensions
    {
        #region 类型操作

        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return (guid == null || guid == Guid.Empty);
        }

        public static Guid? ToGuid(this object obj, Guid? defValue = null)
        {
            Guid? rtn = defValue;

            if (obj != null)
            {
                if (obj is Guid?)
                {
                    rtn = (Guid?)obj;
                }
                else
                {
                    Guid guid = Guid.Empty;

                    if (Guid.TryParse(obj.ToString(), out guid))
                    {
                        rtn = guid;
                    }
                }
            }

            return rtn;
        }

        #endregion

    }
}
