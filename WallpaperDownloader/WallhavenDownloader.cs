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

        public string DownloadWallpaper()
        {
            var url = GetImageUrl();
            string fileName;
            using (WebClient wc = new WebClient())
            {
                byte[] data = wc.DownloadData(url);

                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (var img = Image.FromStream(memoryStream))
                    {
                        fileName = Path.GetTempFileName();
                        fileName = fileName + ".png";
                        img.Save(fileName, ImageFormat.Png);
                    }
                }
            }
            return fileName;
        }

        private string GetImageUrl()
        {
            var web = new HtmlWeb();
            var doc = web.Load(Url);
            return "http:" + doc.DocumentNode.SelectSingleNode("//img[@id='wallpaper']").Attributes["src"].Value;
        }
    }
}