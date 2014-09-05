using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using PIC.Portal.Model;

namespace PIC.Portal.Services
{
    /// <summary>
    /// 系统内部方法
    /// </summary>
    public class SysInnerTaskAction : ITaskAction
    {
        #region 执行系统内部方法

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public TaskActionResult Execute(TaskActionArgs actionArgs)
        {
            string methodName = null;

            TaskActionResult result = new TaskActionResult();

            try
            {
                if (actionArgs.Scheduler != null
                    && actionArgs.Scheduler.SchedulerConfig != null
                    && actionArgs.Scheduler.SchedulerConfig.Tag != null)
                {
                    methodName = actionArgs.Scheduler.SchedulerConfig.Tag.Get<string>("MethodName");
                }
                else if (actionArgs.Task != null)
                {
                    SysTaskConfig taskConfig = JsonHelper.GetObject<SysTaskConfig>(actionArgs.Task.Config);

                    methodName = taskConfig.MethodName;
                }

                result = ExecuteMethod(methodName, actionArgs);
            }
            catch (Exception ex)
            {
                result = new TaskActionResult(ex);
            }

            return result;
        }

        #endregion

        #region 支持方法

        private TaskActionResult ExecuteMethod(string methodName, TaskActionArgs actionArgs)
        {
            TaskActionResult result = new TaskActionResult();

            switch (methodName)
            {
                case "SendEmail":
                    result = SendEmail(actionArgs);
                    break;
                case "Empty":
                default:
                    result = Empty();
                    break;
            }

            return result;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns></returns>
        private TaskActionResult SendEmail(TaskActionArgs actionArgs)
        {
            SysTaskConfig taskConfig = JsonHelper.GetObject<SysTaskConfig>(actionArgs.Task.Config);
            int amountOnce = taskConfig.Args.Get<int>("AmountOnce", 10);

            // 从数据库从取出最早发送的进行发送
            Data.HqlSearchCriterion crit = new Data.HqlSearchCriterion();
            crit.SetOrder(Message.Prop_SentDate);
            crit.SetSearch(Message.Prop_Type, "Sent");
            crit.SetSearch(Message.Prop_SysType, "Message");
            crit.SetSearch(Message.Prop_Status, "New");

            crit.PageSize = amountOnce;

            Message[] msgList = Message.FindAllByCriterion(crit);

            foreach (var msg in msgList)
            {
                // Utilities.MailHelper.SysSend(msg.ToIDs, msg.Subject, msg.Content);
                Utilities.MailHelper.SysSend("nkxt@bitc.edu.cn", msg.Subject, msg.Content);

                msg.SentDate = DateTime.Now;
                msg.MessageStatus = Message.StatusEnum.Sent;
                msg.DoUpdate();
            }

            TaskActionResult result = new TaskActionResult(TaskActionResult.ResultEnum.Completed, "执行成功");

            return result;
        }

        /// <summary>
        /// 空方法，一般只是用来确定任务正在执行
        /// </summary>
        private TaskActionResult Empty()
        {
            return new TaskActionResult();
        }

        #endregion
    }
}
