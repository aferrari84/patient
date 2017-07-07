using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.ViewModels.Location
{
    public class LocationViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Hashtag { get; set; }
        public StateViewModel State { get; set; }
        public string ImageURL { get; set; }
    }
}
