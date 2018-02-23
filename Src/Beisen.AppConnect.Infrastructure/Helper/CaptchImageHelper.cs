using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public static class CaptchaImageHelper
    {
        private static readonly string[] _strArr = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "Y", "Z", "2", "3", "4", "5", "6", "7", "8", "9" };
        //定义验证码字体集
        private static readonly string[] _familyNames = new string[] { "Airal" };

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <param name="captcha">out型字符串，返回验证码文本内容</param>
        /// <returns>验证码图片，颜色为系统随机，背景色为浅色文本颜色</returns>
        public static byte[] CreatBitmap(out string captcha)
        {
            //系统颜色数组
            Color[] colors = {
                                 Color.Red,
                                 Color.Green,
                                 Color.Blue
                             };

            Color[] backColors = {
                                     //Color.FromArgb(212, 242, 232),
                                     //Color.FromArgb(253, 250, 233),
                                     //Color.FromArgb(234, 220, 246),
                                     //Color.FromArgb(242, 208, 207),
                                     Color.FromArgb(0,Color.White)
                                 };

            var seed = Guid.NewGuid().GetHashCode();
            var rand = new Random(seed);
            var color = colors[rand.Next(0, colors.Length)];
            var backgroundColor = backColors[rand.Next(0, backColors.Length)];

            var map = CreateBitmap(out captcha, backgroundColor, colors, (ushort?)rand.Next(3, 6), true);

            var m = new MemoryStream();
            map.Save(m, ImageFormat.Png);
            return m.ToArray();
        }

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <param name="captcha">out型字符串，返回验证码文本内容</param>
        /// <param name="backgroundColor">背景颜色</param>
        /// <param name="color">字体颜色</param>
        /// <param name="characterNumber">验证码文字个数（空则默认为5，长度为5-16位）</param>
        /// <param name="isRotate">是否扭曲文字图片</param>
        /// <returns>验证码图片</returns>
        private static Bitmap CreateBitmap(out string captcha, Color? backgroundColor, Color[] colors, ushort? characterNumber, bool isRotate)
        {
            //获取图片数组
            var bitmaps = GetBitmaps(out captcha, characterNumber, isRotate);
            //合并图片数组，组成一个大图
            var bitmap = SynthesisImage(bitmaps, backgroundColor);
            //设置文字颜色
            SetColor(bitmap, colors);
            return bitmap;
        }

        /// <summary>
        /// 获取图片组
        /// </summary>
        /// <param name="captcha">out型字符串，返回验证码文本内容</param>
        /// <param name="characterNumber">验证码文字个数（空则默认为5，长度为5-16位）</param>
        /// <param name="isRotate">是否扭曲文字图片</param>
        /// <returns>图片数组</returns>
        private static Bitmap[] GetBitmaps(out string captcha, ushort? characterNumber, bool isRotate)
        {
            if (characterNumber == null || characterNumber < 4)
            {
                characterNumber = 4;
            }
            if (characterNumber > 16)
            {
                characterNumber = 16;
            }

            var bitmaps = new Bitmap[Convert.ToInt32(characterNumber)];
            var code = new string[bitmaps.Length];

            captcha = string.Empty;

            var seed = Guid.NewGuid().GetHashCode();
            var random = new Random(seed);

            for (var i = 0; i < characterNumber; i++)
            {
                bitmaps[i] = new Bitmap(60, 58, PixelFormat.Format32bppArgb);//图片大小为60*60，现为固定值

                //分别生成图片
                code[i] = _strArr[random.Next(0, _strArr.Length)];
                captcha += code[i];
                bitmaps[i].MakeTransparent();
                using (var graphics = Graphics.FromImage(bitmaps[i]))
                {
                    var font = new Font(_familyNames[random.Next(0, _familyNames.Length)], random.Next(25, 33));//随机字体和文字大小
                    Brush brush = new SolidBrush(Color.Black);
                    var point = new Point(0, 0);
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawString(code[i], font, brush, point);
                    graphics.Save();
                    graphics.Dispose();
                    if (isRotate)//旋转
                    {
                        bitmaps[i] = RotateImg(bitmaps[i], random.Next(-25, 25));
                    }
                }
            }
            return bitmaps;
        }

        /// <summary>
        /// 设置图片文字颜色，必须是Format32bppArgb的图片才能成功，部分算法来自网络
        /// </summary>
        /// <param name="bitmap">需要转换的图片</param>
        /// <param name="colors">文字颜色</param>
        private static void SetColor(Bitmap bitmap, Color[] colors)
        {
            if (colors == null)
                return;
            // Specify a pixel format.
            var pxf = bitmap.PixelFormat;
            if (pxf != PixelFormat.Format32bppArgb)
                return;
            // Lock the bitmap's bits.
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData =
            bitmap.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            // Get the address of the first line.
            var ptr = bmpData.Scan0;
            // Declare an array to hold the bytes of the bitmap.
            var numBytes = bitmap.Width * bitmap.Height * 4;
            var rgbValues = new byte[numBytes];
            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);
            // Manipulate the bitmap, such as changing the
            // blue value for every other pixel in the the bitmap.
            var random = new Random();
            for (var counter = 0; counter < numBytes; counter += 4)
            {
                var color = colors[random.Next(0, colors.Length)];

                //Format32bppArgb中，ARGB是逆序的
                rgbValues[counter] |= color.B;
                rgbValues[counter + 1] |= color.G;
                rgbValues[counter + 2] |= color.R;
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            // Unlock the bits.
            bitmap.UnlockBits(bmpData);
            //Marshal.FreeBSTR(ptr);
        }

        /// <summary>
        /// 扫描图片临界点
        /// 注意：此处不是通用方法，由于在生成图片时，使用的是全透明图片，字体颜色为黑色，
        /// 故 RGB均为0，A为255，此处省略了RGB比较。可用于全透明图片临界点扫描（未测试）。
        /// </summary>
        /// <param name="bitmaps">图片数组</param>
        /// <returns>临界点数组</returns>
        private static Point[,] GetCrossings(Bitmap[] bitmaps)
        {
            //初始化变量
            var points = new Point[bitmaps.Length - 1, 2];
            var height = bitmaps[0].Height;
            var width = bitmaps[0].Width;
            var bmpData = new BitmapData[bitmaps.Length];
            var PixelValues = new byte[bitmaps.Length][];
            var maxSubValue = new int[bitmaps.Length - 1];


            for (var i = 0; i < bitmaps.Length; i++)
            {
                var rect = new Rectangle(0, 0, width, height);
                bmpData[i] = bitmaps[i].LockBits(rect, ImageLockMode.ReadOnly, bitmaps[i].PixelFormat);
                var iPtr = bmpData[i].Scan0;
                //乘以4是由bitmap.PixelFormat决定的，因为这里使用的是PixelFormat.Format32bppArgb
                var iBytes = width * height * 4;
                PixelValues[i] = new byte[iBytes];
                Marshal.Copy(iPtr, PixelValues[i], 0, iBytes);
                bitmaps[i].UnlockBits(bmpData[i]);
                //Marshal.FreeBSTR(iPtr);
            }

            var iPoint = new int[bitmaps.Length];
            for (var i = 0; i < height; i++) //扫描每根横轴上的线
            {
                var leftPoints = new Point[bitmaps.Length];
                var rightPoints = new Point[bitmaps.Length];

                for (var k = 0; k < bitmaps.Length; k++)
                {
                    leftPoints[k] = new Point(0, i);
                    rightPoints[k] = new Point(0, i);
                    for (var j = 0; j < width; j++)
                    {
                        if (PixelValues[k][iPoint[k] + 3] > 10) //如果有像素点。
                        //注意：此处不是通用方法，由于在生成图片时，使用的是全透明图片，字体颜色为黑色，
                        //故 RGB均为0，A为255，此处省略了RGB比较。
                        {
                            if (leftPoints[k].X == 0) // 取做左边的点
                            {
                                leftPoints[k].X = j;
                            }
                            if (rightPoints[k].X < j) //取最右边的点
                            {
                                rightPoints[k].X = j;
                            }
                        }
                        iPoint[k] = iPoint[k] + 4;
                    }
                }

                for (var k = 0; k < bitmaps.Length - 1; k++)
                {
                    if (leftPoints[k + 1].X != 0 && rightPoints[k].X != 0) //保证此线上左边和右边的点都存在
                    {
                        var intMaxSubValue = rightPoints[k].X - leftPoints[k + 1].X;//取差额做大的点
                        if (intMaxSubValue > maxSubValue[k])
                        {
                            maxSubValue[k] = intMaxSubValue;
                            points[k, 0] = rightPoints[k];
                            points[k, 1] = leftPoints[k + 1];
                        }

                    }
                }

            }

            return points;
        }

        /// <summary>
        /// 根据原图和角度，得到旋转后的图片
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后的图片</returns>
        private static Bitmap RotateImg(Bitmap bitmap, int angle)
        {
            angle = angle % 360;
            //弧度转换
            var radian = angle * Math.PI / 180.0;
            var cos = Math.Cos(radian);
            var sin = Math.Sin(radian);
            //原图的宽和高
            var w = bitmap.Width;
            var h = bitmap.Height;
            var W = w;// (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            var H = h;// (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            var dsImage = new Bitmap(W, H);
            using (var graphics = Graphics.FromImage(dsImage))
            {
                //计算偏移量
                var Offset = new Point((W - w) / 2, (H - h) / 2);
                //构造图像显示区域：让图像的中心与窗口的中心点一致
                var rect = new Rectangle(Offset.X, Offset.Y, w, h);
                var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                graphics.TranslateTransform(center.X, center.Y);
                graphics.RotateTransform(360 - angle);
                //恢复图像在水平和垂直方向的平移
                graphics.TranslateTransform(-center.X, -center.Y);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bitmap, rect);
                //重至绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
                bitmap.Dispose();
            }
            return dsImage;

        }


        /// <summary>
        /// 通过图片集合，合并图片
        /// </summary>
        /// <param name="bitmaps"></param>
        /// <param name="IntersectionPoints"></param>
        /// <returns></returns>
        private static Bitmap SynthesisImage(Bitmap[] bitmaps, Color? backgroundColor)
        {
            var IntersectionPoints = GetCrossings(bitmaps);
            var moveValue = 0;
            var bitmapWidth = 148;// bitmaps[0].Width * bitmaps.Length / 2 - 10;//默认宽度为150
            var bitmap = new Bitmap(bitmapWidth, bitmaps[0].Height);
            //bitmap.MakeTransparent();
            using (var graphics = Graphics.FromImage(bitmap))
            {

                if (backgroundColor != null)//设置背景颜色
                {
                    graphics.Clear((Color)backgroundColor);
                }

                var width = 0;//大致计算文字宽度
                for (var i = 0; i < bitmaps.Length - 1; i++)
                {
                    width += (IntersectionPoints[i, 0].X - IntersectionPoints[i, 1].X);
                }
                width += 2 * (IntersectionPoints[0, 0].X - IntersectionPoints[0, 1].X);
                //计算除文字偏移，并画出文字
                for (var i = 0; i < bitmaps.Length; i++)
                {
                    if (i == 0)
                    {
                        moveValue = (bitmapWidth - width) / 2;
                    }
                    else if (i == 1)
                    {
                        moveValue += (IntersectionPoints[i - 1, 0].X - IntersectionPoints[i - 1, 1].X);
                    }
                    else
                    {
                        moveValue += (IntersectionPoints[i - 1, 0].X - IntersectionPoints[i - 1, 1].X);
                    }
                    graphics.DrawImage(bitmaps[i], new Point(moveValue - 10, 3));
                }

                #region 干扰线
                // Create pen.
                var blackPen = new Pen(Color.Black, 2);

                // Create points for curve.
                var seed = Guid.NewGuid().GetHashCode();
                var random = new Random(seed);
                var start = new Point(random.Next(0, bitmap.Width / 8), random.Next(5, bitmap.Height - 5));
                var control1 = new Point(random.Next(bitmap.Width / 4, bitmap.Width / 4 * 2), random.Next(5, bitmap.Height - 5));
                var control2 = new Point(random.Next(bitmap.Width / 4 * 2, bitmap.Width / 4 * 3), random.Next(5, bitmap.Height - 5));
                var end = new Point(random.Next(bitmap.Width / 4 * 3, bitmap.Width - 5), random.Next(5, bitmap.Height - 5));
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // Draw arc to screen.
                graphics.DrawBezier(blackPen, start, control1, control2, end);
                #endregion

                #region 画图片的前景噪音点

                //for (int i = 0; i < 100; i++)
                //{
                //    int x = random.Next(bitmap.Width);
                //    int y = random.Next(bitmap.Height);

                //    bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
                //}

                #endregion

                //graphics.DrawRectangle(new Pen(Color.Black, 1), new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));

                graphics.Save();
            }
            return bitmap;
        }
    }
}
