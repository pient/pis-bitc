using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PIC.Component
{
    public class COMHelper
    {
        /// <summary>
        /// 释放COM对象
        /// </summary>
        /// <param name="obj"></param>
        public static void ReleaseObject(object obj)
        {
            if (obj != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            }
        }

        /// <summary>
        /// 关闭指定名称指定启动时间段的进程(一般用于关闭后台启动的COM组件进程)
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="startTimeBegin"></param>
        /// <param name="startTimeEnd"></param>
        public static void KillProcessByNameAndStartTime(string processName, DateTime startTimeBegin, DateTime startTimeEnd)
        {
            IList<Process> procs = COMHelper.GetProcessesByNameAndStartTime(PIC.Component.ThirdpartySupport.Application.MS_OFFICE_EXCEL_PROCESS_NAME,
                startTimeBegin, startTimeEnd);

            foreach (Process tproc in procs)
            {
                tproc.Kill();
            }
        }

        /// <summary>
        /// 获取指定名称指定启动时间段的进程(一般用于获取后台启动的COM组件进程)
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="startTimeBegin"></param>
        /// <param name="startTimeEnd"></param>
        /// <returns></returns>
        public static IList<Process> GetProcessesByNameAndStartTime(string processName, DateTime startTimeBegin, DateTime startTimeEnd)
        {
            IList<Process> procList = new List<Process>();

            DateTime startTime;

            Process[] proccesses = Process.GetProcessesByName(processName);

            foreach (Process tproc in proccesses)
            {
                startTime = tproc.StartTime;

                if (startTime >= startTimeBegin && startTime <= startTimeEnd)
                {
                    procList.Add(tproc);
                }
            }

            return procList;
        }
    }
}
