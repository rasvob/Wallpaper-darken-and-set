using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperSetter_WPF
{
	class WallpaperDownloader
	{
		public string Link { get; set; }
		private string _fileName;
		public event EventHandler<string> ImageDownloaded;

		public WallpaperDownloader()
		{

		}

		~WallpaperDownloader()
		{
			if(File.Exists(_fileName))
			{
				File.Delete(_fileName);
			}
		}

		public WallpaperDownloader(string link)
		{
			this.Link = link;
		}

		public void DownloadImage()
		{
			ParseFileName();

			using(WebClient client = new WebClient())
			{
				client.DownloadFileCompleted += DownloadCompleted;
				client.DownloadFileAsync(new Uri(Link), _fileName);
			}
		}

		private void DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			OnImageDownloaded();
		}

		private void ParseFileName()
		{
			_fileName = Link.Substring(Link.LastIndexOf('/')+1);
		}

		protected virtual void OnImageDownloaded()
		{
			if(ImageDownloaded != null)
			{
				ImageDownloaded(this,_fileName);
			}
		}
	}
}
