using System;

namespace Api
{

    //public class DrinksModel
    //{
    //    public Class1[] Property1 { get; set; }
    //}

    public class DrinksModel
    {
        public string ProductId { get; set; }
        public string ProductNumber { get; set; }
        public string ProductNameBold { get; set; }
        public string ProductNameThin { get; set; }
        public string Category { get; set; }
        public string ProductNumberShort { get; set; }
        public string ProducerName { get; set; }
        public string SupplierName { get; set; }
        public bool IsKosher { get; set; }
        public string BottleTextShort { get; set; }
        public object Seal { get; set; }
        public int RestrictedParcelQuantity { get; set; }
        public bool IsOrganic { get; set; }
        public bool IsEthical { get; set; }
        public object EthicalLabel { get; set; }
        public bool IsWebLaunch { get; set; }
        public DateTime SellStartDate { get; set; }
        public bool IsCompletelyOutOfStock { get; set; }
        public bool IsTemporaryOutOfStock { get; set; }
        public float AlcoholPercentage { get; set; }
        public float Volume { get; set; }
        public float Price { get; set; }
        public string Country { get; set; }
        public string OriginLevel1 { get; set; }
        public string OriginLevel2 { get; set; }
        public int Vintage { get; set; }
        public string SubCategory { get; set; }
        public string Type { get; set; }
        public object Style { get; set; }
        public string AssortmentText { get; set; }
        public string BeverageDescriptionShort { get; set; }
        public string Usage { get; set; }
        public string Taste { get; set; }
        public string Assortment { get; set; }
        public float RecycleFee { get; set; }
        public bool IsManufacturingCountry { get; set; }
        public bool IsRegionalRestricted { get; set; }
        public object IsInStoreSearchAssortment { get; set; }
        public bool IsNews { get; set; }
    }

}
