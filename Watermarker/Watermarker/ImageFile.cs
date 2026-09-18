using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.Runtime.CompilerServices;

namespace Watermarker
{
    internal class ImageFile
    {
        private string _filePath;
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        private Image<Rgba32> OriginalImage;
        private Image<Rgba32> WatermarkedImage;
        private const int MaxSideLength = 20;

        public ImageFile()
        {
            _filePath = string.Empty;
            FileName = string.Empty;
            FileExtension = string.Empty;
            OriginalImage = null;
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                // Check is a valid file path and if the file exists
                if (System.IO.File.Exists(value))
                {
                    _filePath = value;
                    FileName = System.IO.Path.GetFileName(value);
                    FileExtension = System.IO.Path.GetExtension(value);
                    OriginalImage?.Dispose();
                    OriginalImage = Image.Load<Rgba32>(_filePath);
                }
            }
        }

        public void SaveWatermarkedImage(string watermark)
        {
            // Save the watermarked image to a new file
            string directory = System.IO.Path.GetDirectoryName(_filePath);
            string newFileName = $"{System.IO.Path.GetFileNameWithoutExtension(_filePath)}_watermarked{FileExtension}";
            string newFilePath = System.IO.Path.Combine(directory, newFileName);
            WatermarkedImage.Save(newFilePath);
            Console.WriteLine($"Watermarked image saved to: {newFilePath}");

            // Open the new image file in the default image viewer
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(newFilePath) { UseShellExecute = true });
        }
        public Image<Rgba32> GetWatermarkedImageType1(string watermark, float alpha)
        {
            WatermarkedImage?.Dispose();
            WatermarkedImage = OriginalImage.Clone();

            // Add watermark to the image
            Font baseFont = SystemFonts.CreateFont("Arial", 96);

            FontRectangle textSize = TextMeasurer.MeasureAdvance(watermark, new TextOptions(baseFont));

            double diagonalLength = Math.Sqrt(WatermarkedImage.Width * WatermarkedImage.Width + WatermarkedImage.Height * WatermarkedImage.Height);

            Font adaptedFont = SystemFonts.CreateFont("Arial", (float)(96 * diagonalLength / textSize.Width) * .85f);

            RichTextOptions textOptions = new RichTextOptions(adaptedFont)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            PointF startPoint = new PointF(WatermarkedImage.Width / 2, WatermarkedImage.Height / 2);
            PointF endPoint = new PointF(WatermarkedImage.Width, WatermarkedImage.Height);
            IPath diagonalPath = new SixLabors.ImageSharp.Drawing.Path(new LinearLineSegment(startPoint, endPoint));

            WatermarkedImage.Mutate(ctx => ctx.Paint(CanvasAction =>
            {
                CanvasAction.DrawText(textOptions, watermark, diagonalPath, Brushes.Solid(Color.White.WithAlpha(alpha)), pen: null);
            }));

            return WatermarkedImage;
        }

        public Image<Rgba32> GetWatermarkedImageType2(string watermark, float alpha)
        {
            WatermarkedImage?.Dispose();
            WatermarkedImage = OriginalImage.Clone();

            // Add watermark to the image
            Font baseFont = SystemFonts.CreateFont("Arial", 96);

            FontRectangle textSize = TextMeasurer.MeasureAdvance(watermark, new TextOptions(baseFont));

            double diagonalLength = Math.Sqrt(WatermarkedImage.Width * WatermarkedImage.Width + WatermarkedImage.Height * WatermarkedImage.Height);

            Font adaptedFont = SystemFonts.CreateFont("Arial", (float)(96 * diagonalLength / textSize.Width) * .85f);

            RichTextOptions textOptions = new RichTextOptions(adaptedFont)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            PointF startPoint = new PointF(WatermarkedImage.Width / 2, WatermarkedImage.Height / 2);
            PointF endPoint = new PointF(WatermarkedImage.Width, WatermarkedImage.Height);
            IPath diagonalPath = new SixLabors.ImageSharp.Drawing.Path(new LinearLineSegment(startPoint, endPoint));

            WatermarkedImage.Mutate(ctx => ctx.Paint(CanvasAction =>
            {
                CanvasAction.DrawText(textOptions, watermark, diagonalPath, Brushes.Solid(Color.White.WithAlpha(alpha)), pen: null);
            }));

            return WatermarkedImage;
        }

        public void Dispose()
        {
            OriginalImage?.Dispose();
            WatermarkedImage?.Dispose();
        }
    }
}
