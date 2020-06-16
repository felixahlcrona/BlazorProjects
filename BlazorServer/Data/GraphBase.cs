using AvanzaScraper;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorServer.Pages
{
    public class GraphBase : ComponentBase
    {
        public StockObject[] OMX = GetTradingHistory("OMXS30").ToArray();
        public StockObject[] CertShort = GetTradingHistory("CertShort").ToArray();
        public StockObject[] CertLong = GetTradingHistory("CertLong").ToArray();
        public StockObject[] FutureLong = GetTradingHistory("FutureLong").ToArray();
        public StockObject[] FutureShort = GetTradingHistory("FutureShort").ToArray();

        public int difference;
        public string pickDay;

        public class DataItem
        {
            public DateTime Date { get; set; }
            public double Revenue { get; set; }
        }


        public DateTime? value = DateTime.Now;

        public Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        public void Change(DateTime? value)
        {
            var selectedDate = DateTime.Parse(value.ToString()).ToShortDateString();
            //var x = value.ToString();
            //var z = DateTime.Parse(x);
            //var b = z.ToShortDateString();
            OMX = GetTradingHistory("OMXS30").ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            CertShort = GetTradingHistory("CertShort").ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            CertLong = GetTradingHistory("CertLong").ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            FutureLong = GetTradingHistory("FutureLong").ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            FutureShort = GetTradingHistory("FutureShort").ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            StateHasChanged();
        }

        public void GetDAY()
        {
            int V2 = CertShort.Select(e => e.Price).ElementAt(CertShort.Count() - 2);
            int V1 = CertShort.Select(e => e.Price).LastOrDefault();

            difference = ((V2 - V1) / Math.Abs(V1)) * 100;
        }






        public static List<StockObject> GetTradingHistory(string ticker)
        {
            var tradingData = Repository.GetData().Where(e => e.Name == ticker).ToList();
            return tradingData;
        }

        public List<string> GetTradingDates()
        {
            var tradingData = Repository.GetTradingDays().Select(e => e.FetchDate.ToString()).ToList();

            return tradingData;

        }
    }






}
