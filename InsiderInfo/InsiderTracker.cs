using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InsiderInfo
{
   public class InsiderTracker
    {
        readonly HttpClient client;
        public InsiderTracker(HttpClient client)
        {
            this.client = client;
        }
        public async Task<List<InsiderModel>> GetInsiderData(string ticker)
        {

            HtmlDocument _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml($"http://openinsider.com/screener?s={ticker}&o=&pl=&ph=&ll=&lh=&fd=1461&fdr=&td=0&tdr=&fdlyl=&fdlyh=&daysago=&xp=1&xs=1&vl=&vh=&ocl=&och=&sic1=-1&sicl=100&sich=9999&grp=0&nfl=&nfh=&nil=&nih=&nol=&noh=&v2l=&v2h=&oc2l=&oc2h=&sortcol=0&cnt=1000&page=1");

            var response = await client.GetAsync($"http://openinsider.com/screener?s={ticker}&o=&pl=&ph=&ll=&lh=&fd=1461&fdr=&td=0&tdr=&fdlyl=&fdlyh=&daysago=&xp=1&xs=1&vl=&vh=&ocl=&och=&sic1=-1&sicl=100&sich=9999&grp=0&nfl=&nfh=&nil=&nih=&nol=&noh=&v2l=&v2h=&oc2l=&oc2h=&sortcol=0&cnt=1000&page=1");
            var content = await response.Content.ReadAsStringAsync();
            _htmlDoc.LoadHtml(content);
            var data = _htmlDoc.DocumentNode.SelectNodes("//*[@id='tablewrapper']/table/tbody").FirstOrDefault().ChildNodes.Where(e=>e.NodeType != HtmlNodeType.Text);

            List<InsiderModel> insiderData = new List<InsiderModel>();

            foreach (var item in data)
            {
                var date = item.ChildNodes.ElementAt(2).InnerText;
                var type = item.ChildNodes.ElementAt(6).InnerText;
                var shares = item.ChildNodes.ElementAt(8).InnerText;
                string result = Regex.Replace(shares, @"[^\d]", "");
                var sharesx = Int32.Parse(result);
                insiderData.Add(new InsiderModel() { Date = date, Type = type, Shares = sharesx });
            }
     

            return insiderData;

            //*[@id="tablewrapper"]/table/tbody/tr[1]/td[8]   //*[@id="tablewrapper"]/table/tbody/tr[1]/td[9] //*[@id="tablewrapper"]/table/tbody/tr[2]/td[9]
        }
    }
}
