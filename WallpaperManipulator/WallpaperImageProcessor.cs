using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace WallpaperManipulator
{
    public class WallpaperImageProcessor: IDisposable
    {
        public MemoryStream Stream { get; set; } = new MemoryStream();
        public void ProcessImage(MemoryStream inputImage, Size newSize, Rectangle cropArea, int coverOpacity = 0)
        {
            Bitmap overlay = CreateBlackOverlay(cropArea.Width, cropArea.Height);
            ImageLayer imageLayer = new ImageLayer
            {
                Image = overlay,
                Opacity = coverOpacity,
                Size = newSize
            };

            using (ImageFactory imageFactory = new ImageFactory())
            {
                imageFactory.Load(inputImage)
                    .Resize(newSize)
                    .Crop(cropArea)
                    .Overlay(imageLayer)
                    .Format(new PngFormat {Quality = 100})
                    .Quality(100)
                    .Save(Stream);
            }
        }

        public void SaveFile(string fileName, ImageFormat format)
        {
            Stream.Seek(0, SeekOrigin.Begin);
            using (var img = Image.FromStream(Stream))
            {
                img.Save(fileName, format);
            }
        }

        public string SaveFileToTemp()
        {
            string fileName = Path.GetTempFileName() + ".png";
            SaveFile(fileName, ImageFormat.Png);
            return fileName;
        }

        private Bitmap CreateBlackOverlay(int width, int height)
        {
            Bitmap img = new Bitmap(width, height);
            using (Graphics graph = Graphics.FromImage(img))
            {
                Rectangle imageSize = new Rectangle(0, 0, width, height);
                graph.FillRectangle(Brushes.Black, imageSize);
            }
            return img;
        }

        public void Dispose()
        {
            Stream.Close();
        }
    }
}