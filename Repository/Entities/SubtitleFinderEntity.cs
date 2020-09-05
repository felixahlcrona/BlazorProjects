using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Entities
{
    public class SubtitleFinderEntity
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
