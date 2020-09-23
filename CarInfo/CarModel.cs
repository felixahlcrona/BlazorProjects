using System;
using System.Collections.Generic;
using System.Text;

namespace CarInfo
{
    public class CarModel
    {
        public string YearlyTax { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Traffic_status { get; set; }
        public string Is_leased { get; set; }
        public string StolenStatus { get; set; }
        public IEnumerable<OwnerModel> Owners { get; set; }
        public IEnumerable<Car> History { get; set; }

    }
    public partial class CarHistory
    {
        public List<Car> Items { get; set; }
    }

    public partial class Car
    {
        public string Date { get; set; }
        public string Content { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }

    }
}
