using System.IO;

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
            throw new System.NotImplementedException();
        }
    }
}