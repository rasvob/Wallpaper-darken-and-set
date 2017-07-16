namespace WallpaperDownloader
{
    public abstract class AbstractDownloader: IWallpaperDownloader
    {
        public string Url { get; set; }
        public abstract bool IsLinkValid();
        public abstract string GetImageUrl();

        protected AbstractDownloader(string url)
        {
            Url = url;
        }

        public string DownloadWallpaper()
        {
            string url = GetImageUrl();
            CommonDownloader downloader = new CommonDownloader(url);
            string wallpaper = downloader.DownloadWallpaper();
            return wallpaper;
        }
    }
}