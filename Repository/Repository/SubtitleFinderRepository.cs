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


        public async Task<HtmlDocument> ExecuteGetRequest(string url)
        {
            HtmlDocument _htmlDoc = new HtmlDocument();
            var response = await _clientFactory.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(content);

            return _htmlDoc;
        }

        public async Task<HtmlDocument> ExecutePostRequest(string url)
        {
            HtmlDocument _htmlDoc = new HtmlDocument();
            var response = await _clientFactory.PostAsync(url,null);
            var content = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(content);

            return _htmlDoc;
        }

    }
}



