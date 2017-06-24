using System;
using System.Drawing;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace WallpaperManipulator
{
    public class WallpaperImageProcessor: IDisposable
    {
        private string _file;
        public void ProcessImage(MemoryStream inStream, ISupportedImageFormat format, int width, int height, int percent, MemoryStream outStream)
        {
            Bitmap cover = CreateBlackOverlay(width, height);
            ImageLayer imgLayer = new ImageLayer
            {
                Image = cover,
                Opacity = percent,
                Size = new Size(width, height)
            };
            using (ImageFactory imageFactory = new ImageFactory(true))
            {
                imageFactory.Load(inStream)
                    .Overlay(imgLayer)
                    .Format(format)
                    .Save(outStream);
            }
        }

        public void SaveFile(string path, MemoryStream outStream)
        {
            _file = path;
            using (FileStream fileStream = File.Create(path))
            {
                outStream.Seek(0, SeekOrigin.Begin);
                outStream.CopyTo(fileStream);
            }
        }

        private Bitmap CreateBlackOverlay(int width, int height)
        {
            Bitmap res = new Bitmap(width, height);
            for (int i = 0; i < res.Width; i++)
            {
                for (int j = 0; j < res.Height; j++)
                {
                    res.SetPixel(i, j, Color.Black);
                }
            }
            return res;
        }

        public void Dispose()
        {
            if (File.Exists(_file))
            {
                File.Delete(_file);
            }
        }
    }
}