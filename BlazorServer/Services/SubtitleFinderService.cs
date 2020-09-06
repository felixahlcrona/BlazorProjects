using System;
using System.Collections.Generic;
using System.Linq;
using AgileObjects.AgileMapper;
using System.Threading.Tasks;
using Repository.Repository;
using BlazorServer.Models;
using System.Text.RegularExpressions;
using MoreLinq;
using HtmlAgilityPack;
using Repository.Entities;

namespace BlazorServer.Services
{
    public class SubtitleFinderService : ISubtitleFinderService
    {
        protected ISubtitleFinderRepository _subtitleFinderRepo { get; set; }

        public SubtitleFinderService(ISubtitleFinderRepository subtitleFinderRepo)
        {
            _subtitleFinderRepo = subtitleFinderRepo;
        }


        public async Task<IEnumerable<SubtitleFinderModel>> SubtitleSearch(string searchedMovieTitle)
        {
            var plausibleMovieUrl = await MostPlauisableMovie(CleanUpMovieSearch(searchedMovieTitle));
            var listOfSubtitles = await GetAllSubtitlesForMovie(plausibleMovieUrl);
            var moviePoster = await GetMoviePoster(plausibleMovieUrl);

            var cleanedSubtitleList = listOfSubtitles.Select(movie => new SubtitleFinderModel
            {
                Title = ClearEscapeSequences(movie.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).InnerText),
                Url = movie.ChildNodes.ElementAt(1).Attributes.FirstOrDefault().Value,
                RelevanceScore = CalculateMostPlausibleMovieTitle(searchedMovieTitle, ClearEscapeSequences(movie.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).InnerText)).Item1,
                Poster = moviePoster,
                SubtitleLanguage = ClearEscapeSequences(movie.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).InnerText)

            }).ToList();

            // Sorterar ut dubletter och url med 1080p och 720p variatnter.
            var topSubTitles = cleanedSubtitleList.OrderByDescending(e => e.RelevanceScore).Take(50);
            topSubTitles = topSubTitles.DistinctBy(e => e.Title).DistinctBy(f => f.Url);
            return topSubTitles;
        }

        // kolla över distinkta subtitles, finns flera på subscene med samma namn?
        private async Task<List<HtmlNode>> GetAllSubtitlesForMovie(string movieUrl)
        {
            var htmlDoc = await _subtitleFinderRepo.ExecuteGetRequest($"https://subscene.com/{movieUrl}");

            var subtitleUrls = htmlDoc.DocumentNode.SelectNodes("//*[@class='a1']");

            var englishSubtitles = subtitleUrls.Where(e => e.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).InnerText == "\r\n\t\t\t\t\t\tEnglish\r\n\t\t\t\t\t").ToList();
            var swedishSubtitles = subtitleUrls.Where(e => e.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).InnerText == "\r\n\t\t\t\t\t\tSwedish\r\n\t\t\t\t\t").ToList();

            var selectedSubtitles = englishSubtitles.Concat(swedishSubtitles).ToList();

            return selectedSubtitles;
        }

        private async Task<string> MostPlauisableMovie(string searchQuery)
        {
            var htmlDoc = await _subtitleFinderRepo.ExecutePostRequest($"https://subscene.com/subtitles/searchbytitle?query={searchQuery}");

            var elementsOfMovies = htmlDoc.DocumentNode.SelectNodes("//*[@id='left']/div/div/ul[1]/li/div[1]/a");
            var plausibleMovieTitles = elementsOfMovies.Select(e => CalculateMostPlausibleMovieTitle(searchQuery, e.InnerText)).OrderByDescending(e => e.Item1);
            var mostLikelyMovie = plausibleMovieTitles.FirstOrDefault().Item2;
            var plausibleMovieUrl = elementsOfMovies.Where(e => e.InnerText == mostLikelyMovie).Select(e => e.Attributes.FirstOrDefault().Value).FirstOrDefault();
            return plausibleMovieUrl;
        }


        public async Task<string> GetMoviePoster(string moviePage)
        {
            try
            {
                var htmlDoc = await _subtitleFinderRepo.ExecuteGetRequest($"https://subscene.com/{moviePage}");

                var posterImage = htmlDoc.DocumentNode.SelectNodes("//*[@class='poster']").FirstOrDefault().ChildNodes.ElementAt(1).Attributes.FirstOrDefault().Value;
                // higher quality images.
                posterImage = posterImage.Replace(".154", string.Empty);
                return posterImage;
            }

            catch (Exception e)
            {
                return e.Message;
            }

        }
        public async Task<SubtitleFinderModel> GetMovieDetails(SubtitleFinderModel selectedMovie)
        {
            string ratingValue;
            string ratingCount;
            string downloadsCount;
            string downloadLink;

            var htmlDoc = await _subtitleFinderRepo.ExecuteGetRequest($"https://subscene.com/{selectedMovie.Url}");

            try
            {
                ratingValue = htmlDoc.DocumentNode.SelectNodes("//*[@itemprop='ratingValue']").FirstOrDefault().ChildNodes.FirstOrDefault().InnerText;
                ratingCount = htmlDoc.DocumentNode.SelectNodes("//*[@itemprop='ratingCount']").FirstOrDefault().ChildNodes.FirstOrDefault().InnerText;
                downloadsCount = htmlDoc.DocumentNode.SelectNodes("//*[@id='details']/ul/li[11]").FirstOrDefault().ChildNodes.ElementAt(2).InnerText;
                downloadLink = htmlDoc.DocumentNode.SelectNodes("//*[@class='download']").FirstOrDefault().ChildNodes.ElementAt(1).Attributes.FirstOrDefault().Value;
                downloadLink = $"https://subscene.com/{downloadLink}";
            }
            catch
            {
                ratingValue = string.Empty;
                ratingCount = string.Empty;
                downloadsCount = string.Empty;
                downloadLink = string.Empty;
            }


            downloadsCount = ClearEscapeSequences(downloadsCount);

            SubtitleFinderModel movie = new SubtitleFinderModel()
            {
                DownloadsCount = downloadsCount,
                DownloadLink = downloadLink,
                Title = selectedMovie.Title,
                RatingValue = ratingValue,
                RatingCount = ratingCount,
                RelevanceScore = selectedMovie.RelevanceScore,
                Url = selectedMovie.Url

            };

            return movie;
        }



        private string ClearEscapeSequences(string query)
        {
            query = Regex.Replace(query, @"\t|\n|\r", "");
            return query;
        }

        private string CleanUpMovieSearch(string query)
        {
            query = Regex.Match(query, @"^.*?([\d]+)").Value;
            return query;
        }

        public Tuple<int, string> CalculateMostPlausibleMovieTitle(string s, string t)
        {

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0)
            {
                return new Tuple<int, string>(m, t);
            }
            if (m == 0)
            {
                return new Tuple<int, string>(m, t);
            }
            for (int i = 0; i <= n; d[i, 0] = i++)
                ;
            for (int j = 0; j <= m; d[0, j] = j++)
                ;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            var relevanceScore = d[n, m];
            var title = t;

            var relevanceScorePercentage = 100 - relevanceScore;
            return new Tuple<int, string>(relevanceScorePercentage, title);
        }

    }

}
