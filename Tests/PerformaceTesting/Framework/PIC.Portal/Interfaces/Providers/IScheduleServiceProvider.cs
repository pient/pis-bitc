using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Portal.Model;

namespace PIC.Portal.Services
{
    /// <summary>
    /// 任务调度支持类
    /// </summary>
    public interface IScheduleServiceProvider
    {
        TaskActionResult RunSchedule(string id);
        TaskActionResult RunTask(string id);

        void Start();
        void Stop();
        void Reset();
    }
}
