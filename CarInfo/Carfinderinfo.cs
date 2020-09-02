using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarInfo
{
    public class Carfinderinfo
    {
        readonly HttpClient client;
        public Carfinderinfo(HttpClient client)
        {
            this.client = client;
        }





        public async Task<CarModel> RegLookUp(string regNmr)
        {

            HtmlDocument _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml($"https://biluppgifter.se/fordon/{regNmr}#history-anchor-v2");

            var response = await client.GetAsync($"https://biluppgifter.se/fordon/{regNmr}#history-anchor-v2");
            var content = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(content);
            var previousOwners = _htmlDoc.DocumentNode.SelectNodes("//*[@class='history enlarge']").ElementAt(1).ChildNodes.Where(e => e.InnerText.Contains("Ägarbyte"));


            var csrfToken = _htmlDoc.DocumentNode.SelectNodes("/html/head/meta[4]").FirstOrDefault().Attributes.ElementAt(1).Value;
            var cookie = response.Headers.Where(e => e.Key.Contains("Set-Cookie")).FirstOrDefault().Value.Where(e=>e.Contains("biluppgifter_session")).FirstOrDefault();


            var internalUserId = ExtractInternalUserId(content);
            var history = await ExtractCarHistory(internalUserId, csrfToken, cookie);
            var carModel = ExtractPersonalInfo(history);
            carModel.YearlyTax = ExtractYearlyTax(content);
            carModel.Make = ExtractCarMakeAndModel(_htmlDoc.DocumentNode.SelectNodes("//title").FirstOrDefault().InnerText);
            carModel.Model = ExtractCarModel(content);
            carModel.is_leased = ExtractCarLease(content);
            carModel.traffic_status = ExtractCarTrafficStatus(content);
            return carModel;
        }





   
        public async Task<IEnumerable<Car>> ExtractCarHistory(string internalUserId, string csrfToken,string cookie)
        {
            try
            {

                var client = new RestClient("https://biluppgifter.se/api/v1/vehicle/history");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("authority", "biluppgifter.se");
                request.AddHeader("accept", "application/json, text/plain, */*");
                request.AddHeader("x-csrf-token", csrfToken);
                request.AddHeader("cookie", cookie);
                request.AddHeader("x-requested-with", "XMLHttpRequest");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36";
                request.AddHeader("content-type", "application/json;charset=UTF-8");
                request.AddHeader("origin", "https://biluppgifter.se");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "sv-SE,sv;q=0.9,en-US;q=0.8,en;q=0.7");
                request.AddParameter("application/json;charset=UTF-8", $"{{ \"uuid\": \"{internalUserId}\",\"profile\":\"default\"}}", ParameterType.RequestBody);
              
                IRestResponse response = client.Execute(request);
                var res = JsonConvert.DeserializeObject<CarHistory>(response.Content);
                var history = res.Items.Where(e => e.Title.Contains("Ägarbyte"));
                return history;

            }
            catch (Exception e)
            {
                return null;
            }

        }

        private CarModel ExtractPersonalInfo(IEnumerable<Car> previousOwners)
        {

            CarModel car = new CarModel();
            List<OwnerModel> li = new List<OwnerModel>();
            foreach (var owner in previousOwners)
            {
                var dateOvertaken = owner.Date;
                var location = "";
                var person = ExtractOwner(owner.Content);
                var ownerTraceLink = ExtractOwnerTraceLink(owner.Content);
                li.Add(new OwnerModel() { FullName = person, DateOvertaken = dateOvertaken, Location = location, OwnerTraceLink = ownerTraceLink });
            }
            car.owners = li;
            return car;
        }


        private static string ExtractYearlyTax(string query)
        {
            try
            {
                return Regex.Match(query, @"(?:""yearly_tax"":)(.*?)(?:,)").Groups.Values.ElementAt(1).Value;
            }
            catch
            {
                return "Okänd skatt";
            }
        }

        private static string ExtractOwnerTraceLink(string query)
        {
            try
            {
                return Regex.Match(query, @"href=['""]([^'""]+?)['""]").Groups.Values.ElementAt(1).Value;
            }
            catch
            {
                return "";
            }

        }


        private static string ExtractInternalUserId(string query)
        {
            try
            {
                var x = Regex.Match(query, @"[\n\r].*uuid:\s* ([^\n\r]*)").Value;
                var b = Regex.Match(x, @"""(.*?)""").Groups.Values.ElementAt(1).Value;
                return b;
            }
            catch
            {
                return "Okänt märke";
            }
        }

        private static string ExtractCarMakeAndModel(string query)
        {
            try
            {
                return Regex.Match(query, @".+?(?=\/)").Value;
            }
            catch
            {
                return "Okänt märke";
            }
        }



        private static string ExtractCarTrafficStatus(string query)
        {
            try
            {
                return Regex.Match(query, @"(?:""traffic_status"":)(.*?)(?:,)").Groups.Values.ElementAt(1).Value;
            }
            catch
            {
                return "Okänt trafikstatus";
            }

        }
        private static string ExtractCarModel(string query)
        {
            try
            {
                return Regex.Match(query, @"(?:""model"":)(.*?)(?:,)").Groups.Values.ElementAt(1).Value;
            }
            catch
            {
                return "Okänd modell";
            }


        }
        private static string ExtractCarLease(string query)
        {
            try
            {
                return Regex.Match(query, @"(?:""is_leased"":)(.*?)(?:,)").Groups.Values.ElementAt(1).Value;
            }
            catch
            {
                return "Okänd ägandestatus";
            }

        }
        private static string CleanString(string query)
        {
            var q = query.Replace("-", " ");
            var output = Regex.Replace(q, @"[\d-]", string.Empty);
            return output;
        }



        private static string ExtractOwner(string query)
        {
            try
            {
                var owner = "";

                var node = query.Split(new char[] { '/', ' ' },
                               StringSplitOptions.RemoveEmptyEntries);
                if (node.FirstOrDefault() == "Företag")
                {
                    owner = query.Split(new char[] { '/', ' ' },
                                                StringSplitOptions.RemoveEmptyEntries).ElementAt(7);
                    owner = CleanString(owner);
                }
                else
                {
                    owner = query.Split(new char[] { '/', ' ' },
                                                StringSplitOptions.RemoveEmptyEntries).ElementAt(8);
                    owner = CleanString(owner);
                }

                return owner;
            }

            catch
            {
                return query = "Okänd ägare";
            }
        }



    }
}
