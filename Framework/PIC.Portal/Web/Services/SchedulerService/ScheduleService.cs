using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Quartz;
using Quartz.Impl;
using Castle.ActiveRecord;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Services;

namespace PIC.Portal.Web.Services
{
    public class ScheduleService : IScheduleService
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

        /// <summary>
        /// 执行惹怒
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskActionResult RunTask(string id)
        {
            Task task = Task.Find(id);
            var result = task.Run();

            return result;
        }

        /// <summary>
        /// 执行活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskActionResult RunSchedule(string id)
        {
            Scheduler s = Scheduler.Find(id);
            var result = s.RunTask();

            return result;
        }

        public void Start()
        {
            var sysSchedulers = Scheduler.FindAllByProperties(Scheduler.Prop_Status, "Enabled");

            ReloadSchedulers(sysSchedulers);

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

        protected IList<Scheduler> ReloadSchedulers(IList<Scheduler> sysSchedulers)
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

            foreach (Scheduler s in sysSchedulers)
            {
                var config = s.SchedulerConfig;

                if (!String.IsNullOrEmpty(config.CronString))
                {
                    var job = JobBuilder.Create<TaskJob>()
                        .WithIdentity(s.SchedulerID, "group2")
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(s.SchedulerID, "group2")
                        .WithCronSchedule(config.CronString)
                        .Build();

                    job.JobDataMap.Add("Scheduler", s);

                    scheduler.AddJob(job, true);

                    scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    var job = JobBuilder.Create<TaskJob>()
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

                    var triggerBuilder = TriggerBuilder.Create()
                        .WithIdentity(s.SchedulerID, "group1")
                        .WithSchedule(simpleSBuilder);

                    if (config.StartTime.HasValue)
                    {
                        triggerBuilder = triggerBuilder.StartAt(new DateTimeOffset(config.StartTime.Value, TimeSpan.FromHours(-8)));    // Utc时间 - 8, 当前时间
                    }

                    var trigger = triggerBuilder.Build();

                    job.JobDataMap.Add("Scheduler", s);

                    scheduler.ScheduleJob(job, trigger);
                }
            }

            return sysSchedulers;
        }

        #endregion
    }
}