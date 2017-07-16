using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace WallpaperManipulator
{
	public class OriginalWallpaperGetter
	{
		private readonly string _postfix = @"Microsoft\Windows\Themes\CachedFiles";
		private readonly string _postfixTranscoded = @"Microsoft\Windows\Themes\";
		private readonly string _pathToDirectory;
		private readonly string _pathToDirectoryTranscoded;

	    public OriginalWallpaperGetter()
		{
			_pathToDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _postfix);
			_pathToDirectoryTranscoded = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _postfixTranscoded);
		}

	    public string GetOriginalWallpaper()
	    {
	        string file;
            try
		    {
		        file = Directory.GetFiles(_pathToDirectory).FirstOrDefault();
            }
            catch (DirectoryNotFoundException)
		    {
                file = Directory.GetFiles(_pathToDirectoryTranscoded).FirstOrDefault(t => t.Contains("TranscodedWallpaper"));
            }

	        using (FileStream stream = File.OpenRead(file))
	        {
	            using (var img = Image.FromStream(stream))
	            {
	                string fileName = Path.GetTempFileName() + ".png";
	                img.Save(fileName, ImageFormat.Png);
	                return fileName;
                }
            }
		}
	}
}