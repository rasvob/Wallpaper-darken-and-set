using System;
using System.IO;
using System.Text.RegularExpressions;

namespace WallpaperDownloader
{
    public class UnsplashDownloader: AbstractDownloader
    {
        public UnsplashDownloader(string url) : base(url) {}

        public override bool IsLinkValid()
        {
            var rx = new Regex(@"^https:\/\/unsplash\.com\/\?photo=(\w+)$");
            var rxAlt = new Regex(@"^https:\/\/unsplash\.com\/photos\/(\w+)$");
            return rx.IsMatch(Url) || rxAlt.IsMatch(Url);
        }

        public override string GetImageUrl()
        {
            var rgx = new Regex(@"(^https:\/\/unsplash\.com\/\?photo=)(?<id>\w+)$");
            var rxAlt = new Regex(@"^https:\/\/unsplash\.com\/photos\/(?<id>\w+)$");
            Match match = rgx.Match(Url);

            if (!match.Success)
            {
                match = rxAlt.Match(Url);
            }

            string value = match.Groups["id"]?.Value ?? throw new ApplicationException(UnableToObtainLinkError);

            return $"http://unsplash.com/photos/{value}/download?force=true";
        }
    }
}