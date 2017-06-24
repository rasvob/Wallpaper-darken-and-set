using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Microsoft.Win32;

namespace WallpaperManipulator
{
    class WallSetter
    {
		const int SpiSetdeskwallpaper = 20;
		const int SpifUpdateinifile = 0x01;
		const int SpifSendwininichange = 0x02;

		public string PathToWallpaper { get; set; }
		public Style StyleOfWallpaper { get; set; }
		private const string ProcessedImageName = "output.png";

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		public WallSetter(string pathToWallpaper)
		{
            PathToWallpaper = pathToWallpaper;
        }

	    public void SetOriginalWallpaper()
	    {
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
	        SetRegistryStyleByStyle(key, StyleOfWallpaper);
            key.Close();
			FileInfo fi = new FileInfo(PathToWallpaper);
			SystemParametersInfo(SpiSetdeskwallpaper, 0, fi.FullName, SpifUpdateinifile | SpifSendwininichange);
		}
		
		private void SetDesktopWallpaperFromProcessed()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
		    SetRegistryStyleByStyle(key, StyleOfWallpaper);
			key.Close();

			FileInfo fi = new FileInfo(ProcessedImageName);
			SystemParametersInfo(SpiSetdeskwallpaper, 0, fi.FullName, SpifUpdateinifile | SpifSendwininichange);
		}

        private void SetRegistryStyleByStyle(RegistryKey key, Style style)
        {
            if (StyleOfWallpaper == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (StyleOfWallpaper == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (StyleOfWallpaper == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }
        }

		public string RetrieveCurrentWallPath()
		{
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
			return rk.GetValue("WallPaper").ToString();
		}

        public void SetDesktopWallpaper(int opacity)
        {
            if (PathToWallpaper == null)
            {
                Debug.WriteLine("Null path");
                return;
            }

            int width, height;
            using (Image temp = Image.FromFile(PathToWallpaper))
            {
                width = temp.Width;
                height = temp.Height;
            }

            byte[] photoBytes = File.ReadAllBytes(PathToWallpaper);
            ISupportedImageFormat format = new PngFormat {Quality = 100};
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (WallpaperImageProcessor processor = new WallpaperImageProcessor())
                    {
                        processor.ProcessImage(inStream, format, width, height, opacity, outStream);
                        processor.SaveFile(ProcessedImageName, outStream);
                        SetDesktopWallpaperFromProcessed();
                    }
                }
            }
        }
    }
}
