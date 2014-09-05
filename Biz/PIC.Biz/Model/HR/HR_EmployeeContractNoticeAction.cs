using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using NHibernate;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Biz.Model
{
    public class HR_EmployeeContractNoticeAction : IAction
    {
        /// <summary>
        /// 合同到期提醒（一般由计划任务调用）
        /// </summary>
        public ActionResult Execute(Portal.Model.Task task)
        {
            ActionResult result = null;

            try
            {
                DateTime endDate = DateTime.Now.AddDays(30);

                using (new SessionScope())
                {
                    // 查找出合同将过期的员工，这里设置30天
                    HR_Employee[] employees = HR_Employee.FindAll(
                        Expression.Or(Expression.IsNull(HR_Employee.Prop_Status), Expression.Eq(HR_Employee.Prop_Status, "Enable")),
                        Expression.IsNotNull(HR_Employee.Prop_ContractEndDate),
                        Expression.Le(HR_Employee.Prop_ContractEndDate, endDate));

                    if (employees.Count() > 0)
                    {
                        string date_str = String.Format("{0}年{1}月{2}", endDate.Year, endDate.Month, endDate.Day);

                        string subjectstr = String.Format("有 {0} 位员工合同将在30天后 ({1}) 到期", employees.Count(), date_str);
                        string contentstr = "员工";
                        string employeestrs = "";

                        foreach (HR_Employee tent in employees)
                        {
                            employeestrs += String.Format("{0} (员工号：{1}), ", tent.Name, tent.Code);
                            tent.EmployeeStatus = HR_Employee.StatusEnum.ContractWarned;

                            tent.DoUpdate();
                        }

                        contentstr = String.Format("员工 {0} 的合同将在30天后 ({1}) 到期，请及时处理。", employeestrs.TrimEnd(','), date_str);

                        // 发消息给指定组成员
                        // Message.SysSendTo("HR_HTTX", subjectstr, contentstr, null, null);

                        result = new ActionResult(ActionResult.ResultEnum.Completed, "任务执行完成.");
                    }
                    else
                    {
                        result = new ActionResult(ActionResult.ResultEnum.Completed, "没有将要过期的员工.");
                    }
                }
            }
            catch (Exception ex)
            {
                result = new ActionResult(ActionResult.ResultEnum.Failed, ex.Message);
            }

            return result;
        }
    }
}
