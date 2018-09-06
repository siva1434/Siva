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
    public class ImageCropper
    {
        public Bitmap ResizeImage(System.Drawing.Image originalImage, int newWidth, int newHeight)
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