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
using System.Threading;
using System.Activities.Presentation;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Activities.Presentation.View;
using System.Windows.Input;
using System.Activities.Statements;
using System.Activities.Core.Presentation;
using System.Activities;
using System.Windows.Media;

namespace PIC.Portal.Testing.Services
{
    [TestFixture]
    public class WfServiceTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();

            AuthService.AuthUser("admin", "12");
        }

        /// <summary>
        /// 保存流程执行状态图
        /// </summary>
        [Test]
        public void SaveTrackingImageTest()
        {
            var _ins = WfInstance.FindFirstByProperties("Status", "Started");

            var def = _ins.GetDefine();
            var wfConfig = def.GetConfig();

            var flowObject = wfConfig.NewFlowObject();

            var wfApp = new WorkflowApplication(flowObject);

            // DrawPicture(flowObject);

            new DesignerMetadata().Register();
            WorkflowDesigner designer = new WorkflowDesigner();

            designer.Load(wfApp);
        }

        [Test]
        public void StartFlowTest()
        {
            var basicInfo = GetFlowBasicInfo();
            var formData = GetFlowFormData();

            var opResult = WfService.Run(basicInfo, formData);

            Assert.IsTrue(opResult.Success);
        }

        [Test]
        public void RunFlowTest()
        {
            var formData = GetFlowFormData();
            formData.Set("BZ", "活动备注信息");

            var _ins = WfInstance.FindFirstByProperties("Status", "Started");

            var basicInfo = GetFlowBasicInfo();
            basicInfo.InstanceID = _ins.InstanceID;

            WfAction action = WfAction.FindFirstByProperties("Status", "New", "InstanceID", _ins.InstanceID);
            var actionInfo = new FlowActionInfo()
            {
                ActionID = action.ActionID,
                RouteCode = "Submit",
                Comments = "信息不完整"
            };

            WfService.Run(basicInfo, formData, actionInfo);
        }

        [Test]
        public void StartAndRunTest()
        {
            StartFlowTest();

            RunFlowTest();
        }

        [Test]
        public void WfHelperTest()
        {
            var fState = GetFlowState();

            // string actTitle = WfHelper.GetActionTitle(fState);

            //VelocityEngine vEngine = new VelocityEngine();

            //ExtendedProperties props = new ExtendedProperties();
            //vEngine.Init(props);

            //VelocityContext context = new VelocityContext();
            //context.Put("FlowState", fState);
            //context.Put("FlowRequest", fReq);
            //context.Put("TaskState", fState.Current);

            //StringWriter writer = new StringWriter();
            //vEngine.Evaluate(context, writer, null, fReq.ActionTitleFormat);

            //string str = writer.GetStringBuilder().ToString();
        }

        [Test]
        public void WfActorHelperTest()
        {
            IList<OrgUser> usrList = new List<OrgUser>();
            FlowState fstate = GetFlowState();

            // User Actor
            string userActorsStr = "[{UserIds:'003BC650-B2A0-46B8-AB87-6A93036F2A34,0040E1E3-2D8A-440D-9C20-887601CD6D31'}, {UserCode:'200326,200219,199520'}]";

            var userActors = JsonHelper.GetObject<WfActorCollection>(userActorsStr);
            usrList = userActors.GetUserList();

            Assert.IsTrue(usrList.Count > 0);

            // Role Actor
            string roleActorsStr = "[{RoleCode:'FZR',GroupCode:'DB'}]";

            var roleActors = JsonHelper.GetObject<WfActorCollection>(roleActorsStr);
            usrList = roleActors.GetUserList();

            Assert.IsTrue(usrList.Count > 0);

            roleActorsStr = "[{RoleCode:'${FormData.Get('GWCode')}',GroupCode:'${FormData.Get('BMCode')}'}]";
            roleActorsStr = WfHelper.GetWfDataString(roleActorsStr, flowState: fstate);

            roleActors = JsonHelper.GetObject<WfActorCollection>(roleActorsStr);
            usrList = roleActors.GetUserList();

            Assert.IsTrue(usrList.Count > 0);

            // Group Actor
            string groupActorsStr = "[{GroupCode:'DB',Tag:{RoleCode:'FZR'}}]";

            var groupActors = JsonHelper.GetObject<WfActorCollection>(groupActorsStr);
            usrList = groupActors.GetUserList();

            Assert.IsTrue(usrList.Count > 0);

            // Tmpl Actor
            string funcActorsStr = "[{FuncCode:'GetReportTo',Tag:{Ids:'${FlowState.FormData.Get('YGID')}'}}]";
            funcActorsStr = WfHelper.GetWfDataString(funcActorsStr, flowState: fstate);

            var funcActors = JsonHelper.GetObject<WfActorCollection>(funcActorsStr);
            usrList = funcActors.GetUserList();

            Assert.IsTrue(usrList.Count > 0);
        }

        public void FlowStateSerialize()
        {
            var flowStateStr = "{\"IsFirstEnter\":true,\"Request\":{\"Tag\":{},\"TaskRequest\":{\"Tag\":{},\"RouteCode\":\"Initialized\",\"Title\":null},\"DefineCode\":\"HR_EmployeeDismiss\",\"InstanceID\":\"12977a41-3fb5-4e82-8f5c-a1ff011f4b3a\",\"FlowObjectType\":\"PIC.Biz.Flow.EmployeeDismissFlow, PIC.Biz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"ModelID\":null,\"Title\":null,\"ActionTitleFormat\":null,\"FormPath\":null,\"FormData\":{\"SBSJ\":\"7/18/2013 5:25:53 PM\",\"YGXM\":\"admin\",\"YGWorkNo\":\"E0001\",\"YGID\":\"46C5F4DF-F6D1-4B36-96AC-D39D3DD65A5D\",\"XB\":\"M\",\"CSNY\":\"1983-2-14\",\"BZLB\":\"ZB\",\"BM\":\"ZB\",\"BMID\":\"17d01c06-eb65-4f20-ad1e-a1f100bb652a\",\"BMCode\":\"JW\",\"GWCode\":\"FZR\",\"GWName\":\"FZR\",\"TGSJ\":\"2013-5-18\",\"CBSJ\":\"2013-6-1\",\"BZ\":\"BZXX\",\"JPLY\":\"SBCG\",\"BMLDYJ\":\"\",\"ZGLDYJ\":\"\"}}";
            var flowState = JsonHelper.GetObject<FlowState>(flowStateStr);

            Assert.IsNotNull(flowState);

            var flowInfoStr = "{\"FlowState\": {\"IsFirstEnter\":true,\"Request\":{\"Tag\":{},\"TaskRequest\":{\"Tag\":{},\"RouteCode\":\"Initialized\",\"Title\":null},\"DefineCode\":\"HR_EmployeeDismiss\",\"InstanceID\":\"12977a41-3fb5-4e82-8f5c-a1ff011f4b3a\",\"FlowObjectType\":\"PIC.Biz.Flow.EmployeeDismissFlow, PIC.Biz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"ModelID\":null,\"Title\":null,\"ActionTitleFormat\":null,\"FormPath\":null,\"FormData\":{\"SBSJ\":\"7/18/2013 5:25:53 PM\",\"YGXM\":\"admin\",\"YGWorkNo\":\"E0001\",\"YGID\":\"46C5F4DF-F6D1-4B36-96AC-D39D3DD65A5D\",\"XB\":\"M\",\"CSNY\":\"1983-2-14\",\"BZLB\":\"ZB\",\"BM\":\"ZB\",\"BMID\":\"17d01c06-eb65-4f20-ad1e-a1f100bb652a\",\"BMCode\":\"JW\",\"GWCode\":\"FZR\",\"GWName\":\"FZR\",\"TGSJ\":\"2013-5-18\",\"CBSJ\":\"2013-6-1\",\"BZ\":\"BZXX\",\"JPLY\":\"SBCG\",\"BMLDYJ\":\"\",\"ZGLDYJ\":\"\"}},\"Current\":null}}";
            var flowInfo = JsonHelper.GetObject<FlowInfo>(flowInfoStr);

            Assert.IsNotNull(flowInfo.FlowState);


            var insStateStr = "{\"Tag\":null,\"WfInstanceID\":\"418dc6d0-d59c-4c56-824a-da149a3c58e5\",\"TaskCode\":null,\"WfFlowObjectType\":\"PIC.Biz.Flow.EmployeeDismissFlow, PIC.Biz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"WfFlowInfo\":{\"FlowState\":{\"IsFirstEnter\":true,\"Request\":{\"Tag\":{},\"TaskRequest\":{\"Tag\":{},\"RouteCode\":\"Initialized\",\"Title\":null},\"DefineCode\":\"HR_EmployeeDismiss\",\"InstanceID\":\"12977a41-3fb5-4e82-8f5c-a1ff011f4b3a\",\"FlowObjectType\":\"PIC.Biz.Flow.EmployeeDismissFlow, PIC.Biz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"ModelID\":null,\"Title\":null,\"ActionTitleFormat\":null,\"FormPath\":null,\"FormData\":{\"SBSJ\":\"7/18/2013 5:25:53 PM\",\"YGXM\":\"admin\",\"YGWorkNo\":\"E0001\",\"YGID\":\"46C5F4DF-F6D1-4B36-96AC-D39D3DD65A5D\",\"XB\":\"M\",\"CSNY\":\"1983-2-14\",\"BZLB\":\"ZB\",\"BM\":\"ZB\",\"BMID\":\"17d01c06-eb65-4f20-ad1e-a1f100bb652a\",\"BMCode\":\"JW\",\"GWCode\":\"FZR\",\"GWName\":\"FZR\",\"TGSJ\":\"2013-5-18\",\"CBSJ\":\"2013-6-1\",\"BZ\":\"BZXX\",\"JPLY\":\"SBCG\",\"BMLDYJ\":\"\",\"ZGLDYJ\":\"\"}},\"Current\":null}}}";
            var insState = JsonHelper.GetObject<WfInstanceState>(insStateStr);

            Assert.IsNotNull(insState.WfFlowInfo.FlowState);
        }

        #region Support Methods

        void DrawPicture(object flowInstance)
        {
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            Thread thread = new Thread(new ParameterizedThreadStart((args) =>
            {
                new DesignerMetadata().Register();
                WorkflowDesigner designer = new WorkflowDesigner();

                designer.Load(flowInstance);

                //double DPI = 96.0;

                //Rect size = VisualTreeHelper.GetDescendantBounds(designer.View);
                //int imageWidth = 800;// (int)size.Width;
                //int imageHeight = 800;// (int)size.Height;

                //RenderTargetBitmap renderBitmap = new RenderTargetBitmap(imageWidth, imageHeight, DPI, DPI, PixelFormats.Pbgra32);
                //renderBitmap.Render(designer.View);
                //BitmapFrame bf = BitmapFrame.Create(renderBitmap);

                //using (FileStream fs = new FileStream(@"c:\tmp\test.jpg", FileMode.OpenOrCreate))
                //{
                //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                //    encoder.Frames.Add(BitmapFrame.Create(bf));
                //    encoder.Save(fs);
                //    fs.Close();
                //}

                //Window win = new Window();
                //win.Show();
                //((RoutedCommand)DesignerView.CopyAsImageCommand).Execute(null, designer.Context.Services.GetService<DesignerView>());

                //win.Content = (object)designer.View;
                //win.Show();

                //System.Windows.Interop.InteropBitmap m = (System.Windows.Interop.InteropBitmap)Clipboard.GetImage();

                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(m));
                //using (FileStream fs = new FileStream(@"C:\tmp\out.jpg", FileMode.OpenOrCreate))
                //{
                //    encoder.Save(fs);
                //}

                waitHandler.Set();
            }));

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(waitHandler);

            waitHandler.WaitOne();
        }

        public FlowBasicInfo GetFlowBasicInfo()
        {
            WfDefine fdef = WfDefine.Get("EmployeeDismissFlow2");

            var basicInfo = fdef.GetInitBasicInfo();
            basicInfo.Title = "我的离职申请";

            return basicInfo;
        }

        public FlowFormData GetFlowFormData()
        {
            var formData = new FlowFormData();
            formData.Set("SBSJ", DateTime.Now.ToString());
            formData.Set("YGXM", "admin");
            formData.Set("YGWorkNo", "E0001");
            formData.Set("YGID", "46C5F4DF-F6D1-4B36-96AC-D39D3DD65A5D");
            formData.Set("XB", "Male");
            formData.Set("XB", "M");
            formData.Set("CSNY", "1983-2-14");
            formData.Set("BZLB", "在编");
            formData.Set("BM", "在编");
            formData.Set("BMID", "17d01c06-eb65-4f20-ad1e-a1f100bb652a");
            formData.Set("BMCode", "JW");
            formData.Set("GWCode", "FZR");
            formData.Set("GWName", "负责人");
            formData.Set("TGSJ", "2013-5-18");
            formData.Set("CBSJ", "2013-6-1");
            formData.Set("BZ", "备注信息");
            formData.Set("JPLY", "上班炒股");

            formData.Set("BMLDYJ", "");
            formData.Set("ZGLDYJ", "");

            return formData;
        }

        public FlowRequest GetFlowRequest()
        {
            FlowRequest fReq = new FlowRequest("HR_EmployeeDismiss");

            fReq.FormData = new FlowFormData();
            fReq.FormData.Set("SBSJ", DateTime.Now.ToString());
            fReq.FormData.Set("YGXM", "admin");
            fReq.FormData.Set("YGWorkNo", "E0001");
            fReq.FormData.Set("YGID", "46C5F4DF-F6D1-4B36-96AC-D39D3DD65A5D");
            fReq.FormData.Set("XB", "Male");
            fReq.FormData.Set("XB", "M");
            fReq.FormData.Set("CSNY", "1983-2-14");
            fReq.FormData.Set("BZLB", "在编");
            fReq.FormData.Set("BM", "在编");
            fReq.FormData.Set("BMID", "17d01c06-eb65-4f20-ad1e-a1f100bb652a");
            fReq.FormData.Set("BMCode", "JW");
            fReq.FormData.Set("GWCode", "FZR");
            fReq.FormData.Set("GWName", "负责人");
            fReq.FormData.Set("TGSJ", "2013-5-18");
            fReq.FormData.Set("CBSJ", "2013-6-1");
            fReq.FormData.Set("BZ", "备注信息");
            fReq.FormData.Set("JPLY", "上班炒股");

            fReq.FormData.Set("BMLDYJ", "");
            fReq.FormData.Set("ZGLDYJ", "");

            return fReq;
        }

        public FlowState GetFlowState()
        {
            FlowRequest fReq = GetFlowRequest();
            fReq.BasicInfo.ActionTitleFormat = "${FlowState.Title} - ${TaskState.ActionTitle} - ${FlowState.FormData.Get('BKey')}";
            fReq.Tag.Set("Key", "Value1");
            fReq.BasicInfo.Title = "12345";

            FlowState fState = new FlowState(fReq);
            fState.Current = new TaskState("A");

            return fState;
        }

        #endregion
    }
}
