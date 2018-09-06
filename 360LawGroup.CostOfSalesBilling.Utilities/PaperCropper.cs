using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Utilities
{
    public static class ImageProcessing
    {
        public static Bitmap CoppedImage(Image originalImage, int x, int y, int width, int height)
        {
            var newBmpStream = new MemoryStream();
            var cropRect = new Rectangle(x, y, width, height);
            var target = new Bitmap(cropRect.Width, cropRect.Height);
            target.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
            using (var g = Graphics.FromImage(target))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(originalImage, new Rectangle(0, 0, target.Width, target.Height),
                    cropRect,
                    GraphicsUnit.Pixel);
                target.Save(newBmpStream, ImageFormat.Jpeg);
            }
            return new Bitmap(Image.FromStream(newBmpStream));
        }

        public static Bitmap ResizeImage(Image originalImage, int newWidth, int newHeight)
        {
            var newImage = new Bitmap(newWidth, newHeight);
            using (Graphics thumbGraph = Graphics.FromImage(newImage))
            {
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbGraph.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
    }
}