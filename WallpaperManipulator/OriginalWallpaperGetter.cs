using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WallpaperManipulator
{
	public class OriginalWallpaperGetter: IDisposable
	{
		private readonly string _postfix = @"Microsoft\Windows\Themes\CachedFiles";
		private readonly string _postfixTranscoded = @"Microsoft\Windows\Themes\";
		private readonly string _pathToDirectory;
		private readonly string _pathToDirectoryTranscoded;
		private string _fileName;

		public OriginalWallpaperGetter()
		{
			_pathToDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _postfix);
			_pathToDirectoryTranscoded = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _postfixTranscoded);
		}

	    public void Dispose()
	    {
	        if (File.Exists(_fileName))
	        {
	            File.Delete(_fileName);
	        }
        }

	    public string GetOriginalWallpaper()
		{
			try
			{
				var file = Directory.GetFiles(_pathToDirectory).FirstOrDefault();
			    string fileName = string.Empty;
			    fileName = new FileInfo(file).Name;
			    File.Copy(file, fileName);
			    _fileName = fileName;
			    return fileName;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				var file = Directory.GetFiles(_pathToDirectoryTranscoded).FirstOrDefault(t => t.Contains("TranscodedWallpaper"));
			    string fileName = string.Empty;
                if (file != null)
				{
				    fileName = new FileInfo(file).Name + ".jpg";
				    File.Copy(file, fileName);
				    _fileName = fileName;
                }
			    return fileName;
			}
		}
	}
}