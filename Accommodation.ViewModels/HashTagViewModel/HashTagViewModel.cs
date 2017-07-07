using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;

namespace Accommodation.ViewModels
{
   

    public class HashTagViewModel : IBaseViewModel
    {
        public int ID { get; set; }
        public string LocationTag { get; set; }
        public string Name { get; set; }
        public string HashTag { get; set; }
        public string Type { get; set; }

    }
}
