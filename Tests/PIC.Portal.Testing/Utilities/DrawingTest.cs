using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Aspose.Imaging;
using Aspose.Imaging.Shapes;
using System.IO;
using PIC.Portal.Utilities;
using PIC.Portal.Model;

namespace PIC.Portal.Testing.Utilities
{
    [TestFixture]
    public class DrawingTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();
        }

        [Test]
        public void DrawUserSignature_Test()
        {
            OrgUser user = OrgUser.FindFirstByProperties("Name", "许俊良");

            byte[] data = null;

            if (user != null)
            {
                data = user.GetSignatureData();

                Assert.IsTrue(data.Length > 0);
            }
        }

        [Test]
        public void DrawSignature_Test()
        {
            byte[] data = null;
            string text = "刘毅";

            Stream signStream = DrawingHelper.DrawSignature(text);

            data = CLRHelper.ReadStream(signStream);

            string filePath = @"c:/tmp/" + text + ".jpg";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (FileStream fstream = new FileStream(filePath, FileMode.CreateNew))
            {
                fstream.Write(data, 0, data.Length);

                data = CLRHelper.ReadStream(fstream);
            }

            byte[] data2 = File.ReadAllBytes(filePath);
            Assert.IsTrue(data2.Length == data.Length);
        }

        [Test]
        public void StringDrawing_Test()
        {
            string dir = @"c:\tmp";

            byte[] data = null;
            byte[] data2 = null;
            byte[] datam = null;

            string filePath = DrawSignature("刘毅", dir);

            using (FileStream fstream = new FileStream(filePath, FileMode.Open))
            {
                data = CLRHelper.ReadStream(fstream);
            }

            data2 = File.ReadAllBytes(filePath);

            using (MemoryStream mstream = new MemoryStream(data))
            {
                datam = CLRHelper.ReadStream(mstream);
            }

            Assert.IsTrue(data.Length == data2.Length);
            Assert.IsTrue(data.Length == datam.Length);

            //DrawSignature("测试_Test_123_!", dir);
            //DrawSignature("许俊良", dir);
            //DrawSignature("哆啦A梦", dir);
            //DrawSignature("Bill Gates", dir);
        }

        public string DrawSignature(string text, string dir)
        {
            Aspose.Imaging.ImageOptions.JpegOptions imgOptions = new Aspose.Imaging.ImageOptions.JpegOptions();

            string path = Path.Combine(dir, text + ".jpg");

            //Create an instance of FileCreateSource and assign it to Source property
            imgOptions.Source = new Aspose.Imaging.Sources.FileCreateSource(path, false);

            int fontSize = 20;
            int xOffset = 5;
            int yOffset = 10;

            switch (text.Length)
            {
                case 1:
                case 2:
                    fontSize = 60;
                    yOffset = 40;
                    break;
                case 3:
                    fontSize = 50;
                    yOffset = 30;
                    break;
                case 4:
                    fontSize = 40;
                    yOffset = 20;
                    break;
                default:
                    if (text.Length <= 8)
                    {
                        fontSize = 30;
                        yOffset = 15;
                    }
                    break;
            }

            //Create an instance of Image 
            using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Create(imgOptions, 256, 128))
            {
                Graphics graphics = new Aspose.Imaging.Graphics(image);
                graphics.Clear(Aspose.Imaging.Color.White);

                Aspose.Imaging.Font font = new Aspose.Imaging.Font("Times New Roman", fontSize, Aspose.Imaging.FontStyle.Bold);
                
                Aspose.Imaging.Brushes.SolidBrush brush = new Aspose.Imaging.Brushes.SolidBrush()
                {
                    Color = Aspose.Imaging.Color.Black,
                    Opacity = 100
                };

                // graphics.DrawString(text, font, brush, new Aspose.Imaging.RectangleF(0, 0, 256, 128));
                graphics.DrawString(text, font, brush, new Aspose.Imaging.PointF(xOffset, image.Height / 2 - yOffset));

                image.Save();
            }

            return path;
        }
    }
}
