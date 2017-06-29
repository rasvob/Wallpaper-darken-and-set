using System;

namespace WallpaperDownloader
{
    public static class WallpaperDownloaderFactory
    {
        public static IWallpaperDownloader CreateDownloaderInstance(DownloaderType type, string url)
        {
            switch (type)
            {
                case DownloaderType.Wallhaven:
                    return new WallhavenDownloader(url);
                case DownloaderType.Unsplash:
                    return new UnsplashDownloader(url);
                case DownloaderType.Link:
                    return new CommonDownloader(url);
            }
            return new CommonDownloader(url);
        }
    }
}