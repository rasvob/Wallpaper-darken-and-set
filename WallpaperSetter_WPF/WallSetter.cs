using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Microsoft.Win32;

namespace WallpaperSetter_WPF
{
    class WallSetter
    {
		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

		public enum Style : int
		{
			Tiled,
			Centered,
			Stretched
		}

		public string PathToWallpaper { get; set; }
		public Style StyleOfWallpaper { get; set; }
		private const string PROCESSED_IMAGE_NAME = "output.png";

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		public WallSetter(string pathToWallpaper)
		{
            this.PathToWallpaper = pathToWallpaper;
        }

        public WallSetter()
		{
            this.PathToWallpaper = null;
        }

        private bool IsPathValid()
		{
            if(File.Exists(this.PathToWallpaper))
            {
                return true;
            }
            return false;
        }

	    public void SetOriginalWallpaper()
	    {
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
			if(StyleOfWallpaper == Style.Stretched)
			{
				key.SetValue(@"WallpaperStyle", 2.ToString());
				key.SetValue(@"TileWallpaper", 0.ToString());
			}

			if(StyleOfWallpaper == Style.Centered)
			{
				key.SetValue(@"WallpaperStyle", 1.ToString());
				key.SetValue(@"TileWallpaper", 0.ToString());
			}

			if(StyleOfWallpaper == Style.Tiled)
			{
				key.SetValue(@"WallpaperStyle", 1.ToString());
				key.SetValue(@"TileWallpaper", 1.ToString());
			}
			key.Close();
			FileInfo fi = new FileInfo(PathToWallpaper);
			Trace.WriteLine("Orig wall path " + fi.FullName);
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fi.FullName, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
		}
		
		private void SetDesktopWallpaperFromProcessed()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
			if(StyleOfWallpaper == Style.Stretched)
			{
				key.SetValue(@"WallpaperStyle", 2.ToString());
				key.SetValue(@"TileWallpaper", 0.ToString());
			}

			if(StyleOfWallpaper == Style.Centered)
			{
				key.SetValue(@"WallpaperStyle", 1.ToString());
				key.SetValue(@"TileWallpaper", 0.ToString());
			}

			if(StyleOfWallpaper == Style.Tiled)
			{
				key.SetValue(@"WallpaperStyle", 1.ToString());
				key.SetValue(@"TileWallpaper", 1.ToString());
			}
			key.Close();

			FileInfo fi = new FileInfo(PROCESSED_IMAGE_NAME);
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fi.FullName, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
		}

		public string RetrieveCurrentWallPath()
		{
			string path = null;
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
			path = rk.GetValue("WallPaper").ToString();
			return path;
		}

		public void SetDesktopWallpaper(int opacity)
		{
			if(PathToWallpaper == null)
			{
				Console.WriteLine("Null path");
				return;
			}

			int width, height;
			using(Image temp = Image.FromFile(PathToWallpaper))
			{
				width = temp.Width;
				height = temp.Height;
			}

			byte[] photoBytes = File.ReadAllBytes(PathToWallpaper);
			ISupportedImageFormat format = new PngFormat { Quality=100 };
			using(MemoryStream inStream = new MemoryStream(photoBytes))
			{
				using(MemoryStream outStream = new MemoryStream())
				{
					ProcessImage(inStream, format, width, height, opacity, outStream);
					SaveFile(PROCESSED_IMAGE_NAME, outStream);
				}
			}

			SetDesktopWallpaperFromProcessed();
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

		private void SaveFile(string path, MemoryStream outStream)
		{
			using(FileStream fileStream = File.Create(path))
			{
				outStream.Seek(0, SeekOrigin.Begin);
				outStream.CopyTo(fileStream);
			}
		}

		private void ProcessImage(MemoryStream inStream, ISupportedImageFormat format, int width, int height, int percent, MemoryStream outStream)
		{
			Bitmap cover = CreateBlackOverlay(width, height);
			ImageLayer imgLayer = new ImageLayer();
			imgLayer.Image = cover;
			imgLayer.Opacity = percent;
			imgLayer.Size = new Size(width, height);
			using(ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
			{
				imageFactory.Load(inStream)
							.Overlay(imgLayer)
							.Format(format)
							.Save(outStream);
			}
		}
    }
}
