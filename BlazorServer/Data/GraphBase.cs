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
        public StockObject[] OMX = GetTradingHistory(TradingInstrument.OMXS30.ToString()).ToArray();
        public StockObject[] CertShort = GetTradingHistory(TradingInstrument.CertShort.ToString()).ToArray();
        public StockObject[] CertLong = GetTradingHistory(TradingInstrument.CertLong.ToString()).ToArray();
        public StockObject[] FutureLong = GetTradingHistory(TradingInstrument.FutureLong.ToString()).ToArray();
        public StockObject[] FutureShort = GetTradingHistory(TradingInstrument.FutureShort.ToString()).ToArray();
        public int difference;
        public string pickDay;
        public DateTime? value = DateTime.Now;
        public Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        public void Change(DateTime? value)
        {
            var selectedDate = DateTime.Parse(value.ToString()).ToShortDateString();
            OMX = GetTradingHistory(TradingInstrument.OMXS30.ToString()).ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            CertShort = GetTradingHistory(TradingInstrument.CertShort.ToString()).ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            CertLong = GetTradingHistory(TradingInstrument.CertLong.ToString()).ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            FutureLong = GetTradingHistory(TradingInstrument.FutureLong.ToString()).ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            FutureShort = GetTradingHistory(TradingInstrument.FutureShort.ToString()).ToArray().Where(e => e.FetchDate.ToShortDateString() == selectedDate).ToArray();
            StateHasChanged();
        }


        public static List<StockObject> GetTradingHistory(string ticker)
        {
            var tradingData = AvanzaRepo.GetData().Where(e => e.Name == ticker).ToList();
            return tradingData;
        }

        public List<string> GetTradingDates()
        {
            var tradingData = AvanzaRepo.GetTradingDays().Select(e => e.FetchDate.ToString()).ToList();

            return tradingData;

        }
    }






}
