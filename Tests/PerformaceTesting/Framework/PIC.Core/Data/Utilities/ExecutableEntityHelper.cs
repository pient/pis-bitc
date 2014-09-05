using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    public class ExecutableEntityHelper
    {
        /// <summary>
        /// 设置编辑状态
        /// </summary>
        /// <param name="execState"></param>
        public static void SetExecuteState(IExecutableEntity ent, ExecuteStateEnum execState)
        {
            ent.ExecuteState = execState.ToString();
        }

        /// <summary>
        /// 设置编辑状态
        /// </summary>
        /// <param name="execStateString"></param>
        public static void SetExecuteState(IEditSensitiveEntity ent, string execStateString)
        {
            ent.EditStatus = execStateString;
        }

        /// <summary>
        /// 获取执行状态
        /// </summary>
        public static ExecuteStateEnum GetExecuteStateFlag(string execStateString)
        {
            try
            {
                return (ExecuteStateEnum)Enum.Parse(typeof(ExecuteStateEnum), execStateString, true);
            }
            catch
            {
                return ExecuteStateEnum.Other;
            }
        }
    }
}
