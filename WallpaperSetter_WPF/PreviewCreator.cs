using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WallpaperSetter_WPF
{
	class PreviewCreator
	{
		public string PathToImage { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		private MemoryStream prevStream;
		public MemoryStream OutputStream { get; set; }

		public PreviewCreator()
		{
			prevStream = new MemoryStream();
		}

		public PreviewCreator(string pathToImage, int width, int height)
		{
			this.prevStream = new MemoryStream();
			this.PathToImage = pathToImage;
			this.Width = width;
			this.Height = Height;
		}

		public BitmapImage CreatePreview()
		{
			byte[] imageBytes = File.ReadAllBytes(PathToImage);
			ISupportedImageFormat format = new PngFormat { Quality = 70 };
			using(MemoryStream inStream = new MemoryStream(imageBytes))
			{
				using(ImageFactory factory = new ImageFactory(preserveExifData: true))
				{
					ResizeLayer layer = new ResizeLayer(new Size(Width, Height));
					layer.ResizeMode = ResizeMode.Max;
					factory.Load(inStream).Resize(layer).Format(format).Save(prevStream);
				}
				return MemoryStreamToBitmapImage(prevStream);
			}
		}

		public void DarkenPreviewNoRes(int amount)
		{
			ISupportedImageFormat format = new PngFormat { Quality = 70 };
				using(ImageFactory factory = new ImageFactory(preserveExifData: true))
				{
					Bitmap cover = CreateBlackOverlay(Width, Height);
					ImageLayer imgLayer = new ImageLayer();
					imgLayer.Image = cover;
					imgLayer.Opacity = amount;
					imgLayer.Size = new Size(Width, Height);
					factory.Load(prevStream.GetBuffer()).Overlay(imgLayer).Save(OutputStream);
				}
		}

		public BitmapImage DarkenPreview(int amount)
		{
			ISupportedImageFormat format = new PngFormat { Quality = 70 };
			using(MemoryStream outStream = new MemoryStream())
			{
				using(ImageFactory factory = new ImageFactory(preserveExifData: true))
				{
					Bitmap cover = CreateBlackOverlay(Width, Height);
					ImageLayer imgLayer = new ImageLayer();
					imgLayer.Image = cover;
					imgLayer.Opacity = amount;
					imgLayer.Size = new Size(Width, Height);
					factory.Load(prevStream.GetBuffer()).Overlay(imgLayer).Save(outStream);
				}
				return MemoryStreamToBitmapImage(outStream);
			}
		}

		private void SaveFile(string path, MemoryStream outStream)
		{
			using(FileStream fileStream = File.Create(path))
			{
				outStream.Seek(0, SeekOrigin.Begin);
				outStream.CopyTo(fileStream);
			}
		}

		public BitmapImage MemoryStreamToBitmapImage(MemoryStream memory)
		{
			memory.Position = 0;

			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = memory;
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.EndInit();

			return bitmapImage;
		}

		private Bitmap CreateBlackOverlay(int width, int height)
		{
			Bitmap res = new Bitmap(width, height);
			for(int i = 0; i < res.Width; i++)
			{
				for(int j = 0; j < res.Height; j++)
				{
					res.SetPixel(i, j, Color.Black);
				}
			}
			return res;
		}

		public bool IsReady()
		{
			if(PathToImage == null || Width == 0 || Height == 0)
			{
				return false;
			}
			return true;
		}
	}
}
