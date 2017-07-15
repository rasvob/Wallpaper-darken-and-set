using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace WallpaperDownloader
{
    public class CommonDownloader: IWallpaperDownloader
    {
        public string Url { get; set; }

        public CommonDownloader(string url)
        {
            Url = url;
        }

        public bool IsLinkValid()
        {
            return true;
        }

        public string DownloadWallpaper()
        {
            return DownloadWallpaper(Url);
        }

        public string DownloadWallpaper(string url)
        {
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
    }
}