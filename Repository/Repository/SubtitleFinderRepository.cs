using HtmlAgilityPack;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{


    public class SubtitleFinderRepository : ISubtitleFinderRepository
    {

        private readonly HttpClient _clientFactory;

        public SubtitleFinderRepository(HttpClient clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<HtmlDocument> GetMovieDetails(SubtitleFinderEntity e)
        {
            HtmlDocument _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml($"https://subscene.com/{e.Url}");
            var tes = $"https://subscene.com/{e.Url}";
            var responseSubtitles = await _clientFactory.GetAsync(tes);
            var responseStringSubtiels = await responseSubtitles.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(responseStringSubtiels);

            return _htmlDoc;

        }


        public async Task<HtmlDocument> GetMoviePoster(string moviePage)
        {

            HtmlDocument _htmlDoc = new HtmlDocument();
            var response = await _clientFactory.GetAsync($"https://subscene.com/{moviePage}");
            var responseString = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(responseString);

            return _htmlDoc;

        }


        public async Task<HtmlDocument> MostPlauisableMovie(string searchQuery)
        {
            HtmlDocument _htmlDoc = new HtmlDocument();
            var response = await _clientFactory.PostAsync($"https://subscene.com/subtitles/searchbytitle?query={searchQuery}", null);
            var responseString = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(responseString);

            return _htmlDoc;
        }


        public async Task<HtmlDocument> GetAllSubtitlesForMovie(string movieUrl)
        {
            HtmlDocument _htmlDoc = new HtmlDocument();
            var request = await _clientFactory.GetAsync($"https://subscene.com/{movieUrl}");
            var response = await request.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(response);

            return _htmlDoc;
        }
    }
}



