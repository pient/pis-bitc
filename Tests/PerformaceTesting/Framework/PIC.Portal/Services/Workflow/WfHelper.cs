using System;
using System.Web;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using NVelocity;
using NVelocity.App;
using PIC.Portal.Model;
using PIC.Portal.Services;
using PIC.Portal.Template;
using PIC.Portal.Utilities;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程帮助类
    /// </summary>
    public class WfHelper
    {
        #region 流程配置

        /// <summary>
        /// 获取流程文件路径
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public static string GetFlowFilePath(string subPath)
        {
            if (String.IsNullOrEmpty(subPath))
            {
                return subPath;
            }

            var path = Path.Combine(Workflow.WfServiceConfig.FlowFileFolder, subPath);

            return path;
        }

        #endregion

        #region 流程定义

        /// <summary>
        /// 获取用户的流程代理人
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static MinUserInfo GetUserAgent(OrgUser user)
        {
            var cfg = user.GetBpmConfig();
            var currentDate = DateTime.Now;

            if (cfg.AgentStartDate != null 
                && cfg.AgentEndDate != null
                && currentDate >= cfg.AgentStartDate
                && currentDate <= cfg.AgentEndDate)
            {
                return new MinUserInfo()
                {
                    UserID = cfg.AgentID,
                    Name = cfg.AgentName
                };
            }

            return null;
        }

        /// <summary>
        /// 判断当前是否打回请求
        /// </summary>
        /// <returns></returns>
        public static bool IsAutoRejectRequest(FlowContext ctx)
        {
            if (!"Reject".Equals(ctx.ActionRequest.RouteCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var wfDef = WfDefine.Get(ctx.FlowState.DefineCode);

            if (wfDef != null)
            {
                var fchart = GetFlowchart(wfDef);
                var fstep = GetFlowStep(fchart, ctx.CurrentTask.Code);

                // 当前节点或下一节点都是TaskActivity
                if (fstep.Action is TaskActivity && fstep.Next is FlowStep && ((FlowStep)fstep.Next).Action is TaskActivity)
                {
                    var currTask = fstep.Action as TaskActivity;
                    var rejTask = GetDefaultRejectTaskActivity(fchart);

                    if (rejTask != null && rejTask.TaskCode != currTask.TaskCode)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取下一步可能的路径
        /// </summary>
        /// <param name="fdefine"></param>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public static EasyDictionary<string, TaskState> GetNextTaskStates(WfDefine fdefine, string taskCode)
        {
            Flowchart fChart = GetFlowchart(fdefine);

            FlowStep fStep = GetFlowStep(fChart, taskCode);

            EasyDictionary<string, TaskState> taskDict = new EasyDictionary<string, TaskState>();

            if (fStep != null)
            {
                taskDict = GetNextTaskStates(fChart, fStep);
            }

            return taskDict;
        }

        /// <summary>
        /// 获取下一个节点列表
        /// </summary>
        /// <param name="flowChart"></param>
        /// <param name="fstep"></param>
        /// <returns></returns>
        public static EasyDictionary<string, TaskState> GetNextTaskStates(Flowchart flowChart, FlowStep fstep)
        {
            EasyDictionary<string, TaskState> taskDict = new EasyDictionary<string, TaskState>();

            if (fstep.Next is FlowSwitch<string>)
            {
                var flowSwitch = fstep.Next as FlowSwitch<string>;

                foreach (var _case in flowSwitch.Cases)
                {
                    var _step = _case.Value as FlowStep;

                    if (_step != null && _step.Action is TaskActivity)
                    {
                        var _act = _step.Action as TaskActivity;
                        var _state = _act.TaskState;

                        if (String.IsNullOrEmpty(_act.TaskState.Name))
                        {
                            _act.TaskState.Name = _act.DisplayName;
                        }

                        taskDict.Set(_case.Key, _state);
                    }
                    else
                    {
                        taskDict.Set(_case.Key, null);
                    }
                }
            }
            else if (fstep.Next is FlowStep)
            {
                var nextStep = fstep.Next as FlowStep;

                var _act = nextStep.Action as TaskActivity;

                // 下一步活动为TaskActivity
                if (_act != null)
                {
                    // 默认增加提交节点为下一节点
                    var _state = _act.TaskState;

                    if (String.IsNullOrEmpty(_act.TaskState.Name))
                    {
                        _act.TaskState.Name = _act.DisplayName;
                    }

                    taskDict.Set("Submit", _state);

                    // 设置默认打回节点
                    var _rejAct = GetDefaultRejectTaskActivity(flowChart);

                    if (_rejAct != null && _act.TaskCode != _rejAct.TaskCode)
                    {
                        var _rejState = _rejAct.TaskState;

                        if (String.IsNullOrEmpty(_rejAct.TaskState.Name))
                        {
                            _rejAct.TaskState.Name = _rejAct.DisplayName;
                        }

                        taskDict.Set("Reject", _rejState);
                    }
                }
            }

            return taskDict;
        }

        /// <summary>
        /// 获取默认打回节点
        /// </summary>
        /// <param name="flowChart"></param>
        /// <returns></returns>
        public static TaskActivity GetDefaultRejectTaskActivity(Flowchart flowChart)
        {
            TaskActivity t_act = null;

            foreach (FlowNode fn in flowChart.Nodes)
            {
                var ts = fn as FlowStep;

                if (ts != null)
                {
                    var t = ts.Action as TaskActivity;

                    if (t != null && t.IsDefaultReject == true)
                    {
                        t_act = t;
                        break;
                    }
                }
            }

            return t_act;
        }

        /// <summary>
        /// 获取下一个活动
        /// </summary>
        /// <param name="flowChart"></param>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public static TaskActivity GetNextTaskActivity(Flowchart flowChart, string taskCode)
        {
            var fnode = GetNextFlowNode(flowChart, taskCode);

            if (fnode != null)
            {
                var fstep = fnode as FlowStep;

                if (fstep != null)
                {
                    return fstep.Action as TaskActivity;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取任务活动
        /// </summary>
        /// <param name="fdefine"></param>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public static TaskActivity GetTaskActivity(WfDefine fdefine, string taskCode)
        {
            var fChart = GetFlowchart(fdefine);

            TaskActivity t_act = GetTaskActivity(fChart, taskCode);

            return t_act;
        }

        public static TaskActivity GetTaskActivity(Flowchart flowChart, string taskCode)
        {
            TaskActivity t_act = null;

            foreach (FlowNode fn in flowChart.Nodes)
            {
                var ts = fn as FlowStep;

                if (ts != null)
                {
                    var t = ts.Action as TaskActivity;

                    if (t != null && t.TaskCode == taskCode)
                    {
                        t_act = t;
                        break;
                    }
                }
            }

            return t_act;
        }

        public static Flowchart GetFlowchart(WfDefine define)
        {
            Flowchart fChart = null;

            var flowObj = define.GetConfig().NewFlowObject();

            var list = WorkflowInspectionServices.GetActivities(flowObj).Where(a => a is Flowchart).ToList();

            if (list.Count() >= 1)
            {
                fChart = list.First() as Flowchart;
            }

            return fChart;
        }

        /// <summary>
        /// 获取下一个路径
        /// </summary>
        /// <param name="flowChart"></param>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public static FlowNode GetNextFlowNode(Flowchart flowChart, string taskCode)
        {
            var fstep = GetFlowStep(flowChart, taskCode);

            if (fstep != null)
            {
                return fstep.Next;
            }

            return null;
        }

        /// <summary>
        /// 从Flowchart中获取指定的TaskActivity
        /// </summary>
        /// <param name="flowChart"></param>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public static FlowStep GetFlowStep(Flowchart flowChart, string taskCode)
        {
            FlowStep tstep = null;

            foreach (FlowNode fn in flowChart.Nodes)
            {
                var ts = fn as FlowStep;

                if (ts != null)
                {
                    var t = ts.Action as TaskActivity;

                    if (t != null && t.TaskCode == taskCode)
                    {
                        tstep = ts;
                        break;
                    }
                }
            }

            return tstep;
        }

        /// <summary>
        /// 有Code和Text获取哦活动的OpString
        /// </summary>
        /// <param name="code"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetActionOpString(string code, string text)
        {
            return String.Format("[{{Code: '{0}', Text: '{1}'}}]", code, text);
        }

        /// <summary>
        /// 有Code和Text获取哦活动的OpString
        /// </summary>
        /// <param name="code"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetActionOpString(string code, string text, string tag)
        {
            return String.Format("[{{Code: '{0}', Text: '{1}', Tag: '{2}'}}]", code, text, tag);
        }

        #endregion

        #region 流程消息

        /// <summary>
        /// 发送任务提醒
        /// </summary>
        public static void SendTaskNotification(WfAction action)
        {
            var taskNotiTmpl = TmplHelper.GetTemplateConfig<EasyDictionary>("Sys.Msg.Bpm.TaskNotification");    // 任务通知
            var subjTmpl = taskNotiTmpl.Get<string>("Subject");
            var contTmpl = taskNotiTmpl.Get<string>("Content");

            var tmplCtx = GetWfDataTmplContext(action);

            var subjText = TmplHelper.Render(subjTmpl, tmplCtx);
            var contText = TmplHelper.Render(contTmpl, tmplCtx);

            // 通知任务所有人和代理人
            Message.SysSend(subjText, contText, null, null, action.OwnerID, action.AgentID);
        }

        /// <summary>
        /// 发送任务结束提醒
        /// </summary>
        public static void SendFlowFinishNotification(WfInstance wfInstance)
        {
            SendFlowNotification(wfInstance, "Sys.Msg.Bpm.TaskFinishNotification");
        }

        /// <summary>
        /// 发送任务终止提醒
        /// </summary>
        public static void SendTaskAbortNotification(WfInstance wfInstance)
        {
            SendFlowNotification(wfInstance, "Sys.Msg.Bpm.TaskAbortNotification");
        }

        /// <summary>
        /// 发送流程提醒
        /// </summary>
        public static void SendFlowNotification(WfInstance wfInstance, string tmplCode)
        {
            var taskNotiTmpl = TmplHelper.GetTemplateConfig<EasyDictionary>(tmplCode);    // 任务通知
            var subjTmpl = taskNotiTmpl.Get<string>("Subject");
            var contTmpl = taskNotiTmpl.Get<string>("Content");

            var tmplCtx = GetWfDataTmplContext(wfInstance);

            var subjText = TmplHelper.Render(subjTmpl, tmplCtx);
            var contText = TmplHelper.Render(contTmpl, tmplCtx);

            // 通知任务发起人
            Message.SysSend(subjText, contText, null, null, wfInstance.OwnerID);
        }

        #endregion

        #region WWF操作

        /// <summary>
        /// 强制流程跳转到指定节点
        /// </summary>
        /// <returns></returns>
        public static bool ForceJumpTo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载文件实例
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Activity LoadActivity(string filePath)
        {
            string tempString = "";
            var xamlWFString = new StringBuilder();
            var xamlStreamReader = new StreamReader(filePath);

            while (tempString != null)
            {
                tempString = xamlStreamReader.ReadLine();

                if (tempString != null)
                {
                    xamlWFString.Append(tempString);
                }
            }

            var wfInstance = ActivityXamlServices.Load(new StringReader(xamlWFString.ToString()));
            CompileExpressions((DynamicActivity)wfInstance);

            return wfInstance;
        }

        public static void CompileExpressions(DynamicActivity dynamicActivity)
        {
            // activityName is the Namespace.Type of the activity that contains the
            // C# expressions. For Dynamic Activities this can be retrieved using the
            // name property , which must be in the form Namespace.Type.

            string activityName = dynamicActivity.Name;

            // Split activityName into Namespace and Type.Append _CompiledExpressionRoot to the type name
            // to represent the new type that represents the compiled expressions.
            // Take everything after the last . for the type name.
            string activityType = activityName.Split('.').Last() + "_CompiledExpressionRoot";
            // Take everything before the last . for the namespace.
            string activityNamespace = string.Join(".", activityName.Split('.').Reverse().Skip(1).Reverse());

            // Create a TextExpressionCompilerSettings.
            TextExpressionCompilerSettings settings = new TextExpressionCompilerSettings
            {
                Activity = dynamicActivity,

                Language = "C#",
                ActivityName = activityType,
                ActivityNamespace = activityNamespace,
                RootNamespace = null,
                GenerateAsPartialClass = false,
                AlwaysGenerateSource = true,
                ForImplementation = true
            };

            // Compile the C# expression.
            TextExpressionCompilerResults results =
                new TextExpressionCompiler(settings).Compile();

            // Any compilation errors are contained in the CompilerMessages.
            if (results.HasErrors)
            {
                throw new Exception("Compilation failed.");
            }

            // Create an instance of the new compiled expression type.
            ICompiledExpressionRoot compiledExpressionRoot =
                Activator.CreateInstance(results.ResultType,
                    new object[] { dynamicActivity }) as ICompiledExpressionRoot;

            // Attach it to the activity.
            CompiledExpressionInvoker.SetCompiledExpressionRootForImplementation(
                dynamicActivity, compiledExpressionRoot);
        }

        /// <summary>
        /// 加载xaml文件后，编译DynamicActivity
        /// </summary>
        /// <param name="activity"></param>
        public static void CompileExpressions(Activity activity)
        {
            // activityName is the Namespace.Type of the activity that contains the
            // C# expressions.
            string activityName = activity.GetType().ToString();

            // Split activityName into Namespace and Type.Append _CompiledExpressionRoot to the type name
            // to represent the new type that represents the compiled expressions.
            // Take everything after the last . for the type name.
            string activityType = activityName.Split('.').Last() + "_CompiledExpressionRoot";
            // Take everything before the last . for the namespace.
            string activityNamespace = string.Join(".", activityName.Split('.').Reverse().Skip(1).Reverse());

            // Create a TextExpressionCompilerSettings.
            TextExpressionCompilerSettings settings = new TextExpressionCompilerSettings
            {
                Activity = activity,
                Language = "C#",
                ActivityName = activityType,
                ActivityNamespace = activityNamespace,
                RootNamespace = null,
                GenerateAsPartialClass = false,
                AlwaysGenerateSource = true,
                ForImplementation = false
            };

            // Compile the C# expression.
            TextExpressionCompilerResults results =
                new TextExpressionCompiler(settings).Compile();

            // Any compilation errors are contained in the CompilerMessages.
            if (results.HasErrors)
            {
                throw new Exception("Compilation failed.");
            }

            // Create an instance of the new compiled expression type.
            ICompiledExpressionRoot compiledExpressionRoot =
                Activator.CreateInstance(results.ResultType,
                    new object[] { activity }) as ICompiledExpressionRoot;

            // Attach it to the activity.
            CompiledExpressionInvoker.SetCompiledExpressionRoot(
                activity, compiledExpressionRoot);
        }

        #endregion

        #region 流程模板相关

        /// <summary>
        /// 获取活动标题
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static string GetActionTitle(FlowContext ctx)
        {
            string format = "$FlowState.Title - $TaskState.ActionTitle";

            if (!String.IsNullOrEmpty(ctx.FlowState.ActionTitleFormat))
            {
                format = ctx.FlowState.ActionTitleFormat;
            }

            string title = GetWfDataString(format, ctx);

            return title;
        }

        public static string GetWfDataString(string tmplString, FlowContext ctx = null,
            WfDefine flowDefine = null, WfInstance flowInstance = null, FlowState flowState = null, FlowRequest flowRequest = null,
            TaskState taskState = null, TaskRequest taskRequest = null, FlowBasicInfo basicInfo = null, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            var context = GetWfDataTmplContext(ctx, flowDefine, flowInstance, flowState, flowRequest,
                taskState, taskRequest, basicInfo, formData, actionInfo);

            var dataString = TmplHelper.Render(tmplString, context);

            return dataString;
        }

        /// <summary>
        /// 由活动获取上下文
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static TmplContext GetWfDataTmplContext(WfAction action)
        {
            WfInstance ins = action.GetWfInstance();

            TmplContext tmplCtx = GetWfDataTmplContext(ins);

            tmplCtx.Set("Action", action);

            if (action.ActionState != null)
            {
                tmplCtx.Set("ActionState", action.ActionState);
                if (action.ActionState.Request != null)
                {
                    tmplCtx.Set("Request", action.ActionState.Request);
                }
            }

            return tmplCtx;
        }

        /// <summary>
        /// 由流程实例获取上下文
        /// </summary>
        /// <param name="ins"></param>
        /// <returns></returns>
        public static TmplContext GetWfDataTmplContext(WfInstance ins)
        {
            TmplContext tmplCtx = GetWfDataTmplContext(null, null, ins);

            return tmplCtx;
        }

        /// <summary>
        /// 获取模板上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="flowDefine"></param>
        /// <param name="flowInstance"></param>
        /// <param name="flowState"></param>
        /// <param name="flowRequest"></param>
        /// <param name="taskState"></param>
        /// <param name="taskRequest"></param>
        /// <param name="basicInfo"></param>
        /// <param name="formData"></param>
        /// <param name="actionInfo"></param>
        /// <returns></returns>
        public static TmplContext GetWfDataTmplContext(FlowContext ctx = null,
            WfDefine flowDefine = null, WfInstance flowInstance = null, FlowState flowState = null, FlowRequest flowRequest = null,
            TaskState taskState = null, TaskRequest taskRequest = null, FlowBasicInfo basicInfo = null, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            if (ctx != null)
            {
                if (flowInstance == null)
                {
                    flowInstance = ctx.Instance;
                }

                if (flowState == null && ctx.FlowState != null)
                {
                    flowState = ctx.FlowState;
                }
            }

            if (flowInstance != null)
            {
                if (flowState == null)
                {
                    flowState = flowInstance.FlowState;
                }

                if (flowDefine == null)
                {
                    flowDefine = flowInstance.GetDefine();
                }
            }

            if (flowState != null)
            {
                if (flowRequest == null)
                {
                    flowRequest = flowState.Request;
                }

                if (taskState == null)
                {
                    taskState = flowState.Current;
                }

                if (basicInfo == null)
                {
                    basicInfo = flowState.BasicInfo;
                }

                if (formData == null)
                {
                    formData = flowState.FormData;
                }

                if (actionInfo == null)
                {
                    actionInfo = flowState.ActionInfo;
                }
            }

            var context = new TmplContext();

            context.Set("SystemInfo", PortalService.SystemInfo);

            if (PortalService.CurrentUserInfo != null)
            {
                context.Set("UserInfo", PortalService.CurrentUserInfo);

                OrgUser user = OrgUser.FindFirstByProperties(OrgUser.Prop_UserID, PortalService.CurrentUserInfo.UserID);
                if (user != null)
                {
                    context.Set("UserData", user);

                    OrgGroup dept = user.GetDept();
                    context.Set("DeptData", dept);
                }
            }

            context.Set("Context", ctx);
            context.Set("Instance", flowInstance);
            context.Set("Define", flowDefine);
            context.Set("FlowState", flowState);
            context.Set("FlowRequest", flowRequest);
            context.Set("TaskState", taskState);
            context.Set("TaskRequest", taskRequest);
            context.Set("BasicInfo", basicInfo);
            context.Set("FormData", formData);
            context.Set("ActionInfo", actionInfo);

            return context;
        }


        /// <summary>
        /// 输出文档
        /// </summary>
        /// <returns></returns>
        public static void SaveTmplDoc(WfInstance wfInstance, WfDocTempalte docTmpl, string fileName)
        {
            using (FileStream tmplStream = docTmpl.RetrieveFileStream())
            {
                Aspose.Words.Document doc = RetrieveTmplDoc(wfInstance, tmplStream);

                Aspose.Words.Saving.SaveOptions saveOptions = new Aspose.Words.Saving.DocSaveOptions()
                {
                    SaveFormat = SaveFormat.Doc
                };

                if (!StringHelper.IsEqualsIgnoreCase(Path.GetExtension(fileName), ".doc"))
                {
                    fileName = fileName + ".doc";
                }

                doc.Save(fileName);
            }
        } 

        /// <summary>
        /// 输出文档
        /// </summary>
        /// <returns></returns>
        public static void ResponseTmplDoc(WfInstance wfInstance, WfDocTempalte docTmpl, string fileName)
        {
            using (FileStream tmplStream = docTmpl.RetrieveFileStream())
            {
                Aspose.Words.Document doc = RetrieveTmplDoc(wfInstance, tmplStream);

                Aspose.Words.Saving.SaveOptions saveOptions = new Aspose.Words.Saving.DocSaveOptions()
                {
                    SaveFormat = SaveFormat.Doc
                };

                if (!StringHelper.IsEqualsIgnoreCase(Path.GetExtension(fileName), ".doc"))
                {
                    fileName = fileName + ".doc";
                }

                doc.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, saveOptions);
            }
        }

        /// <summary>
        /// 获取文档模板
        /// </summary>
        public static Aspose.Words.Document RetrieveTmplDoc(WfInstance wfInstance, FileStream tmplStream)
        {
            var doc = new Aspose.Words.Document(tmplStream);
            doc.MailMerge.CleanupOptions = Aspose.Words.Reporting.MailMergeCleanupOptions.RemoveUnusedFields;

            TmplContext tmplCtx = WfHelper.GetWfDataTmplContext(null, null, wfInstance);

            string[] mergeFieldNames = doc.MailMerge.GetFieldNames();

            IList<string> strFieldNames = new List<string>();
            IList<object> strFieldObjs = new List<object>();

            IList<string> signFieldNames = new List<string>();
            IList<DataSignature> signFieldObjs = new List<DataSignature>();

            // 开始合并
            NVelocity.VelocityContext vCtx = tmplCtx.ToVelocityContext();

            NVelocity.Runtime.RuntimeInstance ri = new NVelocity.Runtime.RuntimeInstance();
            ri.Init();

            foreach (var fldName in mergeFieldNames)
            {
                object fldObj = TmplHelper.Execute(fldName, vCtx, ri);

                if (fldObj != null)
                {
                    if (fldObj is string)
                    {
                        if (fldObj.ToString() != fldName)
                        {
                            strFieldNames.Add(fldName);
                            strFieldObjs.Add(fldObj);
                        }
                    }
                    else if (fldObj is DataSignature)
                    {
                        signFieldNames.Add(fldName);
                        signFieldObjs.Add(fldObj as DataSignature);
                    }
                }
            }

            // 导入签名信息
            if (signFieldNames.Count > 0)
            {
                var builder = new DocumentBuilder(doc);

                for (int i = 0; i < signFieldNames.Count; i++)
                {
                    string signFldName = signFieldNames[i];
                    DataSignature signFldObj = signFieldObjs[i];
                    byte[] signData = DrawingHelper.GetSignatureData(signFldObj);

                    builder.MoveToMergeField(signFldName);
                    builder.InsertImage(signData, 60, 30);
                }
            }

            // 合并文字
            if (strFieldNames.Count > 0)
            {
                doc.MailMerge.Execute(
                    strFieldNames.ToArray(),
                    strFieldObjs.ToArray());
            }

            return doc;
        }

        #endregion
    }
}
