using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace WallpaperDownloader
{
    public class WallhavenDownloader: AbstractDownloader
    {
        public WallhavenDownloader(string url) : base(url) {}

        public override bool IsLinkValid()
        {
            return new Regex(@"^https:\/\/alpha\.wallhaven\.cc\/wallpaper\/\d+$").IsMatch(Url);
        }

        public override string GetImageUrl()
        {
            var web = new HtmlWeb();
            var doc = web.Load(Url);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//img[@id='wallpaper']");
            HtmlAttribute attribute = node != null ? node.Attributes["src"] : throw new ApplicationException(UnableToObtainLinkError);
            string imageUrl = "http:" + attribute.Value;
            return imageUrl;
        }
    }
}