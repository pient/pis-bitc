using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Biz.Model;

using System.Threading;
using System.Activities;
using System.Activities.Statements;
using System.Activities.DurableInstancing;
using System.Runtime.DurableInstancing;
using PIC.Portal.Workflow;

namespace PIC.Biz.Model.Test
{
    public class TestService
    {
        static AutoResetEvent instanceUnloaded = new AutoResetEvent(false);

        /// <summary>
        /// 启动项目流程
        /// </summary>
        /// <param name="code"></param>
        public static void StartFlow2()
        {
            FlowRequest request = new FlowRequest("Market_TestFlow");

            WfService.StartFlow<Market_TestFlow>(request);
        }

        /// <summary>
        /// 启动项目流程
        /// </summary>
        /// <param name="code"></param>
        public static void StartFlow()
        {
            FlowRequest request = new FlowRequest("Market_TestFlow");
            Activity flowObject = new Market_TestFlow();

            WfDefine def = WfDefine.Get(request.DefineCode);

            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore(WfService.Instance.STORE_DB_CONNSTR);
            InstanceView view = instanceStore.Execute(instanceStore.CreateInstanceHandle(), new CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(30));
            instanceStore.DefaultInstanceOwner = view.InstanceOwner;

            IDictionary<string, object> inputs = new Dictionary<string, object>();
            inputs.Add("FlowState", new FlowState(request));
            // request.InnerRequest = new FlowRequest("Market_TestFlow", new Market_TestFlow());
            // inputs.Add("Request", request);

            WorkflowApplication application = new WorkflowApplication(flowObject, inputs);

            application.InstanceStore = instanceStore;

            application.PersistableIdle = (e) =>
            {
                instanceUnloaded.Set();
                return PersistableIdleAction.Unload;
            };

            application.Unloaded = (e) =>
            {
                instanceUnloaded.Set();
            };

            application.OnUnhandledException = (ex) =>
            {
                return UnhandledExceptionAction.Terminate;
            };

            application.Run();

            instanceUnloaded.WaitOne();
        }
    }
}
