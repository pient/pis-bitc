using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PIC.Portal.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PIC.Portal.Template;

namespace PIC.Portal.Testing.Services.Template
{
    [TestFixture]
    public class TmplHelperTest
    {
        [SetUp]
        public void Init()
        {
            PortalService.Initialize();

            AuthService.AuthUser("admin", "12");
        }

        [Test]
        public void EasyDictionaryTest()
        {
            string dataCtxConfigStr = "{"
                + "'MsgList':{Type:'HqlQuery', Rows:5, String:'from Message msg'}"
                + ", 'FilePagePath':'/portal/Modules/Common/Doc/File.ashx'"
                + "}";            

            string tmplStr = "{DataContextString: " + dataCtxConfigStr + ", TemplateString: 'ABC'}";

            var obj = JsonConvert.DeserializeObject(dataCtxConfigStr);
            var dict = JsonConvert.DeserializeObject<EasyDictionary>(dataCtxConfigStr);

            var ctx = JsonConvert.DeserializeObject<StandardTemplateConfig>(tmplStr);
        }

        [Test]
        public void FileObjectDataTest()
        {
            var fileObj = new FileObjectData();

            fileObj.files.Add(new FileObject()
            {
                id = "123",
                istemp = true,
                name = "abc.jpg"
            });

            var jstr = fileObj.ToJsonString();

            var obj = JsonConvert.DeserializeObject(jstr);
        }
    }
}
