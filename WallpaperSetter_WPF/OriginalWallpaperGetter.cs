using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WallpaperSetter_WPF
{
	public class OriginalWallpaperGetter
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

		~OriginalWallpaperGetter()
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
				_fileName = new FileInfo(file).Name;
				File.Copy(file, _fileName);
				Trace.WriteLine(_fileName);
				return _fileName;
			}
			catch (Exception e)
			{
				Trace.WriteLine(e.Message);
				try
				{
					var file = Directory.GetFiles(_pathToDirectoryTranscoded).FirstOrDefault(t => t.Contains("TranscodedWallpaper"));
					_fileName = new FileInfo(file).Name + ".jpg";
					File.Copy(file, _fileName);
					return _fileName;
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.Message);
					return string.Empty;
				}
			}
		}
	}
}