using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace WallpaperDownloader
{
    public class CommonDownloader: AbstractDownloader
    {
        public CommonDownloader(string url) : base(url)
        {
        }

        public override bool IsLinkValid()
        {
            return true;
        }

        public override string GetImageUrl()
        {
            return Url;
        }

        public new string DownloadWallpaper()
        {
            return DownloadWallpaper(Url);
        }

        public string DownloadWallpaper(string url)
        {
            string fileName;
            using (WebClient wc = new WebClient())
            {
                byte[] data = wc.DownloadData(new Uri(url));
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
    }
}