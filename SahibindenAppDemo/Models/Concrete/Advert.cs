using System;
using System.Collections.Generic;
using System.Text;

namespace SahibindenAppDemo.Models.Concrete
{
    public class Advert
    {
        public int AdvertID { get; set; }

        public string AdvertName { get; set; }
        public string AdvertURL { get; set; }

        public string AdvertPrice { get; set; }
        public string AdvertDetails { get; set; }

        public List<string> AdvertPropertyName { get; set; }
        public List<string> AdvertPropertyDescription { get; set; }



    }
}
