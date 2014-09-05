using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Common;
using PIC.Portal.Services;
using PIC.Portal.Model;

namespace PIC.Portal
{
    public class SchedulerService
    {
        public const string SCHEDULE_SERVICE_PROVIDER_KEY = "ScheduleServiceProvider";

        #region 成员

        protected IScheduleServiceProvider Provider = null;

        #endregion

        #region 构造函数

        protected SchedulerService()
        {
            Provider = GetDefaultProvider();
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 获取默认SchedulerService提供者
        /// </summary>
        /// <returns></returns>
        public static IScheduleServiceProvider GetDefaultProvider()
        {
            string typeName = PICConfigurationManager.AppSettings[SCHEDULE_SERVICE_PROVIDER_KEY];

            IScheduleServiceProvider ssp = (IScheduleServiceProvider)Activator.CreateInstance(Type.GetType(typeName));

            return ssp;
        }

        private static SchedulerService instance;

        /// <summary>
        /// 服务实例，单体模式
        /// </summary>
        public static SchedulerService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SchedulerService();
                }

                return instance;
            }
        }

        /// <summary>
        /// 立即执行任务调度
        /// </summary>
        /// <param name="id">调度id</param>
        public static void RunSchedule(string id)
        {
            Instance.Provider.RunSchedule(id);
        }

        /// <summary>
        /// 停止调度
        /// </summary>
        /// <param name="id"></param>
        public static void StopSchedule()
        {
            Instance.Provider.Stop();
        }

        /// <summary>
        /// 重新加载调度（即重启调度引擎，并重新加载所有调度）
        /// </summary>
        public static void ReloadSchedule()
        {
            Instance.Provider.Reset();
        }

        /// <summary>
        /// 立即执行调度任务
        /// </summary>
        /// <param name="id">任务id, 调度还未建立时，执行此验证任务</param>
        public static TaskActionResult RunTask(string id)
        {
            return Instance.Provider.RunTask(id);
        }

        #endregion
    }
}
