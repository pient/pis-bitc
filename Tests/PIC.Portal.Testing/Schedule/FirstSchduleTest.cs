using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Quartz;
using Quartz.Impl;

namespace PIC.Portal.Testing.Schedule
{
    [TestFixture]
    public class FirstSchduleTest
    {
        [SetUp]
        public void Init()
        {
            PortalService.Initialize();
        }

        [Test]
        public void ScheduleServiceTest()
        {
            SchedulerService.ReloadSchedule();

            System.Threading.Thread.Sleep(1000 * 60 * 2);

            // var schService = new Web.Services.ScheduleService();
            // schService.Start();
        }

        [Test]
        public void FirstQuartzTest()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // construct job info
            var jobDetail = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob")
                .Build();

            
            var trigger = TriggerBuilder.Create()
                .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(5))  // fire every hour
                // .StartAt(new DateTimeOffset(DateTime.UtcNow))   // start on the next even hour
                .Build();

            trigger.JobDataMap.Add("Name", "myTrigger");

            sched.ScheduleJob(jobDetail, trigger);

            Console.WriteLine("Schedule started");
        }

        public class HelloJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                int times = 0;

                if (context.JobDetail.JobDataMap["times"] != null)
                {
                    times = (int)context.JobDetail.JobDataMap["times"];
                }

                context.JobDetail.JobDataMap["times"] = ++times;

                Console.WriteLine(string.Format("job ran {0} times.", times));
            }
        }
    }
}
