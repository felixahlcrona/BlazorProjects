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
        public string CoverPhotoUrl { get; set; }
      
        public IEnumerable<OwnerModel> Owners { get; set; }
        public IEnumerable<Car> History { get; set; }

        //Carinfo API
        public Hits hits { get; set; }

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

    public class Hits
    {
        public List<Ident> idents { get; set; }
    
    }
    public class Ident
    {
        public int ident_id { get; set; }
        public string licence_plate { get; set; }
        public int gid { get; set; }
        public string search_level { get; set; }
        public int cover_photo_image_id { get; set; }
        public string license_code { get; set; }
        public string country { get; set; }
        public string full_name { get; set; }
        public string slug { get; set; }
        public int core_vehicle { get; set; }
        public string top_color_image_slug { get; set; }
        public string inspection_before { get; set; }
        public string id { get; set; }
    }
}
