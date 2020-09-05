using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Models
{
    public class SubtitleFinderModel
    {
        public string DownloadsCount { get; set; }
        public string DownloadLink { get; set; }
        public string SubtitleLanguage { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string RatingValue { get; set; }
        public string RatingCount { get; set; }
        public int RelevanceScore { get; set; }
        public string Poster { get; set; }
    }
}
