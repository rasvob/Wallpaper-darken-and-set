using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using AngleSharp;
using AngleSharp.Parser.Html;
using System.IO;

namespace WallpaperSetter_WPF
{

	public delegate void DownloadCompleteCallback(string filename);

	class WallhavenDownloader
	{
		public string WallhavenLink { get; set; }
		public DownloadCompleteCallback CallbackDownloadComplete { get; set; }
		private string _filename;

		public WallhavenDownloader()
		{

		}

		~WallhavenDownloader()
		{
			if(File.Exists(_filename))
			{
				File.Delete(_filename);
			}
		}

		public WallhavenDownloader(string WallhavenLink)
		{
			this.WallhavenLink = WallhavenLink;
		}

		public void DownloadImageFromWallhaven()
		{
			using(WebClient wc = new WebClient())
			{
				wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ParseResultHTML);
				wc.DownloadStringAsync(new Uri(WallhavenLink));
			}
		}

		private void ParseResultHTML(object sender, DownloadStringCompletedEventArgs e)
		{
			string source = e.Result;
			var parser = new HtmlParser();
			var document = parser.Parse(source);
			var img = document.QuerySelector("img#wallpaper");
			var link = (from attr in img.Attributes where attr.Name == "src" select attr.Value).FirstOrDefault();
			Trace.WriteLine(link);
			Trace.WriteLine(ParseFilename(link));
			_filename = ParseFilename(link);
			DownloadImage(link);
		}

		private void DownloadImage(string link)
		{
			using(WebClient client = new WebClient())
			{
				client.DownloadFileCompleted += (o,e) => {
					CallbackDownloadComplete(_filename);
				};
				client.DownloadFileAsync(new Uri("http:" + link), _filename);
			}
		}

		private string ParseFilename(string link)
		{
			return link.Substring(link.LastIndexOf('/')+1);
		}
	}
}
