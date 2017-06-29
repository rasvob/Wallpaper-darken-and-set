using System;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using System.Drawing;
using System.Drawing.Imaging;

namespace WallpaperDownloader
{
    public class WallhavenDownloader: IWallpaperDownloader
    {
        public string Url { get; set; }

        public WallhavenDownloader(string url)
        {
            Url = url;
        }

        public bool IsLinkValid()
        {
            return true;
        }

        public (string path, MemoryStream stream) DownloadWallpaper()
        {
            var url = GetImageUrl();
            string fileName;
            MemoryStream memoryStream;
            using (WebClient wc = new WebClient())
            {
                byte[] data = wc.DownloadData(url);
                memoryStream = new MemoryStream(data);

                using (var img = Image.FromStream(memoryStream))
                {
                    fileName = Path.GetTempFileName();
                    fileName = fileName.Substring(0, fileName.IndexOf('.')) + ".png";
                    img.Save(fileName, ImageFormat.Png);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            return (fileName, memoryStream);
        }

        private string GetImageUrl()
        {
            var web = new HtmlWeb();
            var doc = web.Load(Url);
            return "http:" + doc.DocumentNode.SelectSingleNode("//img[@id='wallpaper']").Attributes["src"].Value;
        }
    }
}