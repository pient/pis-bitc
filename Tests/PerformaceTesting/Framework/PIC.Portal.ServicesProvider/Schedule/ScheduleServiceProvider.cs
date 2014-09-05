using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using PIC.Portal.Model;
using PIC.Portal.Services;
using PIC.Portal.Web.Services;

namespace PIC.Portal.ServicesProvider
{
    /// <summary>
    /// 任务调度支持类
    /// </summary>
    public class ScheduleServiceProvider : IScheduleServiceProvider
    {
        #region 成员

        // private ScheduleService.ScheduleServiceClient ssc;
        private IScheduleService ssc;

        #endregion

        #region 构造函数

        public ScheduleServiceProvider()
        {
            // ssc = new ScheduleService.ScheduleServiceClient();
            ssc = new Web.Services.ScheduleService();
        }

        #endregion

        #region 属性

        protected Web.Services.IScheduleService ScheduleService
        {
            get
            {
                if (ssc == null)
                {
                    // ssc = new ScheduleService.ScheduleServiceClient();
                    ssc = new Web.Services.ScheduleService();
                }

                //if (ssc.State == CommunicationState.Closed || ssc.State == CommunicationState.Faulted)
                //{
                //    ssc = new ScheduleService.ScheduleServiceClient();
                //}

                return ssc;
            }
        }

        #endregion

        #region IScheduleServiceProvider 成员

        /// <summary>
        /// 执行Schedule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskActionResult RunSchedule(string id)
        {
            TaskActionResult result = ssc.RunSchedule(id);

            if (result != null && result.Result != TaskActionResult.ResultEnum.Completed)
            {
                if (!String.IsNullOrEmpty(result.Message))
                {
                    throw new MessageException(result.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskActionResult RunTask(string id)
        {
            TaskActionResult result = ssc.RunTask(id);

            if (result != null && result.Result != TaskActionResult.ResultEnum.Completed)
            {
                if (!String.IsNullOrEmpty(result.Message))
                {
                    throw new MessageException(result.Message);
                }
            }

            return result;
        }

        public void Start()
        {
            ssc.Start();
        }

        public void Stop()
        {
            ssc.Stop();
        }

        public void Reset()
        {
            ssc.Reset();
        }

        #endregion
    }
}
