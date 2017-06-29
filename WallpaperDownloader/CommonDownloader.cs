using System.IO;

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
            throw new System.NotImplementedException();
        }

        public (string path, MemoryStream stream) DownloadWallpaper()
        {
            throw new System.NotImplementedException();
        }
    }
}