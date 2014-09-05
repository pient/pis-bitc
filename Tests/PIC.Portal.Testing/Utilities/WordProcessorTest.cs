using System;
using System.IO;
using System.Linq;
using Aspose.Words;
using NUnit.Framework;
using PIC.Portal;
using PIC.Portal.Workflow;
using PIC.Portal.Model;
using System.Collections.Generic;
using PIC.Portal.Template;
using NVelocity.App;
using PIC.Portal.Utilities;

namespace PIC.Component.Testing
{
    [TestFixture]
    public class WordProcessorTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();
        }

        [Test]
        public void DocOperation()
        {
            // The path to the documents directory.
            string dataDir = Path.GetFullPath(@"C:\tmp\");

            // Create a blank document.
            var doc = new Aspose.Words.Document();

            // DocumentBuilder provides members to easily add content to a document.
            var builder = new DocumentBuilder(doc);

            // Insert a table of contents at the beginning of the document.
            builder.InsertTableOfContents("\\o \"1-3\" \\h \\z \\u");
            builder.Writeln();

            // Insert some other fields.
            builder.Write("Page: ");
            builder.InsertField("PAGE");
            builder.Write(" of ");
            builder.InsertField("NUMPAGES");
            builder.Writeln();

            builder.Write("Date: ");
            builder.InsertField("DATE");

            // Start the actual document content on the second page.
            builder.InsertBreak(BreakType.SectionBreakNewPage);

            // Build a document with complex structure by applying different heading styles thus creating TOC entries.
            builder.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading1;

            builder.Writeln("Heading 1");

            builder.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading2;

            builder.Writeln("Heading 1.1");
            builder.Writeln("Heading 1.2");

            builder.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading1;

            builder.Writeln("Heading 2");
            builder.Writeln("Heading 3");

            // Move to the next page.
            builder.InsertBreak(BreakType.PageBreak);

            builder.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading2;

            builder.Writeln("Heading 3.1");

            builder.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading3;

            builder.Writeln("Heading 3.1.1");
            builder.Writeln("Heading 3.1.2");
            builder.Writeln("Heading 3.1.3");

            builder.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading2;

            builder.Writeln("Heading 3.2");
            builder.Writeln("Heading 3.3");

            Console.WriteLine("Updating all fields in the document.");

            if (File.Exists(dataDir + "test.jpg"))
            {
                builder.InsertImage(dataDir + "test.jpg");

                // builder.MoveToBookmark();
            }

            // Call the method below to update the TOC.
            doc.UpdateFields();

            doc.Save(dataDir + "Document Field Update Out.docx");
        }

        [Test]
        public void DocMerge()
        {
            // The path to the documents directory.
            string dataDir = Path.GetFullPath(@"C:\tmp\");

            FileStream stream = File.Open(dataDir + "Template.doc", FileMode.Open);

            Document doc = new Document(stream);

            // Fill the fields in the document with user data.
            doc.MailMerge.Execute(
                new string[] { "FullName", "Company" },
                new object[] { "James Bond", "MI5 Headquarters" });

            doc.MailMerge.Execute(
                new string[] { "Address", "Address2", "City" },
                new object[] { "Milbank", "", "London" });

            var builder = new DocumentBuilder(doc);

            if (File.Exists(dataDir + "test.jpg"))
            {
                builder.MoveToMergeField("Picture");

                var fileData = File.ReadAllBytes(dataDir + "test.jpg");
                builder.InsertImage(fileData, 100, 100);

                builder.MoveToMergeField("Picture");
            }

            // Saves the document to disk.
            doc.Save(dataDir + "MailMerge Result Out.docx");
        }

        /// <summary>
        /// 导出流程模板
        /// </summary>
        [Test]
        public void ExportWfDoc()
        {
            WfDefine def = WfDefine.FindFirstByProperties(WfDefine.Prop_Code, "RSGL04.05");
            WfInstance ins = WfInstance.FindFirstByProperties(WfInstance.Prop_DefineID, def.DefineID);

            WfDocTempalte docTmpl = new WfDocTempalte();

            if (def.DefineConfig.DocTemplates.Count > 0)
            {
                docTmpl = def.DefineConfig.DocTemplates.First();
            }

            string tmplDocTmplPath = WfHelper.GetFlowFilePath(docTmpl.Path);

            using (FileStream stream = File.Open(tmplDocTmplPath, FileMode.Open))
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(stream);
                doc.MailMerge.CleanupOptions = Aspose.Words.Reporting.MailMergeCleanupOptions.RemoveUnusedFields;

                // 开始合并
                string[] mergeFieldNames = doc.MailMerge.GetFieldNames();

                IList<string> strFieldNames = new List<string>();
                IList<object> strFieldObjs = new List<object>();

                IList<string> signFieldNames = new List<string>();
                IList<DataSignature> signFieldObjs = new List<DataSignature>();

                TmplContext tmplCtx = WfHelper.GetWfDataTmplContext(null, def, ins);

                NVelocity.Runtime.RuntimeInstance ri = new NVelocity.Runtime.RuntimeInstance();
                ri.Init();

                NVelocity.VelocityContext vCtx = tmplCtx.ToVelocityContext();

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
                        else if(fldObj is DataSignature)
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

                // Saves the document to disk.
                doc.Save(@"C:/tmp/TestDocOut.docx");
            }
        }


        /// <summary>
        /// 导出流程模板
        /// </summary>
        [Test]
        public void ExportWfDoc2()
        {
            WfDefine def = WfDefine.FindFirstByProperties(WfDefine.Prop_Code, "RSGL04.05");
            WfInstance ins = WfInstance.FindFirstByProperties(WfInstance.Prop_DefineID, def.DefineID);

            WfDocTempalte docTmpl = new WfDocTempalte();

            if (def.DefineConfig.DocTemplates.Count > 0)
            {
                docTmpl = def.DefineConfig.DocTemplates.First();

                WfHelper.SaveTmplDoc(ins, docTmpl, @"C:/tmp/TestDocOut.docx");
            }
        }
    }
}


