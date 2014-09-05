using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Workflow;

namespace PIC.Biz.Model.Reimbursement
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class OA_Reimbursement
    {
        #region 成员变量

        #endregion

        #region 成员属性
        
        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            /*if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }*/
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(Id))
            {
                this.DoCreate();
            }
            else
            {
                this.DoUpdate();
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            this.Status = "New";

            this.CreatorId = UserInfo.UserID;
            this.CreatorName = UserInfo.Name;
            this.CreatedDate = DateTime.Now;

            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        /// <summary>
        /// 获取报销数据
        /// </summary>
        /// <returns></returns>
        public OA_ReimbursementItemsData GetItemsData()
        {
            OA_ReimbursementItemsData itemsData = null;

            if (!String.IsNullOrEmpty(this.Items))
            {
                itemsData = JsonHelper.GetObject<OA_ReimbursementItemsData>(this.Items);
            }

            return itemsData;
        }

        #endregion

        #region 流程相关

        public void DoStartFlow(FlowRequest freq)
        {
            freq.OnTaskExecute = (ins) =>
            {
                // 此处this对象为初始流程启动时Market_ProjectAssign对象不可以直接引用this, 必须重新用Find方法获取对象
                // OA_Reimbursement ent = OA_Reimbursement.Find(ins.FlowState.ModelID.ToString());

                // OnAction(ent, ins.FlowState);
            };

            // WfService.StartFlow<OA_AdminFeeByMonthFlow>(freq);

            this.CreatedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        public void DoAction(string actionId, ActionRequest areq)
        {
            var action = WfAction.Find(actionId);

            var task = action.GetTask();

            if (!String.IsNullOrEmpty(action.TaskID))
            {
                //如果有一个职能经理打回，则整个任务打回
                if (task.Code == "FunctionalManager" && areq.RouteCode == "Reject")
                {
                    // action.ForceEnd(areq);
                }
                else if (task.Code == "AdminSpecialist")
                {
                    // 打回后重新提交
                    // areq.RouteCode = this.Type;
                    // action.Run(areq);
                }
                else if (task.Code == "Accountant")
                {
                    // action.Run(areq);

                    IList<OrgUser> usrs = null;

                    // areq.Title = this.Name + " - 财务报销";
                    areq.Tag["OpString"] = WfHelper.GetActionOpString("Pass", "报销完成", "ReimbFinish");

                    if (this.Type == "Functional")
                    {
                        if (action.ActionState.OwnerTag != null)
                        {
                            string deptId = StringHelper.IsNullValue(action.ActionState.OwnerTag["DepartmentId"]);

                            if (!String.IsNullOrEmpty(deptId))
                            {
                                var grp = OrgGroup.Find(deptId);
                                usrs = grp.GetUserList("Cashier");

                                WfActorCollection actors = new WfActorCollection();

                                foreach (OrgUser tsur in usrs)
                                {
                                    WfActor tactor = actors.AddUsersByID(tsur.UserID);
                                    
                                    tactor.Tag.Set("DepartmentId", deptId);
                                }

                                // action.DispatchActions(areq, usrs.ToArray());

                                // action.DispatchActions(areq, actors);
                            }
                        }
                    }
                    else
                    {
                        usrs = OrgGroup.GetUserList("010", "Cashier");
                        // action.DispatchActions(areq, usrs.ToArray());
                    }

                    // ent.Status = fstate.Current.TaskCode + "-" + fstate.TaskRequest.RouteCode;
                    this.Status = "Completed";

                    this.UpdateAndFlush();
                }
                else
                {
                    // action.Run(areq);
                }
            }
            else
            {
                // 执行自由任务，并发消息给发起人
                // action.Run(areq);

                string deptId = action.ActionState.OwnerTag.Get<string>("DepartmentId");

                if (this.Type == "Functional" && String.IsNullOrEmpty(deptId))
                {
                    OrgGroup grp = OrgGroup.Find(deptId);

                    // 发消息给行政专员
                    Message.SysSend(grp.Name + "财务已通过您提交的编号为“" + this.Code + "”的报销表单“" + this.Name + "”，请核查", areq.Comments, "", "", this.CreatorId);
                }
                else
                {
                    // 发消息给行政专员
                    Message.SysSend("您提交的编号为“" + this.Code + "”报销表单“" + this.Name + "”已报销通过，请核查", areq.Comments, "", "", this.CreatorId);
                }
            }

            if (areq.RouteCode == "Reject")
            {
                // 发消息给行政专员
                Message.SysSend("您提交的编号为“" + this.Code + "”报销表单“" + this.Name + "”已被打回，请核查", areq.Comments, "", "", this.CreatorId);
            }
        }

        /// <summary>
        /// 项目任务接收
        /// </summary>
        public void OnAction(OA_Reimbursement ent, FlowState fstate)
        {
            //IList<OrgUser> usrs = new List<OrgUser>();

            //switch (fstate.Current.TaskCode)
            //{
            //    case "AdminSpecialist":
            //        fstate.TaskRequest.RouteCode = ent.Type;
            //        fstate.TaskRequest.ActorList.SetUsersByID(ent.CreatorId);
            //        break;
            //    case "AdminManager":
            //        usrs = OrgGroup.GetUserList("001", "Management");
            //        fstate.TaskRequest.ActorList.SetUsers(usrs.ToArray());
            //        break;
            //    case "FunctionalManager":
            //        usrs = GetFunctionalUsers("Management");
            //        fstate.TaskRequest.ActorList.SetUsers(usrs.ToArray());
            //        break;
            //    case "CEO":
            //        usrs = OrgGroup.GetUserList("02", "CEO");
            //        fstate.TaskRequest.ActorList.SetUsers(usrs.ToArray());
            //        break;
            //    case "FinanceSupervisor":
            //        usrs = OrgGroup.GetUserList("010", "Management");
            //        fstate.TaskRequest.ActorList.SetUsers(usrs.ToArray());
            //        break;
            //    case "Accountant":
            //        if (ent.Type == "Functional")
            //        {
            //            // 如果是分摊报销，则发给各个相关部门财务记账，并由记账人员发给各个部门报销人员报销
            //            OA_ReimbursementItemsData itemsData = this.GetItemsData();

            //            if (itemsData != null)
            //            {
            //                IEnumerable<string> deptIds = itemsData.Items.Select(item => { return item.DepartmentId; }).Distinct();
            //                OrgGroup[] depts = OrgGroup.FindAll(Expression.In(OrgGroup.Prop_GroupID, deptIds.ToArray()));

            //                foreach (OrgGroup tdept in depts)
            //                {
            //                    var grp = OrgGroup.Find(tdept.GroupID);
            //                    OrgUser tusr = grp.GetUserList("Accountant").First();

            //                    WfActor tactor = fstate.TaskRequest.ActorList.AddUsersByID(tusr.UserID);
            //                    tactor.Tag.Set("DepartmentId", tdept.ID);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            // 如果是综合报销，则发给行政记账人员
            //            usrs = OrgGroup.GetUserList("010", "Accountant");
            //            fstate.TaskRequest.ActorList.SetUsers(usrs.ToArray());
            //        }
            //        break;
            //}

            //// ent.Status = fstate.Current.TaskCode + "-" + fstate.TaskRequest.RouteCode;
            //ent.Status = fstate.Current.TaskCode;

            //ent.UpdateAndFlush();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取指定角色的功能用户
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        private IList<OrgUser> GetFunctionalUsers(string roleCode)
        {
            IList<OrgUser> usrs = new List<OrgUser>();

            OA_ReimbursementItemsData itemsData = this.GetItemsData();

            if (itemsData != null)
            {
                IEnumerable<string> deptIds = itemsData.Items.Select(item => { return item.DepartmentId; }).Distinct();

                foreach (string deptid in deptIds)
                {
                    var grp = OrgGroup.Find(deptid);

                    IList<OrgUser> iusrs = grp.GetUserList(roleCode);

                    foreach (OrgUser tusr in iusrs)
                    {
                        if (!usrs.Contains(tusr))
                        {
                            usrs.Add(tusr);
                        }
                    }
                }
            }

            return usrs;
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			OA_Reimbursement[] tents = OA_Reimbursement.FindAll(Expression.In("Id", args));

			foreach (OA_Reimbursement tent in tents)
			{
				tent.DoDelete();
			}
        }
		
        /// <summary>
        /// 由编码获取模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OA_Reimbursement Get(string code)
        {
            OA_Reimbursement ent = OA_Reimbursement.FindFirstByProperties(OA_Reimbursement.Prop_Code, code);

            return ent;
        }

        /// <summary>
        /// 获取月报销费用
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static decimal GetMonthExpense(int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);
            string sql = String.Format("SELECT SUM(ISNULL(Fee, 0)) FROM OA_Reimbursement WHERE FeeDate>'{0}' AND FeeDate<'{1}'", startDate, endDate);

            decimal expense = BizDataHelper.QueryValue<decimal>(sql);

            return expense;
        }
        
        #endregion

        #region 内部类

        public class OA_ReimbursementItemsData
        {
            public IList<OA_ReimbursementItem> Items
            {
                get;
                set;
            }

            public OA_ReimbursementItemsData()
            {
                Items = new List<OA_ReimbursementItem>();
            }
        }

        public class OA_ReimbursementItem
        {
            public string Index { get; set; }
            public string Title { get; set; }
            public decimal Fee { get; set; }
            public DateTime Date { get; set; }
            public string DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public string IsReceipt { get; set; }
            public string Comments { get; set; }
        }

        #endregion

    } // OA_Reimbursement
}


