using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public static class QrCodeHelper
    {
        public static byte[] Generate(string data, QrCodeImageSize size, string logoDfsPath = null)
        {
            return Generate(data, (int) size, logoDfsPath);
        }

        /// <summary>
        /// return png bytes data
        /// </summary>
        /// <param name="data">input data(url ETC.)</param>
        /// <param name="size">size,width and height</param>
        /// <param name="logoDfsPath">Dfs path for logo</param>
        /// <returns>bytes,png format</returns>
        public static byte[] Generate(string data, int size, string logoDfsPath = null)
        {
            var codeWriter = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions()
                {
                    Height = size,
                    Width = size,
                    Margin = 1,
                    CharacterSet = "UTF-8",
                    ErrorCorrection = ErrorCorrectionLevel.H
                }
            };
            using (var image = codeWriter.Write(data))
            {
                if (!string.IsNullOrEmpty(logoDfsPath))
                {
                    AddLogo(image, logoDfsPath);
                }
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);
                    var buffer = ms.ToArray();
                    return buffer;
                }
            }
        }

        private static void AddLogo(Bitmap image, string logoDfsPath)
        {
            using (var g = Graphics.FromImage(image))
            {
                var stream = new MemoryStream(DfsHelper.GetDataFromDfsPath(logoDfsPath));
                Image logoImage = Image.FromStream(stream);
                float maxWidth = image.Width*0.4f;
                float maxHeight = image.Height*0.4f;
                float xzoomIndicator = maxWidth/logoImage.Width;
                float yzoomIndicator = maxHeight/logoImage.Height;
                float zoomIndicator = Math.Min(xzoomIndicator, yzoomIndicator);
                var drawRect = new RectangleF((image.Width - (logoImage.Width*zoomIndicator))/2.0f, (image.Height - (logoImage.Height*zoomIndicator))/2.0f, logoImage.Width*zoomIndicator, logoImage.Height*zoomIndicator);
                g.DrawImage(logoImage, drawRect);
            }
        }
    }

    public enum QrCodeImageSize
    {
        Small = 240,
        Middle = 320,
        Large = 480,
        XLarge = 640
    }
}
