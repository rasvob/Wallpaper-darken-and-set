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
    public class WallhavenDownloader: IWallpaperDownloader
    {
        public string Url { get; set; }

        public static readonly string RegExString = @"https:\/\/alpha\.wallhaven\.cc\/wallpaper\/\d+";

        public WallhavenDownloader(string url)
        {
            Url = url;
        }

        public bool IsLinkValid()
        {
            return new Regex(RegExString).IsMatch(Url);
        }

        public string DownloadWallpaper()
        {
            string url = GetImageUrl();
            IWallpaperDownloader downloader = new CommonDownloader(url);
            string wallpaper = downloader.DownloadWallpaper();
            return wallpaper;
        }

        private string GetImageUrl()
        {
            var web = new HtmlWeb();
            var doc = web.Load(Url);
            return "http:" + doc.DocumentNode.SelectSingleNode("//img[@id='wallpaper']").Attributes["src"].Value;
        }
    }
}