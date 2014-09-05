using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Quartz;
using Quartz.Impl;
using Castle.ActiveRecord;
using PIC.Portal.Model;

namespace PIC.Portal.Services
{
    public class ScheduleService
    {
        #region 成员

        private static ISchedulerFactory schedulerFactory = CreateSchedulerFactory();
        private static IScheduler scheduler = schedulerFactory.GetScheduler();

        protected static ISchedulerFactory CreateSchedulerFactory()
        {
            if (schedulerFactory != null)
            {
                return schedulerFactory;
            }

            return new StdSchedulerFactory();
        }

        #endregion

        #region 公共方法

        public ActionResult RunTask(string id)
        {
            Task task = Task.Find(id);
            var result = task.Run();

            return result;
        }

        public ActionResult RunSchedule(string id)
        {
            Scheduler s = Scheduler.Find(id);
            var result = s.RunTask();

            return result;
        }

        public void Start()
        {
            var sysschedulers = Scheduler.FindAllByProperties(Scheduler.Prop_Status, "Enabled");

            ReloadTasks(sysschedulers);

            try
            {
                Thread.Sleep(3000);
            }
            catch (ThreadInterruptedException ex)
            {
                LogService.Log(ex, LogTypeEnum.Info);
            }
        }

        public void Stop()
        {
            if (scheduler != null && scheduler.IsStarted)
            {
                scheduler.Shutdown();

                scheduler = null;
            }
        }

        public void Reset()
        {
            Stop();

            try
            {
                Thread.Sleep(3000);
            }
            catch (ThreadInterruptedException)
            {
            }

            Start();
        }

        #endregion

        #region 私有方法

        protected IList<Scheduler> ReloadTasks(IList<Scheduler> sysschedulers)
        {
            if (scheduler != null)
            {
                if (scheduler.IsStarted)
                {
                    Stop();
                }
            }

            scheduler = schedulerFactory.GetScheduler();

            scheduler.Start();

            foreach (Scheduler s in sysschedulers)
            {
                var config = s.SchedulerConfig;

                if (!String.IsNullOrEmpty(config.CronString))
                {
                    var job = JobBuilder.Create<NormalJob>()
                        .WithIdentity(s.SchedulerID, "group2")
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(s.SchedulerID, "group2")
                        .WithCronSchedule(config.CronString)
                        .Build();

                    job.JobDataMap.Add("Config", config);

                    scheduler.AddJob(job, true);

                    scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    var job = JobBuilder.Create<NormalJob>()
                        .WithIdentity(s.SchedulerID, "group1")
                        .Build();

                    var simpleSBuilder = SimpleScheduleBuilder.Create()
                        .WithIntervalInMinutes((int)config.RepeatInterval.Value);

                    if (config.RepeatCount.Value - 1 < 0)
                    {
                        simpleSBuilder = simpleSBuilder.RepeatForever();
                    }
                    else
                    {
                        simpleSBuilder = simpleSBuilder.WithRepeatCount(config.RepeatCount.Value - 1);
                    }
                    
                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(s.SchedulerID, "group1")
                        .WithSchedule(simpleSBuilder)
                        .StartAt(new DateTimeOffset(config.StartTime.Value, TimeSpan.FromHours(-8)))    // Utc时间 - 8, 当前时间
                        .Build();

                    job.JobDataMap.Add("Config", config);

                    scheduler.ScheduleJob(job, trigger);
                }
            }

            return sysschedulers;
        }

        #endregion
    }
}