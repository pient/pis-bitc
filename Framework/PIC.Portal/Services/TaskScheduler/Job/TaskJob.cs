using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Portal.Services
{
    public class TaskJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Scheduler sysScheduler = context.JobDetail.JobDataMap.Get("Scheduler") as Scheduler;

            if (sysScheduler != null)
            {
                sysScheduler.RunTask();
            }
        }
    }
}
