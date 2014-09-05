using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PIC.Portal
{
    public class WinImageRender
    {
        private readonly FrameworkElement _element;

        public string Format { get; set; }

        public WinImageRender(FrameworkElement element, string format = "jpg")
        {
            _element = element;
            Format = format;
        }

        /// <summary>
        /// Renders the wrapped frameworkelement to the clipboard
        /// </summary>
        public void SnapshotToClipboard()
        {
            ElementToClipboard(_element, Format);
        }

        /// <summary>
        /// Renders the wrapped frameworkelement to a bitmap
        /// </summary>
        /// <returns>The rendered bitmap</returns>
        public Bitmap SnapshotToBitmap()
        {
            using (var str = SnapshotToStream())
            {
                return (str == null) ? null : new Bitmap(str);
            }
        }

        /// <summary>
        /// Renders the wrapped frameworkelement to a bitmap stored in a memory stream
        /// </summary>
        /// <returns>Memorystream</returns>
        public Stream SnapshotToStream()
        {
            return ElementStream(_element, Format);
        }

        /// <summary>
        /// Renders a frameworkelement to the clipboard as a image
        /// </summary>
        /// <param name="element">the element to render</param>
        /// <param name="encoding">Bitmap format, values are "gif","png","jpg"</param>
        /// <returns></returns>
        public static void ElementToClipboard(FrameworkElement element, string encoding)
        {
            var bms = ElementSnapShot(element, encoding);
            Clipboard.SetImage(bms);
        }

        /// <summary>
        /// Renders a frameworkelement to a bitmap in a memorystream
        /// </summary>
        /// <param name="element">the element to render</param>
        /// <param name="encoding">Bitmap format, values are "gif","png","jpg"</param>
        /// <returns></returns>
        public static Stream ElementStream(FrameworkElement element, string encoding)
        {
            var rtb = RenderTarget(element);
            var encoder = EncoderFor(encoding);
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            var ms = new MemoryStream();
            encoder.Save(ms);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Renders a frameworkelement to a bitmapsource
        /// </summary>
        /// <param name="element">the element to render</param>
        /// <param name="encoding">Bitmap format, values are "gif","png","jpg"</param>
        /// <returns></returns>
        public static BitmapSource ElementSnapShot(FrameworkElement element, string encoding)
        {
            var rtb = RenderTarget(element);
            var encoder = EncoderFor(encoding);
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            return rtb;
        }

        private static BitmapEncoder EncoderFor(string format)
        {
            BitmapEncoder encoder;
            switch (format)
            {
                case "gif":
                case "GIF":
                    encoder = new GifBitmapEncoder();
                    break;
                case "png":
                case "PNG":
                    encoder = new PngBitmapEncoder();
                    break;
                case "jpg":
                case "JPG":
                    encoder = new JpegBitmapEncoder();
                    break;
                default:
                    encoder = new PngBitmapEncoder();
                    break;
            }
            return encoder;
        }

        private static RenderTargetBitmap RenderTarget(FrameworkElement element)
        {
            var rtb = new RenderTargetBitmap(Convert.ToInt32(element.RenderSize.Width), Convert.ToInt32(element.RenderSize.Height), 96, 96, PixelFormats.Pbgra32);
            var brush = new VisualBrush(element);
            var visual = new DrawingVisual();
            var context = visual.RenderOpen();
            context.DrawRectangle(brush, null, new Rect(new System.Windows.Point(0, 0), new System.Windows.Point(element.RenderSize.Width, element.RenderSize.Height)));
            context.Close();
            rtb.Render(visual);
            return rtb;
        }
    }
}
