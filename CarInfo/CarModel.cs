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
        public string traffic_status { get; set; }
        public string is_leased { get; set; }
        public IEnumerable<OwnerModel> owners { get; set; }

    }
}
