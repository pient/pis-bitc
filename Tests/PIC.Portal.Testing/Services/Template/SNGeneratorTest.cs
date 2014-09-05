using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PIC.Portal.Model;

namespace PIC.Portal.Testing.Services
{
    [TestFixture]
    public class SNGeneratorTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();
        }

        [Test]
        public void IdentifierGeneratorTest()
        {
            int startMillisecond = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

            //Examination exam = new Examination();
            //exam.Create();

            //Console.WriteLine(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond - startMillisecond);

            //Assert.IsNotNull(exam.Id);

            //exam.Delete();

            Console.WriteLine(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond - startMillisecond);
        }

        [Test]
        public void SelfIncreaseGeneratorTest()
        {
            int startMillisecond = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

            string gstr = String.Empty;

            SelfIncreaseGenerator sicGenerator = new SelfIncreaseGenerator(SelfIncreaseGenerator.IncreaseType.Base16);

            gstr = sicGenerator.Generate();
            Assert.AreEqual(String.Empty, sicGenerator.Generate());

            sicGenerator.MaxSN = "A";
            sicGenerator.SNLength = 1;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("B", gstr);

            sicGenerator.MaxSN = "A";
            sicGenerator.SNLength = 3;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("00B", gstr);

            sicGenerator.MaxSN = "F";
            sicGenerator.SNLength = 3;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("010", gstr);

            sicGenerator.MaxSN = "X00F";
            sicGenerator.SNLength = 3;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("X010", gstr);

            sicGenerator = new SelfIncreaseGenerator(SelfIncreaseGenerator.IncreaseType.Base36);
            sicGenerator.MaxSN = "F";
            sicGenerator.SNLength = 1;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("G", gstr);

            sicGenerator.MaxSN = "F";
            sicGenerator.SNLength = 3;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("00G", gstr);

            sicGenerator.MaxSN = "00Z";
            sicGenerator.SNLength = 3;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("010", gstr);

            sicGenerator.MaxSN = "X00Z";
            sicGenerator.SNLength = 3;
            gstr = sicGenerator.Generate();
            Assert.AreEqual("X010", gstr);

            Console.WriteLine(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond - startMillisecond);
        }

        [Test]
        public void NumberGeneratorTest()
        {
            int startMillisecond = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

            string gstr = String.Empty;

            NumberGenerator numGenerator = new NumberGenerator();

            gstr = numGenerator.Generate();
            Assert.AreEqual(String.Empty, gstr);

            numGenerator.TemplateString = "R01PTSU10000001";
            gstr = numGenerator.Generate();
            Assert.AreEqual("R01PTSU10000001", gstr);

            numGenerator.TemplateString = @"R01PTSU10000001-$Date.Year";
            gstr = numGenerator.Generate();

            Assert.AreEqual("R01PTSU10000001-" + DateTime.Now.Year, gstr);

            numGenerator.TemplateString = "R01PTSU10000001-$Service.GetIncreasedCode(\"Base36\", \"A\", 3)";
            gstr = numGenerator.Generate();

            Assert.AreEqual("R01PTSU10000001-" + SNService.Instance.GetIncreasedCode("Base36", "A", 3), gstr);

            Console.WriteLine(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond - startMillisecond);
        }
    }
}
