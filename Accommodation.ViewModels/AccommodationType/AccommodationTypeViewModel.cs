using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accommodation.Models.Enumerators;
using Accommodation.ViewModels.Location;

namespace Accommodation.ViewModels
{
   

    public class AccommodationTypeViewModel : IBaseViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
        
    }
    
}
