using System.IO;
using System.Text.RegularExpressions;

namespace WallpaperDownloader
{
    public class UnsplashDownloader: AbstractDownloader
    {
        public UnsplashDownloader(string url) : base(url) {}

        public override bool IsLinkValid()
        {
            var rx = new Regex(@"https:\/\/unsplash\.com\/\?photo=\w+");
            return rx.IsMatch(Url);
        }

        public override string GetImageUrl()
        {
            var rgx = new Regex(@"https:\/\/unsplash\.com\/\?photo=(<id>)");
            rgx.Matches(Url);
            return null;
        }
    }
}