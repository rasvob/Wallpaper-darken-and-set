using System.IO;

namespace WallpaperDownloader
{
    public class UnsplashDownloader: IWallpaperDownloader
    {
        public string Url { get; set; }

        public UnsplashDownloader(string url)
        {
            Url = url;
        }

        public bool IsLinkValid()
        {
            throw new System.NotImplementedException();
        }

        public string DownloadWallpaper()
        {
            throw new System.NotImplementedException();
        }
    }
}