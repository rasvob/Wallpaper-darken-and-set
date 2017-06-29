using System.IO;

namespace WallpaperDownloader
{
    public interface IWallpaperDownloader
    {
        string Url { get; set; }
        bool IsLinkValid();
        string DownloadWallpaper();
    }
}