using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PIC.Portal.Model;
using PIC.Portal.Workflow;
using NVelocity.App;
using Commons.Collections;
using NVelocity;
using System.IO;
using System.Activities;
using System.Activities.Statements;
using PIC.Common;

namespace PIC.Portal.Testing.Services
{
    [TestFixture]
    public class WfHelperTest
    {
        [SetUp]
        public void Init()
        {
            PortalService.Initialize();
        }

        [Test]
        public void LoadTest()
        {
            var fullPath = Path.Combine(WfServiceConfig.FlowFileFolder, "./hr/EmployeeDismissFlow.xaml");

            if (File.Exists(fullPath))
            {
                var obj = WfHelper.LoadActivity(fullPath);

                Assert.IsNotNull(obj);
            }
        }

        [Test]
        public void WorkflowInspectionServicesTest()
        {
            var fdefine = WfDefine.Get("HR_EmployeeDismiss");

            var fchart = WfHelper.GetFlowchart(fdefine);
            Assert.IsNotNull(fchart);

            var fstep = WfHelper.GetFlowStep(fchart, "RSCZ_SP");
            Assert.IsNotNull(fstep);

            var ftask = WfHelper.GetTaskActivity(fchart, "RSCZ_SP");
            Assert.IsNotNull(ftask.TaskCode == "RSCZ_SP");

            var nextTasks = WfHelper.GetNextTaskStates(fchart, fstep);
            Assert.IsNotNull(nextTasks);

            ftask = WfHelper.GetTaskActivity(fdefine, "RSCZ_SP");
            Assert.IsNotNull(ftask.TaskCode == "RSCZ_SP");

            nextTasks = WfHelper.GetNextTaskStates(fdefine, "RSCZ_SP");
            Assert.IsNotNull(nextTasks);
        }

        #region Support Methods



        #endregion
    }
}
