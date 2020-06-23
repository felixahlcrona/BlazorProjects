using System;

namespace AvanzaScraper
{
    public class StockObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public DateTime FetchDate { get; set; }
    }

    public enum TradingInstrument
    {
        OMXS30,
        CertShort,
        CertLong,
        FutureLong,
        FutureShort
    }

}

