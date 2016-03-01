using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Windows;

namespace WallpaperSetter_WPF
{
	public class OriginalWallpaperGetter
	{
		private readonly string _postfix = @"Microsoft\Windows\Themes\CachedFiles";
		private readonly string _pathToDirectory;
		private string _fileName;

		public OriginalWallpaperGetter()
		{
			_pathToDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _postfix);
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
			var file = Directory.GetFiles(_pathToDirectory).FirstOrDefault();
			_fileName = new FileInfo(file).Name;
			File.Copy(file, _fileName);
			Trace.WriteLine(_fileName);
			return _fileName;
		}
	}
}