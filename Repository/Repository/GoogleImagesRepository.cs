using HtmlAgilityPack;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Repository
{


    public class GoogleImagesRepository : IGoogleImagesRepository
    {
        private readonly HttpClient _clientFactory;
        private string errorResponse;

        public GoogleImagesRepository(HttpClient clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<string>> GetGoogleImages(string query)
        {
            var _clientFactory = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.google.com/search?q={query}&tbm=isch&ved=2ahUKEwiKjuKXxuXqAhWVsSoKHQ9_DscQ2-cCegQIABAA&oq=kock&gs_lcp=CgNpbWcQAzICCAAyAggAMgIIADICCAAyAggAMgIIADICCAAyAggAMgIIADICCAA6BQgAELEDUNArWN0uYLwyaABwAHgAgAF8iAHKA5IBAzAuNJgBAKABAaoBC2d3cy13aXotaW1nwAEB&sclient=img&ei=OqYaX4qJOJXjqgGP_rm4DA&safe=images&tbs=itp%3Aface&hl=en-US");
            _clientFactory.DefaultRequestHeaders.Clear();
            _clientFactory.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _clientFactory.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            _clientFactory.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            _clientFactory.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
            _clientFactory.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            _clientFactory.DefaultRequestHeaders.Add("X-Client-Data", "CI+2yQEIorbJAQjBtskBCKmdygEIiqzKAQjmxsoBCOfIygEIssnKAQi0y8oB");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            _clientFactory.DefaultRequestHeaders.Add("Referer", "https://www.google.com/");
            _clientFactory.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,sv;q=0.8,da;q=0.7");
            _clientFactory.DefaultRequestHeaders.Add("Cookie", "CGIC=EhQxQzFHQ0VBX2VuU0U4MTJTRTgxMiJ8dGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2Uvd2VicCxpbWFnZS9hcG5nLCovKjtxPTAuOCxhcHBsaWNhdGlvbi9zaWduZWQtZXhjaGFuZ2U7dj1iMztxPTAuOQ; CONSENT=WP.289366; NID=204=GWPD4hm9DhuAn70PwdyompsmTOBLSgxfdU0z6KI-X8cAodwJtflRDsBKSi8_KKk_7nLumJmpWcLSzo5n2QYmAHQdYjK59uwaS5FPbJZjCDElGBDQSRHDdW-sf8HeRuXGma4C0AaW_tWKEbQ7U7GXGR5uJ8DkZEGbVd2d7YqUfVM; 1P_JAR=2020-7-23-12; DV=Y6HYpkJTBJMoQIaWaSHSPtADEka6N5fug7JWUYEbdwEAAAA; UULE=a+cm9sZToxIHByb2R1Y2VyOjEyIHByb3ZlbmFuY2U6NiB0aW1lc3RhbXA6MTU5NTUwNzIzNzQ3MzAwMCBsYXRsbmd7bGF0aXR1ZGVfZTc6NTYwNzI2MDE2IGxvbmdpdHVkZV9lNzoxMjcyNzA5MTJ9IHJhZGl1czo0ODEwMjU3NjA=; OTZ=5553387_48_52_123900_48_436380");
            HttpResponseMessage response = await _clientFactory.SendAsync(request);
            await GetAvanzaData();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var collection = Regex.Matches(responseContent, @"(https?:\/\/.*\.(?:png|jpg))");
                var List = collection.Cast<Match>()
                .Select(m => m.Value)
                .ToList();

                return List;
            }
            return null;
        }

        public async Task<List<string>> GetAvanzaData()
        {
            var _clientFactory = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.avanza.se/frontend/template.html/marketing/advanced-filter/advanced-filter-template?1619720498475&widgets.marketCapitalInSek.filter.lower=2000&widgets.marketCapitalInSek.filter.upper=33000&widgets.marketCapitalInSek.active=true&widgets.stockLists.filter.list%5B0%5D=SE.Inofficiella&widgets.stockLists.filter.list%5B1%5D=SE.LargeCap.SE&widgets.stockLists.filter.list%5B2%5D=SE.MidCap.SE&widgets.stockLists.filter.list%5B3%5D=SE.SPAC.SE&widgets.stockLists.filter.list%5B4%5D=SE.SmallCap.SE&widgets.stockLists.filter.list%5B5%5D=SE.Xterna+listan&widgets.stockLists.filter.list%5B6%5D=SE.FNSE&widgets.stockLists.filter.list%5B7%5D=SE.XNGM&widgets.stockLists.filter.list%5B8%5D=SE.NSME&widgets.stockLists.filter.list%5B9%5D=SE.XSAT&widgets.stockLists.filter.list%5B10%5D=US.XNAS&widgets.stockLists.filter.list%5B11%5D=US.XNYS&widgets.stockLists.active=true&widgets.numberOfOwners.filter.lower=75&widgets.numberOfOwners.filter.upper=&widgets.numberOfOwners.active=false&parameters.startIndex=0&parameters.maxResults=100&parameters.selectedFields%5B0%5D=NBR_OF_OWNERS_CHANGE_DAY&parameters.selectedFields%5B1%5D=TICKER_SYMBOL&parameters.sortField=NBR_OF_OWNERS_CHANGE_DAY&parameters.sortOrder=DESCENDING");
            _clientFactory.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _clientFactory.DefaultRequestHeaders.Add("Pragma", "no-cache");
            _clientFactory.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            _clientFactory.DefaultRequestHeaders.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            _clientFactory.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
            _clientFactory.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?1");
            _clientFactory.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Mobile Safari/537.36");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            _clientFactory.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            _clientFactory.DefaultRequestHeaders.Add("Referer", "https://www.avanza.se/aktier/lista.html");
            _clientFactory.DefaultRequestHeaders.Add("Accept-Language", "sv-SE,sv;q=0.9,en-US;q=0.8,en;q=0.7");
            _clientFactory.DefaultRequestHeaders.Add("Cookie", "AZAHLI=bankId; forum=\"{\"postsPerPage\":100,\"showOverview\":true}\"; AZACOOKIECONSENT_ANALYSIS=YES; AZACOOKIECONSENT_UX=YES; AZACOOKIECONSENT_MARKETING=YES; AZAMENUTAB=/min-ekonomi/oversikt.html||; Humany__clientId=1ddc91fd-e03e-0a4d-b66c-775821f683f7; Humany__parameters={\"isLoggedIn\":[\"Nej\"]}; AZAPERSISTANCE=0253c8bd2e-1942-40LYQwl5wCml5B8NoBIEfyFXAWVYBuixFmrkQmcA-qKZl15N5aXcXpZDh60FhmA-dTFYo; JSESSIONID=1gz58ym6xnx4916fvh0xymy5gx; AZAABSESSION=b84z0fd7zmub129mwp19l2uj2; FSESSIONID=14pe6hmqlk4tkref36ynaz5zs");
            HttpResponseMessage response = await _clientFactory.SendAsync(request);
            Console.WriteLine(response.Content);


            HtmlDocument _htmlDoc = new HtmlDocument();
            var responseString = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(responseString);//*[@id="0"]

            var posterImagess = _htmlDoc.DocumentNode.SelectNodes("//*[@class='u-standardTable']").ElementAt(1).ChildNodes.ElementAt(3);
            var posterImageaa = _htmlDoc.DocumentNode.SelectNodes("//*[@class='u-standardTable']").ElementAt(1).ChildNodes.
                ElementAt(3).ChildNodes.Where(e=> e.Name == "tr");

            foreach (var item in posterImageaa)
            {
                item.ChildNodes.Where(e => e.NodeType == HtmlNodeType.Element);
            }

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var collection = Regex.Matches(responseContent, @"(https?:\/\/.*\.(?:png|jpg))");
                var List = collection.Cast<Match>()
                .Select(m => m.Value)
                .ToList();

                return List;
            }
            return null;
        }

   
    }
}
