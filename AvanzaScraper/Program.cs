using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvanzaScraper
{
    public class Program
    {
        // https://www.cnbc.com/rss-feeds/ perfekt RSS feed för sentiment analysis.
        //https://www.avanza.se/borshandlade-produkter/certifikat-torg/lista.html?name=&assetRootCategoryIds=CERTIFICATE_ASSET%7C25&assetSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26&directions=SHORT&page=1&sortField=TOTAL_VALUE_TRADED&sortOrder=DESCENDING
        // Kolla certifkat också!
        // https://www.avanza.se/borshandlade-produkter/certifikat-torg/lista.html?name=&assetRootCategoryIds=CERTIFICATE_ASSET%7C25&assetSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26&directions=LONG&page=1&sortField=TOTAL_VALUE_TRADED&sortOrder=DESCENDING
        private static readonly HttpClient client = new HttpClient();
        private static HtmlDocument _htmlDoc = new HtmlDocument();
        private static HtmlDocument _htmlDocument = new HtmlDocument();
        private static List<int> _totalTraders = new List<int>();
        private static List<string> _tradingLinks = new List<string>();
        private static string OMX30doc = @"C:\Users\felix\Desktop\OMX30.txt";
        private static string DAXdoc = @"C:\Users\felix\Desktop\DAX.txt";
        private static string Spyindex = @"C:\Users\felix\Desktop\Spyindex.txt";
        private static DateTime startDate = new DateTime();
        //private static string BullIndex = @"C:\Users\felix\source\repos\EasySubFinder\EasySubFinder\TradingData\Bullindex.txt";
        //private static string BearIndex = @"C:\Users\felix\source\repos\EasySubFinder\EasySubFinder\TradingData\Bearindex.txt";
        //private static string OMXindex = @"C:\Users\felix\source\repos\EasySubFinder\EasySubFinder\TradingData\OMXindex.txt";
        public static async Task Main(string[] args)
        {



            //await GetIndexData("https://finance.yahoo.com/quote/%5EGSPC?p=^GSPC", Spyindex);


            //await GetFuturesTraders("MiniFuture", DAXdoc, "18893", "SHORT");
            //await GetFuturesTraders("MiniFuture", DAXdoc, "18893", "LONG");
            //await GetFuturesTraders("MiniFuture", OMX30doc, "9270", "SHORT");
            //await GetFuturesTraders("MiniFuture", OMX30doc, "9270", "LONG");


            await CrawlForStocks();


        }


        public static async Task CrawlForStocks()
        {

            startDate = DateTime.Now.AddHours(2);
            //await GetIndexDataLagging("https://www.avanza.se/index/om-indexet.html/19002/omx-stockholm-30", "OMXindex");
            await GetIndexData("https://www.cnbc.com/quotes/?symbol=.OMXS30", "OMXindex");
            Console.WriteLine("index done");
            await GetFuturesTraders("Certificate", "CertLong", "9270", "LONG");
            await GetFuturesTraders("Certificate", "CertShort", "9270", "SHORT");

            // funkar men kanske inte releavnta.
           // await GetFuturesTraders("MiniFuture", "FutureLong", "9270", "LONG");
            //wait GetFuturesTraders("MiniFuture", "FutureShort", "9270", "SHORT");
            Environment.Exit(0);
        }
        // https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=SPY&apikey=FNZ4OYF59H63DYGJ 
        // Kolla SPY priser vid inläsning.
        private static async Task GetFuturesTraders(string stockType, string stockName, string underlyingInstrument, string stockDirection)
        {
            

                for (int i = 0; i < 12;)
                {
                    i++;
                    ScrapeAllTradingLinks(stockType, stockDirection, underlyingInstrument, i).GetAwaiter().GetResult();

                }


                if (stockType == "MiniFuture")
                {
                    await GetCurrentActiveTraders(stockName, stockDirection);
                }
                if (stockType == "Certificate")
                {
                    await GetCurrentActiveTradersCert(stockName, stockDirection);
                }


        }

        static async Task<int> ScrapeAllTradingLinks(string stockType, string stockDirection, string underlyingInstrument, int sitePage)
        {
            var tradingObject = "";
            if (stockType == "MiniFuture")
            {
                tradingObject = $"https://www.avanza.se/borshandlade-produkter/warranter-torg/lista.html?name=&underlyingInstruments={underlyingInstrument}&directions={stockDirection}&page={sitePage}&sortField=TOTAL_VALUE_TRADED&sortOrder=DESCENDING";
            }
            if (stockType == "Certificate")
            {
                // Alla aktieindex 
                // https://www.avanza.se/borshandlade-produkter/certifikat-torg/lista.html?name=&assetRootCategoryIds=CERTIFICATE_ASSET%7C25&assetSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26&directions={stockDirection}&page={sitePage}&sortField=TOTAL_VALUE_TRADED&sortOrder=DESCENDING
                // Bara OMX
                // https://www.avanza.se/borshandlade-produkter/certifikat-torg/lista.html?name=&assetRootCategoryIds=CERTIFICATE_ASSET%7C25&assetSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26&assetSubSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26%7C961&directions={stockDirection}&page={sitePage}&sortField=TOTAL_VALUE_TRADED&sortOrder=DESCENDING
                tradingObject = $"https://www.avanza.se/borshandlade-produkter/certifikat-torg/lista.html?name=&assetRootCategoryIds=CERTIFICATE_ASSET%7C25&assetSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26&assetSubSubCategoryIds=CERTIFICATE_ASSET%7C25%7C26%7C961&directions={stockDirection}&page={sitePage}&sortField=TOTAL_VALUE_TRADED&sortOrder=DESCENDING";
            }

            var res = await client.GetAsync(tradingObject);
            _htmlDocument.LoadHtml(await res.Content.ReadAsStringAsync());

            // 30 tradinglinks per page
            for (int i = 0; i < 30;)
            {
                try
                {
                    i++;
                    var tradingLink = _htmlDocument.DocumentNode
                        .SelectNodes($"//*[@id='contentTable']/tbody/tr[{i}]/td[2]/span/a")
                        .FirstOrDefault().Attributes.FirstOrDefault().Value;

                    _tradingLinks.Add("https://www.avanza.se" + tradingLink);
                }
                catch(Exception e)
                {

                }
             
            }



            return 0;
        }









        static async Task<int> GetCurrentActiveTraders(string stockName, string stockDirection)
        {
            List<int> totalTraders = new List<int>();
            foreach (var item in _tradingLinks)
            {
                var request = await client.GetAsync(item);
                var res = await request.Content.ReadAsStringAsync();
                _htmlDoc.LoadHtml(res);

                var elementType = _htmlDoc.DocumentNode
                    .SelectNodes($"//*[@class='secondaryInfo border XSText noTopBorder highlightOnHover']")
                    .FirstOrDefault().ChildNodes.Count();

                var tradingCount = "";


                // Different layouts for some futures.
                if (elementType == 41)
                {
                    tradingCount = _htmlDoc.DocumentNode
                        .SelectNodes($"//*[@class='secondaryInfo border XSText noTopBorder highlightOnHover']")
                        .FirstOrDefault().ChildNodes.ElementAt(31).InnerText;
                }
                else
                {
                    tradingCount = _htmlDoc.DocumentNode
                        .SelectNodes($"//*[@class='secondaryInfo border XSText noTopBorder highlightOnHover']")
                        .FirstOrDefault().ChildNodes.ElementAt(35).InnerText;
                }

                totalTraders.Add(Int32.Parse(tradingCount));

            }
            AvanzaRepo.InsertStockData(new StockObject() { Name = stockName, Type = stockDirection, Price = totalTraders.Sum(), FetchDate = startDate });


            _tradingLinks.Clear();
            return 0;
        }



        static async Task<int> GetCurrentActiveTradersCert(string stockName, string stockDirection)
        {
           

                List<int> totalTraders = new List<int>();
                foreach (var item in _tradingLinks)
                {
                    var request = await client.GetAsync(item);
                    var res = await request.Content.ReadAsStringAsync();
                    _htmlDoc.LoadHtml(res);

                    var elementType = _htmlDoc.DocumentNode
                        .SelectNodes($"//*[@class='border fLeft XSText rightAlignText highlightOnHover thickBorderBottom']")
                        .FirstOrDefault().ChildNodes.ElementAt(59).InnerText;



                    totalTraders.Add(Int32.Parse(elementType));


                }
                AvanzaRepo.InsertStockData(new StockObject() { Name = stockName, Type = stockDirection, Price = totalTraders.Sum(), FetchDate = startDate });

                _tradingLinks.Clear();
                return 0;
          
        }


        //static async Task<int> GetIndexDataLagging(string indexObject, string docPath)
        //{
        //    HtmlDocument _htmlDocx = new HtmlDocument();
        //    var request = await client.GetAsync(indexObject);
        //    var res = await request.Content.ReadAsStringAsync();
        //    _htmlDocx.LoadHtml(res);

        //    var node = _htmlDocx.DocumentNode
        //            .SelectNodes($"//*[@id='surface']/div[2]/div/div/div/ul/li[3]/span[2]/span").FirstOrDefault().InnerText;




        //    var indexPrice = node.Replace(".", ",");
        //    indexPrice = indexPrice.Remove(5);
        //    indexPrice = indexPrice.Replace(",", "");
        //    indexPrice = Regex.Replace(indexPrice, @"\s+", "");

        //    Repository.inserttest(new StockObject() {Name = "OMXS30Lagging" ,Type="OMXS30Lagging", Price = Convert.ToInt32(indexPrice) });

        //    return 0;
        //}


        static async Task<int> GetIndexData(string indexObject, string docPath)
        {
            HtmlDocument _htmlDocx = new HtmlDocument();
            var request = await client.GetAsync(indexObject);
            var res = await request.Content.ReadAsStringAsync();
            _htmlDocx.LoadHtml(res);

            var node = _htmlDocx.DocumentNode
                    .SelectNodes($"//*[@id='cnbc-contents']/div/div[3]/div[3]/div[1]/div[2]/div/div[1]/div/table/tbody/tr[5]/td[1]/span[1]").FirstOrDefault().InnerText;



            //var indexPrice = node.Replace(".", ",");

            //indexPrice = indexPrice.Remove(5);
            //indexPrice = indexPrice.Replace(",", "");
            //indexPrice = Regex.Replace(indexPrice, @"\s+", "");

            var indexPrice = node.Replace(".", ",");
            indexPrice = indexPrice.Remove(5);
            indexPrice = indexPrice.Replace(",", "");
            indexPrice = Regex.Replace(indexPrice, @"\s+", "");
            AvanzaRepo.InsertStockData(new StockObject() { Name = "OMXS30", Type = "OMXS30", Price = Convert.ToInt32(indexPrice), FetchDate = startDate });

            return 0;
        }
    }

}