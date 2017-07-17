using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WallpaperManipulator
{
    public class WallSetter
    {
		const int SpiSetdeskwallpaper = 20;
		const int SpifUpdateinifile = 0x01;
		const int SpifSendwininichange = 0x02;

		public string PathToWallpaper { get; set; }
		public Style StyleOfWallpaper { get; set; }

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		public WallSetter(string pathToWallpaper, Style style)
		{
            PathToWallpaper = pathToWallpaper;
		    StyleOfWallpaper = style;
		}
		
		public void SetImageAsWallpaper()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
		    SetRegistryStyleByStyle(key);
			key?.Close();
			FileInfo fi = new FileInfo(PathToWallpaper);
			SystemParametersInfo(SpiSetdeskwallpaper, 0, fi.FullName, SpifUpdateinifile | SpifSendwininichange);
		}

        private void SetRegistryStyleByStyle(RegistryKey key)
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
    }
}
