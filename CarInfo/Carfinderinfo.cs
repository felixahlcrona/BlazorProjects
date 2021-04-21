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
            var cookie = response.Headers.Where(e => e.Key.Contains("Set-Cookie")).FirstOrDefault().Value.Where(e => e.Contains("biluppgifter_session")).FirstOrDefault();

 
            var internalUserId = ExtractInternalUserId(content);
            var history = await ExtractCarHistory(internalUserId, csrfToken, cookie);
            var carModel = ExtractPersonalInfo(history);
            carModel.CoverPhotoUrl = await GetCoverPhoto(regNmr);
            carModel.YearlyTax = ExtractYearlyTaxes(_htmlDoc);
            carModel.StolenStatus = ExtractStolenStatus(_htmlDoc);
            carModel.Make = ExtractCarMakeAndModel(_htmlDoc.DocumentNode.SelectNodes("//title").FirstOrDefault().InnerText);
            carModel.Model = ExtractCarModel(content);


            carModel.Is_leased = ExtractMoreInfo(_htmlDoc, "data-leasing");
            carModel.Traffic_status = ExtractMoreInfo(_htmlDoc, "data-traffic-status");

            
            return carModel;
        }




        public async Task<string> GetCoverPhoto(string regNmr)
        {
            try
            {

                var client = new RestClient($"https://www.car.info/sv-se/search/super/{regNmr}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("authority", "www.car.info");
                request.AddHeader("cache-control", "max-age=0");
                request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"88\", \"Google Chrome\";v=\"88\", \";Not\\A\"Brand\";v=\"99\"");
                request.AddHeader("sec-ch-ua-mobile", "?1");
                request.AddHeader("upgrade-insecure-requests", "1");
                client.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.182 Mobile Safari/537.36";
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "none");
                request.AddHeader("sec-fetch-mode", "navigate");
                request.AddHeader("sec-fetch-user", "?1");
                request.AddHeader("sec-fetch-dest", "document");
                request.AddHeader("accept-language", "sv-SE,sv;q=0.9,en-US;q=0.8,en;q=0.7");
                request.AddHeader("cookie", "__cfduid=d1d549f201ba026bf2c86c26655087bc11613464166; XSRF-TOKEN=eyJpdiI6IjgwVVU0YWhIZlJyUkVGT29mU28vL3c9PSIsInZhbHVlIjoibjBNTi9qam5xR091cG5CSzRhdGkrb213aDVmMStRSjZnVTJWMUZ5Sk5BRkdoMVFaTXVOS20rYUcrMWJvTWFrV0FIU0NkR3NKLzRVUHRpb1RiVnpwcWVRTFE4eFluYWUrOTlEN2M0TUZ0TFBaUSt1SmtmSUIrdU10S3lyOUpvWU8iLCJtYWMiOiJlMzMyYzE5MWM0NGY1YWM3NDBjMTMwZTEzZWYxNmRhMDdlODYxMDA1M2U2ZGI5NTIzZjIwODYxYzllYmViZDhkIn0%3D; laravel_session=eyJpdiI6IkhSUFAzbUFucnU2Rkw1aGpmUVBpNHc9PSIsInZhbHVlIjoiKzB0VjY2Qm0rNS9TUk0zRzV3RXJSNUI0K2txQStXNituWm5iejZEeXkydG1XMFVHTEVNaE1qT3ZhZXI5ZlJQZTFhRjR3YkNoUW1aZW95ZElFUHQzcXd5UEhEdGNkUHJCSkdpVml1MHZmRGtLa3ExUXJPOUNQazg5dW40OHlGazAiLCJtYWMiOiJhMWQ2MmMwYWEyODg0ZjRlOWU2NDdhZTlhY2JiYTQ5OTY5NTU5NjFkOTU4Mzg3N2IxOGViYmZkYTQyODNmMGRhIn0%3D; XSRF-TOKEN=eyJpdiI6InNWVHVENnZpaVRwdEFTKzhyRkM0T0E9PSIsInZhbHVlIjoiRlphQzhiVlZ2M2FOcUE0ZVUwM3VpT3hRVnUwMlF2RjBKVmQ0Y1pWTEZUYnFyZlcxN0NMaVJSbHVnb1pDQ0NuWWUxcnVZMDFmbUdkYWsvZVR3a3dVdElyRW4yOHVpY2F3cWl1NTdjeHQ2SHJpUUFISk9ocW5EOCtxNWFGZFA2ZHMiLCJtYWMiOiJlYTVhOGFjZTRlOTZlYWJmYzI3OWVhYjM4Y2ExNjBlN2VjYTQzMzg1YjAzZTAwMjZkZWFlNTQ3N2Q0MmU2ODMwIn0%3D; laravel_session=eyJpdiI6Imtpc0hvZWxMeWlRZElOZnMxQlBtVFE9PSIsInZhbHVlIjoiTXVrMTJJNWliWUxGTERJWDRlMHR2bVVuTnpSUlgzMHp2VzcrZTJXcjhUeG9ud1RrUDNnWjc1Vk5ONTllMkN2K1pLZXFMRFU5bHZmWG9FZkc3b2lhTG5UQXpRUmNKR2dnY1d1TTgvYm51QjlDOUI3VnpuWDBISVZhdjNXczBtd1ciLCJtYWMiOiJiMTdhN2UwZWRiZTE1MGJhN2YzZmU2NjcyM2FjZjQ3MzMxODZmODBlYzAyNDI4NmU3YjNiMTZkMTYxYTY0NDdkIn0%3D");

                IRestResponse response = client.Execute(request);
                var res = JsonConvert.DeserializeObject<CarModel>(response.Content);
                var coverPhoto = res.hits.idents.FirstOrDefault().cover_photo_image_id;
                var coverPhotoUrl = $"https://s.car.info/image_files/360/{coverPhoto}.jpg";
                return coverPhotoUrl;

            }
            catch (Exception e)
            {
                return null;
            }

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
            car.Owners = li;
            return car;
        }


        private static string ExtractYearlyTaxes(HtmlDocument _htmlDoc)
        {
            try
            {
                var res = _htmlDoc.DocumentNode.SelectNodes("//*[@id='box-data']/div/div/ul/li[22]/span[2]").FirstOrDefault().InnerText;
                if(res.Contains("SEK"))
                {
                    return res;
                }
                return "Okänd skatt";
            }
            catch
            {
                return "Okänd skatt";
            }
        }
        private static string ExtractStolenStatus(HtmlDocument _htmlDoc)
        {
            try
            {
                return _htmlDoc.DocumentNode.SelectNodes("//*[@id='box-data']/div/div/ul/li[6]/span[2]").FirstOrDefault().InnerText;
            }
            catch
            {
                return "Okänt status";
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
                return "Okänt";
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
   

        private static string ExtractMoreInfo(HtmlDocument _htmlDoc, string param)
        {
            try
            {
                return _htmlDoc.DocumentNode.SelectNodes($"//*[@id='{param}']").FirstOrDefault().InnerText;
            }
            catch
            {
                return "-";
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
