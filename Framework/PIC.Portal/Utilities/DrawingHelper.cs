using PIC.Doc.Model;
using PIC.Portal.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Utilities
{
    /// <summary>
    /// 画图帮助方法
    /// </summary>
    public class DrawingHelper
    {
        /// <summary>
        /// 获取签名数据
        /// </summary>
        /// <returns></returns>
        public static byte[] GetSignatureData(DataSignature sign)
        {
            byte[] data = null;

            MinFileInfo fileInfo = null;

            if (sign.File != null)
            {
                fileInfo = sign.File.GetMinFileInfo();
            }

            if (fileInfo != null && !String.IsNullOrEmpty(fileInfo.FileID))
            {
                DocFile docFile = DocFile.Find(fileInfo.FileID.ToGuid());
                data = Doc.DocService.Read(docFile);
            }

            if (data == null && !String.IsNullOrEmpty(sign.Name))
            {
                // 获取默认数据
                Stream signStream = DrawingHelper.DrawSignature(sign.Name);

                data = CLRHelper.ReadStream(signStream);
            }

            return data;
        }

        /// <summary>
        /// 将文字转换为签名图片
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Stream DrawSignature(string text)
        {
            Stream signStream = new MemoryStream();

            Aspose.Imaging.ImageOptions.JpegOptions imgOptions = new Aspose.Imaging.ImageOptions.JpegOptions();

            string filePath = Path.Combine(PortalService.TempFolder, Guid.NewGuid().ToString() + ".jpg");

            //Create an instance of FileCreateSource and assign it to Source property
            imgOptions.Source = new Aspose.Imaging.Sources.FileCreateSource(filePath, true);

            int fontSize = 20;
            int xOffset = 5;
            int yOffset = 10;

            // 根据文字数调整文字大小及位置
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
                Aspose.Imaging.Graphics graphics = new Aspose.Imaging.Graphics(image);
                graphics.Clear(Aspose.Imaging.Color.White);

                Aspose.Imaging.Font font = new Aspose.Imaging.Font("Times New Roman", fontSize, Aspose.Imaging.FontStyle.Bold);

                Aspose.Imaging.Brushes.SolidBrush brush = new Aspose.Imaging.Brushes.SolidBrush()
                {
                    Color = Aspose.Imaging.Color.Black,
                    Opacity = 100
                };

                // graphics.DrawString(text, font, brush, new Aspose.Imaging.RectangleF(0, 0, 256, 128));
                graphics.DrawString(text, font, brush, new Aspose.Imaging.PointF(xOffset, image.Height / 2 - yOffset));

                image.Save(signStream);
            }

            return signStream;
        }
    }
}
