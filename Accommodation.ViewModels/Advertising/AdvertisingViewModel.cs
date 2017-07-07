using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;

namespace Accommodation.ViewModels
{
   

    public class AdvertisingViewModel : IBaseViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Link { get; set; }
        public bool ExternalLink { get; set; }
        public bool IsAccommodation { get; set; }

        public LocationViewModel Location { get; set; }
        
    }
}
