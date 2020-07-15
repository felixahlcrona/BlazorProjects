using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            //hzp07s

            HtmlDocument _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml($"https://biluppgifter.se/fordon/{regNmr}#history-anchor-v2");

            var response = await client.GetAsync($"https://biluppgifter.se/fordon/{regNmr}#history-anchor-v2");
            var content = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(content);

            var previousOwners = _htmlDoc.DocumentNode.SelectNodes("//*[@class='history enlarge']").ElementAt(1).ChildNodes.Where(e => e.InnerText.Contains("Ägarbyte"));

            var carModel = ExtractPersonalInfo(previousOwners);
            carModel.YearlyTax = ExtractYearlyTax(content);
            carModel.Make = ExtractCarMakeAndModel(_htmlDoc.DocumentNode.SelectNodes("//title").FirstOrDefault().InnerText);
            carModel.Model = ExtractCarModel(content);
            carModel.is_leased = ExtractCarLease(content);
            carModel.traffic_status = ExtractCarTrafficStatus(content);
            return carModel;
        }
        private CarModel ExtractPersonalInfo(IEnumerable<HtmlNode> previousOwners)
        {
            CarModel car = new CarModel();
            List<OwnerModel> li = new List<OwnerModel>();
            foreach (var owner in previousOwners)
            {
                var dateOvertaken = owner.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1).InnerText;
                var location = owner.ChildNodes.ElementAt(3).ChildNodes.ElementAt(6).InnerText;
                var person = ExtractOwner(owner.ChildNodes.ElementAt(3).ChildNodes.ElementAt(7).InnerHtml);
                var ownerTraceLink = ExtractOwnerTraceLink(owner?.ChildNodes?.ElementAt(3)?.ChildNodes?.ElementAt(7).InnerHtml);
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
                if (node.ElementAt(3) == "foretag")
                {
                    owner = query.Split(new char[] { '/', ' ' },
                                                StringSplitOptions.RemoveEmptyEntries).ElementAt(4);
                    owner = CleanString(owner);
                }
                else
                {
                    owner = query.Split(new char[] { '/', ' ' },
                                                StringSplitOptions.RemoveEmptyEntries).ElementAt(5);
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
