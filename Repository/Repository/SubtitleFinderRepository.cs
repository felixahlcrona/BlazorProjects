using CloudflareSolverRe;
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

        private static HttpClient getCustomHttpClient()
        {
            // Cloudflare Bypass DDOS captcha
            var handler = new ClearanceHandler
            {
                MaxTries = 3,
                ClearanceDelay = 3000
            };

            var client = new HttpClient(handler);
            return client;
        }


        public async Task<HtmlDocument> ExecuteGetRequest(string url)
        {
            HttpClient client = getCustomHttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            HtmlDocument _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml(content);

            return _htmlDoc;
        }

   
        public async Task<HtmlDocument> ExecutePostRequest(string url)
        {
            HttpClient client = getCustomHttpClient();
            var response = await client.PostAsync(url,null);
            var content = await response.Content.ReadAsStringAsync();
            HtmlDocument _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml(content);

            return _htmlDoc;
        }

    }
}



