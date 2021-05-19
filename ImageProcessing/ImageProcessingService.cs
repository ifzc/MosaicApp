using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MosaicApp.ImageProcessing
{
    /// <summary>
    /// 图像处理服务
    /// </summary>
    public class ImageProcessingService : IImageProcessingService
    {
        /// <summary>
        /// 马赛克处理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> MosaicProcessAsync(MosaicProcessParam param)
        {
            var image = Base64ToImage(param.Base64);
            MosaicProcess(image, param.StartPoint, param.EndPoint);
            var base64 = await ImageToBase64Async(image, true);
            image.Dispose();
            return base64;
        }

        /// <summary>
        /// 水印处理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> MarkProcessAsync(MarkProcessParam param)
        {
            var image = Base64ToImage(param.Base64);
            MarkProcess(image, param.Mark);
            var base64 = await ImageToBase64Async(image, true);
            image.Dispose();
            return base64;
        }

        private static void MosaicProcess(Bitmap bitmap, string startPoint, string endPoint)
        {
            if (string.IsNullOrEmpty(startPoint) || string.IsNullOrEmpty(endPoint))
                throw new Exception("PointInvalid");

            var start = startPoint.Split(",").Select(p => Convert.ToInt32(p)).ToArray();
            var end = endPoint.Split(",").Select(p => Convert.ToInt32(p)).ToArray();

            if (start.Length != 2 || end.Length != 2)
                throw new Exception("PointInvalid");

            const int effectWidth = 10;

            for (var heightOffset = start[1]; heightOffset < end[1]; heightOffset += effectWidth)
                for (var widthOffset = start[0]; widthOffset < end[0]; widthOffset += effectWidth)
                {
                    int alpha = 0, red = 0, green = 0, blue = 0;
                    var blurPixelCount = 0;

                    for (var x = widthOffset; x < widthOffset + effectWidth && x < bitmap.Width; x++)
                        for (var y = heightOffset; y < heightOffset + effectWidth && y < bitmap.Height; y++)
                        {
                            var pixel = bitmap.GetPixel(x, y);
                            alpha = pixel.A;
                            red += pixel.R;
                            green += pixel.G;
                            blue += pixel.B;
                            blurPixelCount++;
                        }

                    // Average calculation range
                    alpha /= blurPixelCount;
                    red /= blurPixelCount;
                    green /= blurPixelCount;
                    blue /= blurPixelCount;

                    // Set this value in all ranges
                    for (var x = widthOffset; x < widthOffset + effectWidth && x < bitmap.Width; x++)
                        for (var y = heightOffset; y < heightOffset + effectWidth && y < bitmap.Height; y++)
                        {
                            var newColor = Color.FromArgb(alpha, red, green, blue);
                            bitmap.SetPixel(x, y, newColor);
                        }
                }
        }

        private static void MarkProcess(Image image, string mark)
        {
            if (string.IsNullOrEmpty(mark))
                throw new Exception("MarkInvalid");

            using var graphics = Graphics.FromImage(image);
            graphics.DrawImage(image, 0, 0, image.Width, image.Height);

            var size = Convert.ToInt32(16 + Math.Ceiling(image.Width / 1000.0) + Math.Ceiling(image.Height / 1000.0));

            var ft = new Font("Microsoft YaHei", size, FontStyle.Regular, GraphicsUnit.Pixel);
            graphics.DrawString(mark, ft, Brushes.Black,
                new Point(image.Width - size * (mark.Length + 1), image.Height - size * 2));
        }

        private static async Task<string> ImageToBase64Async(Image image, bool addType = false)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            var arr = new byte[ms.Length];
            ms.Position = 0;
            await ms.ReadAsync(arr, 0, (int)ms.Length);
            ms.Close();

            if (!addType) return Convert.ToBase64String(arr);

            const string type = "data:image/jpeg;base64,";
            return type + Convert.ToBase64String(arr);
        }

        private static Bitmap Base64ToImage(string base64)
        {
            if (base64.Contains(","))
                base64 = base64.Split(",").Last();

            var arr = Convert.FromBase64String(base64);
            var ms = new MemoryStream(arr);
            var bmp = new Bitmap(ms);
            return bmp;
        }
    }
}
